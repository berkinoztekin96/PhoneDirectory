using Microsoft.EntityFrameworkCore.Migrations;

namespace PhoneDirectory.Repository.Migrations
{
    public partial class Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Persons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Informations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Informations");
        }
    }
}
