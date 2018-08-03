using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace PollingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServicePolling" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServicePolling.svc or ServicePolling.svc.cs at the Solution Explorer and start debugging.
    public class ServicePolling : IServicePolling
    {

        enum Actions
        {
            [EnumMember(Value ="None")]
            None,
            [EnumMember(Value ="Notification")]
            Notification,
            [EnumMember(Value = "Emergency notification")]
            Emergency
        }

        string connString = ConfigurationManager.ConnectionStrings["MLODYConnectionString"].ConnectionString.ToString();
        public List<Rules> GetAllRules()
        {
            List<Rules> rules = new List<PollingService.Rules>();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connString))
            {          
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM HealthRules",conn);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                    rules = (from DataRow dr in dt.Rows
                             select new Rules()
                             {
                                 Parameter = dr["Parameter"].ToString(),
                                 AgeLower = Convert.ToInt16(dr["Age_Lower"]),
                                 AgeUpper = Convert.ToInt16(dr["Age_Upper"]),
                                 RangeLower = Convert.ToInt16(dr["Range_Lower"]),
                                 RangeUpper = dr["Range_Upper"] == DBNull.Value ? (Int16?)null : Convert.ToInt16(dr["Range_Upper"]),
                                 Occurence = dr["Occurence"] == DBNull.Value ? (Int16?)null : Convert.ToInt16(dr["Occurence"]),
                                 TimeWindow = dr["Time_Window"] == DBNull.Value ? (Int16?)null : Convert.ToInt16(dr["Time_Window"]),
                                 Action = dr["Action"].ToString()
                                   }).ToList();
                }
            }

            return rules;
        }

        public int InsertRules(Rules rulesValue)
        {
            string message = string.Empty;
            int result;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO HealthRules(Parameter,Age_Lower,Age_Upper,Range_Lower,Range_Upper,Occurence,Time_Window,Action) VALUES (@Parameter, @Age_Lower, @Age_Upper, @Range_Lower, @Range_Upper, @Occurence, @Time_Window, @Action)"
                , conn);
                cmd.Parameters.Add("@Parameter", System.Data.SqlDbType.VarChar).Value = rulesValue.Parameter;
                cmd.Parameters.Add("@Age_Lower", System.Data.SqlDbType.TinyInt).Value = rulesValue.AgeLower;
                cmd.Parameters.Add("@Age_Upper", System.Data.SqlDbType.TinyInt).Value = rulesValue.AgeUpper;
                cmd.Parameters.Add("@Range_Lower", System.Data.SqlDbType.TinyInt).Value = rulesValue.RangeLower;
                cmd.Parameters.Add("@Range_Upper", System.Data.SqlDbType.TinyInt).Value = rulesValue.RangeUpper.HasValue ? rulesValue.RangeUpper : (object)DBNull.Value;
                cmd.Parameters.Add("@Occurence", System.Data.SqlDbType.TinyInt).Value = rulesValue.Occurence.HasValue ? rulesValue.Occurence : (object)DBNull.Value;
                cmd.Parameters.Add("@Time_Window", System.Data.SqlDbType.TinyInt).Value = rulesValue.TimeWindow.HasValue ? rulesValue.TimeWindow : (object)DBNull.Value;
                cmd.Parameters.Add("@Action", System.Data.SqlDbType.VarChar).Value = rulesValue.Action;
                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        public PersonDetails GetUser(int userId)
        {
            PersonDetails user = new PersonDetails();
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                // get the record from healthcollected
                SqlCommand cmd = new SqlCommand("SELECT hc.*,u.Age FROM [HealthDataCollected]  hc join [User] u on hc.UserId = u.Id where hc.Id = " + userId, conn);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                    user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    user.Age = Convert.ToInt16(dt.Rows[0]["Age"]);
                    user.Parameter = dt.Rows[0]["Parameter"].ToString();
                    user.Reading = Convert.ToInt16(dt.Rows[0]["Reading"]);
                    user.RecordTime = Convert.ToDateTime(dt.Rows[0]["ReadingTime"]);
                    user.PersonId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                }
            }
            return user;
        }

        public string ApplyRule(PersonDetails user)
        {
            string actionToBeTaken = string.Empty;
            bool isActionUpdated = false;
            List <Rules> parameterisedRules = this.GetAllRules().Where(r => r.Parameter.ToLower() == user.Parameter.ToLower() 
                                                                        && user.Age >= r.AgeLower && user.Age <= r.AgeUpper
                                                                        && user.Reading >= r.RangeLower 
                                                                        ).ToList();




            if(parameterisedRules.Any())
            {
                actionToBeTaken = parameterisedRules.LastOrDefault().Action;
            }

            if (actionToBeTaken.ToLower().Equals("emergency notification"))//Actions.Emergency.ToString().ToLower()
            {
                isActionUpdated = this.ApplyAdvancedRule(user,true);
            }
            else
            {
                isActionUpdated = this.ApplyAdvancedRule(user);
                actionToBeTaken = isActionUpdated ? Actions.None.ToString() : Actions.Notification.ToString();
            }
            //if(!isActionUpdated)
            {
                this.UpdateAction(user.Id, actionToBeTaken, isActionUpdated);
            }

            return actionToBeTaken;
        }

        private List<PersonDetails> GetAllUserDetails(PersonDetails user)
        {
            List<PersonDetails> userDetails = new List<PersonDetails>();

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM HealthDataCollected where UserId = " + user.PersonId +
                                                " and ReadingTime between '" + user.RecordTime.AddHours(-1) + "' and '" + user.RecordTime + "'", conn);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                    userDetails = (from DataRow dr in dt.Rows
                                   select new PersonDetails()
                                   {
                                       Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                                       Parameter = dt.Rows[0]["Parameter"].ToString(),
                                       Reading = Convert.ToInt16(dt.Rows[0]["Reading"]),
                                       RecordTime = Convert.ToDateTime(dt.Rows[0]["ReadingTime"]),
                                       ActionTaken = dt.Rows[0]["ActionToBeTaken"].ToString()
                                   }).ToList();
                }
            }

            return userDetails;
        }


        public bool ApplyAdvancedRule(PersonDetails user,bool isSevere = false)
        {
            List<PersonDetails> userDetails = GetAllUserDetails(user);
            if (isSevere)
            {
                if (userDetails.Any(a => a.ActionTaken.ToLower().Equals("emergency notification")))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if(
                    (
                    !userDetails.Any(a => a.ActionTaken.ToLower().Equals("emergency notification"))
                    && !userDetails.Any(a => a.ActionTaken.ToLower().Equals("notification"))
                    && userDetails.Count() == 5)
                    ||
                    (
                     !userDetails.Any(a => a.ActionTaken.ToLower().Equals("emergency notification"))
                    && !userDetails.Any(a => a.ActionTaken.ToLower().Equals("notification"))
                    && userDetails.Count() == 10
                    )
                    )
                {
                    return false;
                }
            }

            return true ;
        }


        public int UpdateAction(int rowId,string actionToBeTaken, bool isActionUpdated)
        {
            int result;
            bool monitored = true;

            string strWhereAction = !isActionUpdated ? "Update HealthDataCollected SET IsMonitored = @Monitored , ActionToBeTaken = @ActionToBeTaken Where Id=@rowId" :
                "Update HealthDataCollected SET IsMonitored = @Monitored Where Id=@rowId";


            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(strWhereAction, conn);
                cmd.Parameters.AddWithValue("@Monitored", monitored);
                if (!isActionUpdated)
                {
                    cmd.Parameters.AddWithValue("@ActionToBeTaken", actionToBeTaken);
                }
                cmd.Parameters.AddWithValue("@rowId", rowId);

                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

    }
}
