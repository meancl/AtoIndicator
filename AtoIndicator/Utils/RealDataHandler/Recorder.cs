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
                                        catch (Exception ex)
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
