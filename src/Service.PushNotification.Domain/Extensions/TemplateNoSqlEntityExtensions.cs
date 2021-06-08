using System.Linq;
using Service.PushNotification.Domain.Models;
using Service.PushNotification.Domain.NoSql;

namespace Service.PushNotification.Domain.Extensions
{
    public static class TemplateNoSqlEntityExtensions
    {
        public static NotificationTemplate ToTemplate(this TemplateNoSqlEntity entity)
        {
            return new NotificationTemplate()
            {
                Type = entity.Type,
                DefaultBrand = entity.DefaultBrand,
                DefaultLang = entity.DefaultLang,
                Params = entity.Params,
                Bodies = entity.BodiesSerializable.ToDictionary(pair => ParseTuple(pair.Key), pair => pair.Value)
            };
            
            //locals
            static (string, string) ParseTuple(string key) => (key.Split(':')[0], key.Split(':')[1]);
        }
    }
}