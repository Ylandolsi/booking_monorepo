using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "users",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "users",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "users",
                table: "languages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "users",
                table: "languages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "users",
                table: "expertises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "users",
                table: "expertises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "users",
                table: "experiences",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "users",
                table: "experiences",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "users",
                table: "email_verification_token",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "users",
                table: "email_verification_token",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "users",
                table: "educations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "users",
                table: "educations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "users",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "users",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(7747), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(6853) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8049), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8047) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8051), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8051) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8053), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8053) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8055), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8054) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8061), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8060) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8062), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8062) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8064), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8064) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8066), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8065) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8069), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8068) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8070), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8070) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8072), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8071) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8074), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8073) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8092), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8092) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8094), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8094) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8096), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8095) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8097), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8097) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8100), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8100) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8102), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8102) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8104), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8103) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8105), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8105) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8107), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8107) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8109), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8108) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8110), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8110) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8112), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8112) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8114), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8113) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8116), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8115) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8117), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8117) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8119), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8118) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8121), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8120) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8122), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8122) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8124), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8123) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8126), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8125) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8129), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8128) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8131), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8130) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8132), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8132) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8134), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8134) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8136), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8135) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8137), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8137) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8139), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8139) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8141), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8140) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8142), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8142) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8144), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8144) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8146), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8145) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8147), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8147) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8149), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8148) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8151), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8150) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8152), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8152) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8154), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8154) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8156), new DateTime(2025, 8, 10, 12, 18, 40, 902, DateTimeKind.Utc).AddTicks(8155) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 903, DateTimeKind.Utc).AddTicks(6981), new DateTime(2025, 8, 10, 12, 18, 40, 903, DateTimeKind.Utc).AddTicks(6345) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 903, DateTimeKind.Utc).AddTicks(7005), new DateTime(2025, 8, 10, 12, 18, 40, 903, DateTimeKind.Utc).AddTicks(7004) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 10, 12, 18, 40, 903, DateTimeKind.Utc).AddTicks(7007), new DateTime(2025, 8, 10, 12, 18, 40, 903, DateTimeKind.Utc).AddTicks(7007) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "users",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "users",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "users",
                table: "languages");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "users",
                table: "languages");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "users",
                table: "expertises");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "users",
                table: "expertises");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "users",
                table: "experiences");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "users",
                table: "experiences");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "users",
                table: "email_verification_token");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "users",
                table: "email_verification_token");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "users",
                table: "educations");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "users",
                table: "educations");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "users",
                table: "AspNetUsers");
        }
    }
}
