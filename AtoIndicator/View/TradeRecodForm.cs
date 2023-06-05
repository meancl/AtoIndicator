using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AtoIndicator.View.EachStockHistory;
using AtoIndicator.View.StatisticResult;

namespace AtoIndicator.View.TradeRecod
{

    public partial class TradeRecodForm : Form
    {
        public string NEW_LINE = Environment.NewLine;
        public MainForm mainForm; // 부모폼의 포인터
        public delegate void VoidDelegator();

        public struct TradeInfo
        {
            public string sGubun; // 장구분
            public string sCode; // 종목코드
            public string sCodeName; // 종목명
            public int nTradingNum; // 현재 매매중인 갯수
            public int nTradeNum; // 매매횟수
            public int nFakeBuyNum; // 페이크매수횟수
            public int nFakeAssistantNum;
            public int nFakeResistNum; // 페이크매수횟수
            public int nPriceUpNum;
            public int nPriceDownNum;
            public bool isTradeStatus; // 현재 매매중
            public double fBuyPrice; // 매수가
            public int nCurFb; // 현재가
            public double fEverageProfit; // 평균이익(백분율)
            public double fTotalPrice; // 총구매가격(백만원 단위)
            public double fPower;
            public double fPowerWithOutGap;

            public string[] GetStringArray()
            {
                return new string[] { sGubun, sCode, sCodeName, nTradingNum.ToString(), nTradeNum.ToString(), nFakeBuyNum.ToString(), nFakeAssistantNum.ToString(), nFakeResistNum.ToString(), nPriceUpNum.ToString(), nPriceDownNum.ToString(), isTradeStatus.ToString(), Math.Round(fBuyPrice, 2).ToString(), nCurFb.ToString(), Math.Round(fEverageProfit, 2).ToString(), Math.Round(fTotalPrice, 2).ToString(), Math.Round(fPower * 100, 2).ToString(), Math.Round(fPowerWithOutGap * 100, 2).ToString() };
            }
        }

        public List<TradeInfo> infoList = new List<TradeInfo>();
        public TradeInfo tmpInfo;

        // 생성자
        public TradeRecodForm(MainForm parentForm)
        { 
           
            InitializeComponent();

            // ================================
            // Windows Settings
            // ================================
            this.DoubleBuffered = true;
            this.KeyUp += KeyUpHandler;
            this.KeyDown += KeyDownHandler;
            this.Text = "거래항목 출력기";
            this.FormClosed += FormClosedHandler;

            // context size에 맞게 column space 설정
            tradeRecordListView.View = System.Windows.Forms.View.Details;
            tradeRecordListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            tradeRecordListView.MouseDoubleClick += RowDoubleClickHandler;
            tradeRecordListView.ColumnClick += ColumnClickHandler;

            statisticToolStripMenuItem.Click += ToolTipItemClickHandler;

            mainForm = parentForm;
            RunThread();
        }

        public bool isCtrlPushed = false;
        public bool isShiftPushed = false; // 강제 true

        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            char cPressed = (char)e.KeyValue;

            if (cPressed == 16)
                isShiftPushed = true;

            if (cPressed == 17)
            {
                isCtrlPushed = true;
            }
        }
        public void FormClosedHandler(Object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
        public void KeyUpHandler(object sender, KeyEventArgs e)
        {
            char cUp = (char)e.KeyValue;

            if (cUp == 'U')
                RunThread();

            if (cUp == 27) // esc
                this.Close();

            if (cUp == 16) // shift
                isShiftPushed = false;

            if (cUp == 17)
                isCtrlPushed = false;

            if (cUp == 'R')
                //StatisticResultForm srForm = new StatisticResultForm(mainForm); // 메인폼을 던져준다.
                //srForm.Show();
                CallThreadStatisticResultForm();


        }


        public int sortColumn = -1;
        public const string UP_TIP = " ▲";
        public const string DOWN_TIP = " ▼";
        public int nTipLen = DOWN_TIP.Length;
        public void ColumnClickHandler(Object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                if (sortColumn != -1) // 처음이 아니라면
                    tradeRecordListView.Columns[sortColumn].Text = tradeRecordListView.Columns[sortColumn].Text.Substring(0, tradeRecordListView.Columns[sortColumn].Text.Length - nTipLen);
                sortColumn = e.Column;
                // Set the sort ord6er to ascending by default.
                tradeRecordListView.Sorting = SortOrder.Descending; // 초기에는 내림차순
                tradeRecordListView.Columns[sortColumn].Text = tradeRecordListView.Columns[sortColumn].Text + DOWN_TIP; // 콜롬명 ▼
            }
            else
            {
                tradeRecordListView.Columns[sortColumn].Text = tradeRecordListView.Columns[sortColumn].Text.Substring(0, tradeRecordListView.Columns[sortColumn].Text.Length - nTipLen);
                // Determine what the last sort order was and change it.
                if (tradeRecordListView.Sorting == SortOrder.Descending)
                {
                    tradeRecordListView.Sorting = SortOrder.Ascending;
                    tradeRecordListView.Columns[sortColumn].Text = tradeRecordListView.Columns[sortColumn].Text + UP_TIP;
                }
                else
                {
                    tradeRecordListView.Sorting = SortOrder.Descending;
                    tradeRecordListView.Columns[sortColumn].Text = tradeRecordListView.Columns[sortColumn].Text + DOWN_TIP;
                }
            }
            // Call the sort method to manually sort.
            this.tradeRecordListView.ListViewItemSorter = new MyListViewComparer(e.Column, tradeRecordListView.Sorting);
            tradeRecordListView.Sort();
            tradeRecordListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }


        // =======================================
        // 마지막 편집일 : 2023-04-20
        // 1. 행을 더블클릭하면 해당 종목의 차트뷰 폼을 콜한다.
        // =======================================
        public void RowDoubleClickHandler(Object sender, EventArgs e)
        {
            if (tradeRecordListView.FocusedItem != null)
                CallThreadEachStockHistoryForm(mainForm.eachStockDict[tradeRecordListView.FocusedItem.SubItems[1].Text.Trim()]);
        }


        public void ToolTipItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

            if (menuItem.Name.Equals("statisticToolStripMenuItem")) // 전략현황기록
            {
                //StatisticResultForm srForm = new StatisticResultForm(mainForm); // 메인폼을 던져준다.
                //srForm.Show();
                CallThreadStatisticResultForm();
            }

        }


        public void RunThread() // delegate로 바꿀 수 있게끔 해보자
        {
            Task.Run(() => ShowTradeRecord());
        }

        public void ShowTradeRecord()
        {
            try
            {
                MainForm.EachStock tmpEa;

                int nTotalVolume;
                double fTotalPrice, fTotalBuyPrice;
                double fFs, fBuyFs;
                const double REAL_STOCK_COMMISSION = MainForm.REAL_STOCK_COMMISSION;
                List<ListViewItem> listViewItems = new List<ListViewItem>();


                int nTodayTotalDisposalBuy = 0;
                int nTodayTotalDisposalSell = 0;


                // 리스트뷰 클리어
                if (tradeRecordListView.InvokeRequired)
                    tradeRecordListView.Invoke(new MethodInvoker(delegate
                    {
                        tradeRecordListView.Items.Clear();
                        tradeRecordListView.BeginUpdate();
                    }));
                else
                {
                    tradeRecordListView.Items.Clear();
                    tradeRecordListView.BeginUpdate();
                }

                if (totalClockLabel.InvokeRequired)
                    totalClockLabel.Invoke(new MethodInvoker(delegate { totalClockLabel.Text = "현재시간 : " + mainForm.nSharedTime; }));
                else
                    totalClockLabel.Text = "현재시간 : " + mainForm.nSharedTime;


                infoList.Clear(); // 리스트뷰 클리어




                // 전 종목을 탐색한다.
                for (int i = 0; i < mainForm.nStockLength; i++)
                {
                    tmpEa = mainForm.ea[i];

                    if (tmpEa.paperBuyStrategy.nStrategyNum == 0) // 매매가 없으면 패스
                        continue;

                    tmpInfo.sGubun = tmpEa.sMarketGubunTag;
                    tmpInfo.sCode = tmpEa.sCode;
                    tmpInfo.sCodeName = tmpEa.sCodeName;
                    tmpInfo.nTradingNum = 0;
                    tmpInfo.nTradeNum = tmpEa.paperBuyStrategy.nStrategyNum;
                    tmpInfo.isTradeStatus = false;
                    tmpInfo.nCurFb = 0;
                    tmpInfo.fEverageProfit = 0;
                    tmpInfo.nFakeBuyNum = tmpEa.fakeBuyStrategy.nStrategyNum;
                    tmpInfo.nFakeAssistantNum = tmpEa.fakeAssistantStrategy.nStrategyNum;
                    tmpInfo.nFakeResistNum = tmpEa.fakeResistStrategy.nStrategyNum;
                    tmpInfo.nPriceUpNum = tmpEa.fakeVolatilityStrategy.nStrategyNum;
                    tmpInfo.nPriceDownNum = tmpEa.fakeVolatilityStrategy.nStrategyNum;
                    tmpInfo.fPower = tmpEa.fPower;
                    tmpInfo.fPowerWithOutGap = tmpEa.fPowerWithoutGap;

                    nTotalVolume = 0; // 갯수 
                    fTotalPrice = 0;  // 매도총가격
                    fTotalBuyPrice = 0; // 구매총가격

                    for (int j = 0; j < tmpEa.paperBuyStrategy.nStrategyNum; j++)
                    {
                        if (tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedVolume == tmpEa.paperBuyStrategy.paperTradeSlot[j].nTargetRqVolume && tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedVolume > 0) // 다 사졌으면
                        {
                            
                            if (tmpEa.paperBuyStrategy.paperTradeSlot[j].isAllSelled) // 다 팔렸던거
                            {
                                fTotalPrice += tmpEa.paperBuyStrategy.paperTradeSlot[j].nSellEndPrice * tmpEa.paperBuyStrategy.paperTradeSlot[j].nSellEndVolume;
                                nTotalVolume += tmpEa.paperBuyStrategy.paperTradeSlot[j].nSellEndVolume;
                                nTodayTotalDisposalSell += tmpEa.paperBuyStrategy.paperTradeSlot[j].nSellEndPrice * tmpEa.paperBuyStrategy.paperTradeSlot[j].nSellEndVolume;
                            }
                            else // 매매중
                            {
                                tmpInfo.nTradingNum++;
                                tmpInfo.isTradeStatus = true;

                                if (tmpEa.paperBuyStrategy.paperTradeSlot[j].isSelling) // 판매중이라면
                                {
                                    fTotalPrice += tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedVolume * ((tmpEa.nFb > 0) ? tmpEa.nFb : tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedPrice);
                                    nTotalVolume += tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedVolume; // 판매된만큼 + 잔량만큼 수량
                                }
                                else // 사기만 함
                                {
                                    fTotalPrice += tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedVolume * ((tmpEa.nFb > 0) ? tmpEa.nFb : tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedPrice);
                                    nTotalVolume += tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedVolume;
                                }
                            }

                            fTotalBuyPrice += tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedPrice * tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedVolume;
                            nTodayTotalDisposalBuy += tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedPrice * tmpEa.paperBuyStrategy.paperTradeSlot[j].nBuyedVolume;
                        }
                    }


                    // ========================================
                    // 1. 거래가 있었다면 리스트 뷰에 추가
                    // 2. 색깔 설정
                    // ========================================
                    if (nTotalVolume > 0)
                    {
                        fFs = fTotalPrice / nTotalVolume;
                        fBuyFs = fTotalBuyPrice / nTotalVolume;

                        tmpInfo.fBuyPrice = fBuyFs;
                        tmpInfo.nCurFb = tmpEa.nFb;
                        tmpInfo.fEverageProfit = ((fFs - fBuyFs) / fBuyFs - REAL_STOCK_COMMISSION) * 100;
                        tmpInfo.fTotalPrice = fTotalBuyPrice / MainForm.MILLION;

                        ListViewItem listViewItem = new ListViewItem(tmpInfo.GetStringArray());

                        if (tmpInfo.fEverageProfit == 0) //  평균 이율
                            listViewItem.BackColor = Color.FromArgb(255, 255, 255);
                        else if (tmpInfo.fEverageProfit > 0)
                        {
                            int nColorStep = (int)(tmpInfo.fEverageProfit / 0.1 * 2);
                            if (nColorStep > 255)
                                nColorStep = 255;
                            listViewItem.BackColor = Color.FromArgb(255, 255 - nColorStep, 255 - nColorStep);
                        }
                        else
                        {
                            int nColorStep = (int)(Math.Abs(tmpInfo.fEverageProfit) / 0.1 * 2);
                            if (nColorStep > 255)
                                nColorStep = 255;
                            listViewItem.BackColor = Color.FromArgb(255 - nColorStep, 255 - nColorStep, 255);
                        }

                        infoList.Add(tmpInfo);

                        listViewItems.Add(listViewItem);

                    }
                } // END ---- for


                if (tradeRecordListView.InvokeRequired)
                    tradeRecordListView.Invoke(new MethodInvoker(delegate
                    {
                        if(listViewItems.Count > 0)
                            tradeRecordListView.Items.AddRange(listViewItems.ToArray());
                        tradeRecordListView.EndUpdate();
                    }));
                else
                {
                    if (listViewItems.Count > 0)
                        tradeRecordListView.Items.AddRange(listViewItems.ToArray());
                    tradeRecordListView.EndUpdate();
                }

                int nTodayProfit = nTodayTotalDisposalSell - nTodayTotalDisposalBuy;
                int nTodayProfitWithRealFee = (int)(nTodayTotalDisposalSell * (1 - MainForm.PAPER_STOCK_COMMISSION) - nTodayTotalDisposalBuy );
                int nTodayProfitWithVirtualFree = (int)(nTodayTotalDisposalSell * (1 - MainForm.VIRTUAL_STOCK_COMMISSION) - nTodayTotalDisposalBuy );

                VoidDelegator tmpDelegator = delegate
                {
                    todayTotalResultTextBox.Clear();
                    
                    todayTotalResultTextBox.AppendText($"=============================== 오늘자 현재 결과 =====================================\r\n");
                    todayTotalResultTextBox.AppendText($" 오늘자 매수금액 : {((double)nTodayTotalDisposalBuy / MainForm.MILLION)}(백만원){NEW_LINE}");
                    todayTotalResultTextBox.AppendText($" 오늘자 매도금액 : {((double)nTodayTotalDisposalSell / MainForm.MILLION)}(백만원){NEW_LINE}");
                    todayTotalResultTextBox.AppendText($" 오늘자 총손익금 : {((double)nTodayProfit / MainForm.MILLION)}(백만원){NEW_LINE}");
                    todayTotalResultTextBox.AppendText($" 오늘자 총손익금(실제 수수료) : {((double)nTodayProfitWithRealFee / MainForm.MILLION)}(백만원){NEW_LINE}");
                    todayTotalResultTextBox.AppendText($" 오늘자 총손익금(가상 수수료) : {((double)nTodayProfitWithVirtualFree / MainForm.MILLION)}(백만원){NEW_LINE}");
                };

                if (todayTotalResultTextBox.InvokeRequired)
                    todayTotalResultTextBox.Invoke(new MethodInvoker(tmpDelegator));
                else
                    tmpDelegator();

            }
            catch (Exception ex)
            {

            }

        }// END ---- ShowTradeRecord


        #region Thread Call Method
        public void CallThreadEachStockHistoryForm(int nCallIdx)
        {
            new Thread(() => new EachStockHistoryForm(mainForm, nCallIdx).ShowDialog()).Start();
        }

        public void CallThreadStatisticResultForm()
        {
            new Thread(() => new StatisticResultForm(mainForm).ShowDialog()).Start();
        }
        #endregion
    }

    // 리스트뷰 비교 인스턴스
    class MyListViewComparer : IComparer
    {
        private int col;
        private SortOrder order;
        public MyListViewComparer()
        {
            col = 0;
            order = SortOrder.Ascending;
        }

        public MyListViewComparer(int column, SortOrder order)
        {
            col = column;
            this.order = order;
        }

        public int Compare(object x, object y)
        {
            int returnVal = -1;

            if (col == 0 || col == 1 || col == 2 || col == 10) // string : 장구분, 종목코드, 종목명, 매매상태
            {
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
            else if (col == 3 || col == 4 || col == 5 || col == 6 || col == 7 || col == 8 || col == 9 || col == 12) // int : 매매중갯수, 매매횟수, 페이크매수횟수, 페이크매도횟수, 현재가
            {
                int nX = int.Parse(((ListViewItem)x).SubItems[col].Text);
                int nY = int.Parse(((ListViewItem)y).SubItems[col].Text);
                if (nX == nY)
                    returnVal = 0;
                else if (nX < nY)
                    returnVal = -1;
                else
                    returnVal = 1;
            }
            else if (col == 11 || col == 13 || col == 14 || col == 15 || col == 16) // double : 매수가, 평균이익(백분율), 총구매가격(백만원)
            {
                double fX = double.Parse(((ListViewItem)x).SubItems[col].Text);
                double fY = double.Parse(((ListViewItem)y).SubItems[col].Text);
                if (fX == fY)
                    returnVal = 0;
                else if (fX < fY)
                    returnVal = -1;
                else
                    returnVal = 1;
            }


            // Determine whether the sort order is descending.
            if (order == SortOrder.Descending)
                // Invert the value returned by String.Compare.
                returnVal *= -1;

            return returnVal;
        }
    }

}
