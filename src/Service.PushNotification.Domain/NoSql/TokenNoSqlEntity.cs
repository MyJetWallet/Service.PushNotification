using System;
using MyNoSqlServer.Abstractions;
using Service.PushNotification.Domain.Models;

namespace Service.PushNotification.Domain.NoSql
{
    public class TokenNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-pushnotification-tokens";
        public static string GeneratePartitionKey(string clientId) => clientId;
        public static string GenerateRowKey(string rootSessionId) => rootSessionId;
        
        
        public PushToken PushToken { get; set; }

        /// <summary>
        /// Data and tone of create session
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        public static TokenNoSqlEntity Create(PushToken token)
        {
            return new TokenNoSqlEntity()
            {
                PartitionKey = GeneratePartitionKey(token.ClientId),
                RowKey = GenerateRowKey(token.RootSessionId),
                PushToken = token,
                CreateTime = DateTime.UtcNow
            };
        }
    }
}