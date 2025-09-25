using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameThumbnailPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "thumbnail_thumbnail_link",
                schema: "catalog",
                table: "product",
                newName: "thumbnail_picture_thumbnail_link");

            migrationBuilder.RenameColumn(
                name: "thumbnail_main_link",
                schema: "catalog",
                table: "product",
                newName: "thumbnail_picture_main_link");

            migrationBuilder.RenameColumn(
                name: "preview_thumbnail_link",
                schema: "catalog",
                table: "product",
                newName: "preview_picture_thumbnail_link");

            migrationBuilder.RenameColumn(
                name: "preview_main_link",
                schema: "catalog",
                table: "product",
                newName: "preview_picture_main_link");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "thumbnail_picture_thumbnail_link",
                schema: "catalog",
                table: "product",
                newName: "thumbnail_thumbnail_link");

            migrationBuilder.RenameColumn(
                name: "thumbnail_picture_main_link",
                schema: "catalog",
                table: "product",
                newName: "thumbnail_main_link");

            migrationBuilder.RenameColumn(
                name: "preview_picture_thumbnail_link",
                schema: "catalog",
                table: "product",
                newName: "preview_thumbnail_link");

            migrationBuilder.RenameColumn(
                name: "preview_picture_main_link",
                schema: "catalog",
                table: "product",
                newName: "preview_main_link");
        }
    }
}
