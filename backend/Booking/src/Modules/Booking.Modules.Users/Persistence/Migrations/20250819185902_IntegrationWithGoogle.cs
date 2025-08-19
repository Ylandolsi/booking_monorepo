using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IntegrationWithGoogle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "integrated_with_google",
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
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9673), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9285) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9803), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9798) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9805), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9805) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9806), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9806) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9807), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9807) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9812), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9811) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9813), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9813) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9814), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9814) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9815), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9815) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9817), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9817) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9818), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9818) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9819), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9819) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9820), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9820) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9821), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9821) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9822), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9822) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9823), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9823) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9824), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9824) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9826), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9826) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9827), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9827) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9828), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9828) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9829), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9829) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9831), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9830) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9832), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9831) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9833), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9832) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9834), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9833) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9835), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9834) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9836), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9835) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9837), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9837) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9838), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9838) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9839), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9839) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9840), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9840) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9841), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9841) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9842), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9842) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9844), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9844) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9845), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9845) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9846), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9846) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9847), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9847) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9848), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9848) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9849), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9849) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9851), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9850) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9852), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9851) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9853), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9852) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9855), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9853) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9856), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9855) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9857), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9857) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9859), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9858) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9860), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9859) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9861), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9860) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9862), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9861) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9863), new DateTime(2025, 8, 19, 18, 59, 2, 332, DateTimeKind.Utc).AddTicks(9862) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 333, DateTimeKind.Utc).AddTicks(4588), new DateTime(2025, 8, 19, 18, 59, 2, 333, DateTimeKind.Utc).AddTicks(4163) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 333, DateTimeKind.Utc).AddTicks(4603), new DateTime(2025, 8, 19, 18, 59, 2, 333, DateTimeKind.Utc).AddTicks(4602) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 19, 18, 59, 2, 333, DateTimeKind.Utc).AddTicks(4604), new DateTime(2025, 8, 19, 18, 59, 2, 333, DateTimeKind.Utc).AddTicks(4604) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "integrated_with_google",
                schema: "users",
                table: "AspNetUsers");

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
    }
}
