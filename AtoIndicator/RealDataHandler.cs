using System;
using AtoTrader.DB;
using System.Collections.Generic;
using static AtoTrader.KiwoomLib.TimeLib;
using static AtoTrader.KiwoomLib.PricingLib;
using static AtoTrader.KiwoomLib.Errors;
using static AtoTrader.TradingBlock.TimeLineGenerator;
using System.Linq;
using static AtoTrader.Utils.Protractor;
using static AtoTrader.Utils.Comparer;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace AtoTrader
{
    public partial class MainForm
    {
        #region 변수

        public Random rand = new Random(); // real

        public const int AI_ONCE_MAXNUM = 5;
        public Queue<AIResponseSlot> aiQueue = new Queue<AIResponseSlot>(); // 매매신청을 담는 큐, 매매컨트롤러가 사용할 큐 // real
      
        public List<StrategyHistory>[] strategyHistoryList;
        public List<StrategyHistory> totalTradeHistoryList = new List<StrategyHistory>();

        public AIResponseSlot aiSlot;


        public bool isMarketStart; // true면 장중, false면 장시작전,장마감후 // real
        public int nSharedTime; // 모든 종목들이 공유하는 현재시간( 주식체결 실시간 데이터만 기록, 호가잔량 실시간은 시간이 정렬돼있지 않음 ) // real


        public DateTime dtBeforeOrderTime = DateTime.UtcNow; // real
        public DateTime dtCurOrderTime; // real

        public int nReqestCount; // real
        public bool isSendOrder; // real
        public bool isForbidTrade = false; // real
        public int nFirstTime; // real
        public StockPiece tmpStockPiece; // real
        public StockDashBoard stockDashBoard; // real
        public int nPrevBoardUpdateTime; // real
        public int nTimeLineIdx = BRUSH; // real
        public double[] arrFSlope = new double[PICKUP_CNT]; // real
        public double[] arrRecentFSlope = new double[PICKUP_CNT]; // real
        public double[] arrHourFSlope = new double[PICKUP_CNT]; // real
                                                                // StreamWriter DashSw = new StreamWriter(new FileStream(sMessageLogPath + "게시판.txt", FileMode.Create)); // real 두두두둥

        public bool isOnlyLogTime = false;
        public int nPrevTotalClock;
        public StringBuilder sSharedSellDescription = new StringBuilder();


        public int nSendNum = 0;
        public int nHourPtr = 1;
        public const int PAPER_HOUR_LIMIT = 3600;
        public const int PAPER_HOUR_PAD = 100;
        public const int PAPER_HOUR_BUY_LIMIT_DIFF = 300;
        public DateTime dFirstForPaper = DateTime.UtcNow;

        public bool MEME_Controler = true;
        public bool MEME_BUYController = true;

        #endregion

        #region 상수
        public const double STOCK_TAX = 0.0023; // 거래세 
        public const double STOCK_FEE = 0.00015; // 증권 매매수수료
        public const double VIRTUAL_STOCK_FEE = 0.0035; // 가상증권 매매수수료
        public const double VIRTUAL_STOCK_COMMISSION = STOCK_TAX + VIRTUAL_STOCK_FEE * 2; // 최종 거래수수료 *현재 : 거래세 + 가상증권 매매수수료 *  2( 가상증권 매수수수료 + 가상증권매매수수료 )
        public const double REAL_STOCK_COMMISSION = STOCK_TAX + STOCK_FEE * 2;

        public const double REAL_STOCK_BUY_COMMISION = STOCK_FEE;
        public const double REAL_STOCK_SELL_COMMISION = STOCK_FEE + STOCK_TAX;

        public const double VIRTUAL_STOCK_BUY_COMMISION = VIRTUAL_STOCK_FEE;
        public const double VIRTUAL_STOCK_SELL_COMMISION = VIRTUAL_STOCK_FEE + STOCK_TAX;
        // ------------------------------------------------------
        // 추세 상수
        // ------------------------------------------------------
        public const int BRUSH = 20;
        public const int PICKUP_CNT = 300;
        public const int PICK_BEFORE = 15; // 15분전
        public const int HOUR_BEFORE = 60; // 1시간전
        public const int TEN_SEC_PICKUP_CNT = 300;
        public const int TEN_SEC_PICK_BEFORE = 30; // 5분전
        public const int TEN_SEC_TEN_BEFORE = 60; // 10분전
        public const int MIN_DENOM = 1; // 기울기를 구할때 최소 분모는 MIN_DENOM.
        public const int MAX_DENOM = 90; // 기울기를 구할때 최대 분모. 쓸지 안쓸지 모름
        public const int COMMON_DENOM = 30; // 기울기를 구할때 공통분모. 현재 공통분모로 사용중
        public const int HOUR_COMMON_DENOM = 20; // 기울기를 구할때 공통분모. 현재 공통분모로 사용중
        public const int RECENT_COMMON_DENOM = 10; // 기울기를 구할때 공통분모. 현재 공통분모로 사용중
        public const int STANDARD_BUY_PRICE = (int)MILLION;   // 각 종목이 한번에 최대 살 수 있는 금액 ex. 삼백만원 // real
        // ------------------------------------------------------
        // 이평선 상수
        // ------------------------------------------------------
        public const int MA5M = 5;
        public const int MA20M = 20;
        public const int MA1H = 60;
        public const int MA2H = 120;
        public const int MA_EXCEED_CNT = 30; // ma가 현재값 위나 아래 한 공간에 계속 머무는 횟수가 MA_EXCEED_CNT이었다가 다른 공간으로 넘어가면 역전된다는 의미

        public const int TEN_SEC_MA2M = 18;
        public const int TEN_SEC_MA10M = 60;
        public const int TEN_SEC_MA20M = 120;
        // ------------------------------------------------------
        // 임시파일저장명 상수
        // ------------------------------------------------------
        public const string sMessageLogPath = @"로그\";

        // ------------------------------------------------------
        // 대응영역 상수
        // ------------------------------------------------------
        public const int PREEMPTION_ACCESS_SEC = 120;
        public const int RESPITE_ACCESS_SEC = 20;
        public const int PREEMPTION_UPDATE_SEC = 10;
        public const int RESPITE_UPDATE_SEC = 10;
        public const int RESPITE_LIMIT_SEC = 600;
        public const double RESPITE_INIT = -100;
        public const double RESPITE_CRITICAL_PADDING = 0.015;
        public const int LIMIT_STAY_SEC = 1800; // 제한 미터치시간이 30분
        public const int END_STAY_SEC = 5400; // 마지막 제한 미터치시간이 1시간 30분
        public string NEW_LINE = Environment.NewLine;
        public const int UNDISPOSAL_STRATEGY_IDX = -1;

        // 매도 방식
        public enum TradeMethodCategory
        {
            None,
            FixedMethod,
            RisingMethod,
            BottomUpMethod,
            ScalpingMethod,
            OnlyAIUsedMethod,
        }

        public const int FAKE_REQUEST_SIGNAL = -1;
        public const int FAKE_BUY_SIGNAL = 0;
        public const int FAKE_RESIST_SIGNAL = 1;
        public const int FAKE_ASSISTANT_SIGNAL = 2;
        public const int PRICE_UP_SIGNAL = 3;
        public const int PRICE_DOWN_SIGNAL = 4;
        public const int REAL_BUY_SIGNAL = 5;
        public const int REAL_SELL_SIGNAL = 6;


        public const string SEND_ORDER_ERROR_CHECK_PREFIX = "둥둥둥";


        public const int EYES_CLOSE_CRUSH_NUM = 3;

        public const double fPushWeight = 0.8;
        public const double fRoughPushWeight = 0.6;
        public const int SHORT_UPDATE_TIME = 20;
        public const int MAX_REQ_SEC = 150; // 최대매수요청시간
        public const int BUY_CANCEL_ACCESS_SEC = 15;  // 매수취소 가능할때까지 시간

        public const int REAL_BUY_MAX_NUM = 100; // 최대 매매블록 갯수
        public const int BAR_REAL_BUY_MAX_NUM = 3; // 한 봉에

        public const int PRICE_UP_MAX_NUM = 200; // 최대 가격up 갯수
        public const int PRICE_DOWN_MAX_NUM = 200; // 최대 가격down 갯수
        public const int FAKE_BUY_MAX_NUM = 200;
        public const int FAKE_RESIST_MAX_NUM = 200;
        public const int FAKE_ASSISTANT_MAX_NUM = 200;

        //public const int REAL_BUY_STRATEGY_NUM = 84; // 전략 갯수
        //public const int PRICE_UP_STRATEGY_NUM = 20; // 전략 갯수
        //public const int PRICE_DOWN_STRATEGY_NUM = 20; // 전략 갯수
        //public const int FAKE_BUY_STRATEGY_NUM = 20;
        //public const int FAKE_RESIST_STRATEGY_NUM = 20;
        //public const int FAKE_ASSISTANT_STRATEGY_NUM = 20;

        public const long SHT_PER_INIT = BILLION;

        public static int COMPUTER_LOCATION = -1;
        public static int SELL_VERSION = 0;
        public const int TRADE_CONTROLLER_ACCESS_BUY_LIMIT = 10;
        public const int SYSTEMETIC_SELL_FAIL = 10;
        public const int SYSTEMETIC_BUY_CANCEL_FAIL = 6;


        public const int ONE_SEC_MIL_SEC = 1000;
        public const int OVER_FLOW_MIL_SEC_CHECK = 1100;
        public DateTime dOverFlowToUp = DateTime.UtcNow;
        public int nOverFlowCnt = 0;
        #endregion

        #region  실시간이벤트핸들러
        private void OnReceiveRealDataHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {


            string sCode = e.sRealKey.Trim(); // 종목코드

            bool isHogaJanRyang = false;
            bool isZooSikCheGyul = false;



            #region 실시간 장시작시간
            if (e.sRealType.Equals("장시작시간"))
            {
                string sGubun = axKHOpenAPI1.GetCommRealData(e.sRealKey, 215); // 장운영구분 0 :장시작전, 3 : 장중, 4 : 장종료
                string sTime = axKHOpenAPI1.GetCommRealData(e.sRealKey, 20); // 체결시간
                string sTimeRest = axKHOpenAPI1.GetCommRealData(e.sRealKey, 214); // 잔여시간
                if (sGubun.Equals("0")) // 장시작 전
                {
                    PrintLog($"{sTimeRest} : 장시작전");
                }
                else if (sGubun.Equals("3")) // 장 중
                {
                    PrintLog("장중");
                    isMarketStart = true;
                    isBuyDeny = false;
                    dFirstForPaper = DateTime.UtcNow;
                    dtBeforeOrderTime = DateTime.UtcNow;
                    nFirstTime = int.Parse(sTime);
                    nFirstTime -= nFirstTime % MINUTE_KIWOOM;
                    nSharedTime = nFirstTime;
                    nPrevBoardUpdateTime = nFirstTime;

                }
                else
                {
                    if (sGubun.Equals("2")) // 장 종료 10분전 동시호가
                    {
                        PrintLog($"{sTimeRest} : 장종료전");
                        
                    }
                    else if (sGubun.Equals("4")) // 장 종료
                    {
                        PrintLog("장종료");
                        isMarketStart = false;
                        PutTradeResultAsync();
                        PutChartResultAsync();
                    }
                }
                return; // 장시작시간 실시간데이터는  여기서 종료
            }
            #endregion
            else if (e.sRealType.Equals("주식호가잔량"))
            {
                isHogaJanRyang = true;
            }
            else if (e.sRealType.Equals("주식체결"))
            {
                isZooSikCheGyul = true;

                #region 시간체크
                nSharedTime = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 20))); // 현재시간

                if (nPrevTotalClock != nSharedTime)
                {
                    nPrevTotalClock = nSharedTime;
                    totalClockLabel.Text = "현재시간 : " + nPrevTotalClock;
                }
                #endregion

            }
            else // 장시작시간과 주식호가잔량, 주식체결 제외한 실시간정보는 out
                return;

            #region 장 시작
            if (isMarketStart) // 장 시작하면 찾아와라.. 
            {
                #region SHUTDOWN_TIME 체크
                if (!isOnlyLogTime && nSharedTime >= SHUTDOWN_TIME) // 3시가 넘었으면
                {
                    isOnlyLogTime = true;
                    PrintLog("3시가 지났다");
                }
                #endregion

                #region nCurIdx 가져오기
                try
                {
                    nCurIdx = eachStockDict[sCode]; // 오류 가능원인 1. 해당종목코드가 리스트에 없다 2. sCode가  이상한거다
                    if (nCurIdx == INIT_CODEIDX_NUM)
                        throw new Exception();
                }
                catch
                {
                    return;
                }
                #endregion

                #region 정기적 1분 업데이트
                // =============================================================
                // 정기적 업데이트 
                // =============================================================
                // nPrevBoardUpdateTime은 장시작시간 + MINUTE_SEC 에 처음 접근이 가능해야함.
                if (SubTimeToTimeAndSec(nSharedTime, nPrevBoardUpdateTime) >= MINUTE_SEC && nSharedTime <= MARKET_END_TIME) // 매 분마다 업데이트 진행 
                {
                    PrintLog($"{nSharedTime} 누적 화면번호 수 {nUsingScreenNum}");
                    PrintMemoryUsage();
                    #region 순위 지정
                    // ===========================================================================================================
                    // 게시판 Part
                    // ===========================================================================================================
                    // 매 분마다 해당 개인구조체.rankSystem에 업데이트된 순위를 삽입해준다.
                    int i, j;
                    // 모든 종목 개인구조체에서 거래대금,거래량,속도 등의 정보를 가져온다.
                    // 분당 거래대금 등 분당시리즈는 데이터를 받은 후 초기화한다.
                    // 순위합 역시 초기화한다.
                    try
                    {

                        for (i = 0; i < nStockLength; i++) // 업데이트
                        {
                            // 전체
                            tmpStockPiece = stockDashBoard.stockPanel[i]; // 구조체는 값형식이라 직접대입을 할 수 없다.(클래스로 하면 되긴 하는데 부가적인 성능부하가 생길거같아서) 
                            tmpStockPiece.lTotalTradePrice = ea[tmpStockPiece.nEaIdx].lTotalTradePrice; // 1. 거래대금 
                            tmpStockPiece.fTotalTradeVolume = ea[tmpStockPiece.nEaIdx].fTotalTradeVolume; // 2. 상대적거래수량
                            tmpStockPiece.lTotalBuyPrice = ea[tmpStockPiece.nEaIdx].lTotalBuyPrice; // 3. 매수대금
                            tmpStockPiece.fTotalBuyVolume = ea[tmpStockPiece.nEaIdx].fTotalBuyVolume; // 4. 상대적매수수량
                            tmpStockPiece.nAccumCount = ea[tmpStockPiece.nEaIdx].nChegyulCnt; // 5. 누적카운트
                            tmpStockPiece.fTotalPowerWithOutGap = ea[tmpStockPiece.nEaIdx].fPowerWithoutGap; // 6. 손익률
                            tmpStockPiece.lMarketCap = ea[tmpStockPiece.nEaIdx].lMarketCap; // 7. 시가총액
                            tmpStockPiece.nSummationRank = 0; // 8. 순위합 초기화

                            // 분당
                            tmpStockPiece.lMinuteTradePrice = ea[tmpStockPiece.nEaIdx].lMinuteTradePrice; // 1. 분간 거래대금
                            tmpStockPiece.fMinuteTradeVolume = ea[tmpStockPiece.nEaIdx].fMinuteTradeVolume; // 2. 분간 상대적거래수량
                            tmpStockPiece.lMinuteBuyPrice = ea[tmpStockPiece.nEaIdx].lMinuteBuyPrice; // 3. 분간 매수대금
                            tmpStockPiece.fMinuteBuyVolume = ea[tmpStockPiece.nEaIdx].fMinuteBuyVolume; // 4. 분간 상대적매수수량
                            tmpStockPiece.fMinutePower = ea[tmpStockPiece.nEaIdx].fMinutePower; // 5. 분간 손익율
                            tmpStockPiece.nMinuteCnt = ea[tmpStockPiece.nEaIdx].nMinuteCnt; // 6. 분간 카운트
                            tmpStockPiece.nMinuteUpDown = ea[tmpStockPiece.nEaIdx].nMinuteUpDown; // 7. 분간 위아래
                            tmpStockPiece.nSummationMinuteRank = 0; // 8. 분간 순위합 초기화

                            // 분간 데이터재료 초기화
                            ea[tmpStockPiece.nEaIdx].lMinuteTradePrice = 0; // 1
                            ea[tmpStockPiece.nEaIdx].fMinuteTradeVolume = 0; // 2
                            ea[tmpStockPiece.nEaIdx].lMinuteBuyPrice = 0; // 3
                            ea[tmpStockPiece.nEaIdx].fMinuteBuyVolume = 0; // 4
                            ea[tmpStockPiece.nEaIdx].lMinuteTradeVolume = 0; // 2- 재료
                            ea[tmpStockPiece.nEaIdx].lMinuteBuyVolume = 0; // 4- 재료
                            ea[tmpStockPiece.nEaIdx].fMinutePower = 0; // 5
                            ea[tmpStockPiece.nEaIdx].nMinuteCnt = 0; // 6
                            ea[tmpStockPiece.nEaIdx].nMinuteUpDown = 0; // 7

                            stockDashBoard.stockPanel[i] = tmpStockPiece;


                        }
                        stockDashBoard.nDashBoardCnt++;

                        // -------------------------------------
                        // -------------------------------------
                        // 전체 순위설정
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.lTotalTradePrice.CompareTo(x.lTotalTradePrice)); // 1. 거래대금 내림차순 ( 대금은 높을 수 록 좋다)
                                                                                                                                       //stockDashBoard.stockPanel = stockDashBoard.stockPanel.OrderByDescending<StockPiece, double>(x => x.lTotalTradePrice).ToArray<StockPiece>();
                        for (i = 0; i < nStockLength; i++) // 거래대금 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nTotalTradePriceRanking = i + 1; // 1위부터 ~ 2000위까지
                            stockDashBoard.stockPanel[i].nSummationRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.fTotalTradeVolume.CompareTo(x.fTotalTradeVolume)); // 2. 상대적거래수량 내림차순 ( 수량은 높을 수 록 좋다)
                        for (i = 0; i < nStockLength; i++) // 상대적거래수량 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nTotalTradeVolumeRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.lTotalBuyPrice.CompareTo(x.lTotalBuyPrice)); // 3. 매수대금 내림차순 ( 대금은 높을 수 록 좋다)
                        for (i = 0; i < nStockLength; i++) // 매수대금 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nTotalBuyPriceRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.fTotalBuyVolume.CompareTo(x.fTotalBuyVolume)); // 4. 상대적매수수량 내림차순( 수량은 높을 수 록 좋다)
                        for (i = 0; i < nStockLength; i++) // 상대적매수수량 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nTotalBuyVolumeRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.nAccumCount.CompareTo(x.nAccumCount)); // 5. 누적카운트 내림차순(속도는 빠를 수 록 좋다)
                        for (i = 0; i < nStockLength; i++) // 누적카운트 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nAccumCountRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.fTotalPowerWithOutGap.CompareTo(x.fTotalPowerWithOutGap)); // 6. 손익률 내림차순(가격은 오를 수 록 좋다)
                        for (i = 0; i < nStockLength; i++) // 손익률 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nPowerRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.lMarketCap.CompareTo(x.lMarketCap));  // 7. 시가총액 내림차순( 가격은 높을 수 록 좋다 )
                        for (i = 0; i < nStockLength; i++) // 시가총액 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nMarketCapRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationRank += i + 1;
                        }
                        // -----------------
                        // 바로 위까지 순위합을 구하고 이제 정렬
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => x.nSummationRank.CompareTo(y.nSummationRank));  // 8. 순위합 기준 오름차순( 순위는 낮을 수 록 좋다 )
                        for (i = 0; i < nStockLength; i++)  // 전체순위합 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nSummationRanking = i + 1;
                        }


                        // -------------------------------------
                        // -------------------------------------
                        // 분간 순위설정
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.lMinuteTradePrice.CompareTo(x.lMinuteTradePrice));  // 1. 분당 거래대금 내림차순 ( 대금은 높을 수 록 좋다 )
                        for (i = 0; i < nStockLength; i++) // 분당 거래대금 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nMinuteTradePriceRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationMinuteRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.fMinuteTradeVolume.CompareTo(x.fMinuteTradeVolume));  // 2. 분당 상대적거래수량 내림차순( 수량은 높을 수록 좋다 )
                        for (i = 0; i < nStockLength; i++) // 분당 상대적거래수량 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nMinuteTradeVolumeRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationMinuteRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.lMinuteBuyPrice.CompareTo(x.lMinuteBuyPrice));  // 3. 분당 매수대금 내림차순 ( 대금은 높을 수 록 좋다 )
                        for (i = 0; i < nStockLength; i++) // 분당 매수대금 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nMinuteBuyPriceRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationMinuteRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.fMinuteBuyVolume.CompareTo(x.fMinuteBuyVolume));  // 4. 분당 상대적매수수량 내림차순( 수량은 높을 수록 좋다 )
                        for (i = 0; i < nStockLength; i++) // 분당 상대적매수수량 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nMinuteBuyVolumeRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationMinuteRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.fMinutePower.CompareTo(x.fMinutePower));  // 5. 분당 손익율 내림차순( 손익율은 높을 수록 좋다 )
                        for (i = 0; i < nStockLength; i++) // 분당 손익율 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nMinutePowerRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationMinuteRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.nMinuteCnt.CompareTo(x.nMinuteCnt));  // 6. 분당 카운트 내림차순( 속도는 빠를 수 록 좋다 )
                        for (i = 0; i < nStockLength; i++) // 분당 카운트 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nMinuteCountRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationMinuteRank += i + 1;
                        }
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => y.nMinuteUpDown.CompareTo(x.nMinuteUpDown));  // 7. 분당 위아래 내림차순( 위아래가 많을 수 록 좋다 )
                        for (i = 0; i < nStockLength; i++) // 분당 카운트 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nMinuteUpDownRanking = i + 1;
                            stockDashBoard.stockPanel[i].nSummationMinuteRank += i + 1;
                        }
                        // -----------------
                        // 바로 위까지 분당 순위합을 구하고 이제 정렬
                        Array.Sort<StockPiece>(stockDashBoard.stockPanel, (x, y) => x.nSummationMinuteRank.CompareTo(y.nSummationMinuteRank));  // 8. 분당 순위합 기준 오름차순( 순위는 낮을 수 록 좋다 )
                        for (i = 0; i < nStockLength; i++)  // 분당순위합 순위설정
                        {
                            ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nMinuteSummationRanking = i + 1;
                        }




                        int nRankIdx;
                        for (i = 0; i < nStockLength; i++) // 순위 지정
                        {
                            nRankIdx = stockDashBoard.stockPanel[i].nEaIdx;
                            ea[nRankIdx].rankSystem.nTime = nSharedTime;

                            Ranking tmpRank;
                            tmpRank.nRecordTime = ea[nRankIdx].rankSystem.nTime; // 기록시간 = 해당시간

                            // 전체
                            tmpRank.nTotalTradePriceRanking = ea[nRankIdx].rankSystem.nTotalTradePriceRanking; // 1. 거래대금 순위 지정
                            tmpRank.nTotalTradeVolumeRanking = ea[nRankIdx].rankSystem.nTotalTradeVolumeRanking; // 2. 상대적거래수량 순위 지정
                            tmpRank.nTotalBuyPriceRanking = ea[nRankIdx].rankSystem.nTotalBuyPriceRanking; // 3. 매수대금 순위 지정
                            tmpRank.nTotalBuyVolumeRanking = ea[nRankIdx].rankSystem.nTotalBuyVolumeRanking; // 4. 상대적매수수량 순위 지정
                            tmpRank.nAccumCountRanking = ea[nRankIdx].rankSystem.nAccumCountRanking; // 5. 누적카운트 순위 지정
                            tmpRank.nPowerRanking = ea[nRankIdx].rankSystem.nPowerRanking; // 6. 손익률 순위 지정
                            tmpRank.nMarketCapRanking = ea[nRankIdx].rankSystem.nMarketCapRanking; // 7. 시가총액 순위 지정
                            tmpRank.nSummationRanking = ea[nRankIdx].rankSystem.nSummationRanking; // 8. 전체 순위합 순위 지정

                            tmpRank.nSummationMove = ea[nRankIdx].rankSystem.nSummationMove = ea[nRankIdx].rankSystem.nSummationRanking - ea[nRankIdx].rankSystem.nPrevSummationRanking; // 순위 변동 지정
                            ea[nRankIdx].rankSystem.nPrevSummationRanking = ea[nRankIdx].rankSystem.nSummationRanking; // 현재 총순위 기록

                            // 분당
                            tmpRank.nMinuteTradePriceRanking = ea[nRankIdx].rankSystem.nMinuteTradePriceRanking; // 1. 분당 거래대금 순위 지정 
                            tmpRank.nMinuteTradeVolumeRanking = ea[nRankIdx].rankSystem.nMinuteTradeVolumeRanking; // 2. 분당 상대적거래수량 순위 지정 
                            tmpRank.nMinuteBuyPriceRanking = ea[nRankIdx].rankSystem.nMinuteBuyPriceRanking; // 3. 분당 매수대금 순위 지정 
                            tmpRank.nMinuteBuyVolumeRanking = ea[nRankIdx].rankSystem.nMinuteBuyVolumeRanking; // 4. 분당 상대적매수수량 순위 지정 
                            tmpRank.nMinutePowerRanking = ea[nRankIdx].rankSystem.nMinutePowerRanking; // 5. 분당 손익율 순위 지정
                            tmpRank.nMinuteCountRanking = ea[nRankIdx].rankSystem.nMinuteCountRanking; // 6. 분당 카운트 순위 지정
                            tmpRank.nMinuteUpDownRanking = ea[nRankIdx].rankSystem.nMinuteUpDownRanking; // 7. 분당 위아래 순위 지정
                            tmpRank.nMinuteRanking = ea[nRankIdx].rankSystem.nMinuteSummationRanking; // 8. 분당 순위합 순위 지정

                            if (ea[nRankIdx].rankSystem.nSummationRanking <= 10) // 10위권 내
                                ea[nRankIdx].rankSystem.nRankHold10++;
                            else
                                ea[nRankIdx].rankSystem.nRankHold10 = 0;

                            if (ea[nRankIdx].rankSystem.nSummationRanking <= 20) // 20위권 내
                                ea[nRankIdx].rankSystem.nRankHold20++;
                            else
                                ea[nRankIdx].rankSystem.nRankHold20 = 0;

                            if (ea[nRankIdx].rankSystem.nSummationRanking <= 50) // 50위권 내
                                ea[nRankIdx].rankSystem.nRankHold50++;
                            else
                                ea[nRankIdx].rankSystem.nRankHold50 = 0;

                            if (ea[nRankIdx].rankSystem.nSummationRanking <= 100) // 100위권 내
                                ea[nRankIdx].rankSystem.nRankHold100++;
                            else
                                ea[nRankIdx].rankSystem.nRankHold100 = 0;

                            if (ea[nRankIdx].rankSystem.nSummationRanking <= 200) // 200위권 내
                                ea[nRankIdx].rankSystem.nRankHold200++;
                            else
                                ea[nRankIdx].rankSystem.nRankHold200 = 0;
                            // 별도로 ea[stockDashBoard.stockPanel[i].nEaIdx].rankSystem.nCurIdx를 작업 안해줘도 된다, 0이었다가 1이 됐으니 이상태에서
                            // 누적순위 / nCurIdx하면 된다.
                            // 전체 누적합

                            ea[nRankIdx].rankSystem.arrRanking[nTimeLineIdx] = tmpRank; // arrRanking[0]은 장시작시간 + MINUTE_SEC
                            ea[nRankIdx].rankSystem.nCurIdx++;
                        }

                    }
                    catch (Exception rankEx)
                    {

                    }

                    #endregion

                    #region 각 종목 업데이트
                    // ===========================================================================================================
                    // 각 개인구조체 업데이트
                    // ===========================================================================================================
                    for (i = 0; i < nStockLength; i++)
                    {

                        if (!ea[i].isFirstCheck)
                        {
                            ea[i].nJumpCnt++;
                            continue;
                        }

                        #region 타임라인 업데이트
                        // ===========================================================================================================
                        // 타임라인 Part
                        // ===========================================================================================================
                        {
                            if (nTimeLineIdx >= ea[i].timeLines1m.arrTimeLine.Length)
                                continue;
                            ea[i].timeLines1m.nRealDataIdx = ea[i].timeLines1m.nPrevTimeLineIdx; // 지금은 nRealDataIdx인것
                            ea[i].timeLines1m.nPrevTimeLineIdx++; // 다음 페이즈로 넘어간다는 느낌, 갯수를 기준으로 할 때 유용함
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTimeIdx = nTimeLineIdx; // 배열원소에 현재 타임라인 인덱스 삽입
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime = AddTimeBySec(nFirstTime, (nTimeLineIdx - BRUSH) * MINUTE_SEC); // 9:00 ~~

                            if (ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs == 0)
                            {
                                if (ea[i].timeLines1m.nFsPointer == 0)
                                {
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs = ea[i].nTodayStartPrice;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs = ea[i].nTodayStartPrice;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nMaxFs = ea[i].nTodayStartPrice;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nMinFs = ea[i].nTodayStartPrice;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpFs = ea[i].nTodayStartPrice;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownFs = ea[i].nTodayStartPrice;
                                }
                                else
                                {
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs = ea[i].timeLines1m.nFsPointer;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs = ea[i].timeLines1m.nFsPointer;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nMaxFs = ea[i].timeLines1m.nFsPointer;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nMinFs = ea[i].timeLines1m.nFsPointer;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpFs = ea[i].timeLines1m.nFsPointer;
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownFs = ea[i].timeLines1m.nFsPointer;

                                    ea[i].nMissCount++; // 데이터가 1도 없었다???
                                }

                            }
                            else
                            {
                                if (ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nCount < 20)
                                    ea[i].nFewSpeedCount++; // 한 분봉에 속도가 20도 안된다??

                                if (ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs == ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs)
                                    ea[i].nNoMoveCount++; // 1도 안움직였다???
                            }

                            // 해당 타임라인이 세팅된 다음
                            {

                                double fCurCandlePower = (double)(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs) / ea[i].nYesterdayEndPrice;
                                double fCurUpTailPower = (double)(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nMaxFs - ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpFs) / ea[i].nYesterdayEndPrice;
                                double fCurDownTailPower = (double)(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownFs - ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nMinFs) / ea[i].nYesterdayEndPrice;

                                if (fCurCandlePower >= 0.01)
                                {
                                    ea[i].timeLines1m.upCandleList.Add((nSharedTime, fCurCandlePower));
                                    if (fCurCandlePower >= 0.03)
                                        ea[i].timeLines1m.shootingList.Add((nSharedTime, fCurCandlePower));
                                }
                                else if (fCurCandlePower <= -0.01)
                                    ea[i].timeLines1m.downCandleList.Add((nSharedTime, fCurCandlePower));

                                if (fCurUpTailPower >= 0.01)
                                    ea[i].timeLines1m.upTailList.Add((nSharedTime, fCurCandlePower));

                                if (fCurDownTailPower >= 0.01)
                                    ea[i].timeLines1m.downTailList.Add((nSharedTime, fCurCandlePower));
                            }

                            // -----------------------------------------------------------------
                            // 초기화 영역( 조건 설정을 위한 변수들의 )
                            // -----------------------------------------------------------------
                            {
                                if (ea[i].timeLines1m.nMaxPricePrevMinute < ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nMaxFs)
                                    ea[i].timeLines1m.nMaxPricePrevMinute = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nMaxFs;

                                if (ea[i].timeLines1m.nMaxUpFs < ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpFs) // 최대값 구하기
                                    ea[i].timeLines1m.nMaxUpFs = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpFs;

                                if (ea[i].timeLines1m.nMinDownFs == 0 || ea[i].timeLines1m.nMinDownFs > ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownFs) // 최소값 구하기
                                    ea[i].timeLines1m.nMinDownFs = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownFs;

                            }

                        } // END ---- 타임라인
                        #endregion

                        #region 각도 계산
                        // ===========================================================================================================
                        // 추세 Part 
                        // ===========================================================================================================
                        {
                            int nPick1, nPick2, nPick3, nPick4, nPick5, nPick6;
                            int nLeftPick1, nRightPick2, nLeftPick3, nRightPick4, nLeftPick5, nRightPick6;

                            // 기울기 확인
                            for (j = 0; j < PICKUP_CNT; j++)
                            {

                                nPick1 = nPick2 = nPick3 = nPick4 = nPick5 = nPick6 = 0;
                                nLeftPick1 = nRightPick2 = nLeftPick3 = nRightPick4 = nLeftPick5 = nRightPick6 = 0;

                                int nRecentPickBefore = Min(PICK_BEFORE, ea[i].timeLines1m.nPrevTimeLineIdx);
                                int nHourPickBefore = Min(HOUR_BEFORE, ea[i].timeLines1m.nPrevTimeLineIdx);

                                while ((nPick1 == nPick2) || (nPick3 == nPick4) || (nPick5 == nPick6)) // 같으면 기울기를 구할 수 없기 때문에 서로 다를때까지 반복한다.
                                {
                                    if (nPick1 == nPick2)
                                    {
                                        nPick1 = rand.Next(ea[i].timeLines1m.nPrevTimeLineIdx);  // 0 ~ 현재인덱스
                                        nPick2 = rand.Next(ea[i].timeLines1m.nPrevTimeLineIdx);  // 0 ~ 현재인덱스
                                        nLeftPick1 = Min(nPick1, nPick2);
                                        nRightPick2 = Max(nPick1, nPick2);
                                    }
                                    if (nPick3 == nPick4)
                                    {
                                        nPick3 = rand.Next(ea[i].timeLines1m.nPrevTimeLineIdx - nRecentPickBefore, ea[i].timeLines1m.nPrevTimeLineIdx);
                                        nPick4 = rand.Next(ea[i].timeLines1m.nPrevTimeLineIdx - nRecentPickBefore, ea[i].timeLines1m.nPrevTimeLineIdx);
                                        nLeftPick3 = Min(nPick3, nPick4);
                                        nRightPick4 = Max(nPick3, nPick4);
                                    }
                                    if (nPick5 == nPick6)
                                    {
                                        nPick5 = rand.Next(ea[i].timeLines1m.nPrevTimeLineIdx - nHourPickBefore, ea[i].timeLines1m.nPrevTimeLineIdx);
                                        nPick6 = rand.Next(ea[i].timeLines1m.nPrevTimeLineIdx - nHourPickBefore, ea[i].timeLines1m.nPrevTimeLineIdx);
                                        nLeftPick5 = Min(nPick5, nPick6);
                                        nRightPick6 = Max(nPick5, nPick6);
                                    }
                                }


                                // 왜 gap으로 나누냐면.... 간편해서
                                // Q. 그러면 종목들 간 값의 차이가 나지 않느냐?? 9900짜리는 50으로 나누고 10000짜리는 100으로 나누는데??
                                // A. 100으로 나눈다면 기울기값은 낮아진다. => 패널티를 받게된다. => 종목들 간 gap으로 나누는건 균형을 맞춘다(?? : 원래 한 단계를 넘을때 더 많은 저항이 생길 수 있으니까 패널티를 주는게 괜찮은거 같다는게 내 의견.)
                                // 수정 : (GetAutoGap --> GetSlopeGap) 그딴거 없게 해버림(시도해보자)
                                double fSlope = (double)(ea[i].timeLines1m.arrTimeLine[nRightPick2].nLastFs - ea[i].timeLines1m.arrTimeLine[nLeftPick1].nLastFs) / GetBetweenMinAndMax(ea[i].timeLines1m.arrTimeLine[nRightPick2].nTimeIdx - ea[i].timeLines1m.arrTimeLine[nLeftPick1].nTimeIdx, MIN_DENOM, MAX_DENOM);
                                fSlope /= GetSlopeGap(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs);

                                double fRecentSlope = (double)(ea[i].timeLines1m.arrTimeLine[nRightPick4].nLastFs - ea[i].timeLines1m.arrTimeLine[nLeftPick3].nLastFs) / GetBetweenMinAndMax(ea[i].timeLines1m.arrTimeLine[nRightPick4].nTimeIdx - ea[i].timeLines1m.arrTimeLine[nLeftPick3].nTimeIdx, MIN_DENOM, MAX_DENOM);
                                fRecentSlope /= GetSlopeGap(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs);

                                double fHourSlope = (double)(ea[i].timeLines1m.arrTimeLine[nRightPick6].nLastFs - ea[i].timeLines1m.arrTimeLine[nLeftPick5].nLastFs) / GetBetweenMinAndMax(ea[i].timeLines1m.arrTimeLine[nRightPick6].nTimeIdx - ea[i].timeLines1m.arrTimeLine[nLeftPick5].nTimeIdx, MIN_DENOM, MAX_DENOM);
                                fHourSlope /= GetSlopeGap(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs);

                                arrFSlope[j] = fSlope;
                                arrRecentFSlope[j] = fRecentSlope;
                                arrHourFSlope[j] = fHourSlope;
                            }

                            // 초기기울기
                            ea[i].timeLines1m.fInitSlope = (double)(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[i].nTodayStartPrice) / COMMON_DENOM;
                            ea[i].timeLines1m.fInitSlope /= GetSlopeGap(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs);
                            // 최대값 기울기
                            ea[i].timeLines1m.fMaxSlope = (double)(ea[i].timeLines1m.nMaxUpFs - ea[i].nTodayStartPrice) / COMMON_DENOM;
                            ea[i].timeLines1m.fMaxSlope /= GetSlopeGap(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs);

                            // arrFSlope 모두 채움
                            Array.Sort(arrFSlope, (x, y) => y.CompareTo(x)); // 아마 내림차순
                            ea[i].timeLines1m.fTotalMedian = (arrFSlope[PICKUP_CNT / 2 - 1] + arrFSlope[PICKUP_CNT / 2]) / 2; // 중위수 구함
                            if (isEqualBetweenDouble(ea[i].timeLines1m.fTotalMedian, 0))
                                ea[i].timeLines1m.fTotalMedian = arrFSlope.Sum() / PICKUP_CNT;

                            // arrRecentFSlope 모두 채움
                            Array.Sort(arrRecentFSlope, (x, y) => y.CompareTo(x)); // 아마 내림차순
                            ea[i].timeLines1m.fRecentMedian = (arrRecentFSlope[PICKUP_CNT / 2 - 1] + arrRecentFSlope[PICKUP_CNT / 2]) / 2; // 중위수 구함
                            if (isEqualBetweenDouble(ea[i].timeLines1m.fRecentMedian, 0))
                                ea[i].timeLines1m.fRecentMedian = arrRecentFSlope.Sum() / PICKUP_CNT;

                            // arrHourFSlope 모두 채움
                            Array.Sort(arrHourFSlope, (x, y) => y.CompareTo(x)); // 아마 내림차순
                            ea[i].timeLines1m.fHourMedian = (arrHourFSlope[PICKUP_CNT / 2 - 1] + arrHourFSlope[PICKUP_CNT / 2]) / 2; // 중위수 구함
                            if (isEqualBetweenDouble(ea[i].timeLines1m.fHourMedian, 0))
                                ea[i].timeLines1m.fHourMedian = arrHourFSlope.Sum() / PICKUP_CNT;

                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fInitAngle = ea[i].timeLines1m.fInitAngle = GetAngleBetween(0, ea[i].timeLines1m.fInitSlope);
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fMaxAngle = ea[i].timeLines1m.fMaxAngle = GetAngleBetween(0, ea[i].timeLines1m.fMaxSlope);
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fDAngle = ea[i].timeLines1m.fDAngle = GetAngleBetween(ea[i].timeLines1m.fMaxSlope - ea[i].timeLines1m.fInitSlope, 0);
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fMedianAngle = ea[i].timeLines1m.fTotalMedianAngle = GetAngleBetween(0, ea[i].timeLines1m.fTotalMedian);
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fRecentAngle = ea[i].timeLines1m.fRecentMedianAngle = GetAngleBetween(0, ea[i].timeLines1m.fRecentMedian);
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fHourAngle = ea[i].timeLines1m.fHourMedianAngle = GetAngleBetween(0, ea[i].timeLines1m.fHourMedian);
                        } // END---- 추세
                        #endregion

                        #region 이동평균선 계산
                        // ===========================================================================================================
                        // 이평선 Part
                        // ===========================================================================================================
                        {
                            int nShareIdx, nSummation;
                            double fMaVal;

                            // -----------
                            // DownFs 이평선
                            ea[i].maOverN.fCurDownFs = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownFs;
                            if (ea[i].maOverN.fMaxDownFs < ea[i].maOverN.fCurDownFs)
                            {
                                ea[i].maOverN.fMaxDownFs = ea[i].maOverN.fCurDownFs;
                                ea[i].maOverN.nMaxDownFsTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                            }

                            // -----------
                            // 20분 이평선
                            nShareIdx = MA20M - nTimeLineIdx - 1;
                            nSummation = 0;
                            if (nShareIdx <= 0) // 0보다 작다면 더미데이터가 아니라 전부 리얼데이터로 채울 수 있다는 의미
                            {
                                for (j = nTimeLineIdx; j > nTimeLineIdx - MA20M; j--)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                            }
                            else // 부족하다는 의미
                            {
                                for (j = 0; j <= nTimeLineIdx; j++)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                                for (j = 0; j < nShareIdx; j++)
                                {
                                    nSummation += ea[i].nTodayStartPrice; // 0~ BRUSH -1 까지는 다 같지만 그냥 0번째 데이터를 넣어줌
                                }
                            }
                            fMaVal = (double)nSummation / MA20M; // 현재의 N이동평균선 값
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa0 = ea[i].maOverN.fCurMa20m = fMaVal;

                            if (ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownFs > fMaVal) // 현재의 저가보다 이평선이 아래에 있다면
                            {
                                ea[i].maOverN.nDownCntMa20m++; // 아래가 좋은거임
                                if (ea[i].maOverN.nUpCntMa20m > MA_EXCEED_CNT)// 오래동안 위였다가 가격이 ma를 뚫고 올라간다는 말
                                {
                                    // TODO.
                                }
                                ea[i].maOverN.nUpCntMa20m = 0;
                            }
                            else
                            {
                                ea[i].maOverN.nUpCntMa20m++;
                                if (ea[i].maOverN.nDownCntMa20m > MA_EXCEED_CNT) // 오랫동안 아래였다가 가격이 ma를 뚫고 내려갔다는 말
                                {
                                    // TODO.
                                }
                                ea[i].maOverN.nDownCntMa20m = 0;
                            }

                            if (ea[i].maOverN.fMaxMa20m < fMaVal)
                            {
                                ea[i].maOverN.fMaxMa20m = fMaVal;
                                ea[i].maOverN.nMaxMa20mTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                            }
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownTimeOverMa0 = ea[i].maOverN.nDownCntMa20m;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpTimeOverMa0 = ea[i].maOverN.nUpCntMa20m;


                            // -----------
                            // 60분 이평선
                            nShareIdx = MA1H - nTimeLineIdx - 1;
                            nSummation = 0;
                            if (nShareIdx <= 0) // 0보다 작다면 더미데이터가 아니라 전부 리얼데이터로 채울 수 있다는 의미
                            {
                                for (j = nTimeLineIdx; j > nTimeLineIdx - MA1H; j--)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                            }
                            else // 부족하다는 의미
                            {
                                for (j = 0; j <= nTimeLineIdx; j++)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                                for (j = 0; j < nShareIdx; j++)
                                {
                                    nSummation += ea[i].nTodayStartPrice; // 0~ BRUSH -1 까지는 다 같지만 그냥 0번째 데이터를 넣어줌
                                }
                            }
                            fMaVal = (double)nSummation / MA1H; // 현재의 N이동평균선 값
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa1 = ea[i].maOverN.fCurMa1h = fMaVal;

                            if (ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownFs > fMaVal) // 현재의 저가보다 이평선이 아래에 있다면
                            {
                                ea[i].maOverN.nDownCntMa1h++; // 아래가 좋은거임
                                if (ea[i].maOverN.nUpCntMa1h > MA_EXCEED_CNT)// 오래동안 위였다가 가격이 ma를 뚫고 올라간다는 말
                                {
                                    // TODO.
                                }
                                ea[i].maOverN.nUpCntMa1h = 0;
                            }
                            else
                            {
                                ea[i].maOverN.nUpCntMa1h++;
                                if (ea[i].maOverN.nDownCntMa1h > MA_EXCEED_CNT) // 오랫동안 아래였다가 가격이 ma를 뚫고 내려갔다는 말
                                {
                                    // TODO.
                                }
                                ea[i].maOverN.nDownCntMa1h = 0;
                            }

                            if (ea[i].maOverN.fMaxMa1h < fMaVal)
                            {
                                ea[i].maOverN.fMaxMa1h = fMaVal;
                                ea[i].maOverN.nMaxMa1hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                            }
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownTimeOverMa1 = ea[i].maOverN.nDownCntMa1h;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpTimeOverMa1 = ea[i].maOverN.nUpCntMa1h;



                            // -----------
                            // 120분 이평선
                            nShareIdx = MA2H - nTimeLineIdx - 1;
                            nSummation = 0;
                            if (nShareIdx <= 0) // 0보다 작다면 더미데이터가 아니라 전부 리얼데이터로 채울 수 있다는 의미
                            {
                                for (j = nTimeLineIdx; j > nTimeLineIdx - MA2H; j--)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                            }
                            else // 부족하다는 의미
                            {
                                for (j = 0; j <= nTimeLineIdx; j++)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                                for (j = 0; j < nShareIdx; j++)
                                {
                                    nSummation += ea[i].nTodayStartPrice; // 0~ BRUSH -1 까지는 다 같지만 그냥 0번째 데이터를 넣어줌
                                }
                            }
                            fMaVal = (double)nSummation / MA2H; // 현재의 N이동평균선 값
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa2 = ea[i].maOverN.fCurMa2h = fMaVal;

                            if (ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownFs > fMaVal) // 현재의 저가보다 이평선이 아래에 있다면
                            {
                                ea[i].maOverN.nDownCntMa2h++; // 아래가 좋은거임
                                if (ea[i].maOverN.nUpCntMa2h > MA_EXCEED_CNT)// 오래동안 위였다가 가격이 ma를 뚫고 올라간다는 말
                                {
                                    // TODO.
                                }
                                ea[i].maOverN.nUpCntMa2h = 0;
                            }
                            else
                            {
                                ea[i].maOverN.nUpCntMa2h++;
                                if (ea[i].maOverN.nDownCntMa2h > MA_EXCEED_CNT) // 오랫동안 아래였다가 가격이 ma를 뚫고 내려갔다는 말
                                {
                                    // TODO.
                                }
                                ea[i].maOverN.nDownCntMa2h = 0;
                            }

                            if (ea[i].maOverN.fMaxMa2h < fMaVal)
                            {
                                ea[i].maOverN.fMaxMa2h = fMaVal;
                                ea[i].maOverN.nMaxMa2hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                            }
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownTimeOverMa2 = ea[i].maOverN.nDownCntMa2h;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpTimeOverMa2 = ea[i].maOverN.nUpCntMa2h;


                        }// END ---- 이평선
                        #endregion

                        #region 전고점 계산
                        // ===========================================================================================================
                        // 전고점 Part
                        // ===========================================================================================================
                        {
                            if (ea[i].crushMinuteManager.nCrushMaxPrice < ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs) // 최근기록된 maxPrice보다 현재 종가가 더 높다면
                            {
                                // ----------------------------------------
                                // 전고점 조건 
                                if ((double)(ea[i].crushMinuteManager.nCrushMaxPrice - ea[i].crushMinuteManager.nCrushMinPrice) / ea[i].nYesterdayEndPrice > 0.01 && // 우선 종가대비 전고점과 전저점의 폭이 1퍼센트가 넘어야하고
                                    ea[i].crushMinuteManager.nCrushMinTime > ea[i].crushMinuteManager.nCrushMaxTime) // minTime은 maxTime보다 이후여야한다. minTime == maxTime일 가능성이 있기 때문에.
                                {
                                    Crush tmpCrush;
                                    tmpCrush.nCnt = ea[i].crushMinuteManager.nCurCnt++;
                                    tmpCrush.fMaxMinPower = (double)(ea[i].crushMinuteManager.nCrushMaxPrice - ea[i].crushMinuteManager.nCrushMinPrice) / ea[i].nYesterdayEndPrice;
                                    tmpCrush.fCurMinPower = (double)(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[i].crushMinuteManager.nCrushMinPrice) / ea[i].nYesterdayEndPrice;
                                    tmpCrush.nMaxMinTime = SubTimeToTime(ea[i].crushMinuteManager.nCrushMinTime, ea[i].crushMinuteManager.nCrushMaxTime);
                                    tmpCrush.nMaxCurTime = SubTimeToTime(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime, ea[i].crushMinuteManager.nCrushMaxTime);
                                    tmpCrush.nMinCurTime = SubTimeToTime(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime, ea[i].crushMinuteManager.nCrushMinTime);
                                    tmpCrush.nMinPrice = ea[i].crushMinuteManager.nCrushMinPrice;
                                    tmpCrush.nMaxPrice = ea[i].crushMinuteManager.nCrushMaxPrice;
                                    if (tmpCrush.nCnt == 0)
                                    {
                                        tmpCrush.fUpperNow = (double)(ea[i].crushMinuteManager.nCrushMinPrice - ea[i].crushMinuteManager.nCrushOnlyMinPrice) / ea[i].nYesterdayEndPrice;
                                    }
                                    else
                                    {
                                        tmpCrush.fUpperNow = (double)(ea[i].crushMinuteManager.nCrushMinPrice - ea[i].crushMinuteManager.crushList[tmpCrush.nCnt - 1].nMinPrice) / ea[i].nYesterdayEndPrice;
                                    }

                                    ea[i].crushMinuteManager.crushList.Add(tmpCrush);
                                }

                                ea[i].crushMinuteManager.nCrushMaxPrice = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs;
                                ea[i].crushMinuteManager.nCrushMaxTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                // 전고점에서 minTime은 항상 maxTime보다 높아야하니까 max가 앞서갈때는 minTime을 같이 세팅해준다.
                                ea[i].crushMinuteManager.nCrushMinPrice = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs;
                                ea[i].crushMinuteManager.nCrushMinTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                            }

                            if (ea[i].crushMinuteManager.nCrushMinPrice > ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs)    // 최근 기록된 minPrice보다 현재종가가 낮다면
                            {
                                ea[i].crushMinuteManager.nCrushMinPrice = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs;
                                ea[i].crushMinuteManager.nCrushMinTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                            }
                            if (ea[i].crushMinuteManager.nCrushOnlyMinPrice > ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs) // 최근 기록된 only minPrice보다 현재종가가 낮다면
                            {
                                ea[i].crushMinuteManager.nCrushOnlyMinPrice = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs;
                                ea[i].crushMinuteManager.nCrushOnlyMinTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                            }

                            if (ea[i].crushMinuteManager.nCurCnt != ea[i].crushMinuteManager.nPrevCrushCnt) // 전고점카운트가 오를때마다
                            {
                                int nCrushUpperCnt = 0;
                                int nBadPoint = 0;
                                ea[i].crushMinuteManager.isCrushCheck = true;
                                ea[i].crushMinuteManager.nUpCnt = 0;
                                ea[i].crushMinuteManager.nDownCnt = 0;
                                ea[i].crushMinuteManager.nSpecialDownCnt = 0;

                                for (j = 0; j < ea[i].crushMinuteManager.crushList.Count; j++)
                                {
                                    if (ea[i].crushMinuteManager.crushList[j].fUpperNow > 0) // 올랐다
                                    {
                                        nCrushUpperCnt++;
                                        ea[i].crushMinuteManager.nUpCnt++;
                                    }
                                    else
                                    {
                                        if (j >= ea[i].crushMinuteManager.crushList.Count - EYES_CLOSE_CRUSH_NUM)
                                        {
                                            if (j == ea[i].crushMinuteManager.crushList.Count - 1) // 바로 이전에 전고점에서 하락이었으면 badcount 하나더 플러스
                                            {
                                                nBadPoint++;
                                                ea[i].crushMinuteManager.nSpecialDownCnt++;
                                            }
                                            ea[i].crushMinuteManager.nDownCnt++;
                                            nBadPoint++;
                                        }
                                    }
                                }
                                ea[i].crushMinuteManager.nPrevCrushCnt = ea[i].crushMinuteManager.nCurCnt;
                            } // END --- 전고점 카운트가 오를때마다
                        } // END ---- 전고점 
                        #endregion

                        #region Sequence Strategy
                        { // START ---- 분봉 Sequence Strategy 분기문

                            double fCurPower = (double)(ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;
                            if (fCurPower >= 0.05)
                                ea[i].sequenceStrategy.isFiveReachedMinute = true; // 분봉상으로 갭제외 5퍼를 도달했나

                            ea[i].sequenceStrategy.botUpMinute421.Trace(fCurPower, nSharedTime);
                            ea[i].sequenceStrategy.botUpMinute432.Trace(fCurPower, nSharedTime);
                            ea[i].sequenceStrategy.botUpMinute642.Trace(fCurPower, nSharedTime);
                            ea[i].sequenceStrategy.botUpMinute643.Trace(fCurPower, nSharedTime);
                            ea[i].sequenceStrategy.botUpMinute732.Trace(fCurPower, nSharedTime);
                            ea[i].sequenceStrategy.botUpMinute743.Trace(fCurPower, nSharedTime);
                            ea[i].sequenceStrategy.botUpMinute953.Trace(fCurPower, nSharedTime);


                            #region RealTime Sequence Strategy 초기화
                            {
                                ea[i].sequenceStrategy.isCandleTwoOverReal = false;
                                ea[i].sequenceStrategy.isCandleTwoOverRealNoLeaf = false;
                                ea[i].sequenceStrategy.nResistUpCount = 0;
                            }
                            #endregion
                        } // END ---- 분봉 Sequence Strategy 분기문
                        #endregion

                    }// END ---- 개인구조체 업데이트
                    #endregion

                    // 시간 업데이트
                    nPrevBoardUpdateTime = AddTimeBySec(nPrevBoardUpdateTime, MINUTE_SEC);
                    nTimeLineIdx++;
                }
                #endregion

                #region AI 서비스 창구
#if AI
                // AI 서비스를 요청했으면 
                // 응답 기다리기
                int nAISlotsNum = Min(aiQueue.Count, AI_ONCE_MAXNUM);
                mmf.FetchTargets();
                for (int i = 0; i < nAISlotsNum; i++) // 한바퀴 돈다
                {
                    aiSlot = aiQueue.Dequeue();

                    bool isAIComed = mmf.checkingComeArray[aiSlot.nMMFNumber]; // 응답 여부

                    if (!isAIComed) // 응답이 오지 않았다면
                    {
                        aiQueue.Enqueue(aiSlot);
                        continue;
                    }
                        

                    bool isAIPassed = mmf.checkingBookArray[aiSlot.nMMFNumber]; // 정답확인
                    double fRatio = mmf.checkingRatioArray[aiSlot.nMMFNumber]; // 비율확인

                    switch (aiSlot.nRequestId)
                    {
                        case REAL_BUY_SIGNAL: // 매수 여부 체크

                            if (fRatio >= 0.65)
                            {
                                ea[aiSlot.nEaIdx].myStrategy.nAIPassed++;

                                if (ea[aiSlot.nEaIdx].myStrategy.nAIPrevTimeLineIdx != nTimeLineIdx)
                                {
                                    if (nTimeLineIdx - ea[aiSlot.nEaIdx].myStrategy.nAIPrevTimeLineIdx > 1) //  2칸 이상 떨어져있을때 
                                        ea[aiSlot.nEaIdx].myStrategy.nAIJumpDiffMinuteCount++;
                                    ea[aiSlot.nEaIdx].myStrategy.nAIPrevTimeLineIdx = nTimeLineIdx;
                                    ea[aiSlot.nEaIdx].myStrategy.nAIStepMinuteCount++;
                                }

                                // 예약 생성
                                //ea[aiSlot.slot.nEaIdx].reserveMgr.Approach(nSharedTime, ea[aiSlot.slot.nEaIdx].fPower);
                                //ea[aiSlot.slot.nEaIdx].reserveMgr.listReservation.Add(new ReservedPoint
                                //{
                                //    isReserveEnd = false,
                                //    fReservePower = ea[aiSlot.slot.nEaIdx].fPower,
                                //    nReserveTime = nSharedTime,
                                //    fMaxPower = ea[aiSlot.slot.nEaIdx].fPower,
                                //    fMinusPower = 0.025,
                                //    nTargetLimitTime = AddTimeBySec(nSharedTime, 600)
                                //});
                                //PrintLog($"{nSharedTime}  {ea[aiSlot.slot.nEaIdx].sCode}  {ea[aiSlot.slot.nEaIdx].sCodeName} fs : {ea[aiSlot.slot.nEaIdx].nFs} power : {ea[aiSlot.slot.nEaIdx].fPower} {AddTimeBySec(nSharedTime, 600)}시간 까지 대략 {ea[aiSlot.slot.nEaIdx].nYesterdayEndPrice * (1 + ea[aiSlot.slot.nEaIdx].fPower - 0.025)}원, {ea[aiSlot.slot.nEaIdx].fPower - 0.025}(%) 예약매수", aiSlot.slot.nEaIdx);

                                //ea[aiSlot.slot.nEaIdx].reserveMgr.listReservation.Add(new ReservedPoint
                                //{
                                //    isReserveEnd = false,
                                //    fReservePower = ea[aiSlot.slot.nEaIdx].fPower,
                                //    nReserveTime = nSharedTime,
                                //    fMaxPower = ea[aiSlot.slot.nEaIdx].fPower,
                                //    fMinusPower =  0.05,
                                //    nTargetLimitTime = AddTimeBySec(nSharedTime, 5400)
                                //});
                                //PrintLog($"{nSharedTime}  {ea[aiSlot.slot.nEaIdx].sCode}  {ea[aiSlot.slot.nEaIdx].sCodeName} fs : {ea[aiSlot.slot.nEaIdx].nFs} power : {ea[aiSlot.slot.nEaIdx].fPower} {AddTimeBySec(nSharedTime, 5400)}시간 까지 대략 {ea[aiSlot.slot.nEaIdx].nYesterdayEndPrice * (1 + ea[aiSlot.slot.nEaIdx].fPower - 0.05)}원, {ea[aiSlot.slot.nEaIdx].fPower - 0.05}(%) 예약매수", aiSlot.slot.nEaIdx);

                                //ea[aiSlot.slot.nEaIdx].reserveMgr.listReservation.Add(new ReservedPoint
                                //{
                                //    isReserveEnd = false,
                                //    fReservePower = ea[aiSlot.slot.nEaIdx].fPower,
                                //    nReserveTime = nSharedTime,
                                //    fMaxPower = ea[aiSlot.slot.nEaIdx].fPower,
                                //    fMinusPower = 0.1,
                                //    nTargetLimitTime = AddTimeBySec(nSharedTime, 5400)
                                //});
                                //PrintLog($"{nSharedTime}  {ea[aiSlot.slot.nEaIdx].sCode}  {ea[aiSlot.slot.nEaIdx].sCodeName} fs : {ea[aiSlot.slot.nEaIdx].nFs} power : {ea[aiSlot.slot.nEaIdx].fPower} {AddTimeBySec(nSharedTime, 5400)}시간 까지 대략 {ea[aiSlot.slot.nEaIdx].nYesterdayEndPrice * (1 + ea[aiSlot.slot.nEaIdx].fPower - 0.1)}원, {ea[aiSlot.slot.nEaIdx].fPower - 0.1}(%) 예약매수", aiSlot.slot.nEaIdx);

                                //tradeQueue.Enqueue(aiSlot.slot);
                            }

                            PrintLog($"{nSharedTime}  {ea[aiSlot.nEaIdx].sCode}  {ea[aiSlot.nEaIdx].sCodeName} fs : {ea[aiSlot.nEaIdx].nFs}  #AI 실제 통과여부 : {isAIPassed} 가능성 : {Math.Round(fRatio, 3)} 실매수 시도 : {ea[aiSlot.nEaIdx].myStrategy.nStrategyNum} 실매수 누적 : {ea[aiSlot.nEaIdx].myStrategy.nAIPassed}  실매수 스텝 : {ea[aiSlot.nEaIdx].myStrategy.nAIStepMinuteCount}  실매수 점프 : {ea[aiSlot.nEaIdx].myStrategy.nAIJumpDiffMinuteCount}", aiSlot.nEaIdx);
                            break;
                        case REAL_SELL_SIGNAL: // 분할매도 여부 체크(분할매도를 위해 AI 서비스를 이용한다)
                            if (isAIPassed)
                            {
                            }
                            break;
                        case FAKE_REQUEST_SIGNAL:
                            if (fRatio > 0.45)
                            {
                                ea[aiSlot.nEaIdx].myStrategy.nFakeAccumPassed++;
                                if (ea[aiSlot.nEaIdx].myStrategy.nFakeAIPrevTimeLineIdx != nTimeLineIdx)
                                {
                                    if (nTimeLineIdx - ea[aiSlot.nEaIdx].myStrategy.nFakeAIPrevTimeLineIdx > 1)
                                        ea[aiSlot.nEaIdx].myStrategy.nFakeAIJumpDiffMinuteCount++;
                                    ea[aiSlot.nEaIdx].myStrategy.nFakeAIPrevTimeLineIdx = nTimeLineIdx;
                                    ea[aiSlot.nEaIdx].myStrategy.nFakeAIStepMinuteCount++;
                                }
                            }

                            ea[aiSlot.nEaIdx].myStrategy.nFakeAccumTried++;
                            
                            PrintLog($"{nSharedTime}  {ea[aiSlot.nEaIdx].sCode}  {ea[aiSlot.nEaIdx].sCodeName} fs : {ea[aiSlot.nEaIdx].nFs}  #AI 페이크 통과여부 : {isAIPassed} 가능성 : {Math.Round(fRatio,3)}  페이크 시도 : { ea[aiSlot.nEaIdx].myStrategy.nFakeAccumTried}  페이크 누적 : {ea[aiSlot.nEaIdx].myStrategy.nFakeAccumPassed} 페이크 스텝 : {ea[aiSlot.nEaIdx].myStrategy.nFakeAIStepMinuteCount} 페이크 점프 : {ea[aiSlot.nEaIdx].myStrategy.nFakeAIJumpDiffMinuteCount}", aiSlot.nEaIdx);
                            break;
                        default:
                            break;
                    }
                    TurnOffMMFSlot(aiSlot.nMMFNumber);
                }
#endif
                #endregion


                #region 실시간 호가 
                // =============================================================
                // 실시간 호가잔량
                // =============================================================
                if (isHogaJanRyang) // ##호가잔량##
                {

                    int b4;
                    int s1 = 0, s2 = 0, s3 = 0, s4;
                    int nTotalBuyHoga = 0;
                    int nTotalSellHoga = 0;
                    bool isHogaError = false;
                    bool isSubHogaError = false;

                    ea[nCurIdx].nHogaCnt++; // 호가의 카운트
                    ea[nCurIdx].hogaSpeedStatus.fPush++;

                    try
                    {
                        s1 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 61))); // 매도1호가잔량
                        s2 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 62))); // 매도2호가잔량
                        s3 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 63))); // 매도3호가잔량

                        s4 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 64))); // 매도4호가잔량
                        b4 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 74))); // 매수4호가잔량

                        if (s4 == 0 && b4 == 0 && ea[nCurIdx].isFirstCheck)
                        {
                            if (!ea[nCurIdx].isViMode)
                            {
                                ea[nCurIdx].nViStartTime = nSharedTime;
                                ea[nCurIdx].nViTimeLineIdx = nTimeLineIdx;
                                ea[nCurIdx].nViFs = ea[nCurIdx].nFs;
                            }

                            ea[nCurIdx].nPrevSpeedUpdateTime = nSharedTime;
                            ea[nCurIdx].nPrevPowerUpdateTime = nSharedTime;
                            ea[nCurIdx].isViMode = true;

                        }
                        else
                        {
                            if (ea[nCurIdx].isViMode)
                            {
                                ea[nCurIdx].isViMode = false;
                                ea[nCurIdx].isViGauge = true;
                                ea[nCurIdx].nViEndTime = nSharedTime;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isSubHogaError = true;
                    }


                    if (!ea[nCurIdx].isViMode)
                    {
                        try
                        {
                            nTotalBuyHoga = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 125)));  // 매수호가총잔량
                        }
                        catch
                        {
                            isHogaError = true;
                        }

                        try
                        {
                            nTotalSellHoga = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 121)));  // 매도호가총잔량
                        }
                        catch
                        {
                            isHogaError = true;
                        }


                        if (!isHogaError)
                        {
                            if (!isSubHogaError)
                            {
                                ea[nCurIdx].nThreeSellHogaVolume = s1 + s2 + s3; //  매도1~3 호가잔량합
                            }
                            ea[nCurIdx].nTotalBuyHogaVolume = nTotalBuyHoga;
                            ea[nCurIdx].nTotalSellHogaVolume = nTotalSellHoga;
                            ea[nCurIdx].nTotalHogaVolume = ea[nCurIdx].nTotalBuyHogaVolume + ea[nCurIdx].nTotalSellHogaVolume;
                            ea[nCurIdx].fHogaRatio = (double)(ea[nCurIdx].nTotalSellHogaVolume - ea[nCurIdx].nTotalBuyHogaVolume) / (ea[nCurIdx].nTotalHogaVolume + 1); // -1 ~ +1 ( 매도가 많으면 ) 

                            if (ea[nCurIdx].nPrevHogaUpdateTime == 0)
                            {
                                ea[nCurIdx].nPrevHogaUpdateTime = nFirstTime;
                            }
                            int nSubTimeShort = SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].nPrevHogaUpdateTime);
                            int nHogaUpdatesShort = nSubTimeShort / SHORT_UPDATE_TIME;
                            for (int idxUpdate = 0; idxUpdate < nHogaUpdatesShort; idxUpdate++)
                            {
                                ea[nCurIdx].totalHogaVolumeStatus.Commit(fPushWeight);
                                ea[nCurIdx].hogaRatioStatus.Commit(fPushWeight);
                                ea[nCurIdx].hogaSpeedStatus.Commit(fPushWeight);

                                ea[nCurIdx].nPrevHogaUpdateTime = nSharedTime;
                            }

                            ea[nCurIdx].totalHogaVolumeStatus.Push(ea[nCurIdx].nTotalHogaVolume, fPushWeight);
                            ea[nCurIdx].hogaRatioStatus.Push(ea[nCurIdx].fHogaRatio, fPushWeight);


                            int nTimeHogaPassedShort = SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].nPrevHogaUpdateTime); // 시간이 지났다!
                            double fTimeHogaPassedWeightShort = (double)nTimeHogaPassedShort / SHORT_UPDATE_TIME; // 시간이 얼만큼 지났느냐 0 ~ ( nUpdateTime -1) /nUpdateTime

                            ea[nCurIdx].hogaSpeedStatus.Update(fTimeHogaPassedWeightShort);
                            ea[nCurIdx].hogaRatioStatus.Update(fTimeHogaPassedWeightShort);
                            ea[nCurIdx].totalHogaVolumeStatus.Update(fTimeHogaPassedWeightShort);

                        }
                    }

                }
                #endregion
                #region  실시간 주식체결
                // =============================================================
                // 실시간 주식체결
                // =============================================================
                else if (isZooSikCheGyul) // ##주식체결##
                {

                    int nTimeBetweenPrev = 0; // 해당종목의 마지막접근시간과 현재시간의 차이

                    //if(ea[nCurIdx].nLastRecordTime == 0) // 첫 접근은 어찌 처리할거냐??
                    //{
                    //    nTimeBetweenPrev = 0;
                    //}
                    //else
                    //{
                    //    nTimeBetweenPrev = SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].nLastRecordTime);;;
                    //}
                    // or 
                    nTimeBetweenPrev = SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].nLastRecordTime); // 첫접근시 값(현재와의 시간간격)이 높게 설정돼있음.

                    ea[nCurIdx].nLastRecordTime = nSharedTime;

                    try
                    {
                        int nFs = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 27))); // 최우선매도호가
                        int nFb = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 28))); // 최우선매수호가
                        int nTv = int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 15)); // 거래량
                        double fTs = double.Parse(axKHOpenAPI1.GetCommRealData(sCode, 228)); // 체결강도
                        double fPower = double.Parse(axKHOpenAPI1.GetCommRealData(sCode, 12)) / 100; // 등락율

                        ea[nCurIdx].nFs = nFs;
                        ea[nCurIdx].nFb = nFb;
                        ea[nCurIdx].nTv = nTv;
                        ea[nCurIdx].fTs = fTs;
                        ea[nCurIdx].fPower = fPower;
                    }
                    catch
                    {
                        return;
                    }

                    #region 최우선 매수호가 & 매도호가 세팅
                    if (ea[nCurIdx].nFs == 0 && ea[nCurIdx].nFb == 0)  // 둘 다 데이터가 없는경우는 가격초기화가 불가능하기 return
                        return;
                    else
                    {
                        // 둘다 제대로 받아졌거나 , 둘 중 하나가 안받아졌거나
                        if (ea[nCurIdx].nFs == 0) // fs가 안받아졌으면 fb 가격에 fb갭 한칸을 더해서 설정
                        {
                            ea[nCurIdx].nFs = ea[nCurIdx].nFb + GetIntegratedMarketGap(ea[nCurIdx].nFb);
                        }
                        if (ea[nCurIdx].nFb == 0) // fb가 안받아졌으면 fs 가격에 (fs-1)갭 한칸을 마이너스해서 설정
                        {
                            // fs-1 인 이유는 fs가 1000원이라하면 fb는 999여야하는데 갭을 받을때 5를 받게되니 fb가 995가 되어버린다.이는 오류!
                            ea[nCurIdx].nFb = ea[nCurIdx].nFs - GetIntegratedMarketGap(ea[nCurIdx].nFs - 1);
                        }

                    }
                    #endregion

                    ea[nCurIdx].fDiff = (ea[nCurIdx].nFs - ea[nCurIdx].nFb) / GetIntegratedMarketGap(ea[nCurIdx].nFs);

                    // 이상 데이터 감지
                    // fs와 fb의 가격차이가 2퍼가 넘을경우 이상데이터라 생각하고 리턴한다.
                    // 미리 리턴하는 이유는 이런 이상 데이터로는 전략에 사용하지 않기위해서 전략찾는 부분 위에서 리턴여부를 검증한다.
                    // if ((ea[nCurIdx].nFs - ea[nCurIdx].nFb) / ea[nCurIdx].nFb > 0.02)
                    //    return;

                    #region 초기 세팅
                    // 처음가격과 시간등을 맞추려는 변수이다.
                    if (!ea[nCurIdx].isFirstCheck) // 개인 초기작업
                    {

                        //if (ea[nCurIdx].nFs < 1000) // 1000원도 안한다면 폐기처분
                        //{
                        //    axKHOpenAPI1.SetRealRemove(ea[nCurIdx].sRealScreenNum, ea[nCurIdx].sCode);
                        //    ea[nCurIdx].isExcluded = true;
                        //}

                        ea[nCurIdx].isFirstCheck = true; // 가격설정이 끝났으면 이종목의 초기체크는 완료 설정
                        int nStartGap = int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 11)); // 어제종가와 비교한 가격변화

                        ea[nCurIdx].nStartGap = nStartGap; // 갭

                        if (ea[nCurIdx].nYesterdayEndPrice == 0)
                        {
                            ea[nCurIdx].nYesterdayEndPrice = ea[nCurIdx].nFs - nStartGap; // 시초가에서 변화를 제거하면 어제 종가가 나옴
                            ea[nCurIdx].nTodayStartPrice = ea[nCurIdx].nFs; // 오늘 시초가
                        }
                        else
                        {
                            int nTodayPriceErrorCheck = ea[nCurIdx].nFs - ea[nCurIdx].nStartGap;
                            double fAcceptErrorRange = 0.01;
                            if (Math.Abs((double)(ea[nCurIdx].nYesterdayEndPrice - nTodayPriceErrorCheck) / nTodayPriceErrorCheck) > fAcceptErrorRange) // 어제종가가 기준이 돼야하는데 오늘 아침 갑자기 가격이 훅 바뀌는 경우가 있음
                            {
                                ea[nCurIdx].nYesterdayEndPrice = nTodayPriceErrorCheck;
                            }
                            ea[nCurIdx].nTodayStartPrice = ea[nCurIdx].nYesterdayEndPrice + ea[nCurIdx].nStartGap;
                        }
                        ea[nCurIdx].fStartGap = (double)ea[nCurIdx].nStartGap / ea[nCurIdx].nYesterdayEndPrice; // 갭의 등락율 or ea[nCurIdx].fPower



                        GenerateFrontLine(lineManager: ref ea[nCurIdx].timeLines1m,
                                            nIter: BRUSH + ea[nCurIdx].nJumpCnt, nBirthTime: nFirstTime,
                                            nBirthPrice: ea[nCurIdx].nTodayStartPrice
                                            );

                        int nCutSharedT = nSharedTime - nSharedTime % MINUTE_KIWOOM;
                        ea[nCurIdx].crushMinuteManager.nCrushMaxPrice = ea[nCurIdx].nTodayStartPrice;
                        ea[nCurIdx].crushMinuteManager.nCrushMaxTime = nCutSharedT;
                        ea[nCurIdx].crushMinuteManager.nCrushMinPrice = ea[nCurIdx].nTodayStartPrice;
                        ea[nCurIdx].crushMinuteManager.nCrushMinTime = nCutSharedT;
                        ea[nCurIdx].crushMinuteManager.nCrushOnlyMinPrice = ea[nCurIdx].nTodayStartPrice;
                        ea[nCurIdx].crushMinuteManager.nCrushOnlyMinTime = nCutSharedT;

                    } // END ---- 개인 초기작업
                    #endregion
                    ea[nCurIdx].nChegyulCnt++; // 인덱스를 올린다.
                    if (ea[nCurIdx].nChegyulCnt == 1)  // 첫데이터는  tv가 너무 높을 수 있느니 패스
                        return;

                    // 맥스값
                    if (ea[nCurIdx].nRealMaxPrice < ea[nCurIdx].nFs)
                        ea[nCurIdx].nRealMaxPrice = ea[nCurIdx].nFs;


                    //#region 예약 처리
                    //// 예약 처리
                    //if (!ea[nCurIdx].isExcluded)
                    //{
                    //    for (int i = 0; i < ea[nCurIdx].reserveMgr.listReservation.Count; i++)
                    //    {
                    //        if (!ea[nCurIdx].reserveMgr.listReservation[i].isReserveEnd)
                    //        {
                    //            ea[nCurIdx].reserveMgr.listReservation[i].fMaxPower = Max(ea[nCurIdx].reserveMgr.listReservation[i].fMaxPower, ea[nCurIdx].fPower);

                    //            if (ea[nCurIdx].reserveMgr.listReservation[i].nTargetLimitTime < nSharedTime) // 제한시간을 넘겼다면
                    //            {
                    //                ea[nCurIdx].reserveMgr.listReservation[i].isReserveEnd = true;
                    //                continue;
                    //            }

                    //            if ((ea[nCurIdx].reserveMgr.listReservation[i].fReservePower - ea[nCurIdx].reserveMgr.listReservation[i].fMinusPower) > ea[nCurIdx].fPower) // 원하는 가격으로 내려왔다면
                    //            {
                    //                ea[nCurIdx].reserveMgr.listReservation[i].isReserveEnd = true;
                    //                RequestThisRealBuy(0, isAIUse: false);
                    //            }
                    //        }
                    //    }
                    //}
                    //#endregion

                    if (ea[nCurIdx].isViMode)
                    {
                        ea[nCurIdx].isViMode = false;
                        ea[nCurIdx].isViGauge = true;
                        ea[nCurIdx].nViEndTime = nSharedTime;
                    }

                    ea[nCurIdx].fPowerWithoutGap = ea[nCurIdx].fPower - ea[nCurIdx].fStartGap;
                    ea[nCurIdx].fPowerDiff = ea[nCurIdx].fPowerWithoutGap - ea[nCurIdx].fPrevPowerWithoutGap;
                    ea[nCurIdx].fPrevPowerWithoutGap = ea[nCurIdx].fPowerWithoutGap;


                    #region 실시간 전고점
                    // 실시간 전고점
                    // #경고 : 분봉에서 전고점 체크할때 때마침 해당되는 종목이 때마침 전고점돌파에 성공했다 그러면 현재 실시간 전고점은 maxTime과 minTime이 업데이트되어 실행되지 않음
                    // # 가능성이 희박할 뿐더러 만약 그런경우라 하더라도 가격이 순식간에 급변하는 종목일 가능성이 크고 그럴경우 잡주일 확률이 높으니 일단 신경 안쓰기로 함.
                    if (ea[nCurIdx].crushMinuteManager.nCrushMaxTime < ea[nCurIdx].crushMinuteManager.nCrushMinTime && // 우선 초기1분을 제외할 수 있고 전고점돌파조건인 maxT < minT 
                        ea[nCurIdx].crushMinuteManager.nCrushMaxPrice < ea[nCurIdx].nFs && // 현재값이 전고점을 넘어섰을때
                        (double)(ea[nCurIdx].crushMinuteManager.nCrushMaxPrice - ea[nCurIdx].crushMinuteManager.nCrushMinPrice) / ea[nCurIdx].nYesterdayEndPrice > 0.01 &&
                        ea[nCurIdx].crushMinuteManager.nCrushRealTimePrev != ea[nCurIdx].crushMinuteManager.nCrushMaxTime // 전고점을 했는데 그게 고점이고 종점은 이전고점보다 낮게 됐을때 해당 고점을 두번쨰 돌파했을때 논리적오류가 발생
                        )
                    {
                        ea[nCurIdx].crushMinuteManager.nCrushRealTimeWidthMaxMin = SubTimeToTime(nSharedTime, ea[nCurIdx].crushMinuteManager.nCrushMaxTime);
                        ea[nCurIdx].crushMinuteManager.nCrushRealTimeWidthMaxCur = SubTimeToTime(ea[nCurIdx].crushMinuteManager.nCrushMinTime, ea[nCurIdx].crushMinuteManager.nCrushMaxTime);
                        ea[nCurIdx].crushMinuteManager.fCrushRealTimeHeight = (double)(ea[nCurIdx].crushMinuteManager.nCrushMaxPrice - ea[nCurIdx].crushMinuteManager.nCrushMinPrice) / ea[nCurIdx].nYesterdayEndPrice;
                        ea[nCurIdx].crushMinuteManager.nCrushRealTimePrev = ea[nCurIdx].crushMinuteManager.nCrushMaxTime;
                        ea[nCurIdx].crushMinuteManager.nCrushRealTimeCount++;
                        ea[nCurIdx].crushMinuteManager.nCrushRealTimeLineIdx = nTimeLineIdx;
                        ea[nCurIdx].crushMinuteManager.isCrushRealTimeCheck = true;
                    } // END ---- 실시간 전고점
                    #endregion

                    #region 순위 선정용 변수 세팅
                    // ==================================================
                    // 거래대금 및 수량 기록, 정리
                    // ==================================================
                    ea[nCurIdx].lTotalTradePrice += Math.Abs(ea[nCurIdx].nFs * ea[nCurIdx].nTv); // 1. 거래대금
                    ea[nCurIdx].lTotalTradeVolume += Math.Abs(ea[nCurIdx].nTv);
                    ea[nCurIdx].fTotalTradeVolume = (double)ea[nCurIdx].lTotalTradeVolume / ea[nCurIdx].lTotalNumOfStock; // 2. 상대적거래수량
                    ea[nCurIdx].lTotalBuyPrice += ea[nCurIdx].nFs * ea[nCurIdx].nTv; // 3. 매수대금
                    ea[nCurIdx].lTotalBuyVolume += ea[nCurIdx].nTv;
                    ea[nCurIdx].fTotalBuyVolume = (double)ea[nCurIdx].lTotalBuyVolume / ea[nCurIdx].lTotalNumOfStock; // 4. 상대적매수수량
                    ea[nCurIdx].lMarketCap = ea[nCurIdx].lTotalNumOfStock * ea[nCurIdx].nFs; // 7. 시가총액
                    ea[nCurIdx].lMinuteTradePrice += ea[nCurIdx].nFs * Math.Abs(ea[nCurIdx].nTv); // 8. 분당 거래대금
                    ea[nCurIdx].lMinuteTradeVolume += Math.Abs(ea[nCurIdx].nTv);
                    ea[nCurIdx].fMinuteTradeVolume = (double)ea[nCurIdx].lMinuteTradeVolume / ea[nCurIdx].lTotalNumOfStock; // 9. 분당 상대적거래수량
                    ea[nCurIdx].lMinuteBuyPrice += ea[nCurIdx].nFs * ea[nCurIdx].nTv; // 10. 분당 매수대금
                    ea[nCurIdx].lMinuteBuyVolume += ea[nCurIdx].nTv;
                    ea[nCurIdx].fMinuteBuyVolume = (double)ea[nCurIdx].lMinuteBuyVolume / ea[nCurIdx].lTotalNumOfStock; // 11. 분당 상대적거래수량
                    ea[nCurIdx].fMinutePower += ea[nCurIdx].fPowerDiff; // 12. 분당 손익율
                    ea[nCurIdx].nMinuteCnt++; // 분당 카운트
                    if (ea[nCurIdx].fPowerDiff != 0)
                    {
                        ea[nCurIdx].nMinuteUpDown++;
                        ea[nCurIdx].nAccumUpDownCount++;
                        if (ea[nCurIdx].fPowerDiff > 0)
                            ea[nCurIdx].fAccumUpPower += ea[nCurIdx].fPowerDiff;
                        else
                            ea[nCurIdx].fAccumDownPower -= ea[nCurIdx].fPowerDiff;
                    }
                    if (ea[nCurIdx].nTv > 0)
                    {
                        ea[nCurIdx].lOnlyBuyPrice += Math.Abs(ea[nCurIdx].nFs * ea[nCurIdx].nTv); // 매수대금
                        ea[nCurIdx].lOnlyBuyVolume += Math.Abs(ea[nCurIdx].nTv);
                    }
                    else
                    {
                        ea[nCurIdx].lOnlySellPrice += Math.Abs(ea[nCurIdx].nFs * ea[nCurIdx].nTv); // 매도대금
                        ea[nCurIdx].lOnlySellVolume += Math.Abs(ea[nCurIdx].nTv);
                    }
                    #endregion

                    #region 타임라인 변수 세팅
                    // =========================================================
                    // 개인구조체 현재 타임값들 기록
                    // =========================================================
                    try // 장끝나면 idx오류
                    {
                        if (ea[nCurIdx].timeLines1m.nPrevTimeLineIdx < ea[nCurIdx].timeLines1m.arrTimeLine.Length)
                        {

                            if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs == 0)
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs = ea[nCurIdx].nFs;

                            ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nLastFs = ea[nCurIdx].nFs;

                            if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs < ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nLastFs)
                            {
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nUpFs = ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nLastFs;
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nDownFs = ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs;
                            }
                            else
                            {
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nUpFs = ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs;
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nDownFs = ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nLastFs;
                            }

                            if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nMaxFs == 0 || ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nMaxFs < ea[nCurIdx].nFs)
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nMaxFs = ea[nCurIdx].nFs;

                            if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nMinFs == 0 || ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nMinFs > ea[nCurIdx].nFs)
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nMinFs = ea[nCurIdx].nFs;

                            ea[nCurIdx].timeLines1m.nFsPointer = ea[nCurIdx].nFs;
                            ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nTotalVolume += Math.Abs(ea[nCurIdx].nTv);
                            ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].lTotalPrice += Math.Abs(ea[nCurIdx].nTv * ea[nCurIdx].nFs);
                            if (ea[nCurIdx].nTv > 0)
                            {
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].lBuyPrice += Math.Abs(ea[nCurIdx].nTv * ea[nCurIdx].nFs);
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nBuyVolume += Math.Abs(ea[nCurIdx].nTv);
                            }
                            else // 체결량이 0인 경우는 없다
                            {
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].lSellPrice += Math.Abs(ea[nCurIdx].nTv * ea[nCurIdx].nFs);
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nSellVolume += Math.Abs(ea[nCurIdx].nTv);
                            }
                            ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nCount++;

                            if (ea[nCurIdx].fPowerDiff > 0)
                            {
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].fAccumUpPower += ea[nCurIdx].fPowerDiff;
                            }
                            else if (ea[nCurIdx].fPowerDiff < 0)
                            {
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].fAccumDownPower -= ea[nCurIdx].fPowerDiff;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region CurStatus 변수 세팅
                    ////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////
                    ///////////// 점수 Part /////////////////////////////////////////////

                    // 일정시간마다 fJar 값을 감소시킨다. 이 일정시간을 어떻게 매길것인 지는 고민해볼 문제
                    // 시간 당 ... 
                    if (ea[nCurIdx].nPrevSpeedUpdateTime == 0)
                    {
                        ea[nCurIdx].nPrevSpeedUpdateTime = nFirstTime;
                    }


                    int nTimeUpdate = SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].nPrevSpeedUpdateTime) / SHORT_UPDATE_TIME;

                    // if 가 아닌 for 문은 지연시간만큼 status값이 주는 패널티가 필요해서
                    for (int idxUpdate = 0; idxUpdate < nTimeUpdate; idxUpdate++)
                    {
                        ea[nCurIdx].speedStatus.Commit(fPushWeight);
                        ea[nCurIdx].tradeStatus.Commit(fPushWeight);
                        ea[nCurIdx].pureTradeStatus.Commit(fPushWeight);
                        ea[nCurIdx].pureBuyStatus.Commit(fPushWeight);

                        ea[nCurIdx].nPrevSpeedUpdateTime = nSharedTime; // 업데이트시간 초기화 


                        // 엄격한 방법의 업데이트시간 초기화
                        // ex) nUpdateTime = 20, nTimeUpdate = 38, nPrevSpeedUpdateTime = 0
                        // nPrevSpeedUpdateTime = 20 다음 update까지 2(40 - 38)초 남았음
                        // ea[nCurIdx].nPrevSpeedUpdateTime = AddTimeBySec(ea[nCurIdx].nPrevSpeedUpdateTime, nUpdateTime);
                    }

                    int nTimePassed = SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].nPrevSpeedUpdateTime); // 지난시간 = 현재시간 - 이전시간 
                    double fTimePassedWeight = (double)nTimePassed / SHORT_UPDATE_TIME; // 업데이트한 지 얼마 안됐을경우 지난시간이 지극히 낮고 nSpeedPush는 0에 가까울 것이다.
                    double fTimePassedPushWeight = fTimePassedWeight * fPushWeight; // fPushWeight에서 지난시간의 크기만큼만 곱해 현재정보(nSpeedPush) 적용

                    ea[nCurIdx].speedStatus.fPush++;
                    ea[nCurIdx].tradeStatus.fPush += Math.Abs(ea[nCurIdx].nTv);
                    ea[nCurIdx].pureTradeStatus.fPush += ea[nCurIdx].nTv;
                    if (ea[nCurIdx].nTv > 0)
                        ea[nCurIdx].pureBuyStatus.fPush += ea[nCurIdx].nTv;

                    ea[nCurIdx].speedStatus.Update(fTimePassedPushWeight);
                    ea[nCurIdx].tradeStatus.Update(fTimePassedPushWeight);
                    ea[nCurIdx].pureTradeStatus.Update(fTimePassedPushWeight);
                    ea[nCurIdx].pureBuyStatus.Update(fTimePassedPushWeight);



                    if (isEqualBetweenDouble(ea[nCurIdx].totalHogaVolumeStatus.fCur, 0))
                    {
                        ea[nCurIdx].fSharePerHoga = SHT_PER_INIT;
                        ea[nCurIdx].fHogaPerTrade = SHT_PER_INIT;
                    }
                    else
                    {
                        ea[nCurIdx].fSharePerHoga = Min(ea[nCurIdx].lShareOutstanding / ea[nCurIdx].totalHogaVolumeStatus.fCur, SHT_PER_INIT); // 0에 가까울 수 록 좋음
                        ea[nCurIdx].fHogaPerTrade = Min(ea[nCurIdx].totalHogaVolumeStatus.fCur / ea[nCurIdx].tradeStatus.fCur, SHT_PER_INIT); // 0에 가까울 수 록 좋음
                    }

                    double fTmpDenom;

                    if ((ea[nCurIdx].tradeStatus.fCur * ea[nCurIdx].nFs) < MILLION) // 현체결량이 100만원 이하면
                        fTmpDenom = MILLION / (double)ea[nCurIdx].nFs;
                    else
                        fTmpDenom = ea[nCurIdx].tradeStatus.fCur;

                    ea[nCurIdx].fSharePerTrade = Min(ea[nCurIdx].lShareOutstanding / ea[nCurIdx].tradeStatus.fCur, SHT_PER_INIT); // 0에 가까울 수  록 좋음
                    ea[nCurIdx].fTradePerPure = ea[nCurIdx].pureTradeStatus.fCur / fTmpDenom; // 절대값 1에 가까울 수 록 좋음 -면 매도, +면 매수
                    if (ea[nCurIdx].fTradePerPure > 1)
                        ea[nCurIdx].fTradePerPure = 1;
                    else if (ea[nCurIdx].fTradePerPure < -1)
                        ea[nCurIdx].fTradePerPure = -1;


                    // 가격은 매초당 조금씩 줄여나가게 한다
                    // 1분이 지났을 시 0.47퍼센트
                    if (ea[nCurIdx].nPrevPowerUpdateTime == 0)
                    {
                        ea[nCurIdx].nPrevPowerUpdateTime = nFirstTime;
                    }

                    int nTimeGap = SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].nPrevPowerUpdateTime);
                    for (int _ = 0; _ < nTimeGap; _++)
                    {
                        // 가격변화 업데이트
                        ea[nCurIdx].fPowerJar *= 0.995;
                        ea[nCurIdx].fOnlyDownPowerJar *= 0.995;
                        ea[nCurIdx].fOnlyUpPowerJar *= 0.995;
                        ea[nCurIdx].fPlusCnt07 *= 0.7;
                        ea[nCurIdx].fMinusCnt07 *= 0.7;
                        ea[nCurIdx].fPlusCnt09 *= 0.9;
                        ea[nCurIdx].fMinusCnt09 *= 0.9;

                        ea[nCurIdx].nPrevPowerUpdateTime = nSharedTime;
                    }

                    // 파워는 최우선매수호가와 초기가격의 손익률로 계산한다




                    // 가격변화 실시간 처리
                    ea[nCurIdx].fPowerJar += ea[nCurIdx].fPowerDiff;
                    ea[nCurIdx].fOnlyDownPowerJar += ea[nCurIdx].fPowerDiff;
                    if (ea[nCurIdx].fOnlyDownPowerJar > 0)
                        ea[nCurIdx].fOnlyDownPowerJar = 0;

                    ea[nCurIdx].fOnlyUpPowerJar += ea[nCurIdx].fPowerDiff;
                    if (ea[nCurIdx].fOnlyUpPowerJar < 0)
                        ea[nCurIdx].fOnlyUpPowerJar = 0;

                    if (ea[nCurIdx].fPowerDiff > 0)
                    {
                        ea[nCurIdx].fPlusCnt07++;
                        ea[nCurIdx].fPlusCnt09++;
                    }
                    else if (ea[nCurIdx].fPowerDiff < 0)
                    {
                        ea[nCurIdx].fMinusCnt07++;
                        ea[nCurIdx].fMinusCnt09++;
                    }
                    #endregion

                    #region HIT 초기화
                    // 페이크매수/매도 히트횟수 초기화(*동일분봉동안 가지는 히트횟수)
                    if (nTimeLineIdx != ea[nCurIdx].myStrategy.nFakePrevTimeLineIdx)
                    {
                        ea[nCurIdx].myStrategy.nFakePrevTimeLineIdx = nTimeLineIdx;
                        ea[nCurIdx].myStrategy.nHitNum = 0;
                        ea[nCurIdx].fakeBuyStrategy.nHitNum = 0;
                        ea[nCurIdx].fakeResistStrategy.nHitNum = 0;
                        ea[nCurIdx].fakeAssistantStrategy.nHitNum = 0;
                        ea[nCurIdx].priceUpStrategy.nHitNum = 0;
                        ea[nCurIdx].priceDownStrategy.nHitNum = 0;
                    }
                    #endregion

                    if (ea[nCurIdx].isViGauge)
                    {
                        ea[nCurIdx].nRealDataIdxVi = ea[nCurIdx].nViTimeLineIdx;
                        ea[nCurIdx].isViGauge = false;
                        // ea[nCurIdx].nViFs - ea[nCurIdx].nFs;
                    }
                    else
                        ea[nCurIdx].nRealDataIdxVi = ea[nCurIdx].timeLines1m.nRealDataIdx;

                    #region 실시간 Sequence Strategy
                    { // START ---- 실시간 Sequence Strategy 분기문

                        double fCurPower = (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice;
                        #region 5퍼 달성
                        if (ea[nCurIdx].fPowerWithoutGap >= 0.05)
                        {
                            if (!ea[nCurIdx].sequenceStrategy.isFiveReachedReal && nSharedTime <= AddTimeBySec(nFirstTime, 600))
                            {
                                ea[nCurIdx].sequenceStrategy.nFiveReachedRealTimeLineIdx = nTimeLineIdx;
                                ea[nCurIdx].sequenceStrategy.isFiveKeepingForTwoTimeLine = true;
                            }
                            ea[nCurIdx].sequenceStrategy.isFiveReachedReal = true; // 실시간으로 갭제외 5퍼 도달했나

                            if (!ea[nCurIdx].sequenceStrategy.isFiveReachedRealLeafEntranceBlocked)
                            {
                                ea[nCurIdx].sequenceStrategy.isFiveReachedRealLeafEntranceBlocked = true;
                                if (ea[nCurIdx].lTotalTradePrice > 2 * BILLION) // 20억 이상 
                                    ea[nCurIdx].sequenceStrategy.isFiveReachedRealBillionUp = true;
                                else if (ea[nCurIdx].lTotalTradePrice > 5 * HUNDRED_MILLION) // 5억 이상  20억 이하
                                    ea[nCurIdx].sequenceStrategy.isFiveReachedRealHundredMillion = true;
                                else // 5억이하 
                                    ea[nCurIdx].sequenceStrategy.isFiveReachedRealLeafBan = true;
                            }
                        }

                        if (ea[nCurIdx].sequenceStrategy.nFiveReachedRealTimeLineIdx != 0)
                        {
                            if (ea[nCurIdx].sequenceStrategy.nFiveReachedRealTimeLineIdx + 2 >= nTimeLineIdx)
                            {
                                if (ea[nCurIdx].sequenceStrategy.isFiveKeepingForTwoTimeLine)
                                    ea[nCurIdx].sequenceStrategy.isFiveKeepingForTwoTimeLine = (ea[nCurIdx].sequenceStrategy.nFiveReachedRealTimeLineIdx == nTimeLineIdx && ea[nCurIdx].fPowerWithoutGap >= 0.04) || ea[nCurIdx].fPowerWithoutGap >= 0.05;
                            }
                            else
                            {
                                if (ea[nCurIdx].sequenceStrategy.isFiveKeepingForTwoTimeLine) // 2분동안 계속 5퍼를 지속했었다면
                                {
                                    ea[nCurIdx].sequenceStrategy.isFiveKeepingSuccessForTwoTimeLine = true;
                                }
                            }
                        }

                        #endregion

                        #region 캔들 2퍼
                        if (fCurPower >= 0.02)
                        {
                            if (!ea[nCurIdx].sequenceStrategy.isCandleTwoOverReal)
                            {
                                ea[nCurIdx].sequenceStrategy.nCandleTwoOverRealCount++;
                                ea[nCurIdx].sequenceStrategy.isCandleTwoOverReal = true;
                                ea[nCurIdx].sequenceStrategy.nCandleTwoOverRealTimeLineIdx = nTimeLineIdx;
                            }

                            if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].lTotalPrice > 1.5 * HUNDRED_MILLION)
                            {
                                if (!ea[nCurIdx].sequenceStrategy.isCandleTwoOverRealNoLeaf)
                                {
                                    ea[nCurIdx].sequenceStrategy.nCandleTwoOverRealNoLeafCount++;
                                    ea[nCurIdx].sequenceStrategy.isCandleTwoOverRealNoLeaf = true;
                                    ea[nCurIdx].sequenceStrategy.nCandleTwoOverRealNoLeafTimeLineIdx = nTimeLineIdx;
                                }
                            }
                        }
                        #endregion

                        #region 저항라인 만들기
                        if (ea[nCurIdx].lFixedMarketCap <= TRILLION) // 시가총액 1조 안넘는 항목들만
                        {
                            if (ea[nCurIdx].sequenceStrategy.nResistFs != 0 && ea[nCurIdx].sequenceStrategy.nResistFs < ea[nCurIdx].nFs)
                            {
                                // 다른 timeLineIdx이다
                                if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].sequenceStrategy.nResistTime) > 60)
                                {
                                    // 저항선 뚫은거야 
                                    if (!ea[nCurIdx].sequenceStrategy.isResistPeircing)
                                        ea[nCurIdx].sequenceStrategy.nResistPiercingTime = nSharedTime;
                                    ea[nCurIdx].sequenceStrategy.isResistPeircing = true;
                                }
                            }

                            // 현재 거래대금 10억 넘으면
                            if ((ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].lTotalPrice >= TEN_BILLION &&
                                ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].lSellPrice
                                ) || (nTimeLineIdx <= ea[nCurIdx].sequenceStrategy.nResistTimeLineIdx + 1))
                            {
                                if (ea[nCurIdx].sequenceStrategy.nResistFs < ea[nCurIdx].nFs)
                                {
                                    ea[nCurIdx].sequenceStrategy.nResistFs = ea[nCurIdx].nFs;
                                    ea[nCurIdx].sequenceStrategy.nResistTime = nSharedTime;
                                    ea[nCurIdx].sequenceStrategy.nResistTimeLineIdx = nTimeLineIdx;
                                    ea[nCurIdx].sequenceStrategy.nResistUpCount++;
                                }

                            }


                        }

                        #endregion

                        #region BotUp Trace
                        //ea[nCurIdx].sequenceStrategy.botUpReal532_51.Trace(ea[nCurIdx].fPowerWithoutGap, nSharedTime);
                        #endregion
                    } // END ---- 실시간 Sequence Strategy 분기문
                    #endregion

                    #region 페이크매수 전략 체크
                    //=====================================================
                    // 가짜 매수 Part
                    //=====================================================
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        try
                        {
                            int nFakeBuyStrategyPointer = 0;
                            void FakeBuyPointerMove()
                            {
                                nFakeBuyStrategyPointer++;
                            }



                            { // 가짜매수 구역0
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > BILLION && //분당 10억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                    ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                    )
                                {
                                    SetThisFakeBuy(nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역1 
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 3 * BILLION && //분당 30억이상
                                     ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                     ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                     ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                     )
                                {
                                    SetThisFakeBuy(nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역2
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 2 * BILLION && //분당 20억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                    ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                    )
                                {
                                    SetThisFakeBuy(nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역3
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 5 * HUNDRED_MILLION && // 분당 매수대금 5억이상
                                 ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime3.Add(nSharedTime);
                                    //for (int i = 0; i < ea[nCurIdx].fakeBuyStrategy.listApproachTime3.Count; i++)
                                    //{
                                    //    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeBuyStrategy.listApproachTime3[i]) > 600) // 10분 지났다면
                                    //    {
                                    //        ea[nCurIdx].fakeBuyStrategy.listApproachTime3.RemoveAt(i);
                                    //        i--;
                                    //    }
                                    //}

                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime3.Count >= 5)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime3.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 10분내 분당 매수대금 5억이상 매수 > 매도 시가총액100위이상 5번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역4
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > BILLION && // 분당 매수대금 10억이상
                                    ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    SetThisFakeBuy(nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역5
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * BILLION && // 분당 매수대금 20억이상
                                    ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    SetThisFakeBuy(nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역6
                                if (
                                ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > BILLION && // 분당 매수대금 10억이상
                                ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime6.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime6.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime6.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 5분내 분당 매수대금 10억이상 매수 > 매도 시가총액100위이상 4번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역7
                                if (
                               ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 3 * BILLION && // 분당 매수대금 30억이상
                               ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                               ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                               )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime7.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime7.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime7.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 5분내 분당 거래대금 30억이상 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역8
                                if (
                                   ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 1.5 * BILLION && // 분당 매수대금 15억이상
                                   ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                   ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime8.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime8.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime8.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 15분내 분당 매수대금 15억이상 매수 > 매도 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역9
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * BILLION && // 분당 매수대금 20억이상
                                 ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime9.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime9.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime9.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 15분내 분당 매수대금 20억이상 매수 > 매도 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역10
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > BILLION && // 분당 매수대금 10억이상
                                 ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime10.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime10.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime10.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 15분내 분당 매수대금 10억이상 매수 > 매도 시가총액100위이상 4번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역11
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 2 * BILLION && // 분당 거래대금 20억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime11.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime11.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime11.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 15분내 분당 거래대금 20억이상 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역12
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 3 * BILLION && // 분당 거래대금 30억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime12.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime12.Count >= 2)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime12.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 15분내 분당 거래대금 30억이상 시가총액100위이상 2번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역13
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 4 * BILLION && // 분당 거래대금 40억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime13.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime13.Count >= 2)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime13.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 15분내 분당 거래대금 40억이상 시가총액100위이상 2번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역14
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > BILLION * 1.5 && //분당 15억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                    ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    SetThisFakeBuy(nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역15
                                if (
                               ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 1.5 * BILLION && // 분당 매수대금 15억이상
                               ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖nFakeBuyStrategyPointer
                               ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                               )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime15.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime15.Count >= 5)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime15.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 30분내 분당 매수대금 15억이상 매수 > 매도 시가총액100위이상 5번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역16
                                if (
                                   //  분당 매수대금 15억이상 매수 > 매도 시가총액100위이상..1분제한
                                   ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 1.5 * BILLION && // 분당 매수대금 15억이상
                                   ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                   ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                    )
                                {
                                    SetThisFakeBuy(nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역17
                                if (
                                ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * BILLION && // 분당 매수대금 20억이상
                                ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime17.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime17.Count >= 2)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime17.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 6분내 분당 매수대금 20억이상 매수 > 매도 시가총액100위이상 2번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역18
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 1.5 * BILLION && // 분당 매수대금 15억이상
                                 ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime18.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime18.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime18.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 10분내 분당 매수대금 15억이상 매수 > 매도 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역19
                                if (
                                ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > BILLION && // 분당 매수대금 10억이상
                                ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime19.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime19.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime19.Clear();
                                        SetThisFakeBuy(nFakeBuyStrategyPointer); // 10분내 분당 매수대금 10억이상 매수 > 매도 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                        }
                        catch (Exception fakeBuyException) // 가짜매수에서 문제가 생겼을때 ex) Idx오류
                        {

                        }
                        finally
                        {
                            ea[nCurIdx].fakeBuyStrategy.isSuddenBoom = false;
                        }
                    }// END ---- 가짜매수
                    #endregion

                    #region 페이크보조 전략 체크
                    //=====================================================
                    // 가짜 보조 Part
                    //=====================================================
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        try
                        {
                            int nFakeAssistantStrategyPointer = 0;
                            void FakeAssistantPointerMove()
                            {
                                nFakeAssistantStrategyPointer++;
                            }


                            { // 가짜보조 구역0
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumUpPower + ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumDownPower >= 1.2 &&
                                    ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                    )
                                {
                                    SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역1
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumUpPower + ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumDownPower >= 2 &&
                                     ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                     )
                                {
                                    SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역2
                                if (
                                  ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumUpPower + ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumDownPower >= 1.2 &&
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime2.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime2.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime2.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역3
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumUpPower + ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumDownPower >= 1 &&
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime3.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime3.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime3.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역4
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nCount >= 1000 &&
                                    ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                    ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                    )
                                {
                                    SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역5
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nCount >= 600 &&
                                  ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime5.Add(nSharedTime);

                                    //for (int i = 0; i < ea[nCurIdx].fakeAssistantStrategy.listApproachSellTime.Count; i++)
                                    //{
                                    //    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeAssistantStrategy.listApproachSellTime[i]) > 900) // 15분 지났다면
                                    //    {
                                    //        ea[nCurIdx].fakeAssistantStrategy.listApproachSellTime.RemoveAt(i);
                                    //        i--;
                                    //    }
                                    //}
                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime5.Count >= 7)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime5.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역6
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nCount >= 600 &&
                                  ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime6.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime6.Count >= 5)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime6.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역7
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 5 * HUNDRED_MILLION &&
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                  ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역8
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 5 * HUNDRED_MILLION &&
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                  ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime8.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime8.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime8.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역9
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * HUNDRED_MILLION &&
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                 ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime9.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime9.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime9.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역10
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * HUNDRED_MILLION &&
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                 ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime10.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime10.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime10.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역11
                                if (ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking > 0 &&
                                    ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking <= 2 &&
                                  ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime11.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime11.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime11.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역12
                                if (ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking > 0 &&
                                    ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking <= 5 &&
                                  ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime12.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime12.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime12.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역13
                                if (ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking == 1 &&
                                  ea[nCurIdx].rankSystem.nMarketCapRanking > 200 && // 시가총액 100위 밖
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime13.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime13.Count >= 2)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime13.Clear();
                                        SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역14
                                if (ea[nCurIdx].crushMinuteManager.isCrushRealTimeCheck)
                                {
                                    SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역15
                                if (ea[nCurIdx].crushMinuteManager.isCrushCheck)
                                {
                                    SetThisFakeAssistant(nFakeAssistantStrategyPointer);
                                }
                            }
                        }
                        catch (Exception fakeAssistantException)
                        {

                        }
                        finally
                        {
                            ea[nCurIdx].fakeAssistantStrategy.isSuddenBoom = false;
                        }
                    }
                    #endregion

                    #region 페이크저항 전략체크
                    //=====================================================
                    // 가짜 저항 Part
                    //=====================================================
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        try
                        {
                            int nFakeResistStrategyPointer = 0;
                            void FakeResistPointerMove()
                            {
                                nFakeResistStrategyPointer++;
                            }

                            { // 가짜저항 구역0
                                if (
                                 ea[nCurIdx].fStartGap >= 0.03 &&
                                 ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0
                                 )
                                {
                                    for (int _ = 0; _ < 1; _++)
                                        SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역1 
                                if (
                                  ea[nCurIdx].fStartGap >= 0.04 &&
                                  ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0
                                  )
                                {
                                    for (int _ = 0; _ < 1; _++)
                                        SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역2
                                if (
                                 ea[nCurIdx].fStartGap >= 0.05 &&
                                 ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0
                                 )
                                {
                                    for (int _ = 0; _ < 2; _++)
                                        SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역3
                                if (
                                  ea[nCurIdx].fStartGap >= 0.06 &&
                                  ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0
                                  )
                                {
                                    for (int _ = 0; _ < 2; _++)
                                        SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역4
                                if (
                                  ea[nCurIdx].fStartGap >= 0.07 &&
                                  ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0
                                  )
                                {
                                    for (int _ = 0; _ < 2; _++)
                                        SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역5
                                if (
                                  ea[nCurIdx].fStartGap >= 0.1 &&
                                  ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0
                                  )
                                {
                                    for (int _ = 0; _ < 3; _++)
                                        SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역6
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nMaxFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nUpFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.01 &&
                                  (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                  )
                                {
                                    SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역7
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nMaxFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nUpFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.02 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역8
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.05 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();
                            { // 가짜저항 구역9
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.04 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }

                            FakeResistPointerMove();
                            { // 가짜저항 구역10
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.04 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();
                            { // 가짜저항 구역11
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.03 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();
                            { // 가짜저항 구역12
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.03 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFakeResist(nFakeResistStrategyPointer);
                                }
                            }
                        }
                        catch (Exception fakeSellException)
                        {

                        }
                        finally
                        {
                            ea[nCurIdx].fakeResistStrategy.isSuddenBoom = false;
                        }
                    }// END ---- 가짜매도
                    #endregion

                    #region 가격업 전략체크
                    //=====================================================
                    // 가격 업 Part
                    //=====================================================
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        try
                        {
                            int nPriceUpStrategyPointer = 0;
                            void PriceUpPointerMove()
                            {
                                nPriceUpStrategyPointer++;
                            }

                            { // 가격업 구역0
                                if ((double)(ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.02 &&
                                    ((ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] == 0) || (ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한
                                    )
                                {
                                    SetThisPriceUp(nPriceUpStrategyPointer);
                                }
                            }
                            PriceUpPointerMove();

                            { // 가격업 구역1
                                if (
                                     (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.01 &&
                                     ((ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] == 0) || (ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                     )
                                {
                                    ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceUpStrategy.listApproachTime1.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceUpStrategy.listApproachTime1.Count >= 2)
                                    {
                                        ea[nCurIdx].priceUpStrategy.listApproachTime1.Clear();
                                        SetThisPriceUp(nPriceUpStrategyPointer);
                                    }
                                }
                            }
                            PriceUpPointerMove();

                            { // 가격업 구역2
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.01 &&
                                  ((ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] == 0) || (ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceUpStrategy.listApproachTime2.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceUpStrategy.listApproachTime2.Count >= 3)
                                    {
                                        ea[nCurIdx].priceUpStrategy.listApproachTime2.Clear();
                                        SetThisPriceUp(nPriceUpStrategyPointer);
                                    }
                                }
                            }
                            PriceUpPointerMove();
                            { // 가격업 구역3
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.01 &&
                                   ((ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] == 0) || (ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceUpStrategy.listApproachTime3.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceUpStrategy.listApproachTime3.Count >= 4)
                                    {
                                        ea[nCurIdx].priceUpStrategy.listApproachTime3.Clear();
                                        SetThisPriceUp(nPriceUpStrategyPointer);
                                    }
                                }
                            }
                            PriceUpPointerMove();
                            { // 가격업 구역4
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.01 &&
                                   ((ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] == 0) || (ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceUpStrategy.listApproachTime4.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceUpStrategy.listApproachTime4.Count >= 5)
                                    {
                                        ea[nCurIdx].priceUpStrategy.listApproachTime4.Clear();
                                        SetThisPriceUp(nPriceUpStrategyPointer);
                                    }
                                }
                            }
                            PriceUpPointerMove();
                            { // 가격업 구역5
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.01 &&
                                   ((ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] == 0) || (ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceUpStrategy.listApproachTime5.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceUpStrategy.listApproachTime5.Count >= 6)
                                    {
                                        ea[nCurIdx].priceUpStrategy.listApproachTime5.Clear();
                                        SetThisPriceUp(nPriceUpStrategyPointer);
                                    }
                                }
                            }
                            PriceUpPointerMove();
                            { // 가격업 구역6
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.01 &&
                                   ((ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] == 0) || (ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceUpStrategy.listApproachTime6.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceUpStrategy.listApproachTime6.Count >= 7)
                                    {
                                        ea[nCurIdx].priceUpStrategy.listApproachTime6.Clear();
                                        SetThisPriceUp(nPriceUpStrategyPointer);
                                    }
                                }
                            }
                            PriceUpPointerMove();
                            { // 가격업 구역7
                                if ((double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.02 &&
                                  ((ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] == 0) || (ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한
                                  )
                                {
                                    SetThisPriceUp(nPriceUpStrategyPointer);
                                }
                            }
                            PriceUpPointerMove();
                            { // 가격업 구역8
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.02 &&
                                  ((ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] == 0) || (ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].priceUpStrategy.arrPrevMinuteIdx[nPriceUpStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceUpStrategy.listApproachTime8.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceUpStrategy.listApproachTime8.Count >= 2)
                                    {
                                        ea[nCurIdx].priceUpStrategy.listApproachTime8.Clear();
                                        SetThisPriceUp(nPriceUpStrategyPointer);
                                    }
                                }
                            }
                        }
                        catch (Exception priceUpException)
                        {

                        }
                        finally
                        {
                            ea[nCurIdx].priceUpStrategy.isSuddenBoom = false;
                        }
                    }// END ---- 가격업
                    #endregion

                    #region 가격다운 전략체크
                    //=====================================================
                    // 가격 다운 Part
                    //=====================================================
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        try
                        {
                            int nPriceDownStrategyPointer = 0;
                            void PriceDownPointerMove()
                            {
                                nPriceDownStrategyPointer++;
                            }

                            { // 가격다운 구역0
                                if ((double)(ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice <= -0.02 &&
                                    ((ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] == 0) || (ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한
                                    )
                                {
                                    SetThisPriceDown(nPriceDownStrategyPointer);
                                }
                            }
                            PriceDownPointerMove();

                            { // 가격다운 구역1
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice <= -0.01 &&
                                   ((ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] == 0) || (ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceDownStrategy.listApproachTime1.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceDownStrategy.listApproachTime1.Count >= 2)
                                    {
                                        ea[nCurIdx].priceDownStrategy.listApproachTime1.Clear();
                                        SetThisPriceDown(nPriceDownStrategyPointer);
                                    }
                                }
                            }
                            PriceDownPointerMove();
                            { // 가격다운 구역2
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice <= -0.01 &&
                                  ((ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] == 0) || (ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceDownStrategy.listApproachTime2.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceDownStrategy.listApproachTime2.Count >= 3)
                                    {
                                        ea[nCurIdx].priceDownStrategy.listApproachTime2.Clear();
                                        SetThisPriceDown(nPriceDownStrategyPointer);
                                    }
                                }
                            }
                            PriceDownPointerMove();
                            { // 가격다운 구역3
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice <= -0.01 &&
                                  ((ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] == 0) || (ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceDownStrategy.listApproachTime3.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceDownStrategy.listApproachTime3.Count >= 4)
                                    {
                                        ea[nCurIdx].priceDownStrategy.listApproachTime3.Clear();
                                        SetThisPriceDown(nPriceDownStrategyPointer);
                                    }
                                }
                            }
                            PriceDownPointerMove();
                            { // 가격다운 구역4
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice <= -0.01 &&
                                  ((ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] == 0) || (ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceDownStrategy.listApproachTime4.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceDownStrategy.listApproachTime4.Count >= 5)
                                    {
                                        ea[nCurIdx].priceDownStrategy.listApproachTime4.Clear();
                                        SetThisPriceDown(nPriceDownStrategyPointer);
                                    }
                                }
                            }
                            PriceDownPointerMove();
                            { // 가격다운 구역5
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice <= -0.01 &&
                                  ((ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] == 0) || (ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceDownStrategy.listApproachTime5.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceDownStrategy.listApproachTime5.Count >= 6)
                                    {
                                        ea[nCurIdx].priceDownStrategy.listApproachTime5.Clear();
                                        SetThisPriceDown(nPriceDownStrategyPointer);
                                    }
                                }
                            }
                            PriceDownPointerMove();
                            { // 가격다운 구역6
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice <= -0.01 &&
                                  ((ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] == 0) || (ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceDownStrategy.listApproachTime6.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceDownStrategy.listApproachTime6.Count >= 7)
                                    {
                                        ea[nCurIdx].priceDownStrategy.listApproachTime6.Clear();
                                        SetThisPriceDown(nPriceDownStrategyPointer);
                                    }
                                }
                            }
                            PriceDownPointerMove();
                            { // 가격다운 구역7
                                if ((double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice <= -0.02 &&
                                   ((ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] == 0) || (ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한
                                   )
                                {
                                    SetThisPriceDown(nPriceDownStrategyPointer);
                                }
                            }
                            PriceDownPointerMove();
                            { // 가격다운 구역8
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice <= -0.02 &&
                                  ((ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] == 0) || (ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].priceDownStrategy.arrPrevMinuteIdx[nPriceDownStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].priceDownStrategy.listApproachTime8.Add(nTimeLineIdx);
                                    if (ea[nCurIdx].priceDownStrategy.listApproachTime8.Count >= 2)
                                    {
                                        ea[nCurIdx].priceDownStrategy.listApproachTime8.Clear();
                                        SetThisPriceDown(nPriceDownStrategyPointer);
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                        finally
                        {
                            ea[nCurIdx].priceDownStrategy.isSuddenBoom = false;
                        }
                    }// END ---- 가격다운
                    #endregion

                    #region 페이크정보 취합
                    //=====================================================
                    // 실매수 전 Update
                    //=====================================================
                    {
                        if (ea[nCurIdx].myStrategy.listFakeHistoryPiece.Count > 0)
                        {
                            if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myStrategy.listFakeHistoryPiece[0].nSharedTime) > 900)
                            {
                                UpdateFakeHistory();
                                CalcFakeHistory();
                            }
                        }
                    }
                    #endregion

                    #region 실매수
                    //=====================================================
                    // 전략매수, 실매수 Part
                    //=====================================================
                    if (!ea[nCurIdx].isExcluded) //&& nSharedTime < BAN_BUY_TIME) // START ---- 제외되지 않았다면 매수 가능, 뒤 조건은 잠시 빼놓은 상태(일단은 매매컨트롤러- 매수에서 매수금지 시간을 정해줬다)
                    {
                        try
                        {
                            // 전략 0번째는 추가매수의 슬롯이다
                            int nRealBuyStrategyPointer = 1;
                            void RealBuyPointerMove()
                            {
                                nRealBuyStrategyPointer++;
                            }

                            { // 실매수 구역1
                                if (nSharedTime < AddTimeBySec(nFirstTime, 300) &&
                                    ea[nCurIdx].fPower >= 0.06 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역2
                                if (nSharedTime < AddTimeBySec(nFirstTime, 600) &&
                                    ea[nCurIdx].fPower >= 0.085 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역3
                                if (ea[nCurIdx].crushMinuteManager.isCrushCheck &&
                                    ea[nCurIdx].sequenceStrategy.isFiveReachedReal &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역4
                                if (ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 15 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                               )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역5
                                if (ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 25 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역6
                                if (ea[nCurIdx].fPlusCnt07 + ea[nCurIdx].fMinusCnt07 >= 30 &&
                                    ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 15 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역7
                                if (ea[nCurIdx].fPlusCnt07 + ea[nCurIdx].fMinusCnt07 >= 50 &&
                                   ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 15 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                               )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역8
                                if (ea[nCurIdx].fPlusCnt09 + ea[nCurIdx].fMinusCnt09 >= 50 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 15 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역9
                                if (ea[nCurIdx].fPlusCnt09 + ea[nCurIdx].fMinusCnt09 >= 70 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 15 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역10
                                if (ea[nCurIdx].fPlusCnt09 + ea[nCurIdx].fMinusCnt09 >= 90 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 10 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역11
                                if (ea[nCurIdx].fPlusCnt09 + ea[nCurIdx].fMinusCnt09 >= 90 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 20 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역12
                                if (ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 30 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역13
                                if (ea[nCurIdx].fPowerJar >= 0.02 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역14
                                if (ea[nCurIdx].fPowerJar >= 0.03 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역15
                                if (ea[nCurIdx].fPowerJar >= 0.04 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역16
                                if (ea[nCurIdx].rankSystem.nSummationRanking == 1 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역17
                                if (ea[nCurIdx].rankSystem.nMinutePowerRanking == 1 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역18
                                if (ea[nCurIdx].rankSystem.nSummationRanking == 2 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역19
                                if (ea[nCurIdx].rankSystem.nMinutePowerRanking == 2 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역20
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nCount >= 1000 &&
                                     ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 15 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                      )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역21
                                if (ea[nCurIdx].crushMinuteManager.isCrushCheck &&
                                    ea[nCurIdx].rankSystem.nSummationRanking > 0 &&
                                    ea[nCurIdx].rankSystem.nSummationRanking <= 30 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역22
                                if (ea[nCurIdx].crushMinuteManager.isCrushCheck &&
                                    ea[nCurIdx].rankSystem.nSummationRanking > 0 &&
                                    ea[nCurIdx].rankSystem.nSummationRanking <= 10 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역23
                                if (ea[nCurIdx].rankSystem.nMinuteSummationRanking == 1 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역24
                                if (ea[nCurIdx].timeLines1m.fRecentMedianAngle >= 50 &&
                                    GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역25
                                if (ea[nCurIdx].myStrategy.isOrderCheck &&
                                    ea[nCurIdx].fPowerJar > 0.117
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역26
                                if (ea[nCurIdx].myStrategy.isOrderCheck &&
                                    ea[nCurIdx].fStartGap >= 0.06 &&
                                    ea[nCurIdx].speedStatus.fCur >= 700
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역27
                                if (ea[nCurIdx].myStrategy.isOrderCheck &&
                                     ea[nCurIdx].fStartGap >= 0.07 &&
                                     ea[nCurIdx].fMinusCnt09 >= 100
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역28
                                if (ea[nCurIdx].myStrategy.isOrderCheck &&
                                    ea[nCurIdx].speedStatus.fCur >= 1200 &&
                                    ea[nCurIdx].speedStatus.fCur <= 1350
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역29
                                if (ea[nCurIdx].myStrategy.isManualOrderSignal)
                                {
                                    ea[nCurIdx].myStrategy.isManualOrderSignal = false;
                                    if (nTimeBetweenPrev >= ea[nCurIdx].myStrategy.nManualEndurationTime)
                                    {
                                        PrintLog("시간 : " + nSharedTime.ToString() + ", 종목코드 : " + ea[nCurIdx].sCode + ", 종목명 : " + ea[nCurIdx].sCodeName + " 전략 : " + nRealBuyStrategyPointer.ToString() + " " + nTimeBetweenPrev.ToString() + "초동안 체결전적이 없어" + " 직접입력매수신청 기각 ", nCurIdx);
                                    }
                                    else
                                    {
                                        RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, nExtraChance: 3);
                                    }
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역30
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute421.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute421.CheckIsRedundancy()
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역31
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute432.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute432.CheckIsRedundancy()
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역32
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute642.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute642.CheckIsRedundancy()
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역33
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute643.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute643.CheckIsRedundancy()
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역34
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute732.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute732.CheckIsRedundancy()
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역35
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute743.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute743.CheckIsRedundancy()
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역36
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute953.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute953.CheckIsRedundancy()
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역37
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute421.isJumped
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역38
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute432.isJumped
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역39
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute642.isJumped
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역40
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute643.isJumped
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역41
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute732.isJumped
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역42
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute743.isJumped
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역43
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute953.isJumped
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역44
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute421.isCrushed
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역45
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute432.isCrushed
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역46
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute642.isCrushed
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역47
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute643.isCrushed
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역48
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute732.isCrushed
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역49
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute743.isCrushed
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역50
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute953.isCrushed
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역51
                                if (
                                        ea[nCurIdx].maOverN.nDownCntMa1h >= 30 &&
                                        ea[nCurIdx].fPower >= 0.04 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역52
                                if (
                                         ea[nCurIdx].fStartGap >= 0.02 &&
                                         ea[nCurIdx].fStartGap < 0.03 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역53
                                if (
                                        ea[nCurIdx].fStartGap >= 0.03 &&
                                        ea[nCurIdx].fStartGap < 0.04 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역54
                                if (
                                        ea[nCurIdx].fStartGap >= 0.04 &&
                                        ea[nCurIdx].fStartGap < 0.05 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역55
                                if (
                                        ea[nCurIdx].fStartGap >= 0.05 &&
                                        ea[nCurIdx].fStartGap < 0.06 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역56
                                if (
                                        ea[nCurIdx].fStartGap >= 0.06 &&
                                        ea[nCurIdx].fStartGap < 0.07 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역57
                                if (
                                        ea[nCurIdx].fStartGap >= 0.07 &&
                                        ea[nCurIdx].fStartGap < 0.1 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역58
                                if (
                                        ea[nCurIdx].fStartGap >= 0.1 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역59
                                if (
                                        (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.02 &&
                                        (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice < 0.03 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 1)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역60
                                if (
                                        (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.03 &&
                                        (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice < 0.04 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 1)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역61
                                if (
                                        (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.04 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 1)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역62
                                if (
                                        ea[nCurIdx].timeLines1m.fDAngle <= -60 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역63
                                if (
                                        ea[nCurIdx].sequenceStrategy.isFiveReachedMinute &&
                                        ea[nCurIdx].timeLines1m.fDAngle <= -40 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역64
                                if (
                                        ea[nCurIdx].sequenceStrategy.isFiveReachedMinute &&
                                        ea[nCurIdx].timeLines1m.fDAngle <= -50 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역65
                                if (
                                        ea[nCurIdx].sequenceStrategy.isFiveReachedMinute &&
                                        ea[nCurIdx].timeLines1m.fDAngle <= -60 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역66
                                if (
                                        ea[nCurIdx].fakeBuyStrategy.nStrategyNum >= 10 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역67
                                if (
                                        ea[nCurIdx].fakeBuyStrategy.nStrategyNum >= 20 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역68
                                if (
                                        ea[nCurIdx].fakeBuyStrategy.nStrategyNum >= 30 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역69
                                if (
                                        ea[nCurIdx].fakeResistStrategy.nStrategyNum >= 10 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역70
                                if (
                                        ea[nCurIdx].fakeResistStrategy.nStrategyNum >= 20 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역71
                                if (
                                        ea[nCurIdx].priceUpStrategy.nStrategyNum >= 10 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역72
                                if (
                                        ea[nCurIdx].priceUpStrategy.nStrategyNum >= 20 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역73
                                if (
                                        ea[nCurIdx].fakeAssistantStrategy.nStrategyNum >= 20 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역74
                                if (
                                        ea[nCurIdx].fakeBuyStrategy.nUpperCount >= 10 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역75
                                if (
                                        ea[nCurIdx].fakeAssistantStrategy.nUpperCount >= 10 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역76
                                if (
                                        ea[nCurIdx].priceUpStrategy.nUpperCount >= 10 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역77
                                if (
                                        ea[nCurIdx].fakeAssistantStrategy.nHitNum + ea[nCurIdx].fakeBuyStrategy.nHitNum + ea[nCurIdx].priceUpStrategy.nHitNum >= 10 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역78
                                if (
                                        ea[nCurIdx].fakeAssistantStrategy.nHitNum + ea[nCurIdx].fakeBuyStrategy.nHitNum + ea[nCurIdx].priceUpStrategy.nHitNum + ea[nCurIdx].fakeResistStrategy.nHitNum >= 20 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역79
                                if (
                                        ea[nCurIdx].fPowerWithoutGap <= -0.05 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역80
                                if (
                                        ea[nCurIdx].fPowerWithoutGap <= -0.08 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역81
                                if (
                                        ea[nCurIdx].fPowerWithoutGap <= -0.1 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역82
                                if (
                                        ea[nCurIdx].fPowerWithoutGap <= -0.13 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역83
                                if (
                                        ea[nCurIdx].fPowerWithoutGap <= -0.17 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역84
                                if (nSharedTime < AddTimeBySec(nFirstTime, 300) &&
                                    ea[nCurIdx].sequenceStrategy.isFiveReachedReal &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역85
                                if (nSharedTime >= AddTimeBySec(nFirstTime, 300) &&
                                    nSharedTime < AddTimeBySec(nFirstTime, 600) &&
                                    ea[nCurIdx].sequenceStrategy.isFiveReachedReal &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역86
                                if (ea[nCurIdx].sequenceStrategy.isFiveKeepingSuccessForTwoTimeLine &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                 )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역87
                                if (ea[nCurIdx].fakeBuyStrategy.nStrategyNum >= 40 &&
                                   GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                               )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역88
                                if (ea[nCurIdx].fakeBuyStrategy.nMinuteLocationCount >= 3 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역89
                                if (ea[nCurIdx].fakeBuyStrategy.nMinuteLocationCount >= 5 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역90
                                if (ea[nCurIdx].fakeBuyStrategy.nMinuteLocationCount >= 10 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                               )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역91
                                if (ea[nCurIdx].fakeBuyStrategy.nMinuteLocationCount >= 15 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역92
                                if (ea[nCurIdx].priceUpStrategy.nMinuteLocationCount >= 5 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역93
                                if (ea[nCurIdx].priceUpStrategy.nMinuteLocationCount >= 10 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역94
                                if (ea[nCurIdx].priceUpStrategy.nMinuteLocationCount >= 15 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역95
                                if (ea[nCurIdx].priceUpStrategy.nMinuteLocationCount >= 25 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                  )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역96
                                if (ea[nCurIdx].myStrategy.nSharedMinuteLocationCount >= 20 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역97
                                if (ea[nCurIdx].myStrategy.nSharedMinuteLocationCount >= 30 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역98
                                if (ea[nCurIdx].myStrategy.nSharedMinuteLocationCount >= 40 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역99
                                if (ea[nCurIdx].myStrategy.nMinuteLocationCount >= 3 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역100
                                if (ea[nCurIdx].myStrategy.nMinuteLocationCount >= 5 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역101
                                if (ea[nCurIdx].myStrategy.nMinuteLocationCount >= 10 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역102
                                if (ea[nCurIdx].myStrategy.nMinuteLocationCount >= 15 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역103
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs > 0 &&
                                    (double)(ea[nCurIdx].nFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.03 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 10)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역104
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs > 0 &&
                                    (double)(ea[nCurIdx].nFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.05 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 5)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역105
                                if (ea[nCurIdx].fPowerWithoutGap >= 0.065 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역106
                                if (ea[nCurIdx].fPowerWithoutGap >= 0.08 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역107
                                if (ea[nCurIdx].fPowerWithoutGap >= 0.11 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역108
                                if (ea[nCurIdx].timeLines1m.fTotalMedianAngle >= 20 &&
                                    ea[nCurIdx].timeLines1m.fHourMedianAngle >= 30 &&
                                    ea[nCurIdx].timeLines1m.fRecentMedianAngle >= 40 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역109
                                if (ea[nCurIdx].rankSystem.nAccumCountRanking > 0 &&
                                    ea[nCurIdx].rankSystem.nAccumCountRanking <= 10 &&
                                    ea[nCurIdx].rankSystem.nMinuteSummationRanking <= 3 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역110
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nCount >= 1000 &&
                                    ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 15 &&
                                      GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                     )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역111
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nCount >= 600 &&
                                   ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 20 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역112
                                if (ea[nCurIdx].fPowerWithoutGap >= 0.04 &&
                                        GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer))
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역113
                                if (ea[nCurIdx].crushMinuteManager.isCrushCheck &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nTrial: 2)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역114
                                if (ea[nCurIdx].timeLines1m.fRecentMedianAngle >= 60 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역115
                                if (ea[nCurIdx].fStartGap >= 0.03 &&
                                    ea[nCurIdx].fPowerJar >= 0.02 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역116
                                if (ea[nCurIdx].fStartGap >= 0.04 &&
                                    ea[nCurIdx].fPowerJar >= 0.02 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역117
                                if (ea[nCurIdx].fStartGap >= 0.04 &&
                                    ea[nCurIdx].fPowerJar >= 0.03 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역118
                                if (ea[nCurIdx].fStartGap >= 0.03 &&
                                    ea[nCurIdx].speedStatus.fCur >= 300 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역119
                                if (ea[nCurIdx].fStartGap >= 0.03 &&
                                    ea[nCurIdx].speedStatus.fCur >= 500 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역120
                                if (ea[nCurIdx].fStartGap >= 0.03 &&
                                    ea[nCurIdx].speedStatus.fCur >= 800 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역121
                                if (ea[nCurIdx].crushMinuteManager.nUpCnt == 1 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역122
                                if (ea[nCurIdx].crushMinuteManager.nUpCnt == 2 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역123
                                if (ea[nCurIdx].crushMinuteManager.nUpCnt == 3 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역124
                                if (ea[nCurIdx].myStrategy.nTotalFakeMinuteAreaNum >= 10 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역125
                                if (ea[nCurIdx].myStrategy.nTotalFakeMinuteAreaNum >= 15 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역126
                                if (ea[nCurIdx].myStrategy.nTotalFakeMinuteAreaNum >= 20 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역127
                                if (ea[nCurIdx].myStrategy.nTotalFakeMinuteAreaNum >= 30 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역128
                                if (ea[nCurIdx].fakeBuyStrategy.nHitNum >= 5 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역129
                                if (ea[nCurIdx].fakeBuyStrategy.nHitNum >= 7 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역130
                                if (ea[nCurIdx].fakeAssistantStrategy.nHitNum >= 7 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역131
                                if (ea[nCurIdx].fMinusCnt09 >= 100 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역132
                                if (ea[nCurIdx].fStartGap >= 0.03 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 20 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역133
                                if (ea[nCurIdx].fStartGap >= 0.03 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 30 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역134
                                if (ea[nCurIdx].fPlusCnt09 >= 70 &&
                                    ea[nCurIdx].fPlusCnt09 > ea[nCurIdx].fMinusCnt09 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역135
                                if (ea[nCurIdx].fSharePerHoga < 100 &&
                                    ea[nCurIdx].fHogaPerTrade < 25 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역136
                                if (ea[nCurIdx].fSharePerTrade < 200 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역137
                                if (ea[nCurIdx].fPower <= -0.07 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역138
                                if (ea[nCurIdx].fPower <= -0.1 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역139
                                if (ea[nCurIdx].fPower <= -0.13 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역140
                                if (ea[nCurIdx].fPower <= -0.15 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역141
                                if (ea[nCurIdx].fPower <= -0.2 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO, eTradeMethod: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역142
                                if (ea[nCurIdx].fOnlyUpPowerJar >= 0.02 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역143
                                if (ea[nCurIdx].fOnlyUpPowerJar >= 0.03 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역144
                                if (ea[nCurIdx].fOnlyUpPowerJar >= 0.04 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역145
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 1800) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.07 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역146
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 1800) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.1 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역147
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 1800) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.12 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역148
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 3600) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.08 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역149
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 3600) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.12 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역150
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 3600) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.15 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역151
                                if (ea[nCurIdx].speedStatus.fCur >= 400 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역152
                                if (ea[nCurIdx].speedStatus.fCur >= 600 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                            { // 실매수 구역153
                                if (ea[nCurIdx].speedStatus.fCur >= 500 &&
                                    ea[nCurIdx].hogaRatioStatus.fCur >= 0.3 &&
                                     GetAccess(ref ea[nCurIdx].myStrategy, nRealBuyStrategyPointer)
                                    )
                                {
                                    RequestThisRealBuy(nRealBuyStrategyPointer, NORMAL_TRADE_RATIO);
                                }
                            }
                            RealBuyPointerMove();

                        }
                        catch  // 혹시 내 실수로 STRATEGY_NUM을 초과한 전략을 세울 수 도 있으니까
                        {

                        }
                        finally
                        {
                            ea[nCurIdx].myStrategy.isSuddenBoom = false;
                            ea[nCurIdx].myStrategy.isOrderCheck = false;
                            ea[nCurIdx].crushMinuteManager.isCrushCheck = false;
                            ea[nCurIdx].crushMinuteManager.isCrushRealTimeCheck = false;

                            ea[nCurIdx].sequenceStrategy.botUpMinute421.Confirm();
                            ea[nCurIdx].sequenceStrategy.botUpMinute432.Confirm();
                            ea[nCurIdx].sequenceStrategy.botUpMinute642.Confirm();
                            ea[nCurIdx].sequenceStrategy.botUpMinute643.Confirm();
                            ea[nCurIdx].sequenceStrategy.botUpMinute732.Confirm();
                            ea[nCurIdx].sequenceStrategy.botUpMinute743.Confirm();
                            ea[nCurIdx].sequenceStrategy.botUpMinute953.Confirm();
                        }
                    }// END ---- 전략매수
                    #endregion


                  
                }// End ---- e.sRealType.Equals("주식체결")
                #endregion
                #region 매매블럭 모니터링
                // ==============================================================================
                // 당일해당종목 매매 관리창
                // ==============================================================================
                // 추매, 유예, 선점 등의 부분
                if (isZooSikCheGyul || isHogaJanRyang)
                {
                    if (ea[nCurIdx].isFirstCheck)
                    {
                        #region 페이크 블록 기록
                        try
                        {
                            if (nSharedTime < SHUTDOWN_TIME)
                            {
                                for (int i = 0; i < ea[nCurIdx].myStrategy.nTotalBlockCount; i++)
                                {
                                    int nOverPrice = ea[nCurIdx].myStrategy.fd[i].fr.nOverPrice;
                                    // 거래하고 3시전까지 실시간으로
                                    ea[nCurIdx].myStrategy.fd[i].maxMinRealTilThree.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);


                                    // 거래하고 3시전까지 분봉으로(바로 처음 타임라인인덱스는 사기전을 가리키기 때문에 접근못한다)
                                    if (ea[nCurIdx].myStrategy.fd[i].nTimeLineIdx < ea[nCurIdx].timeLines1m.nRealDataIdx) // nBuyMinuteIdx가 N일때 nRealDataIdx는 N -1 , 사고 난 다음분봉 데이터부터 기록
                                    {
                                        ea[nCurIdx].myStrategy.fd[i].maxMinMinuteTilThree.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                        if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myStrategy.fd[i].fr.nRqTime) <= 600)
                                            ea[nCurIdx].myStrategy.fd[i].maxMinMinuteTilThreeWhile10.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                        if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myStrategy.fd[i].fr.nRqTime) <= 1800)
                                            ea[nCurIdx].myStrategy.fd[i].maxMinMinuteTilThreeWhile30.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);
                                    }

                                    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myStrategy.fd[i].fr.nRqTime) <= 600) // 거래하고 10분정도만
                                        ea[nCurIdx].myStrategy.fd[i].maxMinRealWhile10.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myStrategy.fd[i].fr.nRqTime) <= 1800) // 거래하고 30분정도만
                                        ea[nCurIdx].myStrategy.fd[i].maxMinRealWhile30.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myStrategy.fd[i].fr.nRqTime) <= 3600) // 거래하고 1시간정도만
                                        ea[nCurIdx].myStrategy.fd[i].maxMinRealWhile60.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                    #region 슬리피지용 변수 기록
                                    ea[nCurIdx].myStrategy.fd[i].fr.nNoMoveEndCnt = ea[nCurIdx].nNoMoveCount;
                                    ea[nCurIdx].myStrategy.fd[i].fr.nFewSpeedEndCnt = ea[nCurIdx].nFewSpeedCount;
                                    ea[nCurIdx].myStrategy.fd[i].fr.nMissEndCnt = ea[nCurIdx].nMissCount;
                                    ea[nCurIdx].myStrategy.fd[i].fr.lTotalTradeEndPrice = ea[nCurIdx].lTotalTradePrice;
                                    ea[nCurIdx].myStrategy.fd[i].fr.lTotalBuyEndPrice = ea[nCurIdx].lOnlyBuyPrice;
                                    ea[nCurIdx].myStrategy.fd[i].fr.lTotalSellEndPrice = ea[nCurIdx].lOnlySellPrice;
                                    #endregion

                                    if (isHogaJanRyang)
                                        ea[nCurIdx].myStrategy.fd[i].fr.nSlotHogaEndCnt++;
                                    if (isZooSikCheGyul)
                                    {
                                        ea[nCurIdx].myStrategy.fd[i].fr.nSlotChegyulEndCnt++;

                                        if (ea[nCurIdx].fPowerDiff != 0)
                                        {
                                            ea[nCurIdx].myStrategy.fd[i].fr.nSlotUpDownEndCnt++;
                                            if (ea[nCurIdx].fPowerDiff > 0)
                                            {
                                                ea[nCurIdx].myStrategy.fd[i].fr.fSlotUpEndPower += ea[nCurIdx].fPowerDiff;
                                            }
                                            else
                                            {
                                                ea[nCurIdx].myStrategy.fd[i].fr.fSlotDownEndPower -= ea[nCurIdx].fPowerDiff;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }

                        #endregion

                    }
                }
                #endregion
            }
            #endregion
        }
        #endregion

    } // END ---- MainForm
}
