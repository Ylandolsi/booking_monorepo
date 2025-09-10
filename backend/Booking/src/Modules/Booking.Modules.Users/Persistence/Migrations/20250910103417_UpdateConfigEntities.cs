using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConfigEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "token",
                schema: "users",
                table: "refresh_tokens",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "profile_picture_url_thumbnail_url_picture_link",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "profile_picture_url_profile_picture_link",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(2840), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(1999) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3171), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3169) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3173), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3173) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3175), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3175) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3177), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3176) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3187), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3186) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3188), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3188) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3190), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3189) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3191), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3191) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3194), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3194) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3196), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3195) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3197), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3197) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3199), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3198) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3200), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3200) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3202), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3201) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3203), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3203) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3205), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3205) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3208), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3207) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3209), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3209) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3211), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3210) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3212), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3212) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3214), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3213) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3215), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3215) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3217), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3217) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3219), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3218) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3220), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3220) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3222), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3221) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3223), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3223) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3225), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3224) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3227), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3226) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3228), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3228) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3230), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3229) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3232), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3232) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3235), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3235) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3237), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3236) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3239), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3238) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3240), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3240) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3242), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3241) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3243), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3243) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3245), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3244) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3246), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3246) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3248), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3248) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3250), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3249) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3251), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3251) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3274), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3274) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3276), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3275) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3277), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3277) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3279), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3278) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3280), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3280) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3282), new DateTime(2025, 9, 10, 10, 34, 16, 333, DateTimeKind.Utc).AddTicks(3282) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 334, DateTimeKind.Utc).AddTicks(2500), new DateTime(2025, 9, 10, 10, 34, 16, 334, DateTimeKind.Utc).AddTicks(1908) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 334, DateTimeKind.Utc).AddTicks(2527), new DateTime(2025, 9, 10, 10, 34, 16, 334, DateTimeKind.Utc).AddTicks(2525) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 34, 16, 334, DateTimeKind.Utc).AddTicks(2529), new DateTime(2025, 9, 10, 10, 34, 16, 334, DateTimeKind.Utc).AddTicks(2528) });

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_expires_on_utc",
                schema: "users",
                table: "refresh_tokens",
                column: "expires_on_utc");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_revoked_on_utc",
                schema: "users",
                table: "refresh_tokens",
                column: "revoked_on_utc");

            migrationBuilder.AddCheckConstraint(
                name: "CK_RefreshToken_Dates_Valid",
                schema: "users",
                table: "refresh_tokens",
                sql: "expires_on_utc > created_on_utc");

            migrationBuilder.AddCheckConstraint(
                name: "CK_RefreshToken_Revoked_Valid",
                schema: "users",
                table: "refresh_tokens",
                sql: "revoked_on_utc IS NULL OR revoked_on_utc >= created_on_utc");

            migrationBuilder.AddCheckConstraint(
                name: "CK_RefreshToken_Token_Length",
                schema: "users",
                table: "refresh_tokens",
                sql: "LENGTH(token) >= 32 AND LENGTH(token) <= 512");

            migrationBuilder.CreateIndex(
                name: "ix_experiences_start_date",
                schema: "users",
                table: "experiences",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "ix_experiences_user_id_start_date",
                schema: "users",
                table: "experiences",
                columns: new[] { "user_id", "start_date" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Experience_Company_Length",
                schema: "users",
                table: "experiences",
                sql: "LENGTH(company) >= 2 AND LENGTH(company) <= 100");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Experience_Dates_Valid",
                schema: "users",
                table: "experiences",
                sql: "end_date IS NULL OR end_date >= start_date");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Experience_Title_Length",
                schema: "users",
                table: "experiences",
                sql: "LENGTH(title) >= 2 AND LENGTH(title) <= 100");

            migrationBuilder.CreateIndex(
                name: "ix_email_verification_token_expires_on_utc",
                schema: "users",
                table: "email_verification_token",
                column: "expires_on_utc");

            migrationBuilder.AddCheckConstraint(
                name: "CK_EmailVerificationToken_Dates_Valid",
                schema: "users",
                table: "email_verification_token",
                sql: "expires_on_utc > created_on_utc");

            migrationBuilder.CreateIndex(
                name: "ix_educations_start_date",
                schema: "users",
                table: "educations",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "ix_educations_user_id_start_date",
                schema: "users",
                table: "educations",
                columns: new[] { "user_id", "start_date" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Education_Dates_Valid",
                schema: "users",
                table: "educations",
                sql: "end_date IS NULL OR end_date >= start_date");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Education_Field_Length",
                schema: "users",
                table: "educations",
                sql: "LENGTH(field) >= 2 AND LENGTH(field) <= 100");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Education_University_Length",
                schema: "users",
                table: "educations",
                sql: "LENGTH(university) >= 2 AND LENGTH(university) <= 100");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_created_at",
                schema: "users",
                table: "AspNetUsers",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_email",
                schema: "users",
                table: "AspNetUsers",
                column: "email",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Bio_Length",
                schema: "users",
                table: "AspNetUsers",
                sql: "LENGTH(bio) <= 500");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Gender_Valid",
                schema: "users",
                table: "AspNetUsers",
                sql: "gender IS NULL OR gender IN ('Male', 'Female', 'Other', 'Prefer not to say')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_refresh_tokens_expires_on_utc",
                schema: "users",
                table: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "ix_refresh_tokens_revoked_on_utc",
                schema: "users",
                table: "refresh_tokens");

            migrationBuilder.DropCheckConstraint(
                name: "CK_RefreshToken_Dates_Valid",
                schema: "users",
                table: "refresh_tokens");

            migrationBuilder.DropCheckConstraint(
                name: "CK_RefreshToken_Revoked_Valid",
                schema: "users",
                table: "refresh_tokens");

            migrationBuilder.DropCheckConstraint(
                name: "CK_RefreshToken_Token_Length",
                schema: "users",
                table: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "ix_experiences_start_date",
                schema: "users",
                table: "experiences");

            migrationBuilder.DropIndex(
                name: "ix_experiences_user_id_start_date",
                schema: "users",
                table: "experiences");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Experience_Company_Length",
                schema: "users",
                table: "experiences");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Experience_Dates_Valid",
                schema: "users",
                table: "experiences");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Experience_Title_Length",
                schema: "users",
                table: "experiences");

            migrationBuilder.DropIndex(
                name: "ix_email_verification_token_expires_on_utc",
                schema: "users",
                table: "email_verification_token");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EmailVerificationToken_Dates_Valid",
                schema: "users",
                table: "email_verification_token");

            migrationBuilder.DropIndex(
                name: "ix_educations_start_date",
                schema: "users",
                table: "educations");

            migrationBuilder.DropIndex(
                name: "ix_educations_user_id_start_date",
                schema: "users",
                table: "educations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Education_Dates_Valid",
                schema: "users",
                table: "educations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Education_Field_Length",
                schema: "users",
                table: "educations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Education_University_Length",
                schema: "users",
                table: "educations");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_created_at",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_email",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Bio_Length",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Gender_Valid",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                schema: "users",
                table: "refresh_tokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                schema: "users",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "profile_picture_url_thumbnail_url_picture_link",
                schema: "users",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "profile_picture_url_profile_picture_link",
                schema: "users",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6574), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(5243) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6973), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6970) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6976), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6975) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6978), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6978) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6981), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6980) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6990), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6989) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6992), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(6992) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7019), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7018) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7022), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7021) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7026), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7025) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7028), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7028) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7031), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7030) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7033), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7032) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7035), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7034) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7037), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7036) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7039), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7038) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7041), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7040) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7045), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7044) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7047), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7046) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7049), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7048) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7051), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7050) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7053), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7052) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7055), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7054) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7057), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7056) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7059), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7058) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7061), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7060) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7063), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7062) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7065), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7064) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7067), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7066) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7069), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7068) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7071), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7070) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7073), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7072) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7075), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7075) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7079), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7078) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7081), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7080) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7083), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7082) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7085), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7084) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7087), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7086) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7089), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7088) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7091), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7090) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7093), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7093) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7095), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7095) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7097), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7097) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7099), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7099) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7101), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7101) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7104), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7103) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7106), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7105) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7108), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7107) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7110), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7110) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7113), new DateTime(2025, 9, 6, 9, 30, 24, 843, DateTimeKind.Utc).AddTicks(7112) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 844, DateTimeKind.Utc).AddTicks(6373), new DateTime(2025, 9, 6, 9, 30, 24, 844, DateTimeKind.Utc).AddTicks(5759) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 844, DateTimeKind.Utc).AddTicks(6397), new DateTime(2025, 9, 6, 9, 30, 24, 844, DateTimeKind.Utc).AddTicks(6395) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 6, 9, 30, 24, 844, DateTimeKind.Utc).AddTicks(6399), new DateTime(2025, 9, 6, 9, 30, 24, 844, DateTimeKind.Utc).AddTicks(6398) });
        }
    }
}
