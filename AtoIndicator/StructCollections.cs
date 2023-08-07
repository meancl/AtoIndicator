using System;
using System.Collections.Generic;
using static AtoIndicator.KiwoomLib.TimeLib;
using AtoIndicator.DB;
using System.Text;

namespace AtoIndicator
{
    public partial class MainForm
    {
        // ============================================
        // 각 종목이 가지는 개인 구조체
        // ============================================
        public struct EachStock
        {
            public ManualReservation manualReserve;



            // ----------------------------------
            // 공용 변수
            // ----------------------------------
            public bool isExcluded; // 실시간 제외대상확인용 bool변수
            public int nLastRecordTime; // 마지막 기록시간
            public long lTotalTradePrice; // 거래대금(매수+매도대금)
            public long lTotalTradeVolume; // 거래수량
            public double fTotalTradeVolume; // 거래수량비율
            public long lTotalBuyPrice; // 매수대금(매수-매도대금)
            public long lTotalBuyVolume; // 매수수량
            public long lOnlyBuyPrice;
            public long lOnlyBuyVolume;
            public long lOnlySellPrice;
            public long lOnlySellVolume;
            public double fTotalBuyVolume; // 매수수량비율
            public long lMarketCap; // 시가총액
            public int nChegyulCnt; // 인덱스 
            public long lMinuteTradePrice;
            public double fMinuteTradeVolume;
            public long lMinuteBuyPrice;
            public double fMinuteBuyVolume;
            public long lMinuteTradeVolume;
            public long lMinuteBuyVolume;
            public double fMinutePower;
            public int nMinuteCnt;
            public int nMinuteUpDown;
            public int nJumpCnt;
            public int nMissCount;
            public int nNoMoveCount;
            public int nFewSpeedCount;
            public int nAccumUpDownCount;
            public double fAccumUpPower;
            public double fAccumDownPower;
            public int nAccumUpCnt;
            public int nAccumDownCnt;



            public BuyedManager myTradeManager;       // 매매관리자 변수
            public MaOverN maOverN;                   // 이동평균선 변수
            public TimeLineManager timeLines1m;       // 차트데이터 변수
            public CrushManager crushMinuteManager;   // 전고점 변수
            public RankSystem rankSystem;             // 랭킹데이터 변수
            public SequenceStrategy sequenceStrategy; // 순차적전략 변수
            public FeeManager feeMgr;                 // 세금, 수수료 관련 변수
            public EventManager eventMgr;

            // ----------------------------------
            // 기본정보 변수
            // ----------------------------------
            public string sRealScreenNum; // 실시간화면번호
            public string sCode; // 종목번호
            public string sCodeName; // 종목명
            public int nMarketGubun; // 코스닥번호면 코스닥, 코스피번호면 코스피
            public string sMarketGubunTag; // 코스닥번호면 "KOSDAQ", 코스피번호면 "KOSPI"
            public long lShareOutstanding; // 유통주식수
            public long lTotalNumOfStock;  // 상장주식수
            public long lFixedMarketCap;
            public int nYesterdayEndPrice; // 전날 종가 

            // ----------------------------------
            // 매매관련 변수
            // ----------------------------------
            public int nHoldingsCnt; // 보유종목수
            public double fAccumBuyedRatio; // 최대매수기준 현재 비율(추매 시 1을 넘을 수 있음) // 추가??
            public PaperBuyStrategy paperBuyStrategy;
            public FakeVolatilityStrategy fakeVolatilityStrategy;
            public FakeDownStrategy fakeDownStrategy;
            public FakeBuyStrategy fakeBuyStrategy;
            public FakeResistStrategy fakeResistStrategy;
            public FakeAssistantStrategy fakeAssistantStrategy;
            public FakeStrategyManager fakeStrategyMgr;


            public List<string> unhandledBuyOrderIdList;
            public List<string> unhandledSellOrderIdList;
            // ----------------------------------
            // 초기 변수
            // ----------------------------------
            public bool isFirstCheck;    // 초기설정용 bool 변수
            public int nTodayStartPrice; // 시초가
            public int nStartGap;    // 갭 가격
            public double fStartGap; // 갭 등락율


            // ----------------------------------
            // 장 시작 전 호가변수
            // ----------------------------------
            public int nStopHogaCnt; // 장시작전 호가카운트
            public int nStopUpDownCnt; // 장시작전 
            public int nStopFirstPrice;
            public int nStopPrevPrice;
            public int nStopMaxPrice;
            public int nStopMinPrice;
            public double fStopMaxPower;
            public double fStopMinPower;
            public double fStopMaxMinDiff; // max와 min사이 벌어짐


            // ----------------------------------
            // 주식호가 변수
            // ----------------------------------
            public int nTotalBuyHogaVolume; // 총매수호가수량
            public int nTotalSellHogaVolume; // 총매도호가수량
            public int nThreeSellHogaVolume; // 매도1~3호가수량
            public int nTotalHogaVolume; //  총호가수량
            public double fHogaRatio; // 매수매도대비율

            public int nCurHogaPrice; // 현재 호가 1가격

            // ----------------------------------
            // 호가상태 변수
            // ----------------------------------
            public CurStatus totalHogaVolumeStatus;
            public CurStatus hogaRatioStatus;
            public CurStatus hogaSpeedStatus;
            public int nHogaCnt; // 호가카운트
            public int nPrevHogaUpdateTime; // 호가이전조정시간

            // ----------------------------------
            // 주식체결 변수
            // ----------------------------------
            public int nFs; // 최우선 매도호가
            public int nFb; // 최우선 매수호가
            public double fDiff; // 최우선 매수호가와 최우선 매도호가의 값 차이
            public int nTv;  // 체결량 
            public double fTs; // 체결강도
            public double fPowerWithoutGap; // 시초가 등락율
            public double fPower; // 전일종가 등락률 
            public double fPrevPowerWithoutGap; // 이전 시초가 등락율;
            public double fPowerDiff;
            public double fTradeRatioCompared;

            // ----------------------------------
            // 체결상태 변수
            // ----------------------------------
            public int nPrevSpeedUpdateTime; // 이전기본(속도, 체결량, 순체결량)조정 시간
            public int nPrevPowerUpdateTime; // 이전가격조정 시간
            public CurStatus speedStatus;
            public CurStatus tradeStatus;
            public CurStatus pureTradeStatus;
            public CurStatus pureBuyStatus;
            public CurStatus priceMoveStatus;
            public CurStatus priceUpMoveStatus;
            public double fCntPerTime; // 시간 대비 누적카운트 
            public double fPlusCnt09; // 초당0.9 상승카운트
            public double fMinusCnt09; // 초당0.9 하락카운트
            public double fPlusCnt07; // 초당0.7 상승카운트
            public double fMinusCnt07; // 초당0.7 하락카운트
            public double fPowerJar; // 초당0.995 가격변화카운트
            public double fOnlyDownPowerJar;
            public double fOnlyUpPowerJar;
            public double fSharePerTrade; // 체결량 대비 유통주
            public double fTradePerPure; // 체결량차 대비 체결량
            public double fHogaPerTrade; // 체결량 대비 호가 
            public double fSharePerHoga; // 호가 대비 유통주

            public double fPositiveStickPower;
            public double fNegativeStickPower;
            public int nFirstVolume;
            public long lFirstPrice;


            public int nRealMaxPrice;
            // ----------------------------------
            // Vi관련 변수
            // ----------------------------------
            public bool isViMode; // 현재 vi상태인가? 장 초반에 vi인척하는 딜레이상태가 존재하는 듯 보임
            public bool isViGauge; // vi가 종료되면 그동안 허비된 공백시간만큼의 조정을 제외하기 위한 변수
            public int nViStartTime; // vi 시작시간
            public int nViEndTime; // vi 종료시간 nViEndTime - nViStartTime  >= 2min 인 지 확인하여 vi인지 vi인척하는 놈인지 체크 
            public int nViTimeLineIdx;
            public int nViFs;
            public int nViCnt;
            public int nRealDataIdxVi;


            // ------------------------------
            // 오늘 가격
            //-------------------------------
            public int nTodayMinPrice;
            public int nTodayMinTime;
            public double fTodayMinPower;

            public int nTodayMaxPrice;
            public int nTodayMaxTime;
            public double fTodayMaxPower;

            public int nTodayBottomPrice;
            public int nTodayBottomTime;
            public double fTodayBottomPower;


            public void Init()
            {
                crushMinuteManager.Init();
                rankSystem.Init();
                sequenceStrategy.Init();
                timeLines1m.Init();
                fakeStrategyMgr.Init();

                feeMgr = default;

                eventMgr = new EventManager();
                manualReserve = new ManualReservation();

                unhandledBuyOrderIdList = new List<string>();
                unhandledSellOrderIdList = new List<string>();

                myTradeManager = new BuyedManager(); // 개인구조체 매매관리자 초기화
            }



            public void GetFakeFix(FakeReport rep)
            {
                try
                {
                    rep.dTradeTime = DateTime.Today; // key 
                    rep.sCode = sCode; // key
                    rep.sCodeName = sCodeName;
                    rep.nLocationOfComp = COMPUTER_LOCATION; // key

                    // 개인구조체 정보

                    rep.nStopHogaCnt = nStopHogaCnt;
                    rep.nStopUpDownCnt = nStopUpDownCnt;
                    rep.nStopFirstPrice = nStopFirstPrice;
                    rep.nStopMaxPrice = nStopMaxPrice;
                    rep.nStopMinPrice = nStopMinPrice;
                    rep.fStopMaxPower = fStopMaxPower;
                    rep.fStopMinPower = fStopMinPower;
                    rep.fStopMaxMinDiff = fStopMaxMinDiff;

                    rep.fPositiveStickPower = fPositiveStickPower;
                    rep.fNegativeStickPower = fNegativeStickPower;
                    rep.nFirstVolume = nFirstVolume;
                    rep.lFirstPrice = lFirstPrice;
                    rep.nFs = nFs;
                    rep.nFb = nFb;
                    rep.fTs = fTs;
                    rep.fTradeRatioCompared = fTradeRatioCompared;
                    rep.nEveryCount = fakeStrategyMgr.nEveryAICount;
                    rep.nPrevLastFs = timeLines1m.arrTimeLine[timeLines1m.nRealDataIdx].nLastFs;
                    rep.nPrevStartFs = timeLines1m.arrTimeLine[timeLines1m.nRealDataIdx].nStartFs;
                    rep.nPrevMaxFs = timeLines1m.arrTimeLine[timeLines1m.nRealDataIdx].nMaxFs;
                    rep.nPrevMinFs = timeLines1m.arrTimeLine[timeLines1m.nRealDataIdx].nMinFs;
                    rep.nPrevVolume = timeLines1m.arrTimeLine[timeLines1m.nRealDataIdx].nTotalVolume;
                    rep.nCurLastFs = timeLines1m.arrTimeLine[timeLines1m.nPrevTimeLineIdx].nLastFs;
                    rep.nCurStartFs = timeLines1m.arrTimeLine[timeLines1m.nPrevTimeLineIdx].nStartFs;
                    rep.nCurMaxFs = timeLines1m.arrTimeLine[timeLines1m.nPrevTimeLineIdx].nMaxFs;
                    rep.nCurMinFs = timeLines1m.arrTimeLine[timeLines1m.nPrevTimeLineIdx].nMinFs;
                    rep.nCurVolume = timeLines1m.arrTimeLine[timeLines1m.nPrevTimeLineIdx].nTotalVolume;
                    rep.nTodayMaxPrice = nTodayMaxPrice;
                    rep.nTodayMaxTime = nTodayMaxTime;
                    rep.fTodayMaxPower = fTodayMaxPower;
                    rep.nTodayMinPrice = nTodayMinPrice;
                    rep.nTodayMinTime = nTodayMinTime;
                    rep.fTodayMinPower = fTodayMinPower;
                    rep.fStartGap = fStartGap;
                    rep.sType = sMarketGubunTag;
                    rep.fPowerWithOutGap = fPowerWithoutGap;
                    rep.fPower = fPower;
                    rep.fPlusCnt07 = fPlusCnt07;
                    rep.fMinusCnt07 = fMinusCnt07;
                    rep.fPlusCnt09 = fPlusCnt09;
                    rep.fMinusCnt09 = fMinusCnt09;
                    rep.fPowerJar = fPowerJar;
                    rep.fOnlyDownPowerJar = fOnlyDownPowerJar;
                    rep.fOnlyUpPowerJar = fOnlyUpPowerJar;
                    rep.nChegyulCnt = nChegyulCnt;
                    rep.nNoMoveCnt = nNoMoveCount;
                    rep.nFewSpeedCnt = nFewSpeedCount;
                    rep.nMissCnt = nMissCount;
                    rep.lTotalTradePrice = lTotalTradePrice;
                    rep.lTotalBuyPrice = lOnlyBuyPrice;
                    rep.lTotalSellPrice = lOnlySellPrice;
                    rep.nDownCntMa20m = maOverN.nDownCntMa20m;
                    rep.nDownCntMa1h = maOverN.nDownCntMa1h;
                    rep.nDownCntMa2h = maOverN.nDownCntMa2h;
                    rep.nUpCntMa20m = maOverN.nUpCntMa20m;
                    rep.nUpCntMa1h = maOverN.nUpCntMa1h;
                    rep.nUpCntMa2h = maOverN.nUpCntMa2h;
                    rep.fMa20mDiff = (maOverN.fCurDownFs - maOverN.fCurMa20m) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fMa1hDiff = (maOverN.fCurDownFs - maOverN.fCurMa1h) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fMa2hDiff = (maOverN.fCurDownFs - maOverN.fCurMa2h) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fMa20mCurDiff = (nFs - maOverN.fCurMa20m) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fMa1hCurDiff = (nFs - maOverN.fCurMa1h) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fMa2hCurDiff = (nFs - maOverN.fCurMa2h) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fGapMa20mDiff = (maOverN.fCurDownFs - maOverN.fCurGapMa20m) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fGapMa1hDiff = (maOverN.fCurDownFs - maOverN.fCurGapMa1h) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fGapMa2hDiff = (maOverN.fCurDownFs - maOverN.fCurGapMa2h) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fGapMa20mCurDiff = (nFs - maOverN.fCurGapMa20m) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fGapMa1hCurDiff = (nFs - maOverN.fCurGapMa1h) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fGapMa2hCurDiff = (nFs - maOverN.fCurGapMa2h) / ((nYesterdayEndPrice > 0) ? nYesterdayEndPrice : 1);
                    rep.fIAngle = timeLines1m.fInitAngle;
                    rep.fMAngle = timeLines1m.fMaxAngle;
                    rep.fTAngle = timeLines1m.fTotalMedianAngle;
                    rep.fHAngle = timeLines1m.fHourMedianAngle;
                    rep.fRAngle = timeLines1m.fRecentMedianAngle;
                    rep.fDAngle = timeLines1m.fDAngle;
                    rep.fISlope = timeLines1m.fInitSlope;
                    rep.fMSlope = timeLines1m.fMaxSlope;
                    rep.fTSlope = timeLines1m.fTotalMedian;
                    rep.fHSlope = timeLines1m.fHourMedian;
                    rep.fRSlope = timeLines1m.fRecentMedian;
                    rep.fDSlope = timeLines1m.fMaxSlope - timeLines1m.fInitSlope;
                    rep.fSpeedCur = speedStatus.fCur;
                    rep.fHogaSpeedCur = hogaSpeedStatus.fCur;
                    rep.fTradeCur = tradeStatus.fCur;
                    rep.fPureTradeCur = pureTradeStatus.fCur;
                    rep.fPureBuyCur = pureBuyStatus.fCur;
                    rep.fPriceMoveCur = priceMoveStatus.fCur;
                    rep.fPriceUpMoveCur = priceUpMoveStatus.fCur;
                    rep.fHogaRatioCur = hogaRatioStatus.fCur;
                    rep.fSharePerHoga = fSharePerHoga;
                    rep.fSharePerTrade = fSharePerTrade;
                    rep.fHogaPerTrade = fHogaPerTrade;
                    rep.fTradePerPure = fTradePerPure;

                    // new
                    rep.nHogaCnt = nHogaCnt;
                    rep.lTotalTradeVolume = lTotalTradeVolume;
                    rep.lTotalBuyVolume = lOnlyBuyVolume;
                    rep.lTotalSellVolume = lOnlySellVolume;
                    rep.nAccumUpDownCount = nAccumUpDownCount;
                    rep.fAccumUpPower = fAccumUpPower;
                    rep.fAccumDownPower = fAccumDownPower;
                    rep.lMarketCap = lFixedMarketCap;

                    rep.nRankHold10 = rankSystem.nRankHold10;
                    rep.nRankHold20 = rankSystem.nRankHold20;
                    rep.nRankHold50 = rankSystem.nRankHold50;
                    rep.nRankHold100 = rankSystem.nRankHold100;
                    rep.nRankHold200 = rankSystem.nRankHold200;
                    rep.nRankHold500 = rankSystem.nRankHold500;
                    rep.nRankHold1000 = rankSystem.nRankHold1000;

                    rep.nSummationRankMove = rankSystem.nSummationMove;
                    rep.nTotalRank = rankSystem.nSummationRanking;
                    rep.nAccumCountRanking = rankSystem.nAccumCountRanking;
                    rep.nMarketCapRanking = rankSystem.nMarketCapRanking;
                    rep.nPowerRanking = rankSystem.nPowerRanking;
                    rep.nTotalBuyPriceRanking = rankSystem.nTotalBuyPriceRanking;
                    rep.nTotalBuyVolumeRanking = rankSystem.nTotalBuyVolumeRanking;
                    rep.nTotalTradePriceRanking = rankSystem.nTotalTradePriceRanking;
                    rep.nTotalTradeVolumeRanking = rankSystem.nTotalTradeVolumeRanking;

                    rep.nMinuteTotalRank = rankSystem.nMinuteSummationRanking;
                    rep.nMinuteBuyPriceRanking = rankSystem.nMinuteBuyPriceRanking;
                    rep.nMinuteBuyVolumeRanking = rankSystem.nMinuteBuyVolumeRanking;
                    rep.nMinuteCountRanking = rankSystem.nMinuteCountRanking;
                    rep.nMinutePowerRanking = rankSystem.nMinutePowerRanking;
                    rep.nMinuteTradePriceRanking = rankSystem.nMinuteTradePriceRanking;
                    rep.nMinuteTradeVolumeRanking = rankSystem.nMinuteTradeVolumeRanking;
                    rep.nMinuteUpDownRanking = rankSystem.nMinuteUpDownRanking;
                    rep.nTradeCnt = paperBuyStrategy.nStrategyNum;
                    rep.nFakeBuyCnt = fakeBuyStrategy.nStrategyNum;
                    rep.nFakeResistCnt = fakeResistStrategy.nStrategyNum;
                    rep.nFakeAssistantCnt = fakeAssistantStrategy.nStrategyNum;
                    rep.nPriceUpCnt = fakeVolatilityStrategy.nStrategyNum;
                    rep.nPriceDownCnt = fakeDownStrategy.nStrategyNum;
                    rep.nRealBuyMinuteCnt = paperBuyStrategy.nMinuteLocationCount;
                    rep.nFakeBuyMinuteCnt = fakeBuyStrategy.nMinuteLocationCount;
                    rep.nFakeResistMinuteCnt = fakeResistStrategy.nMinuteLocationCount;
                    rep.nFakeAssistantMinuteCnt = fakeAssistantStrategy.nMinuteLocationCount;
                    rep.nPriceUpMinuteCnt = fakeVolatilityStrategy.nMinuteLocationCount;
                    rep.nPriceDownMinuteCnt = fakeDownStrategy.nMinuteLocationCount;
                    rep.nFakeBuyUpperCnt = fakeBuyStrategy.nUpperCount;
                    rep.nFakeResistUpperCnt = fakeResistStrategy.nUpperCount;
                    rep.nFakeAssistantUpperCnt = fakeAssistantStrategy.nUpperCount;
                    rep.nPriceUpUpperCnt = fakeVolatilityStrategy.nUpperCount;
                    rep.nPriceDownUpperCnt = fakeDownStrategy.nUpperCount;
                    rep.nTotalFakeCnt = fakeStrategyMgr.nTotalFakeCount;
                    rep.nTotalFakeMinuteCnt = fakeStrategyMgr.nTotalFakeMinuteAreaNum;
                    rep.nUpCandleCnt = timeLines1m.onePerCandleList.Count;
                    rep.nTwoPerCandleCnt = timeLines1m.twoPerCandleList.Count;
                    rep.nShootingCnt = timeLines1m.threePerCandleList.Count;
                    rep.nFourPerCandleCnt = timeLines1m.fourPerCandleList.Count;
                    rep.nDownCandleCnt = timeLines1m.downCandleList.Count;
                    rep.nUpTailCnt = timeLines1m.upTailList.Count;
                    rep.nDownTailCnt = timeLines1m.downTailList.Count;
                    rep.nCrushCnt = crushMinuteManager.nCurCnt;
                    rep.nCrushUpCnt = crushMinuteManager.nUpCnt;
                    rep.nCrushDownCnt = crushMinuteManager.nDownCnt;
                    rep.nCrushSpecialDownCnt = crushMinuteManager.nSpecialDownCnt;

                    rep.nYesterdayEndPrice = nYesterdayEndPrice;

                    rep.nRealBuyAIPass = fakeStrategyMgr.nAIPassed;
                    rep.nFakeBuyAIPass = fakeStrategyMgr.nFakeAccumPassed;
                    rep.nEveryBuyAIPass = fakeStrategyMgr.nEveryAIPassNum;
                    rep.fAIScore = fakeStrategyMgr.fAIScore;
                    rep.fAIScoreJar = fakeStrategyMgr.fAIScoreJar;
                    rep.fAIScoreJarDegree = fakeStrategyMgr.fAIScoreJarDegree;

                    rep.nCandleTwoOverRealCnt = sequenceStrategy.nCandleTwoOverRealCount;
                    rep.nCandleTwoOverRealNoLeafCnt = sequenceStrategy.nCandleTwoOverRealNoLeafCount;

                    rep.fMaDownFsVal = maOverN.fCurDownFs;
                    rep.fMa20mVal = maOverN.fCurMa20m;
                    rep.fMa1hVal = maOverN.fCurMa1h;
                    rep.fMa2hVal = maOverN.fCurMa2h;
                    rep.fMaxMaDownFsVal = maOverN.fMaxDownFs;
                    rep.fMaxMa20mVal = maOverN.fMaxMa20m;
                    rep.fMaxMa1hVal = maOverN.fMaxMa1h;
                    rep.fMaxMa2hVal = maOverN.fMaxMa2h;
                    rep.nMaxMaDownFsTime = maOverN.nMaxDownFsTime;
                    rep.nMaxMa20mTime = maOverN.nMaxMa20mTime;
                    rep.nMaxMa1hTime = maOverN.nMaxMa1hTime;
                    rep.nMaxMa2hTime = maOverN.nMaxMa2hTime;
                }
                catch { }
            }

            public string GetInfoString()
            {
                string NEW_LINE = Environment.NewLine;
                string sMessage;
                try
                {
                    sMessage =
                        $"============= 누적 ================={NEW_LINE}" +
                        $"초기갭 : {fStartGap}{NEW_LINE}" +
                        $"갭제외파워 : {fPowerWithoutGap}{NEW_LINE}" +
                        $"카운트07  P : {Math.Round(fPlusCnt07, 3)}  M : {Math.Round(fMinusCnt07, 3)}{NEW_LINE}" +
                        $"카운트09  P : {Math.Round(fPlusCnt09, 3)}  M : {Math.Round(fMinusCnt09, 3)}{NEW_LINE}" +
                        $"파워자 : {fPowerJar}{NEW_LINE}" +
                        $"실매수횟수 : {paperBuyStrategy.nStrategyNum}{NEW_LINE}" +
                        $"체결카운트 : {nChegyulCnt}{NEW_LINE}" +
                        $"호가카운트 : {nHogaCnt}{NEW_LINE}" +
                        $"노무브 : {nNoMoveCount}{NEW_LINE}" +
                        $"적은거래 : {nFewSpeedCount}{NEW_LINE}" +
                        $"거래없음 : {nMissCount}{NEW_LINE}" +
                        $"거래대금 : {Math.Round(lTotalTradePrice / (double)MainForm.MILLION, 3)}(백만원){NEW_LINE}" +
                        $"순매수대금 : {Math.Round(lOnlyBuyPrice / (double)MainForm.MILLION, 3)}(백만원){NEW_LINE}" +
                        $"순매도대금 : {Math.Round(lOnlySellPrice / (double)MainForm.MILLION, 3)}(백만원){NEW_LINE}" +
                        $"매수매도차 : {Math.Round(lTotalBuyPrice / (double)MainForm.MILLION, 3)}(백만원){NEW_LINE}" +
                        $"시가총액 : {lMarketCap / (double)HUNDRED_MILLION}(억원){NEW_LINE}" +
                        $"총순위 : {rankSystem.nSummationRanking}({rankSystem.nSummationMove}){NEW_LINE}" +
                        $"================= ARROW ============={NEW_LINE}" +
                        $"페이크매수 : {fakeBuyStrategy.nStrategyNum}{NEW_LINE}" +
                        $"페이크보조 : {fakeAssistantStrategy.nStrategyNum}{NEW_LINE}" +
                        $"페이크저항 : {fakeResistStrategy.nStrategyNum}{NEW_LINE}" +
                        $"페이크저항 : {fakeResistStrategy.nStrategyNum}{NEW_LINE}" +
                        $"총 Arrow : {fakeStrategyMgr.nTotalFakeCount}{NEW_LINE}" +
                        $"총 ArrowMinute : {fakeStrategyMgr.nTotalFakeMinuteAreaNum}{NEW_LINE}" +
                        $"================= 분 봉 ============={NEW_LINE}" +
                        $"1퍼캔들 : {timeLines1m.onePerCandleList.Count}{NEW_LINE}" +
                        $"2퍼캔들 : {timeLines1m.twoPerCandleList.Count}{NEW_LINE}" +
                        $"3퍼캔들 : {timeLines1m.threePerCandleList.Count}{NEW_LINE}" +
                        $"4퍼캔들 : {timeLines1m.fourPerCandleList.Count}{NEW_LINE}" +
                        $"아래캔들 : {timeLines1m.downCandleList.Count}{NEW_LINE}" +
                        $"위꼬리 : {timeLines1m.upTailList.Count}{NEW_LINE}" +
                        $"아래꼬리 : {timeLines1m.downTailList.Count}{NEW_LINE}" +
                        $"전고점 카운트 : {crushMinuteManager.nCurCnt}{NEW_LINE}" +
                        $"전고점   업 : {crushMinuteManager.nUpCnt}{NEW_LINE}" +
                        $"2퍼 : {sequenceStrategy.nCandleTwoOverRealCount}{NEW_LINE}" +
                        $"깃털 2퍼 : {sequenceStrategy.nCandleTwoOverRealNoLeafCount}{NEW_LINE}" +
                        $"=================현상태=============={NEW_LINE}" +
                        $"체결속도 : {speedStatus.fCur}{NEW_LINE}" +
                        $"호가속도 : {hogaSpeedStatus.fCur}{NEW_LINE}" +
                        $"거래량 : {Math.Round(tradeStatus.fCur * nFs / MainForm.MILLION, 3)}(백만원){NEW_LINE}" +
                        $"매수매도차 : {Math.Round(pureTradeStatus.fCur * nFs / MainForm.MILLION, 3)}(백만원){NEW_LINE}" +
                        $"순매수 : {Math.Round(pureBuyStatus.fCur * nFs / MainForm.MILLION, 3)}(백만원){NEW_LINE}" +
                        $"호가비율 : {hogaRatioStatus.fCur}{NEW_LINE}" +
                        $"=================체결비=============={NEW_LINE}" +
                        $"유통대호가 : {fSharePerHoga}{NEW_LINE}" +
                        $"유통대체결 : {fSharePerTrade}{NEW_LINE}" +
                        $"호가대체결 : {fHogaPerTrade}{NEW_LINE}" +
                        $"체결순체결 : {fTradePerPure}{NEW_LINE}" +
                        $"================이동평균선============{NEW_LINE}" +
                        $"-20m : {maOverN.nDownCntMa20m}{NEW_LINE}" +
                        $"-1h : {maOverN.nDownCntMa1h}{NEW_LINE}" +
                        $"-2h : {maOverN.nDownCntMa2h}{NEW_LINE}" +
                        $"+20m : {maOverN.nUpCntMa20m}{NEW_LINE}" +
                        $"+1h : {maOverN.nUpCntMa1h}{NEW_LINE}" +
                        $"+2h : {maOverN.nUpCntMa2h}{NEW_LINE}" +
                        $"=============== 각도 ================={NEW_LINE}" +
                        $"T각도 : {timeLines1m.fTotalMedianAngle}{NEW_LINE}" +
                        $"H각도 : {timeLines1m.fHourMedianAngle}{NEW_LINE}" +
                        $"R각도 : {timeLines1m.fRecentMedianAngle}{NEW_LINE}" +
                        $"D각도 : {timeLines1m.fDAngle}{NEW_LINE}";
                }
                catch (Exception ex)
                {
                    sMessage = $"오류가 발견됐습니다.{NEW_LINE}" +
                        $"오류메시지 : {ex.Message}{NEW_LINE}";
                }

                return sMessage;
            }
        }

        public class EventManager
        {
            public EventHandler cancelEachStockFormEventHandler;

            public EventManager()
            {
                cancelEachStockFormEventHandler = null;
            }
        }
        // ============================================
        // AI서비스 웨이터 큐에 저장하기 위한 구조체변수
        // ============================================
        public struct AIResponseSlot
        {
            public TradeRequestSlot slot;
            public int nEaIdx;
            public int nMMFNumber;
            public int nRequestId;
        }

        // ============================================
        // 매매요청 큐에 저장하기 위한 구조체변수
        // ============================================
        public struct TradeRequestSlot
        {
            // ----------------------------------
            // 공용 인자들
            // ----------------------------------
            public int nRqTime; // 주문요청시간
            public int nEaIdx; // 개인구조체인덱스
            public TradeMethodCategory eTradeMethod; // true면 단계별 상승매매, false면 익절,손절 일괄매매

            public string sDescription;

            // ----------------------------------
            // 매수 인자들
            // ----------------------------------
            public double fRequestRatio; // 매수신청시 최대매수가 기준 비율 

            // ----------------------------------
            // 매도 인자들
            // ----------------------------------
            public int nBuyedSlotIdx; // 구매열람인덱스 , 매도요청이 실패하면 해당인덱스를 통해 다시 요청할 수 있게 하기 위한 변수

            // ----------------------------------
            // SendOrder 인자들
            // ----------------------------------
            public string sRQName; // 사용자 구분명
            public string sScreenNo; // 화면번호
            public string sAccNo; // 계좌번호 10자리
            public int nOrderType; // 주문유형 1:신규매수, 2:신규매도 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정
            public string sCode; // 종목코드(6자리)
            public int nQty; // 주문수량
            public int nOrderPrice; // 주문가격
            public string sHogaGb; // 거래구분 (00:지정가, 03:시장가, ...)
            public string sOrgOrderId;  // 원주문번호. 신규주문에는 공백 입력, 정정/취소시 입력합니다.

            public bool isByHand;
        }

        public struct CurStatus
        {
            public double fVal; // 과거
            public double fPush; // 현재
            public double fCur; // 결과

            public void Commit(double fRatio)
            {
                fVal = fVal == 0 ? fPush : fPush * fRatio + fVal * (1 - fRatio);
                fPush = 0;
            }

            public void Push(double fNew, double fRatio)
            {
                fPush = fPush == 0 ? fNew : fNew * fRatio + fPush * (1 - fRatio);
            }

            public void Update(double fRatio)
            {
                fCur = fVal == 0 ? fPush : fPush * fRatio + fVal * (1 - fRatio);
            }
        }

        // ============================================
        // 종합 개인구조체 매매블록 구조체
        // ============================================
        public class BuyedManager
        {
            // public int nIdx;
            public List<BuyedSlot> arrBuyedSlots;
            public StringBuilder sTotalLog;


            // 현재 거래중... 정보
            public int nBuyReqCnt; // 현재 종목의 매수신청카운트
            public int nSellReqCnt; // 현재 종목의 매도신청카운트 
            public bool isOrderStatus; // 현재 매매중인 지 확인하는 변수;

            public bool isRealBuyChangeNeeded = false; // 체잔에서 하나의 매수, 매도가 끝났을때 ( eachForm에서 갱신을 위해서 )
            public int nAppliedShowingRealBuyedId = -1;

            public int nTotalBuyed;
            public int nTotalSelled;
            public int nTotalSelling;

            public TradeMethodCategory eDefaultTradeCategory;

            // EachStockHistoryForm 조작용 변수
            public bool isTargetChoice;
            public double fTargetPriceTouch;
            public double fBottomPriceTouch;
            public List<(double, double)> posRecordList;
            public DateTime dLatestApproachTime;
            public bool isEachStockHistoryExist;

            public BuyedManager()
            {
                eDefaultTradeCategory = TradeMethodCategory.FixedMethod;
                arrBuyedSlots = new List<BuyedSlot>();
                sTotalLog = new StringBuilder();
                posRecordList = new List<(double, double)>();
                dLatestApproachTime = DateTime.UtcNow;
            }
        }



        // ============================================
        // 개인구조체 매매블록 구조체
        // ============================================
        public class BuyedSlot //TODAY
        {
            public int nEaIdx;
            public int nBuyedSlotId; // 매매블록 아이디 인덱스// 추가??
            public int nBuyPrice; // 구매한 가격
            public int nBuyVolume; // 구매한 주식수
            public int nCurVolume; // 보유 주식수
            public int nOrderVolume; // 주문 수량
            public int nOrderPrice; // 주문 가격
            public int nOriginOrderPrice; // 상한패딩 붙이기 전 가격
            public int nRequestTime; // 매수요청시간 // 추가??
            public int nSellRequestTime; // 매수요청시간 // 추가??
            public int nReceiptTime; // 매수접수시간
            public int nBuyEndTime; //  매수체결완료시간
            public double fTradeRatio; // 구매비율
            public bool isBuyBanned;
            public bool isBuyByHand;

            public string sBuyScrNo;
            public string sSellScrNo;

            public int nBuyMinuteIdx; // 로그용 매매블록에서의 분당 매수시점 인덱스
            public int nSellMinuteIdx; // 로그용 매매블록에서의 분당 매도시점 인덱스

            public int nBuyedSumPrice; // 매수한 가격
            public int nSellVolume; // 처분한 갯수
            public int nSelledSumPrice; // 처분한 총 가격

            public string sCurOrgOrderId; // 원주문번호   default:""
            public bool isBuying; // 매수 중 시그널
            public bool isSelling; // 매도 중 시그널
            public bool isAllSelled; // 매도 종료(모두 팔림)
            public bool isAllBuyed; // 매수완료 시그널 ( 같은 매매블럭에 추매를 했을때 다 사졌나를 확인하기 위한 변수 ) 
            public bool isResponsed; // 응답을 받았는 지
            public bool isSellStarted;
            public int nSellCancelReserveTime;
            public bool isSellCancelReserved;

            public TradeMethodCategory eTradeMethod; // 
            public double fTargetPer; // 얼마에 익절할거야
            public double fBottomPer; // 얼마에 손절할거야

            // 경과 확인용
            public double fPowerWithFee; // 세금 수수료 포함 손익율
            public int nCurLineIdx; // 현재 익절선과 손절선의 인덱스

            public bool isCopied;
            public string sBuyDescription; // 매수원인(Annotation용)
            public string sSellDescription; // 매도원인(Annotation용)
            public StringBuilder sEachLog;

            // 전량 매수취소인 경우는 다른방식으로 입력한다.
            public int nBirthTime;  // 탄생시간
            public int nBirthPrice; // 탄생가격
            public int nDeathTime; // 소멸시간
            public int nDeathPrice; // 소멸가격


            // 매도 관리용
            public int nSellErrorLastTime;
            public int nSellErrorCount;

            public MaOverN maOverN;

            // >=== 가격변화 체크용
            public int nMaxCheckLineIdx = MIDDLE_STEP;
            public int nMinCheckLineIdx = MIDDLE_STEP;

            public int nCheckLineIdx = MIDDLE_STEP;
            public double fCheckCeilingPer = 0;
            public double fCheckBottomPer = -0.0025;
            // <=== 

            public BuyedSlot(int nCurIdx)
            {
                sEachLog = new StringBuilder();
                nEaIdx = nCurIdx;
            }

            public BuyedSlot DeepCopy()
            {
                BuyedSlot newSlot = new BuyedSlot(nEaIdx);

                newSlot.nBuyedSlotId = nBuyedSlotId;
                newSlot.nBuyPrice = nBuyPrice;
                newSlot.nBuyVolume = nBuyVolume;
                newSlot.nCurVolume = nCurVolume;
                newSlot.nOrderVolume = nOrderVolume;
                newSlot.nOrderPrice = nOrderPrice;
                newSlot.nOriginOrderPrice = nOriginOrderPrice;
                newSlot.nRequestTime = nRequestTime;
                newSlot.nSellRequestTime = nSellRequestTime;
                newSlot.nReceiptTime = nReceiptTime;
                newSlot.nBuyEndTime = nBuyEndTime;
                newSlot.fTradeRatio = fTradeRatio;
                newSlot.isBuyBanned = isBuyBanned;
                newSlot.isBuyByHand = isBuyByHand;

                newSlot.nBuyMinuteIdx = nBuyMinuteIdx;
                newSlot.nSellMinuteIdx = nSellMinuteIdx;

                newSlot.nBuyedSumPrice = nBuyedSumPrice;
                newSlot.nSellVolume = nSellVolume;
                newSlot.nSelledSumPrice = nSelledSumPrice;

                newSlot.isBuying = isBuying;
                newSlot.isSelling = isSelling;
                newSlot.isAllSelled = isAllSelled;
                newSlot.isAllBuyed = isAllBuyed;
                newSlot.isResponsed = isResponsed;
                newSlot.isSellStarted = isSellStarted;
                newSlot.isSellCancelReserved = isSellCancelReserved;
                newSlot.nSellCancelReserveTime = nSellCancelReserveTime;

                newSlot.eTradeMethod = eTradeMethod;
                newSlot.fTargetPer = fTargetPer;
                newSlot.fBottomPer = fBottomPer;

                // 경과 확인용
                newSlot.fPowerWithFee = fPowerWithFee;
                newSlot.nCurLineIdx = nCurLineIdx;

                newSlot.isCopied = true;
                newSlot.sBuyDescription = sBuyDescription;
                newSlot.sSellDescription = sSellDescription;
                newSlot.sEachLog.Append(sEachLog.ToString());
                newSlot.sBuyScrNo = sBuyScrNo;
                newSlot.sSellScrNo = sSellScrNo;

                // 전량 매수취소인 경우는 다른방식으로 입력한다.
                newSlot.nBirthTime = nBirthTime;
                newSlot.nBirthPrice = nBirthPrice;
                newSlot.nDeathTime = nDeathTime;
                newSlot.nDeathPrice = nDeathPrice;

                // 매도 관리용
                newSlot.nSellErrorLastTime = nSellErrorLastTime;
                newSlot.nSellErrorCount = nSellErrorCount;

                newSlot.maOverN = maOverN;
                newSlot.nMaxCheckLineIdx = nMaxCheckLineIdx;
                newSlot.nMinCheckLineIdx = nMinCheckLineIdx;
                newSlot.nCheckLineIdx = nCheckLineIdx;
                newSlot.fCheckCeilingPer = fCheckCeilingPer;
                newSlot.fCheckBottomPer = fCheckBottomPer;

                return newSlot;
            }

        }



        // 매수후 맥스값과 민값을 기록하기위한 구조체
        public struct MaxMinRecorder
        {
            public int nMaxPriceAfterBuy; // 팔고난 직후부터 맥스가격 
            public int nMaxTimeAfterBuy; // 팔고난 직후부터 맥스타임
            public double fMaxPowerWithFeeAfterBuy; // 팔고난 직후부터 맥스손익율(수수료 포함)

            public int nMinPriceAfterBuyBeforeMax; // 팔고난 직후부터 맥스이전 미니멈가격 
            public int nMinTimeAfterBuyBeforeMax; // 팔고난 직후부터 맥스이전 미니멈타임
            public double fMinPowerWithFeeAfterBuyBeforeMax; // 팔고난 직후부터 맥스이전 미니멈손익율(수수료 포함)

            public int nBottomPriceAfterBuy;// 팔고난 직후부터 맥스이전 진짜 미니멈가격 
            public int nBottomTimeAfterBuy;// 팔고난 직후부터 맥스이전 진짜 미니멈타임
            public double fBottomPowerWithFeeAfterBuy; // 팔고난 직후부터 진짜 미니멈손익율 (수수료 포함)

            public int nTopPriceAfterBuy;// 바닥 직후 맥스 값
            public int nTopTimeAfterBuy;// 바닥 직후 맥스 시간
            public double fTopPowerWithFeeAfterBuy; // 바닥 직후 맥스 파워

            public int nBoundBottomPriceAfterBuy;
            public int nBoundBottomTimeAfterBuy;
            public double fBoundBottomPowerWithFeeAfterBuy;

            public int nBoundTopPriceAfterBuy;
            public int nBoundTopTimeAfterBuy;
            public double fBoundTopPowerWithFeeAfterBuy;

            public void CheckMaxMin(int nT, int nDownPrice, int nUpPrice, int nBuyedPrice, int nDenomPrice)
            {
                // 바닥
                if (nBottomPriceAfterBuy == 0 || nBottomPriceAfterBuy > nDownPrice)
                {
                    nBottomPriceAfterBuy = nDownPrice;
                    nBottomTimeAfterBuy = nT;
                    fBottomPowerWithFeeAfterBuy = (double)(nBottomPriceAfterBuy - nBuyedPrice) / nDenomPrice - (KOSDAQ_STOCK_TAX + KIWOOM_STOCK_FEE * 2);

                    nTopPriceAfterBuy = nBottomPriceAfterBuy;
                    nTopTimeAfterBuy = nBottomTimeAfterBuy;
                    fTopPowerWithFeeAfterBuy = fBottomPowerWithFeeAfterBuy;
                }

                // 바닥 이후 탑
                if (nTopPriceAfterBuy < nUpPrice)
                {
                    nTopPriceAfterBuy = nUpPrice;
                    nTopTimeAfterBuy = nT;
                    fTopPowerWithFeeAfterBuy = (double)(nTopPriceAfterBuy - nBuyedPrice) / nDenomPrice - (KOSDAQ_STOCK_TAX + KIWOOM_STOCK_FEE * 2);
                }

                // 맥스
                if (nMaxPriceAfterBuy < nUpPrice) // fb로 측정한다.
                {
                    nMaxPriceAfterBuy = nUpPrice;
                    nMaxTimeAfterBuy = nT;
                    fMaxPowerWithFeeAfterBuy = (double)(nMaxPriceAfterBuy - nBuyedPrice) / nDenomPrice - (KOSDAQ_STOCK_TAX + KIWOOM_STOCK_FEE * 2);

                    nBoundBottomPriceAfterBuy = nMaxPriceAfterBuy;
                    nBoundBottomTimeAfterBuy = nMaxTimeAfterBuy;
                    fBoundBottomPowerWithFeeAfterBuy = fMaxPowerWithFeeAfterBuy;

                    nBoundTopPriceAfterBuy = nMaxPriceAfterBuy;
                    nBoundTopTimeAfterBuy = nMaxTimeAfterBuy;
                    fBoundTopPowerWithFeeAfterBuy = fMaxPowerWithFeeAfterBuy;
                }

                // 바운드바닥
                if (nBoundBottomPriceAfterBuy == 0 || nBoundBottomPriceAfterBuy > nDownPrice)
                {
                    nBoundBottomPriceAfterBuy = nDownPrice;
                    nBoundBottomTimeAfterBuy = nT;
                    fBoundBottomPowerWithFeeAfterBuy = (double)(nBoundBottomPriceAfterBuy - nBuyedPrice) / nDenomPrice - (KOSDAQ_STOCK_TAX + KIWOOM_STOCK_FEE * 2);

                    nBoundTopPriceAfterBuy = nBoundBottomPriceAfterBuy;
                    nBoundTopTimeAfterBuy = nBoundBottomTimeAfterBuy;
                    fBoundTopPowerWithFeeAfterBuy = fBoundBottomPowerWithFeeAfterBuy;
                }

                // 바운드바닥 이후 최고점
                if (nBoundTopPriceAfterBuy < nUpPrice)
                {
                    nBoundTopPriceAfterBuy = nUpPrice;
                    nBoundTopTimeAfterBuy = nT;
                    fBoundTopPowerWithFeeAfterBuy = (double)(nBoundTopPriceAfterBuy - nBuyedPrice) / nDenomPrice - (KOSDAQ_STOCK_TAX + KIWOOM_STOCK_FEE * 2);
                }

                // 맥스 이전 저점
                if (nMinPriceAfterBuyBeforeMax == 0 || nBottomTimeAfterBuy < nMaxTimeAfterBuy)
                {
                    nMinPriceAfterBuyBeforeMax = nBottomPriceAfterBuy;
                    nMinTimeAfterBuyBeforeMax = nBottomTimeAfterBuy;
                    fMinPowerWithFeeAfterBuyBeforeMax = fBottomPowerWithFeeAfterBuy;
                }

            }
        }


        // ============================================
        // 현재보유종목 열람용 구조체변수
        // ============================================
        public struct Holdings
        {
            public string sCode;
            public string sCodeName;
            public double fYield;
            public int nHoldingQty;
            public int nBuyedPrice;
            public int nCurPrice;
            public int nTotalPL;
            public int nNumPossibleToSell;
        }

        // ============================================
        // 게시판용 변수
        // ============================================
        public struct StockDashBoard
        {
            public int nDashBoardCnt;
            public StockPiece[] stockPanel; // 현재 갱신용 패널 (개인구조체마다) 
        }

        public struct StockPiece // 정렬대상을 가지고 있다
        {
            public string sCode; // 종목코드
            public int nEaIdx; // 코드 인덱스

            public long lTotalTradePrice; // 거래대금
            public double fTotalTradeVolume; // 거래수량
            public long lTotalBuyPrice; // 매수대금
            public double fTotalBuyVolume; // 매수수량
            public int nAccumCount; // 누적카운트
            public double fTotalPowerWithOutGap; // 갭제외수익률 
            public long lMarketCap; // 시가총액
            public int nSummationRank; // 총 순위합


            public long lMinuteTradePrice; // 분당 거래대금
            public double fMinuteTradeVolume; // 분당 상대적거래수량
            public long lMinuteBuyPrice; // 분당 매수대금
            public double fMinuteBuyVolume; // 분당 상대적매수수량
            public double fMinutePower; // 분당 손익율(갭제외)
            public int nMinuteCnt; // 분당 카운트
            public int nMinuteUpDown; // 분당 위아래
            public int nSummationMinuteRank; // 총 분당순위합

        }
        public class ManualReservation
        {
            public bool isChosenQ;
            public bool isChosenW;
            public bool isChosenE;
            public bool isChosenR;


            public ReservationPoint[] reserveArr;
            public int nCurReserve;

            public ManualReservation()
            {
                nCurReserve = INIT_RESERVE;
                reserveArr = new ReservationPoint[INIT_RESERVE];
            }
            public void ClearAll()
            {
                for (int i = 0; i < INIT_RESERVE; i++)
                    reserveArr[i].Clear();
            }
        }

        public struct ReservationPoint
        {
            public bool isBuyReserved;
            public int nBuyReserveNumStock;
            public bool isSelected;
            public bool isChosen1;
            public int nSelectedTime;
            public double fCritLine1;
            public int nChosenTime;

            public void Clear()
            {
                nBuyReserveNumStock = 0; // 매수예약 갯수
                isBuyReserved = false; // 매수예약
                isSelected = false; // 선택
                isChosen1 = false;
                nSelectedTime = 0;
                fCritLine1 = 0;
                nChosenTime = 0;
            }
        }
        // ============================================
        // 타임라인 변수( 개인구조체 현황 기록용 )
        // ============================================
        public struct TimeLineManager
        {
            public int nRealDataIdx;  // 실제 데이터들이 들어있는 최종인덱스
            public int nPrevTimeLineIdx; // 관리용으로 인덱스가 한칸 더 앞에 있음
            public int nTimeDegree; // 시간단위(초)
            public int nFsPointer;
            public double fTotalMedian;
            public double fTotalMedianAngle;
            public double fInitSlope;
            public double fInitAngle;
            public double fDAngle;
            public int nMaxUpFs;
            public int nMinDownFs;
            public double fMaxSlope;
            public double fMaxAngle;
            public double fHourMedian;
            public double fHourMedianAngle;
            public double fRecentMedian;
            public double fRecentMedianAngle;
            public int nMaxPricePrevMinute;
            public List<(int, double)> onePerCandleList;
            public List<(int, double)> twoPerCandleList;
            public List<(int, double)> threePerCandleList;
            public List<(int, double)> fourPerCandleList;
            public List<(int, double)> downCandleList;
            public List<(int, double)> upTailList;
            public List<(int, double)> downTailList;

            public TimeLine[] arrTimeLine;

            public void Init()
            {
                onePerCandleList = new List<(int, double)>();
                twoPerCandleList = new List<(int, double)>();
                threePerCandleList = new List<(int, double)>();
                fourPerCandleList = new List<(int, double)>();
                downCandleList = new List<(int, double)>();
                upTailList = new List<(int, double)>();
                downTailList = new List<(int, double)>();
                nTimeDegree = MINUTE_SEC;
                arrTimeLine = new TimeLine[BRUSH + SubTimeToTimeAndSec(MARKET_END_TIME, MARKET_START_TIME) / nTimeDegree];
            }
        }


        public struct TimeLine
        {
            public int nTime;
            public int nTimeIdx;
            public int nMaxFs;
            public int nMinFs;
            public int nStartFs;
            public int nLastFs;
            public int nUpFs;
            public int nDownFs;
            public int nTotalVolume;
            public int nBuyVolume;
            public int nSellVolume;
            public long lTotalPrice;
            public long lBuyPrice;
            public long lSellPrice;
            public int nCount;
            public double fAccumUpPower;
            public double fAccumDownPower;

            public double fMedianAngle;
            public double fHourAngle;
            public double fRecentAngle;
            public double fInitAngle;
            public double fMaxAngle;
            public double fDAngle;

            public double fOverMa0;
            public double fOverMa1;
            public double fOverMa2;
            public int nUpTimeOverMa0;
            public int nUpTimeOverMa1;
            public int nUpTimeOverMa2;
            public int nDownTimeOverMa0;
            public int nDownTimeOverMa1;
            public int nDownTimeOverMa2;

            public double fOverMaGap0;
            public double fOverMaGap1;
            public double fOverMaGap2;


            public double fTradeCompared;
            public double fTradeStrength;

        }
        public struct CrushManager
        {
            public int nCrushMaxPrice;
            public int nCrushMaxTime;
            public int nCrushMinPrice;
            public int nCrushMinTime;
            public int nCrushOnlyMinPrice;
            public int nCrushOnlyMinTime;
            public int nPrevCrushCnt;
            public int nCurCnt;
            public int nUpCnt;
            public int nDownCnt;
            public int nSpecialDownCnt;
            public bool isCrushCheck;

            public int nCrushRealTimePrev; // 이전 실시간 전고점 
            public int nCrushRealTimeCount; // 실시간 전고점 누적횟수
            public int nCrushRealTimeLineIdx; // 실시간 전고점의 timeLineIdx
            public bool isCrushRealTimeCheck; // 실시간 전고점이 달성됐는지
            public int nCrushRealTimeWidthMaxMin; // 최고점과 최저점과의 거리(시간)
            public int nCrushRealTimeWidthMaxCur; // 최고점과 현재사이의 거리(시간)
            public double fCrushRealTimeHeight; // 최고점과 최저점 사이의 가격변화율

            public List<Crush> crushList;

            public void Init()
            {
                crushList = new List<Crush>(); // 개인구조체 전고점 리스트 초기화
            }
        }

        public struct Crush
        {
            public int nCnt;
            public double fMaxMinPower;
            public double fCurMinPower;
            public int nMaxMinTime;
            public int nMaxCurTime;
            public int nMinCurTime;
            public int nMinPrice;
            public int nMaxPrice;
            public double fUpperNow;
        }

        public struct RankSystem
        {
            public int nTime;
            public int nCurIdx; // 누적카운트

            // 전체
            public int nTotalTradePriceRanking; // 거래대금
            public int nTotalTradeVolumeRanking; // 상대적거래수량
            public int nTotalBuyPriceRanking; // 매수대금
            public int nTotalBuyVolumeRanking; // 상대적매수수량
            public int nAccumCountRanking; // 누적카운트
            public int nPowerRanking; // 손익률
            public int nMarketCapRanking; // 시가총액
            public int nSummationRanking; // 총 순위
            public int nPrevSummationRanking; // 이전 총순위
            public int nSummationMove; // 총 순위 변동

            // 분당
            public int nMinuteTradePriceRanking; // 분당 거래대금 순위
            public int nMinuteTradeVolumeRanking; // 분당 상대적거래수량 순위
            public int nMinuteBuyPriceRanking; // 분당 매수대금 순위
            public int nMinuteBuyVolumeRanking; // 분당 상대적매수수량 순위
            public int nMinutePowerRanking; // 분당 손익율
            public int nMinuteCountRanking; // 분당 카운트
            public int nMinuteUpDownRanking; // 분당 위아래
            public int nMinuteSummationRanking; // 분당 순위

            public int nRankHold10; // 총 순위 10위권 이내 유지시간
            public int nRankHold20; // 총 순위 20위권 이내 유지시간
            public int nRankHold50; // 총 순위 50위권 이내 유지시간
            public int nRankHold100; // 총 순위 100위권 이내 유지시간
            public int nRankHold200; // 총 순위 200위권 이내 유지시간
            public int nRankHold500; // 총 순위 500위권 이내 유지시간
            public int nRankHold1000; // 총 순위 1000위권 이내 유지시간

            public Ranking[] arrRanking;

            public void Init()
            {
                arrRanking = new Ranking[BRUSH + SubTimeToTimeAndSec(MARKET_END_TIME, MARKET_START_TIME) / MINUTE_SEC]; // 개인구조체 게시판순위 
            }
        }


        public struct Ranking
        {
            public int nRecordTime; // 기록용 시간

            // 전체
            public int nTotalTradePriceRanking; // 1. 거래대금
            public int nTotalTradeVolumeRanking; // 2. 상대적거래수량
            public int nTotalBuyPriceRanking; // 3. 매수대금
            public int nTotalBuyVolumeRanking; // 4. 상대적매수수량
            public int nAccumCountRanking; // 5. 누적카운트
            public int nPowerRanking; // 6. 손익률
            public int nMarketCapRanking; // 7. 시가총액
            public int nSummationRanking; // 8. 총 순위

            // 분당
            public int nMinuteTradePriceRanking; // 1. 분당 거래대금
            public int nMinuteTradeVolumeRanking; // 2. 분당 상대적 거래수량
            public int nMinuteBuyPriceRanking; // 3. 분당 매수대금
            public int nMinuteBuyVolumeRanking; // 4. 분당 상대적 매수수량
            public int nMinutePowerRanking; // 5. 분당 손익율
            public int nMinuteCountRanking; // 6. 분당 카운트
            public int nMinuteUpDownRanking; // 7. 분당 위아래 카운트
            public int nMinuteRanking; //  8. 분당 랭킹

            public int nSummationMove; // 이전 총순위 변동정도
        }

        public struct MaOverN
        {

            // 최근의 ma값
            public double fCurDownFs;
            public double fCurMa20m;
            public double fCurMa1h;
            public double fCurMa2h;
            public double fCurGapMa20m;
            public double fCurGapMa1h;
            public double fCurGapMa2h;

            // 맥스의 ma값
            public double fMaxDownFs;
            public double fMaxMa20m;
            public double fMaxMa1h;
            public double fMaxMa2h;
            public double fMaxGapMa20m;
            public double fMaxGapMa1h;
            public double fMaxGapMa2h;

            // bottom의 ma값
            public double fBottomMa20m;
            public double fBottomMa1h;
            public double fBottomMa2h;
            public double fBottomGapMa20m;
            public double fBottomGapMa1h;
            public double fBottomGapMa2h;

            // max의 ma시간
            public int nMaxDownFsTime;
            public int nMaxMa20mTime;
            public int nMaxMa1hTime;
            public int nMaxMa2hTime;
            public int nMaxGapMa20mTime;
            public int nMaxGapMa1hTime;
            public int nMaxGapMa2hTime;

            // bottom의 ma시간
            public int nBottomMa20mTime;
            public int nBottomMa1hTime;
            public int nBottomMa2hTime;
            public int nBottomGapMa20mTime;
            public int nBottomGapMa1hTime;
            public int nBottomGapMa2hTime;


            // max의 ma power
            public double fMaxMa20mPower;
            public double fMaxMa1hPower;
            public double fMaxMa2hPower;
            public double fMaxGapMa20mPower;
            public double fMaxGapMa1hPower;
            public double fMaxGapMa2hPower;

            // bottom의 ma power
            public double fBottomMa20mPower;
            public double fBottomMa1hPower;
            public double fBottomMa2hPower;
            public double fBottomGapMa20mPower;
            public double fBottomGapMa1hPower;
            public double fBottomGapMa2hPower;

            public int nUpCntMa20m;
            public int nUpCntMa1h;
            public int nUpCntMa2h;

            public int nDownCntMa20m;
            public int nDownCntMa1h;
            public int nDownCntMa2h;
        }


        /// <summary>
        /// 매매하면 기록을 StrategyHistory list에 strategy idx에 맞게 삽입한다.
        /// </summary>
        public struct StrategyHistory
        {
            public int nEaIdx; // nEaIdx
            public int nBuyedIdx; // 매매블럭 인덱스

            public StrategyHistory(int nEaIdx, int nBuyedIdx)
            {
                this.nEaIdx = nEaIdx;
                this.nBuyedIdx = nBuyedIdx;
            }
        }

        // 전략분석에 필요한 각 매매블럭의 데이터 구조체
        public struct EachResultTracker
        {
            public int nEaIdx;
            public int nBuyedIdx;
            public double fProfit;
            public int nTradingHoldingTime; // 매매 유지시간
        }


        public class FakeDBRecordInfo
        {
            public FakeReport fr;

            public int nTimeLineIdx;

            public MaxMinRecorder maxMinRealTilThree;
            public MaxMinRecorder maxMinRealWhile10;
            public MaxMinRecorder maxMinRealWhile30;
            public MaxMinRecorder maxMinRealWhile60;
            public MaxMinRecorder maxMinMinuteTilThree;
            public MaxMinRecorder maxMinMinuteTilThreeWhile10;
            public MaxMinRecorder maxMinMinuteTilThreeWhile30;

            public FakeDBRecordInfo()
            {
                fr = new FakeReport();
            }
        }


        public struct FakeStrategyManager
        {
            public int nSharedPrevMinuteIdx;
            public int nSharedMinuteLocationCount;

            public List<FakeHistoryPiece> listFakeHistoryPiece;
            public List<FakeDBRecordInfo> fd;

            public int nFakeBuyNum;
            public int nFakeResistNum;
            public int nFakeAssistantNum;
            public int nFakeVolatilityNum;
            public int nFakeDownNum;
            public int nPaperBuyNum;
            public int nTotalArrowNum;

            public int nCurHitNum;
            public int nCurHitType;


            public Dictionary<int, int> hitDict25;
            public Dictionary<int, int> hitDict38;
            public Dictionary<int, int> hitDict312;
            public Dictionary<int, int> hitDict410;

            public int nFakeBuyMinuteAreaNum;
            public int nFakeResistMinuteAreaNum;
            public int nFakeAssistantMinuteAreaNum;
            public int nFakeVolatilityMinuteAreaNum;
            public int nFakeDownMinuteAreaNum;
            public int nPaperBuyMinuteAreaNum;

            public int nTotalFakeMinuteAreaNum;
            public int nTotalFakeCount;
            public int nTotalArrowCount;


            public int nFakeAccumPassed;
            public int nFakeAccumTried;
            public int nAIPassed;
            public int nAIPrevTimeLineIdx;
            public int nAIStepMinuteCount;
            public int nAIJumpDiffMinuteCount;
            public int nFakeAIPrevTimeLineIdx;
            public int nFakeAIStepMinuteCount;
            public int nFakeAIJumpDiffMinuteCount;


            public double fAIScore;
            public double fAIScoreJar;
            public double fAIScoreJarDegree;


            public int nEveryAICount;
            public int nEveryAIPassNum;

            public int nAI5Time;
            public int nAI10Time;
            public int nAI15Time;
            public int nAI20Time;
            public int nAI30Time;
            public int nAI50Time;

            public void Init()
            {
                hitDict25 = new Dictionary<int, int>();
                hitDict38 = new Dictionary<int, int>();
                hitDict312 = new Dictionary<int, int>();
                hitDict410 = new Dictionary<int, int>();

                listFakeHistoryPiece = new List<FakeHistoryPiece>();
                fd = new List<FakeDBRecordInfo>();
            }

        }


        public class FakeFrame
        {
            public int nFakeType;

            public int[] arrLastTouch; // 가장최근에 해당전략 요청한 시간
            public int[] arrStrategy; // 전략당 배열
            public int[] arrMinuteIdx;
            public int[] arrSpecificStrategy;// 배열의 각 슬롯당 전략
            public int[] arrBuyTime;
            public int[] arrBuyPrice;

            public int nHitNum;

            public int nLastTouchTime;
            public int nStrategyNum; // 가짜 전략 카운트를 정하는것
            public double fEverageShoulderPrice; // 사려고 했을때가 고점(어깨에서 머리)이라 가정
            public int nSumShoulderPrice; // 여러번 사려하면 가격의 합
            public int nMaxShoulderPrice; // 가장 값이 높았을 경우의 가격
            public int nUpperCount; // 어깨가 계속해서 올라가는 횟수
            public bool isSuddenBoom;
            public int nPrevMaxMinIdx;
            public int nPrevMaxMinUpperCount;
            public int nPrevMinuteIdx;
            public int nCurBarBuyCount;
            public int nMinuteLocationCount;

            public int[] arrPrevMinuteIdx;

        }


        public struct PaperTradeSlot
        {

            public int nRqTime; // 언제 주문신청했는 지
            public int nRqCount; // 어떤 체결카운트에 주문신청했는 지
            public int nRqPrice; // 얼마에 주문신청했는 지
            public int nOverPrice; // 주문시점 + Alpha
            public int nRqVolume; // 얼만큼 주문신청했는 지
            public int nBuyedVolume; // 언제 사졌는 지
            public int nBuyedPrice; // 얼마에 사졌는 지 
            public int nBuyedTimeLineIdx;
            public int nBuyRqTimeLineIdx;
            public int nBuyEndTime;
            public int nSellHogaVolume; // 호가에 얼마나 걸려있는 지
            public int nCanceledVolume; // 매수취소 적용된 매수요청물량
            public int nTargetRqVolume;

            // 매도
            public TradeMethodCategory methodCategory;
            public bool isSelling;
            public bool isAllSelled;
            public int nBuyHogaVolume; // 호가에 얼마나 걸려있는 지
            public int nSellRqVolume; // 얼만큼 주문신청했는 지
            public int nSellRqCount;
            public int nSellRqTime;
            public int nSellRqTimeLineIdx;
            public int nSellRqPrice;
            public int nSellEndVolume;
            public int nSellEndPrice;
            public int nSellEndTime;
            public int nSellEndTimeLineIdx;
            public string sSellDescription;

            public int nCurLineIdx;
            public double fTargetPer;
            public double fBottomPer;
            public double fPowerWithFee;

            public string sFixedMsg;
            public int nSequence;
        }

        /// <summary>
        /// ABOUT 실제전략
        /// </summary>
        public class PaperBuyStrategy : FakeFrame
        {

            // 임시용 
            public bool isOrderCheck;
            public bool isPaperBuyChangeNeeded;

            #region 매매 정보
            public PaperTradeSlot[] paperTradeSlot;
            #endregion

            // -------------------------------------------------------------------------------
            // END ---- 전략별  추가변수들
            // -------------------------------------------------------------------------------
            public PaperBuyStrategy(int t, int s)
            {
                nFakeType = t;

                arrStrategy = new int[s];
                arrLastTouch = new int[s];
                arrPrevMinuteIdx = new int[s];

                arrMinuteIdx = new int[PAPER_TRADE_MAX_NUM];
                arrBuyTime = new int[PAPER_TRADE_MAX_NUM];
                arrBuyPrice = new int[PAPER_TRADE_MAX_NUM];
                arrSpecificStrategy = new int[PAPER_TRADE_MAX_NUM];

                nPrevMinuteIdx = -1;

                paperTradeSlot = new PaperTradeSlot[PAPER_TRADE_MAX_NUM];
            }

        }

        public class FakeVolatilityStrategy : FakeFrame
        {
            public FakeVolatilityStrategy(int t, int s)
            {
                nFakeType = t;

                arrStrategy = new int[s];
                arrLastTouch = new int[s];
                arrPrevMinuteIdx = new int[s];

                arrMinuteIdx = new int[FAKE_VOLATILITY_MAX_NUM];
                arrBuyTime = new int[FAKE_VOLATILITY_MAX_NUM];
                arrBuyPrice = new int[FAKE_VOLATILITY_MAX_NUM];
                arrSpecificStrategy = new int[FAKE_VOLATILITY_MAX_NUM];
            }

        }

        public class FakeDownStrategy : FakeFrame
        {
            public FakeDownStrategy(int t, int s)
            {
                nFakeType = t;

                arrStrategy = new int[s];
                arrLastTouch = new int[s];
                arrPrevMinuteIdx = new int[s];

                arrMinuteIdx = new int[FAKE_DOWN_MAX_NUM];
                arrBuyTime = new int[FAKE_DOWN_MAX_NUM];
                arrBuyPrice = new int[FAKE_DOWN_MAX_NUM];
                arrSpecificStrategy = new int[FAKE_DOWN_MAX_NUM];
            }

        }



        /// <summary>
        ///  가짜 전략 해당 종목의 과열성을 체크함과 동시에 고점 정도를 파악한다.
        /// </summary>
        public class FakeBuyStrategy : FakeFrame
        {
            // 각 전략용
            public List<int> listApproachTime3;
            public List<int> listApproachTime6;
            public List<int> listApproachTime7;
            public List<int> listApproachTime8;
            public List<int> listApproachTime9;
            public List<int> listApproachTime10;
            public List<int> listApproachTime11;
            public List<int> listApproachTime12;
            public List<int> listApproachTime13;
            public List<int> listApproachTime15;
            public List<int> listApproachTime17;
            public List<int> listApproachTime18;
            public List<int> listApproachTime19;

            ///////////////////////////

            public FakeBuyStrategy(int t, int s)
            {
                nFakeType = t;

                arrLastTouch = new int[s];
                arrStrategy = new int[s];
                arrPrevMinuteIdx = new int[s];

                arrMinuteIdx = new int[FAKE_BUY_MAX_NUM];
                arrBuyTime = new int[FAKE_BUY_MAX_NUM];
                arrBuyPrice = new int[FAKE_BUY_MAX_NUM];
                arrSpecificStrategy = new int[FAKE_BUY_MAX_NUM];

                listApproachTime3 = new List<int>();
                listApproachTime6 = new List<int>();
                listApproachTime7 = new List<int>();
                listApproachTime8 = new List<int>();
                listApproachTime9 = new List<int>();
                listApproachTime10 = new List<int>();
                listApproachTime11 = new List<int>();
                listApproachTime12 = new List<int>();
                listApproachTime13 = new List<int>();
                listApproachTime15 = new List<int>();
                listApproachTime17 = new List<int>();
                listApproachTime18 = new List<int>();
                listApproachTime19 = new List<int>();
            }

        }

        /// <summary>
        ///  가짜 전략 해당 종목의 떨어질 조건을 등록한다.
        /// </summary>
        public class FakeResistStrategy : FakeFrame
        {
            // 각 전략용

            ////////////////////////////////////////
            public FakeResistStrategy(int t, int s)
            {
                nFakeType = t;

                arrLastTouch = new int[s];
                arrStrategy = new int[s];
                arrPrevMinuteIdx = new int[s];

                arrMinuteIdx = new int[FAKE_RESIST_MAX_NUM];
                arrBuyTime = new int[FAKE_RESIST_MAX_NUM];
                arrBuyPrice = new int[FAKE_RESIST_MAX_NUM];
                arrSpecificStrategy = new int[FAKE_RESIST_MAX_NUM];

            }
        }

        /// <summary>
        /// 가짜 전략보조용 
        /// </summary>
        public class FakeAssistantStrategy : FakeFrame
        {
            // 각 전략용
            public List<int> listApproachAssistantTime2;
            public List<int> listApproachAssistantTime3;
            public List<int> listApproachAssistantTime5;
            public List<int> listApproachAssistantTime6;
            public List<int> listApproachAssistantTime8;
            public List<int> listApproachAssistantTime9;
            public List<int> listApproachAssistantTime10;
            public List<int> listApproachAssistantTime11;
            public List<int> listApproachAssistantTime12;
            public List<int> listApproachAssistantTime13;
            ///////////////////////////

            public FakeAssistantStrategy(int t, int s)
            {
                nFakeType = t;

                arrLastTouch = new int[s];
                arrStrategy = new int[s];
                arrPrevMinuteIdx = new int[s];

                arrMinuteIdx = new int[FAKE_ASSISTANT_MAX_NUM];
                arrBuyPrice = new int[FAKE_ASSISTANT_MAX_NUM];
                arrBuyTime = new int[FAKE_ASSISTANT_MAX_NUM];
                arrSpecificStrategy = new int[FAKE_ASSISTANT_MAX_NUM];

                // 임시용

                listApproachAssistantTime2 = new List<int>();
                listApproachAssistantTime3 = new List<int>();
                listApproachAssistantTime5 = new List<int>();
                listApproachAssistantTime6 = new List<int>();
                listApproachAssistantTime8 = new List<int>();
                listApproachAssistantTime9 = new List<int>();
                listApproachAssistantTime10 = new List<int>();
                listApproachAssistantTime11 = new List<int>();
                listApproachAssistantTime12 = new List<int>();
                listApproachAssistantTime13 = new List<int>();
            }

        }
        public struct FeeManager
        {

            public double fTotalBuyFeeCutOff;
            public double fTotalSellFeeCutOff;


            public double fOnceSellTaxCutOff1;
            public double fOnceSellTaxCutOff2;



            /*
             세금은 코스피의 경우 거래세 , 특농세 각각 계산하고 원 절사하고 더함
             코스닥의 경우 그냥 거래세만 곱하고 원 절사하고 더함
             
            수수료는 매수매도 각각 곱하고 각각 10원 절사
             */

            public int GetBuyFee(int nPrice, int nVolume)
            {
                double buyFee;

                buyFee = nPrice * nVolume * KIWOOM_STOCK_FEE; // 172.59
                fTotalBuyFeeCutOff += buyFee % 10;  // 2.59
                buyFee = buyFee - buyFee % 10;  // 172.59 - 2.59
                buyFee += (int)fTotalBuyFeeCutOff / 10 * 10; // 
                fTotalBuyFeeCutOff %= 10;

                return (int)buyFee;
            }

            public int GetSellFee(int nPrice, int nVolume)
            {
                double sellFee;

                sellFee = nPrice * nVolume * KIWOOM_STOCK_FEE; // 172.59
                fTotalSellFeeCutOff += sellFee % 10;  // 2.59
                sellFee = sellFee - sellFee % 10;  // 172.59 - 2.59
                sellFee += (int)fTotalSellFeeCutOff / 10 * 10; // 
                fTotalSellFeeCutOff %= 10;

                return (int)sellFee;
            }


            public int GetSellTax(int nPrice, int nVolume, int nMarketType)
            {
                double sellCommission;

                if (nMarketType == KOSPI_ID)
                {
                    double sellTax1;
                    double sellTax2;

                    sellTax1 = nPrice * nVolume * KOSPI_STOCK_TAX1; // 15.3
                    fOnceSellTaxCutOff1 += sellTax1 - (int)sellTax1;  // 15.3 - 15 = 0.3
                    sellTax1 = (int)sellTax1; // 15
                    sellTax1 += (int)fOnceSellTaxCutOff1; // 
                    fOnceSellTaxCutOff1 %= 1;

                    sellTax2 = nPrice * nVolume * KOSPI_STOCK_TAX2;
                    fOnceSellTaxCutOff2 += sellTax2 - (int)sellTax2;
                    sellTax2 = (int)sellTax2;
                    sellTax2 += (int)fOnceSellTaxCutOff2;
                    fOnceSellTaxCutOff2 %= 1;

                    sellCommission = sellTax1 + sellTax2;
                }
                else
                {
                    double sellTax1;

                    sellTax1 = nPrice * nVolume * KOSDAQ_STOCK_TAX;
                    fOnceSellTaxCutOff1 += sellTax1 - (int)sellTax1;
                    sellTax1 = (int)sellTax1;
                    sellTax1 += (int)fOnceSellTaxCutOff1;
                    fOnceSellTaxCutOff1 %= 1;

                    sellCommission = sellTax1;
                }

                return (int)sellCommission;
            }

            public void SetDoneSellOneSet()
            {
                fOnceSellTaxCutOff1 = 0;
                fOnceSellTaxCutOff2 = 0;
            }

            /// 임시로 빼는 수수료
            public int GetRoughBuyFee(int nPrice, int nVolume)
            {
                double EPSILON = 0.000001;
                double retFee = nPrice * nVolume * KIWOOM_STOCK_FEE;
                return (int)(retFee + ((retFee % 1 > EPSILON) ? 1 : 0));
            }

        }

        /// <summary>
        /// 매매정보를 전략별로 분석해 결과struct에 삽입한다.
        /// </summary>
        public struct Statisticer
        {
            public int nCurStrategyNum;
            public List<EachResultTracker> eachTotalTracker;
            public List<EachResultTracker> eachTradedTracker;
            public StrategyResult[] strategyResult;

            public void Init(int s)
            {
                eachTotalTracker = new List<EachResultTracker>();
                eachTradedTracker = new List<EachResultTracker>();
                strategyResult = new StrategyResult[s];
            }

            public void Clear()
            {
                if (eachTotalTracker.Count > 0)
                    eachTotalTracker.Clear();

                if (eachTradedTracker.Count > 0)
                    eachTradedTracker.Clear();

                for (int _ = 0; _ < strategyResult.Length; _++)
                    strategyResult[_].Clear();
            }
        }

        /// <summary>
        /// 전략별 결과를 저장한다.
        /// </summary>
        public struct StrategyResult
        {
            public bool isTradeDataExists; // 매매데이터가 있는지
            public int nTradingNum; // 매매중 총합갯수
            public int nTradedNum; // 매매완료 총합갯수
            public int nCanceledNum; // 전량매수취소 총합갯수
            public int nAllTradeNum; // 매수중, 매매중, 매매완료, 전량매수취소 총합갯수
            public int nStrategyNum; // 전략번호
            public StaticMember<double> traded; // 매매완료
            public StaticMember<double> total; // 매매완료 + 매매중

            public void Clear()
            {
                isTradeDataExists = false;
                nTradingNum = 0;
                nTradedNum = 0;
                nCanceledNum = 0;
                nAllTradeNum = 0;
                nStrategyNum = -1;
                traded.min = 0;
                traded.max = 0;
                traded.everage = 0;
                traded.median = 0;
                total.min = 0;
                total.max = 0;
                total.everage = 0;
                total.median = 0;
            }
        }


        /// <summary>
        /// P버튼의 결과 구조체
        /// </summary>
        public struct PResult
        {
            public int nFakeBuyStrategyNum;
            public int nFakeBuyStrategyMinuteNum;
            public int nFakeAssistantStrategyNum;
            public int nFakeAssistantStrategyMinuteNum;
            public int nFakeResistStrategyNum;
            public int nFakeResistStrategyMinuteNum;
            public int nFakeUpStrategyNum;
            public int nFakeUpStrategyMinuteNum;
            public int nFakeDownStrategyNum;
            public int nFakeDownStrategyMinuteNum;
            public int nPaperBuyStrategyNum;
            public int nPaperBuyStrategyMinuteNum;

            public int nTotalStrategyNum;
            public int nTotalStrategyMinuteNum;
        }

        // 조건의 순서를 연결해 매수 타이밍을 잡으려함.
        public struct SequenceStrategy
        {
            #region 5퍼 달성
            public bool isFiveReachedMinute;
            public bool isFiveReachedReal;
            public int nFiveReachedRealTimeLineIdx;
            public bool isFiveKeepingForTwoTimeLine;
            public bool isFiveKeepingSuccessForTwoTimeLine;
            public bool isFiveReachedRealLeafEntranceBlocked; // 5퍼달성 단한번 접근 
            public bool isFiveReachedRealLeafBan; // 5퍼 달성할때 거래대금 1억 못넘으면 깃털
            public bool isFiveReachedRealHundredMillion; // 5퍼 달성할때 거래대금 1억 이상 10억 이하
            public bool isFiveReachedRealBillionUp; // 5퍼 달성할때 거래대금 10억 이상
            #endregion

            #region 캔들 2퍼
            public bool isCandleTwoOverReal; // 실시간 2퍼 
            public int nCandleTwoOverRealTimeLineIdx; // 실시간 2퍼의 timeLineIdx
            public int nCandleTwoOverRealCount;
            public bool isCandleTwoOverRealNoLeaf; // 실시간 2퍼 + 1억원 넘음 
            public int nCandleTwoOverRealNoLeafTimeLineIdx; // 실시간 2퍼 + 1억원 넘은 timeLineIdx
            public int nCandleTwoOverRealNoLeafCount; // 실시간 2퍼 + 1억원 넘은 timeLineIdx
            #endregion

            #region 저항라인 만들기( 보완필요 )
            public int nResistPiercingTime;
            public bool isResistPeircing;
            public int nResistFs;
            public int nResistTimeLineIdx;
            public int nResistTime;
            public int nResistUpCount;
            #endregion

            #region botUp

            #region Minute botUp
            public BOTUP botUpMinute421;
            public BOTUP botUpMinute432;
            public BOTUP botUpMinute642;
            public BOTUP botUpMinute643;
            public BOTUP botUpMinute732;
            public BOTUP botUpMinute743;
            public BOTUP botUpMinute953;

            #endregion

            public void Init()
            {
                botUpMinute421.Init(0.04, 0.02, 0.01);
                botUpMinute432.Init(0.04, 0.03, 0.02);
                botUpMinute642.Init(0.06, 0.04, 0.02);
                botUpMinute643.Init(0.06, 0.04, 0.03);
                botUpMinute732.Init(0.07, 0.03, 0.02);
                botUpMinute743.Init(0.07, 0.04, 0.03);
                botUpMinute953.Init(0.09, 0.05, 0.03);
            }
            #endregion

            #region 속도차이 확인
            public int nSpeed150TotalSec;
            public int nSpeed150TotalPrevTime;

            public int nSpeed150CurSec;
            public int nSpeed150CurPrevTime;
            #endregion

            #region 호가 비율 확인
            public int nHogaGoodTotalSec;
            public int nHogaGoodTotalPrevTime;

            public int nHogaGoodCurSec;
            public int nHogaGoodCurPrevTime;
            #endregion

        }

        // +N1 -> -N2 -> +N3
        public struct BOTUP
        {
            // M은 조건
            // V는 M(조건)을 달성하고 값을 기록하기 위한 변수 
            private double fV1;
            private double fV2;
            private double fV3;
            private double fM1;
            public int nV1Time;
            public bool isM1Passed;
            private double fM2;
            public int nV2Time;
            public bool isM2Passed;
            private double fM3;
            public int nV3Time;
            public int nV1RecordTime;
            public int nV2RecordTime;
            public int nV3RecordTime;
            public bool isM3Passed; // m3 통과
            public bool isCrushed; // 전고점 돌파
            public bool isJumped; // m3 없이 전고점 돌파

            public void Init(double M1, double M2, double M3)
            {
                fM1 = M1;
                fM2 = M2;
                fM3 = M3;
            }

            public void Trace(double fV, int nT)
            {
                if (fV >= fM1) // M1조건 넘었을 때
                {
                    if (!isM1Passed || fV > fV1) // 추가갱신기록 
                    {
                        fV1 = fV;
                        nV1Time = nT;
                        if (isM2Passed) // m3에서 한순간에 전고점을 돌파했을때
                        {
                            if (!isM3Passed) // m3를 안 지나왔나
                                isJumped = true;
                            isCrushed = true;
                        }
                        isM2Passed = false;
                        isM3Passed = false;
                        fV2 = 0;
                        fV3 = 0;
                        nV2Time = 0;
                        nV3Time = 0;
                    }
                    isM1Passed = true;
                }

                if (isM1Passed)
                {
                    if (fV1 - fV >= fM2) // M2조건 넘었을떄
                    {
                        if (!isM2Passed || fV2 > fV) // 추가기록
                        {
                            fV2 = fV;
                            nV2Time = nT;
                            isM3Passed = false;
                            fV3 = 0;
                            nV3Time = 0;
                        }
                        isM2Passed = true;
                    }

                    if (isM2Passed)
                    {
                        if (fV - fV2 >= fM3) // M3조건 넘었을때
                        {
                            if (!isM3Passed || fV3 < fV) // 추가기록
                            {
                                fV3 = fV;
                                nV3Time = nT;
                            }
                            isM3Passed = true;
                        }
                    }
                }
            }

            // 실매수에서 커밋할 예정(무조건)
            public void Commit()
            {
                nV1RecordTime = nV1Time;
                nV2RecordTime = nV2Time;
                nV3RecordTime = nV3Time;
            }

            // 커밋하고 다시 중복해서 체크하지 않게
            public bool CheckIsRedundancy()
            {
                return nV1RecordTime == nV1Time && nV2RecordTime == nV2Time;
            }

            // 전고점돌파와 점프 false, m3돌파라면 커밋
            public void Confirm()
            {
                isCrushed = false;
                isJumped = false;
                if (isM3Passed)
                    Commit();
            }
        }

        public class StrategyNames
        {
            public List<string> arrFakeBuyStrategyName;
            public List<string> arrFakeResistStrategyName;
            public List<string> arrFakeAssistantStrategyName;
            public List<string> arrFakeVolatilityStrategyName;
            public List<string> arrFakeDownStrategyName;
            public List<string> arrPaperBuyStrategyName;

            public StrategyNames()
            {
                arrFakeBuyStrategyName = new List<string>();
                arrFakeResistStrategyName = new List<string>();
                arrFakeAssistantStrategyName = new List<string>();
                arrFakeVolatilityStrategyName = new List<string>();
                arrFakeDownStrategyName = new List<string>();
                arrPaperBuyStrategyName = new List<string>();

                try
                {
                    arrFakeBuyStrategyName.Add("분당 거래대금 10억이상 매수 > 매도 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 거래대금 30억이상 매수 > 매도 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 거래대금 20억이상 매수 > 매도 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 5억이상 5번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 10억이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 20억이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 10억이상 4번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 30억이상 3번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 15억이상 3번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 20억이상 3번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 10억이상 5번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 거래대금 20억이상 매수 > 매도 3번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 거래대금 30억이상 매수 > 매도 2번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 거래대금 40억이상 매수 > 매도 2번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 거래대금 15억이상 매수 > 매도 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 15억이상 5번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 15억이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 20억이상 2번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 15억이상 4번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 10억이상 3번 .. 리사이클");
                }
                catch (Exception indexError)
                {

                }

                try
                {
                    arrFakeAssistantStrategyName.Add("누적상승 + 누적하락 120퍼 이상 .. 1분제한");
                    arrFakeAssistantStrategyName.Add("누적상승 + 누적하락 200퍼 이상 .. 1분제한");
                    arrFakeAssistantStrategyName.Add("누적상승 + 누적하락 120퍼 이상 3번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("누적상승 + 누적하락 100퍼 이상 4번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당속도 1000건 이상 .. 1분제한");
                    arrFakeAssistantStrategyName.Add("분당속도 600이상 7번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당속도 600이상 5번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당 매수대금 5억이상 매수>매도 .. 1분제한");
                    arrFakeAssistantStrategyName.Add("분당 매수대금 5억이상 매수>매도 3번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당 매수대금 2억이상 매수>매도 4번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당 매수대금 2억이상 매수>매도 3번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당순위2위권 3번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당순위5위권 4번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당순위1위 2번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("전고점돌파(실시간) .. 리사이클");
                    arrFakeAssistantStrategyName.Add("전고점돌파(분봉기준) .. 리사이클");

                }
                catch (Exception indexError)
                {

                }

                try
                {
                    arrFakeResistStrategyName.Add("갭3퍼 1번 1반복.. 단한번");
                    arrFakeResistStrategyName.Add("갭4퍼 1번 1반복.. 단한번");
                    arrFakeResistStrategyName.Add("갭5퍼 1번 2반복.. 단한번");
                    arrFakeResistStrategyName.Add("갭6퍼 1번 2반복.. 단한번");
                    arrFakeResistStrategyName.Add("갭7퍼 1번 2반복.. 단한번");
                    arrFakeResistStrategyName.Add("갭10퍼 1번 3반복.. 단한번");
                    arrFakeResistStrategyName.Add("윗꼬리 1퍼 .. 1분제한");
                    arrFakeResistStrategyName.Add("윗꼬리 2퍼 .. 1분제한");
                    arrFakeResistStrategyName.Add("실시간 양봉 5퍼 .. 1분제한");
                    arrFakeResistStrategyName.Add("지나간 양봉 4퍼 .. 1분제한");
                    arrFakeResistStrategyName.Add("실시간 양봉 4퍼 .. 1분제한");
                    arrFakeResistStrategyName.Add("지나간 양봉 3퍼 .. 1분제한");
                    arrFakeResistStrategyName.Add("실시간 양봉 3퍼 .. 1분제한");
                }
                catch (Exception indexError)
                {

                }


                try
                {
                    arrFakeVolatilityStrategyName.Add("차분 5 0.03 5분주기");
                    arrFakeVolatilityStrategyName.Add("차분 5 0.05 5분주기");
                    arrFakeVolatilityStrategyName.Add("차분 10 0.03 11분주기");
                    arrFakeVolatilityStrategyName.Add("차분 10 0.04 6분주기");
                    arrFakeVolatilityStrategyName.Add("차분 20 0.05 13분주기");
                    arrFakeVolatilityStrategyName.Add("차분 20 0.04 15분주기");
                    arrFakeVolatilityStrategyName.Add("차분 15 0.04 12분주기");
                    arrFakeVolatilityStrategyName.Add("차분 5 0.07 8분주기");
                    arrFakeVolatilityStrategyName.Add("차분 3 0.05 9분주기");
                    arrFakeVolatilityStrategyName.Add("차분 4 0.04 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 37 0.04 21분주기");
                    arrFakeVolatilityStrategyName.Add("차분 35 0.06 23분주기");
                    arrFakeVolatilityStrategyName.Add("차분 30 0.04 30분주기");
                    arrFakeVolatilityStrategyName.Add("차분 30 0.03 26분주기");
                    arrFakeVolatilityStrategyName.Add("차분 7 0.04 20분주기");
                    arrFakeVolatilityStrategyName.Add("차분 23 0.045 17분주기");
                    arrFakeVolatilityStrategyName.Add("차분 20 0.05 16분주기");
                    arrFakeVolatilityStrategyName.Add("차분 13 0.033 12분주기");
                    arrFakeVolatilityStrategyName.Add("차분 20 0.03 14분주기");
                    arrFakeVolatilityStrategyName.Add("차분 36 0.06 22분주기");
                    arrFakeVolatilityStrategyName.Add("차분 22 0.06 14분주기");

                }
                catch (Exception IdxError)
                {

                }


                try
                {
                    arrFakeDownStrategyName.Add("차분 1 -0.025 1분주기");
                    arrFakeDownStrategyName.Add("차분 5 -0.03 5분주기");
                    arrFakeDownStrategyName.Add("차분 8 -0.04 8분주기");
                    arrFakeDownStrategyName.Add("차분 12 -0.05 10분주기");
                    arrFakeDownStrategyName.Add("차분 14 -0.035 12분주기");
                    arrFakeDownStrategyName.Add("차분 15 -0.025 14분주기");
                    arrFakeDownStrategyName.Add("차분 20 -0.04 15분주기");
                    arrFakeDownStrategyName.Add("차분 22 -0.045 20분주기");
                    arrFakeDownStrategyName.Add("차분 17 -0.025 15분주기");
                    arrFakeDownStrategyName.Add("차분 15 -0.03 9분주기");
                    arrFakeDownStrategyName.Add("차분 3 -0.033 7분주기");
                    arrFakeDownStrategyName.Add("차분 13 -0.036 20분주기");
                }
                catch (Exception indexError)
                {

                }

                try
                {
                    arrPaperBuyStrategyName.Add("5분전 갭포함 6퍼 .. 단한번");
                    arrPaperBuyStrategyName.Add("10분전 갭포함 8.5퍼 .. 단한번");
                    arrPaperBuyStrategyName.Add("5퍼돌파후 전고점(매매:전고점) .. 11분 주기");
                    arrPaperBuyStrategyName.Add("p7-m7>=15 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("p7-m7>=25 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("p7+m7>=30 and p7-m7>=15 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("p7+m7>=50 and p7-m7>=15 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("p9+m9>=50 and p9-m9>=15 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("p9+m9>=70 and p9-m9>=15 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("p9+m9>=90 and p9-m9>=10 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("p9+m9>=90 and p9-m9>=20 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("p9-m9>=30 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("파워자 2퍼 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("파워자 3퍼 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("파워자 4퍼 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("총 순위 1위 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("현재 분당 파워 순위 1위 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("총 순위 2위 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("현재 분당 파워 순위 2위 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("분당 속도 1000이상 p7-m7>= 15 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("전고점 총순위 30위 이전(매매:전고점) .. 11분 주기");
                    arrPaperBuyStrategyName.Add("전고점 총순위 10위 이전(매매:전고점) .. 11분 주기");
                    arrPaperBuyStrategyName.Add("분당 순위 1위 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("R각도 50도 이상 .. 11분 주기");
                    arrPaperBuyStrategyName.Add("botUp 421 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 432 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 642 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 643 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 732 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 743 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 953 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 421 전고점 일점돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 432 전고점 일점돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 642 전고점 일점돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 643 전고점 일점돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 732 전고점 일점돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 743 전고점 일점돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 953 전고점 일점돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 421 전고점 돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 432 전고점 돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 642 전고점 돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 643 전고점 돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 732 전고점 돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 743 전고점 돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("botUp 953 전고점 돌파 .. 반복");
                    arrPaperBuyStrategyName.Add("갭제외 +6.5퍼 .. 단한번");
                    arrPaperBuyStrategyName.Add("갭제외 +8퍼 .. 단한번");
                    arrPaperBuyStrategyName.Add("갭제외 +11퍼 .. 단한번");
                    arrPaperBuyStrategyName.Add("onlyUpPowerJar 4퍼 .. 11분주기");
                    arrPaperBuyStrategyName.Add("9시 30분 전 7퍼 상승 .. 단한번");
                    arrPaperBuyStrategyName.Add("9시 30분 전 10퍼 상승 .. 단한번");
                    arrPaperBuyStrategyName.Add("9시 30분 전 12퍼 상승 .. 단한번");
                    arrPaperBuyStrategyName.Add("10시 전 8퍼 상승 .. 단한번");
                    arrPaperBuyStrategyName.Add("10시 전 12퍼 상승 .. 단한번");
                    arrPaperBuyStrategyName.Add("10시 전 15퍼 상승 .. 단한번");
                }
                catch (Exception IdxError)
                {

                }


            }

            public int GetStrategySize(int signal)
            {
                if (signal == PAPER_BUY_SIGNAL)
                    return arrPaperBuyStrategyName.Count;
                else if (signal == FAKE_BUY_SIGNAL)
                    return arrFakeBuyStrategyName.Count;
                else if (signal == FAKE_ASSISTANT_SIGNAL)
                    return arrFakeAssistantStrategyName.Count;
                else if (signal == FAKE_RESIST_SIGNAL)
                    return arrFakeResistStrategyName.Count;
                else if (signal == FAKE_VOLATILE_SIGNAL)
                    return arrFakeVolatilityStrategyName.Count;
                else if (signal == FAKE_DOWN_SIGNAL)
                    return arrFakeDownStrategyName.Count;
                else
                    return -1;
            }

            public bool GetStrategyExistsByIdx(int signal, int idx)
            {
                if (signal == PAPER_BUY_SIGNAL)
                    return arrPaperBuyStrategyName.Count > idx;
                else if (signal == FAKE_BUY_SIGNAL)
                    return arrFakeBuyStrategyName.Count > idx;
                else if (signal == FAKE_ASSISTANT_SIGNAL)
                    return arrFakeAssistantStrategyName.Count > idx;
                else if (signal == FAKE_RESIST_SIGNAL)
                    return arrFakeResistStrategyName.Count > idx;
                else if (signal == FAKE_VOLATILE_SIGNAL)
                    return arrFakeVolatilityStrategyName.Count > idx;
                else if (signal == FAKE_DOWN_SIGNAL)
                    return arrFakeDownStrategyName.Count > idx;
                else
                    return false;
            }

            public string GetStrategyNameByIdx(int signal, int idx)
            {
                if (signal == PAPER_BUY_SIGNAL)
                    return arrPaperBuyStrategyName[idx];
                else if (signal == FAKE_BUY_SIGNAL)
                    return arrFakeBuyStrategyName[idx];
                else if (signal == FAKE_ASSISTANT_SIGNAL)
                    return arrFakeAssistantStrategyName[idx];
                else if (signal == FAKE_RESIST_SIGNAL)
                    return arrFakeResistStrategyName[idx];
                else if (signal == FAKE_VOLATILE_SIGNAL)
                    return arrFakeVolatilityStrategyName[idx];
                else if (signal == FAKE_DOWN_SIGNAL)
                    return arrFakeDownStrategyName[idx];
                else
                    return null;
            }
        }

        public struct StaticMember<T>
        {
            public T min;
            public T max;
            public T everage;
            public T median;
        }

        public struct FakeHistoryPiece
        {
            public int nTypeFakeTrading;
            public int nFakeStrategyNum;
            public int nSharedTime;
            public int nTimeLineIdx;
        }

    }
}
