using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsEfCore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orders_product_product_id",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "fk_product_stores_store_id",
                schema: "catalog",
                table: "product");

            migrationBuilder.DropForeignKey(
                name: "fk_session_products_product_id",
                schema: "catalog",
                table: "session_products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product",
                schema: "catalog",
                table: "product");

            migrationBuilder.RenameTable(
                name: "product",
                schema: "catalog",
                newName: "products",
                newSchema: "catalog");

            migrationBuilder.RenameIndex(
                name: "ix_product_store_id",
                schema: "catalog",
                table: "products",
                newName: "ix_products_store_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                schema: "catalog",
                table: "products",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_orders_products_product_id",
                schema: "catalog",
                table: "orders",
                column: "product_id",
                principalSchema: "catalog",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_products_stores_store_id",
                schema: "catalog",
                table: "products",
                column: "store_id",
                principalSchema: "catalog",
                principalTable: "stores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_session_products_products_id",
                schema: "catalog",
                table: "session_products",
                column: "id",
                principalSchema: "catalog",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orders_products_product_id",
                schema: "catalog",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "fk_products_stores_store_id",
                schema: "catalog",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_session_products_products_id",
                schema: "catalog",
                table: "session_products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                schema: "catalog",
                table: "products");

            migrationBuilder.RenameTable(
                name: "products",
                schema: "catalog",
                newName: "product",
                newSchema: "catalog");

            migrationBuilder.RenameIndex(
                name: "ix_products_store_id",
                schema: "catalog",
                table: "product",
                newName: "ix_product_store_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product",
                schema: "catalog",
                table: "product",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_orders_product_product_id",
                schema: "catalog",
                table: "orders",
                column: "product_id",
                principalSchema: "catalog",
                principalTable: "product",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_product_stores_store_id",
                schema: "catalog",
                table: "product",
                column: "store_id",
                principalSchema: "catalog",
                principalTable: "stores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_session_products_product_id",
                schema: "catalog",
                table: "session_products",
                column: "id",
                principalSchema: "catalog",
                principalTable: "product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
