using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class ChildToReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Offers_OfferId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_OfferId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Reservations");

            migrationBuilder.AddColumn<bool>(
                name: "isChild",
                table: "Reservations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReservationOffers",
                columns: table => new
                {
                    ReservationId = table.Column<int>(nullable: false),
                    OfferId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationOffers", x => new { x.OfferId, x.ReservationId });
                    table.ForeignKey(
                        name: "FK_ReservationOffers_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationOffers_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationOffers_ReservationId",
                table: "ReservationOffers",
                column: "ReservationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationOffers");

            migrationBuilder.DropColumn(
                name: "isChild",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_OfferId",
                table: "Reservations",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Offers_OfferId",
                table: "Reservations",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
