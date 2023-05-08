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

        void UpFakeCount(int nEaIdx, int nFakeNum, int nBuyStrategyNum= -1)
        {
            ea[nEaIdx].myStrategy.nTotalBlockCount++;
            FakeDBStruct newF = new FakeDBStruct();
            ea[nEaIdx].GetFakeFix(newF.fr);

            newF.fr.nRqTime = nSharedTime;
            newF.fr.nOverPrice = newF.fr.nFb;
            for (int i = 0; i < EYES_CLOSE_NUM; i++)
                newF.fr.nOverPrice += GetIntegratedMarketGap(newF.fr.nOverPrice);
            newF.nTimeLineIdx = nTimeLineIdx;

            switch (nFakeNum)
            {
                case FAKE_BUY_SIGNAL:
                    ea[nEaIdx].myStrategy.nTotalFakeCount++;
                    newF.fr.nBuyStrategyIdx = -10 * (FAKE_BUY_SIGNAL + 1);
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].fakeBuyStrategy.nStrategyNum;
                    PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 1. 페이크매수 누적 횟수 : {ea[nEaIdx].fakeBuyStrategy.nStrategyNum} 페이크매수 분포 : {ea[nEaIdx].fakeBuyStrategy.nMinuteLocationCount}", nEaIdx);
                    break;
                case FAKE_RESIST_SIGNAL:
                    ea[nEaIdx].myStrategy.nTotalFakeCount++;
                    newF.fr.nBuyStrategyIdx = -10 * (FAKE_RESIST_SIGNAL + 1);
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].fakeResistStrategy.nStrategyNum;
                    PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 2. 페이크저항 누적 횟수 : {ea[nEaIdx].fakeResistStrategy.nStrategyNum} 페이크저항 분포 : {ea[nEaIdx].fakeResistStrategy.nMinuteLocationCount}", nEaIdx);
                    break;
                case FAKE_ASSISTANT_SIGNAL:
                    ea[nEaIdx].myStrategy.nTotalFakeCount++;
                    newF.fr.nBuyStrategyIdx = -10 * (FAKE_ASSISTANT_SIGNAL + 1);
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].fakeAssistantStrategy.nStrategyNum;
                    PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 3. 페이크보조 누적 횟수 : {ea[nEaIdx].fakeAssistantStrategy.nStrategyNum} 페이크보조 분포 : {ea[nEaIdx].fakeAssistantStrategy.nMinuteLocationCount}", nEaIdx);
                    break;
                case PRICE_UP_SIGNAL:
                    ea[nEaIdx].myStrategy.nTotalFakeCount++;
                    newF.fr.nBuyStrategyIdx = -10 * (PRICE_UP_SIGNAL + 1);
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].priceUpStrategy.nStrategyNum;
                    PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 4. 페이크가격업 누적 횟수 : {ea[nEaIdx].priceUpStrategy.nStrategyNum} 페이크가격업 분포 : {ea[nEaIdx].priceUpStrategy.nMinuteLocationCount}", nEaIdx);
                    break;
                case PRICE_DOWN_SIGNAL:
                    ea[nEaIdx].myStrategy.nTotalFakeCount++;
                    newF.fr.nBuyStrategyIdx = -10 * (PRICE_DOWN_SIGNAL + 1);
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].priceDownStrategy.nStrategyNum;
                    PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 5. 페이크가격다운 누적 횟수 : {ea[nEaIdx].priceDownStrategy.nStrategyNum} 페이크가격다운 분포 : {ea[nEaIdx].priceDownStrategy.nMinuteLocationCount}", nEaIdx);
                    break;
                case REAL_BUY_SIGNAL:
                    newF.fr.nBuyStrategyIdx = strategyNameDict[strategyName.arrRealBuyStrategyName[nBuyStrategyNum]];
                    newF.fr.nBuyStrategySequenceIdx = ea[nEaIdx].myStrategy.nStrategyNum;
                    PrintLog($"{nSharedTime}  {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} fs : {ea[nEaIdx].nFs} 6. 실제 매수 누적 횟수 : {ea[nEaIdx].myStrategy.nStrategyNum} 실제 매수 분포 : {ea[nEaIdx].myStrategy.nMinuteLocationCount}", nEaIdx);
                    break;
                default:
                    break;
            }

            ea[nEaIdx].myStrategy.fd.Add(newF);

#if AI
            // AI 서비스 요청
            double[] features102 = GetParameters(nCurIdx: nCurIdx, 102, eTradeMethod: GET_FEATURE_FAKE, nRealStrategyNum: newF.fr.nBuyStrategyIdx);

            var nMMFNum = mmf.RequestAIService(sCode: ea[nCurIdx].sCode, nRqTime: nSharedTime, nRqType: FAKE_AI_NUM, inputData: features102);
            if (nMMFNum == -1)
            {
                PrintLog($"{nSharedTime} AI Service Slot이 부족합니다.");
                return;
            }
            aiSlot.nRequestId = FAKE_REQUEST_SIGNAL;
            aiSlot.nMMFNumber = nMMFNum;
            aiQueue.Enqueue(aiSlot);
#endif
        }
        #region SetThisFakeBuy
        void SetThisFakeBuy(int nFakeBuyStrategyNum)
        {
            if (ea[nCurIdx].fakeBuyStrategy.nStrategyNum >= FAKE_BUY_MAX_NUM || ea[nCurIdx].fakeBuyStrategy.arrStrategy[nFakeBuyStrategyNum] > 5) // 한 전략당 6번제한
                return;

            if (ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].myStrategy.nSharedMinuteLocationCount++;
            }

            if (ea[nCurIdx].fakeBuyStrategy.nLastTouchTime != 0 && SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeBuyStrategy.nLastTouchTime) >= 2400) // 40분 이상 매수가 안됐었으면
            {
                ea[nCurIdx].fakeBuyStrategy.isSuddenBoom = true;
            }
            ea[nCurIdx].fakeBuyStrategy.nLastTouchTime = nSharedTime;

            ea[nCurIdx].fakeBuyStrategy.arrLastTouch[nFakeBuyStrategyNum] = nSharedTime;
            ea[nCurIdx].fakeBuyStrategy.arrStrategy[nFakeBuyStrategyNum]++;
            ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyNum] = nTimeLineIdx;

            ea[nCurIdx].fakeBuyStrategy.arrBuyPrice[ea[nCurIdx].fakeBuyStrategy.nStrategyNum] = ea[nCurIdx].nFs;
            ea[nCurIdx].fakeBuyStrategy.arrBuyTime[ea[nCurIdx].fakeBuyStrategy.nStrategyNum] = nSharedTime;
            ea[nCurIdx].fakeBuyStrategy.arrMinuteIdx[ea[nCurIdx].fakeBuyStrategy.nStrategyNum] = nTimeLineIdx;
            ea[nCurIdx].fakeBuyStrategy.arrSpecificStrategy[ea[nCurIdx].fakeBuyStrategy.nStrategyNum] = nFakeBuyStrategyNum;
            ea[nCurIdx].fakeBuyStrategy.nStrategyNum++;
            ea[nCurIdx].fakeBuyStrategy.nHitNum++;

            ea[nCurIdx].fakeBuyStrategy.fEverageShoulderPrice = (ea[nCurIdx].fakeBuyStrategy.fEverageShoulderPrice == 0) ? ea[nCurIdx].nFs : (ea[nCurIdx].nFs + ea[nCurIdx].fakeBuyStrategy.fEverageShoulderPrice) / 2;
            ea[nCurIdx].fakeBuyStrategy.nSumShoulderPrice += ea[nCurIdx].nFs;
            if (ea[nCurIdx].fakeBuyStrategy.nMaxShoulderPrice == 0 || ea[nCurIdx].fakeBuyStrategy.nMaxShoulderPrice < ea[nCurIdx].nFs)
            {
                ea[nCurIdx].fakeBuyStrategy.nMaxShoulderPrice = ea[nCurIdx].nFs;

                if (nTimeLineIdx != ea[nCurIdx].fakeBuyStrategy.nPrevMaxMinIdx)
                {
                    ea[nCurIdx].fakeBuyStrategy.nPrevMaxMinIdx = nTimeLineIdx;
                    ea[nCurIdx].fakeBuyStrategy.nPrevMaxMinUpperCount++;
                }
                ea[nCurIdx].fakeBuyStrategy.nUpperCount++;
            }

            if (ea[nCurIdx].fakeBuyStrategy.nPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].fakeBuyStrategy.nPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].fakeBuyStrategy.nMinuteLocationCount++;
            }
            UpdateFakeHistory();
            AddFakeHistory(FAKE_BUY_SIGNAL, nFakeBuyStrategyNum);
            CalcFakeHistory();

            UpFakeCount(nCurIdx, FAKE_BUY_SIGNAL);
        }
        #endregion

        #region SetThisFakeAssistant
        void SetThisFakeAssistant(int nFakeAssistantStrategyNum)
        {
            if (ea[nCurIdx].fakeAssistantStrategy.nStrategyNum >= FAKE_ASSISTANT_MAX_NUM || ea[nCurIdx].fakeAssistantStrategy.arrStrategy[nFakeAssistantStrategyNum] > 5) // 한 전략당 6번제한
                return;


            if (ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].myStrategy.nSharedMinuteLocationCount++;
            }

            if (ea[nCurIdx].fakeAssistantStrategy.nLastTouchTime != 0 && SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeAssistantStrategy.nLastTouchTime) >= 2400) // 40분 이상 매수가 안됐었으면
            {
                ea[nCurIdx].fakeAssistantStrategy.isSuddenBoom = true;
            }
            ea[nCurIdx].fakeAssistantStrategy.nLastTouchTime = nSharedTime;

            ea[nCurIdx].fakeAssistantStrategy.arrLastTouch[nFakeAssistantStrategyNum] = nSharedTime;
            ea[nCurIdx].fakeAssistantStrategy.arrStrategy[nFakeAssistantStrategyNum]++;
            ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyNum] = nTimeLineIdx;

            ea[nCurIdx].fakeAssistantStrategy.arrAssistantPrice[ea[nCurIdx].fakeAssistantStrategy.nStrategyNum] = ea[nCurIdx].nFs;
            ea[nCurIdx].fakeAssistantStrategy.arrAssistantTime[ea[nCurIdx].fakeAssistantStrategy.nStrategyNum] = nSharedTime;
            ea[nCurIdx].fakeAssistantStrategy.arrMinuteIdx[ea[nCurIdx].fakeAssistantStrategy.nStrategyNum] = nTimeLineIdx;
            ea[nCurIdx].fakeAssistantStrategy.arrSpecificStrategy[ea[nCurIdx].fakeAssistantStrategy.nStrategyNum] = nFakeAssistantStrategyNum;
            ea[nCurIdx].fakeAssistantStrategy.nStrategyNum++;
            ea[nCurIdx].fakeAssistantStrategy.nHitNum++;


            ea[nCurIdx].fakeAssistantStrategy.fEverageShoulderPrice = (ea[nCurIdx].fakeAssistantStrategy.fEverageShoulderPrice == 0) ? ea[nCurIdx].nFs : (ea[nCurIdx].nFs + ea[nCurIdx].fakeAssistantStrategy.fEverageShoulderPrice) / 2;
            ea[nCurIdx].fakeAssistantStrategy.nSumShoulderPrice += ea[nCurIdx].nFs;

            if (ea[nCurIdx].fakeAssistantStrategy.nMaxShoulderPrice == 0 || ea[nCurIdx].fakeAssistantStrategy.nMaxShoulderPrice < ea[nCurIdx].nFs)
            {
                ea[nCurIdx].fakeAssistantStrategy.nMaxShoulderPrice = ea[nCurIdx].nFs;

                if (nTimeLineIdx != ea[nCurIdx].fakeAssistantStrategy.nPrevMaxMinIdx)
                {
                    ea[nCurIdx].fakeAssistantStrategy.nPrevMaxMinIdx = nTimeLineIdx;
                    ea[nCurIdx].fakeAssistantStrategy.nPrevMaxMinUpperCount++;
                }
                ea[nCurIdx].fakeAssistantStrategy.nUpperCount++;
            }

            if (ea[nCurIdx].fakeAssistantStrategy.nPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].fakeAssistantStrategy.nPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].fakeAssistantStrategy.nMinuteLocationCount++;
            }

            UpdateFakeHistory();
            AddFakeHistory(FAKE_ASSISTANT_SIGNAL, nFakeAssistantStrategyNum);
            CalcFakeHistory();
            UpFakeCount(nCurIdx, FAKE_ASSISTANT_SIGNAL);
        }
        #endregion

        #region SetThisFakeResist
        void SetThisFakeResist(int nFakeResistStrategyNum)
        {
            if (ea[nCurIdx].fakeResistStrategy.nStrategyNum >= FAKE_RESIST_MAX_NUM || ea[nCurIdx].fakeResistStrategy.arrStrategy[nFakeResistStrategyNum] > 5) // 한 전략당 6번제한
                return;


            if (ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].myStrategy.nSharedMinuteLocationCount++;
            }

            if (ea[nCurIdx].fakeResistStrategy.nLastTouchTime != 0 && SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeResistStrategy.nLastTouchTime) >= 2400) // 40분 이상 매수가 안됐었으면
            {
                ea[nCurIdx].fakeResistStrategy.isSuddenBoom = true;
            }
            ea[nCurIdx].fakeResistStrategy.nLastTouchTime = nSharedTime;

            ea[nCurIdx].fakeResistStrategy.arrLastTouch[nFakeResistStrategyNum] = nSharedTime;
            ea[nCurIdx].fakeResistStrategy.arrStrategy[nFakeResistStrategyNum]++;
            ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyNum] = nTimeLineIdx;

            ea[nCurIdx].fakeResistStrategy.arrResistPrice[ea[nCurIdx].fakeResistStrategy.nStrategyNum] = ea[nCurIdx].nFs;
            ea[nCurIdx].fakeResistStrategy.arrResistTime[ea[nCurIdx].fakeResistStrategy.nStrategyNum] = nSharedTime;
            ea[nCurIdx].fakeResistStrategy.arrMinuteIdx[ea[nCurIdx].fakeResistStrategy.nStrategyNum] = nTimeLineIdx;

            ea[nCurIdx].fakeResistStrategy.arrSpecificStrategy[ea[nCurIdx].fakeResistStrategy.nStrategyNum] = nFakeResistStrategyNum;
            ea[nCurIdx].fakeResistStrategy.nStrategyNum++;
            ea[nCurIdx].fakeResistStrategy.nHitNum++;



            ea[nCurIdx].fakeResistStrategy.fEverageShoulderPrice = (ea[nCurIdx].fakeResistStrategy.fEverageShoulderPrice == 0) ? ea[nCurIdx].nFs : (ea[nCurIdx].nFs + ea[nCurIdx].fakeResistStrategy.fEverageShoulderPrice) / 2;
            ea[nCurIdx].fakeResistStrategy.nSumShoulderPrice += ea[nCurIdx].nFs;

            if (ea[nCurIdx].fakeResistStrategy.nMaxShoulderPrice == 0 || ea[nCurIdx].fakeResistStrategy.nMaxShoulderPrice < ea[nCurIdx].nFs)
            {
                ea[nCurIdx].fakeResistStrategy.nMaxShoulderPrice = ea[nCurIdx].nFs;

                if (nTimeLineIdx != ea[nCurIdx].fakeResistStrategy.nPrevMaxMinIdx)
                {
                    ea[nCurIdx].fakeResistStrategy.nPrevMaxMinIdx = nTimeLineIdx;
                    ea[nCurIdx].fakeResistStrategy.nPrevMaxMinUpperCount++;
                }
                ea[nCurIdx].fakeResistStrategy.nUpperCount++;
            }

            if (ea[nCurIdx].fakeResistStrategy.nPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].fakeResistStrategy.nPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].fakeResistStrategy.nMinuteLocationCount++;
            }

            UpdateFakeHistory();
            AddFakeHistory(FAKE_RESIST_SIGNAL, nFakeResistStrategyNum);
            CalcFakeHistory();

            UpFakeCount(nCurIdx, FAKE_RESIST_SIGNAL);
        }
        #endregion

        #region SetThisPriceUp
        void SetThisPriceUp(int nPowerUpStrategyNum)
        {
            if (ea[nCurIdx].priceUpStrategy.nStrategyNum >= PRICE_UP_MAX_NUM) // || ea[nCurIdx].priceUpStrategy.arrStrategy[nPowerUpStrategyNum] > 7) // 한 전략당 8번제한
                return;


            if (ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].myStrategy.nSharedMinuteLocationCount++;
            }

            if (ea[nCurIdx].priceUpStrategy.nLastTouchTime != 0 && SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].priceUpStrategy.nLastTouchTime) >= 2400) // 40분 이상 매수가 안됐었으면
            {
                ea[nCurIdx].priceUpStrategy.isSuddenBoom = true;
            }
            ea[nCurIdx].priceUpStrategy.nLastTouchTime = nSharedTime;

            ea[nCurIdx].priceUpStrategy.arrLastTouch[nPowerUpStrategyNum] = nSharedTime;
            ea[nCurIdx].priceUpStrategy.arrStrategy[nPowerUpStrategyNum]++;
            ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPowerUpStrategyNum] = nTimeLineIdx;

            ea[nCurIdx].priceUpStrategy.arrUpPrice[ea[nCurIdx].priceUpStrategy.nStrategyNum] = ea[nCurIdx].nFs;
            ea[nCurIdx].priceUpStrategy.arrUpTime[ea[nCurIdx].priceUpStrategy.nStrategyNum] = nSharedTime;
            ea[nCurIdx].priceUpStrategy.arrMinuteIdx[ea[nCurIdx].priceUpStrategy.nStrategyNum] = nTimeLineIdx;
            ea[nCurIdx].priceUpStrategy.arrSpecificStrategy[ea[nCurIdx].priceUpStrategy.nStrategyNum] = nPowerUpStrategyNum;
            ea[nCurIdx].priceUpStrategy.nStrategyNum++;
            ea[nCurIdx].priceUpStrategy.nHitNum++;


            ea[nCurIdx].priceUpStrategy.fEverageShoulderPrice = (ea[nCurIdx].priceUpStrategy.fEverageShoulderPrice == 0) ? ea[nCurIdx].nFs : (ea[nCurIdx].nFs + ea[nCurIdx].priceUpStrategy.fEverageShoulderPrice) / 2;
            ea[nCurIdx].priceUpStrategy.nSumShoulderPrice += ea[nCurIdx].nFs;
            if (ea[nCurIdx].priceUpStrategy.nMaxShoulderPrice == 0 || ea[nCurIdx].priceUpStrategy.nMaxShoulderPrice < ea[nCurIdx].nFs)
            {
                ea[nCurIdx].priceUpStrategy.nMaxShoulderPrice = ea[nCurIdx].nFs;

                if (nTimeLineIdx != ea[nCurIdx].priceUpStrategy.nPrevMaxMinIdx)
                {
                    ea[nCurIdx].priceUpStrategy.nPrevMaxMinIdx = nTimeLineIdx;
                    ea[nCurIdx].priceUpStrategy.nPrevMaxMinUpperCount++;
                }
                ea[nCurIdx].priceUpStrategy.nUpperCount++;
            }

            if (ea[nCurIdx].priceUpStrategy.nPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].priceUpStrategy.nPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].priceUpStrategy.nMinuteLocationCount++;
            }


            UpdateFakeHistory();
            AddFakeHistory(PRICE_UP_SIGNAL, nPowerUpStrategyNum);
            CalcFakeHistory();
            UpFakeCount(nCurIdx, PRICE_UP_SIGNAL);
        }
        #endregion

        #region SetThisPriceDown
        void SetThisPriceDown(int nPowerDownStrategyNum)
        {
            if (ea[nCurIdx].priceDownStrategy.nStrategyNum >= PRICE_DOWN_MAX_NUM) // || ea[nCurIdx].priceDownStrategy.arrStrategy[nPowerDownStrategyNum] > 7) // 한 전략당 8번제한
                return;

            if (ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].myStrategy.nSharedPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].myStrategy.nSharedMinuteLocationCount++;
            }

            if (ea[nCurIdx].priceDownStrategy.nLastTouchTime != 0 && SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].priceDownStrategy.nLastTouchTime) >= 2400) // 40분 이상 매수가 안됐었으면
            {
                ea[nCurIdx].priceDownStrategy.isSuddenBoom = true;
            }
            ea[nCurIdx].priceDownStrategy.nLastTouchTime = nSharedTime;

            ea[nCurIdx].priceDownStrategy.arrLastTouch[nPowerDownStrategyNum] = nSharedTime;
            ea[nCurIdx].priceDownStrategy.arrStrategy[nPowerDownStrategyNum]++;
            ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPowerDownStrategyNum] = nTimeLineIdx;

            ea[nCurIdx].priceDownStrategy.arrDownPrice[ea[nCurIdx].priceDownStrategy.nStrategyNum] = ea[nCurIdx].nFs;
            ea[nCurIdx].priceDownStrategy.arrDownTime[ea[nCurIdx].priceDownStrategy.nStrategyNum] = nSharedTime;
            ea[nCurIdx].priceDownStrategy.arrMinuteIdx[ea[nCurIdx].priceDownStrategy.nStrategyNum] = nTimeLineIdx;
            ea[nCurIdx].priceDownStrategy.arrSpecificStrategy[ea[nCurIdx].priceDownStrategy.nStrategyNum] = nPowerDownStrategyNum;
            ea[nCurIdx].priceDownStrategy.nStrategyNum++;
            ea[nCurIdx].priceDownStrategy.nHitNum++;


            ea[nCurIdx].priceDownStrategy.fEverageShoulderPrice = (ea[nCurIdx].priceDownStrategy.fEverageShoulderPrice == 0) ? ea[nCurIdx].nFs : (ea[nCurIdx].nFs + ea[nCurIdx].priceDownStrategy.fEverageShoulderPrice) / 2;
            ea[nCurIdx].priceDownStrategy.nSumShoulderPrice += ea[nCurIdx].nFs;

            if (ea[nCurIdx].priceDownStrategy.nMaxShoulderPrice == 0 || ea[nCurIdx].priceDownStrategy.nMaxShoulderPrice < ea[nCurIdx].nFs)
            {
                ea[nCurIdx].priceDownStrategy.nMaxShoulderPrice = ea[nCurIdx].nFs;

                if (nTimeLineIdx != ea[nCurIdx].priceDownStrategy.nPrevMaxMinIdx)
                {
                    ea[nCurIdx].priceDownStrategy.nPrevMaxMinIdx = nTimeLineIdx;
                    ea[nCurIdx].priceDownStrategy.nPrevMaxMinUpperCount++;
                }
                ea[nCurIdx].priceDownStrategy.nUpperCount++;
            }

            if (ea[nCurIdx].priceDownStrategy.nPrevMinuteIdx != nTimeLineIdx)
            {
                ea[nCurIdx].priceDownStrategy.nPrevMinuteIdx = nTimeLineIdx;
                ea[nCurIdx].priceDownStrategy.nMinuteLocationCount++;
            }

            UpdateFakeHistory();
            AddFakeHistory(PRICE_DOWN_SIGNAL, nPowerDownStrategyNum);
            CalcFakeHistory();
            UpFakeCount(nCurIdx, PRICE_DOWN_SIGNAL);
        }
        #endregion

        #region UpdateFakeHistory
        public void UpdateFakeHistory()
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
        public void AddFakeHistory(int nType, int nStrategyNum)
        {
            FakeHistoryPiece tmpFakeHistoryPiece;
            tmpFakeHistoryPiece.nTypeFakeTrading = nType;
            tmpFakeHistoryPiece.nFakeStrategyNum = nStrategyNum;
            tmpFakeHistoryPiece.nSharedTime = nSharedTime;
            tmpFakeHistoryPiece.nTimeLineIdx = nTimeLineIdx;
            ea[nCurIdx].myStrategy.listFakeHistoryPiece.Add(tmpFakeHistoryPiece); // 신규 데이터를 삽입해라

        }
        #endregion

        #region CalcFakeHistory 
        public void CalcFakeHistory()
        {

            ea[nCurIdx].myStrategy.nFakeBuyNum = 0;
            ea[nCurIdx].myStrategy.nFakeResistNum = 0;
            ea[nCurIdx].myStrategy.nFakeAssistantNum = 0;
            ea[nCurIdx].myStrategy.nPriceUpNum = 0;
            ea[nCurIdx].myStrategy.nPriceDownNum = 0;
            ea[nCurIdx].myStrategy.nFakeBuyMinuteAreaNum = 0;
            ea[nCurIdx].myStrategy.nFakeResistMinuteAreaNum = 0;
            ea[nCurIdx].myStrategy.nFakeAssistantMinuteAreaNum = 0;
            ea[nCurIdx].myStrategy.nTotalFakeMinuteAreaNum = 0;
            ea[nCurIdx].myStrategy.nPriceUpMinuteAreaNum = 0;
            ea[nCurIdx].myStrategy.nPriceDownMinuteAreaNum = 0;

            int nPrevFakeBuyMinuteIdx = -1;
            int nPrevFakeResistMinuteIdx = -1;
            int nPrevFakeAssistantMinuteIdx = -1;
            int nPrevPowerUpMinuteIdx = -1;
            int nPrevPowerDownMinuteIdx = -1;
            int nPrevTotalFakeMinuteIdx = -1;

            for (int i = 0; i < ea[nCurIdx].myStrategy.listFakeHistoryPiece.Count; i++)
            {
                if (ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTypeFakeTrading == FAKE_BUY_SIGNAL)
                {
                    ea[nCurIdx].myStrategy.nFakeBuyNum++;
                    if (nPrevFakeBuyMinuteIdx == -1 || nPrevFakeBuyMinuteIdx != ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevFakeBuyMinuteIdx = ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nCurIdx].myStrategy.nFakeBuyMinuteAreaNum++;
                    }
                }
                else if (ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTypeFakeTrading == FAKE_RESIST_SIGNAL)
                {
                    ea[nCurIdx].myStrategy.nFakeResistNum++;
                    if (nPrevFakeResistMinuteIdx == -1 || nPrevFakeResistMinuteIdx != ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevFakeResistMinuteIdx = ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nCurIdx].myStrategy.nFakeResistMinuteAreaNum++;
                    }
                }
                else if (ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTypeFakeTrading == FAKE_ASSISTANT_SIGNAL)
                {
                    ea[nCurIdx].myStrategy.nFakeAssistantNum++;
                    if (nPrevFakeAssistantMinuteIdx == -1 || nPrevFakeAssistantMinuteIdx != ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevFakeAssistantMinuteIdx = ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nCurIdx].myStrategy.nFakeAssistantMinuteAreaNum++;
                    }
                }
                else if (ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTypeFakeTrading == PRICE_UP_SIGNAL)
                {
                    ea[nCurIdx].myStrategy.nPriceUpNum++;
                    if (nPrevPowerUpMinuteIdx == -1 || nPrevPowerUpMinuteIdx != ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevPowerUpMinuteIdx = ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nCurIdx].myStrategy.nPriceUpMinuteAreaNum++;
                    }
                }
                else if (ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTypeFakeTrading == PRICE_DOWN_SIGNAL)
                {
                    ea[nCurIdx].myStrategy.nPriceDownNum++;
                    if (nPrevPowerDownMinuteIdx == -1 || nPrevPowerDownMinuteIdx != ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx)
                    {
                        nPrevPowerDownMinuteIdx = ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx;
                        ea[nCurIdx].myStrategy.nPriceDownMinuteAreaNum++;
                    }
                }
                // total min idx 부분
                if (nPrevTotalFakeMinuteIdx == -1 || nPrevTotalFakeMinuteIdx != ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx)
                {
                    nPrevTotalFakeMinuteIdx = ea[nCurIdx].myStrategy.listFakeHistoryPiece[i].nTimeLineIdx;
                    ea[nCurIdx].myStrategy.nTotalFakeMinuteAreaNum++;
                }
            }
        }
        #endregion

        #region GetAccess
        public bool GetAccess(ref MyStrategy my, int nStrategy, int? nTrial = null, int? nFailToApprove = null, int? nCycle = null)
        {
            bool isRet;
            int nTrial_ = (nTrial == null) ? 1 : (int)nTrial;
            int nFailToApprove_ = (nFailToApprove == null) ? 0 : (int)nFailToApprove;
            int nCycle_ = (nCycle == null) ? 0 : (int)nCycle;

            // 사이클이 없다면 기본 실패 0회 도전 1회이다.
            if (nCycle_ == 0)
            {
                bool isTrialOnceDefense = true;
                if (nTrial_ > 1)
                    isTrialOnceDefense = my.arrPrevMinuteIdx[nStrategy] < nTimeLineIdx; // 1분 당 최대 한번만 거래가능

                isRet = isTrialOnceDefense && my.arrReqFail[nStrategy] <= nFailToApprove_ && my.arrStrategy[nStrategy] < nTrial_;
            }
            else
            {
                bool isTrial = true;
                bool isFailurePass = true;

                if (nTrial == null) // default일때는 검사 안함 
                    isTrial = my.arrStrategy[nStrategy] < nTrial_;

                if (nFailToApprove == null) // default일때는 검사 안함
                    isFailurePass = my.arrReqFail[nStrategy] <= nFailToApprove_;

                isRet = isTrial && isFailurePass && (my.arrPrevMinuteIdx[nStrategy] == 0 || my.arrPrevMinuteIdx[nStrategy] + nCycle_ - 1 < nTimeLineIdx);
            }
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
