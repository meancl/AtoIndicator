namespace AtoIndicator
{
    public partial class MainForm
    {

        public const int STEP_TRADE = 121; // 0%에서 60%까지 0.5% 만큼씩 나뉠때의 갯수 60 / 0.5 + 1(0) = 121


        // 천장선 : 0% ~ 60% 까지 있음
        public double[] arrCeiling = new double[STEP_TRADE] {   0, 0.005, 0.01, 0.015, 0.02, 0.025,
                                                                0.03, 0.035, 0.04, 0.045, 0.05,
                                                                0.055, 0.06, 0.065, 0.07, 0.075,
                                                                0.08, 0.085, 0.09, 0.095, 0.1,
                                                                0.105, 0.11, 0.115, 0.12, 0.125,
                                                                0.13, 0.135, 0.14, 0.145, 0.15,
                                                                0.155, 0.16, 0.165, 0.17, 0.175,
                                                                0.18, 0.185, 0.19, 0.195, 0.2,
                                                                0.205, 0.21, 0.215, 0.22, 0.225,
                                                                0.23, 0.235, 0.24, 0.245, 0.25,
                                                                0.255, 0.26, 0.265, 0.27, 0.275,
                                                                0.28, 0.285, 0.29, 0.295, 0.3,
                                                                0.305, 0.31, 0.315, 0.32, 0.325,
                                                                0.33, 0.335, 0.34, 0.345, 0.35,
                                                                0.355, 0.36, 0.365, 0.37, 0.375,
                                                                0.38, 0.385, 0.39, 0.395, 0.4,
                                                                0.405, 0.41, 0.415, 0.42, 0.425,
                                                                0.43, 0.435, 0.44, 0.445, 0.45,
                                                                0.455, 0.46, 0.465, 0.47, 0.475,
                                                                0.48, 0.485, 0.49, 0.495, 0.5,
                                                                0.505, 0.51, 0.515, 0.52, 0.525,
                                                                0.53, 0.535, 0.54, 0.545, 0.55,
                                                                0.555, 0.56, 0.565, 0.57, 0.575,
                                                                0.58, 0.585, 0.59, 0.595, 0.6
                                                            };

        // 기본형 바닥선 : 처음~ 1프로까지는 2퍼 차이, 나머지는 1프로 차이
        public double[] arrStepByStepFloor = new double[STEP_TRADE] {
                                                                -0.025, -0.015, -0.01, 0.005, 0.01, 0.015,
                                                                0.02, 0.025, 0.03, 0.035, 0.04,
                                                                0.045, 0.05, 0.055, 0.06, 0.065,
                                                                0.07, 0.075, 0.08, 0.085, 0.09,
                                                                0.095, 0.1, 0.105, 0.11, 0.115,
                                                                0.12, 0.125, 0.13, 0.135, 0.14,
                                                                0.145, 0.15, 0.155, 0.16, 0.165,
                                                                0.17, 0.175, 0.18, 0.185, 0.19,
                                                                0.195, 0.2, 0.205, 0.21, 0.215,
                                                                0.22, 0.225, 0.23, 0.235, 0.24,
                                                                0.245, 0.25, 0.255, 0.26, 0.265,
                                                                0.27, 0.275, 0.28, 0.285, 0.29,
                                                                0.295, 0.3, 0.305, 0.31, 0.315,
                                                                0.32, 0.325, 0.33, 0.335, 0.34,
                                                                0.345, 0.35, 0.355, 0.36, 0.365,
                                                                0.37, 0.375, 0.38, 0.385, 0.39,
                                                                0.395, 0.4, 0.405, 0.41, 0.415,
                                                                0.42, 0.425, 0.43, 0.435, 0.44,
                                                                0.445, 0.45, 0.455, 0.46, 0.465,
                                                                0.47, 0.475, 0.48, 0.485, 0.49,
                                                                0.495, 0.5, 0.505, 0.51, 0.515,
                                                                0.52, 0.525, 0.53, 0.535, 0.54,
                                                                0.545, 0.55, 0.555, 0.56, 0.565,
                                                                0.57, 0.575, 0.58, 0.585, 0.6
                                                           };

        public double[] arrBottomUpFloor = new double[STEP_TRADE] {
                                                                -0.025, -0.02, -0.01, -0.01, 0, 0.01,
                                                                0.015, 0.02, 0.025, 0.03, 0.035, 0.04,
                                                                0.045, 0.05, 0.055, 0.06, 0.065,
                                                                0.07, 0.075, 0.08, 0.085, 0.09,
                                                                0.095, 0.1, 0.105, 0.11, 0.115,
                                                                0.12, 0.125, 0.13, 0.135, 0.14,
                                                                0.145, 0.15, 0.155, 0.16, 0.165,
                                                                0.17, 0.175, 0.18, 0.185, 0.19,
                                                                0.195, 0.2, 0.205, 0.21, 0.215,
                                                                0.22, 0.225, 0.23, 0.235, 0.24,
                                                                0.245, 0.25, 0.255, 0.26, 0.265,
                                                                0.27, 0.275, 0.28, 0.285, 0.29,
                                                                0.295, 0.3, 0.305, 0.31, 0.315,
                                                                0.32, 0.325, 0.33, 0.335, 0.34,
                                                                0.345, 0.35, 0.355, 0.36, 0.365,
                                                                0.37, 0.375, 0.38, 0.385, 0.39,
                                                                0.395, 0.4, 0.405, 0.41, 0.415,
                                                                0.42, 0.425, 0.43, 0.435, 0.44,
                                                                0.445, 0.45, 0.455, 0.46, 0.465,
                                                                0.47, 0.475, 0.48, 0.485, 0.49,
                                                                0.495, 0.5, 0.505, 0.51, 0.515,
                                                                0.52, 0.525, 0.53, 0.535, 0.54,
                                                                0.545, 0.55, 0.555, 0.56, 0.565,
                                                                0.57, 0.575, 0.58, 0.6
                                                           };

        public double[] arrScalpingFloor = new double[STEP_TRADE] {
                                                                -0.02, -0.015, -0.01, 0, 0.01, 0.015,
                                                                0.02, 0.025, 0.03, 0.035, 0.04,
                                                                0.045, 0.05, 0.055, 0.06, 0.065,
                                                                0.07, 0.075, 0.08, 0.085, 0.09,
                                                                0.095, 0.1, 0.105, 0.11, 0.115,
                                                                0.12, 0.125, 0.13, 0.135, 0.14,
                                                                0.145, 0.15, 0.155, 0.16, 0.165,
                                                                0.17, 0.175, 0.18, 0.185, 0.19,
                                                                0.195, 0.2, 0.205, 0.21, 0.215,
                                                                0.22, 0.225, 0.23, 0.235, 0.24,
                                                                0.245, 0.25, 0.255, 0.26, 0.265,
                                                                0.27, 0.275, 0.28, 0.285, 0.29,
                                                                0.295, 0.3, 0.305, 0.31, 0.315,
                                                                0.32, 0.325, 0.33, 0.335, 0.34,
                                                                0.345, 0.35, 0.355, 0.36, 0.365,
                                                                0.37, 0.375, 0.38, 0.385, 0.39,
                                                                0.395, 0.4, 0.405, 0.41, 0.415,
                                                                0.42, 0.425, 0.43, 0.435, 0.44,
                                                                0.445, 0.45, 0.455, 0.46, 0.465,
                                                                0.47, 0.475, 0.48, 0.485, 0.49,
                                                                0.495, 0.5, 0.505, 0.51, 0.515,
                                                                0.52, 0.525, 0.53, 0.535, 0.54,
                                                                0.545, 0.55, 0.555, 0.56, 0.565,
                                                                0.57, 0.575, 0.58, 0.585, 0.6
                                                           };



        // =========================================
        // 마지막 편집일 : 2023-04-20
        // 1. 다음 윗 계단을 반환
        // =========================================
        double GetNextCeiling(ref int fCurTargetIdx)
        {
            if (fCurTargetIdx >= STEP_TRADE)
                fCurTargetIdx = STEP_TRADE - 1;
            return arrCeiling[fCurTargetIdx];
        }

        // =========================================
        // 마지막 편집일 : 2023-04-20
        // 1. 다음 아래 계단을 반환
        // =========================================
        double GetNextFloor(ref int nIdx, TradeMethodCategory eTradeMethod)
        {
            if (nIdx >= STEP_TRADE)
                nIdx = STEP_TRADE - 1;

            double retVal = 0;
            switch (eTradeMethod)
            {
                case TradeMethodCategory.BottomUpMethod:
                    retVal = arrBottomUpFloor[nIdx];
                    break;
                case TradeMethodCategory.RisingMethod:
                    retVal = arrStepByStepFloor[nIdx];
                    break;
                case TradeMethodCategory.ScalpingMethod:
                    retVal = arrScalpingFloor[nIdx];
                    break;
                case TradeMethodCategory.None:
                    retVal = arrStepByStepFloor[nIdx];
                    break;
                default:
                    PrintLog("매매방식이 잘못됐습니다.");
                    break;
            }
            return retVal;
        }


        // =========================================
        // 마지막 편집일 : 2023-04-20
        // 1. 매매에 맞게 처리 후 매매 컨트롤러에 전해준다.
        // =========================================
        public void SetAndServeCurSlot(bool isByHand, int nOrderType, int nEaIdx, string sCode, string sHogaGb, 
                                        int nOrderPrice, int nQty, string sRQName,  string sOrgOrderId, string sDescription,
                                        int nBuyedSlotIdx = 0, int nSequence = 0, double fRequestRatio=NORMAL_TRADE_RATIO, int nStrategyIdx=0,
                                        double fCeil = 0.02, double fFloor = -0.025, TradeMethodCategory eTradeMethod = TradeMethodCategory.None
                                        )
        {
            // 공용
            curSlot.isByHand = isByHand;
            curSlot.nOrderType = nOrderType;
            curSlot.nEaIdx = nEaIdx;
            curSlot.sCode = sCode;
            curSlot.sHogaGb = sHogaGb;
            curSlot.nOrderPrice = nOrderPrice;
            curSlot.nQty = nQty;
            curSlot.sRQName = sRQName;
            curSlot.sOrgOrderId = sOrgOrderId;
            curSlot.sDescription = sDescription;

            curSlot.nRqTime = nSharedTime;
            curSlot.eTradeMethod = eTradeMethod;


            switch (nOrderType)
            {
                case NEW_BUY:
                    curSlot.fRequestRatio = fRequestRatio;
                    curSlot.nStrategyIdx = nStrategyIdx;
                    curSlot.nSequence = nSequence;
                    curSlot.fTargetPercent = fCeil;
                    curSlot.fBottomPercent = fFloor;
                    break;
                case NEW_SELL:
                    if (!isByHand)
                    {
                        curSlot.nBuyedSlotIdx = nBuyedSlotIdx;
                        ea[nEaIdx].myTradeManager.arrBuyedSlots[nBuyedSlotIdx].isSelling = true;
                    }
                    break;
                case BUY_CANCEL:
                    BuyedSlot slot = slotDict[sOrgOrderId];
                    slot.isCanceling = true;
                    slot.nDeathTime = nSharedTime; // 매수취소가 일부 혹은 전량 실패해도 매도하면서 nDeathTime이 덮어지니 괜찮다.
                    slot.nDeathPrice = ea[nEaIdx].nFs;
                    slot.sSellDescription = sRQName;
                    break;
                default:
                    break;
            }

            tradeQueue.Enqueue(curSlot);
        }
    }
}
