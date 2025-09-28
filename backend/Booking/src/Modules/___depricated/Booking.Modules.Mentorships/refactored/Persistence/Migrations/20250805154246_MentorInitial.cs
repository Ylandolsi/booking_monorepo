using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MentorInitial : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mentorships");

            migrationBuilder.CreateTable(
                name: "mentors",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    user_slug = table.Column<string>(type: "text", nullable: false),
                    hourly_rate_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    hourly_rate_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_active_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mentors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "availabilities",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mentor_id = table.Column<int>(type: "integer", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    start_hour = table.Column<int>(type: "integer", nullable: false),
                    end_hour = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_availabilities", x => x.id);
                    table.ForeignKey(
                        name: "fk_availabilities_mentors_mentor_id",
                        column: x => x.mentor_id,
                        principalSchema: "mentorships",
                        principalTable: "mentors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mentorship_relationships",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mentor_id = table.Column<int>(type: "integer", nullable: false),
                    mentee_id = table.Column<int>(type: "integer", nullable: false),
                    session_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_spent = table.Column<decimal>(type: "numeric(10,2)", nullable: false, defaultValue: 0m),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_session_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mentorship_relationships", x => x.id);
                    table.ForeignKey(
                        name: "fk_mentorship_relationships_mentors_mentor_id",
                        column: x => x.mentor_id,
                        principalSchema: "mentorships",
                        principalTable: "mentors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mentor_id = table.Column<int>(type: "integer", nullable: false),
                    mentee_id = table.Column<int>(type: "integer", nullable: false),
                    duration_minutes = table.Column<int>(type: "integer", nullable: false),
                    price_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    price_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    scheduled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    google_meet_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    reschedule_requested = table.Column<bool>(type: "boolean", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    mentorship_relationship_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_sessions_mentors_mentor_id",
                        column: x => x.mentor_id,
                        principalSchema: "mentorships",
                        principalTable: "mentors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sessions_mentorship_relationships_mentorship_relationship_id",
                        column: x => x.mentorship_relationship_id,
                        principalSchema: "mentorships",
                        principalTable: "mentorship_relationships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<int>(type: "integer", nullable: false),
                    mentor_id = table.Column<int>(type: "integer", nullable: false),
                    mentee_id = table.Column<int>(type: "integer", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_reviews_mentors_mentor_id",
                        column: x => x.mentor_id,
                        principalSchema: "mentorships",
                        principalTable: "mentors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reviews_sessions_session_id",
                        column: x => x.session_id,
                        principalSchema: "mentorships",
                        principalTable: "sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_availabilities_day_of_week",
                schema: "mentorships",
                table: "availabilities",
                column: "day_of_week");

            migrationBuilder.CreateIndex(
                name: "ix_availabilities_is_active",
                schema: "mentorships",
                table: "availabilities",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_availabilities_mentor_id",
                schema: "mentorships",
                table: "availabilities",
                column: "mentor_id");

            migrationBuilder.CreateIndex(
                name: "ix_mentors_user_id",
                schema: "mentorships",
                table: "mentors",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_mentorship_relationships_mentee_id",
                schema: "mentorships",
                table: "mentorship_relationships",
                column: "mentee_id");

            migrationBuilder.CreateIndex(
                name: "ix_mentorship_relationships_mentor_id",
                schema: "mentorships",
                table: "mentorship_relationships",
                column: "mentor_id");

            migrationBuilder.CreateIndex(
                name: "ix_mentorship_relationships_mentor_id_mentee_id",
                schema: "mentorships",
                table: "mentorship_relationships",
                columns: new[] { "mentor_id", "mentee_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reviews_mentee_id",
                schema: "mentorships",
                table: "reviews",
                column: "mentee_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_mentor_id",
                schema: "mentorships",
                table: "reviews",
                column: "mentor_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_session_id",
                schema: "mentorships",
                table: "reviews",
                column: "session_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sessions_mentee_id",
                schema: "mentorships",
                table: "sessions",
                column: "mentee_id");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_mentor_id",
                schema: "mentorships",
                table: "sessions",
                column: "mentor_id");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_mentorship_relationship_id",
                schema: "mentorships",
                table: "sessions",
                column: "mentorship_relationship_id");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_scheduled_at",
                schema: "mentorships",
                table: "sessions",
                column: "scheduled_at");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_status",
                schema: "mentorships",
                table: "sessions",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "availabilities",
                schema: "mentorships");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "mentorships");

            migrationBuilder.DropTable(
                name: "reviews",
                schema: "mentorships");

            migrationBuilder.DropTable(
                name: "sessions",
                schema: "mentorships");

            migrationBuilder.DropTable(
                name: "mentorship_relationships",
                schema: "mentorships");

            migrationBuilder.DropTable(
                name: "mentors",
                schema: "mentorships");
        }
    }
}
