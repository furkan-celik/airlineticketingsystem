using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class SmallFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagingCompanyId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ManagingCompanyId",
                table: "AspNetUsers",
                column: "ManagingCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_ManagingCompanyId",
                table: "AspNetUsers",
                column: "ManagingCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_ManagingCompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ManagingCompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ManagingCompanyId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CId",
                table: "AspNetUsers",
                column: "CId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_CId",
                table: "AspNetUsers",
                column: "CId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
