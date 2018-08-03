using Ahc.Health.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;


namespace Ahc.Health.ORM.DataAccess.ExtensionMethods
{
    public static class JSONHelper
    {

        public static string ToJSON(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static List<HealthDataCollected> DeserializeJSON(this string jSonString)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<List<HealthDataCollected>>(jSonString);
        }

    }
}