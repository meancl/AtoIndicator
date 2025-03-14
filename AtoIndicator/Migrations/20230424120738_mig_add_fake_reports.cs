﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AtoIndicator.Migrations
{
    public partial class mig_add_fake_reports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fakeReports",
                columns: table => new
                {
                    dTradeTime = table.Column<DateTime>(nullable: false),
                    sCode = table.Column<string>(nullable: false),
                    nBuyStrategyIdx = table.Column<int>(nullable: false),
                    nBuyStrategySequenceIdx = table.Column<int>(nullable: false),
                    nLocationOfComp = table.Column<int>(nullable: false),
                    sCodeName = table.Column<string>(nullable: false),
                    nRqTime = table.Column<int>(nullable: false),
                    nSlotHogaEndCnt = table.Column<int>(nullable: false),
                    nSlotChegyulEndCnt = table.Column<int>(nullable: false),
                    nSlotUpDownEndCnt = table.Column<int>(nullable: false),
                    fSlotUpEndPower = table.Column<double>(nullable: false),
                    fSlotDownEndPower = table.Column<double>(nullable: false),
                    nNoMoveEndCnt = table.Column<int>(nullable: false),
                    nFewSpeedEndCnt = table.Column<int>(nullable: false),
                    nMissEndCnt = table.Column<int>(nullable: false),
                    lTotalTradeEndPrice = table.Column<long>(nullable: false),
                    lTotalBuyEndPrice = table.Column<long>(nullable: false),
                    lTotalSellEndPrice = table.Column<long>(nullable: false),
                    nMaxPriceAfterBuy = table.Column<int>(nullable: false),
                    nMaxTimeAfterBuy = table.Column<int>(nullable: false),
                    fMaxPowerAfterBuy = table.Column<double>(nullable: false),
                    nMinPriceAfterBuy = table.Column<int>(nullable: false),
                    nMinTimeAfterBuy = table.Column<int>(nullable: false),
                    fMinPowerAfterBuy = table.Column<double>(nullable: false),
                    nBottomPriceAfterBuy = table.Column<int>(nullable: false),
                    nBottomTimeAfterBuy = table.Column<int>(nullable: false),
                    fBottomPowerAfterBuy = table.Column<double>(nullable: false),
                    nTopPriceAfterBuy = table.Column<int>(nullable: false),
                    nTopTimeAfterBuy = table.Column<int>(nullable: false),
                    fTopPowerAfterBuy = table.Column<double>(nullable: false),
                    nMaxPriceMinuteAfterBuy = table.Column<int>(nullable: false),
                    nMaxTimeMinuteAfterBuy = table.Column<int>(nullable: false),
                    fMaxPowerMinuteAfterBuy = table.Column<double>(nullable: false),
                    nMinPriceMinuteAfterBuy = table.Column<int>(nullable: false),
                    nMinTimeMinuteAfterBuy = table.Column<int>(nullable: false),
                    fMinPowerMinuteAfterBuy = table.Column<double>(nullable: false),
                    nBottomPriceMinuteAfterBuy = table.Column<int>(nullable: false),
                    nBottomTimeMinuteAfterBuy = table.Column<int>(nullable: false),
                    fBottomPowerMinuteAfterBuy = table.Column<double>(nullable: false),
                    nTopPriceMinuteAfterBuy = table.Column<int>(nullable: false),
                    nTopTimeMinuteAfterBuy = table.Column<int>(nullable: false),
                    fTopPowerMinuteAfterBuy = table.Column<double>(nullable: false),
                    nMaxPriceAfterBuyWhile10 = table.Column<int>(nullable: false),
                    nMaxTimeAfterBuyWhile10 = table.Column<int>(nullable: false),
                    fMaxPowerAfterBuyWhile10 = table.Column<double>(nullable: false),
                    nMinPriceAfterBuyWhile10 = table.Column<int>(nullable: false),
                    nMinTimeAfterBuyWhile10 = table.Column<int>(nullable: false),
                    fMinPowerAfterBuyWhile10 = table.Column<double>(nullable: false),
                    nBottomPriceAfterBuyWhile10 = table.Column<int>(nullable: false),
                    nBottomTimeAfterBuyWhile10 = table.Column<int>(nullable: false),
                    fBottomPowerAfterBuyWhile10 = table.Column<double>(nullable: false),
                    nTopPriceAfterBuyWhile10 = table.Column<int>(nullable: false),
                    nTopTimeAfterBuyWhile10 = table.Column<int>(nullable: false),
                    fTopPowerAfterBuyWhile10 = table.Column<double>(nullable: false),
                    nMaxPriceAfterBuyWhile30 = table.Column<int>(nullable: false),
                    nMaxTimeAfterBuyWhile30 = table.Column<int>(nullable: false),
                    fMaxPowerAfterBuyWhile30 = table.Column<double>(nullable: false),
                    nMinPriceAfterBuyWhile30 = table.Column<int>(nullable: false),
                    nMinTimeAfterBuyWhile30 = table.Column<int>(nullable: false),
                    fMinPowerAfterBuyWhile30 = table.Column<double>(nullable: false),
                    nBottomPriceAfterBuyWhile30 = table.Column<int>(nullable: false),
                    nBottomTimeAfterBuyWhile30 = table.Column<int>(nullable: false),
                    fBottomPowerAfterBuyWhile30 = table.Column<double>(nullable: false),
                    nTopPriceAfterBuyWhile30 = table.Column<int>(nullable: false),
                    nTopTimeAfterBuyWhile30 = table.Column<int>(nullable: false),
                    fTopPowerAfterBuyWhile30 = table.Column<double>(nullable: false),
                    nMaxPriceAfterBuyWhile60 = table.Column<int>(nullable: false),
                    nMaxTimeAfterBuyWhile60 = table.Column<int>(nullable: false),
                    fMaxPowerAfterBuyWhile60 = table.Column<double>(nullable: false),
                    nMinPriceAfterBuyWhile60 = table.Column<int>(nullable: false),
                    nMinTimeAfterBuyWhile60 = table.Column<int>(nullable: false),
                    fMinPowerAfterBuyWhile60 = table.Column<double>(nullable: false),
                    nBottomPriceAfterBuyWhile60 = table.Column<int>(nullable: false),
                    nBottomTimeAfterBuyWhile60 = table.Column<int>(nullable: false),
                    fBottomPowerAfterBuyWhile60 = table.Column<double>(nullable: false),
                    nTopPriceAfterBuyWhile60 = table.Column<int>(nullable: false),
                    nTopTimeAfterBuyWhile60 = table.Column<int>(nullable: false),
                    fTopPowerAfterBuyWhile60 = table.Column<double>(nullable: false),
                    nMaxPriceMinuteAfterBuyWhile10 = table.Column<int>(nullable: false),
                    nMaxTimeMinuteAfterBuyWhile10 = table.Column<int>(nullable: false),
                    fMaxPowerMinuteAfterBuyWhile10 = table.Column<double>(nullable: false),
                    nMinPriceMinuteAfterBuyWhile10 = table.Column<int>(nullable: false),
                    nMinTimeMinuteAfterBuyWhile10 = table.Column<int>(nullable: false),
                    fMinPowerMinuteAfterBuyWhile10 = table.Column<double>(nullable: false),
                    nBottomPriceMinuteAfterBuyWhile10 = table.Column<int>(nullable: false),
                    nBottomTimeMinuteAfterBuyWhile10 = table.Column<int>(nullable: false),
                    fBottomPowerMinuteAfterBuyWhile10 = table.Column<double>(nullable: false),
                    nTopPriceMinuteAfterBuyWhile10 = table.Column<int>(nullable: false),
                    nTopTimeMinuteAfterBuyWhile10 = table.Column<int>(nullable: false),
                    fTopPowerMinuteAfterBuyWhile10 = table.Column<double>(nullable: false),
                    nMaxPriceMinuteAfterBuyWhile30 = table.Column<int>(nullable: false),
                    nMaxTimeMinuteAfterBuyWhile30 = table.Column<int>(nullable: false),
                    fMaxPowerMinuteAfterBuyWhile30 = table.Column<double>(nullable: false),
                    nMinPriceMinuteAfterBuyWhile30 = table.Column<int>(nullable: false),
                    nMinTimeMinuteAfterBuyWhile30 = table.Column<int>(nullable: false),
                    fMinPowerMinuteAfterBuyWhile30 = table.Column<double>(nullable: false),
                    nBottomPriceMinuteAfterBuyWhile30 = table.Column<int>(nullable: false),
                    nBottomTimeMinuteAfterBuyWhile30 = table.Column<int>(nullable: false),
                    fBottomPowerMinuteAfterBuyWhile30 = table.Column<double>(nullable: false),
                    nTopPriceMinuteAfterBuyWhile30 = table.Column<int>(nullable: false),
                    nTopTimeMinuteAfterBuyWhile30 = table.Column<int>(nullable: false),
                    fTopPowerMinuteAfterBuyWhile30 = table.Column<double>(nullable: false),
                    fStartGap = table.Column<double>(nullable: false),
                    sType = table.Column<string>(nullable: true),
                    fPowerWithOutGap = table.Column<double>(nullable: false),
                    fPower = table.Column<double>(nullable: false),
                    fPlusCnt07 = table.Column<double>(nullable: false),
                    fMinusCnt07 = table.Column<double>(nullable: false),
                    fPlusCnt09 = table.Column<double>(nullable: false),
                    fMinusCnt09 = table.Column<double>(nullable: false),
                    fPowerJar = table.Column<double>(nullable: false),
                    fOnlyDownPowerJar = table.Column<double>(nullable: false),
                    fOnlyUpPowerJar = table.Column<double>(nullable: false),
                    nTradeCnt = table.Column<int>(nullable: false),
                    nChegyulCnt = table.Column<int>(nullable: false),
                    nHogaCnt = table.Column<int>(nullable: false),
                    nNoMoveCnt = table.Column<int>(nullable: false),
                    nFewSpeedCnt = table.Column<int>(nullable: false),
                    nMissCnt = table.Column<int>(nullable: false),
                    lTotalTradeVolume = table.Column<long>(nullable: false),
                    lTotalBuyVolume = table.Column<long>(nullable: false),
                    lTotalSellVolume = table.Column<long>(nullable: false),
                    nAccumUpDownCount = table.Column<int>(nullable: false),
                    fAccumUpPower = table.Column<double>(nullable: false),
                    fAccumDownPower = table.Column<double>(nullable: false),
                    lTotalTradePrice = table.Column<long>(nullable: false),
                    lTotalBuyPrice = table.Column<long>(nullable: false),
                    lTotalSellPrice = table.Column<long>(nullable: false),
                    lMarketCap = table.Column<long>(nullable: false),
                    nAccumCountRanking = table.Column<int>(nullable: false),
                    nMarketCapRanking = table.Column<int>(nullable: false),
                    nPowerRanking = table.Column<int>(nullable: false),
                    nTotalBuyPriceRanking = table.Column<int>(nullable: false),
                    nTotalBuyVolumeRanking = table.Column<int>(nullable: false),
                    nTotalTradePriceRanking = table.Column<int>(nullable: false),
                    nTotalTradeVolumeRanking = table.Column<int>(nullable: false),
                    nTotalRank = table.Column<int>(nullable: false),
                    nMinuteTotalRank = table.Column<int>(nullable: false),
                    nMinuteTradePriceRanking = table.Column<int>(nullable: false),
                    nMinuteTradeVolumeRanking = table.Column<int>(nullable: false),
                    nMinuteBuyPriceRanking = table.Column<int>(nullable: false),
                    nMinuteBuyVolumeRanking = table.Column<int>(nullable: false),
                    nMinutePowerRanking = table.Column<int>(nullable: false),
                    nMinuteCountRanking = table.Column<int>(nullable: false),
                    nMinuteUpDownRanking = table.Column<int>(nullable: false),
                    nFakeBuyCnt = table.Column<int>(nullable: false),
                    nFakeAssistantCnt = table.Column<int>(nullable: false),
                    nFakeResistCnt = table.Column<int>(nullable: false),
                    nPriceUpCnt = table.Column<int>(nullable: false),
                    nPriceDownCnt = table.Column<int>(nullable: false),
                    nTotalFakeCnt = table.Column<int>(nullable: false),
                    nTotalFakeMinuteCnt = table.Column<int>(nullable: false),
                    nUpCandleCnt = table.Column<int>(nullable: false),
                    nDownCandleCnt = table.Column<int>(nullable: false),
                    nUpTailCnt = table.Column<int>(nullable: false),
                    nDownTailCnt = table.Column<int>(nullable: false),
                    nShootingCnt = table.Column<int>(nullable: false),
                    nCandleTwoOverRealCnt = table.Column<int>(nullable: false),
                    nCandleTwoOverRealNoLeafCnt = table.Column<int>(nullable: false),
                    nFs = table.Column<int>(nullable: false),
                    nFb = table.Column<int>(nullable: false),
                    fSpeedCur = table.Column<double>(nullable: false),
                    fHogaSpeedCur = table.Column<double>(nullable: false),
                    fTradeCur = table.Column<double>(nullable: false),
                    fPureTradeCur = table.Column<double>(nullable: false),
                    fPureBuyCur = table.Column<double>(nullable: false),
                    fHogaRatioCur = table.Column<double>(nullable: false),
                    fSharePerHoga = table.Column<double>(nullable: false),
                    fSharePerTrade = table.Column<double>(nullable: false),
                    fHogaPerTrade = table.Column<double>(nullable: false),
                    fTradePerPure = table.Column<double>(nullable: false),
                    fMaDownFsVal = table.Column<double>(nullable: false),
                    fMa20mVal = table.Column<double>(nullable: false),
                    fMa1hVal = table.Column<double>(nullable: false),
                    fMa2hVal = table.Column<double>(nullable: false),
                    fMaxMaDownFsVal = table.Column<double>(nullable: false),
                    fMaxMa20mVal = table.Column<double>(nullable: false),
                    fMaxMa1hVal = table.Column<double>(nullable: false),
                    fMaxMa2hVal = table.Column<double>(nullable: false),
                    nMaxMaDownFsTime = table.Column<double>(nullable: false),
                    nMaxMa20mTime = table.Column<double>(nullable: false),
                    nMaxMa1hTime = table.Column<double>(nullable: false),
                    nMaxMa2hTime = table.Column<double>(nullable: false),
                    nDownCntMa20m = table.Column<int>(nullable: false),
                    nDownCntMa1h = table.Column<int>(nullable: false),
                    nDownCntMa2h = table.Column<int>(nullable: false),
                    nUpCntMa20m = table.Column<int>(nullable: false),
                    nUpCntMa1h = table.Column<int>(nullable: false),
                    nUpCntMa2h = table.Column<int>(nullable: false),
                    fMSlope = table.Column<double>(nullable: false),
                    fISlope = table.Column<double>(nullable: false),
                    fTSlope = table.Column<double>(nullable: false),
                    fHSlope = table.Column<double>(nullable: false),
                    fRSlope = table.Column<double>(nullable: false),
                    fDSlope = table.Column<double>(nullable: false),
                    fMAngle = table.Column<double>(nullable: false),
                    fIAngle = table.Column<double>(nullable: false),
                    fTAngle = table.Column<double>(nullable: false),
                    fHAngle = table.Column<double>(nullable: false),
                    fRAngle = table.Column<double>(nullable: false),
                    fDAngle = table.Column<double>(nullable: false),
                    nCrushCnt = table.Column<int>(nullable: false),
                    nCrushUpCnt = table.Column<int>(nullable: false),
                    nCrushDownCnt = table.Column<int>(nullable: false),
                    nCrushSpecialDownCnt = table.Column<int>(nullable: false),
                    nYesterdayEndPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fakeReports", x => new { x.dTradeTime, x.sCode, x.nBuyStrategyIdx, x.nBuyStrategySequenceIdx, x.nLocationOfComp });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fakeReports");
        }
    }
}
