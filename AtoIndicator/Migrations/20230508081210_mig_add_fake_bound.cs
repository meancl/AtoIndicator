using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoTrader.Migrations
{
    public partial class mig_add_fake_bound : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fSlotDownEndPower",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fSlotUpEndPower",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "lTotalBuyEndPrice",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "lTotalSellEndPrice",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "lTotalTradeEndPrice",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nFewSpeedEndCnt",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nMissEndCnt",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nNoMoveEndCnt",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nSlotChegyulEndCnt",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nSlotHogaEndCnt",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nSlotUpDownEndCnt",
                table: "fakeReports");

            migrationBuilder.AddColumn<double>(
                name: "fBoundBottomPowerAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundBottomPowerAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundBottomPowerAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundBottomPowerAfterBuyWhile60",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundBottomPowerMinuteAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundBottomPowerMinuteAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundBottomPowerMinuteAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundTopPowerAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundTopPowerAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundTopPowerAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundTopPowerAfterBuyWhile60",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundTopPowerMinuteAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundTopPowerMinuteAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fBoundTopPowerMinuteAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fDownPowerAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fUpPowerAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "lTotalBuyPriceAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalSellPriceAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalTradePriceAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomPriceAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomPriceAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomPriceAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomPriceAfterBuyWhile60",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomPriceMinuteAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomPriceMinuteAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomPriceMinuteAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomTimeAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomTimeAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomTimeAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomTimeAfterBuyWhile60",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomTimeMinuteAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomTimeMinuteAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundBottomTimeMinuteAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopPriceAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopPriceAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopPriceAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopPriceAfterBuyWhile60",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopPriceMinuteAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopPriceMinuteAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopPriceMinuteAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopTimeAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopTimeAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopTimeAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopTimeAfterBuyWhile60",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopTimeMinuteAfterBuy",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopTimeMinuteAfterBuyWhile10",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nBoundTopTimeMinuteAfterBuyWhile30",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nChegyulCntAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nFewSpeedCntAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nHogaCntAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMissCntAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nNoMoveCntAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nPriceAfter1Sec",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nUpDownCntAfterCheck",
                table: "fakeReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fBoundBottomPowerAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundBottomPowerAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundBottomPowerAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundBottomPowerAfterBuyWhile60",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundBottomPowerMinuteAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundBottomPowerMinuteAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundBottomPowerMinuteAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundTopPowerAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundTopPowerAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundTopPowerAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundTopPowerAfterBuyWhile60",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundTopPowerMinuteAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundTopPowerMinuteAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fBoundTopPowerMinuteAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fDownPowerAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "fUpPowerAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "lTotalBuyPriceAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "lTotalSellPriceAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "lTotalTradePriceAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomPriceAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomPriceAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomPriceAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomPriceAfterBuyWhile60",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomPriceMinuteAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomPriceMinuteAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomPriceMinuteAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomTimeAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomTimeAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomTimeAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomTimeAfterBuyWhile60",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomTimeMinuteAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomTimeMinuteAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundBottomTimeMinuteAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopPriceAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopPriceAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopPriceAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopPriceAfterBuyWhile60",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopPriceMinuteAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopPriceMinuteAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopPriceMinuteAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopTimeAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopTimeAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopTimeAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopTimeAfterBuyWhile60",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopTimeMinuteAfterBuy",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopTimeMinuteAfterBuyWhile10",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nBoundTopTimeMinuteAfterBuyWhile30",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nChegyulCntAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nFewSpeedCntAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nHogaCntAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nMissCntAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nNoMoveCntAfterCheck",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nPriceAfter1Sec",
                table: "fakeReports");

            migrationBuilder.DropColumn(
                name: "nUpDownCntAfterCheck",
                table: "fakeReports");

            migrationBuilder.AddColumn<double>(
                name: "fSlotDownEndPower",
                table: "fakeReports",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "fSlotUpEndPower",
                table: "fakeReports",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "lTotalBuyEndPrice",
                table: "fakeReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalSellEndPrice",
                table: "fakeReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "lTotalTradeEndPrice",
                table: "fakeReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "nFewSpeedEndCnt",
                table: "fakeReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nMissEndCnt",
                table: "fakeReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nNoMoveEndCnt",
                table: "fakeReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotChegyulEndCnt",
                table: "fakeReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotHogaEndCnt",
                table: "fakeReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nSlotUpDownEndCnt",
                table: "fakeReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }
    }
}
