using System;
using System.Collections.Generic;
using static AtoTrader.KiwoomLib.TimeLib;
using AtoTrader.DB;
using System.Text;

namespace AtoTrader
{
    public partial class MainForm
    {
        // ============================================
        // 각 종목이 가지는 개인 구조체
        // ============================================
        public struct EachStock
        {
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
            public int nFirstVolume;
            public long lFirstPrice;
            public double fPositiveStickPower;
            public double fNegativeStickPower;

            public MaOverN maOverN;                   // 이동평균선 변수
            public TimeLineManager timeLines1m;       // 차트데이터 변수
            public CrushManager crushMinuteManager;   // 전고점 변수
            public RankSystem rankSystem;             // 랭킹데이터 변수
            public SequenceStrategy sequenceStrategy; // 순차적전략 변
            public ReservationManager reserveMgr;     // 예약매수 관련 변수

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
            public FakeVolatilityStrategy fakeVolatilityStrategy;
            public FakeBuyStrategy fakeBuyStrategy;
            public FakeResistStrategy fakeResistStrategy;
            public FakeAssistantStrategy fakeAssistantStrategy;
            public FakeStrategyManager fakeStrategyMgr;

            // ----------------------------------
            // 초기 변수
            // ----------------------------------
            public bool isFirstCheck;    // 초기설정용 bool 변수
            public int nTodayStartPrice; // 시초가
            public int nStartGap;    // 갭 가격
            public double fStartGap; // 갭 등락율

            // ----------------------------------
            // 주식호가 변수
            // ----------------------------------
            public int nTotalBuyHogaVolume; // 총매수호가수량
            public int nTotalSellHogaVolume; // 총매도호가수량
            public int nThreeSellHogaVolume; // 매도1~3호가수량
            public int nTotalHogaVolume; //  총호가수량
            public double fHogaRatio; // 매수매도대비율

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

            // ----------------------------------
            // 체결상태 변수
            // ----------------------------------
            public int nPrevSpeedUpdateTime; // 이전기본(속도, 체결량, 순체결량)조정 시간
            public int nPrevPowerUpdateTime; // 이전가격조정 시간
            public CurStatus speedStatus;
            public CurStatus tradeStatus;
            public CurStatus pureTradeStatus;
            public CurStatus pureBuyStatus;
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
            public int nRealDataIdxVi;

            public void Init()
            {
                crushMinuteManager.Init();
                rankSystem.Init();
                sequenceStrategy.Init();
                timeLines1m.Init();
                reserveMgr.Init();
                fakeStrategyMgr.Init();


            }

            public void GetFakeFix(FakeReports rep)
            {
                try
                {
                    rep.dTradeTime = DateTime.Today;
                    rep.sCode = sCode;
                    rep.sCodeName = sCodeName;
                    rep.nLocationOfComp = COMPUTER_LOCATION;

                    // 개인구조체 정보
                    rep.fPositiveStickPower = fPositiveStickPower;
                    rep.fNegativeStickPower = fNegativeStickPower;
                    rep.nFirstVolume = nFirstVolume;
                    rep.lFirstPrice = lFirstPrice;
                    rep.nFs = nFs;
                    rep.nFb = nFb;
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
                    rep.nFakeBuyCnt = fakeBuyStrategy.nStrategyNum;
                    rep.nFakeResistCnt = fakeResistStrategy.nStrategyNum;
                    rep.nFakeAssistantCnt = fakeAssistantStrategy.nStrategyNum;
                    rep.nFakeVolatilityCnt = fakeVolatilityStrategy.nStrategyNum;
                    rep.nFakeBuyMinuteCnt = fakeBuyStrategy.nMinuteLocationCount;
                    rep.nFakeResistMinuteCnt = fakeResistStrategy.nMinuteLocationCount;
                    rep.nFakeAssistantMinuteCnt = fakeAssistantStrategy.nMinuteLocationCount;
                    rep.nFakeVolatilityMinuteCnt = fakeVolatilityStrategy.nMinuteLocationCount;
                    rep.nFakeBuyUpperCnt = fakeBuyStrategy.nUpperCount;
                    rep.nFakeResistUpperCnt = fakeResistStrategy.nUpperCount;
                    rep.nFakeAssistantUpperCnt = fakeAssistantStrategy.nUpperCount;
                    rep.nFakeVolatilityUpperCnt = fakeVolatilityStrategy.nUpperCount;
                    rep.nTotalFakeCnt = fakeStrategyMgr.nTotalFakeCount;
                    rep.nTotalFakeMinuteCnt = fakeStrategyMgr.nTotalFakeMinuteAreaNum;
                    rep.nUpCandleCnt = timeLines1m.upCandleList.Count;
                    rep.nDownCandleCnt = timeLines1m.downCandleList.Count;
                    rep.nUpTailCnt = timeLines1m.upTailList.Count;
                    rep.nDownTailCnt = timeLines1m.downTailList.Count;
                    rep.nShootingCnt = timeLines1m.shootingList.Count;
                    rep.nCrushCnt = crushMinuteManager.nCurCnt;
                    rep.nCrushUpCnt = crushMinuteManager.nUpCnt;
                    rep.nCrushDownCnt = crushMinuteManager.nDownCnt;
                    rep.nCrushSpecialDownCnt = crushMinuteManager.nSpecialDownCnt;

                    rep.nYesterdayEndPrice = nYesterdayEndPrice;
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
                        $"실매수횟수 : {fakeVolatilityStrategy.nStrategyNum}{NEW_LINE}" +
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
                        $"총 Arrow : {fakeStrategyMgr.nTotalFakeCount}{NEW_LINE}" +
                        $"총 ArrowMinute : {fakeStrategyMgr.nTotalFakeMinuteAreaNum}{NEW_LINE}" +
                        $"================= 분 봉 ============={NEW_LINE}" +
                        $"위캔들 : {timeLines1m.upCandleList.Count}{NEW_LINE}" +
                        $"아래캔들 : {timeLines1m.downCandleList.Count}{NEW_LINE}" +
                        $"위꼬리 : {timeLines1m.upTailList.Count}{NEW_LINE}" +
                        $"아래꼬리 : {timeLines1m.downTailList.Count}{NEW_LINE}" +
                        $"슈팅 : {timeLines1m.shootingList.Count}{NEW_LINE}" +
                        $"전고점 카운트 : {crushMinuteManager.nCurCnt}{NEW_LINE}" +
                        $"전고점   업 : {crushMinuteManager.nUpCnt}{NEW_LINE}" +
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

        // ============================================
        // AI서비스 웨이터 큐에 저장하기 위한 구조체변수
        // ============================================
        public struct AIResponseSlot
        {
            public int nEaIdx;
            public int nMMFNumber;
            public int nRequestId;
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

        // 예약 관리용 클래스
        public struct ReservationManager
        {
            public int nFirstTime;  // 이 종목에 처음 전략이 통과된 시점
            public double fFirstPower; // 그때 파워

            public int nLastTime; // 종목이 가장 최근에 통과된 시점
            public double fLastPower; // 그때 파워
            

            public void Approach(int nTime, double fPower)
            {
                if(nFirstTime == 0)
                {
                    nFirstTime = nTime;
                    fFirstPower = fPower;
                }
                nLastTime = nTime;
                fLastPower = fPower;
            }

            public List<ReservedPoint> listReservation;
            public void Init()
            {
                listReservation = new List<ReservedPoint>();
            }
        }

        // 하나의 예약 블럭
        public class ReservedPoint
        {
            // condition values.
            public bool isReserveEnd;  // 예약 종료인지 아닌지


            public double fReservePower; // 생성시점 파워
            public int nReserveTime; // 생성시점 시간
            public int nReservePrice; // 생성시점 가격

            public double fMaxPower; // 최대 가격
            public double fMinusPower; // 마이너스 파워
            public int nTargetLimitTime; // 예약매수 제한시간
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
                // 바닥 가격 잡기
                if (nBottomPriceAfterBuy == 0 || nBottomPriceAfterBuy > nDownPrice) 
                {
                    nBottomPriceAfterBuy = nDownPrice;
                    nBottomTimeAfterBuy = nT;
                    fBottomPowerWithFeeAfterBuy = (double)(nBottomPriceAfterBuy - nBuyedPrice) / nDenomPrice - REAL_STOCK_COMMISSION;

                    nTopPriceAfterBuy = 0;
                    nTopTimeAfterBuy = 0;
                    fTopPowerWithFeeAfterBuy = 0;
                }

                // 바닥 이후 탑 가격 잡기
                if (nTopPriceAfterBuy < nUpPrice)
                {
                    nTopPriceAfterBuy = nUpPrice;
                    nTopTimeAfterBuy = nT;
                    fTopPowerWithFeeAfterBuy = (double)(nTopPriceAfterBuy - nBuyedPrice) / nDenomPrice - REAL_STOCK_COMMISSION;
                }

                // 매수된 후부터 가장 높은 가격을 측정한다.(Fb 기준)
                if (nMaxPriceAfterBuy < nUpPrice) // fb로 측정한다.
                {
                    nMaxPriceAfterBuy = nUpPrice;
                    nMaxTimeAfterBuy = nT;
                    fMaxPowerWithFeeAfterBuy = (double)(nMaxPriceAfterBuy - nBuyedPrice) / nDenomPrice - REAL_STOCK_COMMISSION;

                    nBoundBottomPriceAfterBuy = 0;
                    nBoundBottomTimeAfterBuy = 0;
                    fBoundBottomPowerWithFeeAfterBuy = 0;

                    nBoundTopPriceAfterBuy = 0;
                    nBoundTopTimeAfterBuy = 0;
                    fBoundTopPowerWithFeeAfterBuy = 0;
                }

                // 맥스 찍은 후 최저점
                if(nBoundBottomPriceAfterBuy == 0  || nBoundBottomPriceAfterBuy > nDownPrice)
                {
                    nBoundBottomPriceAfterBuy = nDownPrice;
                    nBoundBottomTimeAfterBuy = nT;
                    fBoundBottomPowerWithFeeAfterBuy = (double)(nBoundBottomPriceAfterBuy - nBuyedPrice) / nDenomPrice - REAL_STOCK_COMMISSION;

                    nBoundTopPriceAfterBuy = 0;
                    nBoundTopTimeAfterBuy = 0;
                    fBoundTopPowerWithFeeAfterBuy = 0;
                }

                // 맥스 찍은 후 바닥 이후 최고점
                if (nBoundTopPriceAfterBuy < nUpPrice)
                {
                    nBoundTopPriceAfterBuy = nUpPrice;
                    nBoundTopTimeAfterBuy = nT;
                    fBoundTopPowerWithFeeAfterBuy = (double)(nBoundTopPriceAfterBuy - nBuyedPrice) / nDenomPrice - REAL_STOCK_COMMISSION;
                }

                // 매수된 후부터 가장 높은 가격을 발견하기 전까지의 민 값을 측정한다. 
                if (nMinPriceAfterBuyBeforeMax == 0 || nBottomTimeAfterBuy < nMaxTimeAfterBuy)
                {
                    nMinPriceAfterBuyBeforeMax = nBottomPriceAfterBuy;
                    nMinTimeAfterBuyBeforeMax = nBottomTimeAfterBuy;
                    fMinPowerWithFeeAfterBuyBeforeMax = fBottomPowerWithFeeAfterBuy;
                }

            }
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
            public List<(int, double)> upCandleList;
            public List<(int, double)> downCandleList;
            public List<(int, double)> shootingList;
            public List<(int, double)> upTailList;
            public List<(int, double)> downTailList;

            public TimeLine[] arrTimeLine;

            public void Init()
            {
                upCandleList = new List<(int, double)>();
                downCandleList = new List<(int, double)>();
                upTailList = new List<(int, double)>();
                downTailList = new List<(int, double)>();
                shootingList = new List<(int, double)>();
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
            public double fMaxDownFs;
            public double fMaxMa20m;
            public double fMaxMa1h;
            public double fMaxMa2h;

            public double fCurDownFs;
            public double fCurMa20m;
            public double fCurMa1h;
            public double fCurMa2h;

            public double fCurGapMa20m;
            public double fCurGapMa1h;
            public double fCurGapMa2h;

            public int nMaxDownFsTime;
            public int nMaxMa20mTime;
            public int nMaxMa1hTime;
            public int nMaxMa2hTime;

            public int nUpCntMa20m;
            public int nUpCntMa1h;
            public int nUpCntMa2h;

            public int nDownCntMa20m;
            public int nDownCntMa1h;
            public int nDownCntMa2h;
        }


        /// <summary>
        /// ABOUT 실제전략
        /// </summary>
        public struct fakeVolatilityStrategy
        {

            public int[] arrLastTouch; // 가장최근에 해당전략 요청한 시간
            public int[] arrStrategy; // 전략당 배열
            public int[] arrMinuteIdx;
            public int[] arrSpecificStrategy; // 배열의 각 슬롯당 전략
            public int[] arrAssistantTime;
            public int[] arrAssistantPrice;

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
            public int nMinuteLocationCount;
            public int nPrevMinuteIdx;

            public int[] arrPrevMinuteIdx;



            //
            public int[] arrBuyPrice;
            public int[] arrBuyTime;

            public int nSharedMinuteLocationCount;
            public int nSharedPrevMinuteIdx;

            // =========================================================================
            // 전략별 추가 변수들 
            // =========================================================================
            // ***  번호와 변수  ***
            // 추가매수 전략 0번 
            // 직접입력 전략 마지막 번
            public bool isManualOrderSignal;
            public int nManualEndurationTime; // 매수버튼을 눌렀는데 한참동안 매수가 안되면 문제가 있는거니 취소

            public int nTotalFail;
            // 임시용 
            public bool isOrderCheck;
          
            public int nCurBarBuyCount;
            public int nFakePrevTimeLineIdx;
           


            // 페이크 DB 관련 변수
            public int nFakeAccumPassed;
            public int nFakeAccumTried;
            public int nAIPassed;
            public int nAIPrevTimeLineIdx;
            public int nAIStepMinuteCount;
            public int nAIJumpDiffMinuteCount;
            public int nFakeAIPrevTimeLineIdx;
            public int nFakeAIStepMinuteCount;
            public int nFakeAIJumpDiffMinuteCount;

            public int nTotalBlockCount;
            public int nTotalFakeCount;
            
            // END-- 페이크 DB 관련 변수


            // -------------------------------------------------------------------------------
            // END ---- 전략별  추가변수들
            // -------------------------------------------------------------------------------
            public void Init(int s)
            {
                arrStrategy = new int[s];
                arrLastTouch = new int[s];
                arrPrevMinuteIdx = new int[s];

                arrMinuteIdx = new int[REAL_BUY_MAX_NUM];
                arrBuyTime = new int[REAL_BUY_MAX_NUM];
                arrBuyPrice = new int[REAL_BUY_MAX_NUM];
                arrSpecificStrategy = new int[REAL_BUY_MAX_NUM];

                nManualEndurationTime = 200;
                nPrevMinuteIdx = -1;
          
              
            }

        }


        public struct FakeStrategyManager
        {
            public int nFakePrevTimeLineIdx;
            public int nSharedPrevMinuteIdx;
            public int nSharedMinuteLocationCount;

            public List<FakeHistoryPiece> listFakeHistoryPiece;
            public List<FakeDBRecordInfo> fd;

            public int nFakeBuyNum;
            public int nFakeResistNum;
            public int nFakeAssistantNum;
            public int nFakeVolatilityNum;

            public int nFakeBuyMinuteAreaNum;
            public int nFakeResistMinuteAreaNum;
            public int nFakeAssistantMinuteAreaNum;
            public int nFakeVolatilityMinuteAreaNum;

            public int nTotalFakeMinuteAreaNum;
            public int nTotalFakeCount;

            public int nFakeAccumPassed;
            public int nFakeAIJumpDiffMinuteCount;
            public int nFakeAIPrevTimeLineIdx;
            public int nFakeAIStepMinuteCount;
            public int nFakeAccumTried;


            public void Init()
            {
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
            public int nMinuteLocationCount;
            
            public int[] arrPrevMinuteIdx;

        }


        public class FakeVolatilityStrategy : FakeFrame
        {
            public FakeVolatilityStrategy(int t, int s)
            {
                nFakeType = t;

                arrStrategy = new int[s];
                arrLastTouch = new int[s];
                arrPrevMinuteIdx = new int[s];

                arrMinuteIdx = new int[REAL_BUY_MAX_NUM];
                arrBuyTime = new int[REAL_BUY_MAX_NUM];
                arrBuyPrice = new int[REAL_BUY_MAX_NUM];
                arrSpecificStrategy = new int[REAL_BUY_MAX_NUM];
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
            public FakeResistStrategy(int t,int s)
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

            public FakeAssistantStrategy(int t,int s)
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

        public class FakeDBRecordInfo
        {
            public FakeReports fr;

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
                fr = new FakeReports();
            }
        }
        /// <summary>
        /// P버튼의 결과 구조체
        /// </summary>
        public struct PResult
        {
            public StaticMember<double> rAngle;
            public StaticMember<double> hAngle;
            public StaticMember<double> dAngle;
            public StaticMember<double> tAngle;

            public StaticMember<double> rBAngle; // B(Buyed)각도 
            public StaticMember<double> hBAngle;
            public StaticMember<double> dBAngle;
            public StaticMember<double> tBAngle;

            public bool isDownFs20m;
            public bool isDownFs1h;
            public bool isDownFs2h;
            public bool is1h2h;
            public bool is20m1h;

            public bool isBDownFs2m;
            public bool isBDownFs10m;
            public bool isBDownFs20m;
            public bool isB2m10m;
            public bool isB10m20m;

            public void Init()
            {
                rAngle.min = 0;
                rAngle.max = 0;
                rAngle.everage = 0;
                rAngle.median = 0;

                hAngle.min = 0;
                hAngle.max = 0;
                hAngle.everage = 0;
                hAngle.median = 0;

                dAngle.min = 0;
                dAngle.max = 0;
                dAngle.everage = 0;
                dAngle.median = 0;

                tAngle.min = 0;
                tAngle.max = 0;
                tAngle.everage = 0;
                tAngle.median = 0;

                rBAngle.min = 0;
                rBAngle.max = 0;
                rBAngle.everage = 0;
                rBAngle.median = 0;

                hBAngle.min = 0;
                hBAngle.max = 0;
                hBAngle.everage = 0;
                hBAngle.median = 0;

                dBAngle.min = 0;
                dBAngle.max = 0;
                dBAngle.everage = 0;
                dBAngle.median = 0;

                tBAngle.min = 0;
                tBAngle.max = 0;
                tBAngle.everage = 0;
                tBAngle.median = 0;

                isDownFs20m = false;
                isDownFs1h = false;
                isDownFs2h = false;
                is20m1h = false;
                is1h2h = false;

                isBDownFs2m = false;
                isBDownFs10m = false;
                isBDownFs20m = false;
                isB2m10m = false;
                isB10m20m = false;
            }
        }

        // 조건의 순서를 연결해 매수 타이밍을 잡으려함.
        public struct SequenceStrategy
        {
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
            public List<string> arrFakeVolatilityStrategyName;
            public List<string> arrFakeBuyStrategyName;
            public List<string> arrFakeResistStrategyName;
            public List<string> arrFakeAssistantStrategyName;
            public StrategyNames()
            {
                arrFakeVolatilityStrategyName = new List<string>();
                arrFakeBuyStrategyName =new List<string>();
                arrFakeResistStrategyName = new List<string>();
                arrFakeAssistantStrategyName = new List<string>();

                try
                {
                    arrFakeBuyStrategyName.Add("분당 거래대금 10억이상 매수 > 매도 시가총액200위이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 거래대금 30억이상 매수 > 매도 시가총액200위이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 거래대금 20억이상 매수 > 매도 시가총액200위이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 5억이상 시가총액200위이상 5번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 10억이상 시가총액200위이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 20억이상 시가총액200위이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 10억이상 시가총액200위이상 4번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 거래대금 30억이상 매수 > 매도 시가총액200위이상 3번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 15억이상 시가총액200위이상 3번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 20억이상 시가총액200위이상 3번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 10억이상 시가총액200위이상 4번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 거래대금 20억이상 매수 > 매도 시가총액200위이상 3번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 거래대금 30억이상 매수 > 매도 시가총액200위이상 2번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 거래대금 40억이상 매수 > 매도 시가총액200위이상 2번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 거래대금 15억이상 매수 > 매도 시가총액200위이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 15억이상 시가총액200위이상 5번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 15억이상 시가총액200위이상 .. 1분제한");
                    arrFakeBuyStrategyName.Add("분당 매수대금 20억이상 시가총액200위이상 2번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 15억이상 시가총액200위이상 4번 .. 리사이클");
                    arrFakeBuyStrategyName.Add("분당 매수대금 10억이상 시가총액200위이상 3번 .. 리사이클");
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
                    arrFakeAssistantStrategyName.Add("분당 속도 1000건 이상 시가총액200위이상 .. 1분제한");
                    arrFakeAssistantStrategyName.Add("분당속도 600이상 시가총액200위이상 7번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당속도 600이상 시가총액200위이상 5번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당 매수대금 5억이상 매수>매도 시가총액200위이상 .. 1분제한");
                    arrFakeAssistantStrategyName.Add("분당 매수대금 5억이상 매수>매도 시가총액200위이상 3번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당 매수대금 2억이상 매수>매도 시가총액200위이상 4번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당 매수대금 2억이상 매수>매도 시가총액200위이상 3번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당순위2위권 시가총액200위이상 3번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당순위5위권 시가총액200위이상 4번 .. 리사이클");
                    arrFakeAssistantStrategyName.Add("분당순위1위 시가총액200위이상 2번 .. 리사이클");
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
                    arrFakeVolatilityStrategyName.Add("차분 1 0.015 1분주기");
                    arrFakeVolatilityStrategyName.Add("차분 1 0.025 1분주기");
                    arrFakeVolatilityStrategyName.Add("차분 3 0.02 3분주기");
                    arrFakeVolatilityStrategyName.Add("차분 5 0.02 5분주기");
                    arrFakeVolatilityStrategyName.Add("차분 5 0.03 5분주기");
                    arrFakeVolatilityStrategyName.Add("차분 5 0.05 5분주기");
                    arrFakeVolatilityStrategyName.Add("차분 10 0.03 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 10 0.04 6분주기");
                    arrFakeVolatilityStrategyName.Add("차분 20 0.05 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 20 0.04 15분주기");
                    arrFakeVolatilityStrategyName.Add("차분 15 0.04 12분주기");
                    arrFakeVolatilityStrategyName.Add("차분 5 0.07 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 3 0.05 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 4 0.04 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 20 0.1 15분주기");
                    arrFakeVolatilityStrategyName.Add("차분 30 0.05 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 23 0.12 20분주기");
                    arrFakeVolatilityStrategyName.Add("차분 37 0.04 20분주기");
                    arrFakeVolatilityStrategyName.Add("차분 34 0.05 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 35 0.07 20분주기");
                    arrFakeVolatilityStrategyName.Add("차분 30 0.04 30분주기");
                    arrFakeVolatilityStrategyName.Add("차분 30 0.03 20분주기");
                    arrFakeVolatilityStrategyName.Add("차분 7 0.04 20분주기");
                    arrFakeVolatilityStrategyName.Add("차분 8 0.02 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 12 0.02 11분주기");
                    arrFakeVolatilityStrategyName.Add("차분 13 0.03 10분주기");
                    arrFakeVolatilityStrategyName.Add("차분 16 0.025 6분주기");
                    arrFakeVolatilityStrategyName.Add("차분 23 0.045 17분주기");
                    arrFakeVolatilityStrategyName.Add("차분 20 0.05 16분주기");
                    arrFakeVolatilityStrategyName.Add("차분 16 0.03 7분주기");
                    arrFakeVolatilityStrategyName.Add("차분 13 0.033 12분주기");
                    arrFakeVolatilityStrategyName.Add("차분 17 0.023 11분주기");
                    arrFakeVolatilityStrategyName.Add("차분 40 0.021 15분주기");
                    arrFakeVolatilityStrategyName.Add("차분 30 0.02 34분주기");
                    arrFakeVolatilityStrategyName.Add("차분 20 0.03 14분주기");
                    arrFakeVolatilityStrategyName.Add("차분 36 0.06 22분주기");
                    arrFakeVolatilityStrategyName.Add("차분 22 0.06 14분주기");

                }
                catch (Exception IdxError)
                {

                }
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
