using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class FlightToRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("Flights", newName: "Routes");
            migrationBuilder.RenameColumn("FlightNo", "Offers", "RouteId");
            migrationBuilder.RenameColumn("FlightNo", "Routes", "RouteId"); 

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Flights_FlightNo",
                table: "Events");

            migrationBuilder.DropIndex("IX_Events_FlightNo", table: "Events");

            migrationBuilder.DropColumn("FlightNo", "Events");

            migrationBuilder.AddColumn<string>(
                name: "RouteId",
                table: "Events",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_RouteId",
                table: "Events",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Routes_RouteId",
                table: "Events",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Flights_FlightNo",
                table: "Offers");
            
            migrationBuilder.RenameIndex("IX_Offers_FlightNo", "IX_Offers_RouteId", table: "Offers");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Routes_RouteId",
                table: "Offers",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Routes_FlightNo",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Routes_RouteId",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Offers_RouteId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Offers");

            migrationBuilder.AddColumn<string>(
                name: "FlightNo",
                table: "Offers",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightNo = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Arrival = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Departure = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    ETA = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightNo);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_FlightNo",
                table: "Offers",
                column: "FlightNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Flights_FlightNo",
                table: "Events",
                column: "FlightNo",
                principalTable: "Flights",
                principalColumn: "FlightNo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Flights_FlightNo",
                table: "Offers",
                column: "FlightNo",
                principalTable: "Flights",
                principalColumn: "FlightNo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
