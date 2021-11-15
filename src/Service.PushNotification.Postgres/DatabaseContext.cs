using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MyJetWallet.Sdk.Postgres;

namespace Service.PushNotification.Postgres
{
    public class DatabaseContext : MyDbContext
    {
        public const string Schema = "pushnotification";
        public const string NotificationHistoryTableName = "notification_history";
        public const string NotificationStatusTableName = "notification_statuses";

        public DbSet<NotificationHistoryDbEntity> NotificationHistoryDbEntities { get; set; }
        public DbSet<NotificationStatusDbEntity> NotificationStatusDbEntities { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<NotificationHistoryDbEntity>().ToTable(NotificationHistoryTableName);
            modelBuilder.Entity<NotificationHistoryDbEntity>().HasKey(e => e.NotificationId);
            modelBuilder.Entity<NotificationHistoryDbEntity>()
                .Property(e => e.Params)
                .HasColumnName("ParamsJson")
                .HasColumnType("jsonb");

            modelBuilder.Entity<NotificationStatusDbEntity>().ToTable(NotificationStatusTableName);
            modelBuilder.Entity<NotificationStatusDbEntity>().HasKey(e => e.StatusId);

            modelBuilder.Entity<NotificationStatusDbEntity>()
                .HasOne(s => s.HistoryDbEntity)
                .WithMany(h => h.DeliveryStatuses);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}