using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductCartMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Size",
                table: "ProductCart",
                newName: "Quantity");

            migrationBuilder.AlterColumn<float>(
                name: "SizeNumber",
                table: "Size",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "ColorId",
                table: "ProductCart",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SizeId",
                table: "ProductCart",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductCart_ColorId",
                table: "ProductCart",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCart_SizeId",
                table: "ProductCart",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCart_Color_ColorId",
                table: "ProductCart",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCart_Size_SizeId",
                table: "ProductCart",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCart_Color_ColorId",
                table: "ProductCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCart_Size_SizeId",
                table: "ProductCart");

            migrationBuilder.DropIndex(
                name: "IX_ProductCart_ColorId",
                table: "ProductCart");

            migrationBuilder.DropIndex(
                name: "IX_ProductCart_SizeId",
                table: "ProductCart");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ProductCart");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "ProductCart");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ProductCart",
                newName: "Size");

            migrationBuilder.AlterColumn<int>(
                name: "SizeNumber",
                table: "Size",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
