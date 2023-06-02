using System;
using static AtoIndicator.KiwoomLib.TimeLib;
using static AtoIndicator.KiwoomLib.PricingLib;
using static AtoIndicator.TradingBlock.TimeLineGenerator;


namespace AtoIndicator
{
    public partial class MainForm
    {

        #region 변수
        public int nCurDeposit;  // 현재 예수금 // chejan
        public int nCurDepositCalc; // 계산하기 위한 예수금... 으음 // chejan
        public char[] charsToTrim = { '+', '-', ' ' }; // 키움API 데이터에 +, -, 공백이 같이 들어와짐 // chejan
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
        #endregion

        #region 체잔 핸들러
        // ==================================================
        // 주식주문(접수, 체결, 잔고) 이벤트발생시 핸들러메소드
        // ==================================================
        private void OnReceiveChejanDataHandler(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            #region 접수 & 체결
            if (e.sGubun.Equals("0")) // 접수와 체결 
            {
                #region 접수 & 체결변수 세팅
                string sTradeTime = axKHOpenAPI1.GetChejanData(908).Trim(); // 체결시간
                string sCode = axKHOpenAPI1.GetChejanData(9001).Substring(1).Trim(); // 종목코드 전위 알파벳하나가 붙어서 삭제
                int nCurIdx = eachStockDict[sCode]; // 해당 개인구조체 인덱스
                string sOrderType = axKHOpenAPI1.GetChejanData(905).Trim(charsToTrim); // +매수, -매도, 매수취소
                string sOrderStatus = axKHOpenAPI1.GetChejanData(913).Trim(); // 주문상태(접수, 체결, 확인)
                string sOrderId = axKHOpenAPI1.GetChejanData(9203).Trim(); // 주문번호
                int nOrderVolume = Math.Abs(int.Parse(axKHOpenAPI1.GetChejanData(900))); // 주문수량
                string sCurOkTradePrice = axKHOpenAPI1.GetChejanData(914).Trim(); // 단위체결가 없을땐 ""
                string sCurOkTradeVolume = axKHOpenAPI1.GetChejanData(915).Trim(); // 단위체결량 없을땐 ""
                int nNoTradeVolume = Math.Abs(int.Parse(axKHOpenAPI1.GetChejanData(902))); // 미체결량
                string sScrNo = axKHOpenAPI1.GetChejanData(920).Trim(); // 화면번호
                ReceiveScreenNoIdx(sScrNo, out int? nEaIdx, out int? nSlotIdx, out string sPurpose);

                int nCurBuySlotIdx;
                if (nSlotIdx == null || nEaIdx != nCurIdx || nCurIdx == INIT_CODEIDX_NUM)
                {
                    PrintLog($"시간 : {nSharedTime}  화면번호 : {sScrNo}, 종목명 : {ea[nCurIdx].sCodeName}  개인번호 : {nEaIdx}  슬롯번호 : {nSlotIdx} 치명적인 오류 발견");
                    return;
                }
                else
                    nCurBuySlotIdx = (int)nSlotIdx;

                #endregion

                #region 매수
                // ---------------------------------------------
                // 매수 데이터 수신 순서
                // 매수접수 - 매수체결
                // 매수접수 - (매수취소) - 매수체결
                // 매수접수 - (매수취소) - 매수취소접수 - 매수취소확인 - 매수접수(매수취소확인)
                // 매수접수 - (매수취소) - 매수체결 - 매수취소접수 - 매수취소확인 - 매수체결(매수취소확인)
                // ---------------------------------------------
                if (sOrderType.Equals("매수"))
                {

                    #region 체결
                    if (sOrderStatus.Equals("체결"))
                    {
                        // 매수-체결됐으면 3가지로 나눠볼 수 있는데
                        // 1. 일반적으로 일부 체결된 경우
                        // 2. 전량 체결된 경우
                        // 3. 일부 체결된 후 나머지는 매수취소된 경우(미체결 클리어를 위해 얻어지는 경우)


                        // 문자열로 받아진 단위체결량과 단위체결가를 정수로 바꾸는 작업을 한다.
                        // 접수나 취소 때는 체결가~ 종류는 "" 공백으로 받아지기 때문에
                        // 정수 캐스팅을 하면 오류가 나기 때문이다
                        int nCurOkTradeVolume;
                        int nCurOkTradePrice;


                        try
                        {
                            nCurOkTradeVolume = Math.Abs(int.Parse(sCurOkTradeVolume)); // n단위체결량
                            nCurOkTradePrice = Math.Abs(int.Parse(sCurOkTradePrice)); // n단위체결가
                            if (nCurOkTradeVolume == 0) // 원래 데이터가 ""인 지 확인하는 걸로 매수취소인 지 아닌지를 알 수 있는데, 나중에 API가 "" 대신 0을 보내줄 수 도 있으니까 미리 분기함
                                throw new Exception(); // 혹시 정상체결이 다 됐는데 단위체결량을 0으로 보내줘도 어차피 catch문에서 종결에서 하는 모든 임무를 수행하니 문제는 없음.
                        }
                        #region 일부 매수취소
                        catch
                        {
                            if (!ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isAllBuyed)
                            {
                                string sTmpScrNo = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sBuyScrNo;
                                ShutOffScreen(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sBuyScrNo);

                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isCanceling = false; // 해당종목의 현재매수취소버튼 초기화
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isAllBuyed = true; // 해당종목의 매수레코드의 매수완료 on
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isBuying = false;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyEndTime = nSharedTime; // 주문체결완료 시간 설정
                                if (ea[nCurIdx].myTradeManager.nBuyReqCnt > 0)
                                    ea[nCurIdx].myTradeManager.nBuyReqCnt--; // 매수요청 카운트 감소
                                ea[nCurIdx].myTradeManager.isOrderStatus = false; // 매매중 off
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBirthTime = nSharedTime;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBirthPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyPrice;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nLastTouchLineTime = nSharedTime;

                            
                                // rough 수수료
                                nCurDepositCalc += (ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderVolume - ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyVolume) * ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderPrice; // 차액 더하기
                                nCurDepositCalc += ea[nCurIdx].feeMgr.GetRoughFee(ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderVolume * ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderPrice); //수수료 더하기
                                nCurDepositCalc -= ea[nCurIdx].feeMgr.GetRoughFee(ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyedSumPrice);

                                // 기록용
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyMinuteIdx = nTimeLineIdx; 


                                PrintLog($"시간 : {sTradeTime} {sCode} {ea[nCurIdx].sCodeName} 매매블럭 : {nCurBuySlotIdx} 화면번호 : {sTmpScrNo} {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyVolume}(주) 일부 체결완료, 총 주문수량 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderVolume} 매수가 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyPrice}", nCurIdx, nCurBuySlotIdx);
                            }

                            
                            return;
                        }
                        #endregion
                        #region 일반체결
                        // 예수금에 지정상한가와 매입금액과의 차이만큼을 다시 복구시켜준다.
                        nCurDepositCalc += (ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderPrice - nCurOkTradePrice) * nCurOkTradeVolume; // (추정매수가 - 실매수가) * 실매수량 더해준다, 오차만큼 더해준다. 

                        // 이것은 현재매수 구간이기 떄문에
                        // 해당레코드의 평균매입가와 매수수량을 조정하기 위한 과정이다
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyedSumPrice += nCurOkTradePrice * nCurOkTradeVolume;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nCurVolume += nCurOkTradeVolume;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyVolume += nCurOkTradeVolume;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyedSumPrice / ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyVolume;
                        nTodayDisposalBuyPrice += nCurOkTradePrice * nCurOkTradeVolume; //오늘자 매수가격 증가

                        PrintLog($"[매수] {sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName}  {nCurBuySlotIdx}번째슬롯  매입가 : {nCurOkTradePrice}, 매입수량 : {nCurOkTradeVolume}, 미체결량 : {nNoTradeVolume}", nCurIdx, nCurBuySlotIdx, false);
                        #region 전량 체결
                        if (nNoTradeVolume == 0) // 매수 전량 체결됐다면
                        {
                            // 매수가 전량 체결됐다면 
                            // 체결-매수취소와 유사하게 진행된다 하나 다른점은 매수취소완료 시그널을 건들 필요가 없다는 것이다.
                            // 현재매수취소 그리고 일부라도 체결됐으니 해당레코드에 구매됐다는 시그널을 on해주고 레코드인덱스를 한칸 늘린다
                            // 매수요청 카운트도 낮추고 현재 매매중인 시그널을 off해준다.
                            string sTmpScrNo = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sBuyScrNo;
                            ShutOffScreen(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sBuyScrNo); // 매수체결완료 해당화면번호 꺼줍니다.

                      
                            // rough 수수료
                            nCurDepositCalc += ea[nCurIdx].feeMgr.GetRoughFee(ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderPrice * ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderVolume); // 최대 수수료 다시 복구하고
                            nCurDepositCalc -= ea[nCurIdx].feeMgr.GetRoughFee(ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyedSumPrice); // 매수한 만큼만 빼준다.

                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isAllBuyed = true; // 해당종목의 매수레코드의 매수완료 on
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isBuying = false;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyEndTime = nSharedTime; // 주문체결완료 시간 설정
                            if (ea[nCurIdx].myTradeManager.nBuyReqCnt > 0)
                                ea[nCurIdx].myTradeManager.nBuyReqCnt--; // 매수요청 카운트 감소
                            ea[nCurIdx].myTradeManager.isOrderStatus = false; // 매매중 off

                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBirthTime = nSharedTime;

                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBirthPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyPrice;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nLastTouchLineTime = nSharedTime;
                            
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyMinuteIdx = nTimeLineIdx;


                            PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sTmpScrNo} 매매블럭 : {nCurBuySlotIdx} 매수 체결완료, 매수가 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyPrice}", nCurIdx, nCurBuySlotIdx);

                            
                            
                        }
                        #endregion
                        #endregion
                    } //  END ---- 매수체결 끝
                    #endregion
                    #region 접수
                    else if (sOrderStatus.Equals("접수"))
                    {
                        #region 전량 매수취소 확정
                        if (nNoTradeVolume == 0) // 전량 매수취소가 완료됐다면 ( 접수에서 미체결량이 0인경우가 되는건 전량매수취소됐을 경우 뿐)
                        {
                            // 접수-매수취소는
                            // 체결이 하나도 안된상태에서 매수주문이 모두 매수취소 된 상황이다
                            // 많은 설정을 할 필요가 없다
                            // 여기서는 isAllBuyed와 현재레코드인덱스를 더하지 않는 이유는 체결데이터가 없기때문에
                            // 굳이 인덱스를 늘려 레코드만 증가시킨다면 실시간에서 관리함에 시간이 더 소요되기 때문이다

                            string sTmpScrNo = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sBuyScrNo;
                            ShutOffScreen(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sBuyScrNo); // 전량매수취소된 매수체결완료 해당화면번호 꺼줍니다.

                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isCanceling = false; // 해당종목의 현재매수취소버튼 초기화
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isAllBuyed = true; // 해당종목은 모두 사졌다.( 사실 사지도 못했지만 매커니즘의 통일성을 위해)
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isBuying = false;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isAllSelled = true; // 해당종목은 모두 팔렸다.
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyEndTime = nSharedTime; // 주문체결완료 시간 설정
                            if (ea[nCurIdx].myTradeManager.nBuyReqCnt > 0)
                                ea[nCurIdx].myTradeManager.nBuyReqCnt--; // 매수요청 카운트 감소
                            ea[nCurIdx].myTradeManager.isOrderStatus = false; // 매매중 off
                            nCurDepositCalc += ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderVolume * ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderPrice + ea[nCurIdx].feeMgr.GetRoughFee(ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderVolume * ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOrderPrice); // 전량 취소됐으니 전량만큼 더해준다.
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBirthTime = nSharedTime;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBirthPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nOriginOrderPrice;

                            // 기록용
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyMinuteIdx = nTimeLineIdx;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nSellMinuteIdx = nTimeLineIdx;


                            PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sTmpScrNo} {nCurBuySlotIdx}  {nOrderVolume}(주) 전량매수취소 완료", nCurIdx, nCurBuySlotIdx);
                            
                            
                        }
                        #endregion
                        #region 주문
                        else // 매수 주문인경우
                        {
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nReceiptTime = nSharedTime;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sCurOrgOrderId = sOrderId; // 현재원주문번호 설정
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isResponsed = true;
                            ea[nCurIdx].myTradeManager.isOrderStatus = true; // 매매중 on
                            

                            PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sBuyScrNo} {nCurBuySlotIdx}번째슬롯 {nOrderVolume}(주) 매수 접수완료", nCurIdx, nCurBuySlotIdx);

                            strategyHistoryList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nStrategyIdx].Add(new StrategyHistory(nCurIdx, nCurBuySlotIdx)); // 전략리스트 인덱스에 맞게 삽입
                            totalTradeHistoryList.Add(new StrategyHistory(nCurIdx, nCurBuySlotIdx)); // 전체 매매리스트 
                        }
                        #endregion
                    }
                    #endregion

                } // END ---- orderType.Equals("매수")
                #endregion
                #region 매도
                else if (sOrderType.Equals("매도"))
                {
                    #region 체결
                    if (sOrderStatus.Equals("체결"))
                    {
                        int nOkTradeVolume = Math.Abs(int.Parse(sCurOkTradeVolume));
                        int nOkTradePrice = Math.Abs(int.Parse(sCurOkTradePrice));
                        nCurDepositCalc += nOkTradeVolume * nOkTradePrice - (ea[nCurIdx].feeMgr.GetRoughFee(nOkTradeVolume * nOkTradePrice) + ea[nCurIdx].feeMgr.GetRoughTax(nOkTradeVolume * nOkTradePrice)); // 세금 빼고 판만큼 더해주기
                                         
               
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nCurVolume -= nOkTradeVolume;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nTotalSelledVolume += nOkTradeVolume; // 처분갯수 증가
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nTotalSelledPrice += nOkTradeVolume * nOkTradePrice; // 처분총가격 증가 

                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].nTotalSellVolume += nOkTradeVolume;
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].nTotalSellPrice += nOkTradeVolume * nOkTradePrice;

                        nTodayDisposalSellPrice += nOkTradeVolume * nOkTradePrice; // 오늘자 매도가격 증가
                        PrintLog($"[매도] {sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName}  {nCurBuySlotIdx}번째슬롯  매도가 : {nOkTradePrice}, 매도수량 : {nOkTradeVolume}, 미체결량 : {nNoTradeVolume}", nCurIdx, nCurBuySlotIdx, false);

                        
                        #region 체결 완료
                        if (nNoTradeVolume == 0)
                        {
                            string sTmpScrNo = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sSellScrNo;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isSelling = false; // 일부만 매도 됐을 수 있으니
                            ShutOffScreen(ref ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sSellScrNo); // 매도체결완료 해당화면번호 꺼줍니다.

                            // 분할매도 (최소 1회 방문)
                            {
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].isSelled = true;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].nDeathTime = nSharedTime;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].nDeathPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].nTotalSellPrice / ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].nTotalSellVolume;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].fProfit = (double)(ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].nDeathPrice - ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyPrice) / ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyPrice - REAL_STOCK_COMMISSION;
                                
                                PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sTmpScrNo} {nCurBuySlotIdx}번쨰슬롯 {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen}번째 분할매도 체결완료, 매도가 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].nDeathPrice} 손익 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.recList[ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen].fProfit * 100}", nCurIdx, nCurBuySlotIdx);
                                
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].recGroup.nLen++;
                            }


                            if (ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nCurVolume == 0)
                            {
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nSellMinuteIdx = nTimeLineIdx;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].isAllSelled = true;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nDeathTime = nSharedTime;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nDeathPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nTotalSelledPrice / ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nTotalSelledVolume;
                                ea[nCurIdx].myTradeManager.nSellReqCnt--;
                                ea[nCurIdx].myTradeManager.isOrderStatus = false;

                                PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sTmpScrNo} {nCurBuySlotIdx}번쨰슬롯 매도 체결완료, 매도가 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nDeathPrice} 손익 : {((double)(ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nDeathPrice - ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyPrice) / ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].nBuyPrice - REAL_STOCK_COMMISSION) * 100}", nCurIdx, nCurBuySlotIdx);
                            }

                        }
                        #endregion
                    }
                    #endregion
                    #region 접수
                    else if (sOrderStatus.Equals("접수"))
                    {
                        PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sSellScrNo} {nOrderVolume}(주) 매도 접수완료", nCurIdx, nCurBuySlotIdx);
                        ea[nCurIdx].myTradeManager.isOrderStatus = true; // 매매중 on
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[nCurBuySlotIdx].sCurOrgOrderId = sOrderId; // 원주문번호

                    }
                    #endregion

                } // END ---- orderType.Equals("매도")
                #endregion
                #region 매수취소
                else if (sOrderType.Equals("매수취소"))
                {
                    // ----------------------------------
                    // 야기할 수 있는 문제
                    // 1. 매수취소확인후 접수,체결을 안보내준다.
                    // 2. 매수취소확인전에 접수,체결을 보내준다.
                    // ----------------------------------
                    #region 접수
                    // 매수취소에서는 매수취소완료버튼 on
                    // 매수취소수량이 있으면 그만큼 예수금 더해주면 된다
                    // 거래중, 매매완료 등등의 처리는 매수에서 완료한다.
                    if (sOrderStatus.Equals("접수"))
                    {
                        PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName}  {nCurBuySlotIdx}  {nOrderVolume}(주) 매수취소 접수완료", nCurIdx, nCurBuySlotIdx);
                        // 매수취소 접수가 되면 거의 확정적으로 매수취소확인 따라오며 
                        // 매수취소 접수때부터 이미 매수취소된거같음.
                    }
                    #endregion
                    #region 확인
                    else if (sOrderStatus.Equals("확인"))
                    {
                        // 매수취소확인은 사실상 매수취소 수량이 있는거고 미체결량은 0인 상태일 테지만 
                        // 예기치 못한 오류로 인해 문제가 생길 수 도 있으니
                        // 매수취소 수량과 미체결량을 검사해준다.
                        PrintLog($"({sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName}  {nCurBuySlotIdx}  {nOrderVolume}(주) 매수취소 접수확인", nCurIdx, nCurBuySlotIdx);
                    }
                    #endregion

                } // END ---- orderType.Equals("매수취소")
                #endregion
                #region 매도취소
                else if (sOrderType.Equals("매도취소"))
                {
                }
                #endregion
                #region 매수정정
                else if (sOrderType.Equals("매수정정"))
                {
                }
                #endregion
                #region 매도정정
                else if (sOrderType.Equals("매도정정"))
                {
                }
                #endregion

            } // End ---- e.sGubun.Equals("0") : 접수,체결
            #endregion
            #region 잔고
            else if (e.sGubun.Equals("1")) // 잔고
            {
                string sCode = axKHOpenAPI1.GetChejanData(9001).Substring(1); // 종목코드
                int nCurIdx  = eachStockDict[sCode];

                int nHoldingQuant = Math.Abs(int.Parse(axKHOpenAPI1.GetChejanData(930))); // 보유수량
                ea[nCurIdx].nHoldingsCnt = nHoldingQuant;
            } // End ---- e.sGubun.Equals("1") : 잔고
            #endregion
        }// End ---- 체잔 핸들러
        #endregion
    }
}
