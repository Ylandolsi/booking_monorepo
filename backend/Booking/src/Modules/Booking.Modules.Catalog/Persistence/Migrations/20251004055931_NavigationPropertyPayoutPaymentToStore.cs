using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Modules.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NavigationPropertyPayoutPaymentToStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "catalog",
                table: "payouts",
                newName: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_payouts_store_id",
                schema: "catalog",
                table: "payouts",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_reference",
                schema: "catalog",
                table: "payments",
                column: "reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_payments_store_id",
                schema: "catalog",
                table: "payments",
                column: "store_id");

            migrationBuilder.AddForeignKey(
                name: "fk_payments_stores_store_id",
                schema: "catalog",
                table: "payments",
                column: "store_id",
                principalSchema: "catalog",
                principalTable: "stores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_payouts_stores_store_id",
                schema: "catalog",
                table: "payouts",
                column: "store_id",
                principalSchema: "catalog",
                principalTable: "stores",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_payments_stores_store_id",
                schema: "catalog",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "fk_payouts_stores_store_id",
                schema: "catalog",
                table: "payouts");

            migrationBuilder.DropIndex(
                name: "ix_payouts_store_id",
                schema: "catalog",
                table: "payouts");

            migrationBuilder.DropIndex(
                name: "ix_payments_reference",
                schema: "catalog",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "ix_payments_store_id",
                schema: "catalog",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "store_id",
                schema: "catalog",
                table: "payouts",
                newName: "user_id");
        }
    }
}
