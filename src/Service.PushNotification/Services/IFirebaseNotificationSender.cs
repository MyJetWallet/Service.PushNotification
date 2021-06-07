using System;
using System.Threading.Tasks;
using Autofac;

namespace Service.PushNotification.Services
{
    public interface IFirebaseNotificationSender : IStartable
    {
        Task SendNotificationPush(Guid notificationId, string[] tokens, string title, string body);
    }
}