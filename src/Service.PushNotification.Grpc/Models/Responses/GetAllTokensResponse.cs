using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.PushNotification.Domain.Models;

namespace Service.PushNotification.Grpc.Models.Responses
{
    [DataContract]
    public class GetAllTokensResponse
    {
        [DataMember(Order = 1)] public List<PushToken> Tokens { get; set; }
    }
}