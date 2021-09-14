using System.Collections.Generic;
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

        [YamlProperty("PushNotification.PostgresConnectionString")]
        public string PostgresConnectionString { get; set; }
        
        [YamlProperty("PushNotification.EncodedFirebaseCredentials")]
        public Dictionary<string,string> EncodedFirebaseCredentials { get; set; }
        
        [YamlProperty("PushNotification.AuthMyNoSqlReaderHostPort")]
        public string AuthMyNoSqlReaderHostPort { get; set; }
        
        [YamlProperty("PushNotification.TimerPeriodInSec")]
        public int TimerPeriodInSec { get; set; }
        
    }
}
