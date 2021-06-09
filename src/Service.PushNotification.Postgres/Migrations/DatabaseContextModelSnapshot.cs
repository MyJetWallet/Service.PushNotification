﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Service.PushNotification.Postgres;

namespace Service.PushNotification.Postgres.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("pushnotification")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Service.PushNotification.Postgres.NotificationHistoryDbEntity", b =>
                {
                    b.Property<Guid>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ClientId")
                        .HasColumnType("text");

                    b.Property<string>("Params")
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("NotificationId");

                    b.ToTable("notification_history");
                });

            modelBuilder.Entity("Service.PushNotification.Postgres.NotificationStatusDbEntity", b =>
                {
                    b.Property<Guid>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("HistoryDbEntityNotificationId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsSuccess")
                        .HasColumnType("boolean");

                    b.Property<Guid>("NotificationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<string>("UserAgent")
                        .HasColumnType("text");

                    b.HasKey("StatusId");

                    b.HasIndex("HistoryDbEntityNotificationId");

                    b.ToTable("notification_statuses");
                });

            modelBuilder.Entity("Service.PushNotification.Postgres.NotificationStatusDbEntity", b =>
                {
                    b.HasOne("Service.PushNotification.Postgres.NotificationHistoryDbEntity", "HistoryDbEntity")
                        .WithMany("DeliveryStatuses")
                        .HasForeignKey("HistoryDbEntityNotificationId");

                    b.Navigation("HistoryDbEntity");
                });

            modelBuilder.Entity("Service.PushNotification.Postgres.NotificationHistoryDbEntity", b =>
                {
                    b.Navigation("DeliveryStatuses");
                });
#pragma warning restore 612, 618
        }
    }
}
