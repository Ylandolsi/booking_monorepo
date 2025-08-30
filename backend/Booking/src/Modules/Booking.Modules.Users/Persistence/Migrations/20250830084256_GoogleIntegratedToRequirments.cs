using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GoogleIntegratedToRequirments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_connected_with_google",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(319), new DateTime(2025, 8, 30, 8, 42, 55, 198, DateTimeKind.Utc).AddTicks(9348) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(627), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(625) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(630), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(629) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(631), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(631) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(633), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(632) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(642), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(641) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(644), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(643) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(645), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(645) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(647), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(646) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(680), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(680) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(682), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(682) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(684), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(684) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(686), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(685) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(687), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(687) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(689), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(689) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(691), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(690) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(693), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(692) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(696), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(695) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(697), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(697) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(699), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(699) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(701), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(700) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(703), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(702) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(704), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(704) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(706), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(705) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(708), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(707) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(709), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(709) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(711), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(710) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(713), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(712) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(714), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(714) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(716), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(715) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(718), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(717) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(719), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(719) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(721), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(720) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(724), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(724) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(726), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(725) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(728), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(727) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(730), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(729) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(731), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(731) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(733), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(732) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(735), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(734) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(736), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(736) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(738), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(737) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(740), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(739) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(741), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(741) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(743), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(742) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(745), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(744) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(746), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(746) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(748), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(747) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(749), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(749) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(751), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(751) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(9448), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(8866) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(9472), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(9470) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(9474), new DateTime(2025, 8, 30, 8, 42, 55, 199, DateTimeKind.Utc).AddTicks(9473) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_connected_with_google",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7384), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(6211) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7717), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7714) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7719), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7718) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7721), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7721) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7723), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7723) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7732), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7731) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7734), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7733) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7736), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7735) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7738), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7737) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7741), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7741) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7743), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7743) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7769), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7768) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7771), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7771) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7774), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7773) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7776), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7775) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7778), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7777) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7780), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7779) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7784), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7783) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7786), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7785) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7788), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7787) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7790), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7789) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7792), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7791) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7794), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7793) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7796), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7795) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7798), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7797) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7800), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7799) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7802), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7801) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7804), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7803) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7806), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7805) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7808), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7807) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7810), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7810) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7812), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7812) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7814), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7814) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7818), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7817) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7820), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7819) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7822), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7821) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7824), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7823) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7826), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7826) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7828), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7828) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7830), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7830) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7833), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7832) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7835), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7834) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7837), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7836) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7839), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7838) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7841), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7840) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7843), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7842) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7845), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7844) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7847), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7846) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7849), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7848) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7851), new DateTime(2025, 8, 24, 15, 26, 51, 961, DateTimeKind.Utc).AddTicks(7851) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 962, DateTimeKind.Utc).AddTicks(7228), new DateTime(2025, 8, 24, 15, 26, 51, 962, DateTimeKind.Utc).AddTicks(6602) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 962, DateTimeKind.Utc).AddTicks(7254), new DateTime(2025, 8, 24, 15, 26, 51, 962, DateTimeKind.Utc).AddTicks(7252) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 26, 51, 962, DateTimeKind.Utc).AddTicks(7256), new DateTime(2025, 8, 24, 15, 26, 51, 962, DateTimeKind.Utc).AddTicks(7255) });
        }
    }
}
