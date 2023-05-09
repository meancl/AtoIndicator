using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static AtoTrader.KiwoomLib.TimeLib;
using static AtoTrader.Utils.Comparer;
using static AtoTrader.KiwoomLib.PricingLib;

namespace AtoTrader
{
    public partial class MainForm
    {


        public const int BUY_AI_NUM = 0;
        public const int SELL_AI_NUM = 1;
        public const int FAKE_AI_NUM = 2;




        public const int GET_FEATURE_BUY = 0;
        public const int GET_FEATURE_SELL = 1;
        public const int GET_FEATURE_FAKE = 1;
        // 머신러닝에 사용할 변수를 
        #region GetParameters
        public double[] GetParameters(int nCurIdx, int nGetSize, int eTradeMethod, int nRealStrategyNum)
        {
            double[] features102 = new double[] {
                                nRealStrategyNum,
                                nSharedTime,
                                ea[nCurIdx].fStartGap,
                                ea[nCurIdx].fPowerWithoutGap,
                                ea[nCurIdx].fPower,
                                ea[nCurIdx].fPlusCnt07,
                                ea[nCurIdx].fMinusCnt07,
                                ea[nCurIdx].fPlusCnt09,
                                ea[nCurIdx].fMinusCnt09,
                                ea[nCurIdx].fPowerJar,
                                ea[nCurIdx].fOnlyDownPowerJar,
                                ea[nCurIdx].fOnlyUpPowerJar,
                                ea[nCurIdx].fakeVolatilityStrategy.nStrategyNum,
                                ea[nCurIdx].nChegyulCnt,
                                ea[nCurIdx].nHogaCnt,
                                ea[nCurIdx].nNoMoveCount,
                                ea[nCurIdx].nFewSpeedCount,
                                ea[nCurIdx].nMissCount,
                                ea[nCurIdx].lTotalTradeVolume,
                                ea[nCurIdx].lOnlyBuyVolume,
                                ea[nCurIdx].lOnlySellVolume,
                                ea[nCurIdx].nAccumUpDownCount,
                                ea[nCurIdx].fAccumUpPower,
                                ea[nCurIdx].fAccumDownPower,
                                ea[nCurIdx].lTotalTradePrice,
                                ea[nCurIdx].lOnlyBuyPrice,
                                ea[nCurIdx].lOnlySellPrice,
                                ea[nCurIdx].lMarketCap,
                                ea[nCurIdx].rankSystem.nAccumCountRanking,
                                ea[nCurIdx].rankSystem.nMarketCapRanking,
                                ea[nCurIdx].rankSystem.nPowerRanking,
                                ea[nCurIdx].rankSystem.nTotalBuyPriceRanking,
                                ea[nCurIdx].rankSystem.nTotalBuyVolumeRanking,
                                ea[nCurIdx].rankSystem.nTotalTradePriceRanking,
                                ea[nCurIdx].rankSystem.nTotalTradeVolumeRanking,
                                ea[nCurIdx].rankSystem.nSummationRanking,
                                ea[nCurIdx].rankSystem.nMinuteSummationRanking,
                                ea[nCurIdx].rankSystem.nMinuteTradePriceRanking,
                                ea[nCurIdx].rankSystem.nMinuteTradeVolumeRanking,
                                ea[nCurIdx].rankSystem.nMinuteBuyPriceRanking,
                                ea[nCurIdx].rankSystem.nMinuteBuyVolumeRanking,
                                ea[nCurIdx].rankSystem.nMinutePowerRanking,
                                ea[nCurIdx].rankSystem.nMinuteCountRanking,
                                ea[nCurIdx].rankSystem.nMinuteUpDownRanking,
                                ea[nCurIdx].fakeBuyStrategy.nStrategyNum,
                                ea[nCurIdx].fakeAssistantStrategy.nStrategyNum,
                                ea[nCurIdx].fakeResistStrategy.nStrategyNum,
                                0,
                                0,
                                ea[nCurIdx].fakeStrategyMgr.nTotalFakeMinuteAreaNum,
                                ea[nCurIdx].fakeStrategyMgr.nTotalFakeMinuteAreaNum,
                                ea[nCurIdx].timeLines1m.upCandleList.Count,
                                ea[nCurIdx].timeLines1m.downCandleList.Count,
                                ea[nCurIdx].timeLines1m.upTailList.Count,
                                ea[nCurIdx].timeLines1m.downTailList.Count,
                                ea[nCurIdx].timeLines1m.shootingList.Count,
                                0,
                                0,
                                ea[nCurIdx].speedStatus.fCur,
                                ea[nCurIdx].hogaSpeedStatus.fCur,
                                ea[nCurIdx].tradeStatus.fCur,
                                ea[nCurIdx].pureTradeStatus.fCur,
                                ea[nCurIdx].pureBuyStatus.fCur,
                                ea[nCurIdx].hogaRatioStatus.fCur,
                                ea[nCurIdx].fSharePerHoga,
                                ea[nCurIdx].fSharePerTrade,
                                ea[nCurIdx].fHogaPerTrade,
                                ea[nCurIdx].fTradePerPure,
                                ea[nCurIdx].maOverN.fCurDownFs,
                                ea[nCurIdx].maOverN.fCurMa20m,
                                ea[nCurIdx].maOverN.fCurMa1h,
                                ea[nCurIdx].maOverN.fCurMa2h,
                                ea[nCurIdx].maOverN.fMaxDownFs,
                                ea[nCurIdx].maOverN.fMaxMa20m,
                                ea[nCurIdx].maOverN.fMaxMa1h,
                                ea[nCurIdx].maOverN.fMaxMa2h,
                                ea[nCurIdx].maOverN.nMaxDownFsTime,
                                ea[nCurIdx].maOverN.nMaxMa20mTime,
                                ea[nCurIdx].maOverN.nMaxMa1hTime,
                                ea[nCurIdx].maOverN.nMaxMa2hTime,
                                ea[nCurIdx].maOverN.nDownCntMa20m,
                                ea[nCurIdx].maOverN.nDownCntMa1h,
                                ea[nCurIdx].maOverN.nDownCntMa2h,
                                ea[nCurIdx].maOverN.nUpCntMa20m,
                                ea[nCurIdx].maOverN.nUpCntMa1h,
                                ea[nCurIdx].maOverN.nUpCntMa2h,
                                ea[nCurIdx].timeLines1m.fMaxSlope,
                                ea[nCurIdx].timeLines1m.fInitSlope,
                                ea[nCurIdx].timeLines1m.fTotalMedian,
                                ea[nCurIdx].timeLines1m.fHourMedian,
                                ea[nCurIdx].timeLines1m.fRecentMedian,
                                ea[nCurIdx].timeLines1m.fMaxSlope - ea[nCurIdx].timeLines1m.fInitSlope,
                                ea[nCurIdx].timeLines1m.fMaxAngle,
                                ea[nCurIdx].timeLines1m.fInitAngle,
                                ea[nCurIdx].timeLines1m.fTotalMedianAngle,
                                ea[nCurIdx].timeLines1m.fHourMedianAngle,
                                ea[nCurIdx].timeLines1m.fRecentMedianAngle,
                                ea[nCurIdx].timeLines1m.fDAngle,
                                ea[nCurIdx].crushMinuteManager.nCurCnt,
                                ea[nCurIdx].crushMinuteManager.nUpCnt,
                                ea[nCurIdx].crushMinuteManager.nDownCnt,
                                ea[nCurIdx].crushMinuteManager.nSpecialDownCnt };



            return features102;
        }
        #endregion
    }
}
