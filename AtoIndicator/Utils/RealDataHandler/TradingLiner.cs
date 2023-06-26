using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static AtoIndicator.KiwoomLib.TimeLib;
using static AtoIndicator.Utils.Comparer;
using static AtoIndicator.KiwoomLib.PricingLib;

namespace AtoIndicator
{
    public partial class MainForm
    {
        public bool HandleTradeLine(int nCurIdx, int checkSellIterIdx)
        {
            void RequestThisSell()
            {
                try
                {
                    sSharedSellDescription.Append($"매도단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx}");

                    SetAndServeCurSlot(false, NEW_SELL, nCurIdx, ea[nCurIdx].sCode, MARKET_ORDER, 0, ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurVolume,"매도", "", sSharedSellDescription.ToString(), nBuyedSlotIdx:checkSellIterIdx);

                    PrintLog($"{checkSellIterIdx}번째 매매슬롯 매도단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx}", nCurIdx, checkSellIterIdx, false);
                    PrintLog($"매도인큐 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%) 현재단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx}");

                }
                catch (Exception ex)
                {
                }
            }

            bool isSell = false;

            sSharedSellDescription.Clear();


            // <=== 공통적으로 가격 움직임 체크가 가능하다
            if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee > ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fCheckCeilingPer)
            {
                while (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee > ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fCheckCeilingPer)
                {
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx = RaiseStepUp(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx);
                    var nextFullStep = GetMovedFullStep(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx);
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fCheckCeilingPer = nextFullStep.Item1;
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fCheckBottomPer = nextFullStep.Item2;

                    if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx > ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nMaxCheckLineIdx)
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nMaxCheckLineIdx = ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx;
                }
            }
            else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee < ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fCheckBottomPer)
            {
                while (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee < ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fCheckBottomPer)
                {
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx = PullStepDown(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx);
                    var nextFullStep = GetMovedFullStep(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx);
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fCheckCeilingPer = nextFullStep.Item1;
                    ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fCheckBottomPer = nextFullStep.Item2;

                    if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx < ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nMinCheckLineIdx)
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nMinCheckLineIdx = ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCheckLineIdx;
                }
            }
            // >===



            if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].eTradeMethod == TradeMethodCategory.RisingMethod) // 단계별 매매기법
            {
                {
                }

                if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee <= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer) // 처분라인
                {
                    isSell = true;
                    PrintLog($"라이징 매도 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"라이징 매도{NEW_LINE}");
                } // END ---- 처분라인
                else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer) // 상승라인
                {
                    while (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer)
                    {
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx = RaiseStepUp(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx);
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer = GetNextCeiling(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx); // something higher
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer = GetNextFloor(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx, TradeMethodCategory.RisingMethod); // something higher
                    }

                    if (isSell) // 단계는 상승했는데 팔린다는 건 이상하니 
                    {
                        isSell = false;
                        PrintLog($"단계상승으로 일시적 매도취소 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} 단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx} 손익 : {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx, false);
                    }

                    // 추가매수 파트, 추가매수의 전략은 항상 0번이다.
                    //if (!ea[nCurIdx].isExcluded)
                    //  {} // END ---- 추가매수 파트

                } // END---- 상승라인


            } // END ---- 단계형 매매
            else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].eTradeMethod == TradeMethodCategory.ScalpingMethod) // 스캘핑 매매기법
            {
                {

                }

                if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee <= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer) // 처분라인
                {
                    isSell = true;
                    PrintLog($"스캘핑 매도 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"스캘핑 매도{NEW_LINE}");
                }
                else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer) // 상승라인
                {
                    while (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer)
                    {
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx = RaiseStepUp(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx);
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer = GetNextCeiling(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx); // something higher
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer = GetNextFloor(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx, TradeMethodCategory.ScalpingMethod); // something higher
                    }

                    if (isSell) // 단계는 상승했는데 팔린다는 건 이상하니 
                    {
                        isSell = false;
                        PrintLog($"단계상승으로 일시적 매도취소 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} 단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx} 손익 : {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx, false);
                    }
                }

            }// END ----  스캘핑 매매
            else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].eTradeMethod == TradeMethodCategory.BottomUpMethod) // 바텀업 매매기법
            {
                {

                }

                if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee <= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer) // 처분라인
                {
                    isSell = true;
                    PrintLog($"바텀업 매도 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"바텀업 매도{NEW_LINE}");
                }
                else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer) // 상승라인
                {
                    while (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer)
                    {
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx = RaiseStepUp(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx);
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer = GetNextCeiling(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx); // something higher
                        ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer = GetNextFloor(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx, TradeMethodCategory.BottomUpMethod); // something higher
                    }

                    if (isSell) // 단계는 상승했는데 팔린다는 건 이상하니 
                    {
                        isSell = false;
                        PrintLog($"단계상승으로 일시적 매도취소 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName} 단계 : {ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].nCurLineIdx} 손익 : {Math.Round(ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee * 100, 1)}(%)", nCurIdx, checkSellIterIdx, false);
                    }
                }

            }// END ---- 바텀업 매매
            else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].eTradeMethod == TradeMethodCategory.FixedMethod) // 고정형 매매기법
            {
                if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee <= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fBottomPer) // 손익률이 손절퍼센트보다 낮으면
                {
                    isSell = true;
                    PrintLog($"고정형 매도 - 손절 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} : {ea[nCurIdx].sCode} 손절매도신청", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"고정형 매도 - 손절{NEW_LINE}");
                }
                else if (ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fPowerWithFee >= ea[nCurIdx].myTradeManager.arrBuyedSlots[checkSellIterIdx].fTargetPer) // 손익률이 익절퍼센트를 넘기면
                {
                    isSell = true;
                    PrintLog($"고정형 매도 - 익절 : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} : {ea[nCurIdx].sCode} 익절매도신청", nCurIdx, checkSellIterIdx);
                    sSharedSellDescription.Append($"고정형 매도 - 익절{NEW_LINE}");
                }
            } // END---- 고정형 매매
            else // None
            {

            }

            if (ea[nCurIdx].fPower >= 0.29)
            {
                isSell = true;
                sSharedSellDescription.Append($"상한가 도달!{NEW_LINE}");
                PrintLog($"상한가 도달! : {checkSellIterIdx}번째 매매슬롯 {nSharedTime} {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx, checkSellIterIdx);
            }

            if (isSell)
                RequestThisSell();

            return isSell;
        }


        #region RequestHandBuy
        public void RequestHandBuy(int nCurIdx, int nRequestPrice = 0, int nQty = 0)
        {
            try
            {
                int priceToOrder;
                if (nRequestPrice <= 0)
                    priceToOrder = ea[nCurIdx].nFb + GetIntegratedMarketGap(ea[nCurIdx].nFb);
                else
                    priceToOrder = nRequestPrice;


                SetAndServeCurSlot(true, NEW_BUY, nCurIdx, ea[nCurIdx].sCode, PENDING_ORDER, priceToOrder, nQty, "신규매수", "", "손매수");


                PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}, 매수가 : {priceToOrder} 손매수신청", nCurIdx);
            }
            catch (Exception ex)
            {
                PrintLog($"매수 체크 중 오류 발생 {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx);
            }
        }
        #endregion

        #region RequestHandSell
        public void RequestHandSell(int nCurIdx, int nRequestPrice, int nQty)
        {
            try
            {
                int priceToOrder = nRequestPrice;
                int volumeToOrder = nQty;

                if (volumeToOrder <= 0)
                {
                    PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}  매도 수량을 선택하지 않았습니다.");
                    return;
                }

                if (volumeToOrder > ea[nCurIdx].myTradeManager.nTotalBuyed - (ea[nCurIdx].myTradeManager.nTotalSelled + ea[nCurIdx].myTradeManager.nTotalSelling))
                {
                    volumeToOrder = ea[nCurIdx].myTradeManager.nTotalBuyed - (ea[nCurIdx].myTradeManager.nTotalSelled + ea[nCurIdx].myTradeManager.nTotalSelling);
                    if (volumeToOrder == 0)
                    {
                        PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}  보유주식이 없습니다.");
                        return;
                    }
                }
               

                SetAndServeCurSlot(true, NEW_SELL, nCurIdx, ea[nCurIdx].sCode, PENDING_ORDER, priceToOrder, volumeToOrder, "신규매도", "", "손매도");


                PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}, 매도가 : {priceToOrder} 손매도신청", nCurIdx);
            }
            catch (Exception ex)
            {
                PrintLog($"매도 체크 중 오류 발생 {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx);
            }
        }
        #endregion

        #region RequestHandBuyCancel
        public void RequestHandBuyCancel(int nCurIdx, string sOriginOrderId)
        {
            try
            {
                SetAndServeCurSlot(true, BUY_CANCEL, nCurIdx, ea[nCurIdx].sCode, MARKET_ORDER, 0, 0, "신규매수취소", sOriginOrderId, "손매수취소");


                PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}  매수취소신청", nCurIdx);
            }
            catch (Exception ex)
            {
                PrintLog($"매수 체크 중 오류 발생 {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx);
            }
        }
        #endregion

        #region RequestHandSellCancel
        public void RequestHandSellCancel(int nCurIdx, string sOriginOrderId)
        {
            try
            {
                SetAndServeCurSlot(true, SELL_CANCEL, nCurIdx, ea[nCurIdx].sCode, MARKET_ORDER, 0, 0, "신규매도취소", sOriginOrderId, "손매도취소");


                PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}  매도취소신청", nCurIdx);
            }
            catch (Exception ex)
            {
                PrintLog($"매수 체크 중 오류 발생 {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx);
            }
        }
        #endregion

        #region RequestMachineBuy
        public void RequestMachineBuy(int nCurIdx, int nRequestPrice = 0, int nQty = 0)
        {
            try
            {
                int priceToOrder;
                if (nRequestPrice <= 0)
                    priceToOrder = GetPriceFewSteps(ea[nCurIdx].nFb, 2);
                else
                    priceToOrder = nRequestPrice;


                SetAndServeCurSlot(false, NEW_BUY, nCurIdx, ea[nCurIdx].sCode, PENDING_ORDER, priceToOrder, nQty, "신규매수", "", "기계매수", eTradeMethod:TradeMethodCategory.FixedMethod);


                PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}, 매수가 : {priceToOrder} 손매수신청", nCurIdx);
            }
            catch (Exception ex)
            {
                PrintLog($"매수 체크 중 오류 발생 {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx);
            }
        }
        #endregion


        #region RequestHandBlockSell
        public void RequestHandBlockSell(int nCurIdx, int nBlockIdx)
        {
            try
            {
                SetAndServeCurSlot(false, NEW_SELL, nCurIdx, ea[nCurIdx].sCode, MARKET_ORDER, 0, ea[nCurIdx].myTradeManager.arrBuyedSlots[nBlockIdx].nCurVolume, "매도", "", "블럭 손매도" , nBuyedSlotIdx: nBlockIdx);
                
                PrintLog($"시간 : {nSharedTime}, 종목코드 : {ea[nCurIdx].sCode} 종목명 : {ea[nCurIdx].sCodeName}, 블럭 : {nBlockIdx} 블럭 손매도 신청", nCurIdx);
            }
            catch (Exception ex)
            {
                PrintLog($"블럭 손매도 체크 중 오류 발생 {ea[nCurIdx].sCode} {ea[nCurIdx].sCodeName}", nCurIdx);
            }
        }
        #endregion

    }
}
