using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class Adingqueuemessagetolinkeachtootherasareply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QueueId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_QueueId",
                table: "Messages",
                column: "QueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_QueueId",
                table: "Messages",
                column: "QueueId",
                principalTable: "Messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_QueueId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_QueueId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "QueueId",
                table: "Messages");
        }
    }
}
