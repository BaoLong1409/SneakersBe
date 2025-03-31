using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameOfEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Shipping",
                newName: "ShippingName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Product",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Payment",
                newName: "PaymentName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Color",
                newName: "ColorName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "CategoryName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingName",
                table: "Shipping",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Product",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PaymentName",
                table: "Payment",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ColorName",
                table: "Color",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Category",
                newName: "Name");
        }
    }
}
