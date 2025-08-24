using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GmailEmailProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "google_email",
                schema: "users",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5235), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(4267) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5563), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5560) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5565), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5564) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5567), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5566) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5569), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5569) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5577), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5576) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5579), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5579) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5581), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5581) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5584), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5583) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5587), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5586) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5589), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5588) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5591), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5590) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5593), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5592) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5595), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5594) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5597), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5596) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5599), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5598) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5601), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5600) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5605), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5604) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5607), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5606) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5609), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5608) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5611), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5610) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5613), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5612) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5615), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5614) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5617), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5616) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5619), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5618) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5621), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5620) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5623), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5622) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5625), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5624) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5627), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5626) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5629), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5628) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5631), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5630) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5633), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5632) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5635), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5634) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5639), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5638) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5641), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5640) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5643), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5642) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5645), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5644) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5647), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5646) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5649), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5648) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5651), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5650) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5653), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5652) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5655), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5654) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5657), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5656) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5659), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5658) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5661), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5660) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5663), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5662) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5665), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5664) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5667), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5666) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5669), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5668) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5671), new DateTime(2025, 8, 24, 15, 24, 16, 684, DateTimeKind.Utc).AddTicks(5670) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 685, DateTimeKind.Utc).AddTicks(5753), new DateTime(2025, 8, 24, 15, 24, 16, 685, DateTimeKind.Utc).AddTicks(5057) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 685, DateTimeKind.Utc).AddTicks(5779), new DateTime(2025, 8, 24, 15, 24, 16, 685, DateTimeKind.Utc).AddTicks(5777) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 24, 15, 24, 16, 685, DateTimeKind.Utc).AddTicks(5781), new DateTime(2025, 8, 24, 15, 24, 16, 685, DateTimeKind.Utc).AddTicks(5780) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "google_email",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(2747), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(1843) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3067), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3065) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3069), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3069) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3071), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3071) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3073), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3072) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3080), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3079) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3081), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3081) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3083), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3082) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3085), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3084) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3087), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3087) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3089), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3089) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3091), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3090) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3093), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3092) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3094), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3094) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3096), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3095) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3097), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3097) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3099), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3099) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3102), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3101) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3104), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3103) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3105), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3105) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3107), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3107) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3109), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3108) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3110), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3110) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3112), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3111) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3114), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3113) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3115), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3115) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3117), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3116) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3119), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3118) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3120), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3120) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3122), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3121) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3124), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3123) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3125), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3125) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3127), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3126) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3130), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3129) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3132), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3131) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3133), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3133) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3135), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3135) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3137), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3136) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3139), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3138) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3141), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3140) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3142), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3142) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3144), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3143) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3145), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3145) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3147), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3147) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3149), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3148) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3150), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3150) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3152), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3152) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3154), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3153) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3172), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3171) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3174), new DateTime(2025, 8, 23, 9, 1, 42, 461, DateTimeKind.Utc).AddTicks(3173) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 462, DateTimeKind.Utc).AddTicks(1968), new DateTime(2025, 8, 23, 9, 1, 42, 462, DateTimeKind.Utc).AddTicks(1317) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 462, DateTimeKind.Utc).AddTicks(1991), new DateTime(2025, 8, 23, 9, 1, 42, 462, DateTimeKind.Utc).AddTicks(1989) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 23, 9, 1, 42, 462, DateTimeKind.Utc).AddTicks(1993), new DateTime(2025, 8, 23, 9, 1, 42, 462, DateTimeKind.Utc).AddTicks(1993) });
        }
    }
}
