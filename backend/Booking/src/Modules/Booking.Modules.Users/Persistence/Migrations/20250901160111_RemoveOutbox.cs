using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "users");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4498), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(3272) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4840), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4838) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4843), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4842) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4845), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4844) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4847), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4846) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4853), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4853) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4855), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4855) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4857), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4857) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4859), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4858) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4862), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4862) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4864), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4864) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4866), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4865) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4868), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4867) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4870), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4869) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4872), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4871) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4873), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4873) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4875), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4875) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4878), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4878) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4880), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4880) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4882), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4882) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4884), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4883) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4886), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4885) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4888), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4887) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4890), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4889) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4891), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4891) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4893), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4893) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4895), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4895) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4897), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4896) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4899), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4898) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4901), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4900) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4903), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4902) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4904), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4904) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4906), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4906) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4910), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4909) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4912), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4911) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4913), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4913) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4915), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4915) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4917), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4917) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4919), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4918) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4921), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4920) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4923), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4922) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4925), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4924) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4926), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4926) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4928), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4928) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4930), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4929) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4932), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4931) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4934), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4933) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4936), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4935) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4938), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4937) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "expertises",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4939), new DateTime(2025, 9, 1, 16, 1, 10, 388, DateTimeKind.Utc).AddTicks(4939) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 389, DateTimeKind.Utc).AddTicks(4543), new DateTime(2025, 9, 1, 16, 1, 10, 389, DateTimeKind.Utc).AddTicks(3866) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 389, DateTimeKind.Utc).AddTicks(4569), new DateTime(2025, 9, 1, 16, 1, 10, 389, DateTimeKind.Utc).AddTicks(4568) });

            migrationBuilder.UpdateData(
                schema: "users",
                table: "languages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 1, 16, 1, 10, 389, DateTimeKind.Utc).AddTicks(4571), new DateTime(2025, 9, 1, 16, 1, 10, 389, DateTimeKind.Utc).AddTicks(4570) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    error = table.Column<string>(type: "text", nullable: true),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

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
    }
}
