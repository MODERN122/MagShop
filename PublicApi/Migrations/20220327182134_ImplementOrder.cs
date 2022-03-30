using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicApi.Migrations
{
    public partial class ImplementOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "SelectedProductPropertyItemIds",
                table: "OrderItems",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SelectedProductPropertyItemIds",
                table: "OrderItems");
        }
    }
}
