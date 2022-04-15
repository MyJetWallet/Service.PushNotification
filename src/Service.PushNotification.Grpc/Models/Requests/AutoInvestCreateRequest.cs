using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class AutoInvestCreateRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
        [DataMember(Order = 2)] public string ToAsset { get; set; }
        [DataMember(Order = 3)] public string ScheduleType { get; set; }
        [DataMember(Order = 4)] public decimal FromAmount { get; set; }
        [DataMember(Order = 5)] public string FromAsset { get; set; }
    }
}