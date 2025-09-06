using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypeInTimeZoneId : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timezone_id",
                schema: "mentorships",
                table: "mentors",
                newName: "time_zone_id");

            migrationBuilder.RenameColumn(
                name: "timezone_id",
                schema: "mentorships",
                table: "availabilities",
                newName: "time_zone_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time_zone_id",
                schema: "mentorships",
                table: "mentors",
                newName: "timezone_id");

            migrationBuilder.RenameColumn(
                name: "time_zone_id",
                schema: "mentorships",
                table: "availabilities",
                newName: "timezone_id");
        }
    }
}
