using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoTrader.Migrations
{
    public partial class mig_add_candle_delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nCandleTwoOverRealCnt",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nCandleTwoOverRealNoLeafCnt",
                table: "fakeReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "nCandleTwoOverRealCnt",
                table: "fakeReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nCandleTwoOverRealNoLeafCnt",
                table: "fakeReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
