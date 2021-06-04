using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.PushNotification.Settings
{
    public class SettingsModel
    {
        [YamlProperty("PushNotification.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("PushNotification.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("PushNotification.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("PushNotification.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }

    }
}
