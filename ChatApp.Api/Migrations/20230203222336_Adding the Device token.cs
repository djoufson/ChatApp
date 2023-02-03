using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddingtheDevicetoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceToken",
                table: "AspNetUsers");
        }
    }
}
