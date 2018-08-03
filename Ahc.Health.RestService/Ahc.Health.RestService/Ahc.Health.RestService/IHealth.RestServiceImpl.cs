// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHealthRestService.cs" company="Advanced Health & Care">
//   Copyright © Advanced Health & Care 2018
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ahc.Health.RestService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    [ServiceContract]
    public interface IHealthRestService
    {
        /// <summary>
        /// Gets xml health Data
        /// </summary>
        /// <param name="key">The secret key to access this method</param>
        /// <returns>json data</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "xml/{key}")]
        string XmlHealthData(string key);

        /// <summary>
        /// Gets json health Data
        /// </summary>
        /// <param name="key">The secret key to access this method</param>
        /// <returns>json data</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "json/{key}")]
        string JSONHealthData(string key);

        /// <summary>
        /// Send SendNotification
        /// </summary>
        /// <param name="key">The secret key to access this method</param>
        /// <returns>json data</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "SendNotification?serverApiKey={serverApiKey}&senderId={senderId}&deviceId={deviceId}&message={message}")]
        string SendNotification(string serverApiKey, string senderId, string deviceId, string message);

        void SendMessage();

    }
}
