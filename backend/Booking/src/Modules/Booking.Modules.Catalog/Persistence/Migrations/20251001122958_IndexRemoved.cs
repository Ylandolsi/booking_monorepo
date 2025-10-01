using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IndexRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_stores_is_published",
                schema: "catalog",
                table: "stores");

            migrationBuilder.DropIndex(
                name: "ix_stores_slug",
                schema: "catalog",
                table: "stores");

            migrationBuilder.DropIndex(
                name: "ix_session_products_time_zone",
                schema: "catalog",
                table: "session_products");

            migrationBuilder.DropIndex(
                name: "ix_session_availability_product_day_active",
                schema: "catalog",
                table: "session_availabilities");

            migrationBuilder.DropIndex(
                name: "ix_product_is_published",
                schema: "catalog",
                table: "product");

            migrationBuilder.DropIndex(
                name: "ix_product_store_id_display_order",
                schema: "catalog",
                table: "product");

            migrationBuilder.DropIndex(
                name: "ix_payouts_status",
                schema: "catalog",
                table: "payouts");

            migrationBuilder.DropIndex(
                name: "ix_payouts_user_id",
                schema: "catalog",
                table: "payouts");

            migrationBuilder.DropIndex(
                name: "ix_payments_reference",
                schema: "catalog",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "ix_orders_created_at",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_customer_email",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_payment_ref",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_scheduled_at",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_status",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_escrows_state",
                schema: "catalog",
                table: "escrows");

            migrationBuilder.DropIndex(
                name: "ix_days_day_of_week",
                schema: "catalog",
                table: "days");

            migrationBuilder.DropIndex(
                name: "ix_days_is_active",
                schema: "catalog",
                table: "days");

            migrationBuilder.DropIndex(
                name: "ix_days_product_id_day_of_week",
                schema: "catalog",
                table: "days");

            migrationBuilder.RenameIndex(
                name: "ix_session_availability_session_product_id",
                schema: "catalog",
                table: "session_availabilities",
                newName: "ix_session_availabilities_session_product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_session_availabilities_session_product_id",
                schema: "catalog",
                table: "session_availabilities",
                newName: "ix_session_availability_session_product_id");

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

            migrationBuilder.CreateIndex(
                name: "ix_session_products_time_zone",
                schema: "catalog",
                table: "session_products",
                column: "time_zone_id");

            migrationBuilder.CreateIndex(
                name: "ix_session_availability_product_day_active",
                schema: "catalog",
                table: "session_availabilities",
                columns: new[] { "session_product_id", "day_of_week", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_product_is_published",
                schema: "catalog",
                table: "product",
                column: "is_published");

            migrationBuilder.CreateIndex(
                name: "ix_product_store_id_display_order",
                schema: "catalog",
                table: "product",
                columns: new[] { "store_id", "display_order" });

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
                name: "ix_payments_reference",
                schema: "catalog",
                table: "payments",
                column: "reference",
                unique: true);

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
                name: "ix_escrows_state",
                schema: "catalog",
                table: "escrows",
                column: "state");

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
        }
    }
}
