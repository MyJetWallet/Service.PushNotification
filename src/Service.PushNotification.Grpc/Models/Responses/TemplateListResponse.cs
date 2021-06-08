using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.PushNotification.Domain.Models;

namespace Service.PushNotification.Grpc.Models.Responses
{
    [DataContract]
    public class TemplateListResponse
    {
        [DataMember(Order = 1)]
        public List<NotificationTemplate> Templates { get; set; }
    }
}