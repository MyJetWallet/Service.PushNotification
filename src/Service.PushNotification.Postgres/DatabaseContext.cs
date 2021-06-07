using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Service.PushNotification.Postgres
{
    public class DatabaseContext : DbContext
    {
        public const string Schema = "pushnotification";
        public const string NotificationHistoryTableName = "notification_history";
        public const string NotificationStatusTableName = "notification_statuses";

        public DbSet<NotificationHistoryDbEntity> NotificationHistoryDbEntities { get; set; }
        public DbSet<NotificationStatusDbEntity> NotificationStatusDbEntities { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public static ILoggerFactory LoggerFactory { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (LoggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(LoggerFactory).EnableSensitiveDataLogging();
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<NotificationHistoryDbEntity>().ToTable(NotificationHistoryTableName);
            modelBuilder.Entity<NotificationHistoryDbEntity>().HasKey(e => e.NotificationId);
            modelBuilder.Entity<NotificationHistoryDbEntity>().Property(e => e.Params).HasConversion(
                p => JsonSerializer.Serialize(p, null),
                p => JsonSerializer.Deserialize<List<string>>(p, null));

            modelBuilder.Entity<NotificationStatusDbEntity>().ToTable(NotificationStatusTableName);
            modelBuilder.Entity<NotificationStatusDbEntity>().HasKey(e => e.StatusId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}