namespace AtoIndicator
{
    public partial class MainForm
    {

        #region 상수

        #region 매매요청
        // ------------------------------------------------------
        // 매매요청 상수
        // ------------------------------------------------------
        public const int IGNORE_REQ_SEC = 10; // 요청무시용 seconds 변수

        public const int NEW_BUY = 1; // 신규매수
        public const int NEW_SELL = 2; // 신규매도
        public const int BUY_CANCEL = 3; // 매수취소
        public const int SELL_CANCEL = 4; // 매도취소
        public const int BUY_UPDATE = 5; // 매수정정
        public const int SELL_UPDATE = 6; // 매도정정

        public const string ORDER_NEW_BUY = "신규매수";
        public const string ORDER_NEW_SELL = "신규매도";
        public const string ORDER_BUY_CANCEL = "매수취소";
        public const string ORDER_SELL_CANCEL = "매도취소";
        public const string ORDER_BUY_UPDATE = "매수정정";
        public const string ORDER_SELL_UPDATE = "매도정정";


        public const string PENDING_ORDER = "00"; // 지정가주문
        public const string MARKET_ORDER = "03"; // 시장가주문

        public const int LIMIT_SENDORDER_NUM = 5;
        #if DEBUG 
        public const int LIMIT_SENDORDER_PAD = 1;
        #else
        public const int LIMIT_SENDORDER_PAD = 0;
        #endif

        public const int EYES_CLOSE_NUM = 4; // 현재가에서 EYES_CLOSE_NUM 스텝만큼 가격을 올려 지정가에 두기 위한 스텝 변수 *시장가매수는 해당종목의 상한가 기준이기에 풀매수가 안되기 때문에 지정가로 하는거.
        public const double NORMAL_TRADE_RATIO = 0.05;
        public const double PAPER_TRADE_RATIO = 1.0;
        #endregion

        #region 시간
        //--------------------------------------------------------
        // 시간 상수
        //--------------------------------------------------------
        public const int TEN_SEC = 10;
        public const int MINUTE_SEC = 60;
        public const int MINUTE_KIWOOM = 100;
        public const int FIVE_MINUTE_SEC = 300;
        public const int FIVE_MINUTE_KIWOOM = 500;
        public const int TEN_MINUTE_SEC = 600;
        public const int TEN_MINUTE_KIWOOM = 1000;
        public const int HOUR_KIWOOM = 10000;
        public const int SHUTDOWN_TIME = 152000; // 매매마감시간
        public const int MARKET_END_TIME = 153000; // 시장마감시간
        public const int REAL_DATA_END_TIME = 152000; // 시장마감시간u
        public const int MARKET_START_TIME = 90000; // 시장시작시간
        public const int BAN_BUY_TIME = 151000; // 매수마감시간
        #endregion
        #region 가격
        //--------------------------------------------------------
        // 가격 상수
        //--------------------------------------------------------
        public const long TRILLION = 1000000000000; // 일조
        public const long HUNDRED_BILLION = 100000000000; // 천억
        public const long TEN_BILLION = 10000000000; // 백억
        public const long BILLION = 1000000000; // 십억
        public const long HUNDRED_MILLION = 100000000; // 일억
        public const long TWENTY_MILLION = 20000000; // 이천만
        public const long TEN_MILLION = 10000000; // 천만
        public const long FIVE_MILLION = 5000000; // 오백만
        public const long MILLION = 1000000; // 백만
        public const long TEN_THOUSAND = 10000; // 일만
        public const long HUNDRED_THOUSAND = 100000; // 십만
        #endregion

        #endregion

    }
}
