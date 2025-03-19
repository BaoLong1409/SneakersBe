using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifyShipping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "MinimumDeliverdTime",
                table: "Shipping",
                newName: "MinimumDeliveredTime");

            migrationBuilder.RenameColumn(
                name: "MaximumDeliverdTime",
                table: "Shipping",
                newName: "MaximumDeliveredTime");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "MinimumDeliveredTime",
                table: "Shipping",
                newName: "MinimumDeliverdTime");

            migrationBuilder.RenameColumn(
                name: "MaximumDeliveredTime",
                table: "Shipping",
                newName: "MaximumDeliverdTime");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
