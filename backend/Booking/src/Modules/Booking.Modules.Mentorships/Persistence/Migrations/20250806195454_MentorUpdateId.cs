using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MentorUpdateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_mentors_user_id",
                schema: "mentorships",
                table: "mentors");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "mentorships",
                table: "mentors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                schema: "mentorships",
                table: "mentors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_mentors_user_id",
                schema: "mentorships",
                table: "mentors",
                column: "user_id",
                unique: true);
        }
    }
}
