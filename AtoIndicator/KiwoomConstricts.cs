using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AtoIndicator.KiwoomLib.TimeLib;

namespace AtoIndicator
{
    public partial class MainForm
    {
        /*
         * 제한이 많음
         * 일단 주문제한은 1초에 5회 제한, 초과하면 오류가 뜨진 않지만 -308을 반환하고 주문전송이 안된다
         * 조회제한은 1초에 5회제한, 1시간 1000회 제한
         * 또한 화면번호는 한 프로그램에서 최대 200개까지만 사용가능하며 초과할 시 마지막 TR데이터가 고정된 상태로 오작동한다
         * (오작동하는 상태에서 주문전송TR이면 주문번호가 "000000"로 입력된다. 여튼 200개 내에서 재활용해가며 사용하면 문제없다.
         */

        internal const int SCREEN_NUM_START = 1000;
        internal const int SCREEN_NUM_LIMIT = 190;
        internal const int SCREEN_NUM_PADDING = 10;
        internal const int SCREEN_NUM_LAST = SCREEN_NUM_START + SCREEN_NUM_LIMIT;

        internal const int REACCESSIBLE_SCREEN_TIME = 30;

        internal ScreenStruct[] arrScreen = new ScreenStruct[SCREEN_NUM_LIMIT];

        internal int nUsingScreenNum = 0;

        internal struct ScreenStruct
        {
            public bool isUsing;
            public int nLastScreenTime;
            public BuyedSlot slot;
        }


        private string GetScreenNum()
        {
            string sRet = null;

            try
            {
                // 한번은 random으로 뽑자 
                int nRand = rand.Next(0, SCREEN_NUM_LIMIT);
                if (!arrScreen[nRand].isUsing && (nSharedTime ==0 || SubTimeToTimeAndSec(nSharedTime, arrScreen[nRand].nLastScreenTime) >= REACCESSIBLE_SCREEN_TIME))
                {
                    arrScreen[nRand].isUsing = true;

                    nUsingScreenNum++;
                    screenNumLabel.Text = nUsingScreenNum.ToString();
                    sRet = (nRand + SCREEN_NUM_START).ToString();
                }
                else
                {
                    for (int curScreen = 0; curScreen < SCREEN_NUM_LIMIT; curScreen++)
                    {
                        if (!arrScreen[curScreen].isUsing && (nSharedTime == 0 || SubTimeToTimeAndSec(nSharedTime, arrScreen[nRand].nLastScreenTime) >= REACCESSIBLE_SCREEN_TIME))
                        {
                            arrScreen[curScreen].isUsing = true;

                            nUsingScreenNum++;
                            screenNumLabel.Text = nUsingScreenNum.ToString();
                            sRet = (curScreen + SCREEN_NUM_START).ToString();
                            break;
                        }
                    }
                }   
            }
            catch
            {
                PrintLog($"{nSharedTime} : 화면번호 획득오류가 발생했어..!");
            }
            return sRet;
        }

        private void SetSlotInScreen(string sScrNo, BuyedSlot slot)
        {
            try
            {
                int nScrNoIdx = int.Parse(sScrNo) - SCREEN_NUM_START;
                arrScreen[nScrNoIdx].slot = slot;
            }
            catch
            { }
        }

        private BuyedSlot GetSlotFromScreen(string sScrNo)
        {
            try
            {
                int nScrNoIdx = int.Parse(sScrNo) - SCREEN_NUM_START;
                return arrScreen[nScrNoIdx].slot;
            }
            catch
            {
                return null;
            }
        }

        private void ShutOffScreen(string sScrNo)
        {
            try
            {
                int nScrNoIdx = int.Parse(sScrNo) - SCREEN_NUM_START;
                if (arrScreen[nScrNoIdx].isUsing)
                {
                    nUsingScreenNum--;
                    arrScreen[nScrNoIdx].isUsing = false;
                    arrScreen[nScrNoIdx].slot = null;
                    arrScreen[nScrNoIdx].nLastScreenTime = nSharedTime;

                    if (screenNumLabel.InvokeRequired)
                        screenNumLabel.Invoke(new MethodInvoker(delegate { screenNumLabel.Text = nUsingScreenNum.ToString(); }));
                    else
                        screenNumLabel.Text = nUsingScreenNum.ToString();
                }
            }
            catch
            {
                PrintLog($"{nSharedTime} : 화면번호 처분오류가 발생했어..!  {sScrNo}");
            }
            finally
            {
            }
        }

    }
}
