using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models
{    
    [DataContract]
    public class HistoryRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
        [DataMember(Order = 2)] public string RootSessionId { get; set; }
        [DataMember(Order = 3)] public Guid NotificationId { get; set; }
    }
}