using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Serilog;
using Service.PushNotification.Domain.Models;
using Service.PushNotification.Postgres;

namespace Service.PushNotification.Services
{
    public class FirebaseNotificationSender : IFirebaseNotificationSender, IStartable
    {
        private readonly ILogger<FirebaseNotificationSender> _logger;
        private readonly IHistoryRecordingService _historyService;

        public FirebaseNotificationSender(ILogger<FirebaseNotificationSender> logger, IHistoryRecordingService historyService)
        {
            _logger = logger;
            _historyService = historyService;
        }

        public void Start()
        {
            try
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(DecodeFromBase64(Program.Settings.EncodedFirebaseCredentials))
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "When creating instance of Firebase App");
                throw;
            }
        }

        public async Task SendNotificationPush(Guid messageId, PushToken[] tokens, string title, string body)
        {
            try
            {
                var statuses = new List<NotificationStatusDbEntity>();
                var firebaseMessage = new MulticastMessage
                {
                    Tokens = tokens.Select(t=>t.Token).ToList(),
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    }
                };
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(firebaseMessage);

                for (var index = 0; index < response.Responses.Count; index++)
                {
                    var msg = response.Responses[index];
                    statuses.Add(new NotificationStatusDbEntity()
                    {
                        NotificationId = messageId,
                        StatusId = Guid.NewGuid(),
                        IsSuccess = msg.IsSuccess,
                        Token = tokens[index].Token,
                        UserAgent = tokens[index].UserAgent 
                    });
                    if (!msg.IsSuccess)
                        _logger.LogWarning("Notification {MessageId} with {Token} failed with exception {Exception}", messageId, tokens[index],
                            msg.Exception);
                }

                await _historyService.RecordNotificationStatuses(messageId, statuses);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Firebase message");
                throw;
            }
        }

        private string DecodeFromBase64(string encoded)
        {
            var bytes = Convert.FromBase64String(encoded);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}