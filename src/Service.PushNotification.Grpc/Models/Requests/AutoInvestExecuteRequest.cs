using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class AutoInvestExecuteRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
        [DataMember(Order = 2)] public string ToAsset { get; set; }
        [DataMember(Order = 3)] public decimal ToAmount { get; set; }
        [DataMember(Order = 4)] public decimal FromAmount { get; set; }
        [DataMember(Order = 5)] public string FromAsset { get; set; }
        [DataMember(Order = 6)] public DateTime ExecutionTime { get; set; }
    }
}