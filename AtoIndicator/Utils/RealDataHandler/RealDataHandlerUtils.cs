using AtoIndicator.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtoIndicator.KiwoomLib.TimeLib;
using static AtoIndicator.TradingBlock.TimeLineGenerator;
using static AtoIndicator.KiwoomLib.PricingLib;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;

namespace AtoIndicator
{
    public partial class MainForm
    {

        void UpFakeCount(int nEaIdx, int nFakeNum, int nBuyStrategyNum)
        {
            if (nFakeNum != EVERY_SIGNAL && nFakeNum != PAPER_SELL_SIGNAL)
            {
                if (nFakeNum != PAPER_BUY_SIGNAL)
                    ea[nEaIdx].fakeStrategyMgr.nTotalFakeCount++; // 실매수 미포함
                ea[nEaIdx].fakeStrategyMgr.nTotalArrowCount++; // 실매수 포함 

            }

            FakeDBRecordInfo newF = new FakeDBRecordInfo();
            ea[nEaIdx].GetFakeFix(newF.fr);

            newF.fr.nRqTime = nSharedTime;
            newF.fr.nOverPrice = newF.fr.nFb;

            if (newF.fr.nOverPrice == 0) // 값이 없다면..?
                newF.fr.nOverPrice = ea[nEaIdx].nCurHogaPrice;

            if (nFakeNum != PAPER_SELL_SIGNAL)
            {
                for (int i = 0; i < EYES_CLOSE_NUM; i++)
                    newF.fr.nOverPrice += GetIntegratedMarketGap(newF.fr.nOverPrice);
            }

            newF.nTimeLineIdx = nTimeLineIdx;

            int nRequestSignal = FAKE_REQUEST_SIGNAL; // default로 FAKE_REQUEST_SIGNAL


            switch (nFakeNum)
            {
                case FAKE_BUY_SIGNAL:
                    newF.fr.nBuyStrategyIdx = strategyNameDict[(FAKE_BUY_SIGNAL, strategyName.arrFakeBuyStrategyName[nBuyStrategyNum])];
                    newF.fr.nBuyStrategyGroupNum = FAKE_BUY_SIGNAL; //key
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].fakeBuyStrategy.arrStrategy[nBuyStrategyNum]; // key
                    break;
                case FAKE_RESIST_SIGNAL:
                    newF.fr.nBuyStrategyIdx = strategyNameDict[(FAKE_RESIST_SIGNAL, strategyName.arrFakeResistStrategyName[nBuyStrategyNum])];
                    newF.fr.nBuyStrategyGroupNum = FAKE_RESIST_SIGNAL; // key
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].fakeResistStrategy.arrStrategy[nBuyStrategyNum]; // key
                    break;
                case FAKE_ASSISTANT_SIGNAL:
                    newF.fr.nBuyStrategyIdx = strategyNameDict[(FAKE_ASSISTANT_SIGNAL, strategyName.arrFakeAssistantStrategyName[nBuyStrategyNum])];
                    newF.fr.nBuyStrategyGroupNum = FAKE_ASSISTANT_SIGNAL; // key
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].fakeAssistantStrategy.arrStrategy[nBuyStrategyNum]; // key
                    break;
                case FAKE_VOLATILE_SIGNAL:
                    newF.fr.nBuyStrategyIdx = strategyNameDict[(FAKE_VOLATILE_SIGNAL, strategyName.arrFakeVolatilityStrategyName[nBuyStrategyNum])];
                    newF.fr.nBuyStrategyGroupNum = FAKE_VOLATILE_SIGNAL; // key
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].fakeVolatilityStrategy.arrStrategy[nBuyStrategyNum]; // key
                    break;
                case FAKE_DOWN_SIGNAL:
                    newF.fr.nBuyStrategyIdx = strategyNameDict[(FAKE_DOWN_SIGNAL, strategyName.arrFakeDownStrategyName[nBuyStrategyNum])];
                    newF.fr.nBuyStrategyGroupNum = FAKE_DOWN_SIGNAL; // key
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].fakeDownStrategy.arrStrategy[nBuyStrategyNum]; // key
                    break;
                case PAPER_BUY_SIGNAL:
                    newF.fr.nBuyStrategyIdx = strategyNameDict[(PAPER_BUY_SIGNAL, strategyName.arrPaperBuyStrategyName[nBuyStrategyNum])]; // key
                    newF.fr.nBuyStrategyGroupNum = PAPER_BUY_SIGNAL; // key
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].paperBuyStrategy.arrStrategy[nBuyStrategyNum]; // key
                    break;
                case PAPER_SELL_SIGNAL:
                    newF.fr.nBuyStrategyIdx = strategyNameDict[(PAPER_BUY_SIGNAL, strategyName.arrPaperBuyStrategyName[nBuyStrategyNum])]; // key
                    newF.fr.nBuyStrategyGroupNum = PAPER_SELL_SIGNAL; // key
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].paperBuyStrategy.arrStrategy[nBuyStrategyNum]; // key
                    nRequestSignal = EVERY_SIGNAL;
                    break;
                case EVERY_SIGNAL:
                    newF.fr.nBuyStrategyIdx = nBuyStrategyNum; // key
                    newF.fr.nBuyStrategyGroupNum = EVERY_SIGNAL; // key
                    newF.fr.nBuyStrategySequenceIdx = ++ea[nEaIdx].fakeStrategyMgr.nEveryAICount;
                    nRequestSignal = EVERY_SIGNAL;
                    break;
                default:
                    break;
            }

            ea[nEaIdx].fakeStrategyMgr.fd.Add(newF);


#if AI
            // AI 서비스 요청
            double[] features102 = GetParameters(nCurIdx: nEaIdx, 102, nTradeMethod: nRequestSignal, nRealStrategyNum: newF.fr.nBuyStrategyIdx);

            var nMMFNum = mmf.RequestAIService(sCode: ea[nEaIdx].sCode, nRqTime: nSharedTime, nRqType: nRequestSignal, inputData: features102);
            if (nMMFNum == -1)
            {
                PrintLog($"{nSharedTime} AI Service Slot이 부족합니다.");
                return;
            }
            aiSlot.nEaIdx = nEaIdx;
            aiSlot.nRequestId = nRequestSignal;
            aiSlot.nMMFNumber = nMMFNum;

            aiQueue.Enqueue(aiSlot);

#endif

        }

        #region SetThisFake
        bool SetThisFake(FakeFrame frame, int nEaIdx, int nFakeBuyStrategyNum)
        {
            if (frame.nFakeType != PAPER_BUY_SIGNAL)
            {
                if (frame.nStrategyNum >= FAKE_BUY_MAX_NUM || frame.arrStrategy[nFakeBuyStrategyNum] > 5) // 한 전략당 6번제한
                    return false;


            }

            #region 공용 파트
            if (ea[nEaIdx].fakeStrategyMgr.nSharedPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nEaIdx].fakeStrategyMgr.nSharedPrevMinuteIdx = nTimeLineIdx;
                ea[nEaIdx].fakeStrategyMgr.nSharedMinuteLocationCount++;
            }


            if (frame.nPrevMinuteIdx != nTimeLineIdx)
            {
                frame.nPrevMinuteIdx = nTimeLineIdx;
                frame.nMinuteLocationCount++;
                frame.nCurBarBuyCount = 1;
            }
            else
            {
                if (frame.nCurBarBuyCount >= BAR_FAKE_MAX_NUM)
                {
                    if (frame.nFakeType == FAKE_VOLATILE_SIGNAL || frame.nFakeType == FAKE_DOWN_SIGNAL)
                        return false;
                }
                else
                {
                    frame.nCurBarBuyCount++;
                }
            }



            if (frame.nLastTouchTime != 0 && SubTimeToTimeAndSec(nSharedTime, frame.nLastTouchTime) >= 2400) // 40분 이상 매수가 안됐었으면
            {
                frame.isSuddenBoom = true;
            }
            frame.nLastTouchTime = nSharedTime;

            frame.arrLastTouch[nFakeBuyStrategyNum] = nSharedTime;
            frame.arrStrategy[nFakeBuyStrategyNum]++;
            frame.arrPrevMinuteIdx[nFakeBuyStrategyNum] = nTimeLineIdx;

            frame.arrBuyPrice[frame.nStrategyNum] = ea[nEaIdx].nFs;
            frame.arrBuyTime[frame.nStrategyNum] = nSharedTime;
            frame.arrMinuteIdx[frame.nStrategyNum] = nTimeLineIdx;
            frame.arrSpecificStrategy[frame.nStrategyNum] = nFakeBuyStrategyNum;
            frame.nStrategyNum++;
            frame.nHitNum++;

            frame.fEverageShoulderPrice = (frame.fEverageShoulderPrice == 0) ? ea[nEaIdx].nFs : (ea[nEaIdx].nFs + frame.fEverageShoulderPrice) / 2;
            frame.nSumShoulderPrice += ea[nEaIdx].nFs;
            if (frame.nMaxShoulderPrice == 0 || frame.nMaxShoulderPrice < ea[nEaIdx].nFs)
            {
                frame.nMaxShoulderPrice = ea[nEaIdx].nFs;

                if (nTimeLineIdx != frame.nPrevMaxMinIdx)
                {
                    frame.nPrevMaxMinIdx = nTimeLineIdx;
                    frame.nPrevMaxMinUpperCount++;
                }
                frame.nUpperCount++;
            }
            #endregion


            if (frame.nFakeType != PAPER_BUY_SIGNAL)
            {
                UpdateFakeHistory(nEaIdx);
                AddFakeHistory(frame.nFakeType, nEaIdx, nFakeBuyStrategyNum);
                CalcFakeHistory(nEaIdx);

                UpFakeCount(nEaIdx, frame.nFakeType, nFakeBuyStrategyNum);
            }

            return true;
        }

        #endregion
        #region SetThisPaperSell
        private void SetThisPaperSell(PaperBuyStrategy frame, int nCurIdx, int i)
        {
            bool isSell = false;

            if (frame.paperTradeSlot[i].isSelling)
                return;

            sbPaperSellDescription.Clear();


            // ---------------------------------------------------------
            // 선점 조건 확인
            // ----------------------------------------------------------
            if (frame.paperTradeSlot[i].methodCategory == TradeMethodCategory.RisingMethod)
            {
                if (SubTimeToTimeAndSec(nSharedTime, frame.paperTradeSlot[i].nBuyEndTime) >= PREEMPTION_ACCESS_SEC)  //선점의 영역, 일정시간동안 접근불가
                {
                    // PREEMPTION_UPDATE_SEC 마다 검사할 부분
                    if (SubTimeToTimeAndSec(nSharedTime, frame.paperTradeSlot[i].nPreemptionPrevUpdateTime) >= PREEMPTION_UPDATE_SEC)
                    {
                        frame.paperTradeSlot[i].nPreemptionPrevUpdateTime = nSharedTime;
                        // TODO.
                    }

                    // 매 tick마다 검사할 부분
                    {

                        if (SubTimeToTimeAndSec(nSharedTime, frame.paperTradeSlot[i].nLastTouchLineTime) >= LIMIT_STAY_SEC) // 미터치시간이 제한시간을 넘겼을때
                        {
                            if (ea[nCurIdx].timeLines1m.fHourMedianAngle < 0) // 한시간 각도가 음수라면
                            {
                                sbPaperSellDescription.Append($"선점 - 제한 미터치시간 넘기고 각도 음수임{NEW_LINE}");
                                isSell = true;
                            }
                            if (SubTimeToTimeAndSec(nSharedTime, frame.paperTradeSlot[i].nLastTouchLineTime) >= END_STAY_SEC)
                            {
                                sbPaperSellDescription.Append($"선점 - 마지막 제한 미터치시간 넘음{NEW_LINE}");
                                isSell = true;
                            }
                        }

                        // 기본선점내용(분할 기준: nCurLineIdx) --> 이쯤하면 더 안오르겠다
                        {

                        }
                    } // END ---- 매 틱마다
                } // END ---- 선점의 영역


                if (frame.paperTradeSlot[i].fPowerWithFee <= frame.paperTradeSlot[i].fBottomPer) // 처분라인
                {
                    if ((SubTimeToTimeAndSec(nSharedTime, frame.paperTradeSlot[i].nBuyEndTime) > RESPITE_ACCESS_SEC && (
                            (ea[nCurIdx].maOverN.nUpCntMa2h > 0) || // 1. 120평선이 뚫린 상황
                            (ea[nCurIdx].maOverN.fCurMa1h <= ea[nCurIdx].maOverN.fCurMa2h) || // 4. 1시간평선 <= 2시간평선
                            (ea[nCurIdx].timeLines1m.fDAngle <= -25) || // 5. D각도가 25도 떨어짐
                            (ea[nCurIdx].timeLines1m.fHourMedianAngle <= -10)) // 6. 한시간 추세가 -10도 이하인경우
                          ) ||
                          frame.paperTradeSlot[i].nCurLineIdx <= 1 // 한칸 이하밖에 못올라간 경우
                       ) // 무조건 팔아야하는 조건들 ( 유예취소사항에 속한다면 )
                    {
                        if (frame.paperTradeSlot[i].nCurLineIdx <= 1)
                        {
                            sbPaperSellDescription.Append($"유예 0단  ");
                        }
                        if (ea[nCurIdx].maOverN.nUpCntMa2h > 0)
                        {
                            sbPaperSellDescription.Append($"유예 1단  ");
                        }
                        if (ea[nCurIdx].maOverN.fCurMa1h <= ea[nCurIdx].maOverN.fCurMa2h)
                        {
                            sbPaperSellDescription.Append($"유예 4단  ");
                        }
                        if (ea[nCurIdx].timeLines1m.fDAngle <= -25)
                        {
                            sbPaperSellDescription.Append($"유예 5단  ");
                        }
                        if (ea[nCurIdx].timeLines1m.fHourMedianAngle <= -10)
                        {
                            sbPaperSellDescription.Append($"유예 6단  ");
                        }

                        sbPaperSellDescription.Append($"{NEW_LINE}##유예 불가##{NEW_LINE}");
                        isSell = true;
                    } // END ---- 유예취소사항에 속한다면
                    else // 유예가능하다면 --> 다시 오를 수 있겠다
                    {
                        if (!frame.paperTradeSlot[i].isRespiteSignal) // 유예중이 아니라면
                        {
                            frame.paperTradeSlot[i].isRespiteSignal = true; // 유예시그널 on
                            frame.paperTradeSlot[i].nRespitePrevUpdateTime = nSharedTime; // 유예이전업데이트시간 on
                            frame.paperTradeSlot[i].fRespiteCriticalLine = frame.paperTradeSlot[i].fPowerWithFee - RESPITE_CRITICAL_PADDING; // 새로운 유예 첫 손절선 등록
                            if (frame.paperTradeSlot[i].nRespiteFirstTime == 0)// 이전 유예가 끝났다면
                            {
                                frame.paperTradeSlot[i].nRespiteFirstTime = nSharedTime; // 새로운 유예 첫시간을 등록 
                                frame.paperTradeSlot[i].nEachRespiteCount++; // 독립적인 유예의 카운트 등록
                            }
                        }// END----유예중이 아니라면
                        else // 유예중이라면
                        {
                            if (frame.paperTradeSlot[i].fPowerWithFee < frame.paperTradeSlot[i].fRespiteCriticalLine ||
                                SubTimeToTimeAndSec(nSharedTime, frame.paperTradeSlot[i].nRespitePrevUpdateTime) >= RESPITE_LIMIT_SEC // 유예를 10분동안하고 있다니
                                )
                            { // 잘못봤었네 팔아야지
                                sbPaperSellDescription.Append($"유예 - 유예 중에 제한선 넘음.{NEW_LINE}");
                                isSell = true;
                            }
                        }// END----유예중이라면
                    } // END ---- 유예가능하다면
                } // END ---- 처분라인
                else if (frame.paperTradeSlot[i].fPowerWithFee >= frame.paperTradeSlot[i].fTargetPer) // 상승라인
                {
                    while (frame.paperTradeSlot[i].fPowerWithFee >= frame.paperTradeSlot[i].fTargetPer)
                    {
                        frame.paperTradeSlot[i].nCurLineIdx = RaiseStepUp(frame.paperTradeSlot[i].nCurLineIdx);
                        frame.paperTradeSlot[i].fTargetPer = GetNextCeiling(frame.paperTradeSlot[i].nCurLineIdx); // something higher
                        frame.paperTradeSlot[i].fBottomPer = GetNextFloor(frame.paperTradeSlot[i].nCurLineIdx, TradeMethodCategory.RisingMethod); // something higher
                    }

                    if (frame.paperTradeSlot[i].isRespiteSignal)  // 상승선을 터치했으니 이전 유예정보를 초기화한다
                    {
                        frame.paperTradeSlot[i].isRespiteSignal = false;
                        frame.paperTradeSlot[i].fRespiteCriticalLine = RESPITE_INIT;
                        frame.paperTradeSlot[i].nRespiteFirstTime = 0;
                    }

                    frame.paperTradeSlot[i].nLastTouchLineTime = nSharedTime;

                    if (isSell) // 단계는 상승했는데 팔린다는 건 이상하니 
                    {
                        isSell = false;
                    }


                } // END---- 상승라인


            }
            else if (frame.paperTradeSlot[i].methodCategory == TradeMethodCategory.BottomUpMethod)
            {
                {

                }

                if (frame.paperTradeSlot[i].fPowerWithFee <= frame.paperTradeSlot[i].fBottomPer) // 처분라인
                {
                    isSell = true;
                    sbPaperSellDescription.Append($"바텀업 매도{NEW_LINE}");
                }
                else if (frame.paperTradeSlot[i].fPowerWithFee >= frame.paperTradeSlot[i].fTargetPer) // 상승라인
                {
                    while (frame.paperTradeSlot[i].fPowerWithFee >= frame.paperTradeSlot[i].fTargetPer)
                    {
                        frame.paperTradeSlot[i].nCurLineIdx= RaiseStepUp(frame.paperTradeSlot[i].nCurLineIdx);
                        frame.paperTradeSlot[i].fTargetPer = GetNextCeiling(frame.paperTradeSlot[i].nCurLineIdx); // something higher
                        frame.paperTradeSlot[i].fBottomPer = GetNextFloor(frame.paperTradeSlot[i].nCurLineIdx, TradeMethodCategory.BottomUpMethod); // something higher
                    }

                    frame.paperTradeSlot[i].nLastTouchLineTime = nSharedTime;
                    if (isSell) // 단계는 상승했는데 팔린다는 건 이상하니 
                    {
                        isSell = false;
                    }
                }
            }
            else if (frame.paperTradeSlot[i].methodCategory == TradeMethodCategory.FixedMethod)
            {
                if (frame.paperTradeSlot[i].fPowerWithFee <= frame.paperTradeSlot[i].fBottomPer) // 손익률이 손절퍼센트보다 낮으면
                {
                    isSell = true;
                    sbPaperSellDescription.Append($"고정형 매도 - 손절{NEW_LINE}");
                }
                else if (frame.paperTradeSlot[i].fPowerWithFee >= frame.paperTradeSlot[i].fTargetPer) // 손익률이 익절퍼센트를 넘기면
                {
                    isSell = true;
                    sbPaperSellDescription.Append($"고정형 매도 - 익절{NEW_LINE}");
                }
            }
            else if (frame.paperTradeSlot[i].methodCategory == TradeMethodCategory.ScalpingMethod)
            {
                {

                }

                if (frame.paperTradeSlot[i].fPowerWithFee <= frame.paperTradeSlot[i].fBottomPer) // 처분라인
                {
                    isSell = true;
                    sbPaperSellDescription.Append($"스캘핑 매도{NEW_LINE}");
                }
                else if (frame.paperTradeSlot[i].fPowerWithFee >= frame.paperTradeSlot[i].fTargetPer) // 상승라인
                {
                    while (frame.paperTradeSlot[i].fPowerWithFee >= frame.paperTradeSlot[i].fTargetPer)
                    {
                        frame.paperTradeSlot[i].nCurLineIdx = RaiseStepUp(frame.paperTradeSlot[i].nCurLineIdx);
                        frame.paperTradeSlot[i].fTargetPer = GetNextCeiling(frame.paperTradeSlot[i].nCurLineIdx); // something higher
                        frame.paperTradeSlot[i].fBottomPer = GetNextFloor(frame.paperTradeSlot[i].nCurLineIdx, TradeMethodCategory.ScalpingMethod); // something higher
                    }

                    frame.paperTradeSlot[i].nLastTouchLineTime = nSharedTime;
                    if (isSell) // 단계는 상승했는데 팔린다는 건 이상하니 
                    {
                        isSell = false;
                    }
                }
            }

            // 매도 결정
            if (ea[nCurIdx].fPower >= 0.29)
            {
                sbPaperSellDescription.Append($"상한가 도달!{NEW_LINE}");
                isSell = true;
            }

            if (isSell)
            {
                UpFakeCount(nCurIdx, PAPER_SELL_SIGNAL, i);
                frame.paperTradeSlot[i].isSelling = true;
                frame.paperTradeSlot[i].nBuyHogaVolume = ea[nCurIdx].nTotalBuyHogaVolume / 10;
                frame.paperTradeSlot[i].nSellRqVolume = frame.paperTradeSlot[i].nBuyedVolume;
                frame.paperTradeSlot[i].nSellRqTime = nSharedTime;
                frame.paperTradeSlot[i].nSellRqCount = ea[nCurIdx].nChegyulCnt;
                frame.paperTradeSlot[i].nSellRqPrice = ea[nCurIdx].nFb;
                frame.paperTradeSlot[i].sSellDescription = sbPaperSellDescription.ToString();
            }

        }
        #endregion

        #region SetThisPaperBuy
        private void SetThisPaperBuy(PaperBuyStrategy frame, int nEaIdx, int nPaperBuyStrategyNum, TradeMethodCategory methodCategory = TradeMethodCategory.RisingMethod)
        {
            try
            {

                if (frame.nStrategyNum >= PAPER_TRADE_MAX_NUM || frame.arrStrategy[nPaperBuyStrategyNum] > 2 || ea[nEaIdx].fPower > 0.27) // 한 전략당 3번제한(nExtraChance 미포함)
                    return;

                int nMaxNumToBuy = (int)(STANDARD_BUY_PRICE * NORMAL_TRADE_RATIO) / ea[nEaIdx].nFs; // 최대매수가능금액으로 살 수 있을 만큼 

                if (nMaxNumToBuy <= 0)
                    return;

                #region 모의매수 세팅
                frame.paperTradeSlot[frame.nStrategyNum].nRqPrice = ea[nEaIdx].nFs;
                frame.paperTradeSlot[frame.nStrategyNum].nOverPrice = ea[nEaIdx].nFb;
                for (int i = 0; i < EYES_CLOSE_NUM; i++)
                    frame.paperTradeSlot[frame.nStrategyNum].nOverPrice += GetIntegratedMarketGap(frame.paperTradeSlot[frame.nStrategyNum].nOverPrice);
                frame.paperTradeSlot[frame.nStrategyNum].nRqTime = nSharedTime;
                frame.paperTradeSlot[frame.nStrategyNum].nRqVolume = nMaxNumToBuy;
                frame.paperTradeSlot[frame.nStrategyNum].nRqTimeLineIdx= nTimeLineIdx;
                frame.paperTradeSlot[frame.nStrategyNum].nTargetRqVolume = nMaxNumToBuy;
                frame.paperTradeSlot[frame.nStrategyNum].nSellHogaVolume = ea[nEaIdx].nThreeSellHogaVolume / 3;
                frame.paperTradeSlot[frame.nStrategyNum].nCanceledVolume = 0;
                frame.paperTradeSlot[frame.nStrategyNum].nRqCount = ea[nEaIdx].nChegyulCnt;
                frame.paperTradeSlot[frame.nStrategyNum].methodCategory = methodCategory;
                frame.paperTradeSlot[frame.nStrategyNum].fTargetPer = GetNextCeiling(frame.paperTradeSlot[frame.nStrategyNum].nCurLineIdx);
                frame.paperTradeSlot[frame.nStrategyNum].fBottomPer = GetNextFloor(frame.paperTradeSlot[frame.nStrategyNum].nCurLineIdx, frame.paperTradeSlot[frame.nStrategyNum].methodCategory);
                frame.paperTradeSlot[frame.nStrategyNum].sFixedMsg = ea[nEaIdx].GetInfoString();
                frame.paperTradeSlot[frame.nStrategyNum].nSequence = frame.arrStrategy[nPaperBuyStrategyNum] + 1;
                frame.paperTradeSlot[frame.nStrategyNum].nLastTouchLineTime = nSharedTime;

                strategyHistoryList[nPaperBuyStrategyNum].Add(new StrategyHistory(nEaIdx, frame.nStrategyNum)); // 전략리스트 인덱스에 맞게 삽입
                totalTradeHistoryList.Add(new StrategyHistory(nEaIdx, frame.nStrategyNum)); // 전체 매매리스트
                #endregion

                bool isFakeSet = SetThisFake(ea[nEaIdx].paperBuyStrategy, nEaIdx, nPaperBuyStrategyNum);


                frame.isOrderCheck = true;


#if AI

                double[] features102 = GetParameters(nCurIdx: nEaIdx, 102, nTradeMethod: PAPER_BUY_SIGNAL, nRealStrategyNum: nPaperBuyStrategyNum);

                var nMMFNum = mmf.RequestAIService(sCode: ea[nEaIdx].sCode, nRqTime: nSharedTime, nRqType: PAPER_BUY_SIGNAL, inputData: features102);
                if (nMMFNum == -1)
                {
                    PrintLog($"{nSharedTime} AI Service Slot이 부족합니다.");
                    return;
                }
                aiSlot.nEaIdx = nEaIdx;
                aiSlot.nRequestId = PAPER_BUY_SIGNAL;
                aiSlot.nMMFNumber = nMMFNum;

                aiQueue.Enqueue(aiSlot);

#endif
                //  PrintLog($"[모의매수] 시간 : {nSharedTime}, 종목코드 : {ea[nEaIdx].sCode} 종목명 : {ea[nEaIdx].sCodeName}, 현재가 : {ea[nEaIdx].nFs} 전략 : {nPaperBuyStrategyNum} {strategyName.arrPaperBuyStrategyName[nPaperBuyStrategyNum]} 매수신청");
            }
            catch (Exception ex)
            {
                PrintLog($"[모의매수] 체크 중 오류 발생 {ea[nEaIdx].sCode} {ea[nEaIdx].sCodeName} {nPaperBuyStrategyNum}");
            }
        }
        #endregion

        #region UpdateFakeHistory
        public void UpdateFakeHistory(int nCurIdx)
        {
            //for (int i = 0; i < ea[nCurIdx].myStrategy.listFakeHistoryPiece.Count; i++)
            //{
            //    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nSharedTime) > 900) // 15분이 넘은 건 삭제해라
            //    {
            //        ea[nCurIdx].myStrategy.listFakeHistoryPiece.RemoveAt(i);
            //        i--;
            //    }
            //}
        }
        #endregion

        #region AddFakeHistory
        public void AddFakeHistory(int nType, int nCurIdx, int nStrategyNum)
        {
            FakeHistoryPiece tmpFakeHistoryPiece;
            tmpFakeHistoryPiece.nTypeFakeTrading = nType;
            tmpFakeHistoryPiece.nFakeStrategyNum = nStrategyNum;
            tmpFakeHistoryPiece.nSharedTime = nSharedTime;
            tmpFakeHistoryPiece.nTimeLineIdx = nTimeLineIdx;
            ea[nCurIdx].fakeStrategyMgr.listFakeHistoryPiece.Add(tmpFakeHistoryPiece); // 신규 데이터를 삽입해라

        }
        #endregion

        #region CalcFakeHistory 
        public void CalcFakeHistory(int nEaIdx)
        {

            ea[nEaIdx].fakeStrategyMgr.nFakeBuyNum = 0;
            ea[nEaIdx].fakeStrategyMgr.nFakeResistNum = 0;
            ea[nEaIdx].fakeStrategyMgr.nFakeAssistantNum = 0;
            ea[nEaIdx].fakeStrategyMgr.nFakeVolatilityNum = 0;
            ea[nEaIdx].fakeStrategyMgr.nFakeBuyMinuteAreaNum = 0;
            ea[nEaIdx].fakeStrategyMgr.nFakeResistMinuteAreaNum = 0;
            ea[nEaIdx].fakeStrategyMgr.nFakeAssistantMinuteAreaNum = 0;
            ea[nEaIdx].fakeStrategyMgr.nTotalFakeMinuteAreaNum = 0;
            ea[nEaIdx].fakeStrategyMgr.nFakeVolatilityMinuteAreaNum = 0;
            ea[nEaIdx].fakeStrategyMgr.nFakeDownMinuteAreaNum = 0;

            int nPrevFakeBuyMinuteIdx = -1;
            int nPrevFakeResistMinuteIdx = -1;
            int nPrevFakeAssistantMinuteIdx = -1;
            int nPrevFakeVolatilityMinuteIdx = -1;
            int nPrevFakeDownMinuteIdx = -1;
            int nPrevTotalFakeMinuteIdx = -1;

            for (int i = 0; i < ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece.Count; i++)
            {
                if (ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTypeFakeTrading == FAKE_BUY_SIGNAL)
                {
                    ea[nEaIdx].fakeStrategyMgr.nFakeBuyNum++;

                    if (nPrevFakeBuyMinuteIdx == -1 || nPrevFakeBuyMinuteIdx != ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevFakeBuyMinuteIdx = ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nEaIdx].fakeStrategyMgr.nFakeBuyMinuteAreaNum++;
                    }
                }
                else if (ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTypeFakeTrading == FAKE_RESIST_SIGNAL)
                {
                    ea[nEaIdx].fakeStrategyMgr.nFakeResistNum++;

                    if (nPrevFakeResistMinuteIdx == -1 || nPrevFakeResistMinuteIdx != ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevFakeResistMinuteIdx = ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nEaIdx].fakeStrategyMgr.nFakeResistMinuteAreaNum++;
                    }
                }
                else if (ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTypeFakeTrading == FAKE_ASSISTANT_SIGNAL)
                {
                    ea[nEaIdx].fakeStrategyMgr.nFakeAssistantNum++;

                    if (nPrevFakeAssistantMinuteIdx == -1 || nPrevFakeAssistantMinuteIdx != ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevFakeAssistantMinuteIdx = ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nEaIdx].fakeStrategyMgr.nFakeAssistantMinuteAreaNum++;
                    }
                }
                else if (ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTypeFakeTrading == FAKE_VOLATILE_SIGNAL)
                {
                    ea[nEaIdx].fakeStrategyMgr.nFakeVolatilityNum++;

                    if (nPrevFakeVolatilityMinuteIdx == -1 || nPrevFakeVolatilityMinuteIdx != ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevFakeVolatilityMinuteIdx = ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nEaIdx].fakeStrategyMgr.nFakeVolatilityMinuteAreaNum++;
                    }
                }
                else if (ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTypeFakeTrading == FAKE_DOWN_SIGNAL)
                {
                    ea[nEaIdx].fakeStrategyMgr.nFakeVolatilityNum++;

                    if (nPrevFakeDownMinuteIdx == -1 || nPrevFakeDownMinuteIdx != ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevFakeDownMinuteIdx = ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nEaIdx].fakeStrategyMgr.nFakeDownMinuteAreaNum++;
                    }
                }

                // total min idx 부분
                if (nPrevTotalFakeMinuteIdx == -1 || nPrevTotalFakeMinuteIdx != ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx)
                {
                    nPrevTotalFakeMinuteIdx = ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece[i].nTimeLineIdx;
                    ea[nEaIdx].fakeStrategyMgr.nTotalFakeMinuteAreaNum++;
                }
            }
        }
        #endregion

        #region GetAccess
        public bool GetAccess(FakeFrame frame, int nStrategy, int? nTrial = null, int? nCycle = null)
        {
            bool isRet;

            // 사이클이 없다면 기본 도전 1회이다.
            if (nCycle == null)
                isRet = frame.arrPrevMinuteIdx[nStrategy] < nTimeLineIdx &&
                    frame.arrStrategy[nStrategy] < ((nTrial != null) ? (int)nTrial : 1);
            else
                isRet = ((nTrial != null) ? frame.arrStrategy[nStrategy] < (int)nTrial : true) &&
                    (frame.arrPrevMinuteIdx[nStrategy] == 0 || frame.arrPrevMinuteIdx[nStrategy] + (int)nCycle - 1 < nTimeLineIdx);

            return isRet;
        }

        #endregion

        #region Memory Usage Print
        public void PrintMemoryUsage()
        {
            int nMB = 1024 * 1024;
            Process curProcess = Process.GetCurrentProcess();
            PrintLog($"Memory Usage - WorkingSet64 : {(double)curProcess.WorkingSet64 / nMB}(MB){NEW_LINE}Memory Usage - PrivateMemorySize64 : {(double)curProcess.PrivateMemorySize64 / nMB}(MB){NEW_LINE}Memory Usage - VirtualMemorySize64 : {(double)curProcess.VirtualMemorySize64 / nMB}(MB){NEW_LINE}Memory Usage - PagedMemorySize64 : {(double)curProcess.PagedMemorySize64 / nMB}(MB){NEW_LINE}");
        }
        #endregion



        #region ShutOff MMF Slot
        public void TurnOffMMFSlot(int nMMFSlot)
        {
#if AI
            if (nMMFSlot != -1)
            {
                mmf.checkingRequestArray[nMMFSlot] = false;
                mmf.checkingComeArray[nMMFSlot] = false;
            }
#endif
        }
        #endregion

        public long GetHeavyPrice(long lMarketCap)
        {
            long lHeavyPrice = 0;
            if (lMarketCap >= TRILLION)
            {
                lHeavyPrice += lMarketCap / TRILLION * BILLION; // 조당 10억씩 추가
            }
            return lHeavyPrice;
        }

        public long GetHeavyCount(long lMarketCap)
        {
            long lHeavyCount = 0;
            if (lMarketCap >= TRILLION)
            {
                lHeavyCount += lMarketCap / TRILLION * 200; // 조당 200회 추가
            }
            return lHeavyCount;
        }

    

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

        #region 미처분 매매블록화
        public bool isUndisposalHandle = true;
        public void BlockizeUndisposal()
        {
            if (isUndisposalHandle)
            {
                isUndisposalHandle = false;

                #region 미처분 매매블록화
                StringBuilder tmpSB = new StringBuilder();
                for (int nUndisposalIdx = 0; nUndisposalIdx < nHoldingCnt; nUndisposalIdx++)
                {
                    // 미처분 가격처리

                    nYesterdayUndisposalBuyPrice += holdingsArray[nUndisposalIdx].nBuyedPrice * holdingsArray[nUndisposalIdx].nHoldingQty;

                    if (holdingsArray[nUndisposalIdx].nNumPossibleToSell != holdingsArray[nUndisposalIdx].nHoldingQty)
                        tmpSB.Append($"{nUndisposalIdx + 1}번째  {holdingsArray[nUndisposalIdx].sCode}  {holdingsArray[nUndisposalIdx].sCodeName} 매매가능수량 : {holdingsArray[nUndisposalIdx].nNumPossibleToSell}(주)가 보유수량 : {holdingsArray[nUndisposalIdx].nHoldingQty}(주)와 같지 않습니다.{NEW_LINE}");

                    if (holdingsArray[nUndisposalIdx].nNumPossibleToSell > 0)
                    {
                        int nCurIdx = eachStockDict[holdingsArray[nUndisposalIdx].sCode.Trim()];
                        int nSlotIdx = ea[nCurIdx].myTradeManager.arrBuyedSlots.Count;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots.Add(new BuyedSlot(nCurIdx));
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyedSlotId = nSlotIdx;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyedSumPrice = holdingsArray[nUndisposalIdx].nNumPossibleToSell * holdingsArray[nUndisposalIdx].nBuyedPrice;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].isAllBuyed = true;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBirthPrice = holdingsArray[nUndisposalIdx].nBuyedPrice;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyPrice = holdingsArray[nUndisposalIdx].nBuyedPrice;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nCurVolume = holdingsArray[nUndisposalIdx].nNumPossibleToSell;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyVolume = holdingsArray[nUndisposalIdx].nNumPossibleToSell;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBirthTime = nFirstTime;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nLastTouchLineTime = nFirstTime;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyMinuteIdx = BRUSH - 1 ;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyEndTime = nFirstTime;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].sBuyDescription = $"미처분 매매블록{NEW_LINE}";
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].sBuyScrNo = null;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].sSellScrNo = null;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].eTradeMethod = TradeMethodCategory.RisingMethod;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nCurLineIdx = 0;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].fTargetPer = GetNextCeiling(ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nCurLineIdx);
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].fBottomPer = GetNextFloor(ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nCurLineIdx, ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].eTradeMethod);
                        PrintLog($"미처분 매매블록화 성공  체결가 : {holdingsArray[nUndisposalIdx].nBuyedPrice}  체결량 : {holdingsArray[nUndisposalIdx].nHoldingQty}  매매가능 : { holdingsArray[nUndisposalIdx].nNumPossibleToSell}", nCurIdx, nSlotIdx, false);

                        ea[nCurIdx].myTradeManager.nTotalBuyed += ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nCurVolume;

                        tmpSB.Append($"{nUndisposalIdx + 1}번째 미처분 매매블록 생성 : {nFirstTime}  {holdingsArray[nUndisposalIdx].sCode}  {holdingsArray[nUndisposalIdx].sCodeName}  {holdingsArray[nUndisposalIdx].nNumPossibleToSell} {holdingsArray[nUndisposalIdx].nBuyedPrice}{NEW_LINE}");
                    }
                    


                }
                #endregion
                PrintLog(tmpSB.ToString());
            }
            else
            {
                PrintLog($"미처분 매매블록화가 이미 완료됐습니다.");
            }
        }
        #endregion

    }
}
