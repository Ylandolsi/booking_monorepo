using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminNotifEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "stores",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "store_visits",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "store_daily_stats",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "session_availabilities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "product_daily_stats",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "payouts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "payments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "escrows",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "days",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "booked_sessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "catalog",
                table: "admin_notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "stores");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "store_visits");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "store_daily_stats");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "session_availabilities");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "products");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "product_daily_stats");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "payouts");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "escrows");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "days");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "booked_sessions");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "catalog",
                table: "admin_notifications");
        }
    }
}
