using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtoIndicator.DB
{
    public class FakeReport
    {
        // 별도의 엔티티로 분리시킬 예정
        public DateTime dTradeTime { get; set; }
        public string sCode { get; set; }
        public string sCodeName { get; set; }
        public int nBuyStrategyGroupNum { get; set; }  // 어떤 페이크 그룹??
        public int nBuyStrategyIdx { get; set; } // 그룹 안에서 어떤 전략?
        public int nBuyStrategySequenceIdx { get; set; } // 그 전략이 몇번째??
        public int nLocationOfComp { get; set; }


        public int nRqTime { get; set; }
        public int nOverPrice { get; set; }
        public int nPriceAfter1Sec { get; set; } // 1초 후 가격
        public int nYesterdayEndPrice { get; set; }

        #region 매매블럭 정보
        // 매매슬롯 정보
        #region 3시 시점의 정보 슬리피지 파악용
        public int nHogaCntAfterCheck { get; set; }
        public int nChegyulCntAfterCheck { get; set; }
        public int nUpDownCntAfterCheck { get; set; }
        public double fUpPowerAfterCheck { get; set; }
        public double fDownPowerAfterCheck { get; set; }
        public int nNoMoveCntAfterCheck { get; set; } // 노무브카운트
        public int nFewSpeedCntAfterCheck { get; set; } // 적은거래
        public int nMissCntAfterCheck { get; set; } // 거래없음
        public long lTotalTradePriceAfterCheck { get; set; }
        public long lTotalBuyPriceAfterCheck { get; set; }
        public long lTotalSellPriceAfterCheck { get; set; }
        #endregion

        public int nFinalPrice { get; set; }
        #region 마지막 기울기 값 
        public double fFinalMSlope { get; set; }
        public double fFinalISlope { get; set; }
        public double fFinalTSlope { get; set; }
        public double fFinalHSlope { get; set; }
        public double fFinalRSlope { get; set; }
        public double fFinalDSlope { get; set; }
        
        public double fFinalMAngle { get; set; }
        public double fFinalIAngle { get; set; }
        public double fFinalTAngle { get; set; }
        public double fFinalHAngle { get; set; }
        public double fFinalRAngle { get; set; }
        public double fFinalDAngle { get; set; }
        #endregion
        #region 맥스민값
        public int nMaxPriceAfterBuy { get; set; }
        public int nMaxTimeAfterBuy { get; set; }
        public double fMaxPowerAfterBuy { get; set; }
        public int nMinPriceAfterBuy { get; set; }
        public int nMinTimeAfterBuy { get; set; }
        public double fMinPowerAfterBuy { get; set; }
        public int nBottomPriceAfterBuy { get; set; }
        public int nBottomTimeAfterBuy { get; set; }
        public double fBottomPowerAfterBuy { get; set; }
        public int nTopPriceAfterBuy { get; set; }
        public int nTopTimeAfterBuy { get; set; }
        public double fTopPowerAfterBuy { get; set; }
        public int nBoundBottomPriceAfterBuy { get; set; }
        public int nBoundBottomTimeAfterBuy { get; set; }
        public double fBoundBottomPowerAfterBuy { get; set; }
        public int nBoundTopPriceAfterBuy { get; set; }
        public int nBoundTopTimeAfterBuy { get; set; }
        public double fBoundTopPowerAfterBuy { get; set; }


        public int nMaxPriceMinuteAfterBuy { get; set; }
        public int nMaxTimeMinuteAfterBuy { get; set; }
        public double fMaxPowerMinuteAfterBuy { get; set; }
        public int nMinPriceMinuteAfterBuy { get; set; }
        public int nMinTimeMinuteAfterBuy { get; set; }
        public double fMinPowerMinuteAfterBuy { get; set; }
        public int nBottomPriceMinuteAfterBuy { get; set; }
        public int nBottomTimeMinuteAfterBuy { get; set; }
        public double fBottomPowerMinuteAfterBuy { get; set; }
        public int nTopPriceMinuteAfterBuy { get; set; }
        public int nTopTimeMinuteAfterBuy { get; set; }
        public double fTopPowerMinuteAfterBuy { get; set; }
        public int nBoundBottomPriceMinuteAfterBuy { get; set; }
        public int nBoundBottomTimeMinuteAfterBuy { get; set; }
        public double fBoundBottomPowerMinuteAfterBuy { get; set; }
        public int nBoundTopPriceMinuteAfterBuy { get; set; }
        public int nBoundTopTimeMinuteAfterBuy { get; set; }
        public double fBoundTopPowerMinuteAfterBuy { get; set; }

        public int nMaxPriceAfterBuyWhile10 { get; set; }
        public int nMaxTimeAfterBuyWhile10 { get; set; }
        public double fMaxPowerAfterBuyWhile10 { get; set; }
        public int nMinPriceAfterBuyWhile10 { get; set; }
        public int nMinTimeAfterBuyWhile10 { get; set; }
        public double fMinPowerAfterBuyWhile10 { get; set; }
        public int nBottomPriceAfterBuyWhile10 { get; set; }
        public int nBottomTimeAfterBuyWhile10 { get; set; }
        public double fBottomPowerAfterBuyWhile10 { get; set; }
        public int nTopPriceAfterBuyWhile10 { get; set; }
        public int nTopTimeAfterBuyWhile10 { get; set; }
        public double fTopPowerAfterBuyWhile10 { get; set; }
        public int nBoundBottomPriceAfterBuyWhile10 { get; set; }
        public int nBoundBottomTimeAfterBuyWhile10 { get; set; }
        public double fBoundBottomPowerAfterBuyWhile10 { get; set; }
        public int nBoundTopPriceAfterBuyWhile10 { get; set; }
        public int nBoundTopTimeAfterBuyWhile10 { get; set; }
        public double fBoundTopPowerAfterBuyWhile10 { get; set; }


        public int nMaxPriceAfterBuyWhile30 { get; set; }
        public int nMaxTimeAfterBuyWhile30 { get; set; }
        public double fMaxPowerAfterBuyWhile30 { get; set; }
        public int nMinPriceAfterBuyWhile30 { get; set; }
        public int nMinTimeAfterBuyWhile30 { get; set; }
        public double fMinPowerAfterBuyWhile30 { get; set; }
        public int nBottomPriceAfterBuyWhile30 { get; set; }
        public int nBottomTimeAfterBuyWhile30 { get; set; }
        public double fBottomPowerAfterBuyWhile30 { get; set; }
        public int nTopPriceAfterBuyWhile30 { get; set; }
        public int nTopTimeAfterBuyWhile30 { get; set; }
        public double fTopPowerAfterBuyWhile30 { get; set; }
        public int nBoundBottomPriceAfterBuyWhile30 { get; set; }
        public int nBoundBottomTimeAfterBuyWhile30 { get; set; }
        public double fBoundBottomPowerAfterBuyWhile30 { get; set; }
        public int nBoundTopPriceAfterBuyWhile30 { get; set; }
        public int nBoundTopTimeAfterBuyWhile30 { get; set; }
        public double fBoundTopPowerAfterBuyWhile30 { get; set; }


        public int nMaxPriceAfterBuyWhile60 { get; set; }
        public int nMaxTimeAfterBuyWhile60 { get; set; }
        public double fMaxPowerAfterBuyWhile60 { get; set; }
        public int nMinPriceAfterBuyWhile60 { get; set; }
        public int nMinTimeAfterBuyWhile60 { get; set; }
        public double fMinPowerAfterBuyWhile60 { get; set; }
        public int nBottomPriceAfterBuyWhile60 { get; set; }
        public int nBottomTimeAfterBuyWhile60 { get; set; }
        public double fBottomPowerAfterBuyWhile60 { get; set; }
        public int nTopPriceAfterBuyWhile60 { get; set; }
        public int nTopTimeAfterBuyWhile60 { get; set; }
        public double fTopPowerAfterBuyWhile60 { get; set; }
        public int nBoundBottomPriceAfterBuyWhile60 { get; set; }
        public int nBoundBottomTimeAfterBuyWhile60 { get; set; }
        public double fBoundBottomPowerAfterBuyWhile60 { get; set; }
        public int nBoundTopPriceAfterBuyWhile60 { get; set; }
        public int nBoundTopTimeAfterBuyWhile60 { get; set; }
        public double fBoundTopPowerAfterBuyWhile60 { get; set; }


        public int nMaxPriceMinuteAfterBuyWhile10 { get; set; }
        public int nMaxTimeMinuteAfterBuyWhile10 { get; set; }
        public double fMaxPowerMinuteAfterBuyWhile10 { get; set; }
        public int nMinPriceMinuteAfterBuyWhile10 { get; set; }
        public int nMinTimeMinuteAfterBuyWhile10 { get; set; }
        public double fMinPowerMinuteAfterBuyWhile10 { get; set; }
        public int nBottomPriceMinuteAfterBuyWhile10 { get; set; }
        public int nBottomTimeMinuteAfterBuyWhile10 { get; set; }
        public double fBottomPowerMinuteAfterBuyWhile10 { get; set; }
        public int nTopPriceMinuteAfterBuyWhile10 { get; set; }
        public int nTopTimeMinuteAfterBuyWhile10 { get; set; }
        public double fTopPowerMinuteAfterBuyWhile10 { get; set; }
        public int nBoundBottomPriceMinuteAfterBuyWhile10 { get; set; }
        public int nBoundBottomTimeMinuteAfterBuyWhile10 { get; set; }
        public double fBoundBottomPowerMinuteAfterBuyWhile10 { get; set; }
        public int nBoundTopPriceMinuteAfterBuyWhile10 { get; set; }
        public int nBoundTopTimeMinuteAfterBuyWhile10 { get; set; }
        public double fBoundTopPowerMinuteAfterBuyWhile10 { get; set; }

        public int nMaxPriceMinuteAfterBuyWhile30 { get; set; }
        public int nMaxTimeMinuteAfterBuyWhile30 { get; set; }
        public double fMaxPowerMinuteAfterBuyWhile30 { get; set; }
        public int nMinPriceMinuteAfterBuyWhile30 { get; set; }
        public int nMinTimeMinuteAfterBuyWhile30 { get; set; }
        public double fMinPowerMinuteAfterBuyWhile30 { get; set; }
        public int nBottomPriceMinuteAfterBuyWhile30 { get; set; }
        public int nBottomTimeMinuteAfterBuyWhile30 { get; set; }
        public double fBottomPowerMinuteAfterBuyWhile30 { get; set; }
        public int nTopPriceMinuteAfterBuyWhile30 { get; set; }
        public int nTopTimeMinuteAfterBuyWhile30 { get; set; }
        public double fTopPowerMinuteAfterBuyWhile30 { get; set; }
        public int nBoundBottomPriceMinuteAfterBuyWhile30 { get; set; }
        public int nBoundBottomTimeMinuteAfterBuyWhile30 { get; set; }
        public double fBoundBottomPowerMinuteAfterBuyWhile30 { get; set; }
        public int nBoundTopPriceMinuteAfterBuyWhile30 { get; set; }
        public int nBoundTopTimeMinuteAfterBuyWhile30 { get; set; }
        public double fBoundTopPowerMinuteAfterBuyWhile30 { get; set; }
        #endregion
        public int n2MinPrice { get; set; }
        public double f2MinPower { get; set; }
        public int n3MinPrice { get; set; }
        public double f3MinPower { get; set; }
        public int n5MinPrice { get; set; }
        public double f5MinPower { get; set; }
        public int n10MinPrice { get; set; }
        public double f10MinPower { get; set; }
        public int n15MinPrice { get; set; }
        public double f15MinPower { get; set; }
        public int n20MinPrice { get; set; }
        public double f20MinPower { get; set; }
        public int n30MinPrice { get; set; }
        public double f30MinPower { get; set; }
        public int n50MinPrice { get; set; }
        public double f50MinPower { get; set; }
        #endregion 매매블럭 정보

        #region 장시작전 호가데이터
        public int nStopHogaCnt { get; set; }
        public int nStopUpDownCnt { get; set; }
        public int nStopFirstPrice { get; set; }
        public int nStopMaxPrice { get; set; }
        public int nStopMinPrice { get; set; }
        public double fStopMaxPower { get; set; }
        public double fStopMinPower { get; set; }
        public double fStopMaxMinDiff { get; set; }
        #endregion

        #region 개인구조체 정보
        public int nFirstVolume { get; set; }
        public long lFirstPrice { get; set; }
        public double fStartGap { get; set; }
        public string sType { get; set; }
        public double fPowerWithOutGap { get; set; }
        public double fPower { get; set; }
        public double fPlusCnt07 { get; set; }
        public double fMinusCnt07 { get; set; }
        public double fPlusCnt09 { get; set; }
        public double fMinusCnt09 { get; set; }
        public double fPowerJar { get; set; }
        public double fOnlyDownPowerJar { get; set; }
        public double fOnlyUpPowerJar { get; set; }
        #region 거래정도
        public int nChegyulCnt { get; set; }// 체결카운트
        public int nHogaCnt { get; set; } // 호가카운트
        public int nNoMoveCnt { get; set; } // 노무브카운트
        public int nFewSpeedCnt { get; set; } // 적은거래
        public int nMissCnt { get; set; } // 거래없음
        #endregion
        #region 수량
        public long lTotalTradeVolume { get; set; }
        public long lTotalBuyVolume { get; set; }
        public long lTotalSellVolume { get; set; }
        #endregion
        public int nAccumUpDownCount { get; set; }
        public double fAccumUpPower { get; set; }
        public double fAccumDownPower { get; set; }
        #region 대금
        public long lTotalTradePrice { get; set; }
        public long lTotalBuyPrice { get; set; }
        public long lTotalSellPrice { get; set; }
        public long lMarketCap { get; set; }
        #endregion

        public double fPositiveStickPower { get; set; }
        public double fNegativeStickPower { get; set; }

        #region 랭크
        public int nAccumCountRanking { get; set; }
        public int nMarketCapRanking { get; set; }
        public int nPowerRanking { get; set; }
        public int nTotalBuyPriceRanking { get; set; }
        public int nTotalBuyVolumeRanking { get; set; }
        public int nTotalTradePriceRanking { get; set; }
        public int nTotalTradeVolumeRanking { get; set; }
        public int nTotalRank { get; set; }
        public int nSummationRankMove { get; set; }
        #region 분당
        public int nMinuteTotalRank { get; set; }
        public int nMinuteTradePriceRanking { get; set; }
        public int nMinuteTradeVolumeRanking { get; set; }
        public int nMinuteBuyPriceRanking { get; set; }
        public int nMinuteBuyVolumeRanking { get; set; }
        public int nMinutePowerRanking { get; set; }
        public int nMinuteCountRanking { get; set; }
        public int nMinuteUpDownRanking { get; set; }

        public int nRankHold10 { get; set; }
        public int nRankHold20 { get; set; }
        public int nRankHold50 { get; set; }
        public int nRankHold100 { get; set; }
        public int nRankHold200 { get; set; }
        public int nRankHold500 { get; set; }
        public int nRankHold1000 { get; set; }

        #endregion
        #endregion

        #region 페이크갯수
        public int nTradeCnt { get; set; }
        public int nFakeBuyCnt { get; set; }
        public int nFakeAssistantCnt { get; set; }
        public int nFakeResistCnt { get; set; }
        public int nPriceUpCnt { get; set; }
        public int nPriceDownCnt { get; set; }

        public int nRealBuyMinuteCnt { get; set; }
        public int nFakeBuyMinuteCnt { get; set; }
        public int nFakeAssistantMinuteCnt { get; set; }
        public int nFakeResistMinuteCnt { get; set; }
        public int nPriceUpMinuteCnt { get; set; }
        public int nPriceDownMinuteCnt { get; set; }

        public int nFakeBuyUpperCnt { get; set; }
        public int nFakeAssistantUpperCnt { get; set; }
        public int nFakeResistUpperCnt { get; set; }
        public int nPriceUpUpperCnt { get; set; }
        public int nPriceDownUpperCnt { get; set; }

        public int nTotalFakeCnt { get; set; }
        public int nTotalFakeMinuteCnt { get; set; }
        #endregion
        #region 분봉 움직임
        public int nUpCandleCnt { get; set; } // nOnePerCandleCnt
        public int nTwoPerCandleCnt { get; set; }
        public int nShootingCnt { get; set; } // nThreePerCandleCnt
        public int nFourPerCandleCnt { get; set; }
        public int nDownCandleCnt { get; set; }
        public int nUpTailCnt { get; set; }
        public int nDownTailCnt { get; set; }
        #endregion
        public int nFs { get; set; }
        public int nFb { get; set; }
        public double fTs { get; set; }
        public double fTradeRatioCompared { get; set; }
        public int nTodayMinPrice { get; set; }
        public int nTodayMinTime { get; set; }
        public double fTodayMinPower { get; set; }
        public int nTodayMaxPrice { get; set; }
        public int nTodayMaxTime { get; set; }
        public double fTodayMaxPower { get; set; }
        public int nPrevLastFs { get; set; }
        public int nPrevStartFs { get; set; }
        public int nPrevMaxFs { get; set; }
        public int nPrevMinFs { get; set; }
        public int nPrevVolume { get; set; }
        public int nCurLastFs { get; set; }
        public int nCurStartFs { get; set; }
        public int nCurMaxFs { get; set; }
        public int nCurMinFs { get; set; }
        public int nCurVolume { get; set; }
        public int nEveryCount { get; set; }
        #region 현재상태
        public double fSpeedCur { get; set; }
        public double fHogaSpeedCur { get; set; }
        public double fTradeCur { get; set; }
        public double fPureTradeCur { get; set; }
        public double fPureBuyCur { get; set; }
        public double fPriceMoveCur { get; set; }
        public double fPriceUpMoveCur { get; set; }
        public double fHogaRatioCur { get; set; }
        public double fSharePerHoga { get; set; }
        public double fSharePerTrade { get; set; }
        public double fHogaPerTrade { get; set; }
        public double fTradePerPure { get; set; }
        #endregion
        #region 이동평균선
        #region 이동평균선 값
        public double fMa20mDiff { get; set; }
        public double fMa1hDiff { get; set; }
        public double fMa2hDiff { get; set; }
        public double fMa20mCurDiff { get; set; }
        public double fMa1hCurDiff { get; set; }
        public double fMa2hCurDiff { get; set; }
        public double fGapMa20mDiff { get; set; }
        public double fGapMa1hDiff { get; set; }
        public double fGapMa2hDiff { get; set; }
        public double fGapMa20mCurDiff { get; set; }
        public double fGapMa1hCurDiff { get; set; }
        public double fGapMa2hCurDiff { get; set; }
        #endregion 
        public int nDownCntMa20m { get; set; }
        public int nDownCntMa1h { get; set; }
        public int nDownCntMa2h { get; set; }
        public int nUpCntMa20m { get; set; }
        public int nUpCntMa1h { get; set; }
        public int nUpCntMa2h { get; set; }
        public int nCandleTwoOverRealCnt { get; set; }
        public int nCandleTwoOverRealNoLeafCnt { get; set; }
        public double fMaDownFsVal { get; set; }

        public double fMa20mVal { get; set; }
        public double fMa1hVal { get; set; }
        public double fMa2hVal { get; set; }
        public double fMaxMaDownFsVal { get; set; }
        public double fMaxMa20mVal { get; set; }
        public double fMaxMa1hVal { get; set; }
        public double fMaxMa2hVal { get; set; }
        public int nMaxMaDownFsTime { get; set; }
        public int nMaxMa20mTime { get; set; }
        public int nMaxMa1hTime { get; set; }
        public int nMaxMa2hTime { get; set; }
        #endregion
        #region 각도
        #region 기울기 값 
        public double fMSlope { get; set; }
        public double fISlope { get; set; }
        public double fTSlope { get; set; }
        public double fHSlope { get; set; }
        public double fRSlope { get; set; }
        public double fDSlope { get; set; }
        #endregion
        public double fMAngle { get; set; } // 맥스각도
        public double fIAngle { get; set; } // 초기각도
        public double fTAngle { get; set; }
        public double fHAngle { get; set; }
        public double fRAngle { get; set; }
        public double fDAngle { get; set; }
        #endregion
        #region 전고점
        public int nCrushCnt { get; set; }
        public int nCrushUpCnt { get; set; }
        public int nCrushDownCnt { get; set; }
        public int nCrushSpecialDownCnt { get; set; }
        #endregion
        #endregion 개인구조체 정보

        public int nRealBuyAIPass { get; set; }
        public int nFakeBuyAIPass { get; set; }
        public int nEveryBuyAIPass { get; set; }
        public double fAIScore { get; set; }
        public double fAIScoreJar { get; set; }
        public double fAIScoreJarDegree { get; set; }
        public int nAIVersion { get; set; }
        public FakeReport()
        {
            dTradeTime = DateTime.Today;
            sCode = "";
            sCodeName = "";
            nBuyStrategyGroupNum = 0;
            nBuyStrategyIdx = 0;
            nBuyStrategySequenceIdx = 0;
            nLocationOfComp = 0;

            nRqTime = 0;
            nOverPrice = 0;
            nPriceAfter1Sec = 0;
            nYesterdayEndPrice = 0;

            nStopHogaCnt = 0;
            nStopUpDownCnt = 0;
            nStopFirstPrice = 0;
            nStopMaxPrice = 0;
            nStopMinPrice = 0;
            fStopMaxPower = 0;
            fStopMinPower = 0;
            fStopMaxMinDiff = 0;

            #region 매매블럭 정보
            // 매매슬롯 정보
            #region 3시 시점의 정보 슬리피지 파악용
            nHogaCntAfterCheck = 0;
            nChegyulCntAfterCheck = 0;
            nUpDownCntAfterCheck = 0;
            fUpPowerAfterCheck = 0;
            fDownPowerAfterCheck = 0;
            nNoMoveCntAfterCheck = 0;
            nFewSpeedCntAfterCheck = 0;
            nMissCntAfterCheck = 0;
            lTotalTradePriceAfterCheck = 0;
            lTotalBuyPriceAfterCheck = 0;
            lTotalSellPriceAfterCheck = 0;
            #endregion

            nFinalPrice = 0;
            #region 마지막기울기 값 
            fFinalMSlope = 0;
            fFinalISlope = 0;
            fFinalTSlope = 0;
            fFinalHSlope = 0;
            fFinalRSlope = 0;
            fFinalDSlope = 0;
            #endregion
            fFinalMAngle = 0;
            fFinalIAngle = 0;
            fFinalTAngle = 0;
            fFinalHAngle = 0;
            fFinalRAngle = 0;
            fFinalDAngle = 0;
            
            #region 맥스민값
            nMaxPriceAfterBuy = 0;
            nMaxTimeAfterBuy = 0;
            fMaxPowerAfterBuy = 0;
            nMinPriceAfterBuy = 0;
            nMinTimeAfterBuy = 0;
            fMinPowerAfterBuy = 0;
            nBottomPriceAfterBuy = 0;
            nBottomTimeAfterBuy = 0;
            fBottomPowerAfterBuy = 0;
            nTopPriceAfterBuy = 0;
            nTopTimeAfterBuy = 0;
            fTopPowerAfterBuy = 0;
            nBoundBottomPriceAfterBuy = 0;
            nBoundBottomTimeAfterBuy = 0;
            fBoundBottomPowerAfterBuy = 0;
            nBoundTopPriceAfterBuy = 0;
            nBoundTopTimeAfterBuy = 0;
            fBoundTopPowerAfterBuy = 0;


            nMaxPriceMinuteAfterBuy = 0;
            nMaxTimeMinuteAfterBuy = 0;
            fMaxPowerMinuteAfterBuy = 0;
            nMinPriceMinuteAfterBuy = 0;
            nMinTimeMinuteAfterBuy = 0;
            fMinPowerMinuteAfterBuy = 0;
            nBottomPriceMinuteAfterBuy = 0;
            nBottomTimeMinuteAfterBuy = 0;
            fBottomPowerMinuteAfterBuy = 0;
            nTopPriceMinuteAfterBuy = 0;
            nTopTimeMinuteAfterBuy = 0;
            fTopPowerMinuteAfterBuy = 0;
            nBoundBottomPriceMinuteAfterBuy = 0;
            nBoundBottomTimeMinuteAfterBuy = 0;
            fBoundBottomPowerMinuteAfterBuy = 0;
            nBoundTopPriceMinuteAfterBuy = 0;
            nBoundTopTimeMinuteAfterBuy = 0;
            fBoundTopPowerMinuteAfterBuy = 0;

            nMaxPriceAfterBuyWhile10 = 0;
            nMaxTimeAfterBuyWhile10 = 0;
            fMaxPowerAfterBuyWhile10 = 0;
            nMinPriceAfterBuyWhile10 = 0;
            nMinTimeAfterBuyWhile10 = 0;
            fMinPowerAfterBuyWhile10 = 0;
            nBottomPriceAfterBuyWhile10 = 0;
            nBottomTimeAfterBuyWhile10 = 0;
            fBottomPowerAfterBuyWhile10 = 0;
            nTopPriceAfterBuyWhile10 = 0;
            nTopTimeAfterBuyWhile10 = 0;
            fTopPowerAfterBuyWhile10 = 0;
            nBoundBottomPriceAfterBuyWhile10 = 0;
            nBoundBottomTimeAfterBuyWhile10 = 0;
            fBoundBottomPowerAfterBuyWhile10 = 0;
            nBoundTopPriceAfterBuyWhile10 = 0;
            nBoundTopTimeAfterBuyWhile10 = 0;
            fBoundTopPowerAfterBuyWhile10 = 0;


            nMaxPriceAfterBuyWhile30 = 0;
            nMaxTimeAfterBuyWhile30 = 0;
            fMaxPowerAfterBuyWhile30 = 0;
            nMinPriceAfterBuyWhile30 = 0;
            nMinTimeAfterBuyWhile30 = 0;
            fMinPowerAfterBuyWhile30 = 0;
            nBottomPriceAfterBuyWhile30 = 0;
            nBottomTimeAfterBuyWhile30 = 0;
            fBottomPowerAfterBuyWhile30 = 0;
            nTopPriceAfterBuyWhile30 = 0;
            nTopTimeAfterBuyWhile30 = 0;
            fTopPowerAfterBuyWhile30 = 0;
            nBoundBottomPriceAfterBuyWhile30 = 0;
            nBoundBottomTimeAfterBuyWhile30 = 0;
            fBoundBottomPowerAfterBuyWhile30 = 0;
            nBoundTopPriceAfterBuyWhile30 = 0;
            nBoundTopTimeAfterBuyWhile30 = 0;
            fBoundTopPowerAfterBuyWhile30 = 0;


            nMaxPriceAfterBuyWhile60 = 0;
            nMaxTimeAfterBuyWhile60 = 0;
            fMaxPowerAfterBuyWhile60 = 0;
            nMinPriceAfterBuyWhile60 = 0;
            nMinTimeAfterBuyWhile60 = 0;
            fMinPowerAfterBuyWhile60 = 0;
            nBottomPriceAfterBuyWhile60 = 0;
            nBottomTimeAfterBuyWhile60 = 0;
            fBottomPowerAfterBuyWhile60 = 0;
            nTopPriceAfterBuyWhile60 = 0;
            nTopTimeAfterBuyWhile60 = 0;
            fTopPowerAfterBuyWhile60 = 0;
            nBoundBottomPriceAfterBuyWhile60 = 0;
            nBoundBottomTimeAfterBuyWhile60 = 0;
            fBoundBottomPowerAfterBuyWhile60 = 0;
            nBoundTopPriceAfterBuyWhile60 = 0;
            nBoundTopTimeAfterBuyWhile60 = 0;
            fBoundTopPowerAfterBuyWhile60 = 0;


            nMaxPriceMinuteAfterBuyWhile10 = 0;
            nMaxTimeMinuteAfterBuyWhile10 = 0;
            fMaxPowerMinuteAfterBuyWhile10 = 0;
            nMinPriceMinuteAfterBuyWhile10 = 0;
            nMinTimeMinuteAfterBuyWhile10 = 0;
            fMinPowerMinuteAfterBuyWhile10 = 0;
            nBottomPriceMinuteAfterBuyWhile10 = 0;
            nBottomTimeMinuteAfterBuyWhile10 = 0;
            fBottomPowerMinuteAfterBuyWhile10 = 0;
            nTopPriceMinuteAfterBuyWhile10 = 0;
            nTopTimeMinuteAfterBuyWhile10 = 0;
            fTopPowerMinuteAfterBuyWhile10 = 0;
            nBoundBottomPriceMinuteAfterBuyWhile10 = 0;
            nBoundBottomTimeMinuteAfterBuyWhile10 = 0;
            fBoundBottomPowerMinuteAfterBuyWhile10 = 0;
            nBoundTopPriceMinuteAfterBuyWhile10 = 0;
            nBoundTopTimeMinuteAfterBuyWhile10 = 0;
            fBoundTopPowerMinuteAfterBuyWhile10 = 0;

            nMaxPriceMinuteAfterBuyWhile30 = 0;
            nMaxTimeMinuteAfterBuyWhile30 = 0;
            fMaxPowerMinuteAfterBuyWhile30 = 0;
            nMinPriceMinuteAfterBuyWhile30 = 0;
            nMinTimeMinuteAfterBuyWhile30 = 0;
            fMinPowerMinuteAfterBuyWhile30 = 0;
            nBottomPriceMinuteAfterBuyWhile30 = 0;
            nBottomTimeMinuteAfterBuyWhile30 = 0;
            fBottomPowerMinuteAfterBuyWhile30 = 0;
            nTopPriceMinuteAfterBuyWhile30 = 0;
            nTopTimeMinuteAfterBuyWhile30 = 0;
            fTopPowerMinuteAfterBuyWhile30 = 0;
            nBoundBottomPriceMinuteAfterBuyWhile30 = 0;
            nBoundBottomTimeMinuteAfterBuyWhile30 = 0;
            fBoundBottomPowerMinuteAfterBuyWhile30 = 0;
            nBoundTopPriceMinuteAfterBuyWhile30 = 0;
            nBoundTopTimeMinuteAfterBuyWhile30 = 0;
            fBoundTopPowerMinuteAfterBuyWhile30 = 0;
            #endregion
            #endregion 매매블럭 정보

            #region 개인구조체 정보
            nFirstVolume = 0;
            lFirstPrice = 0;
            fStartGap = 0;
            sType = "";
            fPowerWithOutGap = 0;
            fPower = 0;
            fPlusCnt07 = 0;
            fMinusCnt07 = 0;
            fPlusCnt09 = 0;
            fMinusCnt09 = 0;
            fPowerJar = 0;
            fOnlyDownPowerJar = 0;
            fOnlyUpPowerJar = 0;
            #region 거래정도
            nChegyulCnt = 0;
            nHogaCnt = 0;
            nNoMoveCnt = 0;
            nFewSpeedCnt = 0;
            nMissCnt = 0;
            #endregion
            #region 수량
            lTotalTradeVolume = 0;
            lTotalBuyVolume = 0;
            lTotalSellVolume = 0;
            #endregion
            nAccumUpDownCount = 0;
            fAccumUpPower = 0;
            fAccumDownPower = 0;
            #region 대금
            lTotalTradePrice = 0;
            lTotalBuyPrice = 0;
            lTotalSellPrice = 0;
            lMarketCap = 0;
            #endregion

            fPositiveStickPower = 0;
            fNegativeStickPower = 0;

            #region 랭크
            nAccumCountRanking = 0;
            nMarketCapRanking = 0;
            nPowerRanking = 0;
            nTotalBuyPriceRanking = 0;
            nTotalBuyVolumeRanking = 0;
            nTotalTradePriceRanking = 0;
            nTotalTradeVolumeRanking = 0;
            nTotalRank = 0;
            nSummationRankMove = 0;
            #region 분당
            nMinuteTotalRank = 0;
            nMinuteTradePriceRanking = 0;
            nMinuteTradeVolumeRanking = 0;
            nMinuteBuyPriceRanking = 0;
            nMinuteBuyVolumeRanking = 0;
            nMinutePowerRanking = 0;
            nMinuteCountRanking = 0;
            nMinuteUpDownRanking = 0;

            nRankHold10 = 0;
            nRankHold20 = 0;
            nRankHold50 = 0;
            nRankHold100 = 0;
            nRankHold200 = 0;
            nRankHold500 = 0;
            nRankHold1000 = 0;

            #endregion
            #endregion

            #region 페이크갯수
            nTradeCnt = 0;
            nFakeBuyCnt = 0;
            nFakeAssistantCnt = 0;
            nFakeResistCnt = 0;
            nPriceUpCnt = 0;
            nPriceDownCnt = 0;

            nRealBuyMinuteCnt = 0;
            nFakeBuyMinuteCnt = 0;
            nFakeAssistantMinuteCnt = 0;
            nFakeResistMinuteCnt = 0;
            nPriceUpMinuteCnt = 0;
            nPriceDownMinuteCnt = 0;

            nFakeBuyUpperCnt = 0;
            nFakeAssistantUpperCnt = 0;
            nFakeResistUpperCnt = 0;
            nPriceUpUpperCnt = 0;
            nPriceDownUpperCnt = 0;

            nTotalFakeCnt = 0;
            nTotalFakeMinuteCnt = 0;
            #endregion
            #region 분봉 움직임
            nUpCandleCnt = 0;
            nTwoPerCandleCnt = 0;
            nShootingCnt = 0;
            nFourPerCandleCnt = 0;
            nDownCandleCnt = 0;
            nUpTailCnt = 0;
            nDownTailCnt = 0;
            #endregion
            nFs = 0;
            nFb = 0;
            fTs = 0;
            fTradeRatioCompared = 0;
            nTodayMinPrice = 0;
            nTodayMinTime = 0;
            fTodayMinPower = 0;
            nTodayMaxPrice = 0;
            nTodayMaxTime = 0;
            fTodayMaxPower = 0;
            nPrevLastFs = 0;
            nPrevStartFs = 0;
            nPrevMaxFs = 0;
            nPrevMinFs = 0;
            nPrevVolume = 0;
            nCurLastFs = 0;
            nCurStartFs = 0;
            nCurMaxFs = 0;
            nCurMinFs = 0;
            nCurVolume = 0;
            nEveryCount = 0;
            #region 현재상태
            fSpeedCur = 0;
            fHogaSpeedCur = 0;
            fTradeCur = 0;
            fPureTradeCur = 0;
            fPureBuyCur = 0;
            fPriceMoveCur = 0;
            fPriceUpMoveCur = 0;
            fHogaRatioCur = 0;
            fSharePerHoga = 0;
            fSharePerTrade = 0;
            fHogaPerTrade = 0;
            fTradePerPure = 0;
            #endregion
            #region 이동평균선
            #region 이동평균선 값
            fMa20mDiff = 0;
            fMa1hDiff = 0;
            fMa2hDiff = 0;
            fMa20mCurDiff = 0;
            fMa1hCurDiff = 0;
            fMa2hCurDiff = 0;
            fGapMa20mDiff = 0;
            fGapMa1hDiff = 0;
            fGapMa2hDiff = 0;
            fGapMa20mCurDiff = 0;
            fGapMa1hCurDiff = 0;
            fGapMa2hCurDiff = 0;
            #endregion
            nDownCntMa20m = 0;
            nDownCntMa1h = 0;
            nDownCntMa2h = 0;
            nUpCntMa20m = 0;
            nUpCntMa1h = 0;
            nUpCntMa2h = 0;

            #endregion
            #region 각도
            #region 기울기 값 
            fMSlope = 0;
            fISlope = 0;
            fTSlope = 0;
            fHSlope = 0;
            fRSlope = 0;
            fDSlope = 0;
            #endregion
            fMAngle = 0;
            fIAngle = 0;
            fTAngle = 0;
            fHAngle = 0;
            fRAngle = 0;
            fDAngle = 0;
            #endregion
            #region 전고점
            nCrushCnt = 0;
            nCrushUpCnt = 0;
            nCrushDownCnt = 0;
            nCrushSpecialDownCnt = 0;
            #endregion
            #endregion

            nRealBuyAIPass = 0;
            nFakeBuyAIPass = 0;
            nEveryBuyAIPass = 0;
            fAIScore = 0;
            fAIScoreJar = 0;
            fAIScoreJarDegree = 0;

            nCandleTwoOverRealCnt = 0;
            nCandleTwoOverRealNoLeafCnt = 0;
            fMaDownFsVal = 0;
            fMa20mVal = 0;
            fMa1hVal = 0;
            fMa2hVal = 0;
            fMaxMaDownFsVal = 0;
            fMaxMa20mVal = 0;
            fMaxMa1hVal = 0;
            fMaxMa2hVal = 0;
            nMaxMaDownFsTime = 0;
            nMaxMa20mTime = 0;
            nMaxMa1hTime = 0;
            nMaxMa2hTime = 0;

            nAIVersion = 0;

            n2MinPrice = 0;
            f2MinPower = 0;
            n3MinPrice = 0;
            f3MinPower = 0;
            n5MinPrice = 0;
            f5MinPower = 0;
            n10MinPrice = 0;
            f10MinPower = 0;
            n15MinPrice = 0;
            f15MinPower = 0;
            n20MinPrice = 0;
            f20MinPower = 0;
            n30MinPrice = 0;
            f30MinPower = 0;
            n50MinPrice = 0;
            f50MinPower = 0;
        }
    }
}


