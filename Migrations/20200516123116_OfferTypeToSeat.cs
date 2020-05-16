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
            
        }
    }
}
