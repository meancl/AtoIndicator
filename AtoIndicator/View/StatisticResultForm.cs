using AtoIndicator.View.EachStrategy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AtoIndicator.KiwoomLib.TimeLib;
using static AtoIndicator.Utils.Comparer;

namespace AtoIndicator.View.StatisticResult
{
    public partial class StatisticResultForm : Form
    {

        public MainForm mainForm; // 부모폼의 포인터
        public MainForm.Statisticer statisticer;
        public string NEW_LINE = Environment.NewLine;

        // UI 변수들
        public int nLeftRightPadding = 20;
        public int nMiddlePadding = 30;
        public int nCurStrategyMaking = 0; // 현재 몇개까지 만들었나 (임시변수임)

        public int nLineLimitNum = 3; // 한 줄에 몇개의 박스가 들어갈건가

        public int nBoxWidth; // 박스의 너비
        public int nBoxHeight; // 박스의 높이

        public int nCurWidthSequence = 0;
        public int nCurHeightSequence = 50;

        public TextBox[] textBoxArr;
        // END -- UI 변수들

        public double fDenom = 0.0001;
        public MainForm.StrategyNames strategyNames;
        public StatisticResultForm(MainForm parentForm)
        {
            InitializeComponent();
            updateButton.Click += buttonClickHandler;
            mainForm = parentForm;
            strategyNames = mainForm.strategyName;
            statisticer.Init(strategyNames.arrPaperBuyStrategyName.Count);
            nBoxWidth = (this.ClientSize.Width - (nLeftRightPadding * 2 + nMiddlePadding * (nLineLimitNum - 1))) / nLineLimitNum;
            nBoxHeight = nBoxWidth + 50;
            textBoxArr = new TextBox[strategyNames.arrPaperBuyStrategyName.Count];
            for (int _ = 0; _ < strategyNames.arrPaperBuyStrategyName.Count; _++)
                MakeGroupBox();
            RunThread();
            this.ActiveControl = updateButton;
            this.KeyPreview = true;
            this.KeyUp += KeyUpHandler;
            this.FormClosed += FormClosedHandler;
        }
        public void KeyUpHandler(Object sender, KeyEventArgs e)
        {
            char cUp = (char)e.KeyValue;
            if (cUp == 27) // esc
                this.Close();
            if (cUp == 'U')
                RunThread();

        }
        public void FormClosedHandler(Object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        public void RunThread()
        {
            Task.Run(() => CalcAndShowResult());
        }


        /// <summary>
        /// 전략들의 현재상황을 계산하고 폼화면에 출력해준다.
        /// </summary>
        public void CalcAndShowResult()
        {
            MainForm.StrategyHistory curStrategyHistory;
            MainForm.PaperTradeSlot curBuyedSlot;
            MainForm.EachResultTracker curResultTracker;

            int nZeroCount; // 0퍼 초과
            int nOneCount; // 1퍼 이상
            int nTwoCount; // 2퍼 이상
            int nMinusOneCount; // -1퍼 이하
            int nMinusTwoCount; // -2퍼 이하
            int nNoOneBoundaryCount; // -1 ~ +1퍼 사이

            long lEachSumBuyed; // 한 전략에서 총 매수대금
            long lEachStrategyProfit; // 한 전략에서 총 이익금액


            for (int strategyNum = 0; strategyNum < strategyNames.arrPaperBuyStrategyName.Count; strategyNum++) // 전략별로
            {
                // 전략이 돌때마다 초기화해줘야되는 변수들 정리
                statisticer.Clear(); // 일단 비우고
                statisticer.strategyResult[strategyNum].isTradeDataExists = false;
                statisticer.strategyResult[strategyNum].nStrategyNum = strategyNum;
                nZeroCount = nOneCount = nTwoCount = nMinusOneCount = nMinusTwoCount = nNoOneBoundaryCount = 0; // 전략이 돌때마다 0으로 초기화해준다.
                lEachSumBuyed = lEachStrategyProfit = 0; // 전략이 돌때마다 0으로 초기화해준다.
                // END ---- 초기화

                if (mainForm.strategyHistoryList[strategyNum].Count > 0) // 해당전략이 사용됐을경우 (한번이라도)
                {
                    for (int eachPointer = 0; eachPointer < mainForm.strategyHistoryList[strategyNum].Count; eachPointer++) // 해당 전략의 매수데이터들을 순회한다.
                    {
                        curStrategyHistory = mainForm.strategyHistoryList[strategyNum][eachPointer]; // nEaIdx와 nBuyedIdx를 얻었다
                        curBuyedSlot = mainForm.ea[curStrategyHistory.nEaIdx].paperBuyStrategy.paperTradeSlot[curStrategyHistory.nBuyedIdx]; // 해당 BuyedSlot을 얻었다.
                        
                        curResultTracker.nEaIdx = curStrategyHistory.nEaIdx;
                        curResultTracker.nBuyedIdx = curStrategyHistory.nBuyedIdx;


                        if (curBuyedSlot.isAllSelled) // 매매완료라면 
                        {
                            curResultTracker.nTradingHoldingTime = SubTimeToTimeAndSec(curBuyedSlot.nSellEndTime, curBuyedSlot.nBuyEndTime);
                            if (curBuyedSlot.nBuyedVolume == 0) // 전량매수취소됐다면
                            {
                                statisticer.strategyResult[strategyNum].nCanceledNum++;
                                statisticer.strategyResult[strategyNum].nAllTradeNum++;
                            }
                            else // 정상 매매종료
                            {
                                curResultTracker.fProfit = mainForm.GetProfitPercent(curBuyedSlot.nBuyedPrice * curBuyedSlot.nBuyedVolume, curBuyedSlot.nSellEndPrice * curBuyedSlot.nSellEndVolume, mainForm.ea[curStrategyHistory.nEaIdx].nMarketGubun) / 100;
                                lEachSumBuyed += curBuyedSlot.nBuyedVolume * curBuyedSlot.nBuyedPrice;
                                lEachStrategyProfit += (long)(curBuyedSlot.nBuyedVolume * curBuyedSlot.nBuyedPrice * curResultTracker.fProfit);
                                statisticer.strategyResult[strategyNum].nTradedNum++;
                                statisticer.strategyResult[strategyNum].nAllTradeNum++;
                                statisticer.eachTradedTracker.Add(curResultTracker);
                                statisticer.eachTotalTracker.Add(curResultTracker);
                            }
                        }
                        else // 매매중
                        {
                            if (curBuyedSlot.nBuyedVolume == curBuyedSlot.nTargetRqVolume) // 매수가 완료됐다면
                            {
                                curResultTracker.nTradingHoldingTime = 0;
                                curResultTracker.fProfit = curBuyedSlot.fPowerWithFee;
                                lEachSumBuyed += curBuyedSlot.nBuyedVolume * curBuyedSlot.nBuyedPrice;
                                lEachStrategyProfit += (long)(curBuyedSlot.nBuyedVolume * curBuyedSlot.nBuyedPrice * curResultTracker.fProfit);
                                statisticer.strategyResult[strategyNum].nTradingNum++;
                                statisticer.strategyResult[strategyNum].nAllTradeNum++;
                                statisticer.eachTotalTracker.Add(curResultTracker);
                            }
                            else // 매수가 완료되지 않았다면
                            {
                                // 아직 계산에 안넣을거임 
                                statisticer.strategyResult[strategyNum].nAllTradeNum++;
                            }
                        }
                    } // END ---- 데이터들을 돌아간다.

                    if (statisticer.eachTradedTracker.Count > 0) // 매매완료된 트랙커
                    {
                        statisticer.strategyResult[strategyNum].isTradeDataExists = true;
                        statisticer.eachTradedTracker.Sort((x, y) => { return x.fProfit.CompareTo(y.fProfit); });
                        double fSum = 0;
                        double fMinSum = double.MaxValue;
                        double fMaxSum = double.MinValue;
                        double fMedian = 0;
                        // 계산영역
                        for (int eachTracker = 0; eachTracker < statisticer.eachTradedTracker.Count; eachTracker++)
                        {
                            fSum += statisticer.eachTradedTracker[eachTracker].fProfit;
                            fMinSum = Min(fMinSum, statisticer.eachTradedTracker[eachTracker].fProfit);
                            fMaxSum = Max(fMaxSum, statisticer.eachTradedTracker[eachTracker].fProfit);
                        }
                        if (statisticer.eachTradedTracker.Count % 2 == 0) // 짝수갯수라면
                            fMedian = (statisticer.eachTradedTracker[statisticer.eachTradedTracker.Count / 2].fProfit + statisticer.eachTradedTracker[statisticer.eachTradedTracker.Count / 2 - 1].fProfit) / 2;
                        else
                        {
                            int idx = statisticer.eachTradedTracker.Count / 2 - 1;
                            if (idx < 0) // 한개일때는 인덱스가 0번쨰 하나밖에 없으니까
                                idx = 0;
                            fMedian = statisticer.eachTradedTracker[idx].fProfit;
                        }
                        statisticer.strategyResult[strategyNum].traded.everage = fSum / statisticer.eachTradedTracker.Count;
                        statisticer.strategyResult[strategyNum].traded.max = fMaxSum;
                        statisticer.strategyResult[strategyNum].traded.min = fMinSum;
                        statisticer.strategyResult[strategyNum].traded.median = fMedian;
                    } // END ---- 매매완료된 트랙커


                    if (statisticer.eachTotalTracker.Count > 0) // (매매중 + 매매완료)된 트랙커
                    {
                        statisticer.strategyResult[strategyNum].isTradeDataExists = true;
                        statisticer.eachTotalTracker.Sort((x, y) => { return x.fProfit.CompareTo(y.fProfit); });
                        double fSum = 0;
                        double fMinSum = double.MaxValue;
                        double fMaxSum = double.MinValue;
                        double fMedian = 0;

                        // 계산영역
                        for (int eachTracker = 0; eachTracker < statisticer.eachTotalTracker.Count; eachTracker++)
                        {
                            fSum += statisticer.eachTotalTracker[eachTracker].fProfit;
                            fMinSum = Min(fMinSum, statisticer.eachTotalTracker[eachTracker].fProfit);
                            fMaxSum = Max(fMaxSum, statisticer.eachTotalTracker[eachTracker].fProfit);

                            if (statisticer.eachTotalTracker[eachTracker].fProfit > 0)
                                nZeroCount++;

                            if (statisticer.eachTotalTracker[eachTracker].fProfit >= 0.01)
                                nOneCount++;
                            else if (statisticer.eachTotalTracker[eachTracker].fProfit <= -0.01)
                                nMinusOneCount++;
                            else
                                nNoOneBoundaryCount++;

                            if (statisticer.eachTotalTracker[eachTracker].fProfit >= 0.02)
                                nTwoCount++;
                            else if (statisticer.eachTotalTracker[eachTracker].fProfit <= -0.02)
                                nMinusTwoCount++;

                        }
                        if (statisticer.eachTotalTracker.Count % 2 == 0) // 짝수갯수라면
                            fMedian = (statisticer.eachTotalTracker[statisticer.eachTotalTracker.Count / 2].fProfit + statisticer.eachTotalTracker[statisticer.eachTotalTracker.Count / 2 - 1].fProfit) / 2;
                        else
                        {
                            int idx = statisticer.eachTotalTracker.Count / 2 - 1;
                            if (idx < 0) // 한개일때는 인덱스가 0번쨰 하나밖에 없으니까
                                idx = 0;
                            fMedian = statisticer.eachTotalTracker[idx].fProfit;
                        }


                        statisticer.strategyResult[strategyNum].total.everage = fSum / statisticer.eachTotalTracker.Count;
                        statisticer.strategyResult[strategyNum].total.max = fMaxSum;
                        statisticer.strategyResult[strategyNum].total.min = fMinSum;
                        statisticer.strategyResult[strategyNum].total.median = fMedian;

                    } // END ---- (매매중 + 매매완료)된 트랙커
                } // END ---- 해당전략의 데이터가 하나라도 있다면

                string sMessage = $"##전략명 : {strategyNames.arrPaperBuyStrategyName[strategyNum]} ##{NEW_LINE}{NEW_LINE}";

                if (statisticer.strategyResult[strategyNum].isTradeDataExists)
                {
                    sMessage +=
                        $"매매중 갯수 : {statisticer.strategyResult[strategyNum].nTradingNum}{NEW_LINE}" +
                        $"매매완료 갯수 : {statisticer.strategyResult[strategyNum].nTradedNum}{NEW_LINE}" +
                        $"매수취소 갯수 : {statisticer.strategyResult[strategyNum].nCanceledNum}{NEW_LINE}" +
                        $"전체요청 갯수 : {statisticer.strategyResult[strategyNum].nAllTradeNum}{NEW_LINE}{NEW_LINE}" +
                        $"---- 매매완료 + 매매중 -------{NEW_LINE}" +
                        $"총저점 : {Math.Round(statisticer.strategyResult[strategyNum].total.min * 100, 2)}(%){NEW_LINE}" +
                        $"총고점 : {Math.Round(statisticer.strategyResult[strategyNum].total.max * 100, 2)}(%){NEW_LINE}" +
                        $"총평균 : {Math.Round(statisticer.strategyResult[strategyNum].total.everage * 100, 2)}(%){NEW_LINE}" +
                        $"총중위 : {Math.Round(statisticer.strategyResult[strategyNum].total.median * 100, 2)}(%){NEW_LINE}" +
                        $"투입금액 : {Math.Round((double)lEachSumBuyed / MainForm.MILLION, 3)}(백만원){NEW_LINE}" +
                        $"이율(백만원) : {Math.Round((double)lEachStrategyProfit/ MainForm.STANDARD_BUY_PRICE * 100, 2)}(%){NEW_LINE}" +
                        $"실이득 : {Math.Round((double)lEachStrategyProfit / MainForm.TEN_THOUSAND, 2)}(만원){NEW_LINE}{NEW_LINE}" +
                        $"==> 0%초과 확률 : {Math.Round((double)nZeroCount / statisticer.eachTotalTracker.Count * 100, 2)}(%){NEW_LINE}" +
                        $"==> +1%이상 확률 : {Math.Round((double)nOneCount / statisticer.eachTotalTracker.Count * 100, 2)}(%){NEW_LINE}" +
                        $"==> -1%이하 확률 : {Math.Round((double)nMinusOneCount / statisticer.eachTotalTracker.Count * 100, 2)}(%){NEW_LINE}" +
                        $"==> |1|%미만 확률 : {Math.Round((double)nNoOneBoundaryCount / statisticer.eachTotalTracker.Count * 100, 2)}(%){NEW_LINE}" +
                        $"==> +2%이상 확률 : {Math.Round((double)nTwoCount / statisticer.eachTotalTracker.Count * 100, 2)}(%){NEW_LINE}" +
                        $"==> -2%이하 확률 : {Math.Round((double)nMinusTwoCount / statisticer.eachTotalTracker.Count * 100, 2)}(%)";

                }
                else
                {
                    sMessage += "아직 전략이 활성화되지 않았습니다";
                }

                if (textBoxArr[strategyNum].InvokeRequired)
                {
                    textBoxArr[strategyNum].Invoke(new MethodInvoker(() =>
                    {

                        textBoxArr[strategyNum].Text = sMessage;
                        if (statisticer.strategyResult[strategyNum].isTradeDataExists)
                        {
                            if ((statisticer.strategyResult[strategyNum].nTradedNum + statisticer.strategyResult[strategyNum].nTradingNum) >= 10)
                                textBoxArr[strategyNum].Font = new Font(textBoxArr[strategyNum].Font, FontStyle.Bold);

                            if (isEqualBetweenDouble(statisticer.strategyResult[strategyNum].total.everage, 0))
                                textBoxArr[strategyNum].BackColor = Color.FromArgb(255, 255, 255);
                            else if (statisticer.strategyResult[strategyNum].total.everage > 0)
                            {
                                int nColorStep = (int)(statisticer.strategyResult[strategyNum].total.everage / fDenom * 2);
                                if (nColorStep > 255)
                                    nColorStep = 255;
                                textBoxArr[strategyNum].BackColor = Color.FromArgb(255, 255 - nColorStep, 255 - nColorStep);
                            }
                            else
                            {
                                int nColorStep = (int)(Math.Abs(statisticer.strategyResult[strategyNum].total.everage) / fDenom * 2);
                                if (nColorStep > 255)
                                    nColorStep = 255;
                                textBoxArr[strategyNum].BackColor = Color.FromArgb(255 - nColorStep, 255 - nColorStep, 255);
                            }
                        }
                    }));
                }
                else
                {
                    textBoxArr[strategyNum].Text = sMessage;
                    if (statisticer.strategyResult[strategyNum].isTradeDataExists)
                    {
                        if ((statisticer.strategyResult[strategyNum].nTradedNum + statisticer.strategyResult[strategyNum].nTradingNum) >= 10)
                            textBoxArr[strategyNum].Font = new Font(textBoxArr[strategyNum].Font, FontStyle.Bold);

                        if (isEqualBetweenDouble(statisticer.strategyResult[strategyNum].total.everage, 0))
                            textBoxArr[strategyNum].BackColor = Color.FromArgb(255, 255, 255);
                        else if (statisticer.strategyResult[strategyNum].total.everage > 0)
                        {
                            int nColorStep = (int)(statisticer.strategyResult[strategyNum].total.everage / fDenom * 2);
                            if (nColorStep > 255)
                                nColorStep = 255;
                            textBoxArr[strategyNum].BackColor = Color.FromArgb(255, 255 - nColorStep, 255 - nColorStep);
                        }
                        else
                        {
                            int nColorStep = (int)(Math.Abs(statisticer.strategyResult[strategyNum].total.everage) / fDenom * 2);
                            if (nColorStep > 255)
                                nColorStep = 255;
                            textBoxArr[strategyNum].BackColor = Color.FromArgb(255 - nColorStep, 255 - nColorStep, 255);
                        }
                    }
                }
            } // END ---- 전략별로

            int nSharedTime = mainForm.nSharedTime;
            string sSharedTime = nSharedTime.ToString();

            if (sharedTimeLabel.InvokeRequired)
            {
                sharedTimeLabel.Invoke(new MethodInvoker(delegate { sharedTimeLabel.Text = "현재시간 : " + sSharedTime; }));
            }
            else
            {
                sharedTimeLabel.Text = "현재시간 : " + sSharedTime;
            }
        } // END ---- CalcAndShowResult

        /// <summary>
        /// 전략갯수만큼의 GroupBox를 세팅해준다.
        /// </summary>
        public void MakeGroupBox()
        {
            if (nCurStrategyMaking >= strategyNames.arrPaperBuyStrategyName.Count) // 예비용
                return;

            GroupBox newGroupBox = new GroupBox();
            textBoxArr[nCurStrategyMaking] = new TextBox
            {
                Name = $"{nCurStrategyMaking}", // 더블클릭됐을때 어떤 전략인지 알 수 있도록
                BackColor = System.Drawing.SystemColors.ControlLight,
                Dock = System.Windows.Forms.DockStyle.Fill,
                Visible = true,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = System.Windows.Forms.ScrollBars.Vertical,
                Text = "아직 전략이 활성화되지 않았습니다"
            };
            textBoxArr[nCurStrategyMaking].DoubleClick += TextBoxDoubleClickHandler;

            int nXPoint;
            if (nCurWidthSequence == 0)
                nXPoint = nLeftRightPadding;
            else
                nXPoint = nLeftRightPadding + nCurWidthSequence * (nBoxWidth + nMiddlePadding);
            newGroupBox.Location = new System.Drawing.Point(nXPoint, nCurHeightSequence);
            newGroupBox.Name = nCurStrategyMaking.ToString();
            newGroupBox.Size = new System.Drawing.Size(nBoxWidth, nBoxHeight);
            newGroupBox.TabIndex = nCurStrategyMaking;
            newGroupBox.TabStop = false;
            newGroupBox.Text = $"전략 {nCurStrategyMaking}번";

            nCurWidthSequence++;
            if (nCurWidthSequence >= nLineLimitNum)
            {
                nCurWidthSequence = 0;
                nCurHeightSequence += nBoxHeight + nMiddlePadding;
            }

            this.Controls.Add(newGroupBox);
            newGroupBox.Controls.Add(textBoxArr[nCurStrategyMaking++]);

        } // END ---- MakeGroupBox

        public void buttonClickHandler(object sender, EventArgs e)
        {
            if (sender.Equals(updateButton))
            {
                // TODO
                RunThread();
            }
        }

        public void TextBoxDoubleClickHandler(object sender, EventArgs e)
        {
            var clicked = ((TextBox)sender).Name; // 눌렸다면
            var clickedStrategy = int.Parse(clicked); // 눌린 전략을 구한다.
            // 해당 전략의 현재상황을 가져오고 폼에 뿌려준다.
            var strategyList = mainForm.strategyHistoryList[clickedStrategy]; // 해당 전략의 리스트
            MainForm.StrategyNames strategyName = mainForm.strategyName;
            if (strategyList.Count <= 0 ) // 해당전략이 사용되지 않았다면
            {
                MessageBox.Show($"{clickedStrategy}번 : {strategyName.arrPaperBuyStrategyName[clickedStrategy]} 전략은 사용되지 않았습니다.");
            }
            else // 사용됐다면 
            {
                // 폼을 던져준다.
                //EachStrategyForm esForm = new EachStrategyForm(mainForm, clickedStrategy); // 메인폼을 던져준다.
                //esForm.Show();
                CallThreadEachStrategyForm(clickedStrategy);
            }
        }

        #region Thread Call Method
        public void CallThreadEachStrategyForm(int nCallStrategy)
        {
            try
            {
                new Thread(() => new EachStrategyForm(mainForm, nCallStrategy).ShowDialog()).Start();
            }
            catch { }
        }
        #endregion
    }
}
