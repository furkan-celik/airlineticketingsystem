using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class PurchaseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Seats",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProcessTime = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<string>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PurchaseId",
                table: "Tickets",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_OwnerId",
                table: "Purchases",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Purchases_PurchaseId",
                table: "Tickets",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Purchases_PurchaseId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PurchaseId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Seats");
        }
    }
}
