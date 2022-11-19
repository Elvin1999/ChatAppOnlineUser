using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    public partial class message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_CustomIdentityUserId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Message",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Message",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Message",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "CustomIdentityUserId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Message",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatId",
                table: "Message",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_ReceiverId",
                table: "Chat",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_CustomIdentityUserId",
                table: "Message",
                column: "CustomIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Chat_ChatId",
                table: "Message",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_CustomIdentityUserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chat_ChatId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Message_ChatId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Message",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Message",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Message",
                newName: "Text");

            migrationBuilder.AlterColumn<string>(
                name: "CustomIdentityUserId",
                table: "Message",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_CustomIdentityUserId",
                table: "Message",
                column: "CustomIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
