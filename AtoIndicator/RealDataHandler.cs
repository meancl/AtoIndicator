using System;
using AtoIndicator.DB;
using System.Collections.Generic;
using static AtoIndicator.KiwoomLib.TimeLib;
using static AtoIndicator.KiwoomLib.PricingLib;
using static AtoIndicator.KiwoomLib.Errors;
using static AtoIndicator.TradingBlock.TimeLineGenerator;
using System.Linq;
using static AtoIndicator.Utils.Protractor;
using static AtoIndicator.Utils.Comparer;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace AtoIndicator
{
    public partial class MainForm
    {
        #region 변수

        public Random rand = new Random(); // real

        public const int AI_ONCE_MAXNUM = 5;
        public Queue<AIResponseSlot> aiQueue = new Queue<AIResponseSlot>(); // 매매신청을 담는 큐, 매매컨트롤러가 사용할 큐 // real
        public Queue<TradeRequestSlot> tradeQueue = new Queue<TradeRequestSlot>(); // 매매신청을 담는 큐, 매매컨트롤러가 사용할 큐 // real

        public List<StrategyHistory>[] strategyHistoryList;
        public List<StrategyHistory> totalTradeHistoryList = new List<StrategyHistory>();

        public TradeRequestSlot curSlot; // 임시로 사용하능한 매매요청, 매매컨트롤러 변수 // real
        public AIResponseSlot aiSlot;

        public bool isHoldingsConfirm = false;
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
        public StringBuilder sbPaperSellDescription = new StringBuilder();


        public int nSendNum = 0;
        public int nHourPtr = 1;
        public const int PAPER_HOUR_LIMIT = 3600;
        public const int PAPER_HOUR_PAD = 100;
        public const int PAPER_HOUR_BUY_LIMIT_DIFF = 300;
        public DateTime dFirstForPaper = DateTime.UtcNow;



        #endregion

        #region 상수
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
        }

        public const int FAKE_REQUEST_SIGNAL = -1;
        public const int FAKE_BUY_SIGNAL = 0;
        public const int FAKE_RESIST_SIGNAL = 1;
        public const int FAKE_ASSISTANT_SIGNAL = 2;
        public const int FAKE_VOLATILE_SIGNAL = 7;
        public const int EVERY_SIGNAL = 8;
        public const int FAKE_DOWN_SIGNAL = 9;
        public const int PAPER_BUY_SIGNAL = 10;
        public const int PAPER_SELL_SIGNAL = 11;

        public const int DEFAULT_BOOST_UP_TIME = 5;

        public const int UP_RESERVE = 0;
        public const int DOWN_RESERVE = 1;
        public const int MA_DOWN_RESERVE = 2;
        public const int MA_RESERVE_POSITION_RESERVE = 3;
        public const int MA_UP_RESERVE = 4;
        public const int INIT_RESERVE = 5;

        public const string SEND_ORDER_ERROR_CHECK_PREFIX = "둥둥둥";


        public const int EYES_CLOSE_CRUSH_NUM = 3;

        public const double fPushWeight = 0.8;
        public const double fRoughPushWeight = 0.6;
        public const int SHORT_UPDATE_TIME = 20;
        public const int MAX_REQ_SEC = 130; // 최대매수요청시간
        public const int BUY_CANCEL_ACCESS_SEC = 15;  // 매수취소 가능할때까지 시간

        public const int BAR_FAKE_MAX_NUM = 5; // 한 봉에

        public const int PRICE_UP_MAX_NUM = 200; // 최대 가격up 갯수
        public const int PRICE_DOWN_MAX_NUM = 200; // 최대 가격down 갯수
        public const int FAKE_BUY_MAX_NUM = 200;
        public const int FAKE_RESIST_MAX_NUM = 200;
        public const int FAKE_ASSISTANT_MAX_NUM = 200;
        public const int FAKE_VOLATILITY_MAX_NUM = 200;
        public const int FAKE_DOWN_MAX_NUM = 200;
        public const int PAPER_TRADE_MAX_NUM = 100; // 최대 모의매수 

        public const long SHT_PER_INIT = BILLION;

        public static int COMPUTER_LOCATION = -1;
        public static int SELL_VERSION = 0;
        public static int AI_VERSION = 0;
        public const int TRADE_CONTROLLER_ACCESS_BUY_LIMIT = 10;


        public const int ONE_SEC_MIL_SEC = 1000;
        public const int OVER_FLOW_MIL_SEC_CHECK = 1100;
        public DateTime dOverFlowToUp = DateTime.UtcNow;
        public int nOverFlowCnt = 0;

        public const string HAND_TRADE_SCREEN = "5353";
        #endregion

        #region  실시간이벤트핸들러
        private void OnReceiveRealDataHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            int nCurIdx;

            string sCode = e.sRealKey.Trim(); // 종목코드

            bool isHogaJanRyang = false;
            bool isZooSikCheGyul = false;



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
                    isMarketLabel.Text = $"장시작 : {isMarketStart}";
                    isBuyDeny = false;
                    dFirstForPaper = DateTime.UtcNow;
                    dtBeforeOrderTime = DateTime.UtcNow;
                    nFirstTime = int.Parse(sTime);
                    nFirstTime -= nFirstTime % MINUTE_KIWOOM;
                    nSharedTime = nFirstTime;
                    nPrevBoardUpdateTime = nFirstTime;

                    BlockizeUndisposal();
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
                        isMarketLabel.Text = $"장시작 : {isMarketStart}";
                        PutTradeResultAsync();
                        PutChartResultAsync();
                    }
                }
                return; // 장시작시간 실시간데이터는  여기서 종료
            }
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
                    depositCalcLabel.Text = nCurDepositCalc + "(원)";
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

                            if (ea[nRankIdx].rankSystem.nSummationRanking <= 500) // 500위권 내
                                ea[nRankIdx].rankSystem.nRankHold500++;
                            else
                                ea[nRankIdx].rankSystem.nRankHold500 = 0;

                            if (ea[nRankIdx].rankSystem.nSummationRanking <= 1000) // 1000위권 내
                                ea[nRankIdx].rankSystem.nRankHold1000++;
                            else
                                ea[nRankIdx].rankSystem.nRankHold1000 = 0;
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

                                if (ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs > ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs)
                                {
                                    ea[i].fPositiveStickPower += fCurCandlePower;
                                }
                                else
                                {
                                    ea[i].fNegativeStickPower -= fCurCandlePower;
                                }

                                if (fCurCandlePower >= 0.01)
                                    ea[i].timeLines1m.onePerCandleList.Add((nSharedTime, fCurCandlePower));

                                if (fCurCandlePower >= 0.02)
                                    ea[i].timeLines1m.twoPerCandleList.Add((nSharedTime, fCurCandlePower));

                                if (fCurCandlePower >= 0.03)
                                    ea[i].timeLines1m.threePerCandleList.Add((nSharedTime, fCurCandlePower));

                                if (fCurCandlePower >= 0.04)
                                    ea[i].timeLines1m.fourPerCandleList.Add((nSharedTime, fCurCandlePower));


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
                            int nShareIdx, nSummation, nSummationGap;
                            double fMaVal, fMaGapVal;

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
                            nSummationGap = 0;
                            if (nShareIdx <= 0) // 0보다 작다면 더미데이터가 아니라 전부 리얼데이터로 채울 수 있다는 의미
                            {
                                for (j = nTimeLineIdx; j > nTimeLineIdx - MA20M; j--)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                    if (j < BRUSH)
                                        nSummationGap += ea[i].nYesterdayEndPrice;
                                    else
                                        nSummationGap += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                            }
                            else // 부족하다는 의미
                            {
                                for (j = 0; j <= nTimeLineIdx; j++)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                    if (j < BRUSH)
                                        nSummationGap += ea[i].nYesterdayEndPrice;
                                    else
                                        nSummationGap += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                                for (j = 0; j < nShareIdx; j++)
                                {
                                    nSummation += ea[i].nTodayStartPrice; // 0~ BRUSH -1 까지는 다 같지만 그냥 0번째 데이터를 넣어줌
                                    nSummationGap += ea[i].nYesterdayEndPrice;
                                }
                            }
                            fMaVal = (double)nSummation / MA20M; // 현재의 N이동평균선 값
                            fMaGapVal = (double)nSummationGap / MA20M;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa0 = ea[i].maOverN.fCurMa20m = fMaVal;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMaGap0 = ea[i].maOverN.fCurGapMa1h = fMaGapVal;
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

                            if (ea[i].maOverN.fMaxMa20m < fMaVal) // max ma 20m
                            {
                                ea[i].maOverN.fMaxMa20m = fMaVal;
                                ea[i].maOverN.nMaxMa20mTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fMaxMa20mPower = (fMaVal - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;

                                ea[i].maOverN.fBottomMa20m = fMaVal;
                                ea[i].maOverN.nBottomMa20mTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomMa20mPower = (fMaVal - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;
                            }
                            else if (ea[i].maOverN.fBottomMa20m > fMaVal) // bottom ma 20m 
                            {
                                ea[i].maOverN.fBottomMa20m = fMaVal;
                                ea[i].maOverN.nBottomMa20mTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomMa20mPower = (fMaVal - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;
                            }

                            if (ea[i].maOverN.fMaxGapMa20m < fMaGapVal) // max gap ma 20m 
                            {
                                ea[i].maOverN.fMaxGapMa20m = fMaGapVal;
                                ea[i].maOverN.nMaxGapMa20mTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fMaxGapMa20mPower = (fMaGapVal - ea[i].nYesterdayEndPrice) / ea[i].nYesterdayEndPrice;

                                ea[i].maOverN.fBottomGapMa20m = fMaGapVal;
                                ea[i].maOverN.nBottomGapMa20mTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomGapMa20mPower = (fMaGapVal - ea[i].nYesterdayEndPrice) / ea[i].nYesterdayEndPrice;
                            }
                            else if (ea[i].maOverN.fBottomGapMa20m > fMaGapVal) // bottom gap ma 20m
                            {
                                ea[i].maOverN.fBottomGapMa20m = fMaGapVal;
                                ea[i].maOverN.nBottomGapMa20mTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomGapMa20mPower = (fMaGapVal - ea[i].nYesterdayEndPrice) / ea[i].nYesterdayEndPrice;
                            }

                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownTimeOverMa0 = ea[i].maOverN.nDownCntMa20m;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpTimeOverMa0 = ea[i].maOverN.nUpCntMa20m;


                            // -----------
                            // 60분 이평선
                            nShareIdx = MA1H - nTimeLineIdx - 1;
                            nSummation = 0;
                            nSummationGap = 0;
                            if (nShareIdx <= 0) // 0보다 작다면 더미데이터가 아니라 전부 리얼데이터로 채울 수 있다는 의미
                            {
                                for (j = nTimeLineIdx; j > nTimeLineIdx - MA1H; j--)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                    if (j < BRUSH)
                                        nSummationGap += ea[i].nYesterdayEndPrice;
                                    else
                                        nSummationGap += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                            }
                            else // 부족하다는 의미
                            {
                                for (j = 0; j <= nTimeLineIdx; j++)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                    if (j < BRUSH)
                                        nSummationGap += ea[i].nYesterdayEndPrice;
                                    else
                                        nSummationGap += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                                for (j = 0; j < nShareIdx; j++)
                                {
                                    nSummation += ea[i].nTodayStartPrice; // 0~ BRUSH -1 까지는 다 같지만 그냥 0번째 데이터를 넣어줌
                                    nSummationGap += ea[i].nYesterdayEndPrice; // 0~ BRUSH -1 까지는 다 같지만 그냥 0번째 데이터를 넣어줌
                                }
                            }
                            fMaVal = (double)nSummation / MA1H; // 현재의 N이동평균선 값
                            fMaGapVal = (double)nSummationGap / MA1H; // 현재의 N이동평균선 값
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa1 = ea[i].maOverN.fCurMa1h = fMaVal;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMaGap1 = ea[i].maOverN.fCurGapMa1h = fMaGapVal;
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


                            if (ea[i].maOverN.fMaxMa1h < fMaVal) // max ma 1h
                            {
                                ea[i].maOverN.fMaxMa1h = fMaVal;
                                ea[i].maOverN.nMaxMa1hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fMaxMa1hPower = (fMaVal - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;

                                ea[i].maOverN.fBottomMa1h = fMaVal;
                                ea[i].maOverN.nBottomMa1hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomMa1hPower = (fMaVal - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;
                            }
                            else if (ea[i].maOverN.fBottomMa1h > fMaVal) // bottom ma 1h 
                            {
                                ea[i].maOverN.fBottomMa1h = fMaVal;
                                ea[i].maOverN.nBottomMa1hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomMa1hPower = (fMaVal - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;
                            }

                            if (ea[i].maOverN.fMaxGapMa1h < fMaGapVal) // max gap ma 1h 
                            {
                                ea[i].maOverN.fMaxGapMa1h = fMaGapVal;
                                ea[i].maOverN.nMaxGapMa1hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fMaxGapMa1hPower = (fMaGapVal - ea[i].nYesterdayEndPrice) / ea[i].nYesterdayEndPrice;

                                ea[i].maOverN.fBottomGapMa1h = fMaGapVal;
                                ea[i].maOverN.nBottomGapMa1hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomGapMa1hPower = (fMaGapVal - ea[i].nYesterdayEndPrice) / ea[i].nYesterdayEndPrice;
                            }
                            else if (ea[i].maOverN.fBottomGapMa1h > fMaGapVal) // bottom gap ma 1h
                            {
                                ea[i].maOverN.fBottomGapMa1h = fMaGapVal;
                                ea[i].maOverN.nBottomGapMa1hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomGapMa1hPower = (fMaGapVal - ea[i].nYesterdayEndPrice) / ea[i].nYesterdayEndPrice;
                            }
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownTimeOverMa1 = ea[i].maOverN.nDownCntMa1h;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpTimeOverMa1 = ea[i].maOverN.nUpCntMa1h;



                            // -----------
                            // 120분 이평선
                            nShareIdx = MA2H - nTimeLineIdx - 1;
                            nSummation = 0;
                            nSummationGap = 0;
                            if (nShareIdx <= 0) // 0보다 작다면 더미데이터가 아니라 전부 리얼데이터로 채울 수 있다는 의미
                            {
                                for (j = nTimeLineIdx; j > nTimeLineIdx - MA2H; j--)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                    if (j < BRUSH)
                                        nSummationGap += ea[i].nYesterdayEndPrice;
                                    else
                                        nSummationGap += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                            }
                            else // 부족하다는 의미
                            {
                                for (j = 0; j <= nTimeLineIdx; j++)
                                {
                                    nSummation += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                    if (j < BRUSH)
                                        nSummationGap += ea[i].nYesterdayEndPrice;
                                    else
                                        nSummationGap += ea[i].timeLines1m.arrTimeLine[j].nLastFs;
                                }
                                for (j = 0; j < nShareIdx; j++)
                                {
                                    nSummation += ea[i].nTodayStartPrice; // 0~ BRUSH -1 까지는 다 같지만 그냥 0번째 데이터를 넣어줌
                                    nSummationGap += ea[i].nYesterdayEndPrice; // 0~ BRUSH -1 까지는 다 같지만 그냥 0번째 데이터를 넣어줌
                                }
                            }
                            fMaVal = (double)nSummation / MA2H; // 현재의 N이동평균선 값
                            fMaGapVal = (double)nSummationGap / MA2H; // 현재의 N이동평균선 값
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa2 = ea[i].maOverN.fCurMa2h = fMaVal;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMaGap2 = ea[i].maOverN.fCurGapMa2h = fMaGapVal;
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

                            if (ea[i].maOverN.fMaxMa2h < fMaVal) // max ma 2h
                            {
                                ea[i].maOverN.fMaxMa2h = fMaVal;
                                ea[i].maOverN.nMaxMa2hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fMaxMa2hPower = (fMaVal - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;

                                ea[i].maOverN.fBottomMa2h = fMaVal;
                                ea[i].maOverN.nBottomMa2hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomMa2hPower = (fMaVal - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;
                            }
                            else if (ea[i].maOverN.fBottomMa2h > fMaVal) // bottom ma 2h 
                            {
                                ea[i].maOverN.fBottomMa2h = fMaVal;
                                ea[i].maOverN.nBottomMa2hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomMa2hPower = (fMaVal - ea[i].nTodayStartPrice) / ea[i].nYesterdayEndPrice;
                            }

                            if (ea[i].maOverN.fMaxGapMa2h < fMaGapVal) // max gap ma 2h 
                            {
                                ea[i].maOverN.fMaxGapMa2h = fMaGapVal;
                                ea[i].maOverN.nMaxGapMa2hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fMaxGapMa2hPower = (fMaGapVal - ea[i].nYesterdayEndPrice) / ea[i].nYesterdayEndPrice;

                                ea[i].maOverN.fBottomGapMa2h = fMaGapVal;
                                ea[i].maOverN.nBottomGapMa2hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomGapMa2hPower = (fMaGapVal - ea[i].nYesterdayEndPrice) / ea[i].nYesterdayEndPrice;
                            }
                            else if (ea[i].maOverN.fBottomGapMa2h > fMaGapVal) // bottom gap ma 2h
                            {
                                ea[i].maOverN.fBottomGapMa2h = fMaGapVal;
                                ea[i].maOverN.nBottomGapMa2hTime = ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nTime;
                                ea[i].maOverN.fBottomGapMa2hPower = (fMaGapVal - ea[i].nYesterdayEndPrice) / ea[i].nYesterdayEndPrice;
                            }
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nDownTimeOverMa2 = ea[i].maOverN.nDownCntMa2h;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].nUpTimeOverMa2 = ea[i].maOverN.nUpCntMa2h;


                        }// END ---- 이평선
                        #endregion

                        // 체결정보
                        {
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fTradeCompared = ea[i].fTradeRatioCompared;
                            ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fTradeStrength = ea[i].fTs;
                        }

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

                        #region HIT 초기화
                        // 페이크매수/매도 히트횟수 초기화(*동일분봉동안 가지는 히트횟수)
                        {

                            // 초기화
                            ea[i].paperBuyStrategy.nHitNum = 0;
                            ea[i].fakeBuyStrategy.nHitNum = 0;
                            ea[i].fakeResistStrategy.nHitNum = 0;
                            ea[i].fakeAssistantStrategy.nHitNum = 0;
                            ea[i].fakeVolatilityStrategy.nHitNum = 0;
                            ea[i].fakeDownStrategy.nHitNum = 0;
                            ea[i].fakeStrategyMgr.nCurHitNum = 0;
                            ea[i].fakeStrategyMgr.nCurHitType = 0;
                        }
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

                        #region MA 예약 확인
                        {

                            // MA 역배열
                            if(ea[i].manualReserve.reserveArr[MA_RESERVE_POSITION_RESERVE].isSelected && !ea[i].manualReserve.reserveArr[MA_RESERVE_POSITION_RESERVE].isChosen1)
                            {
                                if (ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa0 <= ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa2 &&
                                    ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa1 <= ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa2
                                    )
                                {
                                    ea[i].manualReserve.reserveArr[MA_RESERVE_POSITION_RESERVE].isChosen1 = true;
                                    ea[i].manualReserve.reserveArr[MA_RESERVE_POSITION_RESERVE].nChosenTime = nSharedTime;
                                }
                            }

                            // MA 다운 
                            if (ea[i].manualReserve.reserveArr[MA_DOWN_RESERVE].isSelected && !ea[i].manualReserve.reserveArr[MA_DOWN_RESERVE].isChosen1)
                            {
                                if (ea[i].nFs <= ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa0 &&
                                    ea[i].nFs <= ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa1 &&
                                    ea[i].nFs <= ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa2 
                                    )
                                {
                                    if (ea[i].manualReserve.reserveArr[MA_DOWN_RESERVE].isBuyReserved)
                                    {
                                        ea[i].manualReserve.reserveArr[MA_DOWN_RESERVE].isBuyReserved = false;
                                        RequestMachineBuy(i, nQty: ea[i].manualReserve.reserveArr[MA_DOWN_RESERVE].nBuyReserveNumStock);
                                    }
                                    ea[i].manualReserve.reserveArr[MA_DOWN_RESERVE].isChosen1 = true;
                                    ea[i].manualReserve.reserveArr[MA_DOWN_RESERVE].nChosenTime = nSharedTime;
                                }
                            }

                            // MA 업 
                            if (ea[i].manualReserve.reserveArr[MA_UP_RESERVE].isSelected && !ea[i].manualReserve.reserveArr[MA_UP_RESERVE].isChosen1)
                            {
                                if (ea[i].nFs >= ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa0 &&
                                    ea[i].nFs >= ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa1 &&
                                    ea[i].nFs >= ea[i].timeLines1m.arrTimeLine[nTimeLineIdx].fOverMa2
                                    )
                                {
                                    if (ea[i].manualReserve.reserveArr[MA_UP_RESERVE].isBuyReserved)
                                    {
                                        ea[i].manualReserve.reserveArr[MA_UP_RESERVE].isBuyReserved = false;
                                        RequestMachineBuy(i, nQty: ea[i].manualReserve.reserveArr[MA_UP_RESERVE].nBuyReserveNumStock);
                                    }
                                    ea[i].manualReserve.reserveArr[MA_UP_RESERVE].isChosen1 = true;
                                    ea[i].manualReserve.reserveArr[MA_UP_RESERVE].nChosenTime = nSharedTime;
                                }
                            }

                        }
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

                    ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScore += fRatio;
                    ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScoreJar += fRatio;
                    ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScoreJarDegree += fRatio;

                    ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScoreJar -= 0.1; // 줄인다.
                    ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScoreJarDegree -= 0.1; // 줄인다.

                    if (ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScoreJar < 0)
                        ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScoreJar = 0;


                    if (ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScore >= 5)
                    {
                        if (ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI5Time == 0)
                        {
                            ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI5Time = nSharedTime;
                            PrintLog($"{nSharedTime} {ea[aiSlot.nEaIdx].sCode} {ea[aiSlot.nEaIdx].sCodeName} AI 5점 달성.", aiSlot.nEaIdx);
                        }
                        if (ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScore >= 10)
                        {
                            if (ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI10Time == 0)
                            {
                                ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI10Time = nSharedTime;
                                PrintLog($"{nSharedTime} {ea[aiSlot.nEaIdx].sCode} {ea[aiSlot.nEaIdx].sCodeName} AI 10점 달성.", aiSlot.nEaIdx);
                            }
                            if (ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScore >= 15)
                            {
                                if (ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI15Time == 0)
                                {
                                    ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI15Time = nSharedTime;
                                    PrintLog($"{nSharedTime} {ea[aiSlot.nEaIdx].sCode} {ea[aiSlot.nEaIdx].sCodeName} AI 15점 달성.", aiSlot.nEaIdx);
                                }
                                if (ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScore >= 20)
                                {
                                    if (ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI20Time == 0)
                                    {
                                        ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI20Time = nSharedTime;
                                        PrintLog($"{nSharedTime} {ea[aiSlot.nEaIdx].sCode} {ea[aiSlot.nEaIdx].sCodeName} AI 20점 달성.", aiSlot.nEaIdx);
                                    }
                                    if (ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScore >= 30)
                                    {
                                        if (ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI30Time == 0)
                                        {
                                            ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI30Time = nSharedTime;
                                            PrintLog($"{nSharedTime} {ea[aiSlot.nEaIdx].sCode} {ea[aiSlot.nEaIdx].sCodeName} AI 30점 달성.", aiSlot.nEaIdx);
                                        }
                                        if (ea[aiSlot.nEaIdx].fakeStrategyMgr.fAIScore >= 50)
                                        {
                                            if (ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI50Time == 0)
                                            {
                                                ea[aiSlot.nEaIdx].fakeStrategyMgr.nAI50Time = nSharedTime;
                                                PrintLog($"{nSharedTime} {ea[aiSlot.nEaIdx].sCode} {ea[aiSlot.nEaIdx].sCodeName} AI 50점 달성.", aiSlot.nEaIdx);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    switch (aiSlot.nRequestId)
                    {
                        case PAPER_BUY_SIGNAL: // 매수 여부 체크

                            if (fRatio >= 0.65)
                            {
                                ea[aiSlot.nEaIdx].fakeStrategyMgr.nAIPassed++;

                                PrintLog($"{nSharedTime} {ea[aiSlot.nEaIdx].sCode} {ea[aiSlot.nEaIdx].sCodeName} 실매수 AI 패스 {ea[aiSlot.nEaIdx].fakeStrategyMgr.nAIPassed}번째, 점수 : {fRatio}", aiSlot.nEaIdx);

                                if (ea[aiSlot.nEaIdx].fakeStrategyMgr.nAIPrevTimeLineIdx != nTimeLineIdx)
                                {
                                    if (nTimeLineIdx - ea[aiSlot.nEaIdx].fakeStrategyMgr.nAIPrevTimeLineIdx > 1) //  2칸 이상 떨어져있을때 
                                        ea[aiSlot.nEaIdx].fakeStrategyMgr.nAIJumpDiffMinuteCount++;
                                    ea[aiSlot.nEaIdx].fakeStrategyMgr.nAIPrevTimeLineIdx = nTimeLineIdx;
                                    ea[aiSlot.nEaIdx].fakeStrategyMgr.nAIStepMinuteCount++;
                                }

                            }
                            break;
                        case FAKE_REQUEST_SIGNAL:
                            if (fRatio > 0.45)
                            {
                                ea[aiSlot.nEaIdx].fakeStrategyMgr.nFakeAccumPassed++;
                                PrintLog($"{nSharedTime} {ea[aiSlot.nEaIdx].sCode} {ea[aiSlot.nEaIdx].sCodeName} 페이크 AI 패스 {ea[aiSlot.nEaIdx].fakeStrategyMgr.nFakeAccumPassed}번째, 점수 : {fRatio}", aiSlot.nEaIdx);
                                if (ea[aiSlot.nEaIdx].fakeStrategyMgr.nFakeAIPrevTimeLineIdx != nTimeLineIdx)
                                {
                                    if (nTimeLineIdx - ea[aiSlot.nEaIdx].fakeStrategyMgr.nFakeAIPrevTimeLineIdx > 1)
                                        ea[aiSlot.nEaIdx].fakeStrategyMgr.nFakeAIJumpDiffMinuteCount++;
                                    ea[aiSlot.nEaIdx].fakeStrategyMgr.nFakeAIPrevTimeLineIdx = nTimeLineIdx;
                                    ea[aiSlot.nEaIdx].fakeStrategyMgr.nFakeAIStepMinuteCount++;
                                }
                            }
                            ea[aiSlot.nEaIdx].fakeStrategyMgr.nFakeAccumTried++;
                            break;

                        case EVERY_SIGNAL:
                            if (fRatio > 0.35)
                            {
                                ea[aiSlot.nEaIdx].fakeStrategyMgr.nEveryAIPassNum++;
                                PrintLog($"{nSharedTime} {ea[aiSlot.nEaIdx].sCode} {ea[aiSlot.nEaIdx].sCodeName} 에브리 AI 패스 {ea[aiSlot.nEaIdx].fakeStrategyMgr.nEveryAIPassNum}번째, 점수 : {fRatio}", aiSlot.nEaIdx);
                            }
                            break;

                        default:
                            break;
                    }
                    TurnOffMMFSlot(aiSlot.nMMFNumber);
                }
#endif
                #endregion

                #region 매매 컨트롤러
                // =============================================================
                // 매매 컨트롤러
                // =============================================================
                if (tradeQueue.Count > 0) // START ---- 매매 컨트롤러
                {
                    #region 주문제한 걸림
#if DEBUG
                    if (isForbidTrade) // 거래정지상태 (주문이 단위시간 한계수량에 도달해서)
                    {
                        dtCurOrderTime = DateTime.UtcNow;
                        if ((dtCurOrderTime - dtBeforeOrderTime).TotalMilliseconds > OVER_FLOW_MIL_SEC_CHECK) // 1초가 지나면 거래 풀어줌.
                        {
                            isForbidTrade = false;
                            dtBeforeOrderTime = dtCurOrderTime;
                            nReqestCount = 0;
                            PrintLog("거래정지상태 풀림");
                        }
                    }
#endif
                    #endregion

                    #region 주문제한 풀림
                    if ((!isForbidTrade) && (nUsingScreenNum < (SCREEN_NUM_LIMIT - SCREEN_NUM_PADDING)))  // 거래정지가 아니라면
                    {
                        curSlot = tradeQueue.Dequeue(); // 우선 디큐한다

                        if (curSlot.isByHand)
                        {
                            if (curSlot.nOrderType == NEW_BUY)
                            {

                                if (curSlot.sHogaGb.Equals(MARKET_ORDER))
                                {

                                }
                                else if (curSlot.sHogaGb.Equals(PENDING_ORDER))
                                {

                                    int nEstimatedPrice = curSlot.nOrderPrice; // 종목의 요청했던 최우선매도호가를 받아온다.

                                    int nNumToBuy = (int)(nCurDepositCalc / (nEstimatedPrice * (1 + KIWOOM_STOCK_FEE))); // 현재 예수금으로 살 수 있을 만큼
                                    int nMaxNumToBuy; // 최대매수가능금액으로 살 수 있을 만큼 

                                    if (curSlot.nQty <= 0)
                                        nMaxNumToBuy = (int)(STANDARD_BUY_PRICE * curSlot.fRequestRatio) / nEstimatedPrice;
                                    else
                                        nMaxNumToBuy = curSlot.nQty;

                                    if (nNumToBuy > nMaxNumToBuy) // 최대매수가능수를 넘는다면
                                        nNumToBuy = nMaxNumToBuy; // 최대매수가능수로 세팅

                                    // 구매수량이 있고 현재종목의 최우선매도호가가 요청하려는 지정가보다 낮을 경우 구매요청을 걸 수 있다.
                                    if (nNumToBuy > 0) // && (ea[curSlot.nEaIdx].nFs < nEstimatedPrice)) // 어차피 매수취소에서 걸린다.
                                    {
                                        int nCurSlotIdx = ea[curSlot.nEaIdx].myTradeManager.arrBuyedSlots.Count;

                                        PrintLog($"{nSharedTime} : {curSlot.sCode}  {ea[curSlot.nEaIdx].sCodeName} 지정가 매수신청 전송", curSlot.nEaIdx);


                                        string sBuyScrNo = HAND_TRADE_SCREEN;
                                        int nBuyReqResult = axKHOpenAPI1.SendOrder($"{SEND_ORDER_ERROR_CHECK_PREFIX}{ORDER_NEW_BUY} {curSlot.nEaIdx}", sBuyScrNo, sAccountNum,
                                            curSlot.nOrderType, curSlot.sCode, nNumToBuy, nEstimatedPrice,
                                            curSlot.sHogaGb, curSlot.sOrgOrderId); // 높은 매도호가에 지정가로 걸어 시장가처럼 사게 한다
                                                                                   // 최우선매도호가보다 높은 가격에 지정가를 걸면 현재매도호가에 구매하게 된다.

                                        if (nBuyReqResult == OP_ERR_NONE) // 요청이 성공하면
                                        {
                                            isSendOrder = true;
                                            ea[curSlot.nEaIdx].myTradeManager.nBuyReqCnt++; // 구매횟수 증가
                                            // nCurDepositCalc -= nNumToBuy * nEstimatedPrice + ea[curSlot.nEaIdx].feeMgr.GetRoughFee(nNumToBuy * nEstimatedPrice);
                                            PrintLog($"{nSharedTime}, {curSlot.sCode}  {ea[curSlot.nEaIdx].sCodeName} 화면번호 : {sBuyScrNo} {curSlot.nOrderPrice}, {nNumToBuy} 지정가 매수신청 전송 성공", curSlot.nEaIdx);
                                        }
                                        else // 요청이 실패했다는것
                                        {
                                            ShutOffScreen(sBuyScrNo);


                                            if (nBuyReqResult == OP_ERR_ORD_OVERFLOW) // 주문전송과부하
                                            {
#if DEBUG
                                                dtCurOrderTime = DateTime.UtcNow;
                                                if ((dtCurOrderTime - dOverFlowToUp).TotalMilliseconds > ONE_SEC_MIL_SEC)
                                                {
                                                    dtBeforeOrderTime = dtCurOrderTime;
                                                    isForbidTrade = true;
                                                    nOverFlowCnt++;
                                                    dOverFlowToUp = dtCurOrderTime;
                                                    PrintLog($"{nSharedTime} 주문전송과부하 카운트 : {nOverFlowCnt}회");
                                                }
#endif
                                                tradeQueue.Enqueue(curSlot);

                                            }
                                            else // 요청 실패
                                            {
                                                PrintLog($"{nSharedTime}, {curSlot.sCode}  {ea[curSlot.nEaIdx].sCodeName},  오류번호 : {nBuyReqResult}  {curSlot.nOrderPrice}(원), {nNumToBuy}(주) 지정가 매수신청 전송 실패!!", curSlot.nEaIdx);
                                            }
                                        }


                                    }
                                    else // 보유금액이 없거나 가격이 너무 올라버린 경우
                                    {
                                        PrintLog($"{nSharedTime} {curSlot.sCode}  {ea[curSlot.nEaIdx].sCodeName} 보유금액이 없거나 가격이 너무 올라서 매수가 불가합니다.", curSlot.nEaIdx);
                                    }

                                }
                            }
                            else if (curSlot.nOrderType == NEW_SELL)
                            {

                                if (curSlot.sHogaGb.Equals(MARKET_ORDER))
                                {

                                }
                                else if (curSlot.sHogaGb.Equals(PENDING_ORDER))
                                {
                                    PrintLog($"{nSharedTime} : {curSlot.sCode} 번째 {ea[curSlot.nEaIdx].sCodeName} 지정가 매도신청 전송", curSlot.nEaIdx);

                                    string sSellScrNo;
                                    sSellScrNo = HAND_TRADE_SCREEN;


                                    int nSellReqResult = axKHOpenAPI1.SendOrder($"{SEND_ORDER_ERROR_CHECK_PREFIX}{ORDER_NEW_SELL} {curSlot.nEaIdx}", sSellScrNo, sAccountNum,
                                            curSlot.nOrderType, curSlot.sCode, curSlot.nQty, curSlot.nOrderPrice,
                                            curSlot.sHogaGb, curSlot.sOrgOrderId);

                                    if (nSellReqResult == OP_ERR_NONE) // 요청이 성공하면
                                    {
                                        isSendOrder = true;
                                        PrintLog($"{curSlot.sCode} {ea[curSlot.nEaIdx].sCodeName}  화면번호 : {sSellScrNo}  지정가 매도신청 전송 성공", curSlot.nEaIdx);
                                    }
                                    else // 요청이 실패했다는 것
                                    {
                                        if (nSellReqResult == OP_ERR_ORD_OVERFLOW) // 주문전송과부화
                                        {
#if DEBUG
                                            dtCurOrderTime = DateTime.UtcNow;
                                            if ((dtCurOrderTime - dOverFlowToUp).TotalMilliseconds > ONE_SEC_MIL_SEC)
                                            {
                                                dtBeforeOrderTime = dtCurOrderTime;
                                                isForbidTrade = true;
                                                nOverFlowCnt++;
                                                dOverFlowToUp = dtCurOrderTime;
                                                PrintLog($"{nSharedTime} 주문전송과부하 카운트 : {nOverFlowCnt}회");
                                            }
#endif
                                            tradeQueue.Enqueue(curSlot);
                                        }
                                        else // 그냥 실패
                                        {
                                            PrintLog($"{curSlot.sCode} {ea[curSlot.nEaIdx].sCodeName}  오류번호 : {nSellReqResult} 매도신청 전송 실패", curSlot.nEaIdx);
                                        }
                                    }

                                }
                            }
                            else if (curSlot.nOrderType == BUY_CANCEL)
                            {
                                if (curSlot.sHogaGb.Equals(MARKET_ORDER))
                                {
                                    if (ea[curSlot.nEaIdx].unhandledBuyOrderIdList.Contains(curSlot.sOrgOrderId)) // 그와중에 매수가 완료되면 매수취소는 삭제된다.
                                    {
                                        PrintLog($"{nSharedTime} : {curSlot.sCode} {ea[curSlot.nEaIdx].sCodeName}  {curSlot.sDescription} 손매수취소신청 전송", curSlot.nEaIdx, curSlot.nBuyedSlotIdx);

                                        int nCancelReqResult = axKHOpenAPI1.SendOrder($"{SEND_ORDER_ERROR_CHECK_PREFIX}{ORDER_BUY_CANCEL} {curSlot.nEaIdx}", HAND_TRADE_SCREEN, sAccountNum,
                                            curSlot.nOrderType, curSlot.sCode, 0, 0,
                                            "", curSlot.sOrgOrderId); // 취소주문수량을 0으로 입력하면 미체결 전량이 취소됩니다.
                                                                      // 취소주문시 주문가격은 필요없으며 취소하려는 주문 그러니까 미체결 주문의 주문번호가 취소주문시 필요한 원주문번호가 됩니다.
                                                                      //마지막으로 취소주문시에는 거래구분(시장가, 지정가 등)값이 사용되지 않습니다.

                                        if (nCancelReqResult == OP_ERR_NONE) // 매수취소 전송이 성공하면
                                        {
                                            isSendOrder = true;
                                            PrintLog($"{nSharedTime} : {curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName} 손매수취소신청 전송 성공", curSlot.nEaIdx);
                                        }
                                        else // 전송이 실패했다는것
                                        {
                                            if (nCancelReqResult == OP_ERR_ORD_OVERFLOW) // 주문전송과부화
                                            {
#if DEBUG
                                                dtCurOrderTime = DateTime.UtcNow;
                                                if ((dtCurOrderTime - dOverFlowToUp).TotalMilliseconds > ONE_SEC_MIL_SEC)
                                                {
                                                    dtBeforeOrderTime = dtCurOrderTime;
                                                    isForbidTrade = true;
                                                    nOverFlowCnt++;
                                                    dOverFlowToUp = dtCurOrderTime;
                                                    PrintLog($"{nSharedTime} 주문전송과부하 카운트 : {nOverFlowCnt}회");
                                                }
#endif
                                                tradeQueue.Enqueue(curSlot);
                                            }
                                            else // 전송이 실패하면
                                            {
                                                PrintLog($"{nSharedTime} : {curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName}  오류번호 : {nCancelReqResult} 손매수취소신청 전송 실패!!", curSlot.nEaIdx, curSlot.nBuyedSlotIdx);
                                            }
                                        }
                                    }
                                }
                                else if (curSlot.sHogaGb.Equals(PENDING_ORDER))
                                {

                                }
                            }
                            else if (curSlot.nOrderType == SELL_CANCEL)
                            {

                                if (curSlot.sHogaGb.Equals(MARKET_ORDER))
                                {
                                    if (ea[curSlot.nEaIdx].unhandledSellOrderIdList.Contains(curSlot.sOrgOrderId))
                                    {
                                        PrintLog($"{nSharedTime} : {curSlot.sCode} {ea[curSlot.nEaIdx].sCodeName}  {curSlot.sDescription} 손매도취소신청 전송", curSlot.nEaIdx, curSlot.nBuyedSlotIdx);

                                        int nCancelReqResult = axKHOpenAPI1.SendOrder($"{SEND_ORDER_ERROR_CHECK_PREFIX}{ORDER_BUY_CANCEL} {curSlot.nEaIdx}", HAND_TRADE_SCREEN, sAccountNum,
                                            curSlot.nOrderType, curSlot.sCode, 0, 0,
                                            "", curSlot.sOrgOrderId); // 취소주문수량을 0으로 입력하면 미체결 전량이 취소됩니다.
                                                                      // 취소주문시 주문가격은 필요없으며 취소하려는 주문 그러니까 미체결 주문의 주문번호가 취소주문시 필요한 원주문번호가 됩니다.
                                                                      //마지막으로 취소주문시에는 거래구분(시장가, 지정가 등)값이 사용되지 않습니다.

                                        if (nCancelReqResult == OP_ERR_NONE) // 매수취소 전송이 성공하면
                                        {
                                            isSendOrder = true;
                                            PrintLog($"{nSharedTime} : {curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName} 손매도취소신청 전송 성공", curSlot.nEaIdx);
                                        }
                                        else // 전송이 실패했다는것
                                        {
                                            if (nCancelReqResult == OP_ERR_ORD_OVERFLOW) // 주문전송과부화
                                            {
#if DEBUG
                                                dtCurOrderTime = DateTime.UtcNow;
                                                if ((dtCurOrderTime - dOverFlowToUp).TotalMilliseconds > ONE_SEC_MIL_SEC)
                                                {
                                                    dtBeforeOrderTime = dtCurOrderTime;
                                                    isForbidTrade = true;
                                                    nOverFlowCnt++;
                                                    dOverFlowToUp = dtCurOrderTime;
                                                    PrintLog($"{nSharedTime} 주문전송과부하 카운트 : {nOverFlowCnt}회");
                                                }
#endif
                                                tradeQueue.Enqueue(curSlot);
                                            }
                                            else // 전송이 실패하면
                                            {
                                                sellCancelingByOrderIdDict.Remove(curSlot.sOrgOrderId);
                                                PrintLog($"{nSharedTime} : {curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName}  오류번호 : {nCancelReqResult} 손매도취소신청 전송 실패!!", curSlot.nEaIdx, curSlot.nBuyedSlotIdx);
                                            }
                                        }
                                    }
                                }
                                else if (curSlot.sHogaGb.Equals(PENDING_ORDER))
                                {

                                }
                            }
                        }
                        else // 기계매매
                        {
                            if (curSlot.nOrderType == NEW_BUY)
                            {
                                if (curSlot.sHogaGb.Equals(PENDING_ORDER)) // 지정가모드 : 시장가로 하면 키움에서 상한가값으로 계산해서 예수금만큼 살 수 가 없다
                                {
                                    if (SubTimeToTimeAndSec(nSharedTime, curSlot.nRqTime) <= TRADE_CONTROLLER_ACCESS_BUY_LIMIT) // 매수요청 사용가능 조건
                                    {
                                        int nEstimatedPrice = curSlot.nOrderPrice; // 종목의 요청했던 최우선매도호가를 받아온다.

                                        int nNumToBuy = (int)(nCurDepositCalc / (nEstimatedPrice * (1 + KIWOOM_STOCK_FEE))); // 현재 예수금으로 살 수 있을 만큼
                                        int nMaxNumToBuy; // 최대매수가능금액으로 살 수 있을 만큼 

                                        if (curSlot.nQty <= 0)
                                            nMaxNumToBuy = (int)(STANDARD_BUY_PRICE * curSlot.fRequestRatio) / nEstimatedPrice;
                                        else
                                            nMaxNumToBuy = curSlot.nQty;

                                        if (nNumToBuy > nMaxNumToBuy) // 최대매수가능수를 넘는다면
                                            nNumToBuy = nMaxNumToBuy; // 최대매수가능수로 세팅

                                        // 구매수량이 있고 현재종목의 최우선매도호가가 요청하려는 지정가보다 낮을 경우 구매요청을 걸 수 있다.
                                        if (nNumToBuy > 0) // && (ea[curSlot.nEaIdx].nFs < nEstimatedPrice)) // 어차피 매수취소에서 걸린다.
                                        {
                                            int nCurSlotIdx = ea[curSlot.nEaIdx].myTradeManager.arrBuyedSlots.Count;

                                            PrintLog($"{nSharedTime} : {curSlot.sCode}  {ea[curSlot.nEaIdx].sCodeName} 매수신청 전송", curSlot.nEaIdx);


                                            string sBuyScrNo = GetScreenNum();
                                            if (sBuyScrNo != null) // 사용가능 화면번호가 있다면
                                            {
                                                int nBuyReqResult = axKHOpenAPI1.SendOrder($"{SEND_ORDER_ERROR_CHECK_PREFIX}{ORDER_NEW_BUY} {curSlot.nEaIdx}", sBuyScrNo, sAccountNum,
                                                    curSlot.nOrderType, curSlot.sCode, nNumToBuy, nEstimatedPrice,
                                                    curSlot.sHogaGb, curSlot.sOrgOrderId); // 높은 매도호가에 지정가로 걸어 시장가처럼 사게 한다
                                                                                           // 최우선매도호가보다 높은 가격에 지정가를 걸면 현재매도호가에 구매하게 된다.

                                                if (nBuyReqResult == OP_ERR_NONE) // 요청이 성공하면
                                                {
                                                    #region 매매블록 준비

                                                    BuyedSlot newSlot = new BuyedSlot(curSlot.nEaIdx);
                                                    newSlot.nRequestTime = curSlot.nRqTime;
                                                    newSlot.nOriginOrderPrice = curSlot.nOrderPrice; // 주문요청금액 설정
                                                    newSlot.nOrderPrice = nEstimatedPrice; // 지정상한가 설정
                                                    newSlot.eTradeMethod = curSlot.eTradeMethod; // 매매방법 설정
                                                    newSlot.nOrderVolume = nNumToBuy;
                                                    newSlot.fTradeRatio = curSlot.fRequestRatio;
                                                    newSlot.sBuyDescription = curSlot.sDescription;
                                                    newSlot.fTradeRatio = curSlot.fRequestRatio;
                                                    newSlot.isBuying = true;

                                                    switch (newSlot.eTradeMethod)
                                                    {
                                                        case TradeMethodCategory.RisingMethod:
                                                            newSlot.fTargetPer = GetNextCeiling(newSlot.nCurLineIdx);
                                                            newSlot.fBottomPer = GetNextFloor(newSlot.nCurLineIdx, TradeMethodCategory.RisingMethod);
                                                            break;
                                                        case TradeMethodCategory.ScalpingMethod:
                                                            newSlot.fTargetPer = GetNextCeiling(newSlot.nCurLineIdx);
                                                            newSlot.fBottomPer = GetNextFloor(newSlot.nCurLineIdx, TradeMethodCategory.ScalpingMethod);
                                                            break;
                                                        case TradeMethodCategory.BottomUpMethod:
                                                            newSlot.fTargetPer = GetNextCeiling(newSlot.nCurLineIdx);
                                                            newSlot.fBottomPer = GetNextFloor(newSlot.nCurLineIdx, TradeMethodCategory.BottomUpMethod);
                                                            break;
                                                        case TradeMethodCategory.FixedMethod:
                                                            newSlot.fTargetPer = DEFAULT_FIXED_CEILING;
                                                            newSlot.fBottomPer = DEFAULT_FIXED_BOTTOM;
                                                            break;
                                                        default:
                                                            break;
                                                    }


                                                    SetSlotInScreen(sBuyScrNo, newSlot);
                                                    #endregion

                                                    isSendOrder = true;
                                                    ea[curSlot.nEaIdx].myTradeManager.nBuyReqCnt++; // 구매횟수 증가
                                                    nCurDepositCalc -= nNumToBuy * nEstimatedPrice + ea[curSlot.nEaIdx].feeMgr.GetRoughBuyFee(nEstimatedPrice, nNumToBuy);
                                                    PrintLog($"{nSharedTime}, {curSlot.sCode}  {ea[curSlot.nEaIdx].sCodeName} 화면번호 : {sBuyScrNo} 매매블록 : {nCurSlotIdx} {curSlot.nOrderPrice}, {nNumToBuy} 매수신청 전송 성공", curSlot.nEaIdx);
                                                }
                                                else // 요청이 실패했다는것
                                                {
                                                    ShutOffScreen(sBuyScrNo);


                                                    if (nBuyReqResult == OP_ERR_ORD_OVERFLOW) // 주문전송과부하
                                                    {
#if DEBUG
                                                        dtCurOrderTime = DateTime.UtcNow;
                                                        if ((dtCurOrderTime - dOverFlowToUp).TotalMilliseconds > ONE_SEC_MIL_SEC)
                                                        {
                                                            dtBeforeOrderTime = dtCurOrderTime;
                                                            isForbidTrade = true;
                                                            nOverFlowCnt++;
                                                            dOverFlowToUp = dtCurOrderTime;
                                                            PrintLog($"{nSharedTime} 주문전송과부하 카운트 : {nOverFlowCnt}회");
                                                        }
#endif
                                                        tradeQueue.Enqueue(curSlot);

                                                    }
                                                    else // 요청 실패
                                                    {
                                                        PrintLog($"{nSharedTime}, {curSlot.sCode}  {ea[curSlot.nEaIdx].sCodeName},  오류번호 : {nBuyReqResult}  {curSlot.nOrderPrice}(원), {nNumToBuy}(주) 매수신청 전송 실패!!", curSlot.nEaIdx);
                                                    }
                                                }
                                            } // END ---- 사용가능 화면번호가 있다면
                                            else // 사용가능 화면번호가 없다면
                                            {
                                                PrintLog($"{nSharedTime} {curSlot.sCode}  {ea[curSlot.nEaIdx].sCodeName} : 할당가능한 화면번호가 없어 매수신청 불가");
                                            }// END ---- 사용가능 화면번호가 없다면



                                        }
                                        else // 보유금액이 없거나 가격이 너무 올라버린 경우
                                        {
                                            PrintLog($"{nSharedTime} {curSlot.sCode}  {ea[curSlot.nEaIdx].sCodeName} 보유금액이 없거나 가격이 너무 올라서 매수가 불가합니다.", curSlot.nEaIdx);
                                        }
                                    }
                                    else // 요청이 10초나 지연된 경우
                                    {
                                    }
                                }
                                else if (curSlot.sHogaGb.Equals(MARKET_ORDER)) // 시장가 매수
                                {
                                    // TODO.
                                }
                            }
                            if (curSlot.nOrderType == NEW_SELL)
                            {

                                if (curSlot.sHogaGb.Equals(MARKET_ORDER)) // 시장가매도
                                {
                                    ea[curSlot.nEaIdx].myTradeManager.arrBuyedSlots[curSlot.nBuyedSlotIdx].nSellRequestTime = curSlot.nRqTime;
                                    ea[curSlot.nEaIdx].myTradeManager.arrBuyedSlots[curSlot.nBuyedSlotIdx].sSellDescription = curSlot.sDescription;

                                    PrintLog($"{nSharedTime} : {curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName} 매도신청 전송", curSlot.nEaIdx);
                                    string sSellScrNo = GetScreenNum();

                                    if (sSellScrNo != null) // 사용가능한 화면번호가 있다면
                                    {
                                        int nSellReqResult = axKHOpenAPI1.SendOrder($"{SEND_ORDER_ERROR_CHECK_PREFIX}{ORDER_NEW_SELL} {curSlot.nEaIdx} {curSlot.nBuyedSlotIdx}", sSellScrNo, sAccountNum,
                                                curSlot.nOrderType, curSlot.sCode, curSlot.nQty, 0,
                                                curSlot.sHogaGb, curSlot.sOrgOrderId);

                                        if (nSellReqResult == OP_ERR_NONE) // 요청이 성공하면
                                        {
                                            isSendOrder = true;
                                            SetSlotInScreen(sSellScrNo, ea[curSlot.nEaIdx].myTradeManager.arrBuyedSlots[curSlot.nBuyedSlotIdx]);
                                            ea[curSlot.nEaIdx].myTradeManager.nSellReqCnt++; // 매도요청전송이 성공하면 매도횟수를 증가한다.

                                            PrintLog($"{curSlot.sCode} {ea[curSlot.nEaIdx].sCodeName}  화면번호 : {sSellScrNo}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName} 매도신청 전송 성공", curSlot.nEaIdx, curSlot.nBuyedSlotIdx);
                                        }
                                        else // 요청이 실패했다는 것
                                        {
                                            ShutOffScreen(sSellScrNo);


                                            if (nSellReqResult == OP_ERR_ORD_OVERFLOW) // 주문전송과부화
                                            {
#if DEBUG
                                                dtCurOrderTime = DateTime.UtcNow;
                                                if ((dtCurOrderTime - dOverFlowToUp).TotalMilliseconds > ONE_SEC_MIL_SEC)
                                                {
                                                    dtBeforeOrderTime = dtCurOrderTime;
                                                    isForbidTrade = true;
                                                    nOverFlowCnt++;
                                                    dOverFlowToUp = dtCurOrderTime;
                                                    PrintLog($"{nSharedTime} 주문전송과부하 카운트 : {nOverFlowCnt}회");
                                                }
#endif
                                                tradeQueue.Enqueue(curSlot);
                                            }
                                            else // 그냥 실패
                                            {
                                                PrintLog($"{curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName}  오류번호 : {nSellReqResult} 매도신청 전송 실패", curSlot.nEaIdx, curSlot.nBuyedSlotIdx);
                                                ea[curSlot.nEaIdx].myTradeManager.arrBuyedSlots[curSlot.nBuyedSlotIdx].isSelling = false;
                                            }
                                        }
                                    } // END ---- 사용가능한 화면번호가 있다면 
                                    else // 사용가능한 화면번호가 없다면
                                    {
                                        PrintLog($"{curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName} 화면번호 할당불가로 매도신청 전송 실패", curSlot.nEaIdx, curSlot.nBuyedSlotIdx);
                                        ea[curSlot.nEaIdx].myTradeManager.arrBuyedSlots[curSlot.nBuyedSlotIdx].isSelling = false;

                                    }// END ---- 사용가능한 화면번호가 없다면 

                                } // END ---- 시장가매도
                                else if (curSlot.sHogaGb.Equals(PENDING_ORDER)) // 지정가매도(손매도)
                                {
                                    // TODO.
                                }

                            }
                            else if (curSlot.nOrderType == BUY_CANCEL)
                            {
                                if (curSlot.sHogaGb.Equals(MARKET_ORDER))
                                {
                                    BuyedSlot slot = buySlotByOrderIdDict[curSlot.sOrgOrderId];

                                    if (!slot.isAllBuyed) // 그와중에 매수가 완료되면 매수취소는 삭제된다.
                                    {
                                        PrintLog($"{nSharedTime} : {curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName}  {curSlot.sDescription} 매수취소신청 전송", curSlot.nEaIdx, curSlot.nBuyedSlotIdx);

                                        int nCancelReqResult = axKHOpenAPI1.SendOrder($"{SEND_ORDER_ERROR_CHECK_PREFIX}{ORDER_BUY_CANCEL} {curSlot.nEaIdx}", slot.sBuyScrNo, sAccountNum,
                                            curSlot.nOrderType, curSlot.sCode, 0, 0,
                                            "", curSlot.sOrgOrderId); // 취소주문수량을 0으로 입력하면 미체결 전량이 취소됩니다.
                                                                      // 취소주문시 주문가격은 필요없으며 취소하려는 주문 그러니까 미체결 주문의 주문번호가 취소주문시 필요한 원주문번호가 됩니다.
                                                                      //마지막으로 취소주문시에는 거래구분(시장가, 지정가 등)값이 사용되지 않습니다.

                                        if (nCancelReqResult == OP_ERR_NONE) // 매수취소 전송이 성공하면
                                        {
                                            isSendOrder = true;
                                            PrintLog($"{nSharedTime} : {curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName} 매수취소신청 전송 성공", curSlot.nEaIdx);
                                        }
                                        else // 전송이 실패했다는것
                                        {
                                            if (nCancelReqResult == OP_ERR_ORD_OVERFLOW) // 주문전송과부화
                                            {
#if DEBUG
                                                dtCurOrderTime = DateTime.UtcNow;
                                                if ((dtCurOrderTime - dOverFlowToUp).TotalMilliseconds > ONE_SEC_MIL_SEC)
                                                {
                                                    dtBeforeOrderTime = dtCurOrderTime;
                                                    isForbidTrade = true;
                                                    nOverFlowCnt++;
                                                    dOverFlowToUp = dtCurOrderTime;
                                                    PrintLog($"{nSharedTime} 주문전송과부하 카운트 : {nOverFlowCnt}회");
                                                }
#endif
                                                tradeQueue.Enqueue(curSlot);
                                            }
                                            else // 전송이 실패하면
                                            {
                                                buyCancelingByOrderIdDict.Remove(curSlot.sOrgOrderId);
                                                PrintLog($"{nSharedTime} : {curSlot.sCode}  {curSlot.nBuyedSlotIdx}번째 {ea[curSlot.nEaIdx].sCodeName}  오류번호 : {nCancelReqResult} 매수취소신청 전송 실패!!", curSlot.nEaIdx, curSlot.nBuyedSlotIdx);
                                            }
                                        }

                                    } // END ---- !ea[curSlot.nEaIdx].myTradeManager.arrBuyedSlots[curSlot.nBuyedSlotIdx].isAllBuyed
                                }
                                else if (curSlot.sHogaGb.Equals(PENDING_ORDER))
                                {
                                    // TODO.
                                }
                            }
                            else if (curSlot.nOrderType == SELL_CANCEL) // 매도취소
                            {
                                if (curSlot.sHogaGb.Equals(MARKET_ORDER))
                                {
                                    // TODO.
                                }
                                else if (curSlot.sHogaGb.Equals(MARKET_ORDER))
                                {
                                    // TODO.
                                }
                            }
                        }


                        #region 주문요청 완료
#if DEBUG
                        if (isSendOrder) // 주문을 요청했을때만
                        {
                            isSendOrder = false;
                            if (nSendNum == 0)
                                dFirstForPaper = DateTime.UtcNow;
                            nSendNum++;
                            PrintLog($"{nSharedTime} 주문성공횟수 : {nSendNum}");

                            dtCurOrderTime = DateTime.UtcNow;
                            if ((nReqestCount == 0) || ((dtCurOrderTime - dtBeforeOrderTime).TotalMilliseconds > OVER_FLOW_MIL_SEC_CHECK)) // 주문시간이 이전주문시간과 다르다 == 1초 제한이 아니다
                            {
                                dtBeforeOrderTime = dtCurOrderTime;
                                nReqestCount = 1;
                            }
                            else  // 주문시간이 이전시간과 같다 == 1초 제한 카운트 증가
                            {
                                nReqestCount++;
                                if (nReqestCount >= (LIMIT_SENDORDER_NUM - LIMIT_SENDORDER_PAD)) // 5번제한이지만 혹시 모르니 4번제한으로
                                {
                                    isForbidTrade = true; // 제한에 걸리면 1초가 지날때까지는 매매 금지
                                }
                            }
                        } // END ---- 주문을 요청했을때만
#endif
                        #endregion

                    } // END ---- 거래정지가 아니라면
                    #endregion
                }

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
#if AI
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        // EVERY AI 요청
                        if (ea[nCurIdx].nHogaCnt % 1000 == 0)
                        {
                            UpFakeCount(nCurIdx, EVERY_SIGNAL, 0);
                        }
                    }
#endif
                    ea[nCurIdx].hogaSpeedStatus.fPush++;

                    try
                    {
                        ea[nCurIdx].nCurHogaPrice = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 41))); // 매도호가1

                        s1 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 61))); // 매도1호가잔량
                        s2 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 62))); // 매도2호가잔량
                        s3 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 63))); // 매도3호가잔량

                        s4 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 64))); // 매도4호가잔량
                        b4 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 74))); // 매수4호가잔량

                        if (s4 == 0 && b4 == 0)
                        {
                            if (Math.Abs(ea[nCurIdx].fPower) > 0.05 || (Math.Abs(ea[nCurIdx].fPowerJar) > 0.01 && (ea[nCurIdx].nChegyulCnt > 1000 || ea[nCurIdx].nStopHogaCnt > 1000)))  // 정확한 vi 기준을 몰라 안좋은 종목들은 걸러내면 진짜 vi만 남을듯
                            {
                                if (!ea[nCurIdx].isViMode)
                                {
                                    ea[nCurIdx].nViStartTime = nSharedTime;
                                    ea[nCurIdx].nViTimeLineIdx = nTimeLineIdx;
                                    ea[nCurIdx].nViFs = ea[nCurIdx].nFs;
                                    ea[nCurIdx].nViCnt++;
                                }

                                ea[nCurIdx].nPrevSpeedUpdateTime = nSharedTime;
                                ea[nCurIdx].nPrevPowerUpdateTime = nSharedTime;
                                ea[nCurIdx].isViMode = true;
                            }
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

                        double fTradeRatioCompared = Math.Abs(double.Parse(axKHOpenAPI1.GetCommRealData(sCode, 30))); // 전일거래량대비(비율)
                        ea[nCurIdx].fTradeRatioCompared = fTradeRatioCompared;

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
                                            nBirthPrice: ea[nCurIdx].nTodayStartPrice,
                                            nYesterdayPrice: ea[nCurIdx].nYesterdayEndPrice
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
                    {
                        ea[nCurIdx].lFirstPrice = ea[nCurIdx].nFs * ea[nCurIdx].nTv;
                        ea[nCurIdx].nFirstVolume = ea[nCurIdx].nTv;
                        return;
                    }


                    // 맥스값
                    if (ea[nCurIdx].nRealMaxPrice < ea[nCurIdx].nFs)
                        ea[nCurIdx].nRealMaxPrice = ea[nCurIdx].nFs;




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


                    // 오늘 저점
                    if (ea[nCurIdx].nTodayMinPrice == 0 || ea[nCurIdx].nTodayMinPrice > ea[nCurIdx].nFs)
                    {
                        ea[nCurIdx].nTodayMinPrice = ea[nCurIdx].nFs;
                        ea[nCurIdx].nTodayMinTime = nSharedTime;
                        ea[nCurIdx].fTodayMinPower = ea[nCurIdx].fPower;
                    }

                    // 오늘 고점
                    if (ea[nCurIdx].nTodayMaxPrice < ea[nCurIdx].nFs)
                    {
                        ea[nCurIdx].nTodayMaxPrice = ea[nCurIdx].nFs;
                        ea[nCurIdx].nTodayMaxTime = nSharedTime;
                        ea[nCurIdx].fTodayMaxPower = ea[nCurIdx].fPower;

                        ea[nCurIdx].nTodayBottomPrice = ea[nCurIdx].nTodayMaxPrice;
                        ea[nCurIdx].nTodayBottomTime = ea[nCurIdx].nTodayMaxTime;
                        ea[nCurIdx].fTodayBottomPower = ea[nCurIdx].fTodayMaxPower;
                    }
                    // 오늘 고점 후 저점
                    else if (ea[nCurIdx].nTodayBottomPrice > ea[nCurIdx].nFs)
                    {
                        ea[nCurIdx].nTodayBottomPrice = ea[nCurIdx].nFs;
                        ea[nCurIdx].nTodayBottomTime = nSharedTime;
                        ea[nCurIdx].fTodayBottomPower = ea[nCurIdx].fPower;
                    }

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
                        {
                            ea[nCurIdx].fAccumUpPower += ea[nCurIdx].fPowerDiff;
                            ea[nCurIdx].nAccumUpCnt++;
                        }
                        else
                        {
                            ea[nCurIdx].fAccumDownPower -= ea[nCurIdx].fPowerDiff;
                            ea[nCurIdx].nAccumDownCnt++;
                        }
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
                        ea[nCurIdx].priceMoveStatus.Commit(fPushWeight);
                        ea[nCurIdx].priceUpMoveStatus.Commit(fPushWeight);

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
                    ea[nCurIdx].tradeStatus.fPush += Math.Abs((double)ea[nCurIdx].nTv * ea[nCurIdx].nFs) / HUNDRED_MILLION;
                    ea[nCurIdx].pureTradeStatus.fPush += ((double)ea[nCurIdx].nTv * ea[nCurIdx].nFs) / HUNDRED_MILLION;
                    if (ea[nCurIdx].nTv > 0)
                        ea[nCurIdx].pureBuyStatus.fPush += ((double)ea[nCurIdx].nTv * ea[nCurIdx].nFs) / HUNDRED_MILLION;
                    if (ea[nCurIdx].fPowerDiff != 0)
                    {
                        ea[nCurIdx].priceMoveStatus.fPush++;
                        if (ea[nCurIdx].fPowerDiff > 0)
                            ea[nCurIdx].priceUpMoveStatus.fPush++;
                    }


                    ea[nCurIdx].speedStatus.Update(fTimePassedPushWeight);
                    ea[nCurIdx].tradeStatus.Update(fTimePassedPushWeight);
                    ea[nCurIdx].pureTradeStatus.Update(fTimePassedPushWeight);
                    ea[nCurIdx].pureBuyStatus.Update(fTimePassedPushWeight);
                    ea[nCurIdx].priceMoveStatus.Update(fTimePassedPushWeight);
                    ea[nCurIdx].priceUpMoveStatus.Update(fTimePassedPushWeight);


                    if (ea[nCurIdx].speedStatus.fCur >= 150)
                    {
                        if (ea[nCurIdx].sequenceStrategy.nSpeed150TotalPrevTime != nSharedTime)
                        {
                            ea[nCurIdx].sequenceStrategy.nSpeed150TotalPrevTime = nSharedTime;
                            ea[nCurIdx].sequenceStrategy.nSpeed150TotalSec++;
                        }

                        if (ea[nCurIdx].sequenceStrategy.nSpeed150CurPrevTime != nSharedTime)
                        {
                            ea[nCurIdx].sequenceStrategy.nSpeed150CurPrevTime = nSharedTime;
                            ea[nCurIdx].sequenceStrategy.nSpeed150CurSec++;
                        }
                    }
                    else
                    {
                        ea[nCurIdx].sequenceStrategy.nSpeed150CurSec = 0;
                        ea[nCurIdx].sequenceStrategy.nSpeed150CurPrevTime = 0;
                    }

                    if (ea[nCurIdx].hogaRatioStatus.fCur > 0.2)
                    {
                        if (ea[nCurIdx].sequenceStrategy.nHogaGoodTotalPrevTime != nSharedTime)
                        {
                            ea[nCurIdx].sequenceStrategy.nHogaGoodTotalPrevTime = nSharedTime;
                            ea[nCurIdx].sequenceStrategy.nHogaGoodTotalSec++;
                        }

                        if (ea[nCurIdx].sequenceStrategy.nHogaGoodCurPrevTime != nSharedTime)
                        {
                            ea[nCurIdx].sequenceStrategy.nHogaGoodCurPrevTime = nSharedTime;
                            ea[nCurIdx].sequenceStrategy.nHogaGoodCurSec++;
                        }
                    }
                    else
                    {
                        ea[nCurIdx].sequenceStrategy.nHogaGoodCurPrevTime = 0;
                        ea[nCurIdx].sequenceStrategy.nHogaGoodCurSec = 0;
                    }


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

                    #region HIT 관리
                    {
                        int nTotalHitNum = ea[nCurIdx].paperBuyStrategy.nHitNum
                                          + ea[nCurIdx].fakeBuyStrategy.nHitNum
                                          + ea[nCurIdx].fakeResistStrategy.nHitNum
                                          + ea[nCurIdx].fakeAssistantStrategy.nHitNum
                                          + ea[nCurIdx].fakeVolatilityStrategy.nHitNum
                                          + ea[nCurIdx].fakeDownStrategy.nHitNum;
                        // 초기화 전 데이터 관리
                        ea[nCurIdx].fakeStrategyMgr.nCurHitNum = nTotalHitNum;
                        ea[nCurIdx].fakeStrategyMgr.nCurHitType = 0;

                        if (ea[nCurIdx].paperBuyStrategy.nHitNum > 0) // 실매수
                            ea[nCurIdx].fakeStrategyMgr.nCurHitType++;
                        if (ea[nCurIdx].fakeBuyStrategy.nHitNum > 0) // 페이크 매수
                            ea[nCurIdx].fakeStrategyMgr.nCurHitType++;
                        if (ea[nCurIdx].fakeResistStrategy.nHitNum > 0) // 페이크 저항
                            ea[nCurIdx].fakeStrategyMgr.nCurHitType++;
                        if (ea[nCurIdx].fakeAssistantStrategy.nHitNum > 0) // 페이크 보조
                            ea[nCurIdx].fakeStrategyMgr.nCurHitType++;
                        if (ea[nCurIdx].fakeVolatilityStrategy.nHitNum > 0) // 페이크 업
                            ea[nCurIdx].fakeStrategyMgr.nCurHitType++;
                        if (ea[nCurIdx].fakeDownStrategy.nHitNum > 0) // 페이크 다운
                            ea[nCurIdx].fakeStrategyMgr.nCurHitType++;

                        if (ea[nCurIdx].fakeStrategyMgr.nCurHitType >= 2 && ea[nCurIdx].fakeStrategyMgr.nCurHitNum >= 5)
                        {
                            if (!ea[nCurIdx].fakeStrategyMgr.hitDict25.ContainsKey(nTimeLineIdx)) // 없다면
                            {
                                ea[nCurIdx].fakeStrategyMgr.hitDict25[nTimeLineIdx] = ea[nCurIdx].nFs;
                            }
                        }

                        if (ea[nCurIdx].fakeStrategyMgr.nCurHitType >= 3 && ea[nCurIdx].fakeStrategyMgr.nCurHitNum >= 8)
                        {
                            if (!ea[nCurIdx].fakeStrategyMgr.hitDict38.ContainsKey(nTimeLineIdx)) // 없다면
                            {
                                ea[nCurIdx].fakeStrategyMgr.hitDict38[nTimeLineIdx] = ea[nCurIdx].nFs;
                            }
                        }

                        if (ea[nCurIdx].fakeStrategyMgr.nCurHitType >= 3 && ea[nCurIdx].fakeStrategyMgr.nCurHitNum >= 12)
                        {
                            if (!ea[nCurIdx].fakeStrategyMgr.hitDict312.ContainsKey(nTimeLineIdx)) // 없다면
                            {
                                ea[nCurIdx].fakeStrategyMgr.hitDict312[nTimeLineIdx] = ea[nCurIdx].nFs;
                            }
                        }

                        if (ea[nCurIdx].fakeStrategyMgr.nCurHitType >= 4 && ea[nCurIdx].fakeStrategyMgr.nCurHitNum >= 10)
                        {
                            if (!ea[nCurIdx].fakeStrategyMgr.hitDict410.ContainsKey(nTimeLineIdx)) // 없다면
                            {
                                ea[nCurIdx].fakeStrategyMgr.hitDict410[nTimeLineIdx] = ea[nCurIdx].nFs;
                            }
                        }
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


                    #region 실시간 manual crush 테스트(예약)
                    {
                        if (ea[nCurIdx].manualReserve.reserveArr[UP_RESERVE].isSelected && !ea[nCurIdx].manualReserve.reserveArr[UP_RESERVE].isChosen1)
                        {
                            if (ea[nCurIdx].nFs >= ea[nCurIdx].manualReserve.reserveArr[UP_RESERVE].fCritLine1)
                            {
                                if (ea[nCurIdx].manualReserve.reserveArr[UP_RESERVE].isBuyReserved)
                                {
                                    ea[nCurIdx].manualReserve.reserveArr[UP_RESERVE].isBuyReserved = false;
                                    RequestMachineBuy(nCurIdx, nQty: ea[nCurIdx].manualReserve.reserveArr[UP_RESERVE].nBuyReserveNumStock);
                                }
                                ea[nCurIdx].manualReserve.reserveArr[UP_RESERVE].isChosen1 = true;
                                ea[nCurIdx].manualReserve.reserveArr[UP_RESERVE].nChosenTime = nSharedTime;
                            }
                        }
                        if (ea[nCurIdx].manualReserve.reserveArr[DOWN_RESERVE].isSelected && !ea[nCurIdx].manualReserve.reserveArr[DOWN_RESERVE].isChosen1)
                        {
                            if (ea[nCurIdx].nFs <= ea[nCurIdx].manualReserve.reserveArr[DOWN_RESERVE].fCritLine1)
                            {
                                if (ea[nCurIdx].manualReserve.reserveArr[DOWN_RESERVE].isBuyReserved)
                                {
                                    ea[nCurIdx].manualReserve.reserveArr[DOWN_RESERVE].isBuyReserved = false;
                                    RequestMachineBuy(nCurIdx, nQty: ea[nCurIdx].manualReserve.reserveArr[DOWN_RESERVE].nBuyReserveNumStock);
                                }
                                ea[nCurIdx].manualReserve.reserveArr[DOWN_RESERVE].isChosen1 = true;
                                ea[nCurIdx].manualReserve.reserveArr[DOWN_RESERVE].nChosenTime = nSharedTime;
                            }
                        }
                    }
                    #endregion

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
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && //분당 10억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                    )
                                {
                                    SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역1 
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 3 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && //분당 30억이상
                                     ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                     ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                     )
                                {
                                    SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역2
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 2 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && //분당 20억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                    )
                                {
                                    SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역3
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 5 * HUNDRED_MILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 5억이상
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
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 10분내 분당 매수대금 5억이상 매수 > 매도 시가총액100위이상 5번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역4
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 10억이상
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역5
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 20억이상
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역6
                                if (
                                ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 10억이상
                                ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime6.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime6.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime6.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 5분내 분당 매수대금 10억이상 매수 > 매도 시가총액100위이상 4번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역7
                                if (
                               ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 3 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 30억이상
                               ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                               )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime7.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime7.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime7.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 5분내 분당 거래대금 30억이상 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();

                            { // 가짜매수 구역8
                                if (
                                   ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 1.5 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 15억이상
                                   ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime8.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime8.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime8.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 15분내 분당 매수대금 15억이상 매수 > 매도 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역9
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 20억이상
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime9.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime9.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime9.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 15분내 분당 매수대금 20억이상 매수 > 매도 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역10
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 10억이상
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime10.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime10.Count >= 5)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime10.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 15분내 분당 매수대금 10억이상 매수 > 매도 시가총액100위이상 5번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역11
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 2 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 거래대금 20억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime11.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime11.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime11.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 15분내 분당 거래대금 20억이상 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역12
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 3 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 거래대금 30억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime12.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime12.Count >= 2)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime12.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 15분내 분당 거래대금 30억이상 시가총액100위이상 2번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역13
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > 4 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 거래대금 40억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime13.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime13.Count >= 2)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime13.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 15분내 분당 거래대금 40억이상 시가총액100위이상 2번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역14
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lTotalPrice > BILLION * 1.5 + GetHeavyPrice(ea[nCurIdx].lMarketCap) && //분당 15억이상
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                    ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                   )
                                {
                                    SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역15
                                if (
                               ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 1.5 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 15억이상
                               ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                               )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime15.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime15.Count >= 5)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime15.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 30분내 분당 매수대금 15억이상 매수 > 매도 시가총액100위이상 5번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역16
                                if (
                                   //  분당 매수대금 15억이상 매수 > 매도 시가총액100위이상..1분제한
                                   ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 1.5 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 15억이상
                                   ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                    )
                                {
                                    SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer);
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역17
                                if (
                                ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 20억이상
                                ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime17.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime17.Count >= 2)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime17.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 6분내 분당 매수대금 20억이상 매수 > 매도 시가총액100위이상 2번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역18
                                if (
                                 ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 1.5 * BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 15억이상
                                 ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime18.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime18.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime18.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 10분내 분당 매수대금 15억이상 매수 > 매도 시가총액100위이상 3번 이상..리사이클
                                    }
                                }
                            }
                            FakeBuyPointerMove();
                            { // 가짜매수 구역19
                                if (
                                ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > BILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) && // 분당 매수대금 10억이상
                                ((ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] == 0) || (ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                )
                                {
                                    ea[nCurIdx].fakeBuyStrategy.arrPrevMinuteIdx[nFakeBuyStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeBuyStrategy.listApproachTime19.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeBuyStrategy.listApproachTime19.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeBuyStrategy.listApproachTime19.Clear();
                                        SetThisFake(ea[nCurIdx].fakeBuyStrategy, nCurIdx, nFakeBuyStrategyPointer); // 10분내 분당 매수대금 10억이상 매수 > 매도 시가총액100위이상 3번 이상..리사이클
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
                                    SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역1
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumUpPower + ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].fAccumDownPower >= 2 &&
                                     ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                     )
                                {
                                    SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
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
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
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
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역4
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nCount >= 1000 + GetHeavyCount(ea[nCurIdx].lMarketCap) &&
                                    ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                    )
                                {
                                    SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역5
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nCount >= 600 + GetHeavyCount(ea[nCurIdx].lMarketCap) &&
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
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역6
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nCount >= 600 + GetHeavyCount(ea[nCurIdx].lMarketCap) &&
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime6.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime6.Count >= 5)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime6.Clear();
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역7
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 5 * HUNDRED_MILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) &&
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역8
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 5 * HUNDRED_MILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) &&
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime8.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime8.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime8.Clear();
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역9
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * HUNDRED_MILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) &&
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime9.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime9.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime9.Clear();
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역10
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > 2 * HUNDRED_MILLION + GetHeavyPrice(ea[nCurIdx].lMarketCap) &&
                                    ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lBuyPrice > ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].lSellPrice &&
                                 ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                 )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime10.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime10.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime10.Clear();
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역11
                                if (ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking > 0 &&
                                    ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking <= 2 &&
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime11.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime11.Count >= 3)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime11.Clear();
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역12
                                if (ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking > 0 &&
                                    ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking <= 5 &&
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime12.Add(nSharedTime);


                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime12.Count >= 4)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime12.Clear();
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역13
                                if (ea[nCurIdx].rankSystem.arrRanking[ea[nCurIdx].nRealDataIdxVi].nMinuteRanking == 1 &&
                                  ((ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] == 0) || (ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] + 0 < nTimeLineIdx))  // 1분 제한 
                                  )
                                {
                                    ea[nCurIdx].fakeAssistantStrategy.arrPrevMinuteIdx[nFakeAssistantStrategyPointer] = nTimeLineIdx;
                                    ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime13.Add(nSharedTime);

                                    if (ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime13.Count >= 2)
                                    {
                                        ea[nCurIdx].fakeAssistantStrategy.listApproachAssistantTime13.Clear();
                                        SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                    }
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역14
                                if (ea[nCurIdx].crushMinuteManager.isCrushRealTimeCheck)
                                {
                                    SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
                                }
                            }
                            FakeAssistantPointerMove();
                            { // 가짜보조 구역15
                                if (ea[nCurIdx].crushMinuteManager.isCrushCheck)
                                {
                                    SetThisFake(ea[nCurIdx].fakeAssistantStrategy, nCurIdx, nFakeAssistantStrategyPointer);
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
                                        SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
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
                                        SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
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
                                        SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
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
                                        SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
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
                                        SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
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
                                        SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역6
                                if (
                                  (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nMaxFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nUpFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.01 &&
                                  (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                  )
                                {
                                    SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역7
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nMaxFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nUpFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.02 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();

                            { // 가짜저항 구역8
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.05 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();
                            { // 가짜저항 구역9
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.04 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
                                }
                            }

                            FakeResistPointerMove();
                            { // 가짜저항 구역10
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.04 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();
                            { // 가짜저항 구역11
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].nRealDataIdxVi].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.03 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
                                }
                            }
                            FakeResistPointerMove();
                            { // 가짜저항 구역12
                                if (
                                   (double)(ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[nTimeLineIdx].nStartFs) / ea[nCurIdx].nYesterdayEndPrice >= 0.03 &&
                                   (ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] == 0 || ea[nCurIdx].fakeResistStrategy.arrPrevMinuteIdx[nFakeResistStrategyPointer] + 0 < nTimeLineIdx)
                                   )
                                {
                                    SetThisFake(ea[nCurIdx].fakeResistStrategy, nCurIdx, nFakeResistStrategyPointer);
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
                    }// END ---- 가짜저항
                    #endregion

                    #region 페이크 변동성
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        try
                        {
                            int nFakeVolatilityStrategyPointer = 0;

                            void FakeVolatilityPointerMove()
                            {
                                nFakeVolatilityStrategyPointer++;
                            }

                            bool TestPriceDiff(int nDiffMinuteNum, double fDiffPower)
                            {
                                return ea[nCurIdx].timeLines1m.nRealDataIdx >= BRUSH + nDiffMinuteNum - 1 && (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nRealDataIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nRealDataIdx - nDiffMinuteNum].nLastFs) / ea[nCurIdx].nYesterdayEndPrice > fDiffPower;
                            }



                            // 변동성 구역# 차분 5 0.03 5분주기
                            if (TestPriceDiff(nDiffMinuteNum: 5, fDiffPower: 0.03) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 5)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 5 0.05 5분주기
                            if (TestPriceDiff(nDiffMinuteNum: 5, fDiffPower: 0.05) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 5)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 10 0.03 11분주기
                            if (TestPriceDiff(nDiffMinuteNum: 10, fDiffPower: 0.03) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 11)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 10 0.04 6분주기
                            if (TestPriceDiff(nDiffMinuteNum: 10, fDiffPower: 0.04) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 6)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 20 0.05 13분주기
                            if (TestPriceDiff(nDiffMinuteNum: 20, fDiffPower: 0.05) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 13)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 20 0.04 15분주기
                            if (TestPriceDiff(nDiffMinuteNum: 20, fDiffPower: 0.04) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 15)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 15 0.04 12분주기
                            if (TestPriceDiff(nDiffMinuteNum: 15, fDiffPower: 0.04) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 12)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();


                            // 변동성 구역# 차분 5 0.07 8분주기
                            if (TestPriceDiff(nDiffMinuteNum: 5, fDiffPower: 0.07) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 8)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 3 0.05 9분주기
                            if (TestPriceDiff(nDiffMinuteNum: 3, fDiffPower: 0.05) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 9)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();


                            // 변동성 구역# 차분 4 0.04 10분주기
                            if (TestPriceDiff(nDiffMinuteNum: 4, fDiffPower: 0.04) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 10)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();


                            // 변동성 구역# 차분 37 0.04 21분주기
                            if (TestPriceDiff(nDiffMinuteNum: 37, fDiffPower: 0.04) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 21)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();


                            // 변동성 구역# 차분 35 0.06 23분주기
                            if (TestPriceDiff(nDiffMinuteNum: 35, fDiffPower: 0.06) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 23)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 30 0.04 30분주기
                            if (TestPriceDiff(nDiffMinuteNum: 30, fDiffPower: 0.04) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 30)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 30 0.03 26분주기
                            if (TestPriceDiff(nDiffMinuteNum: 30, fDiffPower: 0.03) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 26)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 7 0.04 20분주기
                            if (TestPriceDiff(nDiffMinuteNum: 7, fDiffPower: 0.04) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 20)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();


                            // 변동성 구역# 차분 23 0.045 17분주기
                            if (TestPriceDiff(nDiffMinuteNum: 23, fDiffPower: 0.045) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 17)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 20 0.05 16분주기
                            if (TestPriceDiff(nDiffMinuteNum: 20, fDiffPower: 0.05) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 16)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();


                            // 변동성 구역# 차분 13 0.033 12분주기
                            if (TestPriceDiff(nDiffMinuteNum: 13, fDiffPower: 0.033) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 12)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();



                            // 변동성 구역# 차분 20 0.03 14분주기
                            if (TestPriceDiff(nDiffMinuteNum: 20, fDiffPower: 0.03) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 14)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 36 0.06 22분주기
                            if (TestPriceDiff(nDiffMinuteNum: 36, fDiffPower: 0.06) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 22)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                            // 변동성 구역# 차분 22 0.06 14분주기
                            if (TestPriceDiff(nDiffMinuteNum: 22, fDiffPower: 0.06) &&
                                 GetAccess(ea[nCurIdx].fakeVolatilityStrategy, nFakeVolatilityStrategyPointer, nCycle: 14)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeVolatilityStrategy, nCurIdx, nFakeVolatilityStrategyPointer);
                            }
                            FakeVolatilityPointerMove();

                        }
                        catch  // 혹시 내 실수로 STRATEGY_NUM을 초과한 전략을 세울 수 도 있으니까
                        {

                        }
                        finally
                        {
                            ea[nCurIdx].fakeVolatilityStrategy.isSuddenBoom = false;
                        }
                    }// END ---- 전략매수
                    #endregion

                    #region 페이크 다운
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        try
                        {
                            int nFakeDownStrategyPointer = 0;

                            void FakeDownPointerMove()
                            {
                                nFakeDownStrategyPointer++;
                            }

                            bool TestPriceDiff(int nDiffMinuteNum, double fDiffPower)
                            {
                                return ea[nCurIdx].timeLines1m.nRealDataIdx >= BRUSH + nDiffMinuteNum - 1 && (double)(ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nRealDataIdx].nLastFs - ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nRealDataIdx - nDiffMinuteNum].nLastFs) / ea[nCurIdx].nYesterdayEndPrice < (-1) * fDiffPower;
                            }

                            // 페이크 다운# 차분 1 -0.025 1분주기
                            if (TestPriceDiff(nDiffMinuteNum: 1, fDiffPower: 0.025) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 1)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                            // 페이크 다운# 차분 5 -0.03 5분주기
                            if (TestPriceDiff(nDiffMinuteNum: 5, fDiffPower: 0.03) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 5)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                            // 페이크 다운# 차분 8 -0.04 8분주기
                            if (TestPriceDiff(nDiffMinuteNum: 8, fDiffPower: 0.04) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 8)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                            // 페이크 다운# 차분 12 -0.05 10분주기
                            if (TestPriceDiff(nDiffMinuteNum: 12, fDiffPower: 0.05) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 10)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                            // 페이크 다운# 차분 14 -0.035 12분주기
                            if (TestPriceDiff(nDiffMinuteNum: 14, fDiffPower: 0.035) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 12)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                            // 페이크 다운# 차분 15 -0.025 14분주기 
                            if (TestPriceDiff(nDiffMinuteNum: 15, fDiffPower: 0.025) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 14)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                            // 페이크 다운# 차분 20 -0.04 15분주기
                            if (TestPriceDiff(nDiffMinuteNum: 20, fDiffPower: 0.04) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 15)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                            // 페이크 다운# 차분 22 -0.045 20분주기 
                            if (TestPriceDiff(nDiffMinuteNum: 22, fDiffPower: 0.045) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 20)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();


                            // 페이크 다운# 차분 17 -0.025 15분주기 
                            if (TestPriceDiff(nDiffMinuteNum: 17, fDiffPower: 0.025) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 15)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                            // 페이크 다운# 차분 15 -0.03 9분주기 
                            if (TestPriceDiff(nDiffMinuteNum: 15, fDiffPower: 0.03) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 9)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();


                            // 페이크 다운# 차분 3 -0.033 7분주기 
                            if (TestPriceDiff(nDiffMinuteNum: 3, fDiffPower: 0.033) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 7)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                            // 페이크 다운# 차분 13 -0.036 20분주기 
                            if (TestPriceDiff(nDiffMinuteNum: 13, fDiffPower: 0.036) &&
                                 GetAccess(ea[nCurIdx].fakeDownStrategy, nFakeDownStrategyPointer, nCycle: 20)
                               )
                            {
                                SetThisFake(ea[nCurIdx].fakeDownStrategy, nCurIdx, nFakeDownStrategyPointer);
                            }
                            FakeDownPointerMove();

                        }
                        catch  // 혹시 내 실수로 STRATEGY_NUM을 초과한 전략을 세울 수 도 있으니까
                        {

                        }
                        finally
                        {
                            ea[nCurIdx].fakeDownStrategy.isSuddenBoom = false;
                        }
                    }// END ---- 페이크다운
                    #endregion

                    #region 페이크정보 취합
                    //=====================================================
                    // 실매수 전 Update
                    //=====================================================
                    {
                        //if (ea[nCurIdx].fakeStrategyMgr.listFakeHistoryPiece.Count > 0)
                        //{
                        //    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.listFakeHistoryPiece[0].nSharedTime) > 900)
                        //    {
                        //        UpdateFakeHistory(nCurIdx);
                        //        CalcFakeHistory(nCurIdx);
                        //    }
                        //}
                    }
                    #endregion

                    #region 실매수
                    //=====================================================
                    // 전략매수, 실매수 Part
                    //=====================================================
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        try
                        {
                            // 전략 0번째는 추가매수의 슬롯이다
                            int nPaperBuyStrategyPointer = 0;
                            void PaperBuyPointerMove()
                            {
                                nPaperBuyStrategyPointer++;
                            }

                            { // 실매수 구역 5분전 갭포함 6퍼 .. 단한번
                                if (nSharedTime < AddTimeBySec(nFirstTime, 300) &&
                                    ea[nCurIdx].fPower >= 0.06 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                  )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 10분전 갭포함 8.5퍼 .. 단한번
                                if (nSharedTime < AddTimeBySec(nFirstTime, 600) &&
                                    ea[nCurIdx].fPower >= 0.085 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                  )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 5퍼돌파후 전고점(매매:전고점) .. 11분 주기
                                if (ea[nCurIdx].crushMinuteManager.isCrushCheck &&
                                    ea[nCurIdx].sequenceStrategy.isFiveReachedReal &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 p7-m7>=15 .. 11분 주기
                                if (ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 15 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                               )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 p7-m7>=25 .. 11분 주기
                                if (ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 25 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 p7+m7>=30 and p7-m7>=15 .. 11분 주기
                                if (ea[nCurIdx].fPlusCnt07 + ea[nCurIdx].fMinusCnt07 >= 30 &&
                                    ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 15 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 p7+m7>=50 and p7-m7>=15 .. 11분 주기
                                if (ea[nCurIdx].fPlusCnt07 + ea[nCurIdx].fMinusCnt07 >= 50 &&
                                   ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 15 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                               )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 p9+m9>=50 and p9-m9>=15 .. 11분 주기
                                if (ea[nCurIdx].fPlusCnt09 + ea[nCurIdx].fMinusCnt09 >= 50 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 15 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 p9+m9>=70 and p9-m9>=15 .. 11분 주기
                                if (ea[nCurIdx].fPlusCnt09 + ea[nCurIdx].fMinusCnt09 >= 70 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 15 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 p9+m9>=90 and p9-m9>=10 .. 11분 주기
                                if (ea[nCurIdx].fPlusCnt09 + ea[nCurIdx].fMinusCnt09 >= 90 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 10 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 p9+m9>=90 and p9-m9>=20 .. 11분 주기
                                if (ea[nCurIdx].fPlusCnt09 + ea[nCurIdx].fMinusCnt09 >= 90 &&
                                    ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 20 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 p9-m9>=30 .. 11분 주기
                                if (ea[nCurIdx].fPlusCnt09 - ea[nCurIdx].fMinusCnt09 >= 30 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                  )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 파워자 2퍼 .. 11분 주기
                                if (ea[nCurIdx].fPowerJar >= 0.02 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 파워자 3퍼 .. 11분 주기
                                if (ea[nCurIdx].fPowerJar >= 0.03 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 파워자 4퍼 .. 11분 주기
                                if (ea[nCurIdx].fPowerJar >= 0.04 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 총 순위 1위 .. 11분 주기
                                if (ea[nCurIdx].rankSystem.nSummationRanking == 1 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 현재 분당 파워 순위 1위 .. 11분 주기
                                if (ea[nCurIdx].rankSystem.nMinutePowerRanking == 1 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 총 순위 2위 .. 11분 주기
                                if (ea[nCurIdx].rankSystem.nSummationRanking == 2 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 현재 분당 파워 순위 2위 .. 11분 주기
                                if (ea[nCurIdx].rankSystem.nMinutePowerRanking == 2 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 분당 속도 1000이상 p7-m7>= 15 .. 11분 주기
                                if (ea[nCurIdx].timeLines1m.arrTimeLine[ea[nCurIdx].timeLines1m.nPrevTimeLineIdx].nCount >= 1000 &&
                                     ea[nCurIdx].fPlusCnt07 - ea[nCurIdx].fMinusCnt07 >= 15 &&
                                     GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                      )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 전고점 총순위 30위 이전(매매:전고점) .. 11분 주기
                                if (ea[nCurIdx].crushMinuteManager.isCrushCheck &&
                                    ea[nCurIdx].rankSystem.nSummationRanking > 0 &&
                                    ea[nCurIdx].rankSystem.nSummationRanking <= 30 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 전고점 총순위 10위 이전(매매:전고점) .. 11분 주기
                                if (ea[nCurIdx].crushMinuteManager.isCrushCheck &&
                                    ea[nCurIdx].rankSystem.nSummationRanking > 0 &&
                                    ea[nCurIdx].rankSystem.nSummationRanking <= 10 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                     )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 분당 순위 1위 .. 11분 주기
                                if (ea[nCurIdx].rankSystem.nMinuteSummationRanking == 1 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 R각도 50도 이상 .. 11분 주기
                                if (ea[nCurIdx].timeLines1m.fRecentMedianAngle >= 50 &&
                                    GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();


                            { // 실매수 구역 botUp 421 .. 반복
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute421.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute421.CheckIsRedundancy()
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 432 .. 반복
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute432.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute432.CheckIsRedundancy()
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 642 .. 반복
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute642.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute642.CheckIsRedundancy()
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 643 .. 반복
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute643.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute643.CheckIsRedundancy()
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 732 .. 반복
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute732.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute732.CheckIsRedundancy()
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 743 .. 반복
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute743.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute743.CheckIsRedundancy()
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 953 .. 반복
                                if (
                                    ea[nCurIdx].sequenceStrategy.botUpMinute953.isM3Passed &&
                                    !ea[nCurIdx].sequenceStrategy.botUpMinute953.CheckIsRedundancy()
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 421 전고점 일점돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute421.isJumped
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 432 전고점 일점돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute432.isJumped
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 642 전고점 일점돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute642.isJumped
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 643 전고점 일점돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute643.isJumped
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 732 전고점 일점돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute732.isJumped
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 743 전고점 일점돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute743.isJumped
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 953 전고점 일점돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute953.isJumped
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 421 전고점 돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute421.isCrushed
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 432 전고점 돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute432.isCrushed
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 642 전고점 돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute642.isCrushed
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 643 전고점 돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute643.isCrushed
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 732 전고점 돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute732.isCrushed
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 743 전고점 돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute743.isCrushed
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 botUp 953 전고점 돌파 .. 반복
                                if (
                                        ea[nCurIdx].sequenceStrategy.botUpMinute953.isCrushed
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer, methodCategory: TradeMethodCategory.BottomUpMethod);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 갭제외 +6.5퍼 .. 단한번
                                if (ea[nCurIdx].fPowerWithoutGap >= 0.065 &&
                                        GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                     )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 갭제외 +8퍼 .. 단한번
                                if (ea[nCurIdx].fPowerWithoutGap >= 0.08 &&
                                        GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 갭제외 +11퍼 .. 단한번
                                if (ea[nCurIdx].fPowerWithoutGap >= 0.11 &&
                                        GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 onlyUpPowerJar 4퍼 .. 11분주기
                                if (ea[nCurIdx].fOnlyUpPowerJar >= 0.04 &&
                                     GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer, nCycle: 11)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 9시 30분 전 7퍼 상승 .. 단한번
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 1800) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.07 &&
                                     GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 9시 30분 전 10퍼 상승 .. 단한번
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 1800) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.1 &&
                                     GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 9시 30분 전 12퍼 상승 .. 단한번
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 1800) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.12 &&
                                     GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 10시 전 8퍼 상승 .. 단한번
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 3600) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.08 &&
                                     GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 10시 전 12퍼 상승 .. 단한번
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 3600) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.12 &&
                                     GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                            { // 실매수 구역 10시 전 15퍼 상승 .. 단한번
                                if (nSharedTime <= AddTimeBySec(nFirstTime, 3600) &&
                                    ea[nCurIdx].fPowerWithoutGap >= 0.15 &&
                                     GetAccess(ea[nCurIdx].paperBuyStrategy, nPaperBuyStrategyPointer)
                                    )
                                {
                                    SetThisPaperBuy(ea[nCurIdx].paperBuyStrategy, nCurIdx, nPaperBuyStrategyPointer);
                                }
                            }
                            PaperBuyPointerMove();

                        }
                        catch  // 혹시 내 실수로 STRATEGY_NUM을 초과한 전략을 세울 수 도 있으니까
                        {

                        }
                        finally
                        {
                            ea[nCurIdx].paperBuyStrategy.isSuddenBoom = false;
                            ea[nCurIdx].paperBuyStrategy.isOrderCheck = false;
                        }
                    }// END ---- 전략매수

                    #endregion

                    ea[nCurIdx].crushMinuteManager.isCrushCheck = false;
                    ea[nCurIdx].crushMinuteManager.isCrushRealTimeCheck = false;

                    ea[nCurIdx].sequenceStrategy.botUpMinute421.Confirm();
                    ea[nCurIdx].sequenceStrategy.botUpMinute432.Confirm();
                    ea[nCurIdx].sequenceStrategy.botUpMinute642.Confirm();
                    ea[nCurIdx].sequenceStrategy.botUpMinute643.Confirm();
                    ea[nCurIdx].sequenceStrategy.botUpMinute732.Confirm();
                    ea[nCurIdx].sequenceStrategy.botUpMinute743.Confirm();
                    ea[nCurIdx].sequenceStrategy.botUpMinute953.Confirm();

#if AI
                    if (nSharedTime < BAN_BUY_TIME)
                    {
                        // EVERY AI 요청
                        if (ea[nCurIdx].nChegyulCnt % 1000 == 0 || (ea[nCurIdx].fPowerDiff > 0 && ea[nCurIdx].nAccumUpCnt % 250 == 0))
                        {
                            if (ea[nCurIdx].nChegyulCnt % 1000 == 0)
                                UpFakeCount(nCurIdx, EVERY_SIGNAL, 1);
                            else
                                UpFakeCount(nCurIdx, EVERY_SIGNAL, 2);
                        }
                    }
#endif

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

                            for (int i = 0; i < ea[nCurIdx].fakeStrategyMgr.fd.Count; i++)
                            {
                                if (nSharedTime < SHUTDOWN_TIME || ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nFinalPrice == 0)
                                {
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nPriceAfter1Sec == 0 || AddTimeBySec(ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime, 2) >= nSharedTime)
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nPriceAfter1Sec = ea[nCurIdx].nFb;

                                    #region 시간 내 가격 추격
                                    // 2분 내
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n2MinPrice == 0 || SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 120)
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n2MinPrice = ea[nCurIdx].nFb;
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.f2MinPower = ea[nCurIdx].fPower;
                                    }

                                    // 3분 내
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n3MinPrice == 0 || SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 180)
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n3MinPrice = ea[nCurIdx].nFb;
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.f3MinPower = ea[nCurIdx].fPower;
                                    }

                                    // 5분 내
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n5MinPrice == 0 || SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 300)
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n5MinPrice = ea[nCurIdx].nFb;
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.f5MinPower = ea[nCurIdx].fPower;
                                    }

                                    // 10분 내
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n10MinPrice == 0 || SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 600)
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n10MinPrice = ea[nCurIdx].nFb;
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.f10MinPower = ea[nCurIdx].fPower;
                                    }

                                    // 15분 내
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n15MinPrice == 0 || SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 900)
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n15MinPrice = ea[nCurIdx].nFb;
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.f15MinPower = ea[nCurIdx].fPower;
                                    }

                                    // 20분 내
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n20MinPrice == 0 || SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 1200)
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n20MinPrice = ea[nCurIdx].nFb;
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.f20MinPower = ea[nCurIdx].fPower;
                                    }

                                    // 30분 내
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n30MinPrice == 0 || SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 1800)
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n30MinPrice = ea[nCurIdx].nFb;
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.f30MinPower = ea[nCurIdx].fPower;
                                    }

                                    // 50분 내
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n50MinPrice == 0 || SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 3000)
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.n50MinPrice = ea[nCurIdx].nFb;
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.f50MinPower = ea[nCurIdx].fPower;
                                    }
                                    #endregion

                                    ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nFinalPrice = ea[nCurIdx].nFb;

                                    int nOverPrice = ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nOverPrice;
                                    // 거래하고 3시전까지 실시간으로
                                    ea[nCurIdx].fakeStrategyMgr.fd[i].maxMinRealTilThree.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                    // 거래하고 3시전까지 분봉으로(바로 처음 타임라인인덱스는 사기전을 가리키기 때문에 접근못한다)
                                    if (ea[nCurIdx].fakeStrategyMgr.fd[i].nTimeLineIdx < ea[nCurIdx].timeLines1m.nRealDataIdx) // nBuyMinuteIdx가 N일때 nRealDataIdx는 N -1 , 사고 난 다음분봉 데이터부터 기록
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].maxMinMinuteTilThree.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                        if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 600)
                                            ea[nCurIdx].fakeStrategyMgr.fd[i].maxMinMinuteTilThreeWhile10.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                        if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 1800)
                                            ea[nCurIdx].fakeStrategyMgr.fd[i].maxMinMinuteTilThreeWhile30.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);
                                    }

                                    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 600) // 거래하고 10분정도만
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].maxMinRealWhile10.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 1800) // 거래하고 30분정도만
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].maxMinRealWhile30.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);

                                    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nRqTime) <= 3600) // 거래하고 1시간정도만
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].maxMinRealWhile60.CheckMaxMin(nSharedTime, ea[nCurIdx].nFb, ea[nCurIdx].nFb, nOverPrice, nOverPrice);


                                    #region 슬리피지용 변수 기록
                                    ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nNoMoveCntAfterCheck = ea[nCurIdx].nNoMoveCount - ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nNoMoveCnt;
                                    ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nFewSpeedCntAfterCheck = ea[nCurIdx].nFewSpeedCount - ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nFewSpeedCnt;
                                    ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nMissCntAfterCheck = ea[nCurIdx].nMissCount - ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nMissCnt;
                                    ea[nCurIdx].fakeStrategyMgr.fd[i].fr.lTotalTradePriceAfterCheck = ea[nCurIdx].lTotalTradePrice - ea[nCurIdx].fakeStrategyMgr.fd[i].fr.lTotalTradePrice;
                                    ea[nCurIdx].fakeStrategyMgr.fd[i].fr.lTotalBuyPriceAfterCheck = ea[nCurIdx].lOnlyBuyPrice - ea[nCurIdx].fakeStrategyMgr.fd[i].fr.lTotalBuyPrice;
                                    ea[nCurIdx].fakeStrategyMgr.fd[i].fr.lTotalSellPriceAfterCheck = ea[nCurIdx].lOnlySellPrice - ea[nCurIdx].fakeStrategyMgr.fd[i].fr.lTotalSellPrice;
                                    #endregion

                                    if (isHogaJanRyang)
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nHogaCntAfterCheck++;

                                    if (isZooSikCheGyul)
                                    {
                                        ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nChegyulCntAfterCheck++;

                                        if (ea[nCurIdx].fPowerDiff != 0)
                                        {
                                            ea[nCurIdx].fakeStrategyMgr.fd[i].fr.nUpDownCntAfterCheck++;
                                            if (ea[nCurIdx].fPowerDiff > 0)
                                            {
                                                ea[nCurIdx].fakeStrategyMgr.fd[i].fr.fUpPowerAfterCheck += ea[nCurIdx].fPowerDiff;
                                            }
                                            else
                                            {
                                                ea[nCurIdx].fakeStrategyMgr.fd[i].fr.fDownPowerAfterCheck -= ea[nCurIdx].fPowerDiff;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }


                        #endregion

                        #region 모의 매매장
                        if (isZooSikCheGyul)
                        {
                            for (int i = 0; i < ea[nCurIdx].paperBuyStrategy.nStrategyNum; i++)
                            {
                                if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume == ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nTargetRqVolume) // 다 사졌다면
                                {
                                    if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume > 0)
                                    {
                                        if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].isAllSelled)
                                        {
                                            // 할일 없을듯?
                                        }
                                        else if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].isSelling)
                                        {
                                            if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqCount + 10 < ea[nCurIdx].nChegyulCnt)
                                            {
                                                if (ea[nCurIdx].nFb <= ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqPrice)
                                                {
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyHogaVolume -= Math.Abs(ea[nCurIdx].nTv);
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndPrice = Min(ea[nCurIdx].nFb, ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqPrice);

                                                    if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyHogaVolume < 0)
                                                    {
                                                        if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndVolume == 0)
                                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndVolume -= ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyHogaVolume;
                                                        else
                                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndVolume += Math.Abs(ea[nCurIdx].nTv);
                                                        if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndVolume >= ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqVolume)
                                                        {
                                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqVolume;
                                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndTime = nSharedTime;
                                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndTimeLineIdx = nTimeLineIdx;
                                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].isAllSelled = true;
                                                            ea[nCurIdx].paperBuyStrategy.isPaperBuyChangeNeeded = true;

                                                        }

                                                    }
                                                }
                                                else if (ea[nCurIdx].nFb > ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqPrice)
                                                {
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndPrice = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqPrice;
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqVolume;
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndTime = nSharedTime;
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndTimeLineIdx = nTimeLineIdx;
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].isAllSelled = true;
                                                    ea[nCurIdx].paperBuyStrategy.isPaperBuyChangeNeeded = true;
                                                }

                                            }
                                        }
                                        else //  판매중도 판매완료도 아닌 상황
                                        {
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].fPowerWithFee = GetProfitPercent(ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedPrice * ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume, ea[nCurIdx].nFb * ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume, ea[nCurIdx].nMarketGubun) / 100; // 수수료 포함 손익율

                                            SetThisPaperSell(ea[nCurIdx].paperBuyStrategy, nCurIdx, i);
                                        }
                                    }
                                }
                                else // 전부 체결이 안됐다면
                                {
                                    if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nRqTime) >= MAX_REQ_SEC) // 시간 지나 매수취소
                                    {
                                        // 남은 잔량만큼 매수취소
                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nCanceledVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nRqVolume - ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume;
                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nTargetRqVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume;
                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedPrice = Max(ea[nCurIdx].nFs, ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nOverPrice);
                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyEndTime = nSharedTime;
                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedTimeLineIdx = nTimeLineIdx;
                                        ea[nCurIdx].paperBuyStrategy.isPaperBuyChangeNeeded = true;

                                        if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume == 0)
                                        {
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].isAllSelled = true;
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndPrice = Max(ea[nCurIdx].nFs, ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nOverPrice);
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume;
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndTime = nSharedTime;
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndTimeLineIdx = nTimeLineIdx;
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqTime = nSharedTime;
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqTimeLineIdx = nTimeLineIdx;
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].sSellDescription = "시간초과 전량 매수취소";
                                            ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedTimeLineIdx = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyRqTimeLineIdx;
                                        }
                                    }
                                    else // 시간이 안지남
                                    {
                                        if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nRqCount + 10 < ea[nCurIdx].nChegyulCnt) // 매수신청하고 10체결이 지나야함
                                        {
                                            if (ea[nCurIdx].nFs == ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nOverPrice) // 가격이 같다면
                                            {

                                                ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellHogaVolume -= ea[nCurIdx].nTv;
                                                ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedPrice = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nOverPrice;

                                                if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellHogaVolume < 0)
                                                {
                                                    if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume == 0)
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume -= ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellHogaVolume;
                                                    else
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume += ea[nCurIdx].nTv;

                                                    if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume >= ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nTargetRqVolume) // 다 사졌다
                                                    {
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nTargetRqVolume;
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyEndTime = nSharedTime;
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedTimeLineIdx = nTimeLineIdx;
                                                        ea[nCurIdx].paperBuyStrategy.isPaperBuyChangeNeeded = true;
                                                    }
                                                }

                                            }
                                            else if (ea[nCurIdx].nFs < ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nOverPrice) // 현재 체결대금보다 내 가격이 높다면
                                            {
                                                ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedPrice = ea[nCurIdx].nFs;

                                                ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nTargetRqVolume;
                                                ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyEndTime = nSharedTime;
                                                ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedTimeLineIdx = nTimeLineIdx;
                                                ea[nCurIdx].paperBuyStrategy.isPaperBuyChangeNeeded = true;
                                            }
                                            else // 현재 체결대금보다 내 가격이 낮다면
                                            {
                                                if (SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nRqTime) >= BUY_CANCEL_ACCESS_SEC)
                                                {
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nCanceledVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nRqVolume - ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume;
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nTargetRqVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume;
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedPrice = ea[nCurIdx].nFs;
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyEndTime = nSharedTime;
                                                    ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedTimeLineIdx = nTimeLineIdx;

                                                    ea[nCurIdx].paperBuyStrategy.isPaperBuyChangeNeeded = true;

                                                    // 남은 잔량만큼 매수취소
                                                    if (ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume == 0)
                                                    {
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].isAllSelled = true;
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndPrice = ea[nCurIdx].nFs;
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndVolume = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedVolume;
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndTime = nSharedTime;
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqTime = nSharedTime;
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellRqTimeLineIdx = nTimeLineIdx;
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nSellEndTimeLineIdx = nTimeLineIdx;
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].sSellDescription = "가격상승 전량 매수취소";
                                                        ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyedTimeLineIdx = ea[nCurIdx].paperBuyStrategy.paperTradeSlot[i].nBuyRqTimeLineIdx;
                                                    }
                                                }
                                            }


                                        }

                                    }
                                }
                            }
                        }
                        #endregion

                        #region 매매 블록 기록
                        int nBuySlotIdx = ea[nCurIdx].myTradeManager.arrBuyedSlots.Count; // 매수블록갯수
                        if (nBuySlotIdx > 0) // 보유종목이 있다면 추가매수를 할것인지 분할매도를 할것인지 전량매도를 할것인 지 등등을 결정해야함.
                        {
                            for (int checkSellIterIdx = 0; checkSellIterIdx < nBuySlotIdx; checkSellIterIdx++) // 매매블록갯수만큼 반복함
                            {
                                if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isAllBuyed) // 구매가 완료됐다면( 매수취소로 전량 매수취소인경우 또한 기록함)
                                {

                                    // 매도 취소
                                    if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isSellCancelReserved &&
                                        SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nSellCancelReserveTime) >= 15
                                      )
                                    {
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isSellCancelReserved = false;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nSellCancelReserveTime = 0;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isSelling = false;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isSellStarted = false;
                                        PrintLog($"[그룹핑 대기후 종료] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} {checkSellIterIdx}블록 {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurVolume} 잔량", nCurIdx, checkSellIterIdx);
                                    }

                                    // START ---- 기록영역
                                    // 매매가 종료됐어도 기록은 함


                                    int nRecordBuyPrice;

                                    if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nBuyVolume > 0) // 매매된 정보만
                                        nRecordBuyPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nBuyPrice;
                                    else
                                        nRecordBuyPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nOrderPrice; // 전량매수취소됐다면 상향주문가로 측정한다.


                                    // END ---- 기록영역 

                                    // if (!ea[nCurIdx].isExcluded)  // 개인종목이 제외되지 않았으면 매매 가능 강제장종료가 되도 매도는 해야지..
                                    {
                                        // ===================================================================================
                                        // -----------------------------------------------------------------
                                        // Also 대응의 영역
                                        if (!ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isAllSelled &&
                                            !ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].isSelling &&
                                            ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurVolume > 0 && // 매수만 돼있을때

                                            SubTimeToTimeAndSec(nSharedTime, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nSellErrorLastTime) >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nSellErrorCount / 2 + 1
                                          )
                                        {
                                            ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee = GetProfitPercent(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nBuyedSumPrice, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nBuyVolume * ea[nCurIdx].nFb, ea[nCurIdx].nMarketGubun) / 100; // 수수료 포함 손익율


                                            //////////////////////////////////////////////////////////////////////////////////
                                            /////  가격변동, 시간변동, 가격가속도, 시간가속도, 호가속도, 체결속도, 체결량, 호가매수매도대비, vi_cnt  등등 고려해서 ( 가능성을 봐야함 .. ) 
                                            /////  매니징한다. ex) 더 일찍 판다던가 팔지않고 기다린다던가 아니면 추가매수를 한다던가 방식
                                            ////////////////////////////////////////////////////////////////////////////////////
                                            /// 대응의 영역
                                            { // START ---- 대응의 영역
                                                // 상한가 도달했을때
                                                bool isSell = HandleTradeLine(nCurIdx, checkSellIterIdx);
                                            } // END---- 대응의 영역
                                        }  // END ---- 매수만 돼있을때 
                                    } // END ---- 제외되지 않았다면( 장마감이후에는 접근 불가란 소리) isExcluded
                                } // END ---- 구매가 완료됐다면

                            } // END ---- 매매블록 반복적 확인 종료


                        } // END ---- 보유종목이 있다면 ( 적어도 하나의 매매블록이 있다면 )
                        #endregion

                        for (int unIdx = 0; unIdx < ea[nCurIdx].unhandledBuyOrderIdList.Count; unIdx++)
                        {
                            string sOrderId = ea[nCurIdx].unhandledBuyOrderIdList[unIdx];

                            if (!buySlotByOrderIdDict.ContainsKey(sOrderId)) // 주문번호가 없다면 삭제
                            {
                                ea[nCurIdx].unhandledBuyOrderIdList.RemoveAt(unIdx--);
                                continue;
                            }

                            BuyedSlot slot = buySlotByOrderIdDict[sOrderId];

                            if (!slot.isBuyByHand && slot.isResponsed && !buyCancelingByOrderIdDict.ContainsKey(sOrderId)) // 취소 가능하다면
                            {
                                // nFb를 기준으로 지정상한가를 만드니 일정시간동안은 가격이 더 높았어도 버틸 예정
                                if (SubTimeToTimeAndSec(nSharedTime, slot.nRequestTime) >= BUY_CANCEL_ACCESS_SEC)
                                {
                                    // 현재 최우선매도호가가 지정상한가를 넘었거나 매매 요청시간과 현재시간이 너무 오래 차이난다면(= 매수가 너무 오래걸린다 = 거래량이 낮고 머 별거 없다)
                                    if ((ea[nCurIdx].nFs > slot.nOrderPrice) || (SubTimeToTimeAndSec(nSharedTime, slot.nRequestTime) >= MAX_REQ_SEC)) // 지정가를 초과하거나 오래걸린다면
                                    {
                                        string sRq;
                                        if ((ea[nCurIdx].nFs > slot.nOrderPrice))
                                            sRq = "가격초과 매수취소";
                                        else
                                            sRq = "시간초과 매수취소";

                                        SetAndServeCurSlot(false, BUY_CANCEL, nCurIdx, ea[nCurIdx].sCode, MARKET_ORDER, 0, 0, sRq, sOrderId, sRq);


                                        PrintLog($"{nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName}  {sRq} 매수취소신청", nCurIdx);
                                    }
                                }
                            }
                        } // END ---- 구매가 완료되지 않았다면

                    }
                }
                #endregion
            }
            else // 장시작 전
            {
                if (isHogaJanRyang && nFirstTime == 0)
                {
                    nCurIdx = eachStockDict[sCode]; // 오류 가능원인 1. 해당종목코드가 리스트에 없다 2. sCode가  이상한거다
                    if (nCurIdx == INIT_CODEIDX_NUM)
                        throw new Exception();

                    ea[nCurIdx].nStopHogaCnt++;

                    int s1 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(sCode, 41))); // 매도호가1
                    if (s1 == 0)
                        return;


                    if (ea[nCurIdx].nStopFirstPrice == 0)
                    {
                        ea[nCurIdx].nStopFirstPrice = s1;
                        ea[nCurIdx].nStopPrevPrice = s1;
                        ea[nCurIdx].nStopMaxPrice = s1;
                        ea[nCurIdx].nStopMinPrice = s1;
                        ea[nCurIdx].fStopMaxPower = (double)(ea[nCurIdx].nStopMaxPrice - ea[nCurIdx].nStopFirstPrice) / ((ea[nCurIdx].nYesterdayEndPrice == 0) ? ea[nCurIdx].nStopFirstPrice : ea[nCurIdx].nYesterdayEndPrice);
                        ea[nCurIdx].fStopMinPower = (double)(ea[nCurIdx].nStopMinPrice - ea[nCurIdx].nStopFirstPrice) / ((ea[nCurIdx].nYesterdayEndPrice == 0) ? ea[nCurIdx].nStopFirstPrice : ea[nCurIdx].nYesterdayEndPrice);
                    }
                    else
                    {
                        if (ea[nCurIdx].nStopPrevPrice != s1)
                        {
                            ea[nCurIdx].nStopUpDownCnt++;
                            ea[nCurIdx].nStopPrevPrice = s1;
                        }
                    }

                    if (ea[nCurIdx].nStopMaxPrice < s1) // max
                    {
                        ea[nCurIdx].nStopMaxPrice = s1;
                        ea[nCurIdx].fStopMaxPower = (double)(ea[nCurIdx].nStopMaxPrice - ea[nCurIdx].nStopFirstPrice) / ((ea[nCurIdx].nYesterdayEndPrice == 0) ? ea[nCurIdx].nStopFirstPrice : ea[nCurIdx].nYesterdayEndPrice);
                    }

                    if (ea[nCurIdx].nStopMinPrice > s1) // min
                    {
                        ea[nCurIdx].nStopMinPrice = s1;
                        ea[nCurIdx].fStopMinPower = (double)(ea[nCurIdx].nStopMinPrice - ea[nCurIdx].nStopFirstPrice) / ((ea[nCurIdx].nYesterdayEndPrice == 0) ? ea[nCurIdx].nStopFirstPrice : ea[nCurIdx].nYesterdayEndPrice);
                    }

                    ea[nCurIdx].fStopMaxMinDiff = ea[nCurIdx].fStopMaxPower - ea[nCurIdx].fStopMinPower;
                }
            }

        }
        #endregion
    }
    #endregion

} // END ---- MainForm

