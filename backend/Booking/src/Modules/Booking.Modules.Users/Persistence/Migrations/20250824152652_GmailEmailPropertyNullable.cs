using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GmailEmailPropertyNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "google_email",
                schema: "users",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "google_email",
                schema: "users",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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
    }
}
