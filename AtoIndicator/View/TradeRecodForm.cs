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

        public ListView curListView;
        // 생성자
        public TradeRecodForm(MainForm parentForm)
        {

            InitializeComponent();


            string sString = "STRING";
            string sInt = "INT";
            string sDouble = "DOUBLE";

            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sString, Text = "장구분" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sString, Text = "종목코드" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sString, Text = "종목명" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "매매중" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "실매매" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가짜매수" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가짜보조" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가짜저항" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가격업" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가격다운" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sString, Text = "매매상태" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "매수가" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "현재가" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "평균이익(백분율)" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "총구매가격(백만원)" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "당일기세" });
            tradeRecordListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "갭제외" });
            // context size에 맞게 column space 설정
            tradeRecordListView.View = System.Windows.Forms.View.Details;
            tradeRecordListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            tradeRecordListView.MouseDoubleClick += RowDoubleClickHandler;
            tradeRecordListView.ColumnClick += ColumnClickHandler;

            realTradeListView.Columns.Add(new ColumnHeader { Name = sString, Text = "장구분" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sString, Text = "종목코드" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sString, Text = "종목명" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "매매중" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "실매매" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가짜매수" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가짜보조" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가짜저항" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가격업" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "가격다운" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sString, Text = "매매상태" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "매수가" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "현재가" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "평균이익(백분율)" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "총구매가격(백만원)" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "당일기세" });
            realTradeListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "갭제외" });
            // context size에 맞게 column space 설정
            realTradeListView.View = System.Windows.Forms.View.Details;
            realTradeListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            realTradeListView.MouseDoubleClick += RowDoubleClickHandler;
            realTradeListView.ColumnClick += ColumnClickHandler;

            curListView = tradeRecordListView;
            // ================================
            // Windows Settings
            // ================================
            this.DoubleBuffered = true;
            this.KeyUp += KeyUpHandler;
            this.KeyDown += KeyDownHandler;
            this.Text = "거래항목 출력기";
            this.FormClosed += FormClosedHandler;

            tabControl1.SelectedIndexChanged += SelectedIndexChangedHandler;
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
                    curListView.Columns[sortColumn].Text = curListView.Columns[sortColumn].Text.Substring(0, curListView.Columns[sortColumn].Text.Length - nTipLen);
                sortColumn = e.Column;
                // Set the sort ord6er to ascending by default.
                curListView.Sorting = SortOrder.Descending; // 초기에는 내림차순
                curListView.Columns[sortColumn].Text = curListView.Columns[sortColumn].Text + DOWN_TIP; // 콜롬명 ▼
            }
            else
            {
                curListView.Columns[sortColumn].Text = curListView.Columns[sortColumn].Text.Substring(0, curListView.Columns[sortColumn].Text.Length - nTipLen);
                // Determine what the last sort order was and change it.
                if (curListView.Sorting == SortOrder.Descending)
                {
                    curListView.Sorting = SortOrder.Ascending;
                    curListView.Columns[sortColumn].Text = curListView.Columns[sortColumn].Text + UP_TIP;
                }
                else
                {
                    curListView.Sorting = SortOrder.Descending;
                    curListView.Columns[sortColumn].Text = curListView.Columns[sortColumn].Text + DOWN_TIP;
                }
            }
            // Call the sort method to manually sort.
            this.curListView.ListViewItemSorter = new MyListViewComparer(curListView.Columns[e.Column].Name, e.Column, curListView.Sorting);
            curListView.Sort();
            curListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }


        // =======================================
        // 마지막 편집일 : 2023-04-20
        // 1. 행을 더블클릭하면 해당 종목의 차트뷰 폼을 콜한다.
        // =======================================
        public void RowDoubleClickHandler(Object sender, EventArgs e)
        {
            if (curListView.FocusedItem != null)
                CallThreadEachStockHistoryForm(mainForm.eachStockDict[curListView.FocusedItem.SubItems[1].Text.Trim()]);
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
        public void SelectedIndexChangedHandler(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                curListView = tradeRecordListView;
            else
                curListView = realTradeListView;

            ShowTradeRecord();
        }

        public void RunThread() // delegate로 바꿀 수 있게끔 해보자
        {
            Task.Run(() => ShowTradeRecord());
        }

        public void ShowTradeRecord()
        {
            void ShowRecords()
            {
                try
                {
                    MainForm.EachStock tmpEa;

                    int nTotalVolume;
                    double fTotalPrice, fTotalBuyPrice;
                    double fFs, fBuyFs;
                    List<ListViewItem> listViewItems = new List<ListViewItem>();


                    int nTodayTotalDisposalBuy = 0;
                    int nTodayTotalDisposalSell = 0;


                    // 리스트뷰 클리어
                    if (curListView.InvokeRequired)
                        curListView.Invoke(new MethodInvoker(delegate
                        {
                            curListView.Items.Clear();
                            curListView.BeginUpdate();
                        }));
                    else
                    {
                        curListView.Items.Clear();
                        curListView.BeginUpdate();
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

                        if (tabControl1.SelectedIndex == 0 && tmpEa.paperBuyStrategy.nStrategyNum == 0)
                            continue;
                        else if (tabControl1.SelectedIndex == 1 && tmpEa.myTradeManager.arrBuyedSlots.Count == 0)
                            continue;

                        tmpInfo.sGubun = tmpEa.sMarketGubunTag;
                        tmpInfo.sCode = tmpEa.sCode;
                        tmpInfo.sCodeName = tmpEa.sCodeName;
                        tmpInfo.nTradingNum = 0;
                        tmpInfo.nTradeNum = 0;
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

                        if (tabControl1.SelectedIndex == 0)
                        {
                            tmpInfo.nTradeNum = tmpEa.paperBuyStrategy.nStrategyNum;

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
                        }
                        else
                        {
                            tmpInfo.nTradeNum = tmpEa.myTradeManager.arrBuyedSlots.Count;

                            for (int j = 0; j < tmpEa.myTradeManager.arrBuyedSlots.Count; j++)
                            {
                                if (tmpEa.myTradeManager.arrBuyedSlots[j].isAllBuyed) // 다 사졌으면
                                {
                                    if (tmpEa.myTradeManager.arrBuyedSlots[j].isAllSelled) // 다 팔렸던거
                                    {
                                        fTotalPrice += tmpEa.myTradeManager.arrBuyedSlots[j].nSelledSumPrice;
                                        nTotalVolume += tmpEa.myTradeManager.arrBuyedSlots[j].nSellVolume;
                                        nTodayTotalDisposalSell += tmpEa.myTradeManager.arrBuyedSlots[j].nSelledSumPrice;
                                    }
                                    else // 매매중
                                    {
                                        tmpInfo.nTradingNum++;
                                        tmpInfo.isTradeStatus = true;

                                        if (tmpEa.myTradeManager.arrBuyedSlots[j].isSelling) // 판매중이라면
                                        {
                                            fTotalPrice += tmpEa.myTradeManager.arrBuyedSlots[j].nBuyVolume * ((tmpEa.nFb > 0) ? tmpEa.nFb : tmpEa.myTradeManager.arrBuyedSlots[j].nBuyPrice);
                                            nTotalVolume += tmpEa.myTradeManager.arrBuyedSlots[j].nBuyVolume; // 판매된만큼 + 잔량만큼 수량
                                        }
                                        else // 사기만 함
                                        {
                                            fTotalPrice += tmpEa.myTradeManager.arrBuyedSlots[j].nBuyVolume * ((tmpEa.nFb > 0) ? tmpEa.nFb : tmpEa.myTradeManager.arrBuyedSlots[j].nBuyPrice);
                                            nTotalVolume += tmpEa.myTradeManager.arrBuyedSlots[j].nBuyVolume;
                                        }
                                    }

                                    fTotalBuyPrice += tmpEa.myTradeManager.arrBuyedSlots[j].nBuyPrice * tmpEa.myTradeManager.arrBuyedSlots[j].nBuyVolume;
                                    nTodayTotalDisposalBuy += tmpEa.myTradeManager.arrBuyedSlots[j].nBuyPrice * tmpEa.myTradeManager.arrBuyedSlots[j].nBuyVolume;
                                }
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
                            tmpInfo.fEverageProfit = mainForm.GetProfitPercent((int)fBuyFs, (int)fFs, MainForm.KOSDAQ_ID);
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


                    if (curListView.InvokeRequired)
                        curListView.Invoke(new MethodInvoker(delegate
                        {
                            if (listViewItems.Count > 0)
                                curListView.Items.AddRange(listViewItems.ToArray());
                            curListView.EndUpdate();
                        }));
                    else
                    {
                        if (listViewItems.Count > 0)
                            curListView.Items.AddRange(listViewItems.ToArray());
                        curListView.EndUpdate();
                    }

                    int nTodayProfit = nTodayTotalDisposalSell - nTodayTotalDisposalBuy;
                    int nTodayProfitWithRealFee = mainForm.GetProfitPrice(nTodayTotalDisposalBuy, nTodayTotalDisposalSell, MainForm.KOSDAQ_ID);

                    VoidDelegator tmpDelegator = delegate
                    {
                        todayTotalResultTextBox.Clear();

                        string sStartMsg;
                        if(tabControl1.SelectedIndex == 0)
                            sStartMsg = "=============================== 모의매매 현재 결과 =====================================\r\n";
                        else
                            sStartMsg = "=============================== 실매매 현재 결과 =====================================\r\n";
                        todayTotalResultTextBox.AppendText(sStartMsg);
                        todayTotalResultTextBox.AppendText($" 오늘자 매수금액 : {((double)nTodayTotalDisposalBuy / MainForm.MILLION)}(백만원){NEW_LINE}");
                        todayTotalResultTextBox.AppendText($" 오늘자 매도금액 : {((double)nTodayTotalDisposalSell / MainForm.MILLION)}(백만원){NEW_LINE}");
                        todayTotalResultTextBox.AppendText($" 오늘자 총손익금 : {((double)nTodayProfit / MainForm.MILLION)}(백만원){NEW_LINE}");
                        todayTotalResultTextBox.AppendText($" 오늘자 총손익금(실제 수수료) : {((double)nTodayProfitWithRealFee / MainForm.MILLION)}(백만원){NEW_LINE}");
                    };

                    if (todayTotalResultTextBox.InvokeRequired)
                        todayTotalResultTextBox.Invoke(new MethodInvoker(tmpDelegator));
                    else
                        tmpDelegator();

                }
                catch (Exception ex)
                {

                }
            }

            if (curListView.InvokeRequired)
            {
                curListView.Invoke(new MethodInvoker(ShowRecords));
            }
            else
                ShowRecords();

        }// END ---- ShowTradeRecord


        #region Thread Call Method
        public void CallThreadEachStockHistoryForm(int nCallIdx)
        {
            try
            {
                if ((DateTime.UtcNow - mainForm.ea[nCallIdx].myTradeManager.dLatestApproachTime).TotalSeconds >= 1 && !mainForm.ea[nCallIdx].myTradeManager.isEachStockHistoryExist)
                {
                    mainForm.ea[nCallIdx].myTradeManager.dLatestApproachTime = DateTime.UtcNow;
                    mainForm.ea[nCallIdx].myTradeManager.isEachStockHistoryExist = true;
                    new Thread(() => new EachStockHistoryForm(mainForm, nCallIdx).ShowDialog()).Start();
                }
            }
            catch
            { }
        }

        public void CallThreadStatisticResultForm()
        {
            try
            {
                new Thread(() => new StatisticResultForm(mainForm).ShowDialog()).Start();
            }
            catch { }
        }
        #endregion
    }

    // 리스트뷰 비교 인스턴스
    class MyListViewComparer : IComparer
    {
        private int col;
        private SortOrder order;
        private string s;

        public MyListViewComparer()
        {
            col = 0;
            order = SortOrder.Ascending;
        }

        public MyListViewComparer(string s, int column, SortOrder order)
        {
            col = column;
            this.order = order;
            this.s = s;
        }

        public int Compare(object x, object y)
        {
            int returnVal = -1;

            if (s.Equals("STRING")) // string | boolean sort
            {
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
            else if (s.Equals("INT")) // int sort
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
            else if (s.Equals("DOUBLE")) // double sort
            {
                // 전량매수취소된경우 수익률이 0.0일텐데 이게 건들지 않아서  0.0이지 
                // canceld는 다른애들과 대비되야 한다. 그래서 수익률 비교하기 전에 isCanceld를 먼저 비교해준다.
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
