using Autofac;
using Service.PushNotification.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.PushNotification.Client
{
    public static class AutofacHelper
    {
        public static void RegisterPushNotificationClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new PushNotificationClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetTokenManager()).As<ITokenManager>().SingleInstance();

            builder.RegisterInstance(factory.GetNotificationService()).As<INotificationService>().SingleInstance();

            builder.RegisterInstance(factory.GetHistoryService()).As<IHistoryService>().SingleInstance();

            builder.RegisterInstance(factory.GetTemplateService()).As<ITemplateService>().SingleInstance();
        }
    }
}
