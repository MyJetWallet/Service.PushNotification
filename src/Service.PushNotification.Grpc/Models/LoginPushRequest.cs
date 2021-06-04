using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models
{
    [DataContract]
    public class LoginPushRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
        
        [DataMember(Order = 2)]public DateTime Date { get; set; }
        
        [DataMember(Order = 3)]public string Ip { get; set; }
    }
}