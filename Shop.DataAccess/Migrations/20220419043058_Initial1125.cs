using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.DataAccess.Migrations
{
    public partial class Initial1125 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_NotifierId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationObject_NotificationObjectId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationChange_AspNetUsers_ActorId",
                table: "NotificationChange");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationChange_NotificationObject_NotificationObjectId",
                table: "NotificationChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationObject",
                table: "NotificationObject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationChange",
                table: "NotificationChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "NotificationObject",
                newName: "NotificationObjects");

            migrationBuilder.RenameTable(
                name: "NotificationChange",
                newName: "NotificationsChanges");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationChange_NotificationObjectId",
                table: "NotificationsChanges",
                newName: "IX_NotificationsChanges_NotificationObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationChange_ActorId",
                table: "NotificationsChanges",
                newName: "IX_NotificationsChanges_ActorId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_NotifierId",
                table: "Notifications",
                newName: "IX_Notifications_NotifierId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_NotificationObjectId",
                table: "Notifications",
                newName: "IX_Notifications_NotificationObjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationObjects",
                table: "NotificationObjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationsChanges",
                table: "NotificationsChanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_NotifierId",
                table: "Notifications",
                column: "NotifierId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationObjects_NotificationObjectId",
                table: "Notifications",
                column: "NotificationObjectId",
                principalTable: "NotificationObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationsChanges_AspNetUsers_ActorId",
                table: "NotificationsChanges",
                column: "ActorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationsChanges_NotificationObjects_NotificationObjectId",
                table: "NotificationsChanges",
                column: "NotificationObjectId",
                principalTable: "NotificationObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_NotifierId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationObjects_NotificationObjectId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationsChanges_AspNetUsers_ActorId",
                table: "NotificationsChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationsChanges_NotificationObjects_NotificationObjectId",
                table: "NotificationsChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationsChanges",
                table: "NotificationsChanges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationObjects",
                table: "NotificationObjects");

            migrationBuilder.RenameTable(
                name: "NotificationsChanges",
                newName: "NotificationChange");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "NotificationObjects",
                newName: "NotificationObject");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationsChanges_NotificationObjectId",
                table: "NotificationChange",
                newName: "IX_NotificationChange_NotificationObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationsChanges_ActorId",
                table: "NotificationChange",
                newName: "IX_NotificationChange_ActorId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_NotifierId",
                table: "Notification",
                newName: "IX_Notification_NotifierId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_NotificationObjectId",
                table: "Notification",
                newName: "IX_Notification_NotificationObjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationChange",
                table: "NotificationChange",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationObject",
                table: "NotificationObject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_NotifierId",
                table: "Notification",
                column: "NotifierId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationObject_NotificationObjectId",
                table: "Notification",
                column: "NotificationObjectId",
                principalTable: "NotificationObject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationChange_AspNetUsers_ActorId",
                table: "NotificationChange",
                column: "ActorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationChange_NotificationObject_NotificationObjectId",
                table: "NotificationChange",
                column: "NotificationObjectId",
                principalTable: "NotificationObject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
