using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_2020_01_30.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakmicenjeUcesnik_OdjeljenjeStavka_OdjeljenjeId",
                table: "TakmicenjeUcesnik");

            migrationBuilder.DropIndex(
                name: "IX_TakmicenjeUcesnik_OdjeljenjeId",
                table: "TakmicenjeUcesnik");

            migrationBuilder.AddColumn<int>(
                name: "OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TakmicenjeUcesnik_OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik",
                column: "OdjeljenjeStavkaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakmicenjeUcesnik_OdjeljenjeStavka_OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik",
                column: "OdjeljenjeStavkaId",
                principalTable: "OdjeljenjeStavka",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakmicenjeUcesnik_OdjeljenjeStavka_OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik");

            migrationBuilder.DropIndex(
                name: "IX_TakmicenjeUcesnik_OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik");

            migrationBuilder.DropColumn(
                name: "OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnik");

            migrationBuilder.CreateIndex(
                name: "IX_TakmicenjeUcesnik_OdjeljenjeId",
                table: "TakmicenjeUcesnik",
                column: "OdjeljenjeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakmicenjeUcesnik_OdjeljenjeStavka_OdjeljenjeId",
                table: "TakmicenjeUcesnik",
                column: "OdjeljenjeId",
                principalTable: "OdjeljenjeStavka",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
