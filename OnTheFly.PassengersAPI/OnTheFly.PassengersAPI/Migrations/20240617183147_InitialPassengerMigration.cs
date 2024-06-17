using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnTheFly.PassengersAPI.Migrations
{
    public partial class InitialPassengerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Passenger",
                columns: table => new
                {
                    Cpf = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DtBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtRegister = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Restricted = table.Column<bool>(type: "bit", nullable: false),
                    AddressZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passenger", x => x.Cpf);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Passenger");
        }
    }
}
