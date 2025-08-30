using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class KonnectWalletIdToUSersDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "konnect_walled_id",
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
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(6637), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(5338) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7054), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7051) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7056), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7056) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7059), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7058) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7061), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7060) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7069), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7068) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7071), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7070) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7073), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7072) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7075), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7074) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7079), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7078) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7081), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7080) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7083), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7082) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7085), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7084) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7087), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7086) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7089), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7088) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7091), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7090) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7093), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7092) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7097), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7096) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7099), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7098) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7101), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7100) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7103), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7102) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7105), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7104) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7107), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7106) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7109), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7108) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7111), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7110) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7113), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7112) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7115), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7114) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7117), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7117) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7119), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7119) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7121), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7121) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7174), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7173) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7176), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7176) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7179), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7178) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7183), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7182) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7185), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7184) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7187), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7186) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7189), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7188) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7191), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7190) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7193), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7192) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7195), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7194) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7197), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7196) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7199), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7198) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7201), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7200) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7203), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7202) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7205), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7205) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7207), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7207) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7209), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7209) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7211), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7211) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7214), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7213) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7216), new DateTime(2025, 8, 30, 15, 4, 6, 99, DateTimeKind.Utc).AddTicks(7215) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 100, DateTimeKind.Utc).AddTicks(6378), new DateTime(2025, 8, 30, 15, 4, 6, 100, DateTimeKind.Utc).AddTicks(5746) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 100, DateTimeKind.Utc).AddTicks(6402), new DateTime(2025, 8, 30, 15, 4, 6, 100, DateTimeKind.Utc).AddTicks(6400) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 30, 15, 4, 6, 100, DateTimeKind.Utc).AddTicks(6404), new DateTime(2025, 8, 30, 15, 4, 6, 100, DateTimeKind.Utc).AddTicks(6403) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "konnect_walled_id",
                schema: "users",
                table: "AspNetUsers");

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
    }
}
