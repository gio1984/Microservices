using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservices.Migrations
{
    /// <inheritdoc />
    public partial class AddDoorsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DataElevator",
                table: "DataElevator");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DataElevator");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "DataElevator");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "DataElevator");

            migrationBuilder.DropColumn(
                name: "status",
                table: "DataElevator");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventIdUtc",
                table: "DataElevator",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataElevator",
                table: "DataElevator",
                column: "EventIdUtc");

            migrationBuilder.CreateTable(
                name: "DataDoor",
                columns: table => new
                {
                    EventIdUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDoor", x => x.EventIdUtc);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataDoor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataElevator",
                table: "DataElevator");

            migrationBuilder.DropColumn(
                name: "EventIdUtc",
                table: "DataElevator");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DataElevator",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "DataElevator",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "DataElevator",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "DataElevator",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataElevator",
                table: "DataElevator",
                column: "Id");
        }
    }
}
