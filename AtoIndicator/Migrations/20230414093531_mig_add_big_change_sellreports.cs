using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_big_change_sellreports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nFewSpeedCount",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMissCount",
                table: "sellReports");

            migrationBuilder.AddColumn<double>(
                name: "fAccumDownPower",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fAccumUpPower",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBottomPowerMinuteAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMaxPowerMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMaxPowerMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMinPowerMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fMinPowerMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "lMarketCap",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalBuyVolume",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalSellVolume",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalTradeVolume",
                table: "sellReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "nAccumCountRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nAccumUpDownCount",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomPriceMinuteAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBottomTimeMinuteAfterBuy",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nCandleTwoOverRealCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nCandleTwoOverRealNoLeafCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nCrushCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nCrushDownCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nCrushSpecialDownCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nCrushUpCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nDownCandleCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nDownTailCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeAssistantCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeBuyCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFakeResistCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFewSpeedCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nHogaCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMarketCapRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMaxPriceMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMaxPriceMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMaxTimeMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMaxTimeMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinPriceMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinPriceMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinTimeMinuteAfterBuyWhile10",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinTimeMinuteAfterBuyWhile30",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinuteBuyPriceRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinuteBuyVolumeRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinuteCountRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinutePowerRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinuteTotalRank",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinuteTradePriceRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinuteTradeVolumeRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMinuteUpDownRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMissCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nOrderPrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nOriginOrderPrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nPowerRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nPriceDownCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nPriceUpCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nShootingCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTotalBuyPriceRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTotalBuyVolumeRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTotalFakeCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTotalFakeMinuteCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTotalRank",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTotalTradePriceRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTotalTradeVolumeRanking",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nTradeCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nUpCandleCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nUpTailCnt",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nYesterdayEndPrice",
                table: "sellReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fAccumDownPower",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fAccumUpPower",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fBottomPowerMinuteAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fMaxPowerMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fMaxPowerMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fMinPowerMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "fMinPowerMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "lMarketCap",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "lTotalBuyVolume",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "lTotalSellVolume",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "lTotalTradeVolume",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nAccumCountRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nAccumUpDownCount",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomPriceMinuteAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nBottomTimeMinuteAfterBuy",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nCandleTwoOverRealCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nCandleTwoOverRealNoLeafCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nCrushCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nCrushDownCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nCrushSpecialDownCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nCrushUpCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nDownCandleCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nDownTailCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFakeAssistantCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFakeBuyCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFakeResistCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nFewSpeedCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nHogaCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMarketCapRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMaxPriceMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMaxPriceMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMaxTimeMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMaxTimeMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinPriceMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinPriceMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinTimeMinuteAfterBuyWhile10",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinTimeMinuteAfterBuyWhile30",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinuteBuyPriceRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinuteBuyVolumeRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinuteCountRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinutePowerRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinuteTotalRank",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinuteTradePriceRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinuteTradeVolumeRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMinuteUpDownRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nMissCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nOrderPrice",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nOriginOrderPrice",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nPowerRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nPriceDownCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nPriceUpCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nShootingCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTotalBuyPriceRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTotalBuyVolumeRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTotalFakeCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTotalFakeMinuteCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTotalRank",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTotalTradePriceRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTotalTradeVolumeRanking",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nTradeCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nUpCandleCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nUpTailCnt",
                table: "sellReports");

            migrationBuilder.DropColumn(
                name: "nYesterdayEndPrice",
                table: "sellReports");

            migrationBuilder.AddColumn<int>(
                name: "nFewSpeedCount",
                table: "sellReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMissCount",
                table: "sellReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
