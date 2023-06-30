using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AtoIndicator.View.EachStockHistory;
using static AtoIndicator.KiwoomLib.TimeLib;

namespace AtoIndicator.View.EachStrategy
{
    public partial class EachStrategyForm : Form
    {

        #region 변수
        MainForm mainForm;
        int nStrategyIdx;
        MainForm.EachStock curEa;
        MainForm.PaperTradeSlot buyedSlot;
        string NEW_LINE = Environment.NewLine;
        #endregion

        #region 리스트뷰 속성정보
        public struct EachStrategyInfo
        {
            public int nBuyedTime;
            public string sCode;
            public string sCodeName;
            public int nStrategySequence;
            public double fProfit;
            public int nBuyedPrice;
            public int nCurPrice;
            public bool isTrading;
            public bool isAllCanceled;
            public int nSlotIdx;

            public string[] GetStringArray()
            {
                return new string[] { nBuyedTime.ToString(), sCode, sCodeName, nStrategySequence.ToString(),  Math.Round(fProfit * 100, 2).ToString(),
                    nBuyedPrice.ToString(), nCurPrice.ToString(),
                isTrading.ToString(), isAllCanceled.ToString(), nSlotIdx.ToString()};
            }

        }
        #endregion

        #region 생성자 
        public EachStrategyForm(MainForm parentForm, int strategyIdx)
        {
            mainForm = parentForm; // 메인폼을 받는다.
            nStrategyIdx = strategyIdx; // 해당전략idx를 받는다.
            InitializeComponent(); // Default Form Method



            string sString = "STRING";
            string sInt = "INT";
            string sDouble = "DOUBLE";

            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "매수시간" });
            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sString, Text = "종목번호" });
            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sString, Text = "종목명" });
            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "전략순번" });
            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sDouble, Text = "이익률" });
            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "매수가" });
            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "현재가" });
            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sString, Text = "거래중" });
            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sString, Text = "전량취소" });
            eachStrategyListView.Columns.Add(new ColumnHeader { Name = sInt, Text = "매매블록" });


            this.KeyPreview = true;
            this.KeyUp += KeyUpHandler;
            this.KeyDown += KeyDownHandler;
            this.Text = $"{strategyIdx}번 전략 결과화면";
            this.FormClosed += FormClosedHandler;
            this.DoubleBuffered = true;
            eachStrategyListView.MouseDoubleClick += EachStrategyDoubleClick;
            eachStrategyListView.MouseClick += EachStrategyMouseClick;
            eachStrategyListView.ColumnClick += ColumnClick;
            eachStrategyListView.View = System.Windows.Forms.View.Details;
            eachStrategyListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            UpdateThread();
        }
        #endregion
        public List<EachStrategyInfo> infoList = new List<EachStrategyInfo>(); // 통계 낼때를 위해 삽입해놓음

        public void ShowEachBuyedSlot()
        {
            void InvokeFunction()
            {
                try
                {
                    eachStrategyListView.Items.Clear();
                    eachStrategyListView.BeginUpdate();

                    infoList.Clear(); // update를 위해 매번 초기화

                    var list = mainForm.strategyHistoryList[nStrategyIdx]; // count > 0 이어야지 폼이 던져졌으니 최소 1개의 데이터가 있는걸 보장한다.
                    EachStrategyInfo eachStrategyInfo;

                    for (int i = 0; i < list.Count; i++)
                    {
                        curEa = mainForm.ea[list[i].nEaIdx];
                        buyedSlot = curEa.paperBuyStrategy.paperTradeSlot[list[i].nBuyedIdx];

                        // Default 작업
                        eachStrategyInfo.isAllCanceled = false;
                        eachStrategyInfo.isTrading = false;
                        eachStrategyInfo.sCode = curEa.sCode;
                        eachStrategyInfo.sCodeName = curEa.sCodeName;
                        eachStrategyInfo.nStrategySequence = buyedSlot.nSequence;
                        eachStrategyInfo.fProfit = 0.0;
                        eachStrategyInfo.nBuyedPrice = buyedSlot.nBuyedPrice;
                        eachStrategyInfo.nCurPrice = curEa.nFs;
                        eachStrategyInfo.nBuyedTime = buyedSlot.nBuyEndTime;
                        eachStrategyInfo.nSlotIdx = list[i].nBuyedIdx;
                        //////////////////

                        if (buyedSlot.isAllSelled)
                        {
                            if (buyedSlot.nBuyedVolume == 0) // 전량 매수취소된 케이스 
                            {
                                eachStrategyInfo.isAllCanceled = true;
                            }
                            else // 정상매매
                            {
                                eachStrategyInfo.fProfit = mainForm.GetProfitPercent(buyedSlot.nBuyedPrice * buyedSlot.nBuyedVolume, buyedSlot.nSellEndPrice * buyedSlot.nSellEndVolume, curEa.nMarketGubun) / 100;
                            }
                        }
                        else
                        {
                            if (buyedSlot.nBuyedVolume == buyedSlot.nTargetRqVolume) // 다 사지긴 함
                            {
                                eachStrategyInfo.fProfit = buyedSlot.fPowerWithFee;
                                eachStrategyInfo.isTrading = true;
                            }
                            else // 사는 중
                            {
                                // 데이터를 넣으면 오류 가능성이 있음. 안넣을 예정
                            }
                        }

                        int nMake255 = 6375; //  0.04 * 6375 = 255가 나온다( 최대값 : 0.04 )
                        ListViewItem listViewItem = new ListViewItem(eachStrategyInfo.GetStringArray());


                        // 색깔 설정하기
                        if (eachStrategyInfo.fProfit == 0)
                            listViewItem.BackColor = Color.FromArgb(255, 255, 255);
                        else if (eachStrategyInfo.fProfit > 0)
                        {
                            int nColorStep = (int)(eachStrategyInfo.fProfit * nMake255);
                            if (nColorStep > 255)
                                nColorStep = 255;
                            listViewItem.BackColor = Color.FromArgb(255, 255 - nColorStep, 255 - nColorStep);
                        }
                        else
                        {
                            int nColorStep = (int)(Math.Abs(eachStrategyInfo.fProfit) * nMake255);
                            if (nColorStep > 255)
                                nColorStep = 255;
                            listViewItem.BackColor = Color.FromArgb(255 - nColorStep, 255 - nColorStep, 255);
                        }

                        eachStrategyListView.Items.Add(listViewItem); // 삽입하기
                        infoList.Add(eachStrategyInfo);
                    }
                    eachStrategyListView.EndUpdate();
                }
                catch (Exception ex)
                {

                }
            }
            if (eachStrategyListView.InvokeRequired)  // 우선 리스트를 비워준다. 왜? update를 할것이기 때문에.. 
            {
                eachStrategyListView.Invoke(new MethodInvoker(InvokeFunction));
            }
            else
            {
                InvokeFunction();
            }
        }

        /// <summary>
        /// 키가 떼지면 수행되는 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="k"></param>
        public void KeyUpHandler(object sender, KeyEventArgs k)
        {
            char cUp = (char)k.KeyValue;
            if (cUp.Equals('U'))
                UpdateThread();

            if (cUp == 27 || cUp == 32) // esc or space
                this.Close();

        }
        public void FormClosedHandler(Object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// 키가 눌리면 수행되는 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="k"></param>
        public void KeyDownHandler(object sender, KeyEventArgs k)
        {
            char cDown = (char)k.KeyValue;
        }


        public void EachStrategyMouseClick(object sender, EventArgs e)
        {
            if (eachStrategyListView.FocusedItem != null)
            {
                int nEaIdx = mainForm.eachStockDict[eachStrategyListView.FocusedItem.SubItems[1].Text.Trim()];
                int nSlotIdx = int.Parse(eachStrategyListView.FocusedItem.SubItems[9].Text);

                curEa = mainForm.ea[nEaIdx];
                buyedSlot = mainForm.ea[nEaIdx].paperBuyStrategy.paperTradeSlot[nSlotIdx];

                if (buyedSlot.sFixedMsg == null)
                    return;

                choosedInfoTextBox.Text =
                    $"종목코드 : {curEa.sCode}{NEW_LINE}" +
                    $"매매블럭 : {nSlotIdx}{NEW_LINE}" +
                    $"종목명 : {curEa.sCodeName}{NEW_LINE}" +
                    $"-------------매수--------------{NEW_LINE}" +
                    $"신청시간 : {buyedSlot.nRqTime}{NEW_LINE}" +
                    $"체결시간 : {buyedSlot.nBuyEndTime}{NEW_LINE}" +
                    $"체결수량 : {buyedSlot.nBuyedVolume}{NEW_LINE}" +
                    $"체결가 : {buyedSlot.nBuyedPrice}{NEW_LINE}";

                if (buyedSlot.isAllSelled)
                {
                    if (buyedSlot.nBuyedVolume > 0)
                        choosedInfoTextBox.Text +=
                            $"-------------매도--------------{NEW_LINE}" +
                            $"매도시간 : {buyedSlot.nSellEndTime}{NEW_LINE}" +
                            $"매도가 : {buyedSlot.nSellEndPrice}{NEW_LINE}" +
                            $"매매시간 : {SubTimeToTime(buyedSlot.nSellEndTime, buyedSlot.nBuyEndTime)}{NEW_LINE}" +
                            $"매매결과 : {Math.Round(mainForm.GetProfitPercent(buyedSlot.nBuyedPrice * buyedSlot.nBuyedVolume, buyedSlot.nSellEndPrice * buyedSlot.nSellEndVolume, curEa.nMarketGubun), 2)}(%){NEW_LINE}";
                    else
                        choosedInfoTextBox.Text += $"전량 매수취소됐습니다.{NEW_LINE}";

                }
                choosedInfoTextBox.Text += buyedSlot.sFixedMsg;


            }
        }

        /// <summary>
        /// 매매정보 행을 더블클릭했을때 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EachStrategyDoubleClick(object sender, EventArgs e)
        {
            if (eachStrategyListView.FocusedItem != null)
            {
                string sCode = eachStrategyListView.FocusedItem.SubItems[1].Text;

                //EachStockHistoryForm eaForm = new EachStockHistoryForm(mainForm, mainForm.eachStockIdxArray[nCodeIdx], nStrategyIdx); // 메인폼, nCurIdx, buyedSlotIdx 를 던져준다.
                //eaForm.Show();
                try
                {
                    CallThreadEachStockHistoryForm(mainForm.eachStockDict[sCode.Trim()], nStrategyIdx);
                }
                catch { }
            }

        }

        public int sortColumn = -1;
        public const string UP_TIP = " ▲";
        public const string DOWN_TIP = " ▼";
        public int nTipLen = UP_TIP.Length;
        public void ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                if (sortColumn != -1)
                {
                    eachStrategyListView.Columns[sortColumn].Text = eachStrategyListView.Columns[sortColumn].Text.Substring(0, eachStrategyListView.Columns[sortColumn].Text.Length - nTipLen);
                }
                sortColumn = e.Column;
                // Set the sort ord6er to ascending by default.
                eachStrategyListView.Sorting = SortOrder.Descending;
                eachStrategyListView.Columns[sortColumn].Text = eachStrategyListView.Columns[sortColumn].Text + DOWN_TIP;
            }
            else
            {
                eachStrategyListView.Columns[sortColumn].Text = eachStrategyListView.Columns[sortColumn].Text.Substring(0, eachStrategyListView.Columns[sortColumn].Text.Length - nTipLen);
                // Determine what the last sort order was and change it.
                if (eachStrategyListView.Sorting == SortOrder.Ascending)
                {
                    eachStrategyListView.Sorting = SortOrder.Descending;
                    eachStrategyListView.Columns[sortColumn].Text = eachStrategyListView.Columns[sortColumn].Text + DOWN_TIP;
                }
                else
                {
                    eachStrategyListView.Sorting = SortOrder.Ascending;
                    eachStrategyListView.Columns[sortColumn].Text = eachStrategyListView.Columns[sortColumn].Text + UP_TIP;
                }
            }
            // Call the sort method to manually sort.
            this.eachStrategyListView.ListViewItemSorter = new EachStrategyViewComparer(eachStrategyListView.Columns[e.Column].Name, e.Column, eachStrategyListView.Sorting);
            eachStrategyListView.Sort();
            eachStrategyListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public void UpdateThread()
        {
            Task.Run(() => ShowEachBuyedSlot());
        }

        #region Thread Call Method
        public void CallThreadEachStockHistoryForm(int nCallIdx, int nCallStrategy)
        {
            try
            {
                if ((DateTime.UtcNow - mainForm.ea[nCallIdx].myTradeManager.dLatestApproachTime).TotalSeconds >= 1 && !mainForm.ea[nCallIdx].myTradeManager.isEachStockHistoryExist)
                {
                    mainForm.ea[nCallIdx].myTradeManager.dLatestApproachTime = DateTime.UtcNow;
                    mainForm.ea[nCallIdx].myTradeManager.isEachStockHistoryExist = true;
                    new Thread(() => new EachStockHistoryForm(mainForm, nCallIdx, nCallStrategy).ShowDialog()).Start();
                }
            }
            catch { }
        }
        #endregion
    }

    class EachStrategyViewComparer : IComparer
    {
        private int col;
        private string s;
        private SortOrder order;

        public EachStrategyViewComparer(string s, int column = 0, SortOrder order = SortOrder.Ascending)
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
