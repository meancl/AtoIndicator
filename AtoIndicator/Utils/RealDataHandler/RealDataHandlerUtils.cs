using AtoTrader.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtoTrader.KiwoomLib.TimeLib;
using static AtoTrader.TradingBlock.TimeLineGenerator;
using static AtoTrader.KiwoomLib.PricingLib;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;

namespace AtoTrader
{
    public partial class MainForm
    {

        void UpFakeCount(int nEaIdx, int nFakeNum, int nBuyStrategyNum)
        {
            try
            {
                ea[nEaIdx].fakeStrategyMgr.nTotalFakeCount++;
                FakeDBRecordInfo newF = new FakeDBRecordInfo();
                ea[nEaIdx].GetFakeFix(newF.fr);

                newF.fr.nRqTime = nSharedTime;
                newF.fr.nOverPrice = newF.fr.nFb;
                for (int i = 0; i < EYES_CLOSE_NUM; i++)
                    newF.fr.nOverPrice += GetIntegratedMarketGap(newF.fr.nOverPrice);
                newF.nTimeLineIdx = nTimeLineIdx;
                newF.fr.nAccessFakeStrategyIdx = nBuyStrategyNum;

                switch (nFakeNum)
                {
                    case FAKE_BUY_SIGNAL:
                        newF.fr.nAccessFakeStrategyGroupNum = -10 * (FAKE_BUY_SIGNAL + 1);
                        newF.fr.nAccessFakeStrategySequenceIdx = ea[nEaIdx].fakeBuyStrategy.arrStrategy[nBuyStrategyNum];
                        PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 1. 페이크매수 누적 횟수 : {ea[nEaIdx].fakeBuyStrategy.nStrategyNum} 페이크매수 분포 : {ea[nEaIdx].fakeBuyStrategy.nMinuteLocationCount}", nEaIdx);
                        break;
                    case FAKE_RESIST_SIGNAL:
                        newF.fr.nAccessFakeStrategyGroupNum = -10 * (FAKE_RESIST_SIGNAL + 1);
                        newF.fr.nAccessFakeStrategySequenceIdx = ea[nEaIdx].fakeResistStrategy.arrStrategy[nBuyStrategyNum];
                        PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 2. 페이크저항 누적 횟수 : {ea[nEaIdx].fakeResistStrategy.nStrategyNum} 페이크저항 분포 : {ea[nEaIdx].fakeResistStrategy.nMinuteLocationCount}", nEaIdx);
                        break;
                    case FAKE_ASSISTANT_SIGNAL:
                        newF.fr.nAccessFakeStrategyGroupNum = -10 * (FAKE_ASSISTANT_SIGNAL + 1);
                        newF.fr.nAccessFakeStrategySequenceIdx = ea[nEaIdx].fakeAssistantStrategy.arrStrategy[nBuyStrategyNum];
                        PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 3. 페이크보조 누적 횟수 : {ea[nEaIdx].fakeAssistantStrategy.nStrategyNum} 페이크보조 분포 : {ea[nEaIdx].fakeAssistantStrategy.nMinuteLocationCount}", nEaIdx);
                        break;
                    case FAKE_VOLATILE_SIGNAL:
                        newF.fr.nAccessFakeStrategyGroupNum = -10 * (FAKE_VOLATILE_SIGNAL + 1);
                        newF.fr.nAccessFakeStrategySequenceIdx = ea[nEaIdx].fakeVolatilityStrategy.arrStrategy[nBuyStrategyNum];
                        PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 6. 변동성 누적 횟수 : {ea[nEaIdx].fakeVolatilityStrategy.nStrategyNum} 실제 매수 분포 : {ea[nEaIdx].fakeVolatilityStrategy.nMinuteLocationCount}", nEaIdx);
                        break;
                    default:
                        break;
                }

                ea[nEaIdx].fakeStrategyMgr.fd.Add(newF);

#if AI
                // AI 서비스 요청
                double[] features102 = GetParameters(nCurIdx: nEaIdx, 102, eTradeMethod: GET_FEATURE_FAKE, nRealStrategyNum: newF.fr.nAccessFakeStrategyGroupNum);

                var nMMFNum = mmf.RequestAIService(sCode: ea[nEaIdx].sCode, nRqTime: nSharedTime, nRqType: FAKE_AI_NUM, inputData: features102);
                if (nMMFNum == -1)
                {
                    PrintLog($"{nSharedTime} AI Service Slot이 부족합니다.");
                    return;
                }
                aiSlot.nEaIdx = nEaIdx;
                aiSlot.nRequestId = FAKE_REQUEST_SIGNAL;
                aiSlot.nMMFNumber = nMMFNum;
                aiQueue.Enqueue(aiSlot);
#endif
            }
            catch { }
        }

        #region SetThisFake
        void SetThisFake(FakeFrame frame, int nEaIdx, int nFakeBuyStrategyNum)
        {

            if (frame.nStrategyNum >= FAKE_BUY_MAX_NUM || frame.arrStrategy[nFakeBuyStrategyNum] > 5) // 한 전략당 6번제한
                return;

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

            if (frame.nPrevMinuteIdx != nTimeLineIdx)
            {
                frame.nPrevMinuteIdx = nTimeLineIdx;
                frame.nMinuteLocationCount++;
            }

            UpdateFakeHistory(nEaIdx);
            AddFakeHistory(nEaIdx, frame.nFakeType, nFakeBuyStrategyNum);
            CalcFakeHistory(nEaIdx);




            UpFakeCount(nEaIdx, frame.nFakeType, nFakeBuyStrategyNum);
        }
        #endregion


        #region UpdateFakeHistory
        public void UpdateFakeHistory(int nEaIdx)
        {
            //for (int i = 0; i < ea[nCurIdx].fakeVolatilityStrategy.listFakeHistoryPiece.Count; i++)
            //{
            //    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeVolatilityStrategy.listFakeHistoryPiece[i].nSharedTime) > 900) // 15분이 넘은 건 삭제해라
            //    {
            //        ea[nCurIdx].fakeVolatilityStrategy.listFakeHistoryPiece.RemoveAt(i);
            //        i--;
            //    }
            //}
        }
        #endregion

        #region AddFakeHistory
        public void AddFakeHistory(int nEaIdx, int nType, int nStrategyNum)
        {
            FakeHistoryPiece tmpFakeHistoryPiece;
            tmpFakeHistoryPiece.nTypeFakeTrading = nType;
            tmpFakeHistoryPiece.nFakeStrategyNum = nStrategyNum;
            tmpFakeHistoryPiece.nSharedTime = nSharedTime;
            tmpFakeHistoryPiece.nTimeLineIdx = nTimeLineIdx;
            ea[nEaIdx].fakeStrategyMgr.listFakeHistoryPiece.Add(tmpFakeHistoryPiece); // 신규 데이터를 삽입해라

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
            ea[nEaIdx].fakeStrategyMgr.nTotalFakeCount = 0;
            ea[nEaIdx].fakeStrategyMgr.nFakeVolatilityMinuteAreaNum = 0;

            int nPrevFakeBuyMinuteIdx = -1;
            int nPrevFakeResistMinuteIdx = -1;
            int nPrevFakeAssistantMinuteIdx = -1;
            int nPrevFakeVolatilityMinuteIdx = -1;
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
                        ea[nEaIdx].fakeStrategyMgr.nFakeAssistantMinuteAreaNum++;
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
                isRet = frame.arrPrevMinuteIdx[nStrategy] < nTimeLineIdx && frame.arrStrategy[nStrategy] < ((nTrial != null) ? (int)nTrial : 1);
            else
                isRet = ((nTrial != null) ? frame.arrStrategy[nStrategy] < (int)nTrial : true) && (frame.arrPrevMinuteIdx[nStrategy] == 0 || frame.arrPrevMinuteIdx[nStrategy] + (int)nCycle - 1 < nTimeLineIdx); // 30분 주기일 수 도 있으니

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
    }
}
