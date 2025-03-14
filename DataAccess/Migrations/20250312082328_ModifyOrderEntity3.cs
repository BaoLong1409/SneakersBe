using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyOrderEntity3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Payment_PaymentId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Shipping_ShippingId",
                table: "Order");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShippingId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: new Guid("f47e3b92-5c1d-4e06-9ea2-8b1d77f8c123"),
                column: "Name",
                value: "VNPay");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Payment_PaymentId",
                table: "Order",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Shipping_ShippingId",
                table: "Order",
                column: "ShippingId",
                principalTable: "Shipping",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Payment_PaymentId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Shipping_ShippingId",
                table: "Order");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShippingId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: new Guid("f47e3b92-5c1d-4e06-9ea2-8b1d77f8c123"),
                column: "Name",
                value: "E-Wallets");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Payment_PaymentId",
                table: "Order",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Shipping_ShippingId",
                table: "Order",
                column: "ShippingId",
                principalTable: "Shipping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
