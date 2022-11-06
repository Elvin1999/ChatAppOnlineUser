using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    public partial class FriendRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomIdentityUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFriend",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomIdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_CustomIdentityUserId",
                        column: x => x.CustomIdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomIdentityUserId",
                table: "AspNetUsers",
                column: "CustomIdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_CustomIdentityUserId",
                table: "FriendRequests",
                column: "CustomIdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CustomIdentityUserId",
                table: "AspNetUsers",
                column: "CustomIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CustomIdentityUserId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CustomIdentityUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CustomIdentityUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsFriend",
                table: "AspNetUsers");
        }
    }
}
