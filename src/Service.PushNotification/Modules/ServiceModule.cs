using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Sdk.NoSql;
using Service.PushNotification.Domain.NoSql;
using Service.PushNotification.Grpc;
using Service.PushNotification.Services;

namespace Service.PushNotification.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMyNoSqlWriter<TokenNoSqlEntity>(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl), TokenNoSqlEntity.TableName);
            builder.RegisterMyNoSqlWriter<TemplateNoSqlEntity>(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl), TemplateNoSqlEntity.TableName);

            builder.RegisterType<TokenManager>().As<ITokenManager>().SingleInstance();
            builder.RegisterType<FirebaseNotificationSender>().As<IFirebaseNotificationSender>().SingleInstance();
            builder.RegisterType<HistoryRecordingService>().As<IHistoryRecordingService>().SingleInstance();
        }
    }
}