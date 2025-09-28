using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PendingBalance : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "pending_balance",
                schema: "mentorships",
                table: "wallets",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pending_balance",
                schema: "mentorships",
                table: "wallets");
        }
    }
}
