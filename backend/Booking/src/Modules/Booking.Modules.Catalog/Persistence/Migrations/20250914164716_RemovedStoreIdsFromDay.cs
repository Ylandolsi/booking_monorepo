using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedStoreIdsFromDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "store_id",
                schema: "catalog",
                table: "days");

            migrationBuilder.DropColumn(
                name: "store_slug",
                schema: "catalog",
                table: "days");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "store_id",
                schema: "catalog",
                table: "days",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "store_slug",
                schema: "catalog",
                table: "days",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
