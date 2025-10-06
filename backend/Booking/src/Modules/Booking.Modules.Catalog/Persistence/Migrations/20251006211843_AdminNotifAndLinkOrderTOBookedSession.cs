using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdminNotifAndLinkOrderTOBookedSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "scheduled_at",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "session_end_time",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "amount_paid",
                schema: "catalog",
                table: "booked_sessions");

            migrationBuilder.AddColumn<int>(
                name: "order_id",
                schema: "catalog",
                table: "booked_sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "admin_notifications",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    severity = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    related_entity_id = table.Column<string>(type: "text", nullable: true),
                    related_entity_type = table.Column<string>(type: "text", nullable: true),
                    metadata = table.Column<string>(type: "text", nullable: true),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_admin_notifications", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_notifications",
                schema: "catalog");

            migrationBuilder.DropColumn(
                name: "order_id",
                schema: "catalog",
                table: "booked_sessions");

            migrationBuilder.AddColumn<DateTime>(
                name: "scheduled_at",
                schema: "catalog",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "session_end_time",
                schema: "catalog",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "amount_paid",
                schema: "catalog",
                table: "booked_sessions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
