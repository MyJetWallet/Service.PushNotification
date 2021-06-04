using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Sdk.NoSql;
using Service.PushNotification.Domain.NoSql;

namespace Service.PushNotification.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMyNoSqlWriter<TokenNoSqlEntity>(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl), TokenNoSqlEntity.TableName);
        }
    }
}