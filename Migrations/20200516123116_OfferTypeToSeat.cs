using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class OfferTypeToSeat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_SeatTypes_TypeId",
                table: "Seats");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_OfferTypes_TypeId",
                table: "Seats",
                column: "TypeId",
                principalTable: "OfferTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_OfferTypes_SeatTypeId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_SeatTypes_SeatTypeId1",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_SeatTypeId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_SeatTypeId1",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatTypeId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatTypeId1",
                table: "Seats");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TypeId",
                table: "Seats",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_SeatTypes_TypeId",
                table: "Seats",
                column: "TypeId",
                principalTable: "SeatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
