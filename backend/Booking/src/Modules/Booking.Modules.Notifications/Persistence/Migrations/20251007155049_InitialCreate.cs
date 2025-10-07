using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Notifications.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notifications");

            migrationBuilder.CreateTable(
                name: "notification_outbox",
                schema: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_reference = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    channel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    recipient = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    recipient_user_id = table.Column<int>(type: "integer", nullable: true),
                    subject = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    template_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    attempts = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    max_attempts = table.Column<int>(type: "integer", nullable: false, defaultValue: 3),
                    last_attempt_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_error = table.Column<string>(type: "text", nullable: true),
                    scheduled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    correlation_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    provider_message_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_removed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_outbox", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_notification_outbox_correlation_id",
                schema: "notifications",
                table: "notification_outbox",
                column: "correlation_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_outbox_created_at",
                schema: "notifications",
                table: "notification_outbox",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_notification_outbox_recipient",
                schema: "notifications",
                table: "notification_outbox",
                column: "recipient");

            migrationBuilder.CreateIndex(
                name: "ix_notification_outbox_reference",
                schema: "notifications",
                table: "notification_outbox",
                column: "notification_reference",
                unique: true,
                filter: "notification_reference IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_notification_outbox_retry_candidates",
                schema: "notifications",
                table: "notification_outbox",
                columns: new[] { "status", "attempts", "max_attempts" });

            migrationBuilder.CreateIndex(
                name: "ix_notification_outbox_status_scheduled",
                schema: "notifications",
                table: "notification_outbox",
                columns: new[] { "status", "scheduled_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_outbox",
                schema: "notifications");
        }
    }
}
