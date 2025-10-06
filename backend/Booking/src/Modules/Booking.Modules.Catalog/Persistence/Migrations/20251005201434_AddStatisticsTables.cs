using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStatisticsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_daily_stats",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    product_slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    store_slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    sales_count = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_daily_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_daily_stats_products_product_id",
                        column: x => x.product_id,
                        principalSchema: "catalog",
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_daily_stats_stores_store_id",
                        column: x => x.store_id,
                        principalSchema: "catalog",
                        principalTable: "stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "store_daily_stats",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    store_slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    sales_count = table.Column<int>(type: "integer", nullable: false),
                    visitors = table.Column<int>(type: "integer", nullable: false),
                    unique_customers = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_store_daily_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_store_daily_stats_stores_store_id",
                        column: x => x.store_id,
                        principalSchema: "catalog",
                        principalTable: "stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_daily_stats_date",
                schema: "catalog",
                table: "product_daily_stats",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "ix_product_daily_stats_product_id_date",
                schema: "catalog",
                table: "product_daily_stats",
                columns: new[] { "product_id", "date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_daily_stats_store_id",
                schema: "catalog",
                table: "product_daily_stats",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_store_daily_stats_date",
                schema: "catalog",
                table: "store_daily_stats",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "ix_store_daily_stats_store_id_date",
                schema: "catalog",
                table: "store_daily_stats",
                columns: new[] { "store_id", "date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_daily_stats",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "store_daily_stats",
                schema: "catalog");
        }
    }
}
