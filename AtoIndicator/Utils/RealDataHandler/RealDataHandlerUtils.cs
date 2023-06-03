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
            if (nFakeNum != EVERY_SIGNAL)
            {
                if (nFakeNum != PAPER_SELL_SIGNAL)
                {
                    if (nFakeNum != PAPER_BUY_SIGNAL)
                        ea[nEaIdx].fakeStrategyMgr.nTotalFakeCount++; // 실매수 미포함
                    ea[nEaIdx].fakeStrategyMgr.nTotalArrowCount++; // 실매수 포함 
                }
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
                    newF.fr.nBuyStrategyIdx = strategyNameDict[(PAPER_SELL_SIGNAL, strategyName.arrPaperSellStrategyName[nBuyStrategyNum])]; // key
                    newF.fr.nBuyStrategyGroupNum = PAPER_SELL_SIGNAL; // key
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].paperSellStrategy.arrStrategy[nBuyStrategyNum]; // key
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

            if (nFakeNum != PAPER_SELL_SIGNAL)
            {
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
        }

        #region SetThisFake
        bool SetThisFake(FakeFrame frame, int nEaIdx, int nFakeBuyStrategyNum)
        {
            if (frame.nFakeType != PAPER_BUY_SIGNAL && frame.nFakeType != PAPER_SELL_SIGNAL)
            {
                if (frame.nStrategyNum >= FAKE_BUY_MAX_NUM || frame.arrStrategy[nFakeBuyStrategyNum] > 5) // 한 전략당 6번제한
                    return false;

                
            }

            #region 공용 파트
            if (frame.nFakeType != PAPER_SELL_SIGNAL && ea[nEaIdx].fakeStrategyMgr.nSharedPrevMinuteIdx != nTimeLineIdx)
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


            if (frame.nFakeType != PAPER_BUY_SIGNAL && frame.nFakeType != PAPER_SELL_SIGNAL)
            {
                UpdateFakeHistory(nEaIdx);
                AddFakeHistory(frame.nFakeType, nEaIdx, nFakeBuyStrategyNum);
                CalcFakeHistory(nEaIdx);

                UpFakeCount(nEaIdx, frame.nFakeType, nFakeBuyStrategyNum);
            }

            return true;
        }

        #endregion

        #region SetThisPaperBuy
        private void SetThisPaperBuy(PaperBuyStrategy frame, int nEaIdx, int nPaperBuyStrategyNum)
        {
            try
            {
                #region 실매수요청 접근시점 기록 및 처리

                if (ea[nEaIdx].myTradeManager.nIdx >= PAPER_BUY_MAX_NUM || frame.arrStrategy[nPaperBuyStrategyNum] > 2 || ea[nEaIdx].fPower > 0.27) // 한 전략당 3번제한(nExtraChance 미포함)
                    return;


                frame.arrRqPrice[frame.nStrategyNum] = ea[nEaIdx].nFs;
                frame.arrRqTime[frame.nStrategyNum] = nSharedTime;

                bool isFakeSet = SetThisFake(ea[nEaIdx].paperBuyStrategy, nEaIdx, nPaperBuyStrategyNum);

                frame.isOrderCheck = true;
                #endregion

                



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
                PrintLog($"[모의매수] 시간 : {nSharedTime}, 종목코드 : {ea[nEaIdx].sCode} 종목명 : {ea[nEaIdx].sCodeName}, 현재가 : {ea[nEaIdx].nFs} 전략 : {nPaperBuyStrategyNum} {strategyName.arrPaperBuyStrategyName[nPaperBuyStrategyNum]} 매수신청");
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

        public void ClearReservation(int nEaIdx)
        {
            ea[nEaIdx].manualReserve.fReserveCheckPrice = 0;
            ea[nEaIdx].manualReserve.fReserveCheckPrice2 = 0;
            ea[nEaIdx].manualReserve.nReserveCheckVersion = NONE_RESERVE;
            ea[nEaIdx].manualReserve.nReserveCheckTime = 0;
        }
    }
}
