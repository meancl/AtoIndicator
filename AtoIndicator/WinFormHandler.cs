using System;
using System.Threading;
using System.Windows.Forms;
using static AtoTrader.TradingBlock.TimeLineGenerator;
using AtoTrader.View.EachStockHistory;
using Microsoft.ML;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtoTrader.View.TextLog;
using System.Diagnostics;
using System.Threading.Tasks;
using AtoTrader.View;

namespace AtoTrader
{
    public partial class MainForm
    {
        public bool isBuyDeny = true;
        // ============================================
        // 버튼 클릭 이벤트의 핸들러 메소드
        // 2. checkChartButton
        // ============================================
        private void CheckChartByButtonHandler(object sender, EventArgs e)
        {
            if (sender.Equals(checkChartButton))
            {
                CheckChartByButton();

            }
        } // End ---- 버튼클릭 핸들러

        public void CheckChartByButton()
        {
            string sCodeTxt = sCodeToBuyTextBox.Text.Trim();
            bool isCorrect;

            try
            {
                nCurIdx = eachStockDict[sCodeTxt];
                isCorrect = true;
            }
            catch
            {
                try
                {
                    nCurIdx = eachStockNameDict[sCodeTxt];
                    isCorrect = true;
                }
                catch
                {
                    isCorrect = false;
                }
            }


            if (isCorrect)
            {
                CallThreadEachStockHistoryForm(nCurIdx);
            }
            else
                MessageBox.Show("입력오류거나 해당종목이 리스트에 없습니다.");

        }


        public bool isCtrlPushed = false;
        public bool isShiftPushed = false;
        public bool isTradeEnd = false;
        public void KeyUpHandler(Object sender, KeyEventArgs e)
        {

            char cUp = (char)e.KeyValue; // 대문자로 준다.

            if (cUp == 191) // 수동작업 on/off (/)
            {
                manualGroupBox.Visible = !manualGroupBox.Visible;
                if (manualGroupBox.Visible)
                {
                    sCodeToBuyTextBox.Clear();
                    this.ActiveControl = this.sCodeToBuyTextBox;
                    PrintLog("수동매수창 출력 완료");
                }
                else
                {
                    PrintLog("수동매수창 은닉 완료");
                }
            }
            
            if (manualGroupBox.Visible)
                return;

            if (cUp == 16)
                isShiftPushed = false;
            if (cUp == 17)
                isCtrlPushed = false;
            if (cUp == 'G')
            {
                ShowConfiguration();
            }
            if (cUp == 'T')
            {
                #region ONNX 실전 TEST
#if AI
                //double test_val = 10;
                //var testData = new double[] {
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val, test_val,
                //    test_val, test_val
                //};
                //var nMMFNum = mmf.RequestAIService(sCode: ea[nCurIdx].sCode, nRqTime: nSharedTime, nRqType: SELL_AI_NUM, inputData: testData);
                //if (nMMFNum == -1)
                //{
                //    PrintLog($"{nSharedTime} AI Service Slot이 부족합니다.");
                //    return;
                //}

                // PutTradeResultAsync();

                //RequestThisRealBuy(-30, isAIUse: true);

                // RequestSellAI(1);
                //var nMMFNum = mmf.RequestAIService(sCode:"005930", nRqTime: 90013, nRqType:0, inputData:testData);
                // mmf.CallEvent();
                //DateTime curTime = DateTime.UtcNow;

                //mmf.FetchTargets();
                //int a;
                //while (true)
                //{
                //    DateTime newTime = DateTime.UtcNow;
                //    if (mmf.checkingComeArray[0])
                //    {
                //        a = 3;
                //        TurnOffMMFSlot(0);
                //        break;
                //    }
                //    mmf.FetchTargets();
                //}
#endif
                #endregion
            }
            if (cUp == 'L')
            {
                CallThreadTextLogForm();
            }
            if (cUp == 'R')
            {
                CallThreadFastInfo();
            }

            if (isShiftPushed && isCtrlPushed)
            {
                if (cUp == 27) // esc
                {
                    StoreLog();
                    this.Close();
                }
            }

            if (cUp == 'W')
            {
                if (isPrintLogBox)
                {
                    PrintLog("화면출력을 종료합니다.");
                    isPrintLogBox = false;
                }
                else
                {
                    isPrintLogBox = true;
                    PrintLog("화면출력을 시작합니다.");
                }
            }

            if (isCtrlPushed)
            {
                if (cUp == 'F') // 강제장시작
                {
                    ForceMarketOn();
                }
            }
        }
        public void KeyDownHandler(Object sender, KeyEventArgs e)
        {
            char cPressed = (char)e.KeyValue; // 대문자로 준다.

            if (manualGroupBox.Visible)
            {
                if(cPressed == 13)
                CheckChartByButton();
                return;
            }


            if (cPressed == 16)
                isShiftPushed = true;
            if (cPressed == 17)
                isCtrlPushed = true;
        }

        
        public void ForceMarketOn()
        {
            isMarketStart = true;
        }

        public void ToolTipItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            
            if (menuItem.Name.Equals("onMarketToolStripMenuItem"))// 강제장시작
            {
                ForceMarketOn();
            }
            else if (menuItem.Name.Equals("onManualToolStripMenuItem"))
            {
                manualGroupBox.Visible = true;
                PrintLog("수동매수창 출력 완료");
            }
            else if (menuItem.Name.Equals("offManualToolStripMenuItem"))
            {
                manualGroupBox.Visible = false;
                PrintLog("수동매수창 은닉 완료");
            }
            else if (menuItem.Name.Equals("realTimeLogStripMenuItem"))
            {
                CallThreadTextLogForm();
            }
            else if (menuItem.Name.Equals("configStripMenuItem"))
            {
                ShowConfiguration();
            }

        }

        public void ShowConfiguration()
        {
            Process curP = Process.GetCurrentProcess();
            new Thread(() => MessageBox.Show($"현재시간 : {nSharedTime}{NEW_LINE}로그인 여부 : {isLoginSucced}{NEW_LINE}화면출력 여부 : {isPrintLogBox}{NEW_LINE}장시작 여부 : {isMarketStart}{NEW_LINE}매수 허용여부 : {!isBuyDeny}{NEW_LINE}임시 장마감 여부 : {isTradeEnd}{NEW_LINE}현재 사용메모리 : {curP.WorkingSet64 / (1024.0 * 1024.0)}(MB){NEW_LINE}현재 가상메모리 : {curP.VirtualMemorySize64 / (1024.0 * 1024.0)}(MB){NEW_LINE}")).Start();
        }

        /// <summary>
        /// eachStockHistoryForm으로 가는 경로
        /// mainForm --> tradeRecordForm --> eachStockHistoryForm 
        ///          --> tradeRecordForm --> statisticResultForm --> eachStrategyForm --> eachStockHistoryForm
        ///          --> eachStockHistoryForm
        /// </summary>
#region Thread Call Method
        public void CallThreadEachStockHistoryForm(int nCallIdx)
        {
            new Thread(() => new EachStockHistoryForm(this, nCallIdx).ShowDialog()).Start();
        }
        public void CallThreadTextLogForm()
        {
            new Thread(() => new TextLogForm(this).ShowDialog()).Start();
        }

        public void CallThreadFastInfo()
        {
            new Thread(() => new FastInfo(this).ShowDialog()).Start();
        }
        #endregion
    }
}
