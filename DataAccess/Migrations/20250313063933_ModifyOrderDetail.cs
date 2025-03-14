using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ColorId",
                table: "OrderDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SizeId",
                table: "OrderDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ColorId",
                table: "OrderDetail",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_SizeId",
                table: "OrderDetail",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Color_ColorId",
                table: "OrderDetail",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Size_SizeId",
                table: "OrderDetail",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Color_ColorId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Size_SizeId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ColorId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_SizeId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "OrderDetail");
        }
    }
}
