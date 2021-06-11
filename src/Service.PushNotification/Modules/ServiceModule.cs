using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Sdk.Authorization.NoSql;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.DataReader;
using Service.PushNotification.Domain.NoSql;
using Service.PushNotification.Grpc;
using Service.PushNotification.Jobs;
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
            builder.RegisterType<TemplateService>().As<ITemplateService>().SingleInstance();
            builder.RegisterType<TokenCleaner>().AsSelf().SingleInstance();
            
            RegisterAuthServices(builder);
        }

        protected void RegisterAuthServices(ContainerBuilder builder)
        {
            // he we do not use CreateNoSqlClient beacuse we have a problem with start many mynosql instances 
            var authNoSql = new MyNoSqlTcpClient(
                Program.ReloadedSettings(e => e.AuthMyNoSqlReaderHostPort),
                ApplicationEnvironment.HostName ?? $"{ApplicationEnvironment.AppName}:{ApplicationEnvironment.AppVersion}");

            builder.RegisterMyNoSqlReader<ShortRootSessionNoSqlEntity>(authNoSql, ShortRootSessionNoSqlEntity.TableName);

            authNoSql.Start();
        }
    }
}