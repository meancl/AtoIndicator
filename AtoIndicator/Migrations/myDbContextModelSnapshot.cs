﻿// <auto-generated />
using System;
using AtoTrader.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AtoTrader.Migrations
{
    [DbContext(typeof(myDbContext))]
    partial class myDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AtoTrader.DB.BasicInfo", b =>
                {
                    b.Property<DateTime>("생성시간")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("종목코드")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("거래량")
                        .HasColumnType("int");

                    b.Property<int>("고가")
                        .HasColumnType("int");

                    b.Property<double>("등락율")
                        .HasColumnType("double");

                    b.Property<long>("상장주식")
                        .HasColumnType("bigint");

                    b.Property<int>("상한가")
                        .HasColumnType("int");

                    b.Property<int>("시가")
                        .HasColumnType("int");

                    b.Property<long>("시가총액")
                        .HasColumnType("bigint");

                    b.Property<int>("연중최고")
                        .HasColumnType("int");

                    b.Property<int>("연중최저")
                        .HasColumnType("int");

                    b.Property<double>("외인소진률")
                        .HasColumnType("double");

                    b.Property<double>("유통비율")
                        .HasColumnType("double");

                    b.Property<long>("유통주식")
                        .HasColumnType("bigint");

                    b.Property<int>("저가")
                        .HasColumnType("int");

                    b.Property<int>("전일대비")
                        .HasColumnType("int");

                    b.Property<string>("종목명")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("종목타입")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("최고250")
                        .HasColumnType("int");

                    b.Property<double>("최고가250대비율")
                        .HasColumnType("double");

                    b.Property<string>("최고가250일")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("최저250")
                        .HasColumnType("int");

                    b.Property<double>("최저가250대비율")
                        .HasColumnType("double");

                    b.Property<string>("최저가250일")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("하한가")
                        .HasColumnType("int");

                    b.Property<int>("현재가")
                        .HasColumnType("int");

                    b.HasKey("생성시간", "종목코드");

                    b.ToTable("basicInfo");
                });

            modelBuilder.Entity("AtoTrader.DB.FakeReports", b =>
                {
                    b.Property<DateTime>("dTradeTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("sCode")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("nBuyStrategyIdx")
                        .HasColumnType("int");

                    b.Property<int>("nBuyStrategySequenceIdx")
                        .HasColumnType("int");

                    b.Property<int>("nLocationOfComp")
                        .HasColumnType("int");

                    b.Property<double>("fAccumDownPower")
                        .HasColumnType("double");

                    b.Property<double>("fAccumUpPower")
                        .HasColumnType("double");

                    b.Property<double>("fBottomPowerAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fBottomPowerAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fBottomPowerAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fBottomPowerAfterBuyWhile60")
                        .HasColumnType("double");

                    b.Property<double>("fBottomPowerMinuteAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fBottomPowerMinuteAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fBottomPowerMinuteAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fBoundBottomPowerAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fBoundBottomPowerAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fBoundBottomPowerAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fBoundBottomPowerAfterBuyWhile60")
                        .HasColumnType("double");

                    b.Property<double>("fBoundBottomPowerMinuteAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fBoundBottomPowerMinuteAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fBoundBottomPowerMinuteAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fBoundTopPowerAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fBoundTopPowerAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fBoundTopPowerAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fBoundTopPowerAfterBuyWhile60")
                        .HasColumnType("double");

                    b.Property<double>("fBoundTopPowerMinuteAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fBoundTopPowerMinuteAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fBoundTopPowerMinuteAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fDAngle")
                        .HasColumnType("double");

                    b.Property<double>("fDSlope")
                        .HasColumnType("double");

                    b.Property<double>("fDownPowerAfterCheck")
                        .HasColumnType("double");

                    b.Property<double>("fHAngle")
                        .HasColumnType("double");

                    b.Property<double>("fHSlope")
                        .HasColumnType("double");

                    b.Property<double>("fHogaPerTrade")
                        .HasColumnType("double");

                    b.Property<double>("fHogaRatioCur")
                        .HasColumnType("double");

                    b.Property<double>("fHogaSpeedCur")
                        .HasColumnType("double");

                    b.Property<double>("fIAngle")
                        .HasColumnType("double");

                    b.Property<double>("fISlope")
                        .HasColumnType("double");

                    b.Property<double>("fMAngle")
                        .HasColumnType("double");

                    b.Property<double>("fMSlope")
                        .HasColumnType("double");

                    b.Property<double>("fMa1hVal")
                        .HasColumnType("double");

                    b.Property<double>("fMa20mVal")
                        .HasColumnType("double");

                    b.Property<double>("fMa2hVal")
                        .HasColumnType("double");

                    b.Property<double>("fMaDownFsVal")
                        .HasColumnType("double");

                    b.Property<double>("fMaxMa1hVal")
                        .HasColumnType("double");

                    b.Property<double>("fMaxMa20mVal")
                        .HasColumnType("double");

                    b.Property<double>("fMaxMa2hVal")
                        .HasColumnType("double");

                    b.Property<double>("fMaxMaDownFsVal")
                        .HasColumnType("double");

                    b.Property<double>("fMaxPowerAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fMaxPowerAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fMaxPowerAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fMaxPowerAfterBuyWhile60")
                        .HasColumnType("double");

                    b.Property<double>("fMaxPowerMinuteAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fMaxPowerMinuteAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fMaxPowerMinuteAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fMinPowerAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fMinPowerAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fMinPowerAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fMinPowerAfterBuyWhile60")
                        .HasColumnType("double");

                    b.Property<double>("fMinPowerMinuteAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fMinPowerMinuteAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fMinPowerMinuteAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fMinusCnt07")
                        .HasColumnType("double");

                    b.Property<double>("fMinusCnt09")
                        .HasColumnType("double");

                    b.Property<double>("fOnlyDownPowerJar")
                        .HasColumnType("double");

                    b.Property<double>("fOnlyUpPowerJar")
                        .HasColumnType("double");

                    b.Property<double>("fPlusCnt07")
                        .HasColumnType("double");

                    b.Property<double>("fPlusCnt09")
                        .HasColumnType("double");

                    b.Property<double>("fPower")
                        .HasColumnType("double");

                    b.Property<double>("fPowerJar")
                        .HasColumnType("double");

                    b.Property<double>("fPowerWithOutGap")
                        .HasColumnType("double");

                    b.Property<double>("fPureBuyCur")
                        .HasColumnType("double");

                    b.Property<double>("fPureTradeCur")
                        .HasColumnType("double");

                    b.Property<double>("fRAngle")
                        .HasColumnType("double");

                    b.Property<double>("fRSlope")
                        .HasColumnType("double");

                    b.Property<double>("fSharePerHoga")
                        .HasColumnType("double");

                    b.Property<double>("fSharePerTrade")
                        .HasColumnType("double");

                    b.Property<double>("fSpeedCur")
                        .HasColumnType("double");

                    b.Property<double>("fStartGap")
                        .HasColumnType("double");

                    b.Property<double>("fTAngle")
                        .HasColumnType("double");

                    b.Property<double>("fTSlope")
                        .HasColumnType("double");

                    b.Property<double>("fTopPowerAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fTopPowerAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fTopPowerAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fTopPowerAfterBuyWhile60")
                        .HasColumnType("double");

                    b.Property<double>("fTopPowerMinuteAfterBuy")
                        .HasColumnType("double");

                    b.Property<double>("fTopPowerMinuteAfterBuyWhile10")
                        .HasColumnType("double");

                    b.Property<double>("fTopPowerMinuteAfterBuyWhile30")
                        .HasColumnType("double");

                    b.Property<double>("fTradeCur")
                        .HasColumnType("double");

                    b.Property<double>("fTradePerPure")
                        .HasColumnType("double");

                    b.Property<double>("fUpPowerAfterCheck")
                        .HasColumnType("double");

                    b.Property<long>("lMarketCap")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalBuyPrice")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalBuyPriceAfterCheck")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalBuyVolume")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalSellPrice")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalSellPriceAfterCheck")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalSellVolume")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalTradePrice")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalTradePriceAfterCheck")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalTradeVolume")
                        .HasColumnType("bigint");

                    b.Property<int>("nAccumCountRanking")
                        .HasColumnType("int");

                    b.Property<int>("nAccumUpDownCount")
                        .HasColumnType("int");

                    b.Property<int>("nBottomPriceAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBottomPriceAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBottomPriceAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBottomPriceAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nBottomPriceMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBottomPriceMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBottomPriceMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBottomTimeAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBottomTimeAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBottomTimeAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBottomTimeAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nBottomTimeMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBottomTimeMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBottomTimeMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomPriceAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomPriceAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomPriceAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomPriceAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomPriceMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomPriceMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomPriceMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomTimeAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomTimeAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomTimeAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomTimeAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomTimeMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomTimeMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBoundBottomTimeMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopPriceAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopPriceAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopPriceAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopPriceAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopPriceMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopPriceMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopPriceMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopTimeAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopTimeAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopTimeAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopTimeAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopTimeMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopTimeMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nBoundTopTimeMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nChegyulCnt")
                        .HasColumnType("int");

                    b.Property<int>("nChegyulCntAfterCheck")
                        .HasColumnType("int");

                    b.Property<int>("nCrushCnt")
                        .HasColumnType("int");

                    b.Property<int>("nCrushDownCnt")
                        .HasColumnType("int");

                    b.Property<int>("nCrushSpecialDownCnt")
                        .HasColumnType("int");

                    b.Property<int>("nCrushUpCnt")
                        .HasColumnType("int");

                    b.Property<int>("nDownCandleCnt")
                        .HasColumnType("int");

                    b.Property<int>("nDownCntMa1h")
                        .HasColumnType("int");

                    b.Property<int>("nDownCntMa20m")
                        .HasColumnType("int");

                    b.Property<int>("nDownCntMa2h")
                        .HasColumnType("int");

                    b.Property<int>("nDownTailCnt")
                        .HasColumnType("int");

                    b.Property<int>("nFakeAssistantCnt")
                        .HasColumnType("int");

                    b.Property<int>("nFakeBuyCnt")
                        .HasColumnType("int");

                    b.Property<int>("nFakeResistCnt")
                        .HasColumnType("int");

                    b.Property<int>("nFb")
                        .HasColumnType("int");

                    b.Property<int>("nFewSpeedCnt")
                        .HasColumnType("int");

                    b.Property<int>("nFewSpeedCntAfterCheck")
                        .HasColumnType("int");

                    b.Property<int>("nFs")
                        .HasColumnType("int");

                    b.Property<int>("nHogaCnt")
                        .HasColumnType("int");

                    b.Property<int>("nHogaCntAfterCheck")
                        .HasColumnType("int");

                    b.Property<int>("nMarketCapRanking")
                        .HasColumnType("int");

                    b.Property<double>("nMaxMa1hTime")
                        .HasColumnType("double");

                    b.Property<double>("nMaxMa20mTime")
                        .HasColumnType("double");

                    b.Property<double>("nMaxMa2hTime")
                        .HasColumnType("double");

                    b.Property<double>("nMaxMaDownFsTime")
                        .HasColumnType("double");

                    b.Property<int>("nMaxPriceAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nMaxPriceAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nMaxPriceAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nMaxPriceAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nMaxPriceMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nMaxPriceMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nMaxPriceMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nMaxTimeAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nMaxTimeAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nMaxTimeAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nMaxTimeAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nMaxTimeMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nMaxTimeMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nMaxTimeMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nMinPriceAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nMinPriceAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nMinPriceAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nMinPriceAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nMinPriceMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nMinPriceMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nMinPriceMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nMinTimeAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nMinTimeAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nMinTimeAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nMinTimeAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nMinTimeMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nMinTimeMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nMinTimeMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nMinuteBuyPriceRanking")
                        .HasColumnType("int");

                    b.Property<int>("nMinuteBuyVolumeRanking")
                        .HasColumnType("int");

                    b.Property<int>("nMinuteCountRanking")
                        .HasColumnType("int");

                    b.Property<int>("nMinutePowerRanking")
                        .HasColumnType("int");

                    b.Property<int>("nMinuteTotalRank")
                        .HasColumnType("int");

                    b.Property<int>("nMinuteTradePriceRanking")
                        .HasColumnType("int");

                    b.Property<int>("nMinuteTradeVolumeRanking")
                        .HasColumnType("int");

                    b.Property<int>("nMinuteUpDownRanking")
                        .HasColumnType("int");

                    b.Property<int>("nMissCnt")
                        .HasColumnType("int");

                    b.Property<int>("nMissCntAfterCheck")
                        .HasColumnType("int");

                    b.Property<int>("nNoMoveCnt")
                        .HasColumnType("int");

                    b.Property<int>("nNoMoveCntAfterCheck")
                        .HasColumnType("int");

                    b.Property<int>("nOverPrice")
                        .HasColumnType("int");

                    b.Property<int>("nPowerRanking")
                        .HasColumnType("int");

                    b.Property<int>("nPriceAfter1Sec")
                        .HasColumnType("int");

                    b.Property<int>("nPriceDownCnt")
                        .HasColumnType("int");

                    b.Property<int>("nPriceUpCnt")
                        .HasColumnType("int");

                    b.Property<int>("nRqTime")
                        .HasColumnType("int");

                    b.Property<int>("nShootingCnt")
                        .HasColumnType("int");

                    b.Property<int>("nTopPriceAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nTopPriceAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nTopPriceAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nTopPriceAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nTopPriceMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nTopPriceMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nTopPriceMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nTopTimeAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nTopTimeAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nTopTimeAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nTopTimeAfterBuyWhile60")
                        .HasColumnType("int");

                    b.Property<int>("nTopTimeMinuteAfterBuy")
                        .HasColumnType("int");

                    b.Property<int>("nTopTimeMinuteAfterBuyWhile10")
                        .HasColumnType("int");

                    b.Property<int>("nTopTimeMinuteAfterBuyWhile30")
                        .HasColumnType("int");

                    b.Property<int>("nTotalBuyPriceRanking")
                        .HasColumnType("int");

                    b.Property<int>("nTotalBuyVolumeRanking")
                        .HasColumnType("int");

                    b.Property<int>("nTotalFakeCnt")
                        .HasColumnType("int");

                    b.Property<int>("nTotalFakeMinuteCnt")
                        .HasColumnType("int");

                    b.Property<int>("nTotalRank")
                        .HasColumnType("int");

                    b.Property<int>("nTotalTradePriceRanking")
                        .HasColumnType("int");

                    b.Property<int>("nTotalTradeVolumeRanking")
                        .HasColumnType("int");

                    b.Property<int>("nTradeCnt")
                        .HasColumnType("int");

                    b.Property<int>("nUpCandleCnt")
                        .HasColumnType("int");

                    b.Property<int>("nUpCntMa1h")
                        .HasColumnType("int");

                    b.Property<int>("nUpCntMa20m")
                        .HasColumnType("int");

                    b.Property<int>("nUpCntMa2h")
                        .HasColumnType("int");

                    b.Property<int>("nUpDownCntAfterCheck")
                        .HasColumnType("int");

                    b.Property<int>("nUpTailCnt")
                        .HasColumnType("int");

                    b.Property<int>("nYesterdayEndPrice")
                        .HasColumnType("int");

                    b.Property<string>("sCodeName")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("sType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("dTradeTime", "sCode", "nBuyStrategyIdx", "nBuyStrategySequenceIdx", "nLocationOfComp");

                    b.ToTable("fakeReports");
                });

            modelBuilder.Entity("AtoTrader.DB.LocationUser", b =>
                {
                    b.Property<string>("sUserName")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("nUserLocationComp")
                        .HasColumnType("int");

                    b.HasKey("sUserName");

                    b.ToTable("locationUserDict");
                });

            modelBuilder.Entity("AtoTrader.DB.StrategyNameDict", b =>
                {
                    b.Property<string>("sStrategyName")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("nStrategyNameIdx")
                        .HasColumnType("int");

                    b.HasKey("sStrategyName");

                    b.HasIndex("nStrategyNameIdx")
                        .IsUnique();

                    b.ToTable("strategyNameDict");
                });
#pragma warning restore 612, 618
        }
    }
}
