using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyProductImageMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: new Guid("2a0b2079-1fa6-4757-8a49-12f30e849774"));

            migrationBuilder.DeleteData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: new Guid("eaf6a465-78c4-4268-b220-77a9ceed2584"));

            migrationBuilder.DeleteData(
                table: "Shipping",
                keyColumn: "Id",
                keyValue: new Guid("0f92c01a-5687-4296-ac95-0a4362dce096"));

            migrationBuilder.DeleteData(
                table: "Shipping",
                keyColumn: "Id",
                keyValue: new Guid("a026ad65-9fc2-4bf3-b053-01e0818a571a"));

            migrationBuilder.DeleteData(
                table: "Shipping",
                keyColumn: "Id",
                keyValue: new Guid("cd70cc8d-e687-4dc6-b674-b930bc12a427"));

            migrationBuilder.AddColumn<int>(
                name: "MaximumDeliverdTime",
                table: "Shipping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumDeliverdTime",
                table: "Shipping",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ColorId",
                table: "ProductImage",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("b3c1f7a4-92d5-4b19-a7e5-6d8b2a9f3d44"), "COD" },
                    { new Guid("f47e3b92-5c1d-4e06-9ea2-8b1d77f8c123"), "E-Wallets" }
                });

            migrationBuilder.InsertData(
                table: "Shipping",
                columns: new[] { "Id", "MaximumDeliverdTime", "MinimumDeliverdTime", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("7e2f4c38-8a5b-402a-b8d1-5e9fbc3d7a92"), 7, 5, "Standard", 0.61m },
                    { new Guid("c3b82e7d-1f92-4a50-a6b3-7d9e4f5c2a88"), -24, -12, "Ultra-Fast Delivery", 1.63m },
                    { new Guid("d8a9c347-6e5a-4b11-bf9d-2f4e9c1a7d55"), 5, 3, "Express", 0.90m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ColorId",
                table: "ProductImage",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Color_ColorId",
                table: "ProductImage",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Color_ColorId",
                table: "ProductImage");

            migrationBuilder.DropIndex(
                name: "IX_ProductImage_ColorId",
                table: "ProductImage");

            migrationBuilder.DeleteData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: new Guid("b3c1f7a4-92d5-4b19-a7e5-6d8b2a9f3d44"));

            migrationBuilder.DeleteData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: new Guid("f47e3b92-5c1d-4e06-9ea2-8b1d77f8c123"));

            migrationBuilder.DeleteData(
                table: "Shipping",
                keyColumn: "Id",
                keyValue: new Guid("7e2f4c38-8a5b-402a-b8d1-5e9fbc3d7a92"));

            migrationBuilder.DeleteData(
                table: "Shipping",
                keyColumn: "Id",
                keyValue: new Guid("c3b82e7d-1f92-4a50-a6b3-7d9e4f5c2a88"));

            migrationBuilder.DeleteData(
                table: "Shipping",
                keyColumn: "Id",
                keyValue: new Guid("d8a9c347-6e5a-4b11-bf9d-2f4e9c1a7d55"));

            migrationBuilder.DropColumn(
                name: "MaximumDeliverdTime",
                table: "Shipping");

            migrationBuilder.DropColumn(
                name: "MinimumDeliverdTime",
                table: "Shipping");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ProductImage");

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2a0b2079-1fa6-4757-8a49-12f30e849774"), "COD" },
                    { new Guid("eaf6a465-78c4-4268-b220-77a9ceed2584"), "E-Wallets" }
                });

            migrationBuilder.InsertData(
                table: "Shipping",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("0f92c01a-5687-4296-ac95-0a4362dce096"), "Standard", 0.61m },
                    { new Guid("a026ad65-9fc2-4bf3-b053-01e0818a571a"), "Express", 0.90m },
                    { new Guid("cd70cc8d-e687-4dc6-b674-b930bc12a427"), "Ultra-Fast Delivery", 1.63m }
                });
        }
    }
}
