using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Modules.Mentorships.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WalletPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "amount_paid",
                schema: "mentorships",
                table: "sessions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "escrows",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    session_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_escrows", x => x.id);
                    table.ForeignKey(
                        name: "fk_escrows_sessions_session_id",
                        column: x => x.session_id,
                        principalSchema: "mentorships",
                        principalTable: "sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reference = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    mentor_id = table.Column<int>(type: "integer", nullable: false),
                    session_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    escrow_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wallets",
                schema: "mentorships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wallets", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_escrows_session_id",
                schema: "mentorships",
                table: "escrows",
                column: "session_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_escrows_state",
                schema: "mentorships",
                table: "escrows",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "ix_payments_reference",
                schema: "mentorships",
                table: "payments",
                column: "reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_payments_session_id",
                schema: "mentorships",
                table: "payments",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_status",
                schema: "mentorships",
                table: "payments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_payments_user_id",
                schema: "mentorships",
                table: "payments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_escrow_id",
                schema: "mentorships",
                table: "transactions",
                column: "escrow_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_status",
                schema: "mentorships",
                table: "transactions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_user_id",
                schema: "mentorships",
                table: "transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_wallets_user_id",
                schema: "mentorships",
                table: "wallets",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "escrows",
                schema: "mentorships");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "mentorships");

            migrationBuilder.DropTable(
                name: "transactions",
                schema: "mentorships");

            migrationBuilder.DropTable(
                name: "wallets",
                schema: "mentorships");

            migrationBuilder.DropColumn(
                name: "amount_paid",
                schema: "mentorships",
                table: "sessions");
        }
    }
}
