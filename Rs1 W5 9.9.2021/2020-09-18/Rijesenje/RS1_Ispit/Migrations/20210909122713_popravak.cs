using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class popravak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Takmicenje_Predmet_PredmetId",
                table: "Takmicenje");

            migrationBuilder.DropColumn(
                name: "PrtedmetId",
                table: "Takmicenje");

            migrationBuilder.AlterColumn<int>(
                name: "PredmetId",
                table: "Takmicenje",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Takmicenje_Predmet_PredmetId",
                table: "Takmicenje",
                column: "PredmetId",
                principalTable: "Predmet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Takmicenje_Predmet_PredmetId",
                table: "Takmicenje");

            migrationBuilder.AlterColumn<int>(
                name: "PredmetId",
                table: "Takmicenje",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PrtedmetId",
                table: "Takmicenje",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Takmicenje_Predmet_PredmetId",
                table: "Takmicenje",
                column: "PredmetId",
                principalTable: "Predmet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
