using Microsoft.EntityFrameworkCore.Migrations;

namespace Gallery.Data.Migrations
{
    public partial class ChangeAlternativeKeyToIndexForAlbumAndTagTitles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Tags_Title",
                table: "Tags");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Albums_Title",
                table: "Albums");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Title",
                table: "Tags",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_Title",
                table: "Albums",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Title",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Albums_Title",
                table: "Albums");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tags",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Tags_Title",
                table: "Tags",
                column: "Title");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Albums_Title",
                table: "Albums",
                column: "Title");
        }
    }
}
