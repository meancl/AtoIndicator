using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtoIndicator.Utils
{
    internal static class Comparer
    {

        /// <summary>
        /// 부동소수점의 동일비교연산자
        /// 부동소수점은 직관적으로는 같아보여도 다른 수라고 판명내릴 수 있다
        /// ex> 3.14 % 3 == 0.14  (false)
        /// </summary>
        public static double EPSILON = 0.000001;
        public static bool isEqualBetweenDouble(double fA, double fB)
        {
            double fDiff = Math.Abs(fB - fA);
            return fDiff < EPSILON;
        }



        // a < b => a
        // else => b
        public static int Min(int a, int b)
        {
            int retVal;
            if (a < b)
                retVal = a;
            else
                retVal = b;
            return retVal;
        }

        // a < b => a
        // else => b
        public static double Min(double a, double b)
        {
            double retVal;
            if (a < b)
                retVal = a;
            else
                retVal = b;
            return retVal;
        }

        // b < a => a
        // else => b
        public static int Max(int a, int b)
        {
            int retVal;
            if (a > b)
                retVal = a;
            else
                retVal = b;
            return retVal;
        }

        // b < a => a
        // else => b
        public static double Max(double a, double b)
        {
            double retVal;
            if (a > b)
                retVal = a;
            else
                retVal = b;
            return retVal;
        }

        // 가변길이 매개변수용 double Min
        public static double MinParams(params double[] itemList)
        {
            double retVal = double.MaxValue;

            foreach (double item in itemList)
            {
                if (retVal > item)
                {
                    retVal = item;
                }
            }
            return retVal;
        }

        // 가변길이 매개변수용 int Min
        public static int MinParams(params int[] itemList)
        {
            int retVal = int.MaxValue;

            foreach (int item in itemList)
            {
                if (retVal > item)
                {
                    retVal = item;
                }
            }
            return retVal;
        }

        // 가변길이 매개변수용 double Max
        public static double MaxParams(params double[] itemList)
        {
            double retVal = double.MinValue;

            foreach (double item in itemList)
            {
                if (retVal < item)
                {
                    retVal = item;
                }
            }
            return retVal;
        }

        // 가변길이 매개변수용 int Max
        public static int MaxParams(params int[] itemList)
        {
            int retVal = int.MinValue;

            foreach (int item in itemList)
            {
                if (retVal < item)
                {
                    retVal = item;
                }
            }
            return retVal;
        }

        // min보다 작으면 min값을
        // max보다 크면 max값을
        // 그 사이라면 myVal를
        public static int GetBetweenMinAndMax(int myVal, int minVal, int maxVal)
        {
            int retVal;

            if (myVal < minVal)
                retVal = minVal;
            else if (myVal > maxVal)
                retVal = maxVal;
            else
                retVal = myVal;

            return retVal;
        }




        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

    }
}
