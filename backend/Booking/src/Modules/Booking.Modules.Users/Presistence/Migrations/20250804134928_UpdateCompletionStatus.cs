using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompletionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_email",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_gender",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "profile_completion_status_has_name",
                schema: "users",
                table: "AspNetUsers",
                newName: "profile_completion_status_has_experience");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "profile_completion_status_has_experience",
                schema: "users",
                table: "AspNetUsers",
                newName: "profile_completion_status_has_name");

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_email",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_gender",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
