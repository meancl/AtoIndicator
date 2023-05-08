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



        // 매수 1차 신청 메서드
        #region RequestThisRealBuy
        private void RequestThisRealBuy(int nRealStrategyNum, double fBuyRatio = NORMAL_TRADE_RATIO, int nExtraChance = 0, TradeMethodCategory eTradeMethod = TradeMethodCategory.RisingMethod, double fCeil = 0, double fFloor = 0, bool isAIUse = true)
        {
            try
            {
                #region 실매수요청 접근시점 기록 및 처리
                if (ea[nCurIdx].myStrategy.nStrategyNum >= REAL_BUY_MAX_NUM || ea[nCurIdx].fakeBuyStrategy.arrStrategy[nRealStrategyNum] > (2 + nExtraChance)) // 한 전략당 6번제한
                    return;


                if (ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx != nTimeLineIdx)
                {
                    ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx = nTimeLineIdx;
                    ea[nCurIdx].myStrategy.nSharedMinuteLocationCount++;
                }

                // 최근 접근시간
                if (ea[nCurIdx].myStrategy.nLastTouchTime != 0 && SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myStrategy.nLastTouchTime) >= 2400) // 40분 이상 매수가 안됐었으면
                {
                    ea[nCurIdx].myStrategy.isSuddenBoom = true;
                }
                ea[nCurIdx].myStrategy.nLastTouchTime = nSharedTime;

                // 전략별 접근 기록
                ea[nCurIdx].myStrategy.arrLastTouch[nRealStrategyNum] = nSharedTime;
                ea[nCurIdx].myStrategy.arrStrategy[nRealStrategyNum]++;
                ea[nCurIdx].myStrategy.arrPrevMinuteIdx[nRealStrategyNum] = nTimeLineIdx;

                ea[nCurIdx].myStrategy.arrBuyPrice[ea[nCurIdx].myStrategy.nStrategyNum] = ea[nCurIdx].nFs;
                ea[nCurIdx].myStrategy.arrBuyTime[ea[nCurIdx].myStrategy.nStrategyNum] = nSharedTime;
                ea[nCurIdx].myStrategy.arrMinuteIdx[ea[nCurIdx].myStrategy.nStrategyNum] = nTimeLineIdx;
                ea[nCurIdx].myStrategy.arrSpecificStrategy[ea[nCurIdx].myStrategy.nStrategyNum] = nRealStrategyNum;
                ea[nCurIdx].myStrategy.nStrategyNum++;
                ea[nCurIdx].myStrategy.nHitNum++;

             
                // 실매수 접근 true(실매수에 다시 쓰임)
                ea[nCurIdx].myStrategy.isOrderCheck = true;

               
                ea[nCurIdx].myStrategy.fEverageShoulderPrice = ea[nCurIdx].myStrategy.fEverageShoulderPrice == 0 ? ea[nCurIdx].nFs : (ea[nCurIdx].nFs + ea[nCurIdx].myStrategy.fEverageShoulderPrice) / 2;

                if (ea[nCurIdx].myStrategy.nMaxShoulderPrice == 0 || ea[nCurIdx].myStrategy.nMaxShoulderPrice < ea[nCurIdx].nFs)
                {
                    ea[nCurIdx].myStrategy.nMaxShoulderPrice = ea[nCurIdx].nFs;

                    if (nTimeLineIdx != ea[nCurIdx].myStrategy.nPrevMaxMinIdx)
                    {
                        ea[nCurIdx].myStrategy.nPrevMaxMinIdx = nTimeLineIdx;
                        ea[nCurIdx].myStrategy.nPrevMaxMinUpperCount++;
                    }
                    ea[nCurIdx].myStrategy.nUpperCount++;
                }

                // 한 분봉에 실매수 횟수제한 
                // ** 잠시 주석처리
                if (ea[nCurIdx].myStrategy.nPrevMinuteIdx != nTimeLineIdx)
                {
                    ea[nCurIdx].myStrategy.nPrevMinuteIdx = nTimeLineIdx;
                    ea[nCurIdx].myStrategy.nMinuteLocationCount++;
                    ea[nCurIdx].myStrategy.nCurBarBuyCount = 1;
                }
                //else // (ea[nCurIdx].myStrategy.nPrevMinuteIdx == nTimeLineIdx)
                //{
                //    if(ea[nCurIdx].myStrategy.nCurBarBuyCount >= BAR_REAL_BUY_MAX_NUM)
                //    {
                //        // return; 
                //    }
                //    else
                //    {
                //        ea[nCurIdx].myStrategy.nCurBarBuyCount++;
                //    }
                //}



                UpFakeCount(nCurIdx, REAL_BUY_SIGNAL, nRealStrategyNum);



                #endregion


#if AI
                if (isAIUse)
                {
                    double[] features102 = GetParameters(nCurIdx: nCurIdx, 102, eTradeMethod: GET_FEATURE_BUY, nRealStrategyNum: nRealStrategyNum);

                    var nMMFNum = mmf.RequestAIService(sCode: ea[nCurIdx].sCode, nRqTime: nSharedTime, nRqType: BUY_AI_NUM, inputData: features102);
                    if (nMMFNum == -1)
                    {
                        PrintLog($"{nSharedTime} AI Service Slot이 부족합니다.");
                        return;
                    }
                    aiSlot.nRequestId = REAL_BUY_SIGNAL;
                    aiSlot.nMMFNumber = nMMFNum;
                    aiQueue.Enqueue(aiSlot);
                }
#endif
                ea[nCurIdx].myStrategy.nApproachNum++;



                PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}, 현재가 : {ea[nCurIdx].nFs} 전략 : {nRealStrategyNum} {strategyName.arrRealBuyStrategyName[nRealStrategyNum]} 매수신청", nCurIdx);
            }
            catch (Exception ex)
            {
                PrintLog($"매수 체크 중 오류 발생 {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nRealStrategyNum}", nCurIdx);
            }
        }
        #endregion

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
                                ea[nCurIdx].myStrategy.nApproachNum,
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
                                ea[nCurIdx].priceUpStrategy.nStrategyNum,
                                ea[nCurIdx].priceDownStrategy.nStrategyNum,
                                ea[nCurIdx].fakeBuyStrategy.nStrategyNum + ea[nCurIdx].fakeAssistantStrategy.nStrategyNum + ea[nCurIdx].fakeResistStrategy.nStrategyNum + ea[nCurIdx].priceUpStrategy.nStrategyNum + ea[nCurIdx].priceDownStrategy.nStrategyNum,
                                ea[nCurIdx].myStrategy.nTotalFakeMinuteAreaNum,
                                ea[nCurIdx].timeLines1m.upCandleList.Count,
                                ea[nCurIdx].timeLines1m.downCandleList.Count,
                                ea[nCurIdx].timeLines1m.upTailList.Count,
                                ea[nCurIdx].timeLines1m.downTailList.Count,
                                ea[nCurIdx].timeLines1m.shootingList.Count,
                                ea[nCurIdx].sequenceStrategy.nCandleTwoOverRealCount,
                                ea[nCurIdx].sequenceStrategy.nCandleTwoOverRealNoLeafCount,
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
