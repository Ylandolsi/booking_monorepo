using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "users",
                table: "refresh_tokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "users",
                table: "email_verification_token",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "users",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "users",
                table: "email_verification_token");
        }
    }
}
