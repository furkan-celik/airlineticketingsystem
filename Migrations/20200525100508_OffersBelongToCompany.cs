using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class OffersBelongToCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Flights_FlightId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_FlightId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Offers");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Offers",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "OfferFlight",
                columns: table => new
                {
                    OfferId = table.Column<int>(nullable: false),
                    FlightId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferFlight", x => new { x.OfferId, x.FlightId });
                    table.ForeignKey(
                        name: "FK_OfferFlight_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferFlight_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CompanyId",
                table: "Offers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferFlight_FlightId",
                table: "OfferFlight",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Companies_CompanyId",
                table: "Offers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Companies_CompanyId",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "OfferFlight");

            migrationBuilder.DropIndex(
                name: "IX_Offers_CompanyId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Offers");

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Offers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_FlightId",
                table: "Offers",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Flights_FlightId",
                table: "Offers",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
