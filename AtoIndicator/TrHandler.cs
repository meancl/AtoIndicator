using System;
using System.IO;
using static AtoIndicator.KiwoomLib.PricingLib;
using static AtoIndicator.KiwoomLib.Errors;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AtoIndicator
{
    public partial class MainForm
    {
        #region 변수
        public Holdings[] holdingsArray; // 현재 보유주식을 담을 구조체 배열 // tr

        public int nYesterdayUndisposalBuyPrice = 0; // 오늘 봤을때 어제자 미처분수량의 총매수가격

        public int nTodayDisposalBuyPrice = 0;
        public int nTodayDisposalSellPrice = 0;
        #endregion

        #region RequestHoldings
        // ============================================
        // 계좌평가잔고내역요청 TR요청메소드
        // CommRqData 3번째 인자 sPrevNext가 0일 경우 처음 20개의 종목을 요청하고
        // 2일 경우 초기20개 초과되는 종목들을 계속해서 요청한다.
        // ============================================
        public int nHoldingCnt; // 총 보유주식의 수 // tr
        public int nCurHoldingsIdx; // 보유주식을 담을때 사용하는 인덱스 변수  // tr
        public string sHoldingsTRSrc;
        public bool isRequestHoldingsUsing;
        public StringBuilder requestHoldingsSb = new StringBuilder();
        private void RequestHoldings(int nPrevNext)
        {
            // 진입조건
            // 처음(0)이면 not using이어야하고
            // 중복(2)이면 using 이어야 함
            if (((nPrevNext == 0) == (!isRequestHoldingsUsing)) || isCtrlPushed)
            {
                if (nPrevNext == 0)
                {
                    isRequestHoldingsUsing = true;
                    nHoldingCnt = 0;
                    nCurHoldingsIdx = 0;
                    sHoldingsTRSrc = GetScreenNum();
                }
                axKHOpenAPI1.SetInputValue("계좌번호", sAccountNum);
                axKHOpenAPI1.SetInputValue("비밀번호", "");
                axKHOpenAPI1.SetInputValue("비밀번호입력매체구분", "00");
                axKHOpenAPI1.SetInputValue("조회구분", "2"); // 1:합산 2:개별
                int nTrResult = axKHOpenAPI1.CommRqData("계좌평가잔고내역요청", "opw00018", nPrevNext, sHoldingsTRSrc);
                if (nTrResult == OP_ERR_NONE)
                {
                    if (nPrevNext == 0)
                    {
                        requestHoldingsSb.Clear();
                    }
                }
                else if (nTrResult == OP_ERR_OVERFLOW1 || nTrResult == OP_ERR_OVERFLOW2 || nTrResult == OP_ERR_OVERFLOW3)
                {
                    Task.Run(() =>
                    {
                        Thread.Sleep(1200);
                        RequestHoldings(2);
                    });
                }
                else
                {
                    ShutOffScreen(sHoldingsTRSrc);
                    isRequestHoldingsUsing = false;
                    PrintLog($"계좌평가잔고내역요청 TR이 비정상처리됐습니다.");
                }
            }
            else
                PrintLog($"계좌평가잔고내역요청 TR이 사용중입니다.");
        }
        #endregion

        #region RequestDeposit
        // ============================================
        // 예수금상세현황요청 TR요청메소드
        // ============================================
        //public string sDepositTRSrc; // tr이 매우 빠르게 진행되더라도 증권사에서는 처리시간내에 1개이상의 같은TR이 들어오면 그냥 1번만 수행하고 끝난다.
        public bool isRequestDepositUsing;
        public string sRequestDepositTrSrc;
        private void RequestDeposit()
        {
            if (!isRequestDepositUsing || isCtrlPushed)
            {
                isRequestDepositUsing = true;
                sRequestDepositTrSrc = GetScreenNum();
                axKHOpenAPI1.SetInputValue("계좌번호", sAccountNum);
                axKHOpenAPI1.SetInputValue("비밀번호", "");
                axKHOpenAPI1.SetInputValue("비밀번호입력매체구분", "00");
                axKHOpenAPI1.SetInputValue("조회구분", "2");
                int nTrResult = axKHOpenAPI1.CommRqData("예수금상세현황요청", "opw00001", 0, sRequestDepositTrSrc);
                if (nTrResult != OP_ERR_NONE) // 오류 발생
                {
                    isRequestDepositUsing = false;
                    ShutOffScreen(sRequestDepositTrSrc);

                    PrintLog($"예수금상세현황요청 TR이 비정상처리됐습니다.");
                }
            }
            else
                PrintLog($"예수금상세현황요청 TR이 사용중입니다.");

        }
        #endregion

        #region RequestTradeResult
        // ============================================
        // 당일실현손익상세요청 TR요청메소드
        // ============================================
        public int nTodayResultTRIdx;
        public int nTodayResultTRCount;
        public string sTodayResultTRSrc;
        public bool isRequestTradeResultUsing;
        public int nC;
        string sPrevCode = "";
        string sPrevCodeName = "";
        int nTotalTradeVolume = 0; // 전체체결량
        double fTotalBuyPrice = 0; // 전체매수가
        double fTotalSellPrice = 0; // 전체체결가
        public StringBuilder todayResultSb = new StringBuilder();
        private void RequestTradeResult(int nPrevNext)
        {
            // 진입조건
            // 처음(0)이면 not using이어야하고
            // 중복(2)이면 using이어야 함
            if ((nPrevNext == 0) == (!isRequestTradeResultUsing) || isCtrlPushed)
            {
                if (nPrevNext == 0)
                {
                    isRequestTradeResultUsing = true;
                    nTodayResultTRIdx = 0;
                    nTodayResultTRCount = 0;
                    sTodayResultTRSrc = GetScreenNum();
                }
                axKHOpenAPI1.SetInputValue("계좌번호", sAccountNum);
                axKHOpenAPI1.SetInputValue("비밀번호", "");
                axKHOpenAPI1.SetInputValue("종목코드", "");
                int nTrResult = axKHOpenAPI1.CommRqData("당일실현손익상세요청", "opt10077", nPrevNext, sTodayResultTRSrc);
                if (nTrResult == OP_ERR_NONE)
                {
                    if (nPrevNext == 0)
                        todayResultSb.Clear();
                }
                else if (nTrResult == OP_ERR_OVERFLOW1 || nTrResult == OP_ERR_OVERFLOW2 || nTrResult == OP_ERR_OVERFLOW3)
                {
                    Task.Run(() =>
                    {
                        Thread.Sleep(1200);
                        RequestTradeResult(2);
                    });
                }
                else
                {
                    isRequestTradeResultUsing = false;
                    ShutOffScreen(sTodayResultTRSrc);
                    PrintLog($"당일실현손익상세요청 TR이 비정상처리됐습니다.");
                }

            }
            else
            {
                PrintLog($"당일실현손익상세요청 TR이 사용중입니다.");
            }
        }
        #endregion

        #region ReceiveMsg 핸들러
        public void OnReceiveMsgHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            if (e.sRQName.Length > SEND_ORDER_ERROR_CHECK_PREFIX.Length && e.sRQName.Substring(0, SEND_ORDER_ERROR_CHECK_PREFIX.Length).Equals(SEND_ORDER_ERROR_CHECK_PREFIX))
            {
                string sRqName = e.sRQName.Substring(SEND_ORDER_ERROR_CHECK_PREFIX.Length);
                string[] sArr = sRqName.Split();
                int nEaIdx = int.Parse(sArr[1]);
            }
        }
        #endregion  

        #region TR 핸들러
        // ============================================
        // TR 이벤트발생시 핸들러 메소드
        // ============================================
        public void OnReceiveTrDataHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            #region RequestDeposit
            if (e.sRQName.Equals("예수금상세현황요청"))
            {
                nCurDeposit = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, 0, "예수금")));
                if (nCurDepositCalc == 0)
                {
                    nCurDepositCalc = nCurDeposit;
                    depositCalcLabel.Text = $"{nCurDepositCalc}(원)";
                    PrintLog("계산용예수금 세팅 완료"); //++
                }
                this.Enabled = true;
                myDepositLabel.Text = $"{nCurDeposit}(원)";
                PrintLog("예수금 세팅 완료"); //++

                ShutOffScreen(sRequestDepositTrSrc); // 예수금상세현황요청 해당화면번호 꺼줍니다.

                isRequestDepositUsing = false;
            }
            #endregion
            #region RequestHoldings
            else if (e.sRQName.Equals("계좌평가잔고내역요청"))
            {
                int rows = axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRecordName);
                nHoldingCnt += rows;

                for (int myMoneyIdx = 0; nCurHoldingsIdx < nHoldingCnt; nCurHoldingsIdx++, myMoneyIdx++)
                {
                    holdingsArray[nCurHoldingsIdx].sCode = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, myMoneyIdx, "종목번호").Trim().Substring(1);
                    holdingsArray[nCurHoldingsIdx].sCodeName = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, myMoneyIdx, "종목명").Trim();
                    holdingsArray[nCurHoldingsIdx].fYield = double.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, myMoneyIdx, "수익률(%)"));
                    holdingsArray[nCurHoldingsIdx].nHoldingQty = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, myMoneyIdx, "보유수량")));
                    holdingsArray[nCurHoldingsIdx].nBuyedPrice = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, myMoneyIdx, "매입가")));
                    holdingsArray[nCurHoldingsIdx].nCurPrice = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, myMoneyIdx, "현재가")));
                    holdingsArray[nCurHoldingsIdx].nTotalPL = int.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, myMoneyIdx, "평가손익"));
                    holdingsArray[nCurHoldingsIdx].nNumPossibleToSell = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, myMoneyIdx, "매매가능수량")));

                }

                if (e.sPrevNext.Equals("2"))
                {
                    RequestHoldings(2);
                }
                else // 보유잔고 확인 끝
                {
                    if (nHoldingCnt == 0)
                    {
                        PrintLog("현재 보유종목이 없습니다.");//++
                    }
                    else
                    {
                        requestHoldingsSb.Append($"============================================= 보유 잔고 ============================================{NEW_LINE}");
                        for (int myStockIdx = 0; myStockIdx < nHoldingCnt; myStockIdx++)
                        {
                            requestHoldingsSb.Append($"{(myStockIdx + 1)} 종목번호 : {holdingsArray[myStockIdx].sCode}, 종목명 : {holdingsArray[myStockIdx].sCodeName}, 보유수량 : {holdingsArray[myStockIdx].nHoldingQty}, 매매가능수량 : {holdingsArray[myStockIdx].nNumPossibleToSell}, 평가손익 : {holdingsArray[myStockIdx].nTotalPL}{NEW_LINE}");
                        }
                        PrintLog(requestHoldingsSb.ToString()); //++
                    }
                    ShutOffScreen(sHoldingsTRSrc); // 계좌평가잔고내역요청 해당화면번호 꺼줍니다.

                    isRequestHoldingsUsing = false;
                }
            } // END ---- e.sRQName.Equals("계좌평가잔고내역요청")
            #endregion
            #region RequestTradeResult
            else if (e.sRQName.Equals("당일실현손익상세요청"))
            {
                if (nTodayResultTRIdx == 0)
                {
                    int nTodayResult = int.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, 0, "당일실현손익"));
                    todayResultSb.Append($"==============================================================================================================================={NEW_LINE}");
                    todayResultSb.Append($"=====================================================     당일실현손익상세     ===================================================={NEW_LINE}");
                    todayResultSb.Append($"당일실현손익 : {nTodayResult}(원){NEW_LINE}");
                    nC = 1; // 몇번째 종목인지
                }


                int rows = axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRecordName);
                nTodayResultTRCount += rows;

                string sCode;
                string sCodeName;
                int nTradeVolume;
                double fBuyPrice;
                double fTradePrice;
                double fYield;


                for (int todayProfitIdx = 0; nTodayResultTRIdx < nTodayResultTRCount; todayProfitIdx++, nTodayResultTRIdx++)
                {
                    sCode = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, todayProfitIdx, "종목코드").Trim().Substring(1);
                    sCodeName = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, todayProfitIdx, "종목명").Trim();
                    fYield = double.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, todayProfitIdx, "손익율")); // 이건 흠... 안써
                    nTradeVolume = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, todayProfitIdx, "체결량")));
                    fBuyPrice = Math.Abs(double.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, todayProfitIdx, "매입단가")));
                    fTradePrice = Math.Abs(double.Parse(axKHOpenAPI1.GetCommData(e.sTrCode, e.sRecordName, todayProfitIdx, "체결가")));

                    if (!sPrevCode.Equals(sCode)) // 같지 않다면
                    {
                        if (sPrevCode.Equals("")) // 초기화된 상태라면
                        {
                            // 할게 없다   
                        }
                        else // 종목이 변경됐다면
                        {
                            // 이전까지의 종목의 당일실현손익을 출력한다.
                            todayResultSb.Append($"{nC++}번째 종목명 : {sPrevCodeName}, 종목코드 : {sPrevCode}, 거래량 : {nTotalTradeVolume}, 매수단가 : {Math.Round(fTotalBuyPrice / nTotalTradeVolume, 2)}, 매도단가 : {Math.Round(fTotalSellPrice / nTotalTradeVolume, 2)}, 손익율 : {Math.Round(((fTotalSellPrice - fTotalBuyPrice) / fTotalBuyPrice - REAL_STOCK_COMMISSION) * 100, 2)}(%), 투입금액 : {Math.Round(fTotalBuyPrice / MILLION, 2)}(백만원){NEW_LINE}");
                        }
                        // 전체변수들을 초기화한다.
                        sPrevCode = sCode;
                        sPrevCodeName = sCodeName;
                        nTotalTradeVolume = nTradeVolume;
                        fTotalBuyPrice = fBuyPrice * nTradeVolume;
                        fTotalSellPrice = fTradePrice * nTradeVolume;
                    }
                    else // 같다면
                    {
                        // 값을 계속 더해준다.
                        nTotalTradeVolume += nTradeVolume;
                        fTotalBuyPrice += fBuyPrice * nTradeVolume;
                        fTotalSellPrice += fTradePrice * nTradeVolume;
                    }
                }

                if (e.sPrevNext.Equals("2"))
                {
                    RequestTradeResult(2);
                }
                else
                {
                    ShutOffScreen(sTodayResultTRSrc); // 당일실현손익상세요청 해당화면번호 꺼줍니다.
                    isRequestTradeResultUsing = false;
                    PrintLog(todayResultSb.ToString());
                }

            } // END ---- e.sRQName.Equals("당일실현손익상세요청")
            #endregion
            else
            {
                if (e.sRQName.Length > SEND_ORDER_ERROR_CHECK_PREFIX.Length)
                {
                    string sRQName = e.sRQName.Substring(0, SEND_ORDER_ERROR_CHECK_PREFIX.Length);
                    #region Order Response TR
                    if (sRQName.Equals(SEND_ORDER_ERROR_CHECK_PREFIX)) // 주식주문의 경우(TR이 먼저 온다)
                    {
                        try
                        {
                            sRQName = e.sRQName.Substring(SEND_ORDER_ERROR_CHECK_PREFIX.Length);

                            string[] sArr = sRQName.Split(); // 메시지가 잘못올때가 있는데..? ( 화면번호를 통해 nErrorOwner와 nErrorBuyedSlot 유추하기 )
                            string sTypeReq = sArr[0].Trim();
                            int nEaReq = int.Parse(sArr[1]);

                            #region FAILURE
                            if (axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "주문번호").Trim().Equals("")) // 오류가 발견됐다면
                            {
                                BuyedSlot slot = GetSlotFromScreen(e.sScrNo);

                                if (sTypeReq.Equals(ORDER_NEW_BUY)) // 신규매수 비정상처리
                                {
                                    if (slot != null)
                                    {
                                        if (slot.isBuying)
                                        {
                                            PrintLog($"시간 : {nSharedTime}  종목코드 : {ea[nEaReq].sCode}  종목명 : {ea[nEaReq].sCodeName}  e화면번호 : {e.sScrNo}  매수가 비정상처리됐습니다.", nEaReq);
                                            slot.isBuying = false;
                                            ea[nEaReq].myTradeManager.nBuyReqCnt--;
                                            ShutOffScreen(e.sScrNo);
                                            slot.isBuyBanned = true;
                                            nCurDepositCalc += slot.nOrderPrice * slot.nOrderVolume + ea[nEaReq].feeMgr.GetRoughFee(slot.nOrderPrice * slot.nOrderVolume);
                                        }
                                        else
                                            PrintLog($"{nSharedTime} 화면번호 : {e.sScrNo} 종목명 : {ea[nEaReq].sCodeName} sRq : {e.sRQName} 이미 신규매수 비정상인데 다시 접근 에러");
                                    }
                                    else
                                    {
                                        PrintLog($"{nSharedTime} 화면번호 : {e.sScrNo} 종목명 : {ea[nEaReq].sCodeName} sRq : {e.sRQName} 손매수 비정상처리");
                                    }
                                }
                                else if (sTypeReq.Equals(ORDER_NEW_SELL)) // 신규매도 비정상처리
                                {
                                    if (slot != null) // 기계매도
                                    {
                                        if (slot.isSelling)
                                        {
                                            PrintLog($"시간 : {nSharedTime}  종목코드 : {ea[nEaReq].sCode}  종목명 : {ea[nEaReq].sCodeName} 블록 : {slot.nBuyedSlotId} e화면번호 : {e.sScrNo}  매도가 비정상처리됐습니다.", nEaReq, slot.nBuyedSlotId);
                                            ea[nEaReq].myTradeManager.nSellReqCnt--;
                                            slot.isSelling = false;
                                            slot.isSellStarted = false;
                                            slot.nSellErrorCount++;
                                            slot.nSellErrorLastTime = nSharedTime;
                                            ShutOffScreen(e.sScrNo);
                                        }
                                        else
                                            PrintLog($"{nSharedTime} 화면번호 : {e.sScrNo} 종목명 : {ea[nEaReq].sCodeName} 블록 : {slot.nBuyedSlotId} sRq : {e.sRQName} 이미 신규매도 비정상인데 다시 접근 에러");
                                    }
                                    else // 손매도
                                    {
                                        PrintLog($"시간 : {nSharedTime}  종목코드 : {ea[nEaReq].sCode}  종목명 : {ea[nEaReq].sCodeName} e화면번호 : {e.sScrNo}  손매도가 비정상처리됐습니다.", nEaReq);
                                        if (virtualSellBlockByScrNoDict.ContainsKey(e.sScrNo))
                                        {
                                            ResetGroupSellingBack(virtualSellBlockByScrNoDict[e.sScrNo]);
                                            virtualSellBlockByScrNoDict.Remove(e.sScrNo);
                                            ShutOffScreen(e.sScrNo);
                                        }
                                    }
                                }
                                // ShutOffScreen(e.sScrNo); // 주식주문 해당화면번호 꺼줍니다.
                            } // END ---- 매매 비정상처리
                            #endregion
                            #region SUCCESS
                            else // 매매 정상처리
                            {
                                if (sTypeReq.Equals(ORDER_NEW_BUY)) // 신규매수 정상처리
                                {
                                    PrintLog($"시간 : {nSharedTime}  종목코드 : {ea[nEaReq].sCode}  종목명 : {ea[nEaReq].sCodeName}e화면번호 : {e.sScrNo}  매수요청이 정상처리됐습니다.", nEaReq);
                                }
                                else if (sTypeReq.Equals(ORDER_NEW_SELL)) // 신규매도 정상처리
                                {
                                    PrintLog($"시간 : {nSharedTime}  종목코드 : {ea[nEaReq].sCode}  종목명 : {ea[nEaReq].sCodeName} e화면번호 : {e.sScrNo}  매도요청이 정상처리됐습니다.", nEaReq);
                                }
                            }
                            #endregion

                        }
                        catch // 먼가 문제 생김
                        {
                            PrintLog($"{nSharedTime} 화면번호 : {e.sScrNo} RQ명 : {e.sRQName} 매매TR 처리중 에러");
                        }
                    }
                    #endregion
                }
            }


        } // END ---- TR 이벤트 핸들러
        #endregion

    }
}
