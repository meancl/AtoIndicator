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
                if (nFakeNum != REAL_BUY_SIGNAL)
                    ea[nEaIdx].fakeStrategyMgr.nTotalFakeCount++; // 실매수 미포함
                ea[nEaIdx].fakeStrategyMgr.nTotalArrowCount++; // 실매수 포함 
            }

            FakeDBRecordInfo newF = new FakeDBRecordInfo();
            ea[nEaIdx].GetFakeFix(newF.fr);

            newF.fr.nRqTime = nSharedTime;
            newF.fr.nOverPrice = newF.fr.nFb;
            if (newF.fr.nOverPrice == 0) // 값이 없다면..?
                newF.fr.nOverPrice = ea[nEaIdx].nCurHogaPrice;

            for (int i = 0; i < EYES_CLOSE_NUM; i++)
                newF.fr.nOverPrice += GetIntegratedMarketGap(newF.fr.nOverPrice);
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
                case REAL_BUY_SIGNAL:
                    newF.fr.nBuyStrategyIdx = strategyNameDict[(REAL_BUY_SIGNAL, strategyName.arrRealBuyStrategyName[nBuyStrategyNum])]; // key
                    newF.fr.nBuyStrategyGroupNum = REAL_BUY_SIGNAL; // key
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].realBuyStrategy.arrStrategy[nBuyStrategyNum]; // key
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
            if (frame.nFakeType != REAL_BUY_SIGNAL) // 페이크의 경우
            {
                if (frame.nStrategyNum >= FAKE_BUY_MAX_NUM || frame.arrStrategy[nFakeBuyStrategyNum] > 5) // 한 전략당 6번제한
                    return false;
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


            if (ea[nEaIdx].fakeStrategyMgr.nSharedPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nEaIdx].fakeStrategyMgr.nSharedPrevMinuteIdx = nTimeLineIdx;
                ea[nEaIdx].fakeStrategyMgr.nSharedMinuteLocationCount++;
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


            if (frame.nFakeType != REAL_BUY_SIGNAL) // 페이크의 경우
            {
                UpdateFakeHistory(nEaIdx);
                AddFakeHistory(frame.nFakeType, nEaIdx, nFakeBuyStrategyNum);
                CalcFakeHistory(nEaIdx);
            }

            UpFakeCount(nEaIdx, frame.nFakeType, nFakeBuyStrategyNum);
            return true;
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
        public bool GetAccess(FakeFrame frame, int nStrategy, int? nTrial = null, int? nCycle = null, int? nFailApprove = null)
        {
            bool isRet;

            // 사이클이 없다면 기본 도전 1회이다.
            if (nCycle == null)
                isRet = frame.arrPrevMinuteIdx[nStrategy] < nTimeLineIdx &&
                    frame.arrStrategy[nStrategy] < ((nTrial != null) ? (int)nTrial : 1) &&
                    (frame.nFakeType == REAL_BUY_SIGNAL && nFailApprove != null ? ((RealBuyStrategy)frame).arrReqFail[nStrategy] < (int)nFailApprove : true);
            else
                isRet = ((nTrial != null) ? frame.arrStrategy[nStrategy] < (int)nTrial : true) &&
                    (frame.arrPrevMinuteIdx[nStrategy] == 0 || frame.arrPrevMinuteIdx[nStrategy] + (int)nCycle - 1 < nTimeLineIdx) &&
                    (frame.nFakeType == REAL_BUY_SIGNAL && nFailApprove != null ? ((RealBuyStrategy)frame).arrReqFail[nStrategy] < (int)nFailApprove : true);

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

        #region SELL ALL
        public void SellALL(string sSellName = "일괄전부매도")
        {
            bool isAllSell = false;
            PrintLog("일괄매도를 시작합니다.");
            isBuyDeny = true;
            for (int id = 0; id < nStockLength; id++)
            {
                ea[id].isExcluded = true;
                for (int buyId = 0; buyId < ea[id].myTradeManager.nIdx; buyId++)
                {
                    if (ea[id].myTradeManager.arrBuyedSlots[buyId].isAllBuyed)
                    {
                        if (!ea[id].myTradeManager.arrBuyedSlots[buyId].isAllSelled && ea[id].myTradeManager.arrBuyedSlots[buyId].nCurVolume > 0 && !ea[id].myTradeManager.arrBuyedSlots[buyId].isSelling) // 처분이 안됐으면(근처에서 오류가 나서 일단 조건 많이 달아둠)
                        {
                            isAllSell = true;
                            SetAndServeCurSlot(sSellName, NEW_SELL, ea[id].sCode, buyId, "", id, 0, 0, 0, MARKET_ORDER, TradeMethodCategory.None, 0, ea[id].myTradeManager.arrBuyedSlots[buyId].nCurVolume, sSellName);
                        }
                    }
                    else
                    {
                        var sRq = "일괄매수취소";
                        SetAndServeCurSlot(sRq, BUY_CANCEL, ea[id].sCode, buyId, ea[id].myTradeManager.arrBuyedSlots[buyId].sCurOrgOrderId, id, 0, 0, 0, MARKET_ORDER, TradeMethodCategory.None, 0, 0, sRq);
                    }
                }
            }
            if (!isAllSell)
                PrintLog("매도물량이 없습니다.");
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
                        int nSlotIdx = ea[nCurIdx].myTradeManager.nIdx++;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots.Add(new BuyedSlot());
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyedSlotId = nSlotIdx;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyedSumPrice = holdingsArray[nUndisposalIdx].nNumPossibleToSell * holdingsArray[nUndisposalIdx].nBuyedPrice;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].isAllBuyed = true;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBirthPrice = holdingsArray[nUndisposalIdx].nBuyedPrice;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyPrice = holdingsArray[nUndisposalIdx].nBuyedPrice;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nCurVolume = holdingsArray[nUndisposalIdx].nNumPossibleToSell;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyVolume = holdingsArray[nUndisposalIdx].nNumPossibleToSell;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBirthTime = nFirstTime;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nLastTouchLineTime = nFirstTime;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyMinuteIdx = BRUSH;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nBuyEndTime = nFirstTime;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].sBuyDescription = $"미처분 매매블록{NEW_LINE}";
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].sBuyScrNo = null;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].sSellScrNo = null;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].eTradeMethod = TradeMethodCategory.RisingMethod;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nCurLineIdx = 0;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].fTargetPer = GetNextCeiling(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nCurLineIdx);
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].fBottomPer = GetNextFloor(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nCurLineIdx, ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].eTradeMethod);
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nSlotIdx].nStrategyIdx = UNDISPOSAL_STRATEGY_IDX;
                        PrintLog($"미처분 매매블록화 성공  체결가 : {holdingsArray[nUndisposalIdx].nBuyedPrice}  체결량 : {holdingsArray[nUndisposalIdx].nHoldingQty}  매매가능 : { holdingsArray[nUndisposalIdx].nNumPossibleToSell}", nCurIdx, nSlotIdx, false);


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
