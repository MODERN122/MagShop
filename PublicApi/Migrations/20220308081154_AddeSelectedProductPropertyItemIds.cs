using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicApi.Migrations
{
    public partial class AddeSelectedProductPropertyItemIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "SelectedProductPropertyItemIds",
                table: "BasketItems",
                type: "_text",
                defaultValue: "{}",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedProductPropertyItemIds",
                table: "BasketItems");
        }
    }
}
