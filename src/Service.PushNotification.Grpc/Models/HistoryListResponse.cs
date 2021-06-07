using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.PushNotification.Grpc.Models
{
    [DataContract]
    public class HistoryListResponse
    {
        [DataMember(Order = 1)] public List<HistoryResponse> HistoryResponses { get; set; }
    }
}