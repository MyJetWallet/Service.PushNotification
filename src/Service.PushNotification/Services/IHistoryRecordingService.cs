using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.PushNotification.Domain.Models.Enums;
using Service.PushNotification.Postgres;

namespace Service.PushNotification.Services
{
    public interface IHistoryRecordingService
    {
        Task RecordNotification(Guid notificationId, NotificationTypeEnum type, string clientId,
            List<string> parameters);
        
        Task RecordNotificationStatuses(Guid notificationId, List<NotificationStatusDbEntity> statuses);

        Task<List<NotificationHistoryDbEntity>> GetAllRecords();
        
        Task<List<NotificationHistoryDbEntity>> GetRecordsByClientId(string clientId);
        
        Task<NotificationHistoryDbEntity> GetRecordByMessageId(Guid notificationId);
    }
}