using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{    
    [DataContract]
    public class HistoryRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
        [DataMember(Order = 2)] public string RootSessionId { get; set; }
        [DataMember(Order = 3)] public Guid NotificationId { get; set; }
        [DataMember(Order = 4)] public DateTime TimeStamp { get; set; }
        [DataMember(Order = 5)] public int? Take { get; set; }
    }
}