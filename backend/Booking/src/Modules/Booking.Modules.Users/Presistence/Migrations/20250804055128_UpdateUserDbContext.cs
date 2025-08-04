using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "profile_completion_status_completion_status",
                schema: "users",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "profile_completion_status_total_fields",
                schema: "users",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile_completion_status_completion_status",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_total_fields",
                schema: "users",
                table: "AspNetUsers");
        }
    }
}
