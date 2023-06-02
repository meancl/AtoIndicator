using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        internal const int SCREEN_NUM_LIMIT = 200;
        internal const int SCREEN_NUM_PADDING = 10;
        internal const int SCREEN_NUM_LAST = SCREEN_NUM_START + SCREEN_NUM_LIMIT;

        internal ScreenStruct[] arrScreen = new ScreenStruct[SCREEN_NUM_LIMIT];

        internal int nUsingScreenNum = 0;

        internal struct ScreenStruct
        {
            public bool isUsing;
            public int? nEaIdx;
            public int? nSlotIdx;
            public string sPurposeToUse;
        }


        private string GetScreenNum(string sPurpose=null, int? nEaIdx=null, int? nSlotIdx=null)
        {
            string sRet = null;

            try
            {
                // 한번은 random으로 뽑자 
                int nRand = rand.Next(0, SCREEN_NUM_LIMIT);
                if (!arrScreen[nRand].isUsing)
                {
                    arrScreen[nRand].isUsing = true;
                    arrScreen[nRand].nEaIdx = nEaIdx;
                    arrScreen[nRand].nSlotIdx = nSlotIdx;
                    arrScreen[nRand].sPurposeToUse = sPurpose;

                    nUsingScreenNum++;
                    screenNumLabel.Text = nUsingScreenNum.ToString();
                    sRet = (nRand + SCREEN_NUM_START).ToString();
                }
                else
                {
                    for (int curScreen = 0; curScreen < SCREEN_NUM_LIMIT; curScreen++)
                    {
                        if (!arrScreen[curScreen].isUsing)
                        {
                            arrScreen[curScreen].isUsing = true;
                            arrScreen[curScreen].nEaIdx = nEaIdx;
                            arrScreen[curScreen].nSlotIdx = nSlotIdx;
                            arrScreen[curScreen].sPurposeToUse = sPurpose;

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


        private void ShutOffScreen(ref string sScrNo)
        {
            try
            {
                int nScrNoIdx = int.Parse(sScrNo) - SCREEN_NUM_START;
                if (arrScreen[nScrNoIdx].isUsing)
                {
                    nUsingScreenNum--;
                    arrScreen[nScrNoIdx].isUsing = false;
                    arrScreen[nScrNoIdx].nEaIdx = null;
                    arrScreen[nScrNoIdx].nSlotIdx = null;
                    arrScreen[nScrNoIdx].sPurposeToUse = null;

                    if(screenNumLabel.InvokeRequired)
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
                sScrNo = null;
            }
        }

        private void ReceiveScreenNoIdx(string sScrNo, out int? nEaIdx, out int? nSlotIdx, out string sPurpose)
        {
            int? nTmpEaIdx = null;
            int? nTmpSlotIdx = null;
            string sPurposeToUse = null;
            try
            {
                int nScrNoIdx = int.Parse(sScrNo) - SCREEN_NUM_START;

                if (arrScreen[nScrNoIdx].isUsing)
                {
                    nTmpEaIdx = arrScreen[nScrNoIdx].nEaIdx;
                    nTmpSlotIdx = arrScreen[nScrNoIdx].nSlotIdx;
                    sPurposeToUse = arrScreen[nScrNoIdx].sPurposeToUse;
                }
                else
                    throw new Exception();
            }
            catch
            {
                PrintLog($"{nSharedTime} ReceiveScreenNoIdx 에러 : {sScrNo}");
            }
            finally
            {
                nEaIdx = nTmpEaIdx;
                nSlotIdx = nTmpSlotIdx;
                sPurpose = sPurposeToUse;
            }
        }
    }
}
