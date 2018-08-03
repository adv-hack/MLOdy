//-----------------------------------------------------------------------
// <copyright file="HealthRestService.cs" company="Advanced Health & Care">
//     Copyright © Advanced Health & Care 2018
// </copyright>
//-----------------------------------------------------------------------
namespace Ahc.Health.RestService
{
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.Text;
    using Ahc.Health.ORM;
    using System.Net;
    using System;
    using ORM.DataAccess.ExtensionMethods;
    using Ahc.Health.ORM.DataAccess;
    using Common.Constant;
    using System.Collections;
    using System.Collections.Generic;
    using Model;
    using System.Web.Script.Serialization;
    using System.IO;
    using System.Net.Http;

    public class HealthRestService : IHealthRestService
    {
        /// <summary>
        /// Gets Json HealthData
        /// </summary>
        /// <param name="key">The secret key to access this method</param>
        /// <returns>json data</returns>
        string IHealthRestService.JSONHealthData(string key)
        {
            //1. Need to validate this secret key to make more secure
            //2. Access the Health API to get the data
            //3. Searizlize the data and save to the DB
            string returnMsg;
            var client = new WebClient();
            try
            {
                client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                var newdatas = new List<HealthDataCollected>();

                using (HealthDbContext db = new HealthDbContext())
                {
                    var users = db.GetRegisteredUsers();
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    foreach (var user in users)
                    {

                        HealthDataCollected newdata = new HealthDataCollected();
                        var response = client.DownloadString(ConstaintAPIs.HEARTRATE);

                        //var routes_list = (object)json_serializer.DeserializeObject(response);

                        newdata.Parameter = GetHealthMetric(EnumHealthMetric.HEARTRATE);
                        newdata.JsonString = response.ToString();
                        newdata.ReadingTime = DateTime.Now;
                        newdata.Status = "Open";
                        newdata.UserId = user.Id;
                        newdatas.Add(newdata);
                    }
                }

                using (HealthDbContext db = new HealthDbContext())
                {
                    var retval = db.AddHealthData(newdatas);
                }
                returnMsg = "Data collected & strored successfully!! ";
            }
            catch(Exception ex)
            {
                returnMsg = "Oops!! Completed with an ERROR " + ex.Message;
            }
            client.Dispose();
            return returnMsg;
        }

        /// <summary>
        /// Gets xml health Data
        /// </summary>
        /// <param name="key">The secret key to access this method</param>
        /// <returns>json data</returns>
        string IHealthRestService.XmlHealthData(string key)
        {
            //1. Need to validate this secret key to make more secure

            return "You requested product " + key;
        }
        private string GetHealthMetric(EnumHealthMetric healthMetric)
        {
            string strHealthMetric = string.Empty;
            switch(healthMetric)
            {
                case EnumHealthMetric.HEARTRATE:
                    strHealthMetric= "HEARTRATE";
                    break;
                case EnumHealthMetric.TOTSLEEPS:
                    strHealthMetric = "TOTSLEEPS";
                    break;
            }
            return strHealthMetric;
        }

        /// <summary>
        /// Gets xml health Data
        /// </summary>
        /// <param name="key">The secret key to access this method</param>
        /// <returns>json data</returns>
        string IHealthRestService.SendNotification(string serverApiKey, string senderId, string deviceId, string message)
        {
            string result;
            var value = message;



            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", serverApiKey));
            tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message="
                + value + "&data.time=" + System.DateTime.Now.ToString() + "®istration_id=" + deviceId + "";

            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();

                        }
                    }
                }
            }


            //try
            //{
                
            //    string Baseurl = "https://fcm.googleapis.com/fcm/send";
            //    var client = new HttpClient();
            //    client.BaseAddress = new Uri(Baseurl);
            //    client.DefaultRequestHeaders.Clear();

            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + serverKey);

            //    HttpResponseMessage Res = await client.PostAsync(Baseurl, new StringContent(json, Encoding.UTF8, "application/json"));

            //    if (Res.IsSuccessStatusCode)
            //    {
            //        string returndata = await Res.Content.ReadAsStringAsync();
            //        return true;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    message = ex.Message;
            //}

            

            return message;
        }

        void IHealthRestService.SendMessage()
        {
            throw new NotImplementedException();
        }

        //static void SendMessage()
        //{
        //    string serverKey = "Your server key";

        //    try
        //    {
        //        var result = "-1";
        //        var webAddr = "https://fcm.googleapis.com/fcm/send";

        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
        //        httpWebRequest.Method = "POST";

        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            string json = "{\"to\": \"client Device token\",\"data\": {\"message\": \"This is a Firebase Cloud Messaging Topic Message!\",}}";
        //            streamWriter.Write(json);
        //            streamWriter.Flush();
        //        }

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            result = streamReader.ReadToEnd();
        //        }

        //        // return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        //  Response.Write(ex.Message);
        //    }
        //}

    }
}
