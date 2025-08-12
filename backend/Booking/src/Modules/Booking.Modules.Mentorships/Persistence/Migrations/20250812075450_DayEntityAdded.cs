using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DayEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "day_id",
                schema: "mentorships",
                table: "availabilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "days",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mentor_id = table.Column<int>(type: "integer", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_days", x => x.id);
                    table.ForeignKey(
                        name: "fk_days_mentors_mentor_id",
                        column: x => x.mentor_id,
                        principalSchema: "mentorships",
                        principalTable: "mentors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_availabilities_day_id",
                schema: "mentorships",
                table: "availabilities",
                column: "day_id");

            migrationBuilder.CreateIndex(
                name: "ix_days_day_of_week",
                schema: "mentorships",
                table: "days",
                column: "day_of_week");

            migrationBuilder.CreateIndex(
                name: "ix_days_is_active",
                schema: "mentorships",
                table: "days",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_days_mentor_id",
                schema: "mentorships",
                table: "days",
                column: "mentor_id");

            migrationBuilder.CreateIndex(
                name: "ix_days_mentor_id_day_of_week",
                schema: "mentorships",
                table: "days",
                columns: new[] { "mentor_id", "day_of_week" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_availabilities_days_day_id",
                schema: "mentorships",
                table: "availabilities",
                column: "day_id",
                principalSchema: "mentorships",
                principalTable: "days",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_availabilities_days_day_id",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropTable(
                name: "days",
                schema: "mentorships");

            migrationBuilder.DropIndex(
                name: "ix_availabilities_day_id",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropColumn(
                name: "day_id",
                schema: "mentorships",
                table: "availabilities");
        }
    }
}
