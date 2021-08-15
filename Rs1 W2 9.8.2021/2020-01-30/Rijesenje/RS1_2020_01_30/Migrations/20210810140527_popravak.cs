using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_2020_01_30.Migrations
{
    public partial class popravak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Takmicenje_Odjeljenje_OdjeljenjeId",
                table: "Takmicenje");

            migrationBuilder.RenameColumn(
                name: "OdjeljenjeId",
                table: "Takmicenje",
                newName: "SkolaId");

            migrationBuilder.RenameIndex(
                name: "IX_Takmicenje_OdjeljenjeId",
                table: "Takmicenje",
                newName: "IX_Takmicenje_SkolaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Takmicenje_Skola_SkolaId",
                table: "Takmicenje",
                column: "SkolaId",
                principalTable: "Skola",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Takmicenje_Skola_SkolaId",
                table: "Takmicenje");

            migrationBuilder.RenameColumn(
                name: "SkolaId",
                table: "Takmicenje",
                newName: "OdjeljenjeId");

            migrationBuilder.RenameIndex(
                name: "IX_Takmicenje_SkolaId",
                table: "Takmicenje",
                newName: "IX_Takmicenje_OdjeljenjeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Takmicenje_Odjeljenje_OdjeljenjeId",
                table: "Takmicenje",
                column: "OdjeljenjeId",
                principalTable: "Odjeljenje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
