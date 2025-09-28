using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedMinuteToTimeRange : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_range_end_minute",
                schema: "mentorships",
                table: "availabilities");

            migrationBuilder.DropColumn(
                name: "time_range_start_minute",
                schema: "mentorships",
                table: "availabilities");
        }
    }
}
