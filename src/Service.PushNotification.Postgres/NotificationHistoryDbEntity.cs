using System;
using System.Collections.Generic;
using Service.PushNotification.Domain.Models.Enums;

namespace Service.PushNotification.Postgres
{
    public class NotificationHistoryDbEntity
    {
        public Guid NotificationId { get; set; }
        
        public string ClientId { get; set; }
        
        public NotificationTypeEnum Type { get; set; }
        
        public List<string> Params { get; set; }
        
        public DateTime TimeStamp { get; set; }
        
        public List<NotificationStatusDbEntity> DeliveryStatuses { get; set; }

    }
}