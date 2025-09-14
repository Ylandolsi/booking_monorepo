using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CatalogInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "BookedSessions",
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
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    amount_paid = table.Column<decimal>(type: "numeric", nullable: false),
                    note = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    MeetLink = table.Column<string>(type: "text", nullable: false),
                    scheduled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                name: "escrows",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    release_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_escrows", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    reference = table.Column<string>(type: "text", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
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
                    status = table.Column<int>(type: "integer", nullable: false),
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    step = table.Column<int>(type: "integer", nullable: false),
                    social_links = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stores", x => x.id);
                    table.CheckConstraint("CK_Store_Slug_Format", "\"slug\" ~ '^[a-z0-9-]+$'");
                    table.CheckConstraint("CK_Store_Slug_NotEmpty", "LENGTH(TRIM(\"slug\")) > 0");
                    table.CheckConstraint("CK_Store_Title_NotEmpty", "LENGTH(TRIM(\"title\")) > 0");
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
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    store_slug = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    click_to_pay = table.Column<string>(type: "text", nullable: false),
                    subtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    thumbnail_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    product_type = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                    table.CheckConstraint("CK_Product_DisplayOrder_NonNegative", "\"display_order\" >= 0");
                    table.CheckConstraint("CK_Product_Price_NonNegative", "\"price\" >= 0");
                    table.CheckConstraint("CK_Product_Title_NotEmpty", "LENGTH(TRIM(\"title\")) > 0");
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "SessionProducts",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    BufferTimeMinutes = table.Column<int>(type: "integer", nullable: false),
                    meeting_instructions = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    time_zone_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Africa/Tunis")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionProducts", x => x.id);
                    table.CheckConstraint("CK_SessionProduct_BufferTime_Valid", "\"BufferTimeMinutes\" >= 0 AND \"BufferTimeMinutes\" % 15 = 0");
                    table.CheckConstraint("CK_SessionProduct_Duration_Valid", "\"DurationMinutes\" > 0 AND \"DurationMinutes\" % 15 = 0");
                    table.ForeignKey(
                        name: "fk_session_products_product_id",
                        column: x => x.id,
                        principalSchema: "catalog",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    store_id = table.Column<int>(type: "integer", nullable: false),
                    store_slug = table.Column<string>(type: "text", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
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
                        principalTable: "SessionProducts",
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
                    table.CheckConstraint("CK_SessionAvailability_DayOfWeek", "\"day_of_week\" >= 0 AND \"day_of_week\" <= 6");
                    table.CheckConstraint("CK_SessionAvailability_TimeRange", "\"start_time\" < \"end_time\"");
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
                        principalTable: "SessionProducts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_days_session_product_id",
                schema: "catalog",
                table: "days",
                column: "session_product_id");

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
                name: "IX_Products_IsPublished",
                schema: "catalog",
                table: "product",
                column: "is_published");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Store_DisplayOrder",
                schema: "catalog",
                table: "product",
                columns: new[] { "store_id", "display_order" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                schema: "catalog",
                table: "product",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_session_availabilities_day_id",
                schema: "catalog",
                table: "session_availabilities",
                column: "day_id");

            migrationBuilder.CreateIndex(
                name: "IX_SessionAvailability_Product_Day_Active",
                schema: "catalog",
                table: "session_availabilities",
                columns: new[] { "session_product_id", "day_of_week", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_SessionAvailability_SessionProductId",
                schema: "catalog",
                table: "session_availabilities",
                column: "session_product_id");

            migrationBuilder.CreateIndex(
                name: "IX_SessionProducts_TimeZone",
                schema: "catalog",
                table: "SessionProducts",
                column: "time_zone_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_IsPublished",
                schema: "catalog",
                table: "stores",
                column: "is_published");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_Slug",
                schema: "catalog",
                table: "stores",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookedSessions",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "escrows",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "orders",
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
                name: "days",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "SessionProducts",
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
