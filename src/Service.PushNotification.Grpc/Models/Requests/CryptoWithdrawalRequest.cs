using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class CryptoWithdrawalRequest
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }

        [DataMember(Order = 2)] public decimal Amount { get; set; }

        [DataMember(Order = 3)] public string Symbol { get; set; }

        [DataMember(Order = 4)] public string Destination { get; set; }
    }
}