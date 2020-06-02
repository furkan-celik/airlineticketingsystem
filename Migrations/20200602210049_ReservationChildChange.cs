using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class ReservationChildChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isChild",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "numOfAdult",
                table: "Reservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "numOfChild",
                table: "Reservations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Purchases_PurchaseId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "numOfAdult",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "numOfChild",
                table: "Reservations");

            migrationBuilder.AddColumn<bool>(
                name: "isChild",
                table: "Reservations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Purchases_PurchaseId",
                table: "Tickets",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
