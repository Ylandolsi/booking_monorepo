using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TimeZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "timezone_id",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timezone_id",
                schema: "users",
                table: "AspNetUsers");

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
    }
}
