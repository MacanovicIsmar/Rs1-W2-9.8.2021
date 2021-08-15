using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_2020_01_30.Migrations
{
    public partial class odjeljenjem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OdjeljenjeId",
                table: "Takmicenje",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenje_OdjeljenjeId",
                table: "Takmicenje",
                column: "OdjeljenjeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Takmicenje_Odjeljenje_OdjeljenjeId",
                table: "Takmicenje",
                column: "OdjeljenjeId",
                principalTable: "Odjeljenje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Takmicenje_Odjeljenje_OdjeljenjeId",
                table: "Takmicenje");

            migrationBuilder.DropIndex(
                name: "IX_Takmicenje_OdjeljenjeId",
                table: "Takmicenje");

            migrationBuilder.DropColumn(
                name: "OdjeljenjeId",
                table: "Takmicenje");
        }
    }
}




