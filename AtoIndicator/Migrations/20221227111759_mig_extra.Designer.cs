﻿// <auto-generated />
using System;
using AtoTrader.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AtoTrader.Migrations
{
    [DbContext(typeof(myDbContext))]
    [Migration("20221227111759_mig_extra")]
    partial class mig_extra
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("AtoTrader.DB.TradeResult", b =>
                {
                    b.Property<DateTime>("dTradeTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("sCode")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("nBuyStrategyIdx")
                        .HasColumnType("int");

                    b.Property<int>("nBuyStrategySequenceIdx")
                        .HasColumnType("int");

                    b.Property<double>("fDAngle")
                        .HasColumnType("double");

                    b.Property<double>("fDSlope")
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

                    b.Property<double>("fMinusCnt07")
                        .HasColumnType("double");

                    b.Property<double>("fMinusCnt09")
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

                    b.Property<double>("fProfit")
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

                    b.Property<double>("fTradeCur")
                        .HasColumnType("double");

                    b.Property<double>("fTradePerPure")
                        .HasColumnType("double");

                    b.Property<bool>("isAllBuyed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("isAllSelled")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("lMarketCap")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalBuyPrice")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalBuyVolume")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalSellPrice")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalSellVolume")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalTradePrice")
                        .HasColumnType("bigint");

                    b.Property<long>("lTotalTradeVolume")
                        .HasColumnType("bigint");

                    b.Property<int>("nAccumCountRanking")
                        .HasColumnType("int");

                    b.Property<int>("nAccumUpDownCount")
                        .HasColumnType("int");

                    b.Property<int>("nBuyEndTime")
                        .HasColumnType("int");

                    b.Property<int>("nBuyPrice")
                        .HasColumnType("int");

                    b.Property<int>("nBuyVolume")
                        .HasColumnType("int");

                    b.Property<int>("nCandleTwoOverRealCnt")
                        .HasColumnType("int");

                    b.Property<int>("nCandleTwoOverRealNoLeafCnt")
                        .HasColumnType("int");

                    b.Property<int>("nChegyulCnt")
                        .HasColumnType("int");

                    b.Property<int>("nCrushCnt")
                        .HasColumnType("int");

                    b.Property<int>("nCrushDownCnt")
                        .HasColumnType("int");

                    b.Property<int>("nCrushSpecialDownCnt")
                        .HasColumnType("int");

                    b.Property<int>("nCrushUpCnt")
                        .HasColumnType("int");

                    b.Property<int>("nDeathPrice")
                        .HasColumnType("int");

                    b.Property<int>("nDeathTime")
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

                    b.Property<int>("nFewSpeedCnt")
                        .HasColumnType("int");

                    b.Property<int>("nHogaCnt")
                        .HasColumnType("int");

                    b.Property<int>("nMarketCapRanking")
                        .HasColumnType("int");

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

                    b.Property<int>("nNoMoveCnt")
                        .HasColumnType("int");

                    b.Property<int>("nOrderPrice")
                        .HasColumnType("int");

                    b.Property<int>("nOriginOrderPrice")
                        .HasColumnType("int");

                    b.Property<int>("nPowerRanking")
                        .HasColumnType("int");

                    b.Property<int>("nPriceDownCnt")
                        .HasColumnType("int");

                    b.Property<int>("nPriceUpCnt")
                        .HasColumnType("int");

                    b.Property<int>("nRqTime")
                        .HasColumnType("int");

                    b.Property<int>("nShootingCnt")
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

                    b.Property<int>("nUpTailCnt")
                        .HasColumnType("int");

                    b.Property<string>("sBuyStrategyName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("sCodeName")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("sExtraVariables")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("sSellStrategyMsg")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("sType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("dTradeTime", "sCode", "nBuyStrategyIdx", "nBuyStrategySequenceIdx");

                    b.ToTable("tradeResult");
                });
#pragma warning restore 612, 618
        }
    }
}
