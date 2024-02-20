using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class SizeandColoradded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BGColor",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BGColor",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "TextColor",
                table: "Colors");
        }
    }
}
