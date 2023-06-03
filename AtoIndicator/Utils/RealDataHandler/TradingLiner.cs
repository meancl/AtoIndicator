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
            bool isSell = false;

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

                if (isSell)
                    RequestThisSell(nCurIdx, checkSellIterIdx, false);
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

                if (isSell)
                    RequestThisSell(nCurIdx, checkSellIterIdx, false);
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

                if (isSell)
                    RequestThisSell(nCurIdx, checkSellIterIdx, false);
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

                if (isSell)
                    RequestThisSell(nCurIdx, checkSellIterIdx, false);
            } // END---- 고정형 매매
            else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].eTradeMethod == TradeMethodCategory.OnlyAIUsedMethod) // END ---- AI 매매기법
            {
                {

                }

                if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].lineCheckingArr[ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx]) >= 300 &&
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee <= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer) // 처분라인
                {
                    isSell = true;
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].lineCheckingArr[ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx] = nSharedTime;

                    PrintLog($"AI 매도 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"AI 매도{NEW_LINE}");
                }
                else if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].lineCheckingArr[STEP_TRADE + ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx]) >= 300 &&
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer) // 상승라인
                {
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].lineCheckingArr[STEP_TRADE + ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx] = nSharedTime;

                    while (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer)
                    {
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx++;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer = GetNextCeiling(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx); // something higher
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer = GetNextFloor(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx, TradeMethodCategory.OnlyAIUsedMethod); // something higher
                    }
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nLastTouchLineTime = nSharedTime;

                    if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx > 2) // 1퍼센트는 넘어야 분할매도 가능..
                        isSell = true;
                }

                if (isSell)
                    RequestThisSell(nCurIdx, checkSellIterIdx, true);
            } // END---- 고정형 매매
            return isSell;
        }


        
        // 매도 1차 신청 메서드
        #region RequestSellAI
        private void RequestThisSell(int nEaIdx, int checkSellIterIdx, bool isAIUse)
        {
            try
            {
                sSharedSellDescription.Append($"매도단계 : {ea[nEaIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx}");

                curSlot = SetAndServeCurSlot("매도", NEW_SELL, ea[nEaIdx].sCode, checkSellIterIdx, "", nEaIdx, 0, 0, 0, MARKET_ORDER, TradeMethodCategory.None, 0, ea[nEaIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurVolume, sSharedSellDescription.ToString(), isAIUse: isAIUse);

                PrintLog($"{checkSellIterIdx}번째 매매슬롯 매도단계 : {ea[nEaIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx}", nEaIdx, checkSellIterIdx, false);
                PrintLog($"매도인큐 {nSharedTime} {ea[nEaIdx].sCode} {ea[nEaIdx].sCodeName} {Math.Round(ea[nEaIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%) 현재단계 : {ea[nEaIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx}");

#if AI
                if (isAIUse)
                {
                    double[] features102 = GetParameters(nCurIdx: nEaIdx, 102, nTradeMethod: REAL_SELL_SIGNAL, nRealStrategyNum: -1);

                    int nMMFNum = mmf.RequestAIService(sCode: ea[nEaIdx].sCode, nRqTime: nSharedTime, nRqType: REAL_SELL_SIGNAL, inputData: features102); // AI 서비스 요청, nSellReqNum는 이벤트 호출용
                    if (nMMFNum == -1) // AI 서비스 슬롯이 없다면
                    {
                        PrintLog($"{nSharedTime} AI Service Slot이 부족합니다.");
                        return;
                    }
                    ea[nEaIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isSelling = true;
                    aiSlot.nEaIdx = nEaIdx;
                    aiSlot.slot = curSlot;
                    aiSlot.nRequestId = REAL_SELL_SIGNAL;
                    aiSlot.nMMFNumber = nMMFNum;
                    aiQueue.Enqueue(aiSlot);
                }
#endif

            }
            catch (Exception ex)
            {
            }
        }
        #endregion


       

        // 머신러닝에 사용할 변수를 
        #region GetParameters
        public double[] GetParameters(int nCurIdx, int nGetSize, int nTradeMethod, int nRealStrategyNum)
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
                                ea[nCurIdx].paperBuyStrategy.nStrategyNum,
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
                                ea[nCurIdx].fakeVolatilityStrategy.nStrategyNum,
                                ea[nCurIdx].fakeDownStrategy.nStrategyNum,
                                ea[nCurIdx].fakeStrategyMgr.nTotalFakeCount,
                                ea[nCurIdx].fakeStrategyMgr.nTotalFakeMinuteAreaNum,
                                ea[nCurIdx].timeLines1m.onePerCandleList.Count,
                                ea[nCurIdx].timeLines1m.downCandleList.Count,
                                ea[nCurIdx].timeLines1m.upTailList.Count,
                                ea[nCurIdx].timeLines1m.downTailList.Count,
                                ea[nCurIdx].timeLines1m.threePerCandleList.Count,
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
