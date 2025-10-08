using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Modules.Notifications.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NotificationPresistence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "in_app_notifications",
                schema: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    recipient_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    read_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    metadata = table.Column<string>(type: "jsonb", nullable: true),
                    related_entity_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    related_entity_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    correlation_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_removed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_in_app_notifications", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_in_app_notifications_admin_queries",
                schema: "notifications",
                table: "in_app_notifications",
                columns: new[] { "recipient_id", "type", "is_read", "created_at" },
                filter: "recipient_id = 'admins'");

            migrationBuilder.CreateIndex(
                name: "ix_in_app_notifications_correlation_id",
                schema: "notifications",
                table: "in_app_notifications",
                column: "correlation_id");

            migrationBuilder.CreateIndex(
                name: "ix_in_app_notifications_created_at",
                schema: "notifications",
                table: "in_app_notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_in_app_notifications_recipient_id",
                schema: "notifications",
                table: "in_app_notifications",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "ix_in_app_notifications_recipient_read_status",
                schema: "notifications",
                table: "in_app_notifications",
                columns: new[] { "recipient_id", "is_read" });

            migrationBuilder.CreateIndex(
                name: "ix_in_app_notifications_related_entity",
                schema: "notifications",
                table: "in_app_notifications",
                columns: new[] { "related_entity_type", "related_entity_id" },
                filter: "related_entity_type IS NOT NULL AND related_entity_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_in_app_notifications_type_severity",
                schema: "notifications",
                table: "in_app_notifications",
                columns: new[] { "type", "severity" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "in_app_notifications",
                schema: "notifications");
        }
    }
}
