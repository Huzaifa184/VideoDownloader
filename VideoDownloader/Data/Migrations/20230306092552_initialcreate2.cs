using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoDownloader.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logins_Urls_Userid",
                table: "Logins");

            migrationBuilder.DropIndex(
                name: "IX_Logins_Userid",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Urls");

            migrationBuilder.AlterColumn<string>(
                name: "Userid",
                table: "Logins",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Urls",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Userid",
                table: "Logins",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_Userid",
                table: "Logins",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Logins_Urls_Userid",
                table: "Logins",
                column: "Userid",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
