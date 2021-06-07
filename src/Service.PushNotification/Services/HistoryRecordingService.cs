using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Service.PushNotification.Domain.Models.Enums;
using Service.PushNotification.Postgres;

namespace Service.PushNotification.Services
{
    public class HistoryRecordingService : IHistoryRecordingService
    {
        private readonly DbContextOptionsBuilder<DatabaseContext> _db;
        private readonly ILogger<HistoryRecordingService> _logger;

        public HistoryRecordingService(DbContextOptionsBuilder<DatabaseContext> db, ILogger<HistoryRecordingService>  logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task RecordNotification(Guid notificationId, NotificationTypeEnum type, string clientId, List<string> parameters)
        {
            try
            {
                using (var ctx = new DatabaseContext(_db.Options))
                {
                    await ctx.NotificationHistoryDbEntities.AddAsync(new NotificationHistoryDbEntity()
                    {
                        ClientId = clientId,
                        NotificationId = notificationId,
                        Params = parameters,
                        TimeStamp = DateTime.UtcNow,
                        Type = type

                    });
                    await ctx.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When recording notification history for message with id {Id}", notificationId);
                throw;
            }
        }

        public async Task RecordNotificationStatuses(Guid notificationId, List<NotificationStatusDbEntity> statuses)
        {
            try
            {
                using (var ctx = new DatabaseContext(_db.Options))
                {
                    var history = await ctx.NotificationHistoryDbEntities.Where(h => h.NotificationId == notificationId)
                        .FirstOrDefaultAsync();
                    foreach (var s in statuses)
                    {
                        s.HistoryDbEntity = history;
                    }

                    await ctx.NotificationStatusDbEntities.AddRangeAsync(statuses);
                    await ctx.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When recording notification statuses for message with id {Id}", notificationId);
                throw;
            }
        }
        
    }
}