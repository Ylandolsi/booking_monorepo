using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConfigEntities : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions",
                schema: "mentorships");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                schema: "mentorships",
                table: "sessions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_mentee_id_status",
                schema: "mentorships",
                table: "sessions",
                columns: new[] { "mentee_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_sessions_mentor_id_status",
                schema: "mentorships",
                table: "sessions",
                columns: new[] { "mentor_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_sessions_scheduled_at_status",
                schema: "mentorships",
                table: "sessions",
                columns: new[] { "scheduled_at", "status" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Session_Date_Valid",
                schema: "mentorships",
                table: "sessions",
                sql: "scheduled_at > created_at");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Session_Duration_Valid",
                schema: "mentorships",
                table: "sessions",
                sql: "duration_minutes > 0 AND duration_minutes <= 480");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Session_Price_Positive",
                schema: "mentorships",
                table: "sessions",
                sql: "price_amount > 0");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_created_at",
                schema: "mentorships",
                table: "reviews",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_mentor_id_rating",
                schema: "mentorships",
                table: "reviews",
                columns: new[] { "mentor_id", "rating" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_Comment_Length",
                schema: "mentorships",
                table: "reviews",
                sql: "comment IS NULL OR LENGTH(comment) <= 1000");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_Dates_Valid",
                schema: "mentorships",
                table: "reviews",
                sql: "updated_at >= created_at");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_Rating_Valid",
                schema: "mentorships",
                table: "reviews",
                sql: "rating >= 1 AND rating <= 5");

            migrationBuilder.CreateIndex(
                name: "ix_payments_mentor_id",
                schema: "mentorships",
                table: "payments",
                column: "mentor_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_mentor_id_status",
                schema: "mentorships",
                table: "payments",
                columns: new[] { "mentor_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_payments_user_id_status",
                schema: "mentorships",
                table: "payments",
                columns: new[] { "user_id", "status" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payment_Price_Positive",
                schema: "mentorships",
                table: "payments",
                sql: "price > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payment_Reference_Valid",
                schema: "mentorships",
                table: "payments",
                sql: "LENGTH(reference) >= 10 AND LENGTH(reference) <= 255");

            migrationBuilder.CreateIndex(
                name: "ix_mentors_created_at",
                schema: "mentorships",
                table: "mentors",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_mentors_is_active",
                schema: "mentorships",
                table: "mentors",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_mentors_is_active_last_active_at",
                schema: "mentorships",
                table: "mentors",
                columns: new[] { "is_active", "last_active_at" });

            migrationBuilder.CreateIndex(
                name: "ix_mentors_user_slug",
                schema: "mentorships",
                table: "mentors",
                column: "user_slug",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mentor_BufferTime_Valid",
                schema: "mentorships",
                table: "mentors",
                sql: "buffer_time_minutes >= 0 AND buffer_time_minutes <= 120");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Mentor_HourlyRate_Positive",
                schema: "mentorships",
                table: "mentors",
                sql: "hourly_rate_amount > 0");

            migrationBuilder.CreateIndex(
                name: "ix_availabilities_mentor_id_day_of_week_is_active",
                schema: "mentorships",
                table: "availabilities",
                columns: new[] { "mentor_id", "day_of_week", "is_active" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Availability_DayOfWeek_Valid",
                schema: "mentorships",
                table: "availabilities",
                sql: "day_of_week >= 0 AND day_of_week <= 6");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Availability_TimeRange_BusinessHours",
                schema: "mentorships",
                table: "availabilities",
                sql: "start_time >= '00:00:00' AND end_time <= '23:59:59'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Availability_TimeRange_Valid",
                schema: "mentorships",
                table: "availabilities",
                sql: "start_time < end_time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_sessions_mentee_id_status",
                schema: "mentorships",
                table: "sessions");

            migrationBuilder.DropIndex(
                name: "ix_sessions_mentor_id_status",
                schema: "mentorships",
                table: "sessions");

            migrationBuilder.DropIndex(
                name: "ix_sessions_scheduled_at_status",
                schema: "mentorships",
                table: "sessions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Session_Date_Valid",
                schema: "mentorships",
                table: "sessions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Session_Duration_Valid",
                schema: "mentorships",
                table: "sessions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Session_Price_Positive",
                schema: "mentorships",
                table: "sessions");

            migrationBuilder.DropIndex(
                name: "ix_reviews_created_at",
                schema: "mentorships",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "ix_reviews_mentor_id_rating",
                schema: "mentorships",
                table: "reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_Comment_Length",
                schema: "mentorships",
                table: "reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_Dates_Valid",
                schema: "mentorships",
                table: "reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_Rating_Valid",
                schema: "mentorships",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "ix_payments_mentor_id",
                schema: "mentorships",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "ix_payments_mentor_id_status",
                schema: "mentorships",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "ix_payments_user_id_status",
                schema: "mentorships",
                table: "payments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Payment_Price_Positive",
                schema: "mentorships",
                table: "payments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Payment_Reference_Valid",
                schema: "mentorships",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "ix_mentors_created_at",
                schema: "mentorships",
                table: "mentors");

            migrationBuilder.DropIndex(
                name: "ix_mentors_is_active",
                schema: "mentorships",
                table: "mentors");

            migrationBuilder.DropIndex(
                name: "ix_mentors_is_active_last_active_at",
                schema: "mentorships",
                table: "mentors");

            migrationBuilder.DropIndex(
                name: "ix_mentors_user_slug",
                schema: "mentorships",
                table: "mentors");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mentor_BufferTime_Valid",
                schema: "mentorships",
                table: "mentors");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Mentor_HourlyRate_Positive",
                schema: "mentorships",
                table: "mentors");

            migrationBuilder.DropIndex(
                name: "ix_availabilities_mentor_id_day_of_week_is_active",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Availability_DayOfWeek_Valid",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Availability_TimeRange_BusinessHours",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Availability_TimeRange_Valid",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                schema: "mentorships",
                table: "sessions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "transactions",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    escrow_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_transactions_escrow_id",
                schema: "mentorships",
                table: "transactions",
                column: "escrow_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_status",
                schema: "mentorships",
                table: "transactions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_user_id",
                schema: "mentorships",
                table: "transactions",
                column: "user_id");
        }
    }
}
