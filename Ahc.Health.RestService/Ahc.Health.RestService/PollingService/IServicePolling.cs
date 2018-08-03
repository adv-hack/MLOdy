using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PollingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServicePolling
    {
        
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "rules/{key}")]
        int InsertRules (Rules value);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetAllRules")]
        List<Rules> GetAllRules();

        [OperationContract]
        string ApplyRule(PersonDetails user);

        [OperationContract]
        PersonDetails GetUser(int userId);

        // TODO: Add your service operations here
    }

    public class InsertRulesCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the result is true
        /// </summary>
        public bool Result
        {
            get;
            set;
        }
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class PersonDetails
    {
        int id ;
        int personId;
        string name ;
        Int16 age ;
        string parameter;
        DateTime recordTime ;
        Int16 reading;
        bool processed;
        bool actionUpdated;
        string actiontobeTaken;
        
        [DataMember]
        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public Int16 Age
        {
            get { return age; }
            set { age = value; }
        }

        [DataMember]
        public DateTime RecordTime
        {
            get { return recordTime; }
            set { recordTime = value; }
        }

        [DataMember]
        public bool Processed
        {
            get { return processed ; }
            set { processed = value; }
        }

        [DataMember]
        public bool ActionUpdated
        {
            get { return actionUpdated ; }
            set { actionUpdated = value; }
        }

        [DataMember]
        public string Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        [DataMember]
        public Int16 Reading
        {
            get { return reading ; }
            set { reading = value; }
        }

        [DataMember]
        public string ActionTaken
        {
            get { return actiontobeTaken; }
            set { actiontobeTaken = value; }
        }
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.

    [DataContract]
    public class Rules
    {
        string parameter ;
        Int16 ageLower;
        Int16 ageUpper;
        Int16 rangeLower;
        Int16? rangeUpper;
        Int16? occurence;
        Int16? timeWindow;
        string action;

        [DataMember]
        public string Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        [DataMember]
        public Int16 AgeLower
        {
            get { return ageLower; }
            set { ageLower = value; }
        }

        [DataMember]
        public Int16 AgeUpper
        {
            get { return ageUpper; }
            set { ageUpper = value; }
        }

        [DataMember]
        public Int16 RangeLower
        {
            get { return rangeLower; }
            set { rangeLower = value; }
        }

        [DataMember]
        public Int16? RangeUpper
        {
            get { return rangeUpper ; }
            set { rangeUpper = value; }
        }

        [DataMember]
        public Int16? Occurence
        {
            get { return occurence; }
            set { occurence = value; }
        }

        [DataMember]
        public Int16? TimeWindow
        {
            get { return timeWindow ; }
            set { timeWindow = value; }
        }

        [DataMember]
        public string Action
        {
            get { return action ; }
            set { action = value; }
        }
    }

}
