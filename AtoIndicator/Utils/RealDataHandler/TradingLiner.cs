using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static AtoIndicator.KiwoomLib.TimeLib;
using static AtoIndicator.Utils.Comparer;
using static AtoIndicator.KiwoomLib.PricingLib;

namespace AtoIndicator
{
    public partial class MainForm
    {
        public bool HandleTradeLine(int nCurIdx, int checkSellIterIdx)
        {
            void RequestThisSell()
            {
                try
                {
                    sSharedSellDescription.Append($"매도단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx}");

                    curSlot = SetAndServeCurSlot("매도", NEW_SELL, ea[nCurIdx].sCode, checkSellIterIdx, "", nCurIdx, 0, 0, 0, MARKET_ORDER, TradeMethodCategory.None, 0, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurVolume, sSharedSellDescription.ToString(), isAIUse: false);

                    PrintLog($"{checkSellIterIdx}번째 매매슬롯 매도단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx}", nCurIdx, checkSellIterIdx, false);
                    PrintLog($"매도인큐 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%) 현재단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx}");

                }
                catch (Exception ex)
                {
                }
            }

            bool isSell = false;

            sSharedSellDescription.Clear();

            if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].eTradeMethod == TradeMethodCategory.RisingMethod) // 단계별 매매기법
            {
                // ---------------------------------------------------------
                // 선점 조건 확인
                // ----------------------------------------------------------
                if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nBuyEndTime) >= PREEMPTION_ACCESS_SEC)  //선점의 영역, 일정시간동안 접근불가
                {
                    // PREEMPTION_UPDATE_SEC 마다 검사할 부분
                    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nPreemptionPrevUpdateTime) >= PREEMPTION_UPDATE_SEC)
                    {
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nPreemptionPrevUpdateTime = nSharedTime;
                        // TODO.
                    }

                    // 매 tick마다 검사할 부분
                    {

                        if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nLastTouchLineTime) >= LIMIT_STAY_SEC) // 미터치시간이 제한시간을 넘겼을때
                        {
                            if (ea[nCurIdx].timeLines1m.fHourMedianAngle < 0) // 한시간 각도가 음수라면
                            {
                                PrintLog($"선점 - 제한 미터치시간 넘기고 각도 음수임 {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee, 2)}", nCurIdx, checkSellIterIdx);
                                sSharedSellDescription.Append($"선점 - 제한 미터치시간 넘기고 각도 음수임{NEW_LINE}");
                                isSell = true;
                            }
                            if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nLastTouchLineTime) >= END_STAY_SEC)
                            {
                                PrintLog($"선점 - 마지막 제한 미터치시간 넘음 {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee, 2)}", nCurIdx, checkSellIterIdx);
                                sSharedSellDescription.Append($"선점 - 마지막 제한 미터치시간 넘음{NEW_LINE}");
                                isSell = true;
                            }
                        }

                        // 기본선점내용(분할 기준: nCurLineIdx) --> 이쯤하면 더 안오르겠다
                        {

                        }
                    } // END ---- 매 틱마다
                } // END ---- 선점의 영역


                if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee <= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer) // 처분라인
                {
                    if ((SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nBuyEndTime) > RESPITE_ACCESS_SEC && (
                            (ea[nCurIdx].maOverN.nUpCntMa2h > 0) || // 1. 120평선이 뚫린 상황
                            (ea[nCurIdx].maOverN.fCurMa1h <= ea[nCurIdx].maOverN.fCurMa2h) || // 4. 1시간평선 <= 2시간평선
                            (ea[nCurIdx].timeLines1m.fDAngle <= -25) || // 5. D각도가 25도 떨어짐
                            (ea[nCurIdx].timeLines1m.fHourMedianAngle <= -10)) // 6. 한시간 추세가 -10도 이하인경우
                          ) ||
                          ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx <= 1 // 한칸 이하밖에 못올라간 경우
                       ) // 무조건 팔아야하는 조건들 ( 유예취소사항에 속한다면 )
                    {
                        if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx <= 1)
                        {
                            PrintLog($"유예 0단 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx, checkSellIterIdx);
                            sSharedSellDescription.Append($"유예 0단  ");
                        }
                        if (ea[nCurIdx].maOverN.nUpCntMa2h > 0)
                        {
                            PrintLog($"유예 1단 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx, checkSellIterIdx);
                            sSharedSellDescription.Append($"유예 1단  ");
                        }
                        if (ea[nCurIdx].maOverN.fCurMa1h <= ea[nCurIdx].maOverN.fCurMa2h)
                        {
                            PrintLog($"유예 4단 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx, checkSellIterIdx);
                            sSharedSellDescription.Append($"유예 4단  ");
                        }
                        if (ea[nCurIdx].timeLines1m.fDAngle <= -25)
                        {
                            PrintLog($"유예 5단 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx, checkSellIterIdx);
                            sSharedSellDescription.Append($"유예 5단  ");
                        }
                        if (ea[nCurIdx].timeLines1m.fHourMedianAngle <= -10)
                        {
                            PrintLog($"유예 6단 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx, checkSellIterIdx);
                            sSharedSellDescription.Append($"유예 6단  ");
                        }

                        sSharedSellDescription.Append($"{NEW_LINE}##유예 불가##{NEW_LINE}");
                        PrintLog($"유예 불가 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx);
                        isSell = true;
                    } // END ---- 유예취소사항에 속한다면
                    else // 유예가능하다면 --> 다시 오를 수 있겠다
                    {
                        if (!ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isRespiteSignal) // 유예중이 아니라면
                        {
                            PrintLog($"유예 성공 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx);

                            ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isRespiteSignal = true; // 유예시그널 on
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nRespitePrevUpdateTime = nSharedTime; // 유예이전업데이트시간 on
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fRespiteCriticalLine = ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee - RESPITE_CRITICAL_PADDING; // 새로운 유예 첫 손절선 등록
                            if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nRespiteFirstTime == 0)// 이전 유예가 끝났다면
                            {
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nRespiteFirstTime = nSharedTime; // 새로운 유예 첫시간을 등록 
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nEachRespiteCount++; // 독립적인 유예의 카운트 등록
                            }
                        }// END----유예중이 아니라면
                        else // 유예중이라면
                        {
                            if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee < ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fRespiteCriticalLine ||
                                SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nRespitePrevUpdateTime) >= RESPITE_LIMIT_SEC // 유예를 10분동안하고 있다니
                                )
                            { // 잘못봤었네 팔아야지
                                sSharedSellDescription.Append($"유예 - 유예 중에 제한선 넘음.{NEW_LINE}");
                                PrintLog($"유예 - 유예중에 제한선 넘음 {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee, 2)}(%)", nCurIdx, checkSellIterIdx);
                                isSell = true;
                            }
                        }// END----유예중이라면
                    } // END ---- 유예가능하다면
                } // END ---- 처분라인
                else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer) // 상승라인
                {
                    while (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer)
                    {
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx++;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer = GetNextCeiling(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx); // something higher
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer = GetNextFloor(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx, TradeMethodCategory.RisingMethod); // something higher
                    }

                    if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isRespiteSignal)  // 상승선을 터치했으니 이전 유예정보를 초기화한다
                    {
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isRespiteSignal = false;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fRespiteCriticalLine = RESPITE_INIT;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nRespiteFirstTime = 0;
                    }

                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nLastTouchLineTime = nSharedTime;

                    if (isSell) // 단계는 상승했는데 팔린다는 건 이상하니 
                    {
                        isSell = false;
                        PrintLog($"단계상승으로 일시적 매도취소 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} 단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx} 손익 : {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx, false);
                    }

                    // 추가매수 파트, 추가매수의 전략은 항상 0번이다.
                    //if (!ea[nCurIdx].isExcluded)
                    //  {} // END ---- 추가매수 파트

                } // END---- 상승라인


            } // END ---- 단계형 매매
            else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].eTradeMethod == TradeMethodCategory.ScalpingMethod) // 스캘핑 매매기법
            {
                {

                }

                if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee <= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer) // 처분라인
                {
                    isSell = true;
                    PrintLog($"스캘핑 매도 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"스캘핑 매도{NEW_LINE}");
                }
                else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer) // 상승라인
                {
                    while (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer)
                    {
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx++;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer = GetNextCeiling(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx); // something higher
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer = GetNextFloor(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx, TradeMethodCategory.ScalpingMethod); // something higher
                    }

                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nLastTouchLineTime = nSharedTime;
                    if (isSell) // 단계는 상승했는데 팔린다는 건 이상하니 
                    {
                        isSell = false;
                        PrintLog($"단계상승으로 일시적 매도취소 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} 단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx} 손익 : {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx, false);
                    }
                }

            }// END ----  스캘핑 매매
            else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].eTradeMethod == TradeMethodCategory.BottomUpMethod) // 바텀업 매매기법
            {
                {

                }

                if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee <= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer) // 처분라인
                {
                    isSell = true;
                    PrintLog($"바텀업 매도 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"바텀업 매도{NEW_LINE}");
                }
                else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer) // 상승라인
                {
                    while (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer)
                    {
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx++;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer = GetNextCeiling(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx); // something higher
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer = GetNextFloor(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx, TradeMethodCategory.BottomUpMethod); // something higher
                    }

                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nLastTouchLineTime = nSharedTime;
                    if (isSell) // 단계는 상승했는데 팔린다는 건 이상하니 
                    {
                        isSell = false;
                        PrintLog($"단계상승으로 일시적 매도취소 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} 단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx} 손익 : {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx, false);
                    }
                }

            }// END ---- 바텀업 매매
            else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].eTradeMethod == TradeMethodCategory.FixedMethod) // 고정형 매매기법
            {
                if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee <= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer) // 손익률이 손절퍼센트보다 낮으면
                {
                    isSell = true;
                    PrintLog($"고정형 매도 - 손절 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} : {ea[nCurIdx].sCode} 손절매도신청", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"고정형 매도 - 손절{NEW_LINE}");
                }
                else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer) // 손익률이 익절퍼센트를 넘기면
                {
                    isSell = true;
                    PrintLog($"고정형 매도 - 익절 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} : {ea[nCurIdx].sCode} 익절매도신청", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"고정형 매도 - 익절{NEW_LINE}");
                }

            } // END---- 고정형 매매

            if (ea[nCurIdx].fPower >= 0.29)
            {
                sSharedSellDescription.Append($"상한가 도달!{NEW_LINE}");
                PrintLog($"상한가 도달! : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx, checkSellIterIdx);
            }

            if (isSell)
                RequestThisSell();

            return isSell;
        }


        #region RequestThisRealBuy
        public void RequestThisRealBuy(int nCurIdx, int nRequestPrice = 0, int eyesUp = 0, double fBuyRatio = NORMAL_TRADE_RATIO, int nExtraChance = 0, TradeMethodCategory eTradeMethod = TradeMethodCategory.RisingMethod, double fCeil = 0, double fFloor = 0, bool isAIUse = true)
        {
            try
            {

                int priceToOrder;
                if (nRequestPrice == 0)
                    priceToOrder = ea[nCurIdx].nFb + GetIntegratedMarketGap(ea[nCurIdx].nFb);
                else
                    priceToOrder = nRequestPrice;

                for (int eyeCloseIdx = 0; eyeCloseIdx < Math.Abs(eyesUp); eyeCloseIdx++)
                {
                    if (eyesUp < 0)
                        priceToOrder -= GetIntegratedMarketGap(priceToOrder); // 반복해서 가격을 n칸 내린다.
                    else
                        priceToOrder += GetIntegratedMarketGap(priceToOrder); // 반복해서 가격을 n칸 올린다.
                }


                curSlot = SetAndServeCurSlot("신규매수", NEW_BUY, ea[nCurIdx].sCode, 0, "", nCurIdx, -1,
                         priceToOrder, // 호가 스프레드를 줄이기 위한 방법 : 호가스프레드가 벌어졌을떄 nFb 기준으로 가격을 산정
                         fBuyRatio, MARKET_ORDER, eTradeMethod, -1, 0, "신규매수", fCeil: fCeil, fFloor: fFloor, isAIUse: false);


                PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}, 현재가 : {ea[nCurIdx].nFs} 매수신청", nCurIdx);
            }
            catch (Exception ex)
            {
                PrintLog($"매수 체크 중 오류 발생 {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx);
            }
        }
        #endregion



    }
}
