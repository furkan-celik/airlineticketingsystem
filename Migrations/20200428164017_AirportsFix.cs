using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class AirportsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airport_City_CityId",
                table: "Airport");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Airport_ArrivalId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Airport_DepartureId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Airport",
                table: "Airport");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "Cities");

            migrationBuilder.RenameTable(
                name: "Airport",
                newName: "Airports");

            migrationBuilder.RenameIndex(
                name: "IX_Airport_CityId",
                table: "Airports",
                newName: "IX_Airports_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Airports",
                table: "Airports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Airports_Cities_CityId",
                table: "Airports",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Airports_ArrivalId",
                table: "Routes",
                column: "ArrivalId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Airports_DepartureId",
                table: "Routes",
                column: "DepartureId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airports_Cities_CityId",
                table: "Airports");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Airports_ArrivalId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Airports_DepartureId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Airports",
                table: "Airports");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "City");

            migrationBuilder.RenameTable(
                name: "Airports",
                newName: "Airport");

            migrationBuilder.RenameIndex(
                name: "IX_Airports_CityId",
                table: "Airport",
                newName: "IX_Airport_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Airport",
                table: "Airport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Airport_City_CityId",
                table: "Airport",
                column: "CityId",
                principalTable: "City",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Airport_ArrivalId",
                table: "Routes",
                column: "ArrivalId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Airport_DepartureId",
                table: "Routes",
                column: "DepartureId",
                principalTable: "Airport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
