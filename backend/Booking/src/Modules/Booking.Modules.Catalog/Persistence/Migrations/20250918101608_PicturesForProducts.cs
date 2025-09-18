using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PicturesForProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "thumbnail_url",
                schema: "catalog",
                table: "product");

            migrationBuilder.AddColumn<string>(
                name: "preview_main_link",
                schema: "catalog",
                table: "product",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "preview_thumbnail_link",
                schema: "catalog",
                table: "product",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "thumbnail_main_link",
                schema: "catalog",
                table: "product",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "thumbnail_thumbnail_link",
                schema: "catalog",
                table: "product",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preview_main_link",
                schema: "catalog",
                table: "product");

            migrationBuilder.DropColumn(
                name: "preview_thumbnail_link",
                schema: "catalog",
                table: "product");

            migrationBuilder.DropColumn(
                name: "thumbnail_main_link",
                schema: "catalog",
                table: "product");

            migrationBuilder.DropColumn(
                name: "thumbnail_thumbnail_link",
                schema: "catalog",
                table: "product");

            migrationBuilder.AddColumn<string>(
                name: "thumbnail_url",
                schema: "catalog",
                table: "product",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
