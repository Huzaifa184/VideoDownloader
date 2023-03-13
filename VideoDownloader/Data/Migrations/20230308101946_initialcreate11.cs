using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoDownloader.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "Logins",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Logins");
        }
    }
}
