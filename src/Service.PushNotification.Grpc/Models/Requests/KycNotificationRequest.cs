using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class KycNotificationRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
    }
}