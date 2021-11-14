using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicApi.Migrations
{
    public partial class AddedUploadDateTimeToImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDateTime",
                table: "Images",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDateTime",
                table: "Images");
        }
    }
}
