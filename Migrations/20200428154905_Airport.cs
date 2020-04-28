using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Airport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Companies_CompanyId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Routes_RouteId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Events_FlightId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Events_FlightId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Events_FlightId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Events_EventId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Arrival",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Departure",
                table: "Routes");

            migrationBuilder.RenameIndex(
                name: "IX_Events_RouteId",
                table: "Flights",
                newName: "IX_Flights_RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_CompanyId",
                table: "Flights",
                newName: "IX_Flights_CompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Seats",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArrivalId",
                table: "Routes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartureId",
                table: "Routes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Airport",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AirportName = table.Column<string>(nullable: false),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Airport_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ArrivalId",
                table: "Routes",
                column: "ArrivalId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_DepartureId",
                table: "Routes",
                column: "DepartureId");

            migrationBuilder.CreateIndex(
                name: "IX_Airport_CityId",
                table: "Airport",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Companies_CompanyId",
                table: "Flights",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Routes_RouteId",
                table: "Flights",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Flights_FlightId",
                table: "Offers",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Flights_FlightId",
                table: "Reservations",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Airport_ArrivalId",
                table: "Routes",
                column: "ArrivalId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Airport_DepartureId",
                table: "Routes",
                column: "DepartureId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Flights_FlightId",
                table: "Seats",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Flights_EventId",
                table: "Tickets",
                column: "EventId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Companies_CompanyId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Routes_RouteId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Flights_FlightId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Flights_FlightId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Airport_ArrivalId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Airport_DepartureId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Flights_FlightId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Flights_EventId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Airport");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ArrivalId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_DepartureId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flights",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ArrivalId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "DepartureId",
                table: "Routes");

            migrationBuilder.RenameTable(
                name: "Flights",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_Flights_RouteId",
                table: "Events",
                newName: "IX_Events_RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Flights_CompanyId",
                table: "Events",
                newName: "IX_Events_CompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Seats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Arrival",
                table: "Routes",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Departure",
                table: "Routes",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "FlightId",
                table: "Reservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Companies_CompanyId",
                table: "Events",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Routes_RouteId",
                table: "Events",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Events_FlightId",
                table: "Offers",
                column: "FlightId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Events_FlightId",
                table: "Reservations",
                column: "FlightId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Events_FlightId",
                table: "Seats",
                column: "FlightId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Events_EventId",
                table: "Tickets",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
