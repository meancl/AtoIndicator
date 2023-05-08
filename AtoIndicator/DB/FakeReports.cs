using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtoTrader.DB
{
    public class FakeReports
    {
        // 별도의 엔티티로 분리시킬 예정
        public DateTime dTradeTime { get; set; }
        public string sCode { get; set; }
        public string sCodeName { get; set; }
        public int nBuyStrategyIdx { get; set; }
        public int nBuyStrategySequenceIdx { get; set; }
        public int nLocationOfComp { get; set; }

        public int nRqTime { get; set; }
        public int nOverPrice { get; set; }

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
        #endregion 매매블럭 정보

        #region 개인구조체 정보
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
        public int nTradeCnt { get; set; }// 매매횟수
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
        #region 랭크
        public int nAccumCountRanking { get; set; }
        public int nMarketCapRanking { get; set; }
        public int nPowerRanking { get; set; }
        public int nTotalBuyPriceRanking { get; set; }
        public int nTotalBuyVolumeRanking { get; set; }
        public int nTotalTradePriceRanking { get; set; }
        public int nTotalTradeVolumeRanking { get; set; }
        public int nTotalRank { get; set; }
        #region 분당
        public int nMinuteTotalRank { get; set; }
        public int nMinuteTradePriceRanking { get; set; }
        public int nMinuteTradeVolumeRanking { get; set; }
        public int nMinuteBuyPriceRanking { get; set; }
        public int nMinuteBuyVolumeRanking { get; set; }
        public int nMinutePowerRanking { get; set; }
        public int nMinuteCountRanking { get; set; }
        public int nMinuteUpDownRanking { get; set; }
        #endregion
        #endregion

        #region 페이크갯수
        public int nFakeBuyCnt { get; set; }
        public int nFakeAssistantCnt { get; set; }
        public int nFakeResistCnt { get; set; }
        public int nPriceUpCnt { get; set; }
        public int nPriceDownCnt { get; set; }
        public int nTotalFakeCnt { get; set; }
        public int nTotalFakeMinuteCnt { get; set; }
        #endregion
        #region 분봉 움직임
        public int nUpCandleCnt { get; set; }
        public int nDownCandleCnt { get; set; }
        public int nUpTailCnt { get; set; }
        public int nDownTailCnt { get; set; }
        public int nShootingCnt { get; set; }
        public int nCandleTwoOverRealCnt { get; set; }
        public int nCandleTwoOverRealNoLeafCnt { get; set; }
        #endregion
        public int nFs { get; set; }
        public int nFb { get; set; }
        #region 현재상태
        public double fSpeedCur { get; set; }
        public double fHogaSpeedCur { get; set; }
        public double fTradeCur { get; set; }
        public double fPureTradeCur { get; set; }
        public double fPureBuyCur { get; set; }
        public double fHogaRatioCur { get; set; }
        public double fSharePerHoga { get; set; }
        public double fSharePerTrade { get; set; }
        public double fHogaPerTrade { get; set; }
        public double fTradePerPure { get; set; }
        #endregion
        #region 이동평균선
        #region 이동평균선 값
        public double fMaDownFsVal { get; set; }
        public double fMa20mVal { get; set; }
        public double fMa1hVal { get; set; }
        public double fMa2hVal { get; set; }

        public double fMaxMaDownFsVal { get; set; }
        public double fMaxMa20mVal { get; set; }
        public double fMaxMa1hVal { get; set; }
        public double fMaxMa2hVal { get; set; }

        public double nMaxMaDownFsTime { get; set; }
        public double nMaxMa20mTime { get; set; }
        public double nMaxMa1hTime { get; set; }
        public double nMaxMa2hTime { get; set; }
        #endregion 
        public int nDownCntMa20m { get; set; }
        public int nDownCntMa1h { get; set; }
        public int nDownCntMa2h { get; set; }
        public int nUpCntMa20m { get; set; }
        public int nUpCntMa1h { get; set; }
        public int nUpCntMa2h { get; set; }
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

        public int nPriceAfter1Sec { get; set; } // 1초 후 가격
        public int nYesterdayEndPrice { get; set; }
    }
}


