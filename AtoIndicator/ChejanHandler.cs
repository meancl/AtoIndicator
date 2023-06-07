using System;
using static AtoIndicator.KiwoomLib.TimeLib;
using static AtoIndicator.KiwoomLib.PricingLib;
using static AtoIndicator.TradingBlock.TimeLineGenerator;
using System.Collections.Generic;

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
        public const double PAPER_STOCK_COMMISSION = 0.004;

        public const double REAL_STOCK_BUY_COMMISION = STOCK_FEE;
        public const double REAL_STOCK_SELL_COMMISION = STOCK_FEE + STOCK_TAX;

        public const double VIRTUAL_STOCK_BUY_COMMISION = VIRTUAL_STOCK_FEE;
        public const double VIRTUAL_STOCK_SELL_COMMISION = VIRTUAL_STOCK_FEE + STOCK_TAX;
        #endregion

        public Dictionary<string, BuyedSlot> slotDict = new Dictionary<string, BuyedSlot>();

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
                string sOriginOrderId = axKHOpenAPI1.GetChejanData(904).Trim(); // 원주문번호
                int nOrderVolume = Math.Abs(int.Parse(axKHOpenAPI1.GetChejanData(900))); // 주문수량
                string sCurOkTradePrice = axKHOpenAPI1.GetChejanData(914).Trim(); // 단위체결가 없을땐 ""
                string sCurOkTradeVolume = axKHOpenAPI1.GetChejanData(915).Trim(); // 단위체결량 없을땐 ""
                int nNoTradeVolume = Math.Abs(int.Parse(axKHOpenAPI1.GetChejanData(902))); // 미체결량
                string sScrNo = axKHOpenAPI1.GetChejanData(920).Trim(); // 화면번호
                string sOrderPrice = axKHOpenAPI1.GetChejanData(901).Trim(); // 주문가격
                string sOrderVolume = axKHOpenAPI1.GetChejanData(900).Trim(); // 주문수량
                #endregion


                if (sOrderStatus.Equals("접수"))
                {
                    if (sOrderType.Equals("매수")) // 매수접수
                    {
                        #region 전량 매수취소 확정
                        if (nNoTradeVolume == 0) // 전량 매수취소가 완료됐다면 ( 접수에서 미체결량이 0인경우가 되는건 전량매수취소됐을 경우 뿐)
                        {
                            PrintLog("전량매수취소 완료");
                            ShutOffScreen(sScrNo);
                            slotDict.Remove(sOrderId); 
                            ea[nCurIdx].unhandledList.Remove(sOrderId);
                        }
                        #endregion
                        #region 정상 매수주문
                        else
                        {
                            BuyedSlot slot = slotDict[sOrderId] = GetSlotFromScreen(sScrNo);

                            if (slot == null)
                            {
                                PrintLog("손매수 접수 완료");
                                

                                slot = slotDict[sOrderId] = new BuyedSlot();
                                slot.nRequestTime = nSharedTime;
                                slot.nOriginOrderPrice = Math.Abs(int.Parse(sOrderPrice)); // 주문요청금액 설정
                                slot.nOrderPrice = slot.nOriginOrderPrice; // 지정상한가 설정
                                slot.eTradeMethod = TradeMethodCategory.FixedMethod; // 매매방법 설정
                                slot.nOrderVolume = Math.Abs(int.Parse(sOrderVolume));
                                slot.fTradeRatio = 1;
                                slot.sBuyDescription = "직접 매수";
                                slot.isBuying = true;
                                slot.isBuyByHand = true;

                                switch (slot.eTradeMethod)
                                {
                                    case TradeMethodCategory.RisingMethod:
                                        slot.fTargetPer = GetNextCeiling(ref slot.nCurLineIdx);
                                        slot.fBottomPer = GetNextFloor(ref slot.nCurLineIdx, TradeMethodCategory.RisingMethod);
                                        break;
                                    case TradeMethodCategory.ScalpingMethod:
                                        slot.fTargetPer = GetNextCeiling(ref slot.nCurLineIdx);
                                        slot.fBottomPer = GetNextFloor(ref slot.nCurLineIdx, TradeMethodCategory.ScalpingMethod);
                                        break;
                                    case TradeMethodCategory.BottomUpMethod:
                                        slot.fTargetPer = GetNextCeiling(ref slot.nCurLineIdx);
                                        slot.fBottomPer = GetNextFloor(ref slot.nCurLineIdx, TradeMethodCategory.BottomUpMethod);
                                        break;
                                    case TradeMethodCategory.FixedMethod:
                                        slot.fTargetPer = 0.01;
                                        slot.fBottomPer = -0.02;
                                        break;
                                    default:
                                        break;
                                }

                                nCurDepositCalc -= slot.nOrderPrice * slot.nOrderVolume + ea[nCurIdx].feeMgr.GetRoughFee(slot.nOrderPrice * slot.nOrderVolume);
                            }

                            slot.nReceiptTime = nSharedTime;
                            slot.sCurOrgOrderId = sOrderId; // 현재원주문번호 설정
                            slot.isResponsed = true;
                            slot.sBuyScrNo = sScrNo;
                            ea[nCurIdx].myTradeManager.isOrderStatus = true; // 매매중 on
                            ea[nCurIdx].unhandledList.Add(sOrderId); // 매수취소할 수 있게

                            slotDict[sOrderId].sEachLog.Append($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sScrNo}  {nOrderVolume}(주) 매수 접수완료");
                            PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sScrNo}  {nOrderVolume}(주) 매수 접수완료", nCurIdx);

                        }
                        #endregion
                    }
                    else if (sOrderType.Equals("매도")) // 매도접수
                    {
                        #region 전량 매도취소 확정
                        if (nNoTradeVolume == 0) // 전량 매도취소가 완료됐다면 
                        {
                            PrintLog("전량매도취소 완료");
                            ShutOffScreen(sScrNo);
                            slotDict.Remove(sOrderId); // 원주문번호로 삭제
                        }
                        #endregion
                        #region  정상 매도주문
                        else // 매도 주문인경우
                        {
                            BuyedSlot slot = slotDict[sOrderId] = GetSlotFromScreen(sScrNo);

                            if (slot != null)
                            {
                                slot.sSellScrNo = sScrNo;
                                slot.sCurOrgOrderId = sOrderId; // 현재원주문번호 설정
                                ea[nCurIdx].myTradeManager.isOrderStatus = true; // 매매중 on

                                slot.sEachLog.Append($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sScrNo} {nOrderVolume}(주) 매도 접수완료");
                                PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sScrNo} {nOrderVolume}(주) 매도 접수완료", nCurIdx);
                            }
                            else
                                PrintLog("손매도 접수");

                        }
                        #endregion

                    }
                    else if (sOrderType.Equals("매수취소")) // 매수취소 접수
                    {
                        PrintLog("매수취소 접수");
                    }

                }
                else if (sOrderStatus.Equals("체결"))
                {
                    if (sOrderType.Equals("매수")) // 매수체결
                    {
                        int nCurOkTradeVolume;
                        int nCurOkTradePrice;
                        BuyedSlot slot = slotDict[sOrderId];

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
                            if (!slot.isAllBuyed)
                            {

                                slot.isCanceling = false; // 해당종목의 현재매수취소버튼 초기화
                                slot.isAllBuyed = true; // 해당종목의 매수레코드의 매수완료 on
                                slot.isBuying = false;
                                slot.nBuyEndTime = nSharedTime; // 주문체결완료 시간 설정
                                if (ea[nCurIdx].myTradeManager.nBuyReqCnt > 0)
                                    ea[nCurIdx].myTradeManager.nBuyReqCnt--; // 매수요청 카운트 감소
                                ea[nCurIdx].myTradeManager.isOrderStatus = false; // 매매중 off
                                slot.nBirthTime = nSharedTime;
                                slot.nBirthPrice = slot.nBuyPrice;
                                slot.nLastTouchLineTime = nSharedTime;


                                // rough 수수료
                                nCurDepositCalc += (slot.nOrderVolume - slot.nBuyVolume) * slot.nOrderPrice; // 차액 더하기
                                nCurDepositCalc += ea[nCurIdx].feeMgr.GetRoughFee(slot.nOrderVolume * slot.nOrderPrice); //수수료 더하기
                                nCurDepositCalc -= ea[nCurIdx].feeMgr.GetRoughFee(slot.nBuyedSumPrice);

                                // 기록용
                                slot.nBuyMinuteIdx = nTimeLineIdx;

                                slot.nBuyedSlotId = ea[nCurIdx].myTradeManager.arrBuyedSlots.Count;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots.Add(slot);
                                ShutOffScreen(sScrNo);
                                slotDict.Remove(sOrderId);

                                PrintLog($"시간 : {sTradeTime} {sCode} {ea[nCurIdx].sCodeName} 화면번호 : {sScrNo} {slot.nBuyVolume}(주) 일부 체결완료, 총 주문수량 : {slot.nOrderVolume} 매수가 : {slot.nBuyPrice}", nCurIdx);
                            }

                            return;
                        }
                        #endregion
                        #region 일반체결
                        // 예수금에 지정상한가와 매입금액과의 차이만큼을 다시 복구시켜준다.
                        nCurDepositCalc += (slot.nOrderPrice - nCurOkTradePrice) * nCurOkTradeVolume; // (추정매수가 - 실매수가) * 실매수량 더해준다, 오차만큼 더해준다. 

                        // 이것은 현재매수 구간이기 떄문에
                        // 해당레코드의 평균매입가와 매수수량을 조정하기 위한 과정이다
                        slot.nBuyedSumPrice += nCurOkTradePrice * nCurOkTradeVolume;
                        slot.nCurVolume += nCurOkTradeVolume;
                        slot.nBuyVolume += nCurOkTradeVolume;
                        slot.nBuyPrice = slot.nBuyedSumPrice / slot.nBuyVolume;
                        nTodayDisposalBuyPrice += nCurOkTradePrice * nCurOkTradeVolume; //오늘자 매수가격 증가

                        PrintLog($"[매수] {sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName}  매입가 : {nCurOkTradePrice}, 매입수량 : {nCurOkTradeVolume}, 미체결량 : {nNoTradeVolume}", nCurIdx, isTxtBx: false);
                        #region 전량 체결
                        if (nNoTradeVolume == 0) // 매수 전량 체결됐다면
                        {
                            // rough 수수료
                            nCurDepositCalc += ea[nCurIdx].feeMgr.GetRoughFee(slot.nOrderPrice * slot.nOrderVolume); // 최대 수수료 다시 복구하고
                            nCurDepositCalc -= ea[nCurIdx].feeMgr.GetRoughFee(slot.nBuyedSumPrice); // 매수한 만큼만 빼준다.

                            slot.isAllBuyed = true; // 해당종목의 매수레코드의 매수완료 on
                            slot.isBuying = false;
                            slot.nBuyEndTime = nSharedTime; // 주문체결완료 시간 설정
                            if (ea[nCurIdx].myTradeManager.nBuyReqCnt > 0)
                                ea[nCurIdx].myTradeManager.nBuyReqCnt--; // 매수요청 카운트 감소
                            ea[nCurIdx].myTradeManager.isOrderStatus = false; // 매매중 off

                            slot.nBirthTime = nSharedTime;

                            slot.nBirthPrice = slot.nBuyPrice;
                            slot.nLastTouchLineTime = nSharedTime;

                            slot.nBuyMinuteIdx = nTimeLineIdx;

                            slot.nBuyedSlotId = ea[nCurIdx].myTradeManager.arrBuyedSlots.Count;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots.Add(slot);
                            ShutOffScreen(sScrNo); // 매수체결완료 해당화면번호 꺼줍니다.
                            slotDict.Remove(sOrderId);
                            PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sScrNo}  매수 체결완료, 매수가 : {slot.nBuyPrice}", nCurIdx);



                        }
                        #endregion
                        #endregion

                    }
                    else if (sOrderType.Equals("매도")) // 매도체결
                    {
                        int nCurOkTradeVolume;
                        int nCurOkTradePrice;

                        try
                        {
                            nCurOkTradeVolume = Math.Abs(int.Parse(sCurOkTradeVolume)); // n단위체결량
                            nCurOkTradePrice = Math.Abs(int.Parse(sCurOkTradePrice)); // n단위체결가
                            if (nCurOkTradeVolume == 0) // 원래 데이터가 ""인 지 확인하는 걸로 매수취소인 지 아닌지를 알 수 있는데, 나중에 API가 "" 대신 0을 보내줄 수 도 있으니까 미리 분기함
                                throw new Exception(); // 혹시 정상체결이 다 됐는데 단위체결량을 0으로 보내줘도 어차피 catch문에서 종결에서 하는 모든 임무를 수행하니 문제는 없음.
                        }
                        catch
                        {
                            return; // 매도취소에 대한 확인용 메시지는 스킵한다.
                        }

                        #region 매도 체결 처리
                        nCurDepositCalc += nCurOkTradeVolume * nCurOkTradePrice - (ea[nCurIdx].feeMgr.GetRoughFee(nCurOkTradeVolume * nCurOkTradePrice) + ea[nCurIdx].feeMgr.GetRoughTax(nCurOkTradeVolume * nCurOkTradePrice)); // 세금 빼고 판만큼 더해주기

                        BuyedSlot slot = slotDict[sOrderId];

                        int nSpecificSellIdx = -1;
                        if (slot != null)
                        {
                            nSpecificSellIdx = slot.nBuyedSlotId;
                            if (nNoTradeVolume == 0)
                                slot.isSelling = false; // 일부만 매도 됐을 수 있으니
                        }
                        else
                        {
                            PrintLog("손매도");
                        }

                        HandleSelledRest(nCurIdx, nCurOkTradeVolume, nCurOkTradePrice, nSpecificSellIdx);

                        if (nNoTradeVolume == 0)
                        {
                            ShutOffScreen(sScrNo);
                            slotDict.Remove(sOrderId);
                        }
                        #endregion
                    }

                }
                else if (sOrderStatus.Equals("확인") && sOrderType.Equals("매수취소")) // 매수취소인데 접수 후 확인
                {
                    PrintLog("매수취소 확인");
                }
            } // End ---- e.sGubun.Equals("0") : 접수,체결
            #endregion
            #region 잔고
            else if (e.sGubun.Equals("1")) // 잔고
            {
                string sCode = axKHOpenAPI1.GetChejanData(9001).Substring(1); // 종목코드
                int nCurIdx = eachStockDict[sCode];

                int nHoldingQuant = Math.Abs(int.Parse(axKHOpenAPI1.GetChejanData(930))); // 보유수량
                ea[nCurIdx].nHoldingsCnt = nHoldingQuant;
            } // End ---- e.sGubun.Equals("1") : 잔고
            #endregion
        }// End ---- 체잔 핸들러
        #endregion

        public void HandleSelledRest(int nEaIdx, int nCurOkTradeVolume, int nCurOkTradePrice, int nSpecificIdx)
        {
            int tmpCurOkTradeVolume = nCurOkTradeVolume;

            void HandleRestOnce(int nIdx)
            {
                if (ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].isAllBuyed && ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nCurVolume > 0 && tmpCurOkTradeVolume > 0 )
                {
                    int disposalVolume = ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nCurVolume > tmpCurOkTradeVolume ? tmpCurOkTradeVolume : ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nCurVolume;
                    ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nCurVolume -= disposalVolume;
                    ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nTotalSelledVolume += disposalVolume; // 처분갯수 증가
                    ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nTotalSelledPrice += disposalVolume * nCurOkTradePrice; // 처분총가격 증가 
                    nTodayDisposalSellPrice += disposalVolume * nCurOkTradePrice; // 오늘자 매도가격 증가


                    if (ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nCurVolume <= 0)
                    {
                        PrintLog("하나 처분완료");
                        ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nCurVolume = 0;
                        ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nSellMinuteIdx = nTimeLineIdx;
                        ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].isAllSelled = true;
                        ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nDeathTime = nSharedTime;
                        ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nDeathPrice = ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nTotalSelledPrice / ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nTotalSelledVolume;
                        ea[nEaIdx].myTradeManager.nSellReqCnt--;
                        ea[nEaIdx].myTradeManager.isOrderStatus = false;

                        PrintLog($"{nSharedTime} : {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} 매도 체결완료, 매도가 : {ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nDeathPrice} 손익 : {((double)(ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nDeathPrice - ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nBuyPrice) / ea[nEaIdx].myTradeManager.arrBuyedSlots[nIdx].nBuyPrice - REAL_STOCK_COMMISSION) * 100}", nEaIdx);

                    }

                    tmpCurOkTradeVolume -= disposalVolume;
                }
            }

            PrintLog($"[매도] {nSharedTime} : {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName}  매도가 : {nCurOkTradePrice}, 매도수량 : {nCurOkTradeVolume}", nEaIdx, isTxtBx: false);

            if(nSpecificIdx != -1)
                HandleRestOnce(nSpecificIdx); // 내꺼 먼저
            //그 다음...

            for (int disposal = 0; disposal < ea[nEaIdx].myTradeManager.arrBuyedSlots.Count && tmpCurOkTradeVolume > 0; disposal++)
                HandleRestOnce(disposal);
            

        }
    }
}
