using Microsoft.EntityFrameworkCore;
using AtoIndicator.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AtoIndicator.KiwoomLib.TimeLib;


namespace AtoIndicator
{
    public partial class MainForm
    {
        // ===============================================
        // 마지막 편집일 : 2023-04-20
        // 1. 장 마감 후 매매 데이터를 DB에 저장한다.
        // ===============================================
        #region 매매 DB작업
        public void PutTradeResultAsync()
        {
            void PutTradeResultToDB()
            {
                using (var dbContext = new myDbContext())
                {
                    EachStock curEa;
                    BuyedSlot curBlock;

                    // Insert 속도 향상시키기
                    // 1.
                    dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                    // 2.
                    dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    PrintLog($"=========================== DB삽입 시작 ===========================");
                    for (int i = 0; i < nStockLength; i++)
                    {
                        curEa = ea[i];
                        try
                        {
                            for (int j = 0; j < curEa.myTradeManager.nIdx; j++)
                            {
                                curBlock = curEa.myTradeManager.arrBuyedSlots[j];

                                #region 매수
                                // 매수 슬롯 정보
                                try
                                {
                                    // slot 접두사가 붙어있으면 slot에서  0부터 계산한거고
                                    // 접두사가 안붙어있으면 개인구조체 값을 그대로 복사한거 사용하려면 이전값과의 차를 통해서 사용해야함.
                                    if (curBlock.fixedBuyingInfo != null)
                                    {
                                        curBlock.fixedBuyingInfo.nFinalPrice = curBlock.recGroup.recList[0].nFinalPrice;
                                        #region 실매수 DB 정보추가
                                        #region 슬리피지 파악용
                                        curBlock.fixedBuyingInfo.nHogaCntAfterCheck = curBlock.recGroup.recList[0].nHogaCnt;
                                        curBlock.fixedBuyingInfo.nChegyulCntAfterCheck = curBlock.recGroup.recList[0].nCnt;
                                        curBlock.fixedBuyingInfo.nUpDownCntAfterCheck = curBlock.recGroup.recList[0].nUpDownCnt;
                                        curBlock.fixedBuyingInfo.fUpPowerAfterCheck = curBlock.recGroup.recList[0].fUpPower;
                                        curBlock.fixedBuyingInfo.fDownPowerAfterCheck = curBlock.recGroup.recList[0].fDownPower;
                                        curBlock.fixedBuyingInfo.nNoMoveCntAfterCheck = curBlock.recGroup.recList[0].nNoMoveCount;
                                        curBlock.fixedBuyingInfo.nFewSpeedCntAfterCheck = curBlock.recGroup.recList[0].nFewSpeedCount;
                                        curBlock.fixedBuyingInfo.nMissCntAfterCheck = curBlock.recGroup.recList[0].nMissCount;
                                        curBlock.fixedBuyingInfo.lTotalTradePriceAfterCheck = curBlock.recGroup.recList[0].lTotalTradePrice;
                                        curBlock.fixedBuyingInfo.lTotalBuyPriceAfterCheck = curBlock.recGroup.recList[0].lOnlyBuyPrice;
                                        curBlock.fixedBuyingInfo.lTotalSellPriceAfterCheck = curBlock.recGroup.recList[0].lOnlySellPrice;
                                        #endregion
                                        curBlock.fixedBuyingInfo.nOrderPrice = curBlock.nOrderPrice;
                                        curBlock.fixedBuyingInfo.nOriginOrderPrice = curBlock.nOriginOrderPrice;
                                        curBlock.fixedBuyingInfo.nRqTime = curBlock.nRequestTime;
                                        curBlock.fixedBuyingInfo.nReceiptTime = curBlock.nReceiptTime;
                                        curBlock.fixedBuyingInfo.nBuyEndTime = curBlock.nBuyEndTime;
                                        curBlock.fixedBuyingInfo.nDeathTime = curBlock.nDeathTime;
                                        curBlock.fixedBuyingInfo.nDeathRqTime = curBlock.nSellRequestTime;
                                        curBlock.fixedBuyingInfo.nBuyPrice = curBlock.nBuyPrice;
                                        curBlock.fixedBuyingInfo.nDeathPrice = curBlock.nDeathPrice;
                                        curBlock.fixedBuyingInfo.isAllBuyed = curBlock.isAllBuyed;
                                        curBlock.fixedBuyingInfo.isAllSelled = curBlock.isAllSelled;
                                        curBlock.fixedBuyingInfo.nBuyVolume = curBlock.nBuyVolume;
                                        curBlock.fixedBuyingInfo.nBuyStrategyIdx = strategyNameDict[(PAPER_BUY_SIGNAL, strategyName.arrPaperBuyStrategyName[curBlock.nStrategyIdx])]; // key
                                        curBlock.fixedBuyingInfo.nBuyStrategySequenceIdx = curBlock.nSequence; // key
                                        curBlock.fixedBuyingInfo.sBuyStrategyName = strategyName.arrPaperBuyStrategyName[curBlock.nStrategyIdx];
                                        curBlock.fixedBuyingInfo.sSellStrategyMsg = curBlock.sSellDescription;

                                        curBlock.fixedBuyingInfo.nSellVersion = SELL_VERSION;
                                        curBlock.fixedBuyingInfo.nAIVersion = AI_VERSION;

                                        curBlock.fixedBuyingInfo.n2MinPrice = curBlock.recGroup.recList[0].n2MinPrice;
                                        curBlock.fixedBuyingInfo.f2MinPower = curBlock.recGroup.recList[0].f2MinPower;
                                        curBlock.fixedBuyingInfo.n3MinPrice = curBlock.recGroup.recList[0].n3MinPrice;
                                        curBlock.fixedBuyingInfo.f3MinPower = curBlock.recGroup.recList[0].f3MinPower;
                                        curBlock.fixedBuyingInfo.n5MinPrice = curBlock.recGroup.recList[0].n5MinPrice;
                                        curBlock.fixedBuyingInfo.f5MinPower = curBlock.recGroup.recList[0].f5MinPower;
                                        curBlock.fixedBuyingInfo.n10MinPrice = curBlock.recGroup.recList[0].n10MinPrice;
                                        curBlock.fixedBuyingInfo.f10MinPower = curBlock.recGroup.recList[0].f10MinPower;
                                        curBlock.fixedBuyingInfo.n15MinPrice = curBlock.recGroup.recList[0].n15MinPrice;
                                        curBlock.fixedBuyingInfo.f15MinPower = curBlock.recGroup.recList[0].f15MinPower;
                                        curBlock.fixedBuyingInfo.n20MinPrice = curBlock.recGroup.recList[0].n20MinPrice;
                                        curBlock.fixedBuyingInfo.f20MinPower = curBlock.recGroup.recList[0].f20MinPower;
                                        curBlock.fixedBuyingInfo.n30MinPrice = curBlock.recGroup.recList[0].n30MinPrice;
                                        curBlock.fixedBuyingInfo.f30MinPower = curBlock.recGroup.recList[0].f30MinPower;
                                        curBlock.fixedBuyingInfo.n50MinPrice = curBlock.recGroup.recList[0].n50MinPrice;
                                        curBlock.fixedBuyingInfo.f50MinPower = curBlock.recGroup.recList[0].f50MinPower;

                                        // 맥스민값
                                        curBlock.fixedBuyingInfo.nMaxPriceAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nMaxPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nMaxTimeAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nMaxTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fMaxPowerAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.fMaxPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nMinPriceAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nMinPriceAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nMinTimeAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nMinTimeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.fMinPowerAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nBottomPriceAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBottomTimeAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBottomPowerAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.fBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopPriceAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopTimeAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fTopPowerAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.fTopPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomPriceAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nBoundBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomTimeAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nBoundBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundBottomPowerAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.fBoundBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopPriceAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nBoundTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopTimeAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.nBoundTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundTopPowerAfterBuy = curBlock.recGroup.recList[0].maxMinRealTilThree.fBoundTopPowerWithFeeAfterBuy;

                                        curBlock.fixedBuyingInfo.nMaxPriceMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nMaxPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nMaxTimeMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nMaxTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fMaxPowerMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.fMaxPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nMinPriceMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nMinPriceAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nMinTimeMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nMinTimeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.fMinPowerMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nBottomPriceMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBottomTimeMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBottomPowerMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.fBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopPriceMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopTimeMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fTopPowerMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.fTopPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomPriceMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nBoundBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomTimeMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nBoundBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundBottomPowerMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.fBoundBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopPriceMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nBoundTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopTimeMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.nBoundTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundTopPowerMinuteAfterBuy = curBlock.recGroup.recList[0].maxMinMinuteTilThree.fBoundTopPowerWithFeeAfterBuy;

                                        curBlock.fixedBuyingInfo.nMaxPriceAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nMaxPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nMaxTimeAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nMaxTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fMaxPowerAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.fMaxPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nMinPriceAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nMinPriceAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nMinTimeAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nMinTimeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.fMinPowerAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nBottomPriceAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBottomTimeAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBottomPowerAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.fBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopPriceAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopTimeAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fTopPowerAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.fTopPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomPriceAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nBoundBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomTimeAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nBoundBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundBottomPowerAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.fBoundBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopPriceAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nBoundTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopTimeAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.nBoundTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundTopPowerAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinRealWhile10.fBoundTopPowerWithFeeAfterBuy;

                                        curBlock.fixedBuyingInfo.nMaxPriceAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nMaxPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nMaxTimeAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nMaxTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fMaxPowerAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.fMaxPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nMinPriceAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nMinPriceAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nMinTimeAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nMinTimeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.fMinPowerAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nBottomPriceAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBottomTimeAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBottomPowerAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.fBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopPriceAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopTimeAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fTopPowerAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.fTopPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomPriceAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nBoundBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomTimeAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nBoundBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundBottomPowerAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.fBoundBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopPriceAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nBoundTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopTimeAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.nBoundTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundTopPowerAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinRealWhile30.fBoundTopPowerWithFeeAfterBuy;

                                        curBlock.fixedBuyingInfo.nMaxPriceAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nMaxPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nMaxTimeAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nMaxTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fMaxPowerAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.fMaxPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nMinPriceAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nMinPriceAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nMinTimeAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nMinTimeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.fMinPowerAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nBottomPriceAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBottomTimeAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBottomPowerAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.fBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopPriceAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopTimeAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fTopPowerAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.fTopPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomPriceAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nBoundBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomTimeAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nBoundBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundBottomPowerAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.fBoundBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopPriceAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nBoundTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopTimeAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.nBoundTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundTopPowerAfterBuyWhile60 = curBlock.recGroup.recList[0].maxMinRealWhile60.fBoundTopPowerWithFeeAfterBuy;

                                        curBlock.fixedBuyingInfo.nMaxPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nMaxPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nMaxTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nMaxTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fMaxPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.fMaxPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nMinPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nMinPriceAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nMinTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nMinTimeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.fMinPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nBottomPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBottomTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBottomPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.fBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fTopPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.fTopPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nBoundBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nBoundBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundBottomPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.fBoundBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nBoundTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.nBoundTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundTopPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile10.fBoundTopPowerWithFeeAfterBuy;

                                        curBlock.fixedBuyingInfo.nMaxPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nMaxPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nMaxTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nMaxTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fMaxPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.fMaxPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nMinPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nMinPriceAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nMinTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nMinTimeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.fMinPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curBlock.fixedBuyingInfo.nBottomPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBottomTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBottomPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.fBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nTopTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fTopPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.fTopPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nBoundBottomPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundBottomTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nBoundBottomTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundBottomPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.fBoundBottomPowerWithFeeAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nBoundTopPriceAfterBuy;
                                        curBlock.fixedBuyingInfo.nBoundTopTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.nBoundTopTimeAfterBuy;
                                        curBlock.fixedBuyingInfo.fBoundTopPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[0].maxMinMinuteTilThreeWhile30.fBoundTopPowerWithFeeAfterBuy;

                                        curBlock.fixedBuyingInfo.fFinalISlope = curEa.timeLines1m.fInitSlope;
                                        curBlock.fixedBuyingInfo.fFinalMSlope = curEa.timeLines1m.fMaxSlope;
                                        curBlock.fixedBuyingInfo.fFinalTSlope = curEa.timeLines1m.fTotalMedian;
                                        curBlock.fixedBuyingInfo.fFinalHSlope = curEa.timeLines1m.fHourMedian;
                                        curBlock.fixedBuyingInfo.fFinalRSlope = curEa.timeLines1m.fRecentMedian;
                                        curBlock.fixedBuyingInfo.fFinalDSlope = curEa.timeLines1m.fMaxSlope - curEa.timeLines1m.fInitSlope;

                                        curBlock.fixedBuyingInfo.fFinalIAngle = curEa.timeLines1m.fInitAngle;
                                        curBlock.fixedBuyingInfo.fFinalMAngle = curEa.timeLines1m.fMaxAngle;
                                        curBlock.fixedBuyingInfo.fFinalTAngle = curEa.timeLines1m.fTotalMedianAngle;
                                        curBlock.fixedBuyingInfo.fFinalHAngle = curEa.timeLines1m.fHourMedianAngle;
                                        curBlock.fixedBuyingInfo.fFinalRAngle = curEa.timeLines1m.fRecentMedianAngle;
                                        curBlock.fixedBuyingInfo.fFinalDAngle = curEa.timeLines1m.fDAngle;

                                        if (curBlock.isAllBuyed && curBlock.isAllSelled && curBlock.nBuyVolume > 0)
                                            curBlock.fixedBuyingInfo.fProfit = (double)(curBlock.nDeathPrice - curBlock.nBuyPrice) / curBlock.nBuyPrice - REAL_STOCK_COMMISSION;
                                        #endregion

                                        try
                                        {
                                            dbContext.buyReports.Add(curBlock.fixedBuyingInfo);
                                            dbContext.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            PrintLog($"buyReports : {curEa.sCode}  {curEa.sCodeName} 위치 : ({j}) 블럭 DB삽입 실패!");
                                            dbContext.buyReports.Remove(curBlock.fixedBuyingInfo);
                                        }
                                    }
                                }
                                catch
                                {
                                    PrintLog($"buyReports : {curEa.sCode}  {curEa.sCodeName} 위치 : ({j})  데이터 문제가 발생했습니다");
                                }
                                #endregion

                                #region 매도
                                // 매도 슬롯정보
                                for (int nRecordNum = 1; nRecordNum < curBlock.recGroup.nLen; nRecordNum++)
                                {
                                    try
                                    {
                                        if (curBlock.recGroup.recList[nRecordNum].fixedSellingInfo != null)
                                        {
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nFinalPrice = curBlock.recGroup.recList[nRecordNum].nFinalPrice;
                                            #region 실매도 DB 정보추가
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nPartedIdx = nRecordNum - 1; // key 
                                            #region 슬리피지 파악용
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nHogaCntAfterCheck = curBlock.recGroup.recList[nRecordNum].nHogaCnt;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nChegyulCntAfterCheck = curBlock.recGroup.recList[nRecordNum].nCnt;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nUpDownCntAfterCheck = curBlock.recGroup.recList[nRecordNum].nUpDownCnt;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fUpPowerAfterCheck = curBlock.recGroup.recList[nRecordNum].fUpPower;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fDownPowerAfterCheck = curBlock.recGroup.recList[nRecordNum].fDownPower;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nNoMoveCntAfterCheck = curBlock.recGroup.recList[nRecordNum].nNoMoveCount;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nFewSpeedCntAfterCheck = curBlock.recGroup.recList[nRecordNum].nFewSpeedCount;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMissCntAfterCheck = curBlock.recGroup.recList[nRecordNum].nMissCount;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.lTotalTradePriceAfterCheck = curBlock.recGroup.recList[nRecordNum].lTotalTradePrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.lTotalBuyPriceAfterCheck = curBlock.recGroup.recList[nRecordNum].lOnlyBuyPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.lTotalSellPriceAfterCheck = curBlock.recGroup.recList[nRecordNum].lOnlySellPrice;
                                            #endregion
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.isAllSelled = curBlock.recGroup.recList[nRecordNum].isSelled;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nSumCut = curBlock.recGroup.recList[nRecordNum].nTotalSellPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBuyStrategyIdx = strategyNameDict[(PAPER_BUY_SIGNAL, strategyName.arrPaperBuyStrategyName[curBlock.nStrategyIdx])]; // key
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.sBuyStrategyName = strategyName.arrPaperBuyStrategyName[curBlock.nStrategyIdx];
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.sSellStrategyMsg = curBlock.recGroup.recList[nRecordNum].sSellDescription;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nDeathTime = curBlock.recGroup.recList[nRecordNum].nDeathTime;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nDeathRqTime = curBlock.recGroup.recList[nRecordNum].nSellRequestTime;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nDeathPrice = curBlock.recGroup.recList[nRecordNum].nDeathPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fProfit = curBlock.recGroup.recList[nRecordNum].fProfit;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.n2MinPrice = curBlock.recGroup.recList[nRecordNum].n2MinPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.f2MinPower = curBlock.recGroup.recList[nRecordNum].f2MinPower;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.n3MinPrice = curBlock.recGroup.recList[nRecordNum].n3MinPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.f3MinPower = curBlock.recGroup.recList[nRecordNum].f3MinPower;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.n5MinPrice = curBlock.recGroup.recList[nRecordNum].n5MinPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.f5MinPower = curBlock.recGroup.recList[nRecordNum].f5MinPower;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.n10MinPrice = curBlock.recGroup.recList[nRecordNum].n10MinPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.f10MinPower = curBlock.recGroup.recList[nRecordNum].f10MinPower;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.n15MinPrice = curBlock.recGroup.recList[nRecordNum].n15MinPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.f15MinPower = curBlock.recGroup.recList[nRecordNum].f15MinPower;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.n20MinPrice = curBlock.recGroup.recList[nRecordNum].n20MinPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.f20MinPower = curBlock.recGroup.recList[nRecordNum].f20MinPower;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.n30MinPrice = curBlock.recGroup.recList[nRecordNum].n30MinPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.f30MinPower = curBlock.recGroup.recList[nRecordNum].f30MinPower;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.n50MinPrice = curBlock.recGroup.recList[nRecordNum].n50MinPrice;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.f50MinPower = curBlock.recGroup.recList[nRecordNum].f50MinPower;

                                            // 맥스민값
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxPriceAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nMaxPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxTimeAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nMaxTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMaxPowerAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.fMaxPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinPriceAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nMinPriceAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinTimeAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nMinTimeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMinPowerAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.fMinPowerWithFeeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomPriceAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomTimeAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBottomPowerAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.fBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopPriceAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopTimeAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fTopPowerAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.fTopPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomPriceAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nBoundBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomTimeAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nBoundBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundBottomPowerAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.fBoundBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopPriceAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nBoundTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopTimeAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.nBoundTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundTopPowerAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinRealTilThree.fBoundTopPowerWithFeeAfterBuy;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxPriceMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nMaxPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxTimeMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nMaxTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMaxPowerMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.fMaxPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinPriceMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nMinPriceAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinTimeMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nMinTimeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMinPowerMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.fMinPowerWithFeeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomPriceMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomTimeMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBottomPowerMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.fBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopPriceMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopTimeMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fTopPowerMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.fTopPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomPriceMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nBoundBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomTimeMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nBoundBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundBottomPowerMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.fBoundBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopPriceMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nBoundTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopTimeMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.nBoundTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundTopPowerMinuteAfterBuy = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThree.fBoundTopPowerWithFeeAfterBuy;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxPriceAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nMaxPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxTimeAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nMaxTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMaxPowerAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.fMaxPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinPriceAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nMinPriceAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinTimeAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nMinTimeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMinPowerAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.fMinPowerWithFeeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomPriceAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomTimeAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBottomPowerAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.fBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopPriceAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopTimeAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fTopPowerAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.fTopPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomPriceAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nBoundBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomTimeAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nBoundBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundBottomPowerAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.fBoundBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopPriceAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nBoundTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopTimeAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.nBoundTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundTopPowerAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile10.fBoundTopPowerWithFeeAfterBuy;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxPriceAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nMaxPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxTimeAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nMaxTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMaxPowerAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.fMaxPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinPriceAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nMinPriceAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinTimeAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nMinTimeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMinPowerAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.fMinPowerWithFeeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomPriceAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomTimeAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBottomPowerAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.fBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopPriceAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopTimeAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fTopPowerAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.fTopPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomPriceAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nBoundBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomTimeAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nBoundBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundBottomPowerAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.fBoundBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopPriceAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nBoundTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopTimeAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.nBoundTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundTopPowerAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile30.fBoundTopPowerWithFeeAfterBuy;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxPriceAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nMaxPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxTimeAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nMaxTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMaxPowerAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.fMaxPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinPriceAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nMinPriceAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinTimeAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nMinTimeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMinPowerAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.fMinPowerWithFeeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomPriceAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomTimeAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBottomPowerAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.fBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopPriceAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopTimeAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fTopPowerAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.fTopPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomPriceAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nBoundBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomTimeAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nBoundBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundBottomPowerAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.fBoundBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopPriceAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nBoundTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopTimeAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.nBoundTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundTopPowerAfterBuyWhile60 = curBlock.recGroup.recList[nRecordNum].maxMinRealWhile60.fBoundTopPowerWithFeeAfterBuy;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nMaxPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nMaxTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMaxPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.fMaxPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nMinPriceAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nMinTimeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMinPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.fMinPowerWithFeeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBottomPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.fBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fTopPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.fTopPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nBoundBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nBoundBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundBottomPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.fBoundBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopPriceMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nBoundTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopTimeMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.nBoundTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundTopPowerMinuteAfterBuyWhile10 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile10.fBoundTopPowerWithFeeAfterBuy;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nMaxPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMaxTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nMaxTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMaxPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.fMaxPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nMinPriceAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nMinTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nMinTimeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fMinPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.fMinPowerWithFeeAfterBuyBeforeMax;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBottomTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBottomPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.fBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nTopTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fTopPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.fTopPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nBoundBottomPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundBottomTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nBoundBottomTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundBottomPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.fBoundBottomPowerWithFeeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopPriceMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nBoundTopPriceAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nBoundTopTimeMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.nBoundTopTimeAfterBuy;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fBoundTopPowerMinuteAfterBuyWhile30 = curBlock.recGroup.recList[nRecordNum].maxMinMinuteTilThreeWhile30.fBoundTopPowerWithFeeAfterBuy;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalISlope = curEa.timeLines1m.fInitSlope;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalMSlope = curEa.timeLines1m.fMaxSlope;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalTSlope = curEa.timeLines1m.fTotalMedian;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalHSlope = curEa.timeLines1m.fHourMedian;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalRSlope = curEa.timeLines1m.fRecentMedian;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalDSlope = curEa.timeLines1m.fMaxSlope - curEa.timeLines1m.fInitSlope;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalIAngle = curEa.timeLines1m.fInitAngle;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalMAngle = curEa.timeLines1m.fMaxAngle;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalTAngle = curEa.timeLines1m.fTotalMedianAngle;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalHAngle = curEa.timeLines1m.fHourMedianAngle;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalRAngle = curEa.timeLines1m.fRecentMedianAngle;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.fFinalDAngle = curEa.timeLines1m.fDAngle;

                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nSellVersion = SELL_VERSION;
                                            curBlock.recGroup.recList[nRecordNum].fixedSellingInfo.nAIVersion = AI_VERSION;
                                            #endregion

                                            try
                                            {
                                                dbContext.sellReports.Add(curBlock.recGroup.recList[nRecordNum].fixedSellingInfo);
                                                dbContext.SaveChanges();
                                            }
                                            catch (Exception ex)
                                            {
                                                PrintLog($"sellReports : {curEa.sCode}  {curEa.sCodeName} 위치 : ({j}, {nRecordNum - 1}) 블럭 DB삽입 실패!");
                                                dbContext.sellReports.Remove(curBlock.recGroup.recList[nRecordNum].fixedSellingInfo);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        PrintLog($"sellReports : {curEa.sCode}  {curEa.sCodeName} 위치 : ({j}, {nRecordNum - 1}) 데이터 문제가 발생했습니다");
                                    }
                                }
                                #endregion
                            }
                            #region 페이크
                            // 페이크 슬롯정보
                            for (int nFakeRecordNum = 0; nFakeRecordNum < curEa.fakeStrategyMgr.fd.Count; nFakeRecordNum++)
                            {

                                try
                                {
                                    if (curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr != null)
                                    {
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nAIVersion = AI_VERSION;

                                        #region 페이크 DB 정보추가
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxPriceAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nMaxPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxTimeAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nMaxTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMaxPowerAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.fMaxPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinPriceAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nMinPriceAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinTimeAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nMinTimeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMinPowerAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomPriceAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomTimeAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBottomPowerAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.fBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopPriceAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopTimeAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fTopPowerAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.fTopPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomPriceAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nBoundBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomTimeAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nBoundBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundBottomPowerAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.fBoundBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopPriceAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nBoundTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopTimeAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.nBoundTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundTopPowerAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealTilThree.fBoundTopPowerWithFeeAfterBuy;


                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxPriceMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nMaxPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxTimeMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nMaxTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMaxPowerMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.fMaxPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinPriceMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nMinPriceAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinTimeMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nMinTimeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMinPowerMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomPriceMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomTimeMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBottomPowerMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.fBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopPriceMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopTimeMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fTopPowerMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.fTopPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomPriceMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nBoundBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomTimeMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nBoundBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundBottomPowerMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.fBoundBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopPriceMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nBoundTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopTimeMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.nBoundTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundTopPowerMinuteAfterBuy = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThree.fBoundTopPowerWithFeeAfterBuy;


                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxPriceAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nMaxPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxTimeAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nMaxTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMaxPowerAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.fMaxPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinPriceAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nMinPriceAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinTimeAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nMinTimeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMinPowerAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomPriceAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomTimeAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBottomPowerAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.fBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopPriceAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopTimeAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fTopPowerAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.fTopPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomPriceAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nBoundBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomTimeAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nBoundBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundBottomPowerAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.fBoundBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopPriceAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nBoundTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopTimeAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.nBoundTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundTopPowerAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile10.fBoundTopPowerWithFeeAfterBuy;


                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxPriceAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nMaxPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxTimeAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nMaxTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMaxPowerAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.fMaxPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinPriceAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nMinPriceAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinTimeAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nMinTimeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMinPowerAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomPriceAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomTimeAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBottomPowerAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.fBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopPriceAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopTimeAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fTopPowerAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.fTopPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomPriceAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nBoundBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomTimeAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nBoundBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundBottomPowerAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.fBoundBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopPriceAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nBoundTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopTimeAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.nBoundTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundTopPowerAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile30.fBoundTopPowerWithFeeAfterBuy;

                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxPriceAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nMaxPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxTimeAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nMaxTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMaxPowerAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.fMaxPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinPriceAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nMinPriceAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinTimeAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nMinTimeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMinPowerAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomPriceAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomTimeAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBottomPowerAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.fBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopPriceAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopTimeAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fTopPowerAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.fTopPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomPriceAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nBoundBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomTimeAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nBoundBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundBottomPowerAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.fBoundBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopPriceAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nBoundTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopTimeAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.nBoundTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundTopPowerAfterBuyWhile60 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinRealWhile60.fBoundTopPowerWithFeeAfterBuy;

                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxPriceMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nMaxPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxTimeMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nMaxTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMaxPowerMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fMaxPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinPriceMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nMinPriceAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinTimeMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nMinTimeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMinPowerMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomPriceMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomTimeMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBottomPowerMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopPriceMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopTimeMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fTopPowerMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fTopPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomPriceMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nBoundBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomTimeMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nBoundBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundBottomPowerMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fBoundBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopPriceMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nBoundTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopTimeMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nBoundTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundTopPowerMinuteAfterBuyWhile10 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fBoundTopPowerWithFeeAfterBuy;

                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxPriceMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nMaxPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMaxTimeMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nMaxTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMaxPowerMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fMaxPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinPriceMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nMinPriceAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nMinTimeMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nMinTimeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fMinPowerMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomPriceMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBottomTimeMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBottomPowerMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopPriceMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nTopTimeMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fTopPowerMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fTopPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomPriceMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nBoundBottomPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundBottomTimeMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nBoundBottomTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundBottomPowerMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fBoundBottomPowerWithFeeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopPriceMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nBoundTopPriceAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.nBoundTopTimeMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nBoundTopTimeAfterBuy;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fBoundTopPowerMinuteAfterBuyWhile30 = curEa.fakeStrategyMgr.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fBoundTopPowerWithFeeAfterBuy;

                                        #endregion
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalISlope = curEa.timeLines1m.fInitSlope;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalMSlope = curEa.timeLines1m.fMaxSlope;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalTSlope = curEa.timeLines1m.fTotalMedian;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalHSlope = curEa.timeLines1m.fHourMedian;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalRSlope = curEa.timeLines1m.fRecentMedian;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalDSlope = curEa.timeLines1m.fMaxSlope - curEa.timeLines1m.fInitSlope;

                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalIAngle = curEa.timeLines1m.fInitAngle;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalMAngle = curEa.timeLines1m.fMaxAngle;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalTAngle = curEa.timeLines1m.fTotalMedianAngle;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalHAngle = curEa.timeLines1m.fHourMedianAngle;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalRAngle = curEa.timeLines1m.fRecentMedianAngle;
                                        curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr.fFinalDAngle = curEa.timeLines1m.fDAngle;

                                        try
                                        {
                                            dbContext.fakeReports.Add(curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr);
                                            dbContext.SaveChanges();
                                        }
                                        catch(Exception ex)
                                        {
                                            PrintLog($"fakeReports : {curEa.sCode}  {curEa.sCodeName} 위치 : ({nFakeRecordNum}) 블럭 DB삽입 실패!");
                                            dbContext.fakeReports.Remove(curEa.fakeStrategyMgr.fd[nFakeRecordNum].fr);
                                        }
                                    }
                                }
                                catch
                                {
                                    PrintLog($"fakeReports : {curEa.sCode}  {curEa.sCodeName} 위치 : ({nFakeRecordNum}) 데이터 문제가 발생했습니다");
                                }
                            }
                            #endregion
                        }
                        catch
                        {
                            PrintLog($"{curEa.sCode} {curEa.sCodeName} {i}번째 반복에서 문제가 발생했습니다.");
                        }
                    }
                    PrintLog($"=========================== DB삽입 종료 ===========================");

                }

            }
            Task.Run(() => PutTradeResultToDB());
        }
        #endregion

        // ===============================================
        // 마지막 편집일 : 2023-04-20
        // 1. 장 마감 후 차트 데이터를 FTP를 통해 파일형태로 저장한다.
        // ===============================================
        #region 차트 DB작업
        public void PutChartResultAsync()
        {
            void PutChartDataToDB()
            {
                string ftpHost = "ftp://221.149.119.60:2021";
                string ftpUsername = "ftp_user";
                string ftpPassword = "jin9409";
                string sToday = DateTime.Now.ToString("yyyy-MM-dd");
                char comma = ',';

                StringBuilder sbEvery1min = new StringBuilder();

                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                EachStock curEa;

                for (int nStockIdx = 0; nStockIdx < nStockLength; nStockIdx++)
                {
                    try
                    {
                        curEa = ea[nStockIdx];

                        sbEvery1min.Clear();

                        if (curEa.timeLines1m.nRealDataIdx > 0)
                        {
                            // 전체 종목 1분 데이터 삽입
                            for (int nLastMinuteIdx = 0; nLastMinuteIdx <= curEa.timeLines1m.nRealDataIdx; nLastMinuteIdx++)
                            {

                                sbEvery1min.Append(
                                            $"{nLastMinuteIdx}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nTime}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nStartFs}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nMaxFs}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nMinFs}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nLastFs}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nTotalVolume}{comma}" +
                                            $"{(double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nBuyVolume - curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nSellVolume) / (curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nTotalVolume + 1) * 100}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nCount}{comma}" +
                                            $"{((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].lTotalPrice / MainForm.MILLION))}{comma}" +
                                            $"{(double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].lBuyPrice / MainForm.MILLION)}{comma}" +
                                            $"{(double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].lSellPrice / MainForm.MILLION)}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fAccumUpPower * 100}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fAccumDownPower * 100}{comma}" +
                                            $"{((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nLastFs - curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nStartFs) / curEa.nYesterdayEndPrice) * 100}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fInitAngle}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fMaxAngle}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fMedianAngle}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fHourAngle}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fRecentAngle}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fDAngle}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa0}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa1}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa2}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMaGap0}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMaGap1}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMaGap2}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nDownTimeOverMa0}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nDownTimeOverMa1}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nDownTimeOverMa2}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nUpTimeOverMa0}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nUpTimeOverMa1}{comma}" +
                                            $"{curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nUpTimeOverMa2}{comma}" +
                                            $"{curEa.rankSystem.arrRanking[nLastMinuteIdx].nSummationRanking}{comma}" +
                                            $"{curEa.rankSystem.arrRanking[nLastMinuteIdx].nSummationMove}{comma}" +
                                            $"{curEa.rankSystem.arrRanking[nLastMinuteIdx].nMinuteRanking}"
                                    );
                                if (nLastMinuteIdx < curEa.timeLines1m.nRealDataIdx)
                                    sbEvery1min.Append($"{NEW_LINE}");
                            }
                            client.UploadString($"{ftpHost}/chart/Every1min/{COMPUTER_LOCATION}-{sToday}-{curEa.sCode}-{curEa.sCodeName}.txt", sbEvery1min.ToString());

                        }
                    }
                    catch { } // 차트 에러
                }
                PrintLog($"차트 데이터를 삽입완료했습니다.");
            }

            Task.Run(() => PutChartDataToDB());
        }
        #endregion
    }
}
