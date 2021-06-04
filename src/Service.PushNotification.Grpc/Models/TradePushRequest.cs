using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models
{
    [DataContract]
    public class TradePushRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }

        [DataMember(Order = 2)]public string Instrument { get; set; }

        [DataMember(Order = 3)]public DateTime Date { get; set; }

        [DataMember(Order = 4)]public decimal Amount { get; set; }
    }
}