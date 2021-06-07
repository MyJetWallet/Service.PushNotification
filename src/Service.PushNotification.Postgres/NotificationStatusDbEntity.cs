using System;

namespace Service.PushNotification.Postgres
{
    public class NotificationStatusDbEntity
    {
        public Guid StatusId { get; set; }
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public NotificationHistoryDbEntity HistoryDbEntity { get; set; }
    }
}