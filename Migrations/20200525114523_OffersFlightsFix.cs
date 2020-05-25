using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class OffersFlightsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferFlight_Flights_FlightId",
                table: "OfferFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferFlight_Offers_OfferId",
                table: "OfferFlight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferFlight",
                table: "OfferFlight");

            migrationBuilder.RenameTable(
                name: "OfferFlight",
                newName: "OfferFlights");

            migrationBuilder.RenameIndex(
                name: "IX_OfferFlight_FlightId",
                table: "OfferFlights",
                newName: "IX_OfferFlights_FlightId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferFlights",
                table: "OfferFlights",
                columns: new[] { "OfferId", "FlightId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OfferFlights_Flights_FlightId",
                table: "OfferFlights",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferFlights_Offers_OfferId",
                table: "OfferFlights",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferFlights_Flights_FlightId",
                table: "OfferFlights");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferFlights_Offers_OfferId",
                table: "OfferFlights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferFlights",
                table: "OfferFlights");

            migrationBuilder.RenameTable(
                name: "OfferFlights",
                newName: "OfferFlight");

            migrationBuilder.RenameIndex(
                name: "IX_OfferFlights_FlightId",
                table: "OfferFlight",
                newName: "IX_OfferFlight_FlightId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferFlight",
                table: "OfferFlight",
                columns: new[] { "OfferId", "FlightId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OfferFlight_Flights_FlightId",
                table: "OfferFlight",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferFlight_Offers_OfferId",
                table: "OfferFlight",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
