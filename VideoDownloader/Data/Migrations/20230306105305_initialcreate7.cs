using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoDownloader.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailId",
                table: "Logins",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "Logins");
        }
    }
}
