using Microsoft.EntityFrameworkCore;
using AtoTrader.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AtoTrader.KiwoomLib.TimeLib;


namespace AtoTrader
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

                    // Insert 속도 향상시키기
                    // 1.
                    dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                    // 2.
                    dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    // 3.
                    List<FakeReports> entitiesFakeReports = new List<FakeReports>();

                    
                    for (int i = 0; i < nStockLength; i++)
                    {
                        curEa = ea[i];
                        try
                        {
                            #region 페이크
                            // 페이크 슬롯정보
                            for (int nFakeRecordNum = 0; nFakeRecordNum < curEa.myStrategy.nTotalBlockCount; nFakeRecordNum++)
                            {

                                try
                                {
                                    if (curEa.myStrategy.fd[nFakeRecordNum].fr != null)
                                    {
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxPriceAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.nMaxPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxTimeAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.nMaxTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMaxPowerAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.fMaxPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinPriceAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.nMinPriceAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinTimeAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.nMinTimeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMinPowerAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomPriceAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.nBottomPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomTimeAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.nBottomTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fBottomPowerAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.fBottomPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopPriceAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.nTopPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopTimeAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.nTopTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fTopPowerAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealTilThree.fTopPowerWithFeeAfterBuy;


                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxPriceMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.nMaxPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxTimeMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.nMaxTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMaxPowerMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.fMaxPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinPriceMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.nMinPriceAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinTimeMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.nMinTimeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMinPowerMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomPriceMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.nBottomPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomTimeMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.nBottomTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fBottomPowerMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.fBottomPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopPriceMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.nTopPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopTimeMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.nTopTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fTopPowerMinuteAfterBuy = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThree.fTopPowerWithFeeAfterBuy;


                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxPriceAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.nMaxPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxTimeAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.nMaxTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMaxPowerAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.fMaxPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinPriceAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.nMinPriceAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinTimeAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.nMinTimeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMinPowerAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomPriceAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.nBottomPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomTimeAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.nBottomTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fBottomPowerAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.fBottomPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopPriceAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.nTopPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopTimeAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.nTopTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fTopPowerAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile10.fTopPowerWithFeeAfterBuy;

                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxPriceAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.nMaxPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxTimeAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.nMaxTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMaxPowerAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.fMaxPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinPriceAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.nMinPriceAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinTimeAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.nMinTimeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMinPowerAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomPriceAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.nBottomPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomTimeAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.nBottomTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fBottomPowerAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.fBottomPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopPriceAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.nTopPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopTimeAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.nTopTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fTopPowerAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile30.fTopPowerWithFeeAfterBuy;


                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxPriceAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.nMaxPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxTimeAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.nMaxTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMaxPowerAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.fMaxPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinPriceAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.nMinPriceAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinTimeAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.nMinTimeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMinPowerAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomPriceAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.nBottomPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomTimeAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.nBottomTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fBottomPowerAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.fBottomPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopPriceAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.nTopPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopTimeAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.nTopTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fTopPowerAfterBuyWhile60 = curEa.myStrategy.fd[nFakeRecordNum].maxMinRealWhile60.fTopPowerWithFeeAfterBuy;


                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxPriceMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nMaxPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxTimeMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nMaxTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMaxPowerMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fMaxPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinPriceMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nMinPriceAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinTimeMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nMinTimeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMinPowerMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomPriceMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nBottomPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomTimeMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nBottomTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fBottomPowerMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fBottomPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopPriceMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nTopPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopTimeMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.nTopTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fTopPowerMinuteAfterBuyWhile10 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile10.fTopPowerWithFeeAfterBuy;

                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxPriceMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nMaxPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMaxTimeMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nMaxTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMaxPowerMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fMaxPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinPriceMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nMinPriceAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nMinTimeMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nMinTimeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fMinPowerMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fMinPowerWithFeeAfterBuyBeforeMax;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomPriceMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nBottomPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nBottomTimeMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nBottomTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fBottomPowerMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fBottomPowerWithFeeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopPriceMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nTopPriceAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.nTopTimeMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.nTopTimeAfterBuy;
                                        curEa.myStrategy.fd[nFakeRecordNum].fr.fTopPowerMinuteAfterBuyWhile30 = curEa.myStrategy.fd[nFakeRecordNum].maxMinMinuteTilThreeWhile30.fTopPowerWithFeeAfterBuy;

                                        entitiesFakeReports.Add(curEa.myStrategy.fd[nFakeRecordNum].fr);
                                    }
                                }
                                catch
                                {
                                    PrintLog($"{curEa.sCode}  {curEa.sCodeName} {nFakeRecordNum}번째 페이크블록(fakereports)에서 문제가 발생했습니다");
                                }
                            }
                            #endregion
                        }
                        catch
                        {
                            PrintLog($"{curEa.sCode} {curEa.sCodeName} {i}번째 반복에서 문제가 발생했습니다.");
                        }
                    }

                    PrintLog($"DB 삽입 진행합니다.");

                    try
                    {
                        PrintLog($"3. DB fakeReports 삽입 전");
                        dbContext.AddRange(entitiesFakeReports);
                        PrintLog($"3. DB fakeReports 삽입 후");

                        PrintLog($"DB 삽입 저장 중..");
                        dbContext.SaveChanges();
                        PrintLog($"DB 삽입 정상 종료됐습니다");
                    }
                    catch
                    {
                        PrintLog($"DB 삽입 비정상 종료됐습니다!!!");
                    }
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
                StringBuilder sbAfterBuy10sec = new StringBuilder();
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                EachStock curEa;

                for (int i = 0; i < nStockLength; i++)
                {
                    try
                    {
                        curEa = ea[i];

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
                            client.UploadString($"{ftpHost}/chart/Every1min/{sToday}-{COMPUTER_LOCATION}-{curEa.sCode}-{curEa.sCodeName}.txt", sbEvery1min.ToString());

                        }
                    }
                    catch { }
                }
                PrintLog($"차트 데이터를 삽입완료했습니다.");
            }

            Task.Run(() => PutChartDataToDB());
        }
        #endregion
    }
}
