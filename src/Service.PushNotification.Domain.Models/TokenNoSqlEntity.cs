using System;
using MyNoSqlServer.Abstractions;

namespace Service.PushNotification.Domain.Models
{
    public class TokenNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-pushnotification-tokens";
        public static string GeneratePartitionKey(string clientId) => clientId;
        public static string GenerateRowKey(string rootSessionId) => rootSessionId;
        
        /// <summary>
        /// Uniq ID of session - fact to enter login and password by user
        /// </summary>
        public string RootSessionId { get; set; }

        /// <summary>
        /// client uniq ID
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// Data and tone of create session
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// Firebase token
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// Preferred user language
        /// </summary>
        public string UserLocale { get; set; }

        public static TokenNoSqlEntity Create(string cliendId, string rootSessionId, string token, string userLocale)
        {
            return new TokenNoSqlEntity()
            {
                PartitionKey = GeneratePartitionKey(cliendId),
                RowKey = GenerateRowKey(rootSessionId),
                Token = token,
                UserLocale = userLocale,
                CreateTime = DateTime.UtcNow
            };
        }
    }
}