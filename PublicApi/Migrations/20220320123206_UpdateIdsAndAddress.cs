using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicApi.Migrations
{
    public partial class UpdateIdsAndAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreditCardId",
                table: "CreditCards",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Addresses",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Addresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Addresses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Housing",
                table: "Addresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Addresses",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Housing",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CreditCards",
                newName: "CreditCardId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Addresses",
                newName: "AddressId");
        }
    }
}
