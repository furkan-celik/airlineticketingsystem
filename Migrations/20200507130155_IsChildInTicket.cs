using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class IsChildInTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferTicket_Offers_OfferId",
                table: "OfferTicket");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferTicket_Tickets_TicketId",
                table: "OfferTicket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferTicket",
                table: "OfferTicket");

            migrationBuilder.RenameTable(
                name: "OfferTicket",
                newName: "OfferTickets");

            migrationBuilder.RenameIndex(
                name: "IX_OfferTicket_TicketId",
                table: "OfferTickets",
                newName: "IX_OfferTickets_TicketId");

            migrationBuilder.AddColumn<bool>(
                name: "isChild",
                table: "Tickets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferTickets",
                table: "OfferTickets",
                columns: new[] { "OfferId", "TicketId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OfferTickets_Offers_OfferId",
                table: "OfferTickets",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferTickets_Tickets_TicketId",
                table: "OfferTickets",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferTickets_Offers_OfferId",
                table: "OfferTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferTickets_Tickets_TicketId",
                table: "OfferTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferTickets",
                table: "OfferTickets");

            migrationBuilder.DropColumn(
                name: "isChild",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "OfferTickets",
                newName: "OfferTicket");

            migrationBuilder.RenameIndex(
                name: "IX_OfferTickets_TicketId",
                table: "OfferTicket",
                newName: "IX_OfferTicket_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferTicket",
                table: "OfferTicket",
                columns: new[] { "OfferId", "TicketId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OfferTicket_Offers_OfferId",
                table: "OfferTicket",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferTicket_Tickets_TicketId",
                table: "OfferTicket",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
