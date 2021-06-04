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

namespace Service.PushNotification.Services
{
    public class FirebaseNotificationSender : IFirebaseNotificationSender, IStartable
    {
        private readonly ILogger<FirebaseNotificationSender> _logger;

        public FirebaseNotificationSender(ILogger<FirebaseNotificationSender> logger)
        {
            _logger = logger;
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

        public async Task SendNotificationPush(string[] tokens, string message)
        {
            try
            {
                var firebaseMessage = new MulticastMessage
                {
                    Tokens = tokens.ToList(),
                    Data = new Dictionary<string, string>
                    {
                        {"message", message}
                    }
                };
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(firebaseMessage);

                if (response.FailureCount > 0)
                {
                    foreach (var resp in response.Responses.Where(r => !r.IsSuccess))
                    {
                        _logger.LogWarning("Notification {MessageId} failed with exception {Exception}",
                            resp.MessageId, resp.Exception);
                    }
                }
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