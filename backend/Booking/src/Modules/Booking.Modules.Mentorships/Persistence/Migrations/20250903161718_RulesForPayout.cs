using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RulesForPayout : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_payouts_status",
                schema: "mentorships",
                table: "payouts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_payouts_user_id",
                schema: "mentorships",
                table: "payouts",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_payouts_status",
                schema: "mentorships",
                table: "payouts");

            migrationBuilder.DropIndex(
                name: "ix_payouts_user_id",
                schema: "mentorships",
                table: "payouts");
        }
    }
}
