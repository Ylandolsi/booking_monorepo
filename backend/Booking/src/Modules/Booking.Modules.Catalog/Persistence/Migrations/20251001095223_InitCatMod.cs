using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitCatMod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "booked_sessions",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    product_slug = table.Column<string>(type: "text", nullable: false),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    store_slug = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    amount_paid = table.Column<decimal>(type: "numeric", nullable: false),
                    note = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    meet_link = table.Column<string>(type: "text", nullable: true),
                    scheduled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ends_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_booked_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    reference = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
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
                name: "payouts",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    konnect_wallet_id = table.Column<string>(type: "text", nullable: false),
                    wallet_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    payment_ref = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payouts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stores",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    picture_main_link = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    picture_thumbnail_link = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    step = table.Column<int>(type: "integer", nullable: false),
                    social_links = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wallets",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false),
                    pending_balance = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wallets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_slug = table.Column<string>(type: "text", nullable: false),
                    store_slug = table.Column<string>(type: "text", nullable: false),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    subtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    product_type = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    click_to_pay = table.Column<string>(type: "text", nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    thumbnail_picture_main_link = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    thumbnail_picture_thumbnail_link = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    preview_picture_main_link = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    preview_picture_thumbnail_link = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_stores_store_id",
                        column: x => x.store_id,
                        principalSchema: "catalog",
                        principalTable: "stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    store_slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    customer_email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    customer_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    customer_phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    product_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    amount_paid = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_ref = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    payment_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    scheduled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    session_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    time_zone_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "catalog",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_orders_stores_store_id",
                        column: x => x.store_id,
                        principalSchema: "catalog",
                        principalTable: "stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "session_products",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    duration_minutes = table.Column<int>(type: "integer", nullable: false),
                    buffer_time_minutes = table.Column<int>(type: "integer", nullable: false),
                    meeting_instructions = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    time_zone_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Africa/Tunis")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_session_products_product_id",
                        column: x => x.id,
                        principalSchema: "catalog",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "escrows",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    release_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_escrows", x => x.id);
                    table.ForeignKey(
                        name: "fk_escrows_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "catalog",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "days",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    product_slug = table.Column<string>(type: "text", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    session_product_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_days", x => x.id);
                    table.ForeignKey(
                        name: "fk_days_session_products_session_product_id",
                        column: x => x.session_product_id,
                        principalSchema: "catalog",
                        principalTable: "session_products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "session_availabilities",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_product_slug = table.Column<string>(type: "text", nullable: false),
                    session_product_id = table.Column<int>(type: "integer", nullable: false),
                    day_id = table.Column<int>(type: "integer", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    time_zone_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_session_availabilities", x => x.id);
                    table.ForeignKey(
                        name: "fk_session_availabilities_days_day_id",
                        column: x => x.day_id,
                        principalSchema: "catalog",
                        principalTable: "days",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_session_availabilities_session_products_session_product_id",
                        column: x => x.session_product_id,
                        principalSchema: "catalog",
                        principalTable: "session_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_days_day_of_week",
                schema: "catalog",
                table: "days",
                column: "day_of_week");

            migrationBuilder.CreateIndex(
                name: "ix_days_is_active",
                schema: "catalog",
                table: "days",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_days_product_id_day_of_week",
                schema: "catalog",
                table: "days",
                columns: new[] { "product_id", "day_of_week" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_days_session_product_id",
                schema: "catalog",
                table: "days",
                column: "session_product_id");

            migrationBuilder.CreateIndex(
                name: "ix_escrows_order_id",
                schema: "catalog",
                table: "escrows",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_escrows_state",
                schema: "catalog",
                table: "escrows",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "ix_orders_created_at",
                schema: "catalog",
                table: "orders",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_orders_customer_email",
                schema: "catalog",
                table: "orders",
                column: "customer_email");

            migrationBuilder.CreateIndex(
                name: "ix_orders_payment_ref",
                schema: "catalog",
                table: "orders",
                column: "payment_ref",
                unique: true,
                filter: "payment_ref IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_orders_product_id",
                schema: "catalog",
                table: "orders",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_scheduled_at",
                schema: "catalog",
                table: "orders",
                column: "scheduled_at",
                filter: "scheduled_at IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_orders_status",
                schema: "catalog",
                table: "orders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_orders_store_id",
                schema: "catalog",
                table: "orders",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_reference",
                schema: "catalog",
                table: "payments",
                column: "reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_payouts_status",
                schema: "catalog",
                table: "payouts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_payouts_user_id",
                schema: "catalog",
                table: "payouts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_is_published",
                schema: "catalog",
                table: "product",
                column: "is_published");

            migrationBuilder.CreateIndex(
                name: "ix_product_store_id",
                schema: "catalog",
                table: "product",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_store_id_display_order",
                schema: "catalog",
                table: "product",
                columns: new[] { "store_id", "display_order" });

            migrationBuilder.CreateIndex(
                name: "ix_session_availabilities_day_id",
                schema: "catalog",
                table: "session_availabilities",
                column: "day_id");

            migrationBuilder.CreateIndex(
                name: "ix_session_availability_product_day_active",
                schema: "catalog",
                table: "session_availabilities",
                columns: new[] { "session_product_id", "day_of_week", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_session_availability_session_product_id",
                schema: "catalog",
                table: "session_availabilities",
                column: "session_product_id");

            migrationBuilder.CreateIndex(
                name: "ix_session_products_time_zone",
                schema: "catalog",
                table: "session_products",
                column: "time_zone_id");

            migrationBuilder.CreateIndex(
                name: "ix_stores_is_published",
                schema: "catalog",
                table: "stores",
                column: "is_published");

            migrationBuilder.CreateIndex(
                name: "ix_stores_slug",
                schema: "catalog",
                table: "stores",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booked_sessions",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "escrows",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "payouts",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "session_availabilities",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "wallets",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "days",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "session_products",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "product",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "stores",
                schema: "catalog");
        }
    }
}
