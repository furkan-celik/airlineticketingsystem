using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class OfferType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "Offers",
                newName: "Type");

            migrationBuilder.CreateTable(
                name: "OfferType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_Type",
                table: "Offers",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_OfferType_Type",
                table: "Offers",
                column: "Type",
                principalTable: "OfferType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_OfferType_Type",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "OfferType");

            migrationBuilder.DropIndex(
                name: "IX_Offers_Type",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Offers",
                newName: "type");
        }
    }
}
