using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDoorIdCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoorId",
                table: "DataDoor",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DataDoor",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoorId",
                table: "DataDoor");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DataDoor");
        }
    }
}
