using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class FixOnTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_OfferType_Type",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferType",
                table: "OfferType");

            migrationBuilder.RenameTable(
                name: "OfferType",
                newName: "OfferTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferTypes",
                table: "OfferTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_OfferTypes_Type",
                table: "Offers",
                column: "Type",
                principalTable: "OfferTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_OfferTypes_Type",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferTypes",
                table: "OfferTypes");

            migrationBuilder.RenameTable(
                name: "OfferTypes",
                newName: "OfferType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferType",
                table: "OfferType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_OfferType_Type",
                table: "Offers",
                column: "Type",
                principalTable: "OfferType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
