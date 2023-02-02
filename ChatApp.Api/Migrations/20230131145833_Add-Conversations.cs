using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddConversations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Conversations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_AppUserId",
                table: "Conversations",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_AspNetUsers_AppUserId",
                table: "Conversations",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_AspNetUsers_AppUserId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_AppUserId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Conversations");
        }
    }
}
