using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Service.PushNotification.Postgres.Migrations
{
    public partial class StatusRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                schema: "pushnotification",
                table: "notification_statuses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationId",
                schema: "pushnotification",
                table: "notification_statuses");
        }
    }
}
