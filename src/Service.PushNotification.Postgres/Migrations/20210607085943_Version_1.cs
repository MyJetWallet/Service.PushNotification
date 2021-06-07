using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Service.PushNotification.Postgres.Migrations
{
    public partial class Version_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pushnotification");

            migrationBuilder.CreateTable(
                name: "notification_history",
                schema: "pushnotification",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Params = table.Column<string>(type: "text", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_history", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "notification_statuses",
                schema: "pushnotification",
                columns: table => new
                {
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    HistoryDbEntityNotificationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_statuses", x => x.StatusId);
                    table.ForeignKey(
                        name: "FK_notification_statuses_notification_history_HistoryDbEntityN~",
                        column: x => x.HistoryDbEntityNotificationId,
                        principalSchema: "pushnotification",
                        principalTable: "notification_history",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notification_statuses_HistoryDbEntityNotificationId",
                schema: "pushnotification",
                table: "notification_statuses",
                column: "HistoryDbEntityNotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_statuses",
                schema: "pushnotification");

            migrationBuilder.DropTable(
                name: "notification_history",
                schema: "pushnotification");
        }
    }
}
