using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.PushNotification.Domain.Models.Enums;

namespace Service.PushNotification.Grpc.Models.Responses
{
    [DataContract]
    public class HistoryResponse
    {
        [DataMember(Order = 1)] public Guid NotificationId { get; set; }
        [DataMember(Order = 2)] public string ClientId { get; set; }
        [DataMember(Order = 3)] public NotificationTypeEnum Type { get; set; }
        [DataMember(Order = 4)] public List<string> Params { get; set; }
        [DataMember(Order = 5)] public List<StatusResponse> StatusResponses { get; set; }
        [DataMember(Order = 6)] public DateTime TimeStamp { get; set; }
    }

    [DataContract]
    public class StatusResponse
    {
        [DataMember(Order = 1)] public Guid StatusId { get; set; }
        [DataMember(Order = 2)] public string Token { get; set; }
        [DataMember(Order = 3)] public bool IsSuccess { get; set; }
        [DataMember(Order = 4)] public string UserAgent { get; set; }
    }
}