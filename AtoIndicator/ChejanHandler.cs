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

        public Dictionary<string, BuyedSlot> slotByOrderIdDict = new Dictionary<string, BuyedSlot>(); // 매매 슬롯 딕셔너리
        public Dictionary<string, BuyedSlot> virtualSellSlotByOrderIdDict = new Dictionary<string, BuyedSlot>(); // 매매 슬롯 딕셔너리
        public Dictionary<string, int> sellVersionByOrderIdDict = new Dictionary<string, int>(); // 손매도의 경우 번호 지정을 위한 딕셔너리 key 주문번호 , value : sell버전
        public Dictionary<string, int> sellVersionByScrNoDict = new Dictionary<string, int>(); // 손매도의 경우 번호 지정을 위한 딕셔너리 key 화면번호 , value : sell버전 * 매도비정상 처리 보조용
        public Dictionary<string, int> sellRemainCheckByOrderIdDict = new Dictionary<string, int>(); // 남은 매도를 확인하기 위한 딕셔너리

        public Dictionary<string, int> buyCancelingByOrderIdDict = new Dictionary<string, int>(); // 매수취소중 딕셔너리
        public Dictionary<string, int> sellCancelingByOrderIdDict = new Dictionary<string, int>(); // 매도취소중 딕셔너리

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
                int nOrderPrice = Math.Abs(int.Parse(axKHOpenAPI1.GetChejanData(901))); // 주문가격
                #endregion


                if (sOrderType.Equals("매수"))
                {
                    if (sOrderStatus.Equals("접수"))
                    {
                        #region 전량 매수취소 확정
                        if (nNoTradeVolume == 0) // 전량 매수취소가 완료됐다면 ( 접수에서 미체결량이 0인경우가 되는건 전량매수취소됐을 경우 뿐)
                        {
                            if (slotByOrderIdDict.ContainsKey(sOrderId))
                            {
                                BuyedSlot slot = slotByOrderIdDict[sOrderId];
                                nCurDepositCalc += slot.nOrderPrice * slot.nOrderVolume + ea[nCurIdx].feeMgr.GetRoughFee(slot.nOrderPrice * slot.nOrderVolume);
                            }
                            PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}가 전량매수취소 완료");
                            ShutOffScreen(sScrNo);
                            slotByOrderIdDict.Remove(sOrderId);
                            buyCancelingByOrderIdDict.Remove(sOrderId);
                            ea[nCurIdx].unhandledBuyOrderIdList.Remove(sOrderId);

                            CallReceiptConfirm(nCurIdx);

                        }
                        #endregion
                        #region 정상 매수주문
                        else
                        {
                            BuyedSlot slot = slotByOrderIdDict[sOrderId] = GetSlotFromScreen(sScrNo);

                            if (slot == null)
                            {
                                PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}가 손매수 접수 완료");


                                slot = slotByOrderIdDict[sOrderId] = new BuyedSlot();
                                slot.nRequestTime = nSharedTime;
                                slot.nOriginOrderPrice = nOrderPrice; // 주문요청금액 설정
                                slot.nOrderPrice = slot.nOriginOrderPrice; // 지정상한가 설정
                                slot.eTradeMethod = TradeMethodCategory.FixedMethod; // 매매방법 설정
                                slot.nOrderVolume = nOrderVolume;
                                slot.fTradeRatio = 1;
                                slot.sBuyDescription = "손매수";
                                slot.isBuying = true;
                                slot.isBuyByHand = true;

                                switch (slot.eTradeMethod)
                                {
                                    case TradeMethodCategory.RisingMethod:
                                        slot.fTargetPer = GetNextCeiling(slot.nCurLineIdx);
                                        slot.fBottomPer = GetNextFloor(slot.nCurLineIdx, TradeMethodCategory.RisingMethod);
                                        break;
                                    case TradeMethodCategory.ScalpingMethod:
                                        slot.fTargetPer = GetNextCeiling(slot.nCurLineIdx);
                                        slot.fBottomPer = GetNextFloor(slot.nCurLineIdx, TradeMethodCategory.ScalpingMethod);
                                        break;
                                    case TradeMethodCategory.BottomUpMethod:
                                        slot.fTargetPer = GetNextCeiling(slot.nCurLineIdx);
                                        slot.fBottomPer = GetNextFloor(slot.nCurLineIdx, TradeMethodCategory.BottomUpMethod);
                                        break;
                                    case TradeMethodCategory.FixedMethod:
                                        slot.fTargetPer = 0.015;
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
                            ea[nCurIdx].unhandledBuyOrderIdList.Add(sOrderId); // 매수취소할 수 있게

                            slotByOrderIdDict[sOrderId].sEachLog.Append($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원 매수 접수완료{NEW_LINE}");
                            PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원 매수 접수완료", nCurIdx);

                            CallReceiptConfirm(nCurIdx);
                        }
                        #endregion
                    }
                    else if (sOrderStatus.Equals("체결"))
                    {
                        int nCurOkTradeVolume;
                        int nCurOkTradePrice;
                        BuyedSlot slot = slotByOrderIdDict[sOrderId];

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
                                slotByOrderIdDict.Remove(sOrderId);
                                buyCancelingByOrderIdDict.Remove(sOrderId);
                                ea[nCurIdx].unhandledBuyOrderIdList.Remove(sOrderId);
                                CallReceiptConfirm(nCurIdx);

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
                        ea[nCurIdx].myTradeManager.nTotalBuyed += nCurOkTradeVolume;
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
                            slotByOrderIdDict.Remove(sOrderId);
                            buyCancelingByOrderIdDict.Remove(sOrderId);
                            ea[nCurIdx].unhandledBuyOrderIdList.Remove(sOrderId);

                            PrintLog($"{sTradeTime} : {sCode}  {ea[nCurIdx].sCodeName} 화면번호 : {sScrNo}  매수 체결완료, 매수가 : {slot.nBuyPrice}", nCurIdx, slot.nBuyedSlotId);
                        }

                        CallReceiptConfirm(nCurIdx);
                        #endregion
                        #endregion
                    }
                }
                else if (sOrderType.Equals("매도"))
                {
                    if (sOrderStatus.Equals("접수"))
                    {
                        #region 전량 매도취소 확정
                        if (nNoTradeVolume == 0) // 전량 매도취소가 완료됐다면 
                        {
                            if (sellVersionByOrderIdDict.ContainsKey(sOrderId))
                                ResetGroupSellingBack(nCurIdx, sellVersionByOrderIdDict[sOrderId]);

                            if (sellRemainCheckByOrderIdDict.ContainsKey(sOrderId))
                                ea[nCurIdx].myTradeManager.nTotalSelling -= sellRemainCheckByOrderIdDict[sOrderId];
                            PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원 전량매도취소 완료");

                            slotByOrderIdDict.Remove(sOrderId);
                            virtualSellSlotByOrderIdDict.Remove(sOrderId);
                            sellVersionByOrderIdDict.Remove(sOrderId);
                            sellVersionByScrNoDict.Remove(sScrNo);
                            sellRemainCheckByOrderIdDict.Remove(sOrderId);
                            sellCancelingByOrderIdDict.Remove(sOrderId);
                            ea[nCurIdx].unhandledSellOrderIdList.Remove(sOrderId);
                            ShutOffScreen(sScrNo);
                            CallReceiptConfirm(nCurIdx);
                        }
                        #endregion
                        #region  정상 매도주문
                        else // 매도 주문인경우
                        {
                            BuyedSlot slot = slotByOrderIdDict[sOrderId] = GetSlotFromScreen(sScrNo);


                            if (slot != null) // 기계주문
                            {
                                slot.sSellScrNo = sScrNo;
                                slot.sCurOrgOrderId = sOrderId; // 현재원주문번호 설정
                                ea[nCurIdx].myTradeManager.isOrderStatus = true; // 매매중 on

                                slot.sEachLog.Append($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원  매도 접수완료{NEW_LINE}");
                                PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원 {slot.nBuyedSlotId}블록 매도 접수완료", nCurIdx);
                            }
                            else // 나 주문
                            {

                                ea[nCurIdx].myTradeManager.nLastSellCheckVersion++;
                                sellVersionByOrderIdDict[sOrderId] = ea[nCurIdx].myTradeManager.nLastSellCheckVersion;
                                sellVersionByScrNoDict[sScrNo] = ea[nCurIdx].myTradeManager.nLastSellCheckVersion;

                                PrintLog($"[손매도접수-처리전] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} 번호 : {sellVersionByOrderIdDict[sOrderId]} 매도가 : {nOrderPrice}, 매도수량 : {nOrderVolume}", nCurIdx);

                                int tmpCurOrderVolume = nOrderVolume;

                                slot = virtualSellSlotByOrderIdDict[sOrderId] = new BuyedSlot();

                                for (int disposal = 0; disposal < ea[nCurIdx].myTradeManager.arrBuyedSlots.Count && tmpCurOrderVolume > 0; disposal++)
                                {
                                    if (!ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].isAllSelled && !ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].isSelling && ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume > 0)
                                    {
                                        int disposalVolume = ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume > tmpCurOrderVolume ? tmpCurOrderVolume : ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume;

                                        if (disposalVolume < ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume) // 분리 조건
                                        {
                                            PrintLog($"[손매도접수-분리] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} {disposal}블록 {disposalVolume}주상태로 분리");
                                            SeperateSlot(nCurIdx, disposal, disposalVolume);
                                        }

                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].isSelling = true;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].sCurOrgOrderId = sOrderId;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSellCheckVersion = ea[nCurIdx].myTradeManager.nLastSellCheckVersion;

                                        tmpCurOrderVolume -= disposalVolume;

                                        PrintLog($"[손매도접수] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} 버전 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSellCheckVersion} {disposal}블록 {disposalVolume}주 접수", nCurIdx, disposal);
                                    }
                                }

                                slot.nSellRequestTime = nSharedTime;
                                slot.nOrderPrice = nOrderPrice;
                                slot.nOrderVolume = nOrderVolume;
                                slot.sCurOrgOrderId = sOrderId;
                                slot.isSelling = true;
                                slot.nTotalSellCheckVersion = ea[nCurIdx].myTradeManager.nLastSellCheckVersion;
                                slot.isBuyByHand = true;
                            }

                            ea[nCurIdx].myTradeManager.nTotalSelling += nOrderVolume;
                            ea[nCurIdx].unhandledSellOrderIdList.Add(sOrderId);
                            sellRemainCheckByOrderIdDict[sOrderId] = nOrderVolume;
                            CallReceiptConfirm(nCurIdx);
                        }
                        #endregion
                    }
                    else if (sOrderStatus.Equals("체결"))
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
                            if (sellVersionByOrderIdDict.ContainsKey(sOrderId))
                                ResetGroupSellingBack(nCurIdx, sellVersionByOrderIdDict[sOrderId]);
                            if (sellRemainCheckByOrderIdDict.ContainsKey(sOrderId))
                                ea[nCurIdx].myTradeManager.nTotalSelling -= sellRemainCheckByOrderIdDict[sOrderId];

                            slotByOrderIdDict.Remove(sOrderId);
                            virtualSellSlotByOrderIdDict.Remove(sOrderId);
                            sellVersionByOrderIdDict.Remove(sOrderId);
                            sellVersionByScrNoDict.Remove(sScrNo);
                            sellRemainCheckByOrderIdDict.Remove(sOrderId);
                            sellCancelingByOrderIdDict.Remove(sOrderId);
                            ea[nCurIdx].unhandledSellOrderIdList.Remove(sOrderId);
                            ShutOffScreen(sScrNo);
                            CallReceiptConfirm(nCurIdx);
                            return; // 매도취소에 대한 확인용 메시지는 스킵한다.
                        }


                        #region 매도 체결 처리
                        nCurDepositCalc += nCurOkTradeVolume * nCurOkTradePrice - (ea[nCurIdx].feeMgr.GetRoughFee(nCurOkTradeVolume * nCurOkTradePrice) + ea[nCurIdx].feeMgr.GetRoughTax(nCurOkTradeVolume * nCurOkTradePrice)); // 세금 빼고 판만큼 더해주기
                        nTodayDisposalSellPrice += nCurOkTradeVolume * nCurOkTradePrice; // 오늘자 매도가격 증가

                        ea[nCurIdx].myTradeManager.nTotalSelling -= nCurOkTradeVolume;
                        ea[nCurIdx].myTradeManager.nTotalSelled += nCurOkTradeVolume;
                        sellRemainCheckByOrderIdDict[sOrderId] -= nCurOkTradeVolume;

                        BuyedSlot slot = slotByOrderIdDict[sOrderId];

                        if (slot != null)
                        {
                            int specificIdx = slot.nBuyedSlotId;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nCurVolume -= nCurOkTradeVolume;
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nTotalSelledVolume += nCurOkTradeVolume; // 처분갯수 증가
                            ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nTotalSelledPrice += nCurOkTradeVolume * nCurOkTradePrice; // 처분총가격 증가 



                            if (ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nCurVolume <= 0)
                            {
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nCurVolume = 0;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nSellMinuteIdx = nTimeLineIdx;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].isAllSelled = true;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nTotalSellCheckVersion = 0;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].sCurOrgOrderId = null;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].isSelling = false;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nDeathTime = nSharedTime;
                                ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nDeathPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nTotalSelledPrice / ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nTotalSelledVolume;
                                ea[nCurIdx].myTradeManager.nSellReqCnt--;
                                ea[nCurIdx].myTradeManager.isOrderStatus = false;

                                PrintLog($"[기계매도] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} 매도 체결완료, 매도가 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nDeathPrice} 손익 : {((double)(ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nDeathPrice - ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nBuyPrice) / ea[nCurIdx].myTradeManager.arrBuyedSlots[specificIdx].nBuyPrice - REAL_STOCK_COMMISSION) * 100}", nCurIdx, specificIdx);
                            }
                            else
                                PrintLog($"[기계매도] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} {specificIdx}블록 {nCurOkTradeVolume}(주) {nCurOkTradePrice}(원) .. 잔량 : {nNoTradeVolume}(주)", nCurIdx, specificIdx);
                        }
                        else
                        {
                            int tmpCurOkTradeVolume = nCurOkTradeVolume;
                            int sellVersion = sellVersionByOrderIdDict[sOrderId];

                            PrintLog($"[손매도체결-처리전] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName}  매도가 : {nCurOkTradePrice}, 매도수량 : {nCurOkTradeVolume}", nCurIdx);


                            for (int disposal = 0; disposal < ea[nCurIdx].myTradeManager.arrBuyedSlots.Count && tmpCurOkTradeVolume > 0; disposal++)
                            {
                                if (!ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].isAllSelled && ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSellCheckVersion == sellVersion)
                                {
                                    int disposalVolume = ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume > tmpCurOkTradeVolume ? tmpCurOkTradeVolume : ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume;

                                    ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume -= disposalVolume;
                                    ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSelledVolume += disposalVolume; // 처분갯수 증가
                                    ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSelledPrice += disposalVolume * nCurOkTradePrice; // 처분총가격 증가 


                                    if (ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume <= 0)
                                    {
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].sSellDescription = "손매도";
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume = 0;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nSellMinuteIdx = nTimeLineIdx;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].isAllSelled = true;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSellCheckVersion = 0;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].sCurOrgOrderId = null;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].isSelling = false;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nDeathTime = nSharedTime;
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nDeathPrice = ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSelledPrice / ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSelledVolume;
                                        ea[nCurIdx].myTradeManager.nSellReqCnt--;
                                        ea[nCurIdx].myTradeManager.isOrderStatus = false;

                                        PrintLog($"[손매도체결] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} {disposal}블록 매도 체결완료, 매도가 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nDeathPrice} 손익 : {((double)(ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nDeathPrice - ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nBuyPrice) / ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nBuyPrice - REAL_STOCK_COMMISSION) * 100}", nCurIdx, disposal);
                                    }
                                    else
                                        PrintLog($"[손매도체결] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} {disposal}블록 {disposalVolume}(주) {nCurOkTradePrice}(원) .. 잔량 : {nNoTradeVolume}(주)", nCurIdx, disposal);

                                    tmpCurOkTradeVolume -= disposalVolume;
                                }
                            }

                        }
                        #endregion

                        if (nNoTradeVolume == 0)
                        {
                            slotByOrderIdDict.Remove(sOrderId);
                            virtualSellSlotByOrderIdDict.Remove(sOrderId);
                            sellVersionByOrderIdDict.Remove(sOrderId);
                            sellVersionByScrNoDict.Remove(sScrNo);
                            sellRemainCheckByOrderIdDict.Remove(sOrderId);
                            sellCancelingByOrderIdDict.Remove(sOrderId);
                            ea[nCurIdx].unhandledSellOrderIdList.Remove(sOrderId);
                            ShutOffScreen(sScrNo);
                        }
                        CallReceiptConfirm(nCurIdx);
                    }
                }
                else if (sOrderType.Equals("매수취소"))
                {

                    if (sOrderStatus.Equals("접수"))
                    {
                        PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원 매수취소 접수");
                    }
                    else if (sOrderStatus.Equals("확인"))
                    {
                        PrintLog("매수취소 확인");
                    }
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

        // 매도취소됐을때 isSelling 푸는 메서드
        public void ResetGroupSellingBack(int nEaIdx, int sellVersion)
        {
            for (int disposal = 0; disposal < ea[nEaIdx].myTradeManager.arrBuyedSlots.Count; disposal++)
            {
                if (!ea[nEaIdx].myTradeManager.arrBuyedSlots[disposal].isAllSelled && ea[nEaIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSellCheckVersion == sellVersion)
                {
                    ea[nEaIdx].myTradeManager.arrBuyedSlots[disposal].isSelling = false;
                    ea[nEaIdx].myTradeManager.arrBuyedSlots[disposal].nTotalSellCheckVersion = 0;
                    ea[nEaIdx].myTradeManager.arrBuyedSlots[disposal].sCurOrgOrderId = null;
                    PrintLog($"[매도취소] {nSharedTime} : {ea[nEaIdx].sCode}  {ea[nEaIdx].sCodeName} {disposal}블록 {ea[nEaIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume}주 취소", nEaIdx, disposal);
                }
            }
        }


        // 슬롯을 두개로 분리한다.
        public void SeperateSlot(int nEaIdx, int nSlotIdx, int nSeperateNum)
        {
            BuyedSlot mySlot = ea[nEaIdx].myTradeManager.arrBuyedSlots[nSlotIdx];
            BuyedSlot newSlot = mySlot.DeepCopy();

            newSlot.nCurVolume = mySlot.nCurVolume - nSeperateNum;
            mySlot.nCurVolume = nSeperateNum;
            newSlot.nBuyVolume = newSlot.nCurVolume;
            mySlot.nBuyVolume -= newSlot.nBuyVolume;
            newSlot.nBuyedSlotId = ea[nEaIdx].myTradeManager.arrBuyedSlots.Count;
            ea[nEaIdx].myTradeManager.arrBuyedSlots.Add(newSlot);

            mySlot.sEachLog.Append($"원본에서 ( 블록 : {mySlot.nBuyedSlotId} 현재 : {mySlot.nCurVolume} )와 분리개체 ( 블록 : {newSlot.nBuyedSlotId} 현재 : {newSlot.nCurVolume} )으로 분리 완료");
            newSlot.sEachLog.Append($"( 블록 : {mySlot.nBuyedSlotId} 현재 : {mySlot.nCurVolume} )와 분리개체 ( 블록 : {newSlot.nBuyedSlotId} 현재 : {newSlot.nCurVolume} )으로 분리 완료");
            PrintLog($"{nSharedTime} {ea[nEaIdx].sCode} {ea[nEaIdx].sCodeName} {newSlot.nBuyedSlotId}블록 분리  {newSlot.nCurVolume}(주)");
        }

        // 접수완료돼 취소 가능 애로우를 띄우기 위한 함수
        public void CallReceiptConfirm(int nEaIdx)
        {
            if (ea[nEaIdx].eventMgr.cancelEachStockFormEventHandler != null)
                ea[nEaIdx].eventMgr.cancelEachStockFormEventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}
