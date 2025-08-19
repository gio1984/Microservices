using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.Migrations
{
    /// <inheritdoc />
    public partial class AddDataDetectorFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DetectorId",
                table: "DataSmokeDetector",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DataSmokeDetector",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetectorId",
                table: "DataSmokeDetector");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DataSmokeDetector");
        }
    }
}
