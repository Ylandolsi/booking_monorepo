using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorMentorshipModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_hour",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropColumn(
                name: "start_hour",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropColumn(
                name: "time_range_end_minute",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropColumn(
                name: "time_range_start_minute",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "end_time",
                schema: "mentorships",
                table: "availabilities",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "start_time",
                schema: "mentorships",
                table: "availabilities",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "timezone_id",
                schema: "mentorships",
                table: "availabilities",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_time",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropColumn(
                name: "start_time",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropColumn(
                name: "timezone_id",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.AddColumn<int>(
                name: "end_hour",
                schema: "mentorships",
                table: "availabilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "start_hour",
                schema: "mentorships",
                table: "availabilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "time_range_end_minute",
                schema: "mentorships",
                table: "availabilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "time_range_start_minute",
                schema: "mentorships",
                table: "availabilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
