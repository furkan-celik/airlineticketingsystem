using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class EventToFlight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Routes_FlightNo",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Events_EventId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Events_EventId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Events_EventId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_EventId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_EventId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Offers_EventId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Events_RouteId",
                table: "Events");

            migrationBuilder.DropColumn("EventId", table: "Seats");
            migrationBuilder.DropColumn("EventId", table: "Reservations");
            migrationBuilder.DropColumn("EventId", table: "Offers");

            migrationBuilder.RenameTable("Events", newName: "Flights");

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Seats",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Offers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlightNo",
                table: "Flights",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_FlightId",
                table: "Seats",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_FlightId",
                table: "Reservations",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_FlightId",
                table: "Offers",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_RouteId",
                table: "Flights",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Routes_RouteId",
                table: "Flights",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Events_FlightId",
                table: "Offers",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Events_FlightId",
                table: "Reservations",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Events_FlightId",
                table: "Seats",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Routes_RouteId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Events_FlightId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Events_FlightId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Events_FlightId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_FlightId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_FlightId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Offers_FlightId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Events_RouteId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Offers");

            migrationBuilder.AlterColumn<int>(
                name: "FlightNo",
                table: "Events",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_EventId",
                table: "Seats",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EventId",
                table: "Reservations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_EventId",
                table: "Offers",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_FlightNo",
                table: "Events",
                column: "FlightNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Routes_FlightNo",
                table: "Events",
                column: "FlightNo",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Events_EventId",
                table: "Offers",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Events_EventId",
                table: "Reservations",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Events_EventId",
                table: "Seats",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
