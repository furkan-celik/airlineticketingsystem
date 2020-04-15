using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class foreignKeyFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Flights_FlightNo1",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_FlightNo1",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FlightNo1",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "FlightNo",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Events_FlightNo",
                table: "Events",
                column: "FlightNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Flights_FlightNo",
                table: "Events",
                column: "FlightNo",
                principalTable: "Flights",
                principalColumn: "FlightNo",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Flights_FlightNo",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_FlightNo",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "FlightNo",
                table: "Events",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "FlightNo1",
                table: "Events",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_FlightNo1",
                table: "Events",
                column: "FlightNo1");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Flights_FlightNo1",
                table: "Events",
                column: "FlightNo1",
                principalTable: "Flights",
                principalColumn: "FlightNo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
