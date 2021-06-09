using System;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models.Requests
{
    [DataContract]
    public class GetAllTokensRequest
    {
        [DataMember(Order = 1)] public DateTime TimeStamp { get; set; }

        [DataMember(Order = 2)] public int? Take { get; set; }
    }
}