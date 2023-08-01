using System;
using System.Threading;
using System.Windows.Forms;
using static AtoIndicator.TradingBlock.TimeLineGenerator;
using AtoIndicator.View.TradeRecod;
using AtoIndicator.View.EachStockHistory;
using Microsoft.ML;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtoIndicator.View.TextLog;
using System.Diagnostics;
using System.Threading.Tasks;
using AtoIndicator.View;
using AtoIndicator.DB;
using Microsoft.EntityFrameworkCore;

namespace AtoIndicator
{
    public partial class MainForm
    {
        public bool isBuyDeny = true;

        // ============================================
        // 버튼 클릭 이벤트의 핸들러 메소드
        // 1. buyButton
        // 2. checkChartButton
        // ============================================
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender.Equals(checkChartButton))
                ViewManualEachStock();
        } // End ---- 버튼클릭 핸들러


        public void ViewManualEachStock()
        {
            string sCodeTxt = sCodeToBuyTextBox.Text.Trim();
            bool isCorrect = false;
            int nCurIdx = -1;

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
                this.ActiveControl = sCodeToBuyTextBox;
                if (manualGroupBox.Visible)
                {
                    sCodeToBuyTextBox.Clear();
                    PrintLog("수동매수창 출력 완료");
                }
                else
                {
                    PrintLog("수동매수창 은닉 완료");
                }
            }



            if (!manualGroupBox.Visible) // 수동매수창 은닉상태
            {
                if (cUp == 16)
                    isShiftPushed = false;
                if (cUp == 17)
                    isCtrlPushed = false;
                if (cUp == 'G')
                {
                    ShowConfiguration();
                }
                if (cUp == 'D') // 예수금확인
                {
                    RequestDeposit();
                }
                if (cUp == 'H') // 보유종목확인
                {
                    RequestHoldings(0);
                }
                if (cUp == 'C')
                {
                    for(int i = 0; i < nStockLength; i++)
                    {
                        ea[i].myTradeManager.isEachStockHistoryExist = false;
                    }
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
                    //var nMMFNum = mmf.RequestAIService(sCode: ea[0].sCode, nRqTime: nSharedTime, nRqType: EVERY_SIGNAL, inputData: testData);
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
                if (cUp == 'R') // 현황기록
                {
                    //TradeRecodForm tradeRecordForm = new TradeRecodForm(this);
                    //tradeRecordForm.Show();
                    CallThreadTradeRecordForm();
                }
                if (cUp == 'L')
                {
                    CallThreadTextLogForm();
                }
                if (cUp == 'I')
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
                        if (isShiftPushed)
                            ForceMarketOn(false);
                        else
                            ForceMarketOn();
                    }
                    if (cUp == 'S') // 강제장종료
                    {
                        if (isShiftPushed)
                            ForceMarketOff(true);
                        else
                            ForceMarketOff();

                    }

                }
            }
            else // 수동매수창 출력상태
            {
                if(cUp == 13) // enter
                    ViewManualEachStock();
            }
        }
        public void KeyDownHandler(Object sender, KeyEventArgs e)
        {
            char cPressed = (char)e.KeyValue; // 대문자로 준다.
            
            if (manualGroupBox.Visible)
                return;


            if (cPressed == 16)
                isShiftPushed = true;
            if (cPressed == 17)
                isCtrlPushed = true;
        }

        public void ForceMarketOff(bool isComplete = false)
        {
            isTradeEnd = true;
            for (int i = 0; i < nStockLength; i++)
                ea[i].isExcluded = true;
            isBuyDeny = true;

            if (isComplete)
            {
                isMarketStart = false;
                isMarketLabel.Text = $"장시작 : {isMarketStart}";
                PrintLog("강제 장종료 완료");
            }
            else
                PrintLog("임시 장종료 완료");
        }

        public void ForceMarketOn(bool isBuyDenied = true)
        {

            if (!isBuyDeny) // 매수 가능상태인데
            {
                if (isBuyDenied) // 매수 금지 장시작을 하네?
                    PrintLog($"매수금지 임시 장마감을 대신 사용하세요"); // 장시작인데 매수 금지를 여기서 할 순 없지
                else
                    PrintLog($"이미 매수 허용 상태입니다"); // 매수 가능상태인데 매수 허용을 한 경우
            }
            else // 매수 금지 상태인데
            {
                isBuyDeny = isBuyDenied;
                for (int i = 0; i < nStockLength; i++)
                    ea[i].isExcluded = isBuyDenied;

                if (isBuyDenied) // 매수 금지 장시작을 한다면?
                {
                    PrintLog($"이미 매수 금지 상태입니다");
                }
                else
                {
                    PrintLog($"매수 허용 성공했습니다");
                }
            }

            if (!isMarketStart) // 장끝나고 강제시작하면 안켜짐.
            {
                if (nSharedTime > REAL_DATA_END_TIME) // MARKET_END_TIME 이후로 설정하면 마켓이후의 체결데이터로 인해 무쓸모한 차트데이터가 더해지기 때문에 시각화에 방해가 된다.
                {
                    PrintLog("장마감시간 이후라 강제 장시작 불가");
                }
                else
                {
                    PrintLog("강제 장시작 완료");
                    if (nFirstTime == 0) // 첫 시간이 설정되지 않았다면 
                    {
                        nFirstTime = nSharedTime - nSharedTime % MINUTE_KIWOOM; // x시간 00분 00 초 형태로 만든다
                        BlockizeUndisposal();
                    }
                    isMarketStart = true;
                    isMarketLabel.Text = $"장시작 : {isMarketStart}";
                    if (nPrevBoardUpdateTime == 0) // 이전업데이트 시간이 초기화되지 않았다면 
                    {
                        nPrevBoardUpdateTime = nFirstTime; // 장초반시간으로 업데이트한다.
                    }
                }
            }

            isTradeEnd = false;
        }
        public int nSellMethodSetting;

        public void ToolTipItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

            if (menuItem.Name.Equals("depositToolStripMenuItem")) // 예수금상세현황요청
            {
                RequestDeposit();
            }
            else if (menuItem.Name.Equals("holdingsToolStripMenuItem")) // 계좌평가현황요청 
            {
                RequestHoldings(0);
            }
            else if (menuItem.Name.Equals("onMarketToolStripMenuItem"))// 강제장시작
            {
                ForceMarketOn();
            }
            else if (menuItem.Name.Equals("onMarketWithBuyAccToolStripMenuItem"))// 강제장시작 with 매수 가능
            {
                ForceMarketOn(false);
            }
            else if (menuItem.Name.Equals("offMarketToolStripMenuItem")) //  임시 장종료
            {
                ForceMarketOff();
            }
            else if (menuItem.Name.Equals("curRecordToolStripMenuItem"))
            {
                CallThreadTradeRecordForm();
            }
            else if (menuItem.Name.Equals("indicatorToolStripMenuItem"))
            {
                CallThreadFastInfo();
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
            else if (menuItem.Name.Equals("risingToolStripMenuItem"))
            {
                PrintLog("디폴트 매도방식을 Rising으로 변경합니다.");
                for (int i = 0; i < nStockLength; i++)
                    ea[i].myTradeManager.eDefaultTradeCategory = TradeMethodCategory.RisingMethod;
            }
            else if (menuItem.Name.Equals("bottomUpToolStripMenuItem"))
            {
                PrintLog("디폴트 매도방식을 BottomUp으로 변경합니다.");
                for (int i = 0; i < nStockLength; i++)
                    ea[i].myTradeManager.eDefaultTradeCategory = TradeMethodCategory.BottomUpMethod;
            }
            else if (menuItem.Name.Equals("scalpingToolStripMenuItem"))
            {
                PrintLog("디폴트 매도방식을 Scalping으로 변경합니다.");
                for (int i = 0; i < nStockLength; i++)
                    ea[i].myTradeManager.eDefaultTradeCategory = TradeMethodCategory.ScalpingMethod;
            }
            else if (menuItem.Name.Equals("fixedToolStripMenuItem"))
            {
                PrintLog("디폴트 매도방식을 Fixed으로 변경합니다.");
                for (int i = 0; i < nStockLength; i++)
                    ea[i].myTradeManager.eDefaultTradeCategory = TradeMethodCategory.FixedMethod;
            }
            else if (menuItem.Name.Equals("noneToolStripMenuItem"))
            {
                PrintLog("디폴트 매도방식을 None으로 변경합니다.");
                for (int i = 0; i < nStockLength; i++)
                    ea[i].myTradeManager.eDefaultTradeCategory = TradeMethodCategory.None;
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
        public void CallThreadTradeRecordForm()
        {
            try
            {
                new Thread(() => new TradeRecodForm(this).ShowDialog()).Start();
            }
            catch
            { }
        }
        public void CallThreadEachStockHistoryForm(int nCallIdx)
        {
            try
            {
                if ((DateTime.UtcNow - ea[nCallIdx].myTradeManager.dLatestApproachTime).TotalSeconds >= 1 && !ea[nCallIdx].myTradeManager.isEachStockHistoryExist)
                {
                    ea[nCallIdx].myTradeManager.dLatestApproachTime = DateTime.UtcNow;
                    ea[nCallIdx].myTradeManager.isEachStockHistoryExist = true;
                    new Thread(() => new EachStockHistoryForm(this, nCallIdx).ShowDialog()).Start();
                }
            }
            catch { }
        }
        public void CallThreadTextLogForm()
        {
            try
            {
                new Thread(() => new TextLogForm(this).ShowDialog()).Start();
            }
            catch { }
        }

        public void CallThreadFastInfo()
        {
            try
            {
                new Thread(() => new FastInfo(this).ShowDialog()).Start();
            }
            catch
            {

            }
        }
        #endregion
    }
}
