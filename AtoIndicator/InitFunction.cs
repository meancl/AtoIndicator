using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using AtoTrader.DB;
using AutoServer.Shared_Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AtoTrader.KiwoomLib.TimeLib;


namespace AtoTrader
{
    public partial class MainForm
    {


        // ----------------------------------------------
        // DB화되면 변경해야함

        //public int[] eachStockIdxArray; // Array[개인구조체 종목코드] => 개인구조체 인덱스
        public Dictionary<string, int> eachStockDict = new  Dictionary<string, int>();
        public Dictionary<string, int> eachStockNameDict = new  Dictionary<string, int>();

        public const int INIT_CODEIDX_NUM = -1; // eachStockIdxArray 초기화 상수
        public EachStock[] ea;  // 각 주식이 가지는 실시간용 구조체(개인구조체)
        public int nCurIdx; // 현재사용중인 개인구조체의 인덱스
        public int nEachStockIdx; // eachStockIdxArray[개인구조체 종목코드] <= nEachStockIdx++;
        public int nStockLength; // 관리대상종목 수

        public const int MAX_STOCK_NUM = 1000000; // 최종주식종목 수 0 ~ 999999 
        public const int MAX_STOCK_HOLDINGS_NUM = 600; // 보유주식을 저장하는 구조체 최대 갯수
        public const byte KOSDAQ_ID = 0;  // 코스닥을 증명하는 상수
        public const byte KOSPI_ID = 1; // 코스피를 증명하는 상수
        public const int NUM_SEP_PER_SCREEN = 100; // 한 화면번호 당 가능요청 수
        public List<string> bKeyList;
        public Dictionary<string, BasicInfo> bDict = new Dictionary<string, BasicInfo>();



        
        public StrategyNames strategyName;
        public Dictionary<string, int> strategyNameDict = new Dictionary<string, int>(); // {"전략명" : DB내 전략번호}

        public Dictionary<string, Dictionary<int, ScaleDatas>> scaleDict; 
        public MLContext mlContext;

#if AI
        public MMF mmf = new MMF(); // 공유메모리를 위한 인스턴스
#endif

        // ============================================
        // 마지막 편집일 : 2023-04-20
        // 1. 모니터링 DB 종목 로드
        // 2. 개인 구조체 배열 생성
        // 3. 보유잔고 구조체 배열 생성
        // 4. 순위계산용 구조체 배열 생성
        // 5. 각 전략 매매기록용 리스트 배열 생성
        // 6. 전략명 구조체 초기화
        // 7. 전략명에 따른 전략번호 로드
        // ============================================
        public void InitAto()
        {
            // 모니터링 DB 종목 로드 작업
            using (var dbContext = new myDbContext())
            {
                dbContext.Database.EnsureCreated();

                int minusDay;


                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                    minusDay = -3;
                else if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                    minusDay = -2;
                else
                    minusDay = -1;
                var yes = DateTime.Today.AddDays(minusDay); // 월요일에는 금요일 데이터를 가져오게 해야함
                DateTime dateTime = new DateTime(yes.Year, yes.Month, yes.Day);
                var yesterday = dateTime;

                bDict = (dbContext.basicInfo.Where(x => x.생성시간.Equals(yesterday)).ToList()).ToDictionary(keySelector: m => m.종목코드, elementSelector: m => m);
                bKeyList = bDict.Keys.ToList();

                nStockLength = bDict.Keys.Count;
            }



            ea = new EachStock[nStockLength]; // 개인 구조체 생성
            stockDashBoard.stockPanel = new StockPiece[nStockLength]; // 순위 결정을 위한 구조체 배열 생성

            strategyName = new StrategyNames();
            strategyHistoryList = new List<StrategyHistory>[strategyName.arrRealBuyStrategyName.Count]; // 전략매매후 정보를 담는 list
            for (int i = 0;i < nStockLength; i++)
            {
                ea[i].myStrategy.Init(strategyName.arrRealBuyStrategyName.Count); // 개인전략 구조체 초기화
                ea[i].fakeBuyStrategy.Init(strategyName.arrFakeBuyStrategyName.Count);
                ea[i].fakeResistStrategy.Init(strategyName.arrFakeResistStrategyName.Count);
                ea[i].fakeAssistantStrategy.Init(strategyName.arrFakeAssistantStrategyName.Count);
                ea[i].priceUpStrategy.Init(strategyName.arrPriceUpStrategyName.Count);
                ea[i].priceDownStrategy.Init(strategyName.arrPriceDownStrategyName.Count);
            }

            // 각 전략마다 기록용 리스트 생성
            for (int i = 0; i < strategyName.arrRealBuyStrategyName.Count; i++)
                strategyHistoryList[i] = new List<StrategyHistory>();


            // 전략명을 Key로 DB에서 전략번호를 받아온다.
            using (var db = new myDbContext())
            {
                #region Read Strategy Name
                // 데이터를 insert할때 데이터를 모두 입력하지 않았으면 나머지는 0 , null 등 초기값으로 myDbContext에서 관리해준다.
                // 해당 데이터를 myDbContext에서 관리하기 때문에 이미 관리대상인 데이터 값을 요청하면 sql에 새로 요청하지 않고
                // 관리데이터중에서 찾아 반환한게 되는데 이러면 자동설정된 값들을 제대로 반영하지 못하는 오류가 일어난다.
                // 추적 vs 비추적 이슈
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                StrategyNameDict cls;

                for (int i = 0; i < strategyName.GetStrategySize(); i++)
                {
                    if (strategyName.GetStrategyExistsByIdx(i))
                    {
                        var data = db.strategyNameDict.FirstOrDefault(x => x.sStrategyName.Equals(strategyName.arrRealBuyStrategyName[i]));
                        if (data == null)
                        {
                            cls = new StrategyNameDict
                            {
                                sStrategyName = strategyName.arrRealBuyStrategyName[i]
                            };

                            try
                            {
                                db.strategyNameDict.Add(cls);
                                db.SaveChanges();
                                // 데이터가 잘 들어가졌을때
                                var checkData = db.strategyNameDict.FirstOrDefault(x => x.sStrategyName.Equals(strategyName.arrRealBuyStrategyName[i]));
                                strategyNameDict[checkData.sStrategyName] = checkData.nStrategyNameIdx;
                            }
                            catch (Exception ex) // 오류가 났다면 다시
                            {
                                db.strategyNameDict.Remove(cls);
                                i--;
                                continue;
                            }

                        }
                        else
                        {
                            strategyNameDict[data.sStrategyName] = data.nStrategyNameIdx;
                        }
                    }
                }
                #endregion
            }
        } // END -- InitAto

        // ============================================
        // string형  코스닥, 코스피 종목코드의 배열 string[n] 변수에서
        // 한 화면번호 당 (최대)100개씩 넣고 주식체결 fid를 넣고
        // 실시간 데이터 요청을 진행
        // 코스닥과 코스피 배열에서 100개가 안되는 나머지 종목들은 코스닥,코스피 각 다른 화면번호에 실시간 데이터 요청
        // ============================================
        private void SubscribeRealData()
        {
            PrintLog("구독 시작..");
            int stockIndex = 0;
            int stockIterNum = nStockLength / NUM_SEP_PER_SCREEN;
            int stockRestNum = nStockLength % NUM_SEP_PER_SCREEN;

            string strStockCodeList;

            const string sFID = "41;228"; // 체결강도. 실시간 목록 FID들 중 겹치는게 가장 적은 FID
            string sScreenNum;
            // ------------------------------------------------------
            // 코스닥 실시간 등록
            // ------------------------------------------------------
            // 100개 단위
            for (int kosdaqIterIdx = 0; kosdaqIterIdx < stockIterNum; kosdaqIterIdx++)
            {
                sScreenNum = GetScreenNum();
                strStockCodeList = ConvertStrCodeList(stockIndex, stockIndex + NUM_SEP_PER_SCREEN, sScreenNum);
                axKHOpenAPI1.SetRealReg(sScreenNum, strStockCodeList, sFID, "0");
                stockIndex += NUM_SEP_PER_SCREEN;
            }
            // 나머지
            if (stockRestNum > 0)
            {
                sScreenNum = GetScreenNum();
                strStockCodeList = ConvertStrCodeList(stockIndex, stockIndex + stockRestNum, sScreenNum);
                axKHOpenAPI1.SetRealReg(sScreenNum, strStockCodeList, sFID, "0");
            }
            PrintLog("구독 완료..");

        } // End ---- SubscribeRealData




        // ============================================
        // 마지막 편집일 : 2023-04-20
        // 1. start ~ end 까지의 종목들을 하나의 문자열로 변환 ex) 1;2;3;4;5 ... 99;100
        // 2. 개인구조체 초기화
        // ============================================
        private string ConvertStrCodeList(int startIdx, int endIdx, string sScreenNum)
        {
            string sCodeList = "";
            for (int j = startIdx; j < endIdx; j++)
            {
                ////// eachStockIdx 설정 부분 ///////
                nCurIdx = eachStockDict[bKeyList[j].Trim()] = nEachStockIdx++;
                eachStockNameDict[bDict[bKeyList[j]].종목명.Trim()] = nCurIdx;

                ////// eachStock 초기화 부분 ////////// 
                ea[nCurIdx].sRealScreenNum = sScreenNum; // 화면번호 설정
                ea[nCurIdx].sCode = bKeyList[j]; // 종목코드 설정

                if (bDict[bKeyList[j]].종목타입.Equals("KOSPI")) // 장구분 문자열 설정
                {
                    ea[nCurIdx].nMarketGubun = KOSPI_ID;
                    ea[nCurIdx].sMarketGubunTag = "KOSPI";
                }
                else if (bDict[bKeyList[j]].종목타입.Equals("KOSDAQ"))
                {
                    ea[nCurIdx].nMarketGubun = KOSDAQ_ID;
                    ea[nCurIdx].sMarketGubunTag = "KOSDAQ";
                }
                ea[nCurIdx].sCodeName = bDict[bKeyList[j]].종목명;
                ea[nCurIdx].lShareOutstanding = bDict[bKeyList[j]].유통주식;
                ea[nCurIdx].lTotalNumOfStock = bDict[bKeyList[j]].상장주식;
                ea[nCurIdx].nYesterdayEndPrice = bDict[bKeyList[j]].현재가;
                ea[nCurIdx].lFixedMarketCap = bDict[bKeyList[j]].시가총액;

                // 게시판의 각 종목 초기화
                tmpStockPiece.sCode = ea[nCurIdx].sCode;
                tmpStockPiece.nEaIdx = nCurIdx;
                stockDashBoard.stockPanel[nCurIdx] = tmpStockPiece;

                // 개인구조체 초기화
                ea[nCurIdx].Init();

                sCodeList += bKeyList[j];
                if (j < endIdx - 1)
                    sCodeList += ';';
            }
            return sCodeList;
        } // End ---- ConvertStrCodeList 


    }
}
