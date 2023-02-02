using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class Finishingtheconversationssystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "AppUserConversation",
                columns: table => new
                {
                    ConversationsId = table.Column<int>(type: "int", nullable: false),
                    ParticipentsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserConversation", x => new { x.ConversationsId, x.ParticipentsId });
                    table.ForeignKey(
                        name: "FK_AppUserConversation_AspNetUsers_ParticipentsId",
                        column: x => x.ParticipentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserConversation_Conversations_ConversationsId",
                        column: x => x.ConversationsId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserConversation_ParticipentsId",
                table: "AppUserConversation",
                column: "ParticipentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserConversation");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Conversations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WithUserId",
                table: "Conversations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
