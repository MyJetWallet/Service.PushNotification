using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.PushNotification.Postgres.Migrations
{
    public partial class UseJsonProperty1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "ParamsJson",
                schema: "pushnotification",
                table: "notification_history",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParamsJson",
                schema: "pushnotification",
                table: "notification_history");
        }
    }
}
