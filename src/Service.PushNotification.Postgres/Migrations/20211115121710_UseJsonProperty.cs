using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.PushNotification.Postgres.Migrations
{
    public partial class UseJsonProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Params",
                schema: "pushnotification",
                table: "notification_history");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStamp",
                schema: "pushnotification",
                table: "notification_history",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStamp",
                schema: "pushnotification",
                table: "notification_history",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Params",
                schema: "pushnotification",
                table: "notification_history",
                type: "text",
                nullable: true);
        }
    }
}
