using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_slippage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "lTotalBuyEndPrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalBuyPrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalSellEndPrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalSellPrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalTradeEndPrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalTradePrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "nFewSpeedCount",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFewSpeedEndCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMissCount",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMissEndCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nNoMoveCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nNoMoveEndCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "fAccumDownPower",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fAccumUpPower",
                table: "buyReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "lTotalBuyEndPrice",
                table: "buyReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalSellEndPrice",
                table: "buyReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalTradeEndPrice",
                table: "buyReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "nFewSpeedEndCnt",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMissEndCnt",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nNoMoveEndCnt",
                table: "buyReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lTotalBuyEndPrice",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "lTotalBuyPrice",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "lTotalSellEndPrice",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "lTotalSellPrice",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "lTotalTradeEndPrice",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "lTotalTradePrice",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFewSpeedCount",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFewSpeedEndCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMissCount",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMissEndCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nNoMoveCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nNoMoveEndCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fAccumDownPower",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "fAccumUpPower",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "lTotalBuyEndPrice",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "lTotalSellEndPrice",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "lTotalTradeEndPrice",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nFewSpeedEndCnt",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nMissEndCnt",
                table: "buyReports");

            migrationBuilder.DropColumn(
                name: "nNoMoveEndCnt",
                table: "buyReports");
        }
    }
}
