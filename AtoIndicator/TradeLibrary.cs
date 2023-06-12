﻿namespace AtoIndicator
{
    public partial class MainForm
    {

        public const int STEP_TRADE = 141; // 0%에서 35 %까지 0.25% 만큼씩 나뉠때의 갯수 35 / 0.25 + 1(0) = 141


        // 수수료 포함해서 계산함
        // 천장선 
        public double[] arrCeiling = new double[STEP_TRADE] {   0,    0.0025, 0.005, 0.0075, 0.01, 0.0125, 0.015, 0.0175, 0.02, 0.0225, 0.025, 0.0275,
                                                                0.03, 0.0325, 0.035, 0.0375, 0.04, 0.0425, 0.045, 0.0475, 0.05, 0.0525, 0.055, 0.0575,
                                                                0.06, 0.0625, 0.065, 0.0675, 0.07, 0.0725, 0.075, 0.0775, 0.08, 0.0825, 0.085, 0.0875,
                                                                0.09, 0.0925, 0.095, 0.0975, 0.1,  0.1025, 0.105, 0.1075, 0.11, 0.1125, 0.115, 0.1175,
                                                                0.12, 0.1225, 0.125, 0.1275, 0.13, 0.1325, 0.135, 0.1375, 0.14, 0.1425, 0.145, 0.1475,
                                                                0.15, 0.1525, 0.155, 0.1575, 0.16, 0.1625, 0.165, 0.1675, 0.17, 0.1725, 0.175, 0.1775,
                                                                0.18, 0.1825, 0.185, 0.1875, 0.19, 0.1925, 0.195, 0.1975, 0.2,  0.2025, 0.205, 0.2075,
                                                                0.21, 0.2125, 0.215, 0.2175, 0.22, 0.2225, 0.225, 0.2275, 0.23, 0.2325, 0.235, 0.2375,
                                                                0.24, 0.2425, 0.245, 0.2475, 0.25, 0.2525, 0.255, 0.2575, 0.26, 0.2625, 0.265, 0.2675,
                                                                0.27, 0.2725, 0.275, 0.2775, 0.28, 0.2825, 0.285, 0.2875, 0.29, 0.2925, 0.295, 0.2975,
                                                                0.3,  0.3025, 0.305, 0.3075, 0.31, 0.3125, 0.315, 0.3175, 0.32, 0.3225, 0.325, 0.3275,
                                                                0.33, 0.3325, 0.335, 0.3375, 0.34, 0.3425, 0.345, 0.3475, 0.35,
                                                            };

        // 기본형 바닥선 
        public double[] arrStepByStepFloor = new double[STEP_TRADE] {   -0.025, -0.02, -0.02 ,-0.0175,-0.01,-0.005,  0.0025,0.005,0.0075, 0.01  ,0.0125, 0.02,
                                                                        0.0225, 0.025, 0.0275, 0.03, 0.0325, 0.035, 0.0375, 0.04, 0.0425, 0.045, 0.0475, 0.05,
                                                                        0.0525, 0.055, 0.0575, 0.06, 0.0625, 0.065, 0.0675, 0.07, 0.0725, 0.075, 0.0775, 0.08,
                                                                        0.0825, 0.085, 0.0875, 0.09, 0.0925, 0.095, 0.0975, 0.1,  0.1025, 0.105, 0.1075, 0.11,
                                                                        0.1125, 0.115, 0.1175, 0.12, 0.1225, 0.125, 0.1275, 0.13, 0.1325, 0.135, 0.1375,0.14,
                                                                        0.1425, 0.145, 0.1475, 0.15, 0.1525, 0.155, 0.1575, 0.16, 0.1625, 0.165, 0.1675, 0.17,
                                                                        0.1725, 0.175, 0.1775, 0.18, 0.1825, 0.185, 0.1875, 0.19, 0.1925, 0.195, 0.1975, 0.2,
                                                                        0.2025, 0.205, 0.2075, 0.21, 0.2125, 0.215, 0.2175, 0.22, 0.2225, 0.225, 0.2275,0.23,
                                                                        0.2325, 0.235, 0.2375, 0.24, 0.2425, 0.245, 0.2475, 0.25, 0.2525, 0.255, 0.2575, 0.26,
                                                                        0.2625, 0.265, 0.2675, 0.27, 0.2725, 0.275, 0.2775, 0.28, 0.2825, 0.285, 0.2875,0.29,
                                                                        0.2925, 0.295, 0.2975, 0.3 , 0.3025, 0.305, 0.3075, 0.31, 0.3125, 0.315, 0.3175,0.32,
                                                                        0.3225, 0.325, 0.3275, 0.33, 0.3325, 0.335, 0.3375, 0.34, 0.3425,
                                                            };

        // 바텀업 바닥선
        public double[] arrBottomUpFloor = new double[STEP_TRADE]  {   -0.025,  -0.02, -0.0175 ,-0.015, 0, 0.005,  0.005, 0.005, 0.0075, 0.01  ,0.0125, 0.02,
                                                                        0.0225, 0.025, 0.0275, 0.03, 0.0325, 0.035, 0.0375, 0.04, 0.0425, 0.045, 0.0475, 0.05,
                                                                        0.0525, 0.055, 0.0575, 0.06, 0.0625, 0.065, 0.0675, 0.07, 0.0725, 0.075, 0.0775, 0.08,
                                                                        0.0825, 0.085, 0.0875, 0.09, 0.0925, 0.095, 0.0975, 0.1,  0.1025, 0.105, 0.1075, 0.11,
                                                                        0.1125, 0.115, 0.1175, 0.12, 0.1225, 0.125, 0.1275, 0.13, 0.1325, 0.135, 0.1375,0.14,
                                                                        0.1425, 0.145, 0.1475, 0.15, 0.1525, 0.155, 0.1575, 0.16, 0.1625, 0.165, 0.1675, 0.17,
                                                                        0.1725, 0.175, 0.1775, 0.18, 0.1825, 0.185, 0.1875, 0.19, 0.1925, 0.195, 0.1975, 0.2,
                                                                        0.2025, 0.205, 0.2075, 0.21, 0.2125, 0.215, 0.2175, 0.22, 0.2225, 0.225, 0.2275,0.23,
                                                                        0.2325, 0.235, 0.2375, 0.24, 0.2425, 0.245, 0.2475, 0.25, 0.2525, 0.255, 0.2575, 0.26,
                                                                        0.2625, 0.265, 0.2675, 0.27, 0.2725, 0.275, 0.2775, 0.28, 0.2825, 0.285, 0.2875,0.29,
                                                                        0.2925, 0.295, 0.2975, 0.3 , 0.3025, 0.305, 0.3075, 0.31, 0.3125, 0.315, 0.3175,0.32,
                                                                        0.3225, 0.325, 0.3275, 0.33, 0.3325, 0.335, 0.3375, 0.34, 0.3425,
                                                            };
        // 스켈핑 바닥선                                                
        public double[] arrScalpingFloor = new double[STEP_TRADE]  {   -0.015, -0.015, -0.01 ,-0.005,     0,     0, 0.0025, 0.005,0.0075,  0.01, 0.0125, 0.02,
                                                                        0.0225, 0.025, 0.0275, 0.03, 0.0325, 0.035, 0.0375, 0.04, 0.0425, 0.045, 0.0475, 0.05,
                                                                        0.0525, 0.055, 0.0575, 0.06, 0.0625, 0.065, 0.0675, 0.07, 0.0725, 0.075, 0.0775, 0.08,
                                                                        0.0825, 0.085, 0.0875, 0.09, 0.0925, 0.095, 0.0975, 0.1,  0.1025, 0.105, 0.1075, 0.11,
                                                                        0.1125, 0.115, 0.1175, 0.12, 0.1225, 0.125, 0.1275, 0.13, 0.1325, 0.135, 0.1375,0.14,
                                                                        0.1425, 0.145, 0.1475, 0.15, 0.1525, 0.155, 0.1575, 0.16, 0.1625, 0.165, 0.1675, 0.17,
                                                                        0.1725, 0.175, 0.1775, 0.18, 0.1825, 0.185, 0.1875, 0.19, 0.1925, 0.195, 0.1975, 0.2,
                                                                        0.2025, 0.205, 0.2075, 0.21, 0.2125, 0.215, 0.2175, 0.22, 0.2225, 0.225, 0.2275,0.23,
                                                                        0.2325, 0.235, 0.2375, 0.24, 0.2425, 0.245, 0.2475, 0.25, 0.2525, 0.255, 0.2575, 0.26,
                                                                        0.2625, 0.265, 0.2675, 0.27, 0.2725, 0.275, 0.2775, 0.28, 0.2825, 0.285, 0.2875,0.29,
                                                                        0.2925, 0.295, 0.2975, 0.3 , 0.3025, 0.305, 0.3075, 0.31, 0.3125, 0.315, 0.3175,0.32,
                                                                        0.3225, 0.325, 0.3275, 0.33, 0.3325, 0.335, 0.3375, 0.34, 0.3425,
                                                            };


        int RaiseStepUp(int nCurLineIdx, int nStep =1)
        {
            return nCurLineIdx +  nStep;
        }

        int PullStepDown(int nCurLineIdx, int nStep = 1)
        {
            return nCurLineIdx - nStep;
        }

        // =========================================
        // 마지막 편집일 : 2023-04-20
        // 1. 다음 윗 계단을 반환
        // =========================================
        double GetNextCeiling(int nCurLineIdx)
        {
            if (nCurLineIdx >= STEP_TRADE)
                return 100; // 닿을 수 없는 천장을 만든다.
            return arrCeiling[nCurLineIdx];
        }

        // =========================================
        // 마지막 편집일 : 2023-04-20
        // 1. 다음 아래 계단을 반환
        // =========================================
        double GetNextFloor(int nIdx, TradeMethodCategory eTradeMethod)
        {
            if (nIdx >= STEP_TRADE)
                return 100; // 닿을 수 없는 바닥을 만든다.(바로 매도가 되게 만든다.

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
                                        int nOrderPrice, int nQty, string sRQName, string sOrgOrderId, string sDescription,
                                        int nBuyedSlotIdx = 0, int nSequence = 0, double fRequestRatio = NORMAL_TRADE_RATIO, int nStrategyIdx = 0,
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
                    BuyedSlot slot = slotByOrderIdDict[sOrgOrderId];
                    buyCancelingByOrderIdDict[sOrgOrderId] = slot.nBuyedSlotId;
                    slot.nDeathTime = nSharedTime; // 매수취소가 일부 혹은 전량 실패해도 매도하면서 nDeathTime이 덮어지니 괜찮다.
                    slot.nDeathPrice = ea[nEaIdx].nFs;
                    slot.sSellDescription = sRQName;
                    break;
                case SELL_CANCEL:
                    sellCancelingByOrderIdDict[sOrgOrderId] = -1;
                    break;
                default:
                    break;
            }

            tradeQueue.Enqueue(curSlot);
        }
    }
}
