using Microsoft.EntityFrameworkCore.Migrations;

namespace Service.PushNotification.Postgres.Migrations
{
    public partial class useragent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                schema: "pushnotification",
                table: "notification_statuses",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAgent",
                schema: "pushnotification",
                table: "notification_statuses");
        }
    }
}
