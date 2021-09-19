using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhoneDirectory.Repository.Migrations
{
    public partial class CreatedDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Informations_InformationId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_InformationId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "InformationId",
                table: "Persons");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Persons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Informations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Informations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Informations_PersonId",
                table: "Informations",
                column: "PersonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Informations_Persons_PersonId",
                table: "Informations",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Informations_Persons_PersonId",
                table: "Informations");

            migrationBuilder.DropIndex(
                name: "IX_Informations_PersonId",
                table: "Informations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Informations");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Informations");

            migrationBuilder.AddColumn<int>(
                name: "InformationId",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_InformationId",
                table: "Persons",
                column: "InformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Informations_InformationId",
                table: "Persons",
                column: "InformationId",
                principalTable: "Informations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
