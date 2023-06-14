using System;
using static AtoIndicator.MainForm;

namespace AtoIndicator.KiwoomLib
{
    internal static class PricingLib
    {
        /// <summary>
        /// Kosdaq현재가 기준 가격틱의 차이를 반환해준다.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static int GetKosdaqGap(int price)
        {
            int gap;
            if (price < 1000)
                gap = 1;
            else if (price < 5000)
                gap = 5;
            else if (price < 10000)
                gap = 10;
            else if (price < 50000)
                gap = 50;
            else
                gap = 100;
            return gap;
        }

        /// <summary>
        /// Kospi현재가 기준 가격틱의 차이를 반환해준다.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static int GetKospiGap(int price)
        {
            int gap;
            if (price < 1000)
                gap = 1;
            else if (price < 5000)
                gap = 5;
            else if (price < 10000)
                gap = 10;
            else if (price < 50000)
                gap = 50;
            else if (price < 100000)
                gap = 100;
            else if (price < 500000)
                gap = 500;
            else
                gap = 1000;

            return gap;
        }

        // 2023 01 25일부터 새로운 호가제도가 시행됨
        public static int GetIntegratedMarketGap(int price)
        {
            int gap;
            if (price < 2000)
                gap = 1;
            else if (price < 5000)
                gap = 5;
            else if (price < 20000)
                gap = 10;
            else if (price < 50000)
                gap = 50;
            else if (price < 200000)
                gap = 100;
            else if (price < 500000)
                gap = 500;
            else
                gap = 1000;
            return gap;
        }


        public static int GetPriceFewSteps(int price, int steps=1)
        {
            int retPrice = price;
            for (int i = 0; i < steps; i++)
                retPrice += GetIntegratedMarketGap(retPrice);

            return retPrice;
        }

        /// <summary>
        /// 마켓을 첫번째인자로 받아 해당마켓의 현재가 기준 가격틱의 차이를 반환해준다.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static int GetAutoGap(int nMarketId, int price)
        {
            //if (DateTime.Compare(DateTime.Today, new DateTime(2023, 1, 25)) < 0)
            //{
            //    if (nMarketId == KOSPI_ID)
            //        return GetKospiGap(price);
            //    else
            //        return GetKosdaqGap(price);
            //}
            //else
            return GetIntegratedMarketGap(price);
        }


        /// <summary>
        /// 가격의 slope갭비율만큼 곱해 반환해준다.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public const double GAP_RATIO = 0.0015;
        public static double GetSlopeGap(int price)
        {
            return price * GAP_RATIO;
        }


    }
}
