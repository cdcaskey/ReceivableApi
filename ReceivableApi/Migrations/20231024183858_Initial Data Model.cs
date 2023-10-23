using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceivableApi.Migrations
{
    public partial class InitialDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Debtors",
                columns: table => new
                {
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address1 = table.Column<string>(type: "TEXT", nullable: true),
                    Address2 = table.Column<string>(type: "TEXT", nullable: true),
                    Town = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    Zip = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode = table.Column<string>(type: "TEXT", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debtors", x => x.Reference);
                });

            migrationBuilder.CreateTable(
                name: "Receivables",
                columns: table => new
                {
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    CurrencyCode = table.Column<string>(type: "TEXT", nullable: false),
                    Issued = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OpeningValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    PaidValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Due = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Cancelled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DebtorId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receivables", x => x.Reference);
                    table.ForeignKey(
                        name: "FK_Receivables_Debtors_DebtorId",
                        column: x => x.DebtorId,
                        principalTable: "Debtors",
                        principalColumn: "Reference",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receivables_DebtorId",
                table: "Receivables",
                column: "DebtorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receivables");

            migrationBuilder.DropTable(
                name: "Debtors");
        }
    }
}
