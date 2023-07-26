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
        public const double KOSPI_STOCK_TAX1 = 0.0005; // 코스피 거래세
        public const double KOSPI_STOCK_TAX2 = 0.0015; // 코스피 농특세
        public const double KOSDAQ_STOCK_TAX = 0.002; // 코스닥 거래세

        public const double KIWOOM_STOCK_FEE = 0.00015; // 증권 매매수수료

        public const double DEFAULT_FIXED_CEILING = 0.03;
        public const double DEFAULT_FIXED_BOTTOM = -0.015;

        #endregion

        public Dictionary<string, int> sellRemainCheckByOrderIdDict = new Dictionary<string, int>(); // 남은 매도를 확인하기 위한 딕셔너리
        public Dictionary<string, BuyedSlot> buySlotByOrderIdDict = new Dictionary<string, BuyedSlot>();

        public Dictionary<string, VirtualSellBlock> virtualSellBlockByOrderIdDict = new Dictionary<string, VirtualSellBlock>();
        public Dictionary<string, VirtualSellBlock> virtualSellBlockByScrNoDict = new Dictionary<string, VirtualSellBlock>();
        
        
        public Dictionary<string, int> buyCancelingByOrderIdDict = new Dictionary<string, int>(); // 매수취소중 딕셔너리
        public Dictionary<string, int> sellCancelingByOrderIdDict = new Dictionary<string, int>(); // 매도취소중 딕셔너리



        public enum SellReceiptType
        {
            None,
            NORMAL_BLOCK,
            GONE_BLOCK,
            GROUPING_BLOCK,
        }

        public class VirtualSellBlock
        {
            public string sOrderId;
            public string sScrNo;
            public List<BuyedSlot> slotList;
            public string sDescription;
            public SellReceiptType eSellReceiptType = SellReceiptType.None; // 1번 : 정상, 2번 : 자리뺏김, 3번 : 그룹핑

            public int nOrderPrice;
            public int nOrderVolume;
            public int nOrderTime;
            public int nProcessedVolume;

            public VirtualSellBlock()
            {
                slotList = new List<BuyedSlot>();
            }

        }

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
                            if (buySlotByOrderIdDict.ContainsKey(sOrderId))
                            {
                                BuyedSlot slot = buySlotByOrderIdDict[sOrderId];
                                // 전량 매수취소의 경우 가격과 러프한 매수수수료를 다시 더해준다.
                                nCurDepositCalc += slot.nOrderPrice * slot.nOrderVolume + ea[nCurIdx].feeMgr.GetRoughBuyFee(slot.nOrderPrice, slot.nOrderVolume); 
                            }
                            PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}가 전량매수취소 완료");
                            ShutOffScreen(sScrNo);
                            buySlotByOrderIdDict.Remove(sOrderId);
                            buyCancelingByOrderIdDict.Remove(sOrderId);
                            ea[nCurIdx].unhandledBuyOrderIdList.Remove(sOrderId);

                            CallReceiptConfirm(nCurIdx);

                        }
                        #endregion
                        #region 정상 매수주문
                        else
                        {
                            BuyedSlot slot = buySlotByOrderIdDict[sOrderId] = GetSlotFromScreen(sScrNo);

                            if (slot == null)
                            {
                                PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}가 손매수 접수 완료");


                                slot = buySlotByOrderIdDict[sOrderId] = new BuyedSlot(nCurIdx);
                                slot.nRequestTime = nSharedTime;
                                slot.nOriginOrderPrice = nOrderPrice; // 주문요청금액 설정
                                slot.nOrderPrice = slot.nOriginOrderPrice; // 지정상한가 설정
                                slot.eTradeMethod = ea[nCurIdx].myTradeManager.eDefaultTradeCategory; // 매매방법 설정
                                slot.nOrderVolume = nOrderVolume;
                                slot.fTradeRatio = 1;
                                slot.sBuyDescription = "손매수";
                                slot.isBuying = true;
                                slot.isBuyByHand = true;
                                slot.nCurLineIdx = 0;

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
                                        slot.fTargetPer = DEFAULT_FIXED_CEILING;
                                        slot.fBottomPer = DEFAULT_FIXED_BOTTOM;
                                        break;
                                    default:
                                        break;
                                }

                                // 손매수접수의 경우, 주문금액과 러프한 수수료를 뺀다.
                                nCurDepositCalc -= slot.nOrderPrice * slot.nOrderVolume + ea[nCurIdx].feeMgr.GetRoughBuyFee(slot.nOrderPrice , slot.nOrderVolume);
                            }

                            slot.nReceiptTime = nSharedTime;
                            slot.sCurOrgOrderId = sOrderId; // 현재원주문번호 설정
                            slot.isResponsed = true;
                            slot.sBuyScrNo = sScrNo;
                            ea[nCurIdx].myTradeManager.isOrderStatus = true; // 매매중 on
                            ea[nCurIdx].unhandledBuyOrderIdList.Add(sOrderId); // 매수취소할 수 있게

                            buySlotByOrderIdDict[sOrderId].sEachLog.Append($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원 매수 접수완료{NEW_LINE}");
                            PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원 매수 접수완료", nCurIdx);

                            CallReceiptConfirm(nCurIdx);
                        }
                        #endregion
                    }
                    else if (sOrderStatus.Equals("체결"))
                    {
                        int nCurOkTradeVolume;
                        int nCurOkTradePrice;
                        BuyedSlot slot = buySlotByOrderIdDict[sOrderId];

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
                                ea[nCurIdx].myTradeManager.isRealBuyChangeNeeded = true;
                                
                                slot.nBirthTime = nSharedTime;
                                slot.nBirthPrice = slot.nBuyPrice;


                                // 일부 매수취소의 경우 잔량의 대금을 더해주고 잔량만큼의 러프한 수수료를 더해준다.
                                nCurDepositCalc += (slot.nOrderVolume - slot.nBuyVolume) * slot.nOrderPrice; // 차액 더하기
                                nCurDepositCalc += ea[nCurIdx].feeMgr.GetRoughBuyFee( slot.nOrderPrice, slot.nOrderVolume - slot.nBuyVolume); // 


                                // 기록용
                                slot.nBuyMinuteIdx = nTimeLineIdx;

                                slot.nBuyedSlotId = ea[nCurIdx].myTradeManager.arrBuyedSlots.Count;
                                ea[nCurIdx].myTradeManager.nAppliedShowingRealBuyedId = slot.nBuyedSlotId;

                                ea[nCurIdx].myTradeManager.arrBuyedSlots.Add(slot);

                                ShutOffScreen(sScrNo);
                                buySlotByOrderIdDict.Remove(sOrderId);
                                buyCancelingByOrderIdDict.Remove(sOrderId);
                                ea[nCurIdx].unhandledBuyOrderIdList.Remove(sOrderId);
                                CallReceiptConfirm(nCurIdx);

                                PrintLog($"시간 : {sTradeTime} {sCode} {ea[nCurIdx].sCodeName} 화면번호 : {sScrNo} {slot.nBuyVolume}(주) 일부 체결완료, 총 주문수량 : {slot.nOrderVolume} 매수가 : {slot.nBuyPrice}", nCurIdx);
                            }

                            return;
                        }
                        #endregion
                        #region 일반체결
                        // 일반체결의 경우 이전의 러프한 수수료만큼 더해주고 체결된 금액과 주문 금액 차를 체결수량만큼 더해주고
                        // 체결 안된거는 러프한 주문상한가로 빼주고 체결된 부분에 대해서는 실제 매수수수료로 책정해서 빼준다.
                        nCurDepositCalc += ea[nCurIdx].feeMgr.GetRoughBuyFee(slot.nOrderPrice, slot.nOrderVolume - slot.nBuyVolume); // 잔량만큼 다시 수수료를 복구해주고
                        nCurDepositCalc += (slot.nOrderPrice - nCurOkTradePrice) * nCurOkTradeVolume;  // 차액만큼은 넣어주고
                        nCurDepositCalc -= ea[nCurIdx].feeMgr.GetRoughBuyFee(slot.nOrderPrice, slot.nOrderVolume - (slot.nBuyVolume + nCurOkTradeVolume)); // 잔량만큼 다시 수수료를 빼주고
                        nCurDepositCalc -= ea[nCurIdx].feeMgr.GetBuyFee(nCurOkTradePrice, nCurOkTradeVolume);

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
                            slot.isAllBuyed = true; // 해당종목의 매수레코드의 매수완료 on
                            slot.isBuying = false;
                            slot.nBuyEndTime = nSharedTime; // 주문체결완료 시간 설정
                            if (ea[nCurIdx].myTradeManager.nBuyReqCnt > 0)
                                ea[nCurIdx].myTradeManager.nBuyReqCnt--; // 매수요청 카운트 감소
                            ea[nCurIdx].myTradeManager.isOrderStatus = false; // 매매중 off
                            ea[nCurIdx].myTradeManager.isRealBuyChangeNeeded = true;
                            

                            slot.nBirthTime = nSharedTime;

                            slot.nBirthPrice = slot.nBuyPrice;

                            slot.nBuyMinuteIdx = nTimeLineIdx;

                            slot.nBuyedSlotId = ea[nCurIdx].myTradeManager.arrBuyedSlots.Count;
                            ea[nCurIdx].myTradeManager.nAppliedShowingRealBuyedId = slot.nBuyedSlotId;

                            ea[nCurIdx].myTradeManager.arrBuyedSlots.Add(slot);

                            ShutOffScreen(sScrNo); // 매수체결완료 해당화면번호 꺼줍니다.
                            buySlotByOrderIdDict.Remove(sOrderId);
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
                            if (virtualSellBlockByOrderIdDict.ContainsKey(sOrderId))
                                ResetGroupSellingBack(virtualSellBlockByOrderIdDict[sOrderId]);

                            if (sellRemainCheckByOrderIdDict.ContainsKey(sOrderId))
                                ea[nCurIdx].myTradeManager.nTotalSelling -= sellRemainCheckByOrderIdDict[sOrderId];
                            PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원 전량매도취소 완료");

                            virtualSellBlockByOrderIdDict.Remove(sOrderId);
                            virtualSellBlockByScrNoDict.Remove(sScrNo);
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
                            BuyedSlot slot = GetSlotFromScreen(sScrNo);

                            VirtualSellBlock virtualSellBlock = new VirtualSellBlock();
                            bool isGoneBlocked = false;
                            if (slot != null) // 기계주문
                            {
                                if (slot.isSellStarted) // 이미 다른 그룹에 뺏겼다면
                                { // 찾아야한다.
                                    slot = null;
                                    isGoneBlocked = true;
                                }
                                else // 정상 블럭매도 가능
                                {
                                    virtualSellBlock.sOrderId = sOrderId;
                                    virtualSellBlock.sScrNo = sScrNo;
                                    virtualSellBlock.sDescription = slot.sSellDescription;
                                    virtualSellBlock.nOrderPrice = nOrderPrice;
                                    virtualSellBlock.nOrderVolume = nOrderVolume;
                                    virtualSellBlock.nOrderTime = nSharedTime;
                                    virtualSellBlock.eSellReceiptType = SellReceiptType.NORMAL_BLOCK;
                                    virtualSellBlock.slotList.Add(slot);

                                    slot.isSelling = true; // 내 슬롯이라 괜찮
                                    slot.isSellStarted = true; // 매도 시작

                                    virtualSellBlockByScrNoDict[sScrNo] = virtualSellBlockByOrderIdDict[sOrderId] = virtualSellBlock;

                                    slot.sEachLog.Append($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원  매도 접수완료{NEW_LINE}");
                                }


                                PrintLog($"{nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {nOrderVolume}주 {nOrderPrice}원 {slot.nBuyedSlotId}블록 매도 접수완료", nCurIdx);
                            }

                            if (slot == null) // 나 주문
                            {
                                PrintLog($"[손매도접수-처리전] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} 매도가 : {nOrderPrice}, 매도수량 : {nOrderVolume}", nCurIdx);

                                int tmpCurOrderVolume = nOrderVolume;

                                for (int disposal = 0; disposal < ea[nCurIdx].myTradeManager.arrBuyedSlots.Count && tmpCurOrderVolume > 0; disposal++)
                                {
                                    if (ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume > 0 && !ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].isSelling)
                                    {
                                        int disposalVolume = ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume > tmpCurOrderVolume ? tmpCurOrderVolume : ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume;

                                        if (disposalVolume < ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].nCurVolume) // 분리 조건
                                        {
                                            PrintLog($"[{(isGoneBlocked ? "빼앗긴블럭" : "손매도접수")}-분리] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} {disposal}블록 {disposalVolume}주상태로 분리");
                                            SeperateSlot(nCurIdx, disposal, disposalVolume);
                                        }

                                        tmpCurOrderVolume -= disposalVolume;

                                        // 등록===>
                                        virtualSellBlock.sOrderId = sOrderId;
                                        virtualSellBlock.sScrNo = sScrNo;
                                        virtualSellBlock.sDescription = isGoneBlocked? GetSlotFromScreen(sScrNo).sSellDescription : "손매도";
                                        virtualSellBlock.nOrderPrice = nOrderPrice;
                                        virtualSellBlock.nOrderVolume = nOrderVolume;
                                        virtualSellBlock.nOrderTime = nSharedTime;
                                        virtualSellBlock.eSellReceiptType = isGoneBlocked ? SellReceiptType.GONE_BLOCK : SellReceiptType.GROUPING_BLOCK;
                                        virtualSellBlock.slotList.Add(ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal]);

                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].isSelling = true; // 내 슬롯이라 괜찮
                                        ea[nCurIdx].myTradeManager.arrBuyedSlots[disposal].isSellStarted = true; // 매도 시작

                                        virtualSellBlockByScrNoDict[sScrNo] = virtualSellBlockByOrderIdDict[sOrderId] = virtualSellBlock;
                                        // <===등록

                                        PrintLog($"{(isGoneBlocked ? "빼앗긴블럭" : "손매도접수")} {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} {disposal}블록 {disposalVolume}주 접수", nCurIdx, disposal);
                                    }
                                }
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
                            if (virtualSellBlockByOrderIdDict.ContainsKey(sOrderId))
                                ResetGroupSellingBack(virtualSellBlockByOrderIdDict[sOrderId]);
                            if (sellRemainCheckByOrderIdDict.ContainsKey(sOrderId))
                                ea[nCurIdx].myTradeManager.nTotalSelling -= sellRemainCheckByOrderIdDict[sOrderId];

                            virtualSellBlockByOrderIdDict.Remove(sOrderId);
                            virtualSellBlockByScrNoDict.Remove(sScrNo);
                            sellRemainCheckByOrderIdDict.Remove(sOrderId);
                            sellCancelingByOrderIdDict.Remove(sOrderId);
                            ea[nCurIdx].unhandledSellOrderIdList.Remove(sOrderId);
                            ShutOffScreen(sScrNo);
                            CallReceiptConfirm(nCurIdx);
                            return; // 매도취소에 대한 확인용 메시지는 스킵한다.
                        }


                        #region 매도 체결 처리
                        nCurDepositCalc += nCurOkTradeVolume * nCurOkTradePrice - (ea[nCurIdx].feeMgr.GetSellFee(nCurOkTradePrice, nCurOkTradeVolume) + ea[nCurIdx].feeMgr.GetSellTax( nCurOkTradePrice, nCurOkTradeVolume, ea[nCurIdx].nMarketGubun)); // 세금 빼고 판만큼 더해주기
                        nTodayDisposalSellPrice += nCurOkTradeVolume * nCurOkTradePrice; // 오늘자 매도가격 증가

                        ea[nCurIdx].myTradeManager.nTotalSelling -= nCurOkTradeVolume;
                        ea[nCurIdx].myTradeManager.nTotalSelled += nCurOkTradeVolume;
                        sellRemainCheckByOrderIdDict[sOrderId] -= nCurOkTradeVolume;

                        var virtualSellBlock = virtualSellBlockByOrderIdDict[sOrderId];

                        {
                            int tmpCurOkTradeVolume = nCurOkTradeVolume;
                            string sMsg;

                            if (virtualSellBlock.eSellReceiptType == SellReceiptType.NORMAL_BLOCK)
                                sMsg = "정상블럭";
                            else if (virtualSellBlock.eSellReceiptType == SellReceiptType.GONE_BLOCK)
                                sMsg = "빼앗긴블럭";
                            else if (virtualSellBlock.eSellReceiptType == SellReceiptType.GROUPING_BLOCK)
                                sMsg = "그룹핑블럭";
                            else
                                sMsg = "오류!";

                            PrintLog($"[{sMsg}-처리전] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName}  매도가 : {nCurOkTradePrice}, 매도수량 : {nCurOkTradeVolume}", nCurIdx);

                            foreach (BuyedSlot member in virtualSellBlock.slotList)
                            {
                                if (!member.isAllSelled)
                                {
                                    int disposalVolume = member.nCurVolume > tmpCurOkTradeVolume ? tmpCurOkTradeVolume : member.nCurVolume;

                                    member.nCurVolume -= disposalVolume;
                                    member.nSellVolume += disposalVolume; // 처분갯수 증가
                                    member.nSelledSumPrice += disposalVolume * nCurOkTradePrice; // 처분총가격 증가 
                                    virtualSellBlock.nProcessedVolume += disposalVolume;

                                    if (member.nCurVolume <= 0)
                                    {
                                        member.sSellDescription = virtualSellBlock.sDescription;
                                        member.nCurVolume = 0;
                                        member.nSellMinuteIdx = nTimeLineIdx;
                                        member.isAllSelled = true;
                                        member.sCurOrgOrderId = null;
                                        member.isSelling = false;
                                        member.isSellStarted = false;
                                        member.nDeathTime = nSharedTime;
                                        member.nDeathPrice = member.nSelledSumPrice / member.nSellVolume;
                                        ea[nCurIdx].myTradeManager.nSellReqCnt--;
                                        ea[nCurIdx].myTradeManager.isOrderStatus = false;
                                        ea[nCurIdx].myTradeManager.isRealBuyChangeNeeded = true;

                                        PrintLog($"[{sMsg}] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} {member.nBuyedSlotId}블록 매도 체결완료, 매도가 : {member.nDeathPrice} 손익 : {GetProfitPercent(member.nBuyedSumPrice, member.nSelledSumPrice, ea[nCurIdx].nMarketGubun)}", nCurIdx, member.nBuyedSlotId);
                                    }
                                    else
                                        PrintLog($"[{sMsg}] {nSharedTime} : {ea[nCurIdx].sCode}  {ea[nCurIdx].sCodeName} {member.nBuyedSlotId}블록 {disposalVolume}(주) {nCurOkTradePrice}(원) .. 잔량 : {nNoTradeVolume}(주)", nCurIdx, member.nBuyedSlotId);

                                    tmpCurOkTradeVolume -= disposalVolume;
                                }
                            }


                        }
                        #endregion

                        if (nNoTradeVolume == 0) // 매도정상완료
                        {
                            ea[nCurIdx].feeMgr.SetDoneSellOneSet();

                            virtualSellBlockByOrderIdDict.Remove(sOrderId);
                            virtualSellBlockByScrNoDict.Remove(sScrNo);
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
        public void ResetGroupSellingBack(VirtualSellBlock groupMembers, bool isDelayed = false)
        {
            foreach (var member in groupMembers.slotList)
            {
                if (isDelayed)
                {
                    member.isSellCancelReserved = true;
                    member.nSellCancelReserveTime = nSharedTime;
                    member.nSellErrorCount++;
                    member.nSellErrorLastTime = nSharedTime;
                    PrintLog($"[그룹핑 종료대기] {nSharedTime} : {ea[member.nEaIdx].sCode}  {ea[member.nEaIdx].sCodeName} {member.nBuyedSlotId}블록 {ea[member.nEaIdx].myTradeManager.arrBuyedSlots[member.nBuyedSlotId].nCurVolume} 잔량", member.nEaIdx, member.nBuyedSlotId);
                }
                else
                {
                    member.isSelling = false;
                    member.isSellStarted = false;
                    PrintLog($"[그룹핑 종료] {nSharedTime} : {ea[member.nEaIdx].sCode}  {ea[member.nEaIdx].sCodeName} {member.nBuyedSlotId}블록 {ea[member.nEaIdx].myTradeManager.arrBuyedSlots[member.nBuyedSlotId].nCurVolume} 잔량", member.nEaIdx, member.nBuyedSlotId);
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
            newSlot.nBuyedSumPrice = newSlot.nBuyPrice * newSlot.nBuyVolume;
            mySlot.nBuyedSumPrice = mySlot.nBuyPrice * mySlot.nBuyVolume;
            newSlot.nBuyedSlotId = ea[nEaIdx].myTradeManager.arrBuyedSlots.Count;
            ea[nEaIdx].myTradeManager.arrBuyedSlots.Add(newSlot);

            //newSlot.nTotalSellCheckVersion = 0;

            mySlot.sEachLog.Append($"원본에서 ( 블록 : {mySlot.nBuyedSlotId} 현재 : {mySlot.nCurVolume} )와 분리개체 ( 블록 : {newSlot.nBuyedSlotId} 현재 : {newSlot.nCurVolume} )으로 분리 완료{NEW_LINE}");
            newSlot.sEachLog.Append($"( 블록 : {mySlot.nBuyedSlotId} 현재 : {mySlot.nCurVolume} )와 분리개체 ( 블록 : {newSlot.nBuyedSlotId} 현재 : {newSlot.nCurVolume} )으로 분리 완료{NEW_LINE}");
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
