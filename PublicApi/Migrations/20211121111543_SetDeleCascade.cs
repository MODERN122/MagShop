using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicApi.Migrations
{
    public partial class SetDeleCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPropertyItems_ProductProperties_ProductPropertyId",
                table: "ProductPropertyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPropertyItems_PropertyItems_PropertyItemId",
                table: "ProductPropertyItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPropertyItems_ProductProperties_ProductPropertyId",
                table: "ProductPropertyItems",
                column: "ProductPropertyId",
                principalTable: "ProductProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPropertyItems_PropertyItems_PropertyItemId",
                table: "ProductPropertyItems",
                column: "PropertyItemId",
                principalTable: "PropertyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPropertyItems_ProductProperties_ProductPropertyId",
                table: "ProductPropertyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPropertyItems_PropertyItems_PropertyItemId",
                table: "ProductPropertyItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPropertyItems_ProductProperties_ProductPropertyId",
                table: "ProductPropertyItems",
                column: "ProductPropertyId",
                principalTable: "ProductProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPropertyItems_PropertyItems_PropertyItemId",
                table: "ProductPropertyItems",
                column: "PropertyItemId",
                principalTable: "PropertyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
