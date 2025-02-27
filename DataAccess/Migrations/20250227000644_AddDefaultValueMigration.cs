using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValueMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
