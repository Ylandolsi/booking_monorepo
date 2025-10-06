using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TimeStampForUsedForStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "stats_processed_at",
                schema: "catalog",
                table: "store_visits",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "stats_processed_at",
                schema: "catalog",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stats_processed_at",
                schema: "catalog",
                table: "store_visits");

            migrationBuilder.DropColumn(
                name: "stats_processed_at",
                schema: "catalog",
                table: "orders");
        }
    }
}
