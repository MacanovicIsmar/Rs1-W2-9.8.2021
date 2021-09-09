using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class add2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Takmicenjeucesnik",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OdjeljenjeStavkaId = table.Column<int>(nullable: false),
                    Pristupio = table.Column<bool>(nullable: false),
                    bodovi = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takmicenjeucesnik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Takmicenjeucesnik_OdjeljenjeStavka_OdjeljenjeStavkaId",
                        column: x => x.OdjeljenjeStavkaId,
                        principalTable: "OdjeljenjeStavka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Takmicenja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SkolaId = table.Column<int>(nullable: false),
                    Razred = table.Column<int>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    PredmetId = table.Column<int>(nullable: false),
                    TakmicenjeUcesnikId = table.Column<int>(nullable: false),
                    zakljucano = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takmicenja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Takmicenja_Predmet_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Takmicenja_Skola_SkolaId",
                        column: x => x.SkolaId,
                        principalTable: "Skola",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Takmicenja_Takmicenjeucesnik_TakmicenjeUcesnikId",
                        column: x => x.TakmicenjeUcesnikId,
                        principalTable: "Takmicenjeucesnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_PredmetId",
                table: "Takmicenja",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_SkolaId",
                table: "Takmicenja",
                column: "SkolaId");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_TakmicenjeUcesnikId",
                table: "Takmicenja",
                column: "TakmicenjeUcesnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenjeucesnik_OdjeljenjeStavkaId",
                table: "Takmicenjeucesnik",
                column: "OdjeljenjeStavkaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Takmicenja");

            migrationBuilder.DropTable(
                name: "Takmicenjeucesnik");
        }
    }
}
