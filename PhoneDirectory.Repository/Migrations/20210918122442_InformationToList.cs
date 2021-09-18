using Microsoft.EntityFrameworkCore.Migrations;

namespace PhoneDirectory.Repository.Migrations
{
    public partial class InformationToList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Informations_PersonId",
                table: "Informations");

            migrationBuilder.CreateIndex(
                name: "IX_Informations_PersonId",
                table: "Informations",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Informations_PersonId",
                table: "Informations");

            migrationBuilder.CreateIndex(
                name: "IX_Informations_PersonId",
                table: "Informations",
                column: "PersonId",
                unique: true);
        }
    }
}
