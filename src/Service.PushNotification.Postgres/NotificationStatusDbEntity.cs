using System;

namespace Service.PushNotification.Postgres
{
    public class NotificationStatusDbEntity
    {
        public Guid StatusId { get; set; }
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        
        public string UserAgent { get; set; }
        public Guid NotificationId { get; set; }
        public NotificationHistoryDbEntity HistoryDbEntity { get; set; }
    }
}