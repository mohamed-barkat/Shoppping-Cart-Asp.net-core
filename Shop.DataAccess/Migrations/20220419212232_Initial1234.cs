using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.DataAccess.Migrations
{
    public partial class Initial1234 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationsChanges_ActorId",
                table: "NotificationsChanges");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsChanges_ActorId",
                table: "NotificationsChanges",
                column: "ActorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationsChanges_ActorId",
                table: "NotificationsChanges");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsChanges_ActorId",
                table: "NotificationsChanges",
                column: "ActorId",
                unique: true,
                filter: "[ActorId] IS NOT NULL");
        }
    }
}
