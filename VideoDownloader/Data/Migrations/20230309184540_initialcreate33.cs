using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoDownloader.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Logins",
                newName: "IsInProgress");

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Logins",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFailed",
                table: "Logins",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Logins",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "IsFailed",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Logins");

            migrationBuilder.RenameColumn(
                name: "IsInProgress",
                table: "Logins",
                newName: "Status");
        }
    }
}
