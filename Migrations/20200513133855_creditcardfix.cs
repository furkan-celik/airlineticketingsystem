using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class creditcardfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_AspNetUsers_OwnerId",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CreditCards");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "CreditCards",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CreditCards",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_AspNetUsers_OwnerId",
                table: "CreditCards",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropPrimaryKey(
                name: "CardDate",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "CardDate",
                table: "CreditCards");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "CreditCards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "CreditCards",
                nullable: false,
                defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_AspNetUsers_OwnerId",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CreditCards");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "CreditCards",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CreditCards",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_AspNetUsers_OwnerId",
                table: "CreditCards",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "Month",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "CreditCards");

            migrationBuilder.AddColumn<int>(
                name: "CardDate",
                table: "CreditCards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
