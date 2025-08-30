using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveKonnectWallerIdFromMentorshipDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "konnect_wallet_id",
                schema: "mentorships",
                table: "mentors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "konnect_wallet_id",
                schema: "mentorships",
                table: "mentors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
