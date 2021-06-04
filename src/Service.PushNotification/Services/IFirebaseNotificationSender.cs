using System.Threading.Tasks;
using Autofac;

namespace Service.PushNotification.Services
{
    public interface IFirebaseNotificationSender : IStartable
    {
        Task SendNotificationPush(string[] tokens, string message);
    }
}