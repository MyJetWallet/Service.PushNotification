using System;
using System.Threading.Tasks;
using Autofac;
using Service.PushNotification.Domain.Models;

namespace Service.PushNotification.Services
{
    public interface IFirebaseNotificationSender : IStartable
    {
        Task SendNotificationPush(Guid notificationId, PushToken[] tokens, string title, string body);
    }
}