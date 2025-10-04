using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProducTIdFromDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_id",
                schema: "catalog",
                table: "days");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "product_id",
                schema: "catalog",
                table: "days",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
