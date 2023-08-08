using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static AtoIndicator.KiwoomLib.TimeLib;
using static AtoIndicator.KiwoomLib.PricingLib;
using static AtoIndicator.Utils.Comparer;
using AtoIndicator.View.ScrollableMsgBox;
using AtoIndicator.MyControl;


namespace AtoIndicator.View.EachStockHistory
{
    #region EachStockHistoryForm
    public partial class EachStockHistoryForm : Form
    {
        #region 변수
        public int? nSpecificStrategyIdx; // 특정한 nStrategyIdx만 보여주게 돼있다 
        public int nCurIdx;
        public MainForm mainForm;
        public MainForm.EachStock curEa;

        public bool isMinuteVisible = false;
        public bool isBuyedBlockVisible = false;

        public bool isBuyArrowVisible = true;
        public bool isSellArrowVisible = true;
        public bool isFakeBuyArrowVisible = true;
        public bool isFakeResistArrowVisible = true;
        public bool isFakeAssistantArrowVisible = true;
        public bool isFakeVolatilityArrowVisible = true;
        public bool isFakeDownArrowVisible = true;
        public bool isPaperBuyArrowVisible = true;
        public bool isPaperSellArrowVisible = true;

        public bool isAllArrowVisible = true;

        public bool isViewGapMa = false;

        public int nLastMinuteIdx = 0;
        public int nLastBuyedBlockIdx = 0;

        public int nCurPaperBuyedId = -1;
        public int nCurRealBuyedId = -1;

        public int nMinPositionX1;
        public int nMinPositionY1;
        public int nMinPositionX2;
        public int nMinPositionY2;

        public bool isArrowGrouping = true;

        public System.Timers.Timer timer; // 타이머 선택이유는 비동기기 떄문에 Windows.Forms.Timer는 크로스 쓰레드 문제가 없는대신 UI쓰레드에서 돌아감
        public int d = 0;

        public string NEW_LINE = Environment.NewLine;
        public bool isAutoBoundaryCheck;

        public MainForm.PResult pResult;
        public MainForm.StrategyNames strategyNames;

        public ArrowControl arrowControl;
        public Dictionary<(int, int), List<PrevCancel>> toCancelDict = new Dictionary<(int, int), List<PrevCancel>>(); // key : (매매예약번호 , 가격) ,  value : list<주문번호>
        public const int BUY_RESERVE = 0;
        public const int SELL_RESERVE = 1;


        #endregion

        #region 이전 어노테이션 같은위치일 시 삭제용
        /// <summary>
        /// Dictionary의 Value위치에 삽입될것이고
        /// 중간중간 수정을 해줘야하니 값형식인 struct는 수정하기 번거로우니
        /// 참조형식인 class를 사용하겠다.
        /// </summary>
        public class RealAnnotationInfo
        {
            public int nCount; // 같은 시간(분봉)에 몇번의 strategy가 있었는지
            public string sTooltipMessage; // 같은 시간에 누적용 메시지
            public int? nLastAnnotationLoc; // 같은 분봉이 1번 초과할경우 이전 어노테이션을 삭제할 시그널 
                                            // 삭제방식 : "M1" , "M2" ... 으로 진행함 . 문자열이 아니라 int로 했을경우 중간의 정보를 삭제했을경우 참조하는 nLastAnnotationLoc를 다 차감해줘야하는데 ..???

            public RealAnnotationInfo(int nCount = 0, string sTooltipMessage = "")
            {
                this.nCount = nCount;
                this.sTooltipMessage = sTooltipMessage;
                nLastAnnotationLoc = null;
            }
        }
        #endregion

        public delegate void SetRadioDelegate();
        public SetRadioDelegate setPaperRadioDelegate;
        public int nPrevPaperRadioCnt = 0;
        public int nBPaper = RADIO_BUTTON_CHECKED; // radioButton 취소용
        public int rdPaperCnt;

        public SetRadioDelegate setRealRadioDelegate;
        public int nPrevRealRadioCnt = 0;
        public int nBReal = RADIO_BUTTON_CHECKED; // radioButton 취소용
        public int rdRealCnt;

        public const int RADIO_BUTTON_REMOVAL = 1;
        public const int RADIO_BUTTON_CHECKED = 2;

        #region 생성자
        public EachStockHistoryForm(MainForm parentForm, int nCallIdx, int? specificStrategy = null)
        {
            InitializeComponent();

            mainForm = parentForm;
            nCurIdx = nCallIdx; // nCurIdx 설정 
            strategyNames = mainForm.strategyName;
            curEa = mainForm.ea[nCurIdx]; // 해당 개인구조체 설정            
            this.Text = curEa.sCode + " - " + curEa.sCodeName;
            this.ActiveControl = historyChart;
            this.DoubleBuffered = true;

            curEa.myTradeManager.isTargetChoice = false;

            mainForm.ea[nCurIdx].eventMgr.cancelEachStockFormEventHandler += CancelEventHandler;

            totalClockLabel.Text = "현재시간 : " + mainForm.nSharedTime;

            SendToFrontSeriesName("MinuteStick");

            timer = new System.Timers.Timer(nTimerMilliSec);
            timer.Elapsed += delegate (Object sender, System.Timers.ElapsedEventArgs e)
            {
                updateDelegate();
            };

            #region Raio Button Delegate
            // ---------------------------------------------
            // START ---- Radio Button 파트
            setPaperRadioDelegate = delegate
            {
                try
                {
                    curEa = mainForm.ea[nCurIdx];
                    rdPaperCnt = curEa.paperBuyStrategy.nStrategyNum;
                    if (rdPaperCnt > 0)
                    {
                        paperBlockFlowLayoutPanel.Controls.Clear();
                        nPrevPaperRadioCnt = rdPaperCnt;
                        RadioButton newRd;

                        for (int i = 0; i < rdPaperCnt; i++)
                        {
                            newRd = new RadioButton();

                            if (curEa.paperBuyStrategy.paperTradeSlot[i].nBuyedVolume == curEa.paperBuyStrategy.paperTradeSlot[i].nTargetRqVolume) // 다 사졌으면
                            {
                                string sSpecificBling = "";

                                if (nSpecificStrategyIdx != null && curEa.paperBuyStrategy.arrSpecificStrategy[i] == nSpecificStrategyIdx)
                                {
                                    sSpecificBling = "#";
                                }


                                if (curEa.paperBuyStrategy.paperTradeSlot[i].nBuyedVolume > 0) // 체결이 일부라도 됐으면
                                {
                                    if (curEa.paperBuyStrategy.paperTradeSlot[i].isAllSelled)
                                        newRd.Text = sSpecificBling + i.ToString() + "번째(매매완료)";
                                    else
                                        newRd.Text = sSpecificBling + i.ToString() + "번째";
                                }
                                else
                                    newRd.Text = sSpecificBling + i.ToString() + "번째(매수취소)";


                                newRd.Name = i.ToString();

                                newRd.CheckedChanged += delegate (Object oo, EventArgs ee)
                                {
                                    // 처음누를때 순서
                                    // checkedChanged -> Click

                                    // 눌려진걸 누를때 순서(radio는 원래 눌려진거 눌러도 안꺼짐)
                                    // Click

                                    // 눌려진걸 눌러서 버튼취소하는 메서드가 있는 경우 순서(사용자 메서드로 강제 버튼취소했을때)
                                    // Click -> checkedChanged

                                    // 눌려져있는데 다른 버튼을 누른경우 순서
                                    // checkedChanged(이전거off) -> checkedChanged(새로운거on) -> Click


                                    RadioButton r = (RadioButton)oo;
                                    if (nBPaper == RADIO_BUTTON_CHECKED)
                                    {
                                        nCurPaperBuyedId = int.Parse(r.Name); // 체크된 매매블록의 인덱스
                                    }
                                    nBPaper = RADIO_BUTTON_CHECKED;


                                };

                                // END ---- CheckedChanged
                                newRd.Click += delegate (Object oo, EventArgs ee)
                                {
                                    if (nBPaper == RADIO_BUTTON_REMOVAL) // 접근 가능한 상황 : 이미 눌려진 버튼을 눌러서 취소하는 상황
                                    {
                                        RadioButton checkedRd = (RadioButton)oo;
                                        checkedRd.Checked = false;
                                        nCurPaperBuyedId = -1;
                                        nBPaper = RADIO_BUTTON_CHECKED;
                                    }
                                    else
                                        nBPaper = RADIO_BUTTON_REMOVAL;

                                    updateDelegate();
                                };

                                if (nSpecificStrategyIdx != null && curEa.paperBuyStrategy.arrSpecificStrategy[i] != nSpecificStrategyIdx)
                                    newRd.Enabled = false;

                            }
                            else
                            {
                                newRd.Text = i.ToString() + "번째(매매중..)";
                                newRd.Enabled = false;
                            }

                            newRd.Width = (TextRenderer.MeasureText(newRd.Text, newRd.Font)).Width + 20; // 글자 안잘리게
                            paperBlockFlowLayoutPanel.Controls.Add(newRd);
                            paperBlockFlowLayoutPanel.SetFlowBreak(newRd, true); // 각행마다 너비가 차이나면 개행이 잘 안돼서 강제하는 코드
                        }
                    }
                }
                catch { }
            };

            setRealRadioDelegate = delegate
            {
                try
                {
                    curEa = mainForm.ea[nCurIdx];

                    rdRealCnt = curEa.myTradeManager.arrBuyedSlots.Count;
                    if (rdRealCnt > 0)
                    {
                        realBlockFlowLayoutPanel.Controls.Clear();
                        nPrevRealRadioCnt = rdRealCnt;
                        RadioButton newRd;

                        for (int i = 0; i < rdRealCnt; i++)
                        {
                            newRd = new RadioButton();

                            if (curEa.myTradeManager.arrBuyedSlots[i].isAllSelled)
                                newRd.Text = i.ToString() + "번째(매매완료)";
                            else
                                newRd.Text = i.ToString() + "번째";
                            newRd.Name = i.ToString();

                            newRd.CheckedChanged += delegate (Object oo, EventArgs ee)
                            {
                                // 처음누를때 순서
                                // checkedChanged -> Click

                                // 눌려진걸 누를때 순서(radio는 원래 눌려진거 눌러도 안꺼짐)
                                // Click

                                // 눌려진걸 눌러서 버튼취소하는 메서드가 있는 경우 순서(사용자 메서드로 강제 버튼취소했을때)
                                // Click -> checkedChanged

                                // 눌려져있는데 다른 버튼을 누른경우 순서
                                // checkedChanged(이전거off) -> checkedChanged(새로운거on) -> Click


                                RadioButton r = (RadioButton)oo;
                                if (nBReal == RADIO_BUTTON_CHECKED)
                                {
                                    nCurRealBuyedId = int.Parse(r.Name); // 체크된 매매블록의 인덱스
                                    curEa.myTradeManager.nAppliedShowingRealBuyedId = nCurRealBuyedId;
                                    eBuyMode = TRADE_MODE.SELL_MODE;
                                    buyModeLabel.Text = $"buy : {eBuyMode}";
                                    // 체크됐을때는?
                                    if (curEa.myTradeManager.arrBuyedSlots[nCurRealBuyedId].eTradeMethod != MainForm.TradeMethodCategory.FixedMethod)
                                        tradeMethodLabel.Text = $"{nCurRealBuyedId}번 매매기법 : {curEa.myTradeManager.arrBuyedSlots[nCurRealBuyedId].eTradeMethod}";
                                    else
                                        tradeMethodLabel.Text = $"{nCurRealBuyedId}번 매매기법 : {curEa.myTradeManager.arrBuyedSlots[nCurRealBuyedId].eTradeMethod} ({Math.Round(curEa.myTradeManager.arrBuyedSlots[nCurRealBuyedId].fTargetPer, 3)}, {Math.Round(curEa.myTradeManager.arrBuyedSlots[nCurRealBuyedId].fBottomPer, 3)})";
                                }
                                nBReal = RADIO_BUTTON_CHECKED;


                            };

                            // END ---- CheckedChanged
                            newRd.Click += delegate (Object oo, EventArgs ee)
                            {
                                if (nBReal == RADIO_BUTTON_REMOVAL) // 접근 가능한 상황 : 이미 눌려진 버튼을 눌러서 취소하는 상황
                                {
                                    RadioButton checkedRd = (RadioButton)oo;
                                    checkedRd.Checked = false;
                                    nCurRealBuyedId = -1;
                                    curEa.myTradeManager.nAppliedShowingRealBuyedId = -1;
                                    nBReal = RADIO_BUTTON_CHECKED;
                                    ClearBuyMode();

                                    // 체크 해제됐을때는??
                                    tradeMethodLabel.Text = $"전체 매매기법 : {curEa.myTradeManager.eDefaultTradeCategory}";
                                }
                                else
                                    nBReal = RADIO_BUTTON_REMOVAL;

                                this.ActiveControl = historyChart;
                                updateDelegate();
                            };


                            newRd.Width = (TextRenderer.MeasureText(newRd.Text, newRd.Font)).Width + 20; // 글자 안잘리게
                            realBlockFlowLayoutPanel.Controls.Add(newRd);
                            realBlockFlowLayoutPanel.SetFlowBreak(newRd, true); // 각행마다 너비가 차이나면 개행이 잘 안돼서 강제하는 코드
                        }
                    }
                }
                catch { }
            };

            #endregion

            historyChart.AxisViewChanged += ChartViewChanged;

            showVarToolStripMenuItem.Click += ToolTipItemClickHandler;
            showLogToolStripMenuItem.Click += ToolTipItemClickHandler;

            tabControl1.SelectedIndexChanged += SelectedIndexChangedHandler;

            historyChart.MouseMove += ChartMouseMoveHandler; // this.MouseMove로 바꾸면 chart cursor이런거 동작 안한다.
            historyChart.MouseClick += ChartMouseClickHandler; // 이하동문
            historyChart.Paint += ChartOnPaintHandler;
            historyChart.Resize += ChartResizeHandler;

            timerUpButton.Click += TimerButtonClickHandler;
            timerDownButton.Click += TimerButtonClickHandler;

            this.KeyPreview = true;
            this.KeyDown += KeyDownHandler;
            this.KeyUp += KeyUpHandler;
            this.FormClosed += FormClosedHandler;
            this.Resize += CancelEventHandler;
            this.MouseWheel += MouseWheelEventHandler;
            nSpecificStrategyIdx = specificStrategy;

            ResetMinuteChart(); // 이게 먼저 실행되어야 chart를 초기화시켜줌. 먼저 실행안되면 차트가 candleStickType아니라 오류생김


            historyChart.ChartAreas["TotalArea"].Visible = true;
            isMinuteVisible = true;

            if (nSpecificStrategyIdx != null)
            {
                this.Text += $" {specificStrategy}번 전략 전용창";
                ReverseAllArrowVisible();
                isPaperBuyArrowVisible = true; // 매수와
                isPaperSellArrowVisible = true; // 매도만 
                UpdateMinuteHistoryData();
            }


            setPaperRadioDelegate();
            setRealRadioDelegate();

            SetCurGraphics();
            timer.Enabled = true;
        }
        #endregion

        public void FormClosedHandler(Object sender, FormClosedEventArgs e)
        {
            timer.Enabled = false;
            mainForm.ea[nCurIdx].eventMgr.cancelEachStockFormEventHandler -= CancelEventHandler;
            mainForm.ea[nCurIdx].myTradeManager.isEachStockHistoryExist = false;
            this.Dispose();
        }

        #region 전체 업데이트
        public void updateDelegate()
        {
            try // 해당 폼이 닫혀도 실시간버튼을 통해 timer스레드가 updateDelegate를 실행시키면 오류가 발생하기 때문
            {

                curEa = mainForm.ea[nCurIdx];

                if (totalClockLabel.InvokeRequired)
                {
                    totalClockLabel.Invoke(new MethodInvoker(delegate
                    {
                        totalClockLabel.Text = "현재시간 : " + mainForm.nSharedTime;
                        depositLabel.Text = $"예수금 : {mainForm.nCurDepositCalc}";
                    }));
                }
                else
                {
                    totalClockLabel.Text = "현재시간 : " + mainForm.nSharedTime;
                    depositLabel.Text = $"예수금 : {mainForm.nCurDepositCalc}";
                }

                if (historyChart.InvokeRequired)
                    historyChart.Invoke(new MethodInvoker(delegate { historyChart.Annotations.Clear(); }));
                else
                    historyChart.Annotations.Clear();

                if (curEa.paperBuyStrategy.nStrategyNum > nPrevPaperRadioCnt || curEa.paperBuyStrategy.isPaperBuyChangeNeeded)
                {
                    curEa.paperBuyStrategy.isPaperBuyChangeNeeded = false;
                    nCurPaperBuyedId = -1;
                    nBPaper = RADIO_BUTTON_CHECKED;
                    if (paperBlockFlowLayoutPanel.InvokeRequired)
                    {
                        paperBlockFlowLayoutPanel.Invoke(new MethodInvoker(setPaperRadioDelegate));
                    }
                    else
                        setPaperRadioDelegate();
                }

                if (curEa.myTradeManager.arrBuyedSlots.Count > nPrevRealRadioCnt || curEa.myTradeManager.isRealBuyChangeNeeded)
                {
                    curEa.myTradeManager.isRealBuyChangeNeeded = false;
                    nCurRealBuyedId = -1;
                    nBReal = RADIO_BUTTON_CHECKED;
                    if (realBlockFlowLayoutPanel.InvokeRequired)
                    {
                        realBlockFlowLayoutPanel.Invoke(new MethodInvoker(setRealRadioDelegate));
                    }
                    else
                        setRealRadioDelegate();
                }


                UpdateMinuteHistoryData();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        public double fMaxPlus = 0;
        public double fMinPlus = 0;


        public int nXPlus = 0;

        #region 차트화면 재조정
        /// <summary>
        /// x값 범위내 y값에 맞게 범위를 지정해준다.
        /// </summary>
        /// <param name="viewMin"></param>
        /// <param name="viewMax"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="chartName"></param>
        public void SetChartViewRange(int viewMin, int viewMax, int max, int min, string chartName)
        {
            try
            {
                MainForm.TimeLineManager timeLineManager;
                int nSeriesCnt;
                curEa = mainForm.ea[nCurIdx];

                timeLineManager = curEa.timeLines1m;
                nSeriesCnt = historyChart.Series["MinuteStick"].Points.Count;

                if (viewMin == 0) // 뷰를 바꿀때 양쪽에 1씩 패딩을 자동으로 측정해주는데 맨왼쪽에서는 왼쪽패딩을 못주는데 여튼 그렇게 됐다.
                    viewMin++;

                if (viewMin - 1 >= nSeriesCnt)
                    return;

                for (int i = viewMin - 1; i < viewMax; i++)
                {
                    if (i >= nSeriesCnt)
                        break;

                    if (timeLineManager.arrTimeLine[i].nMaxFs > max)
                        max = timeLineManager.arrTimeLine[i].nMaxFs;
                    if (timeLineManager.arrTimeLine[i].nMinFs < min && timeLineManager.arrTimeLine[i].nMinFs != 0)
                        min = timeLineManager.arrTimeLine[i].nMinFs;

                    if (isViewGapMa)
                    {
                        if (i <= timeLineManager.nRealDataIdx)
                        {
                            max = (int)Max(max, timeLineManager.arrTimeLine[i].fOverMaGap0);
                            max = (int)Max(max, timeLineManager.arrTimeLine[i].fOverMaGap1);
                            max = (int)Max(max, timeLineManager.arrTimeLine[i].fOverMaGap2);

                            if (timeLineManager.arrTimeLine[i].fOverMaGap0 < min && timeLineManager.arrTimeLine[i].fOverMaGap0 != 0)
                                min = (int)timeLineManager.arrTimeLine[i].fOverMaGap0;

                            if (timeLineManager.arrTimeLine[i].fOverMaGap1 < min && timeLineManager.arrTimeLine[i].fOverMaGap1 != 0)
                                min = (int)timeLineManager.arrTimeLine[i].fOverMaGap1;

                            if (timeLineManager.arrTimeLine[i].fOverMaGap2 < min && timeLineManager.arrTimeLine[i].fOverMaGap2 != 0)
                                min = (int)timeLineManager.arrTimeLine[i].fOverMaGap2;

                        }
                    }

                }



                max += GetAutoGap(curEa.nMarketGubun, max) * 1;
                min -= GetAutoGap(curEa.nMarketGubun, max) * 1;

                max = (int)(max * (1 + fMaxPlus));
                min = (int)(min * (1 - fMinPlus));


                historyChart.ChartAreas[chartName].AxisX.MajorGrid.Enabled = false;
                historyChart.ChartAreas[chartName].AxisY.Maximum = max;
                historyChart.ChartAreas[chartName].AxisY.Minimum = min;
                historyChart.ChartAreas[chartName].AxisY.MajorGrid.LineColor = Color.LightGray;

                historyChart.ChartAreas[chartName].AxisX.Minimum = viewMin - 1;
                historyChart.ChartAreas[chartName].AxisX.Maximum = viewMax + nXPlus;

            }
            catch
            {

            }
        }


        public void ChartViewChanged(Object sender, ViewEventArgs e)
        {
            try
            {
                if (sender.Equals(historyChart))
                {
                    SetChartViewRange((int)e.Axis.ScaleView.ViewMinimum, (int)e.Axis.ScaleView.ViewMaximum, (int)e.ChartArea.AxisY.ScaleView.ViewMinimum, (int)e.ChartArea.AxisY.ScaleView.ViewMaximum, e.ChartArea.Name);
                }
            }
            catch
            {

            }
        }
        #endregion

        #region 1분 차트 초기화
        /// <summary>
        /// 처음 분당 차트데이터를 삽입할때 
        /// </summary>
        public void ResetMinuteChart()
        {
            void voidDelegate()
            {

                nLastMinuteIdx = 0;

                historyChart.Series["MinuteStick"].Points.Clear();
                historyChart.Series["Ma20m"].Points.Clear();
                historyChart.Series["Ma1h"].Points.Clear();
                historyChart.Series["Ma2h"].Points.Clear();

                historyChart.Series["MinuteStick"].ChartType = SeriesChartType.Candlestick;
                historyChart.Series["MinuteStick"].ChartArea = "TotalArea";

                historyChart.Series["Ma20m"].ChartType = SeriesChartType.Line;
                historyChart.Series["Ma1h"].ChartType = SeriesChartType.Line;
                historyChart.Series["Ma2h"].ChartType = SeriesChartType.Line;

                historyChart.Series["Ma20mGap"].Points.Clear();
                historyChart.Series["Ma1hGap"].Points.Clear();
                historyChart.Series["Ma2hGap"].Points.Clear();
                historyChart.Series["Ma20mGap"].ChartType = SeriesChartType.Line;
                historyChart.Series["Ma1hGap"].ChartType = SeriesChartType.Line;
                historyChart.Series["Ma2hGap"].ChartType = SeriesChartType.Line;
                historyChart.Series["Ma20mGap"].Enabled = isViewGapMa;
                historyChart.Series["Ma1hGap"].Enabled = isViewGapMa;
                historyChart.Series["Ma2hGap"].Enabled = isViewGapMa;

                historyChart.Series["MinuteStick"].SetCustomProperty("PriceUpColor", "Red");
                historyChart.Series["MinuteStick"].SetCustomProperty("PriceDownColor", "Blue");

                UpdateMinuteHistoryData();
            }

            if (historyChart.InvokeRequired)
                historyChart.Invoke(new MethodInvoker(voidDelegate));
            else
                voidDelegate();
        }
        #endregion

        #region 1분 차트 업데이트
        public void UpdateMinuteHistoryData()
        {
            void voidDelegate()
            {
                try
                {
                    // 어노테이션 초기화
                    historyChart.Annotations.Clear();

                    int nTimeNow;
                    string sTime;

                    curEa = mainForm.ea[nCurIdx];

                    if (curEa.timeLines1m.nRealDataIdx > 0) // 데이터가 초기화돼야 접근가능, 0이 넘지 않을경우 인덱스 오류에 걸릴 수 도 있다.(어차피 금방 초기화돼서 접근 가능하게 된다)
                    {

                        if (historyChart.Series["MinuteStick"].Points.Count != historyChart.Series["Ma20m"].Points.Count) /// candleStick과 ma의 갯수가 다르다면
                        {
                            historyChart.Series["MinuteStick"].Points.RemoveAt(historyChart.Series["MinuteStick"].Points.Count - 1);
                        }

                        try
                        {
                            while (nLastMinuteIdx <= curEa.timeLines1m.nRealDataIdx)
                            {
                                nTimeNow = curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nTime;
                                sTime = nTimeNow.ToString();

                                historyChart.Series["Ma20m"].Points.AddXY(sTime, curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa0);
                                historyChart.Series["Ma1h"].Points.AddXY(sTime, curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa1);
                                historyChart.Series["Ma2h"].Points.AddXY(sTime, curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa2);
                                historyChart.Series["Ma20mGap"].Points.AddXY(sTime, curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMaGap0);
                                historyChart.Series["Ma1hGap"].Points.AddXY(sTime, curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMaGap1);
                                historyChart.Series["Ma2hGap"].Points.AddXY(sTime, curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMaGap2);

                                historyChart.Series["MinuteStick"].Points.AddXY(sTime, curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nMaxFs);
                                historyChart.Series["MinuteStick"].Points[nLastMinuteIdx].YValues[1] = curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nMinFs;
                                historyChart.Series["MinuteStick"].Points[nLastMinuteIdx].YValues[2] = curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nStartFs;
                                historyChart.Series["MinuteStick"].Points[nLastMinuteIdx].YValues[3] = curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nLastFs;
                                historyChart.Series["MinuteStick"].Points[nLastMinuteIdx].ToolTip =
                                    $"해당시각 : {nTimeNow},  인덱스 : {nLastMinuteIdx}{NEW_LINE}" +
                                    $"종가 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nLastFs}{NEW_LINE}" +
                                    $"시가 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nStartFs}{NEW_LINE}" +
                                    $"고가 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nMaxFs}{NEW_LINE}" +
                                    $"저가 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nMinFs}{NEW_LINE}" +
                                    $"거래량 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nTotalVolume}, 매수/매도비율 : {Math.Round((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nBuyVolume - curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nSellVolume) / (curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nTotalVolume + 1) * 100, 2)}(%){NEW_LINE}" +
                                    $"속도 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nCount}{NEW_LINE}" +
                                    $"*거래대금 : {Math.Round((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].lTotalPrice / MainForm.MILLION), 2)}(백만원){NEW_LINE}" +
                                    $"매수대금 : {Math.Round((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].lBuyPrice / MainForm.MILLION), 2)}(백만원){NEW_LINE}" +
                                    $"매도대금 : {Math.Round((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].lSellPrice / MainForm.MILLION), 2)}(백만원){NEW_LINE}" +
                                    $"누적상승 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fAccumUpPower * 100, 2)}(%),  누적하락 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fAccumDownPower * 100, 2)}(%){NEW_LINE}" +
                                    $"손익률 : {Math.Round(((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nLastFs - curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nStartFs) / curEa.nYesterdayEndPrice) * 100, 2)}(%){NEW_LINE}" +
                                    $"T각도 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fMedianAngle, 2)}{NEW_LINE}" +
                                    $"*H각도 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fHourAngle, 2)}{NEW_LINE}" +
                                    $"*R각도 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fRecentAngle, 2)}{NEW_LINE}" +
                                    $"*D각도 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fDAngle, 2)}{NEW_LINE}" +
                                    $"*Ma20m > Ma1h: {(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa0 > curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa1)}{NEW_LINE}" +
                                    $"*Ma1h > Ma2h: {(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa1 > curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa2)}{NEW_LINE}" +
                                    $"Ma20m > Ma2h: {(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa0 > curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fOverMa2)}{NEW_LINE}" +
                                    $"*Ma20m-- : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nDownTimeOverMa0}{NEW_LINE}" +
                                    $"*Ma1h-- : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nDownTimeOverMa1}{NEW_LINE}" +
                                    $"*Ma2h-- : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nDownTimeOverMa2}{NEW_LINE}" +
                                    $"Ma20m++ : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nUpTimeOverMa0}{NEW_LINE}" +
                                    $"Ma1h++ : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nUpTimeOverMa1}{NEW_LINE}" +
                                    $"Ma2h++ : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nUpTimeOverMa2}{NEW_LINE}" +
                                    $"전일대비 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fTradeCompared, 3)}{NEW_LINE}" +
                                    $"체결강도 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fTradeStrength, 3)}{NEW_LINE}" +
                                    $"*총순위 : {curEa.rankSystem.arrRanking[nLastMinuteIdx].nSummationRanking}위( {curEa.rankSystem.arrRanking[nLastMinuteIdx].nSummationMove} ){NEW_LINE}" +
                                    $"*분당순위 : {curEa.rankSystem.arrRanking[nLastMinuteIdx].nMinuteRanking} 위";
                                nLastMinuteIdx++;
                            }


                            if (curEa.timeLines1m.arrTimeLine[curEa.timeLines1m.nPrevTimeLineIdx].nLastFs != 0) // 최근의 기록이 입력돼있으면
                            {
                                nTimeNow = AddTimeBySec(curEa.timeLines1m.arrTimeLine[curEa.timeLines1m.nRealDataIdx].nTime, MainForm.MINUTE_SEC);
                                sTime = nTimeNow.ToString();

                                historyChart.Series["MinuteStick"].Points.AddXY(sTime, curEa.timeLines1m.arrTimeLine[curEa.timeLines1m.nPrevTimeLineIdx].nMaxFs);
                                historyChart.Series["MinuteStick"].Points[nLastMinuteIdx].YValues[1] = curEa.timeLines1m.arrTimeLine[curEa.timeLines1m.nPrevTimeLineIdx].nMinFs;
                                historyChart.Series["MinuteStick"].Points[nLastMinuteIdx].YValues[2] = curEa.timeLines1m.arrTimeLine[curEa.timeLines1m.nPrevTimeLineIdx].nStartFs;
                                historyChart.Series["MinuteStick"].Points[nLastMinuteIdx].YValues[3] = curEa.timeLines1m.arrTimeLine[curEa.timeLines1m.nPrevTimeLineIdx].nLastFs;
                                historyChart.Series["MinuteStick"].Points[nLastMinuteIdx].ToolTip =
                                    $"**해당시각 : {nTimeNow},  업데이트시각 : {mainForm.nSharedTime},  인덱스 : {nLastMinuteIdx}{NEW_LINE}" +
                                    $"종가 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nLastFs}{NEW_LINE}" +
                                    $"시가 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nStartFs}{NEW_LINE}" +
                                    $"고가 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nMaxFs}{NEW_LINE}" +
                                    $"저가 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nMinFs}{NEW_LINE}" +
                                    $"거래량 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nTotalVolume}, 매수/매도비율 : {Math.Round((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nBuyVolume - curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nSellVolume) / (curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nTotalVolume + 1) * 100, 2)}(%){NEW_LINE}" +
                                    $"속도 : {curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nCount}{NEW_LINE}" +
                                    $"*거래대금 : {Math.Round((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].lTotalPrice / MainForm.MILLION), 2)}(백만원){NEW_LINE}" +
                                    $"매수대금 : {Math.Round((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].lBuyPrice / MainForm.MILLION), 2)}(백만원){NEW_LINE}" +
                                    $"매도대금 : {Math.Round((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].lSellPrice / MainForm.MILLION), 2)}(백만원){NEW_LINE}" +
                                    $"누적상승 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fAccumUpPower * 100, 2)}(%),  누적하락 : {Math.Round(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].fAccumDownPower * 100, 2)}(%){NEW_LINE}" +
                                    $"손익률 : {Math.Round(((double)(curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nLastFs - curEa.timeLines1m.arrTimeLine[nLastMinuteIdx].nStartFs) / curEa.nYesterdayEndPrice) * 100, 2)}(%){NEW_LINE}";
                            }
                        }
                        catch (Exception ex) // 장끝나면 idx에러
                        {

                        }

                        Dictionary<int, RealAnnotationInfo> realDictionary = new Dictionary<int, RealAnnotationInfo>();
                        int nNumInjector = 0; // 어노테이션의 위치정보 .. "M"시리즈에서만 쓸모가 있음.
                                              // "M" 시리즈는 시간의 흐름에 따라 놓여야하는 인덱스 배정이 11 10 11 이렇게 되고 세번째 인덱스의 경우 중첩이 되니 이전꺼를 삭제하려고 하면 가장최근인 10이 되어 오류가 발생한다.
                                              //  그래서 삭제할 위치를 정할때는 Name == "M" + nNumInjector로 구분하면 된다.
                                              // "F" 시리즈는 분봉이 중첩되면 가장최근것만 삭제하면된다

                        //START ---- PAPER BUY ARROW
                        if (curEa.paperBuyStrategy.nStrategyNum > 0 && isPaperBuyArrowVisible)
                        {


                            for (int p = 0; p < curEa.paperBuyStrategy.nStrategyNum; p++)
                            {
                                if (nSpecificStrategyIdx != null && curEa.paperBuyStrategy.arrSpecificStrategy[p] != nSpecificStrategyIdx) // 특정한 전략만 보여주게 설정했다면
                                    continue; // 특정전략인덱스가 아니면 건너뛴다.

                                if (nCurPaperBuyedId != -1 && p != nCurPaperBuyedId)
                                    continue;

                                if (curEa.paperBuyStrategy.paperTradeSlot[p].nBuyedVolume != curEa.paperBuyStrategy.paperTradeSlot[p].nTargetRqVolume)
                                    continue;

                                nNumInjector++;

                                int nPaperBuyAnnotationIdx = curEa.paperBuyStrategy.paperTradeSlot[p].nBuyRqTimeLineIdx;
                                string sPaperBuyArrowToolTip = "";

                                if (curEa.paperBuyStrategy.paperTradeSlot[p].nBuyedVolume == 0) // 전량 매수취소가 된 상황 
                                {
                                    sPaperBuyArrowToolTip +=
                                        $"[매수취소] 매수요청시간 : {curEa.paperBuyStrategy.paperTradeSlot[p].nRqTime}  매도요청시간 : {curEa.paperBuyStrategy.paperTradeSlot[p].nSellRqTime}\n" +
                                        $"매수블록ID : {p}  주문수량 : " + curEa.paperBuyStrategy.paperTradeSlot[p].nRqVolume + "(주)\n" +
                                        "매수설명 : " + strategyNames.arrPaperBuyStrategyName[curEa.paperBuyStrategy.arrSpecificStrategy[p]] + "\n\n";

                                }
                                else // 일부 매수취소 + 정상 매수
                                {
                                    sPaperBuyArrowToolTip +=
                                         $"매수요청시간 : {curEa.paperBuyStrategy.paperTradeSlot[p].nRqTime}  체결시간 : {curEa.paperBuyStrategy.paperTradeSlot[p].nBuyEndTime}\n" +
                                         $"매수블록ID : {p}  주문가격(수량) : {curEa.paperBuyStrategy.paperTradeSlot[p].nOverPrice}(원)({curEa.paperBuyStrategy.paperTradeSlot[p].nRqVolume}),  매수가격(수량) : {curEa.paperBuyStrategy.paperTradeSlot[p].nBuyedPrice}({curEa.paperBuyStrategy.paperTradeSlot[p].nBuyedVolume})\n" +
                                         "매수설명 : " + strategyNames.arrPaperBuyStrategyName[curEa.paperBuyStrategy.arrSpecificStrategy[p]] + "\n\n";
                                }

                                if (realDictionary.ContainsKey(nPaperBuyAnnotationIdx)) // 해당위치(같은분봉) 에 값이 들어있다면
                                {
                                    realDictionary[nPaperBuyAnnotationIdx].nCount++;
                                    realDictionary[nPaperBuyAnnotationIdx].sTooltipMessage += sPaperBuyArrowToolTip;
                                }
                                else // 값이 없다면 => 1번째 데이터
                                {
                                    realDictionary[nPaperBuyAnnotationIdx] = new RealAnnotationInfo(1, sPaperBuyArrowToolTip);
                                }


                                // 매도 어노테이션 삽입
                                ArrowAnnotation arrowPaperBuy = new ArrowAnnotation();
                                if (!isArrowGrouping)
                                {
                                    arrowPaperBuy.Width = -3; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }
                                else
                                {
                                    arrowPaperBuy.Width = -1; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세

                                }
                                arrowPaperBuy.Height = -4;// -면 아래쪽방향, + 면 위쪽방향
                                arrowPaperBuy.AnchorOffsetY = -1.5;



                                arrowPaperBuy.ToolTip = $"*모의매수 총 갯수 : {realDictionary[nPaperBuyAnnotationIdx].nCount}개\n" +
                                   $"=====================================================\n" + realDictionary[nPaperBuyAnnotationIdx].sTooltipMessage;
                                if (realDictionary[nPaperBuyAnnotationIdx].nCount == 1)
                                    arrowPaperBuy.Height = -4;
                                else
                                {
                                    for (int k = 0; k < historyChart.Annotations.Count; k++) // 어노테이션들 중
                                    {
                                        if (historyChart.Annotations[k].Name.Equals("P" + realDictionary[nPaperBuyAnnotationIdx].nLastAnnotationLoc))  // P + 해당분봉의 최근삽입정보
                                        {
                                            historyChart.Annotations.RemoveAt(k);
                                        }
                                    }
                                    arrowPaperBuy.Height = -7;
                                }

                                realDictionary[nPaperBuyAnnotationIdx].nLastAnnotationLoc = nNumInjector;  // 최근 삽입시점 삽입

                                arrowPaperBuy.BackColor = Color.Red;
                                arrowPaperBuy.SetAnchor(historyChart.Series["MinuteStick"].Points[curEa.paperBuyStrategy.paperTradeSlot[p].nBuyRqTimeLineIdx]);
                                // arrowFakeBuy.AnchorY = historyChart.Series["MinuteStick"].Points[curEa.fakeBuyStrategy.arrMinuteIdx[p]].YValues[1]; // 고.저.시종
                                arrowPaperBuy.Name = "P" + nNumInjector;
                                arrowPaperBuy.LineColor = Color.Black;

                                historyChart.Annotations.Add(arrowPaperBuy);
                            }
                        }//END ---- PAPER BUY ARROW
                        realDictionary.Clear(); // 모의매수 어노테이션 설정후 클리어

                        //START ---- FAKE BUY ARROW
                        if (curEa.fakeBuyStrategy.nStrategyNum > 0 && isFakeBuyArrowVisible)
                        {
                            int nPrevFakeBuyMinArrowIdx = -1;
                            int nFakeBuyOverlapCount = 0;
                            string sFakeBuyArrowToolTip = "";

                            for (int p = 0; p < curEa.fakeBuyStrategy.nStrategyNum; p++)
                            {
                                nNumInjector++;
                                if (nPrevFakeBuyMinArrowIdx == curEa.fakeBuyStrategy.arrMinuteIdx[p]) // 같은 minute Idx랑 겹친다면
                                {
                                    nFakeBuyOverlapCount++;
                                }
                                else
                                {
                                    nPrevFakeBuyMinArrowIdx = curEa.fakeBuyStrategy.arrMinuteIdx[p];
                                    nFakeBuyOverlapCount = 0;
                                    sFakeBuyArrowToolTip = "";
                                }

                                // 매도 어노테이션 삽입
                                ArrowAnnotation arrowFakeBuy = new ArrowAnnotation();
                                if (!isArrowGrouping)
                                {
                                    arrowFakeBuy.Width = -2; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }
                                else
                                {
                                    arrowFakeBuy.Width = -0.7; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세

                                }
                                arrowFakeBuy.Height = -4;// -면 아래쪽방향, + 면 위쪽방향
                                arrowFakeBuy.AnchorOffsetY = -1.5;
                                sFakeBuyArrowToolTip +=
                                $"*중첩 : {nFakeBuyOverlapCount + 1}( {p} )  가짜전략 : {curEa.fakeBuyStrategy.arrSpecificStrategy[p]}  주문시간 : {curEa.fakeBuyStrategy.arrBuyTime[p]}  페이크매수가격 : {curEa.fakeBuyStrategy.arrBuyPrice[p]}(원){NEW_LINE}" +
                                $"페이크명 : {strategyNames.arrFakeBuyStrategyName[curEa.fakeBuyStrategy.arrSpecificStrategy[p]]}{NEW_LINE}{NEW_LINE}";

                                arrowFakeBuy.ToolTip = $"*페이크매수 총 갯수 : {nFakeBuyOverlapCount + 1}개\n" +
                                   $"=====================================================\n" + sFakeBuyArrowToolTip;
                                if (nFakeBuyOverlapCount == 0)
                                    arrowFakeBuy.Height = -4;
                                else
                                {
                                    historyChart.Annotations.RemoveAt(historyChart.Annotations.Count - 1);
                                    arrowFakeBuy.Height = -7;
                                }
                                arrowFakeBuy.BackColor = Color.Orange;
                                arrowFakeBuy.SetAnchor(historyChart.Series["MinuteStick"].Points[curEa.fakeBuyStrategy.arrMinuteIdx[p]]);
                                // arrowFakeBuy.AnchorY = historyChart.Series["MinuteStick"].Points[curEa.fakeBuyStrategy.arrMinuteIdx[p]].YValues[1]; // 고.저.시종
                                arrowFakeBuy.Name = "F" + nNumInjector;
                                arrowFakeBuy.LineColor = Color.Black;

                                historyChart.Annotations.Add(arrowFakeBuy);
                            }
                        }//END ---- FAKE BUY ARROW


                        //START ---- FAKE ASSISTANT ARROW
                        if (curEa.fakeAssistantStrategy.nStrategyNum > 0 && isFakeAssistantArrowVisible)
                        {
                            int nPrevFakeAssistantMinArrowIdx = -1;
                            int nFakeAssistantOverlapCount = 0;
                            string sFakeAssistantArrowToolTip = "";

                            for (int p = 0; p < curEa.fakeAssistantStrategy.nStrategyNum; p++)
                            {
                                nNumInjector++;
                                if (nPrevFakeAssistantMinArrowIdx == curEa.fakeAssistantStrategy.arrMinuteIdx[p]) // 같은 minute Idx랑 겹친다면
                                {
                                    nFakeAssistantOverlapCount++;
                                }
                                else
                                {
                                    nPrevFakeAssistantMinArrowIdx = curEa.fakeAssistantStrategy.arrMinuteIdx[p];
                                    nFakeAssistantOverlapCount = 0;
                                    sFakeAssistantArrowToolTip = "";
                                }

                                // 매도 어노테이션 삽입
                                ArrowAnnotation arrowFakeAssistant = new ArrowAnnotation();
                                if (!isArrowGrouping)
                                {
                                    arrowFakeAssistant.Width = -1; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }
                                else
                                {
                                    arrowFakeAssistant.Width = -0.4; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }

                                arrowFakeAssistant.Height = -4;// -면 아래쪽방향, + 면 위쪽방향
                                arrowFakeAssistant.AnchorOffsetY = -1.5;

                                sFakeAssistantArrowToolTip +=
                              $"*중첩 : {nFakeAssistantOverlapCount + 1}( {p} )  가짜전략 : {curEa.fakeAssistantStrategy.arrSpecificStrategy[p]}  주문시간 : {curEa.fakeAssistantStrategy.arrBuyTime[p]}  페이크보조가격 : {curEa.fakeAssistantStrategy.arrBuyPrice[p]}(원){NEW_LINE}" +
                              $"페이크명 : {strategyNames.arrFakeAssistantStrategyName[curEa.fakeAssistantStrategy.arrSpecificStrategy[p]]}{NEW_LINE}{NEW_LINE}";

                                arrowFakeAssistant.ToolTip =
                                $"*페이크보조 총 갯수 : {nFakeAssistantOverlapCount + 1}개\n" +
                                $"주문인덱스 : {curEa.fakeAssistantStrategy.arrMinuteIdx[p]}\n" +
                                   $"=====================================================\n" + sFakeAssistantArrowToolTip;
                                if (nFakeAssistantOverlapCount == 0)
                                    arrowFakeAssistant.Height = -4;
                                else
                                {
                                    historyChart.Annotations.RemoveAt(historyChart.Annotations.Count - 1);
                                    arrowFakeAssistant.Height = -7;
                                }
                                arrowFakeAssistant.SetAnchor(historyChart.Series["MinuteStick"].Points[curEa.fakeAssistantStrategy.arrMinuteIdx[p]]);
                                // arrowFakeAssistant.AnchorY = historyChart.Series["MinuteStick"].Points[curEa.fakeAssistantStrategy.arrMinuteIdx[p]].YValues[1]; // 고.저.시종
                                arrowFakeAssistant.Name = "F" + nNumInjector;
                                arrowFakeAssistant.BackColor = Color.Yellow;
                                arrowFakeAssistant.LineColor = Color.Black;

                                historyChart.Annotations.Add(arrowFakeAssistant);
                            }
                        }//END ---- FAKE ASSISTANT ARROWz


                        //START ---- FAKE RESIST ARROW
                        if (curEa.fakeResistStrategy.nStrategyNum > 0 && isFakeResistArrowVisible)
                        {
                            int nPrevFakeResistMinArrowIdx = -1;
                            int nFakeResistOverlapCount = 0;
                            string sFakeResistArrowToolTip = "";

                            for (int p = 0; p < curEa.fakeResistStrategy.nStrategyNum; p++)
                            {
                                nNumInjector++;
                                if (nPrevFakeResistMinArrowIdx == curEa.fakeResistStrategy.arrMinuteIdx[p]) // 같은 minute Idx랑 겹친다면
                                {
                                    nFakeResistOverlapCount++;
                                }
                                else
                                {
                                    nPrevFakeResistMinArrowIdx = curEa.fakeResistStrategy.arrMinuteIdx[p];
                                    nFakeResistOverlapCount = 0;
                                    sFakeResistArrowToolTip = "";
                                }

                                // 매도 어노테이션 삽입
                                ArrowAnnotation arrowFakeResist = new ArrowAnnotation();
                                if (!isArrowGrouping)
                                {
                                    arrowFakeResist.Width = 0; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }
                                else
                                {
                                    arrowFakeResist.Width = -0.1; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세\
                                }
                                arrowFakeResist.Height = -4;// -면 아래쪽방향, + 면 위쪽방향
                                arrowFakeResist.AnchorOffsetY = -1.5;

                                sFakeResistArrowToolTip +=
                              $"*중첩 : {nFakeResistOverlapCount + 1}( {p} )  가짜전략 : {curEa.fakeResistStrategy.arrSpecificStrategy[p]}  주문시간 : {curEa.fakeResistStrategy.arrBuyTime[p]}  페이크저항가격 : {curEa.fakeResistStrategy.arrBuyPrice[p]}(원){NEW_LINE}" +
                              $"페이크명 : {strategyNames.arrFakeResistStrategyName[curEa.fakeResistStrategy.arrSpecificStrategy[p]]}{NEW_LINE}{NEW_LINE}";

                                arrowFakeResist.ToolTip = $"주문인덱스 : {curEa.fakeResistStrategy.arrMinuteIdx[p]}\n" +
                                    $"*페이크저항 총 갯수 : {nFakeResistOverlapCount + 1}개\n" +
                                   $"=====================================================\n" + sFakeResistArrowToolTip;
                                if (nFakeResistOverlapCount == 0)
                                    arrowFakeResist.Height = -4;
                                else
                                {
                                    historyChart.Annotations.RemoveAt(historyChart.Annotations.Count - 1);
                                    arrowFakeResist.Height = -7;
                                }
                                arrowFakeResist.SetAnchor(historyChart.Series["MinuteStick"].Points[curEa.fakeResistStrategy.arrMinuteIdx[p]]);
                                // arrowFakeResist.AnchorY = historyChart.Series["MinuteStick"].Points[curEa.fakeResistStrategy.arrMinuteIdx[p]].YValues[1]; // 고.저.시종
                                arrowFakeResist.Name = "F" + nNumInjector;
                                arrowFakeResist.BackColor = Color.Green;
                                arrowFakeResist.LineColor = Color.Black;

                                historyChart.Annotations.Add(arrowFakeResist);
                            }
                        }//END ---- FAKE RESIST ARROW


                        //START ---- PAPER SELL ARROW
                        if (curEa.paperBuyStrategy.nStrategyNum > 0 && isPaperSellArrowVisible)
                        {
                            for (int p = 0; p < curEa.paperBuyStrategy.nStrategyNum; p++)
                            {
                                if (nSpecificStrategyIdx != null && curEa.paperBuyStrategy.arrSpecificStrategy[p] != nSpecificStrategyIdx) // 특정한 전략만 보여주게 설정했다면
                                    continue; // 특정전략인덱스가 아니면 건너뛴다.

                                if (nCurPaperBuyedId != -1 && p != nCurPaperBuyedId)
                                    continue;

                                if (!curEa.paperBuyStrategy.paperTradeSlot[p].isAllSelled)
                                    continue;


                                nNumInjector++;

                                int nPaperSellAnnotationIdx = curEa.paperBuyStrategy.paperTradeSlot[p].nSellRqTimeLineIdx;
                                string sPaperSellArrowToolTip = "";


                                if (curEa.paperBuyStrategy.paperTradeSlot[p].nBuyedVolume == 0) // 전량 매수취소가 된 상황 
                                {
                                    sPaperSellArrowToolTip +=
                                        $"[매수취소] 매수요청시간 : {curEa.paperBuyStrategy.paperTradeSlot[p].nRqTime}  매도요청시간 : {curEa.paperBuyStrategy.paperTradeSlot[p].nSellRqTime}{NEW_LINE}" +
                                        $"매매블록ID : {p}  주문수량 : " + curEa.paperBuyStrategy.paperTradeSlot[p].nRqVolume + "(주)\n" +
                                        "매도설명 : " + curEa.paperBuyStrategy.paperTradeSlot[p].sSellDescription + "\n\n";

                                }
                                else // 일부 매수취소 + 정상 매수
                                {
                                    sPaperSellArrowToolTip +=
                                         $"매도요청시간 : {curEa.paperBuyStrategy.paperTradeSlot[p].nSellRqTime}  체결시간 : {curEa.paperBuyStrategy.paperTradeSlot[p].nSellEndTime}  손익률 : {Math.Round(mainForm.GetProfitPercent(curEa.paperBuyStrategy.paperTradeSlot[p].nBuyedPrice * curEa.paperBuyStrategy.paperTradeSlot[p].nBuyedVolume, curEa.paperBuyStrategy.paperTradeSlot[p].nSellEndPrice * curEa.paperBuyStrategy.paperTradeSlot[p].nSellEndVolume, curEa.nMarketGubun), 2)} (%)\n" +
                                         $"매도블록ID : {p}  주문가격(수량) : {curEa.paperBuyStrategy.paperTradeSlot[p].nSellRqPrice}(원)({curEa.paperBuyStrategy.paperTradeSlot[p].nSellRqVolume}),  매도가격(수량) : {curEa.paperBuyStrategy.paperTradeSlot[p].nSellEndPrice}({curEa.paperBuyStrategy.paperTradeSlot[p].nSellEndVolume})\n" +
                                         "매도설명 : " + curEa.paperBuyStrategy.paperTradeSlot[p].sSellDescription + "\n\n";
                                }


                                if (realDictionary.ContainsKey(nPaperSellAnnotationIdx)) // 해당위치(같은분봉) 에 값이 들어있다면
                                {
                                    realDictionary[nPaperSellAnnotationIdx].nCount++;
                                    realDictionary[nPaperSellAnnotationIdx].sTooltipMessage += sPaperSellArrowToolTip;
                                }
                                else // 값이 없다면 => 1번째 데이터
                                {
                                    realDictionary[nPaperSellAnnotationIdx] = new RealAnnotationInfo(1, sPaperSellArrowToolTip);
                                }


                                // 매도 어노테이션 삽입
                                ArrowAnnotation arrowPaperSell = new ArrowAnnotation();
                                if (!isArrowGrouping)
                                {
                                    arrowPaperSell.Width = 1; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }
                                else
                                {
                                    arrowPaperSell.Width = 0.2; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세

                                }
                                arrowPaperSell.Height = -4;// -면 아래쪽방향, + 면 위쪽방향
                                arrowPaperSell.AnchorOffsetY = -1.5;


                                arrowPaperSell.ToolTip = $"*모의매도 총 갯수 : {realDictionary[nPaperSellAnnotationIdx].nCount}개\n" +
                                   $"=====================================================\n" + realDictionary[nPaperSellAnnotationIdx].sTooltipMessage;

                                if (realDictionary[nPaperSellAnnotationIdx].nCount == 1)
                                    arrowPaperSell.Height = -4;
                                else
                                {
                                    for (int k = 0; k < historyChart.Annotations.Count; k++) // 어노테이션들 중
                                    {
                                        if (historyChart.Annotations[k].Name.Equals("P" + realDictionary[nPaperSellAnnotationIdx].nLastAnnotationLoc))  // P + 해당분봉의 최근삽입정보
                                        {
                                            historyChart.Annotations.RemoveAt(k);
                                        }
                                    }
                                    arrowPaperSell.Height = -7;
                                }

                                realDictionary[nPaperSellAnnotationIdx].nLastAnnotationLoc = nNumInjector;  // 최근 삽입시점 삽입

                                arrowPaperSell.BackColor = Color.Blue;
                                arrowPaperSell.SetAnchor(historyChart.Series["MinuteStick"].Points[curEa.paperBuyStrategy.paperTradeSlot[p].nSellRqTimeLineIdx]);
                                // arrowFakeBuy.AnchorY = historyChart.Series["MinuteStick"].Points[curEa.fakeBuyStrategy.arrMinuteIdx[p]].YValues[1]; // 고.저.시종
                                arrowPaperSell.Name = "P" + nNumInjector;
                                arrowPaperSell.LineColor = Color.Black;

                                historyChart.Annotations.Add(arrowPaperSell);
                            }
                        }//END ---- PAPER SELL ARROW
                        realDictionary.Clear(); // 모의매도 어노테이션 설정후 클리어

                        //START ---- FAKE Volatility Arrow
                        if (curEa.fakeVolatilityStrategy.nStrategyNum > 0 && isFakeVolatilityArrowVisible)
                        {
                            int nPrevFakeVolatilityMinArrowIdx = -1;
                            int nFakeVolatilityOverlapCount = 0;
                            string sFakeVolatilityArrowToolTip = "";

                            for (int p = 0; p < curEa.fakeVolatilityStrategy.nStrategyNum; p++)
                            {
                                nNumInjector++;
                                if (nPrevFakeVolatilityMinArrowIdx == curEa.fakeVolatilityStrategy.arrMinuteIdx[p]) // 같은 minute Idx랑 겹친다면
                                {
                                    nFakeVolatilityOverlapCount++;
                                }
                                else
                                {
                                    nPrevFakeVolatilityMinArrowIdx = curEa.fakeVolatilityStrategy.arrMinuteIdx[p];
                                    nFakeVolatilityOverlapCount = 0;
                                    sFakeVolatilityArrowToolTip = "";
                                }

                                // 매도 어노테이션 삽입
                                ArrowAnnotation arrowFakeVolatility = new ArrowAnnotation();
                                if (!isArrowGrouping)
                                {
                                    arrowFakeVolatility.Width = 2; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }
                                else
                                {
                                    arrowFakeVolatility.Width = 0.5; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세

                                }
                                arrowFakeVolatility.Height = -4;// -면 아래쪽방향, + 면 위쪽방향
                                arrowFakeVolatility.AnchorOffsetY = -1.5;
                                sFakeVolatilityArrowToolTip +=
                                $"*중첩 : {nFakeVolatilityOverlapCount + 1}( {p} )  변동성전략 : {curEa.fakeVolatilityStrategy.arrSpecificStrategy[p]}  주문시간 : {curEa.fakeVolatilityStrategy.arrBuyTime[p]}  변동성가격 : {curEa.fakeVolatilityStrategy.arrBuyPrice[p]}(원){NEW_LINE}" +
                                $"변동성명 : {strategyNames.arrFakeVolatilityStrategyName[curEa.fakeVolatilityStrategy.arrSpecificStrategy[p]]}{NEW_LINE}{NEW_LINE}";

                                arrowFakeVolatility.ToolTip = $"*변동성 총 갯수 : {nFakeVolatilityOverlapCount + 1}개\n" +
                                   $"=====================================================\n" + sFakeVolatilityArrowToolTip;
                                if (nFakeVolatilityOverlapCount == 0)
                                    arrowFakeVolatility.Height = -4;
                                else
                                {
                                    historyChart.Annotations.RemoveAt(historyChart.Annotations.Count - 1);
                                    arrowFakeVolatility.Height = -7;
                                }
                                arrowFakeVolatility.BackColor = Color.Navy;
                                arrowFakeVolatility.SetAnchor(historyChart.Series["MinuteStick"].Points[curEa.fakeVolatilityStrategy.arrMinuteIdx[p]]);

                                arrowFakeVolatility.Name = "F" + nNumInjector;
                                arrowFakeVolatility.LineColor = Color.Black;

                                historyChart.Annotations.Add(arrowFakeVolatility);
                            }
                        }//END ---- FAKE Volatility ARROW


                        //START ---- FAKE Down Arrow
                        if (curEa.fakeDownStrategy.nStrategyNum > 0 && isFakeDownArrowVisible)
                        {
                            int nPrevFakeDownMinArrowIdx = -1;
                            int nFakeDownOverlapCount = 0;
                            string sFakeDownArrowToolTip = "";

                            for (int p = 0; p < curEa.fakeDownStrategy.nStrategyNum; p++)
                            {
                                nNumInjector++;
                                if (nPrevFakeDownMinArrowIdx == curEa.fakeDownStrategy.arrMinuteIdx[p]) // 같은 minute Idx랑 겹친다면
                                {
                                    nFakeDownOverlapCount++;
                                }
                                else
                                {
                                    nPrevFakeDownMinArrowIdx = curEa.fakeDownStrategy.arrMinuteIdx[p];
                                    nFakeDownOverlapCount = 0;
                                    sFakeDownArrowToolTip = "";
                                }

                                // 매도 어노테이션 삽입
                                ArrowAnnotation arrowFakeDown = new ArrowAnnotation();
                                if (!isArrowGrouping)
                                {
                                    arrowFakeDown.Width = 3; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }
                                else
                                {
                                    arrowFakeDown.Width = 0.8; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세

                                }
                                arrowFakeDown.Height = -4;// -면 아래쪽방향, + 면 위쪽방향
                                arrowFakeDown.AnchorOffsetY = -1.5;
                                sFakeDownArrowToolTip +=
                                $"*중첩 : {nFakeDownOverlapCount + 1}( {p} )  페이크 다운전략 : {curEa.fakeDownStrategy.arrSpecificStrategy[p]}  주문시간 : {curEa.fakeDownStrategy.arrBuyTime[p]}  페이크 다운 가격 : {curEa.fakeDownStrategy.arrBuyPrice[p]}(원){NEW_LINE}" +
                                $"페이크 다운명 : {strategyNames.arrFakeDownStrategyName[curEa.fakeDownStrategy.arrSpecificStrategy[p]]}{NEW_LINE}{NEW_LINE}";

                                arrowFakeDown.ToolTip = $"*페이크 다운 총 갯수 : {nFakeDownOverlapCount + 1}개\n" +
                                   $"=====================================================\n" + sFakeDownArrowToolTip;
                                if (nFakeDownOverlapCount == 0)
                                    arrowFakeDown.Height = -4;
                                else
                                {
                                    historyChart.Annotations.RemoveAt(historyChart.Annotations.Count - 1);
                                    arrowFakeDown.Height = -7;
                                }
                                arrowFakeDown.BackColor = Color.Purple;
                                arrowFakeDown.SetAnchor(historyChart.Series["MinuteStick"].Points[curEa.fakeDownStrategy.arrMinuteIdx[p]]);

                                arrowFakeDown.Name = "F" + nNumInjector;
                                arrowFakeDown.LineColor = Color.Black;

                                historyChart.Annotations.Add(arrowFakeDown);
                            }
                        }//END ---- FAKE Down ARROW


                        //START ---- REAL BUY ARROW
                        for (int buyId = 0; buyId < curEa.myTradeManager.arrBuyedSlots.Count; buyId++)
                        {
                            nNumInjector++;

                            if (curEa.myTradeManager.arrBuyedSlots[buyId].isAllBuyed && isBuyArrowVisible) // 다 사졌고 접근가능하면
                            {
                                if (nCurRealBuyedId != -1 && buyId != nCurRealBuyedId)
                                    continue;

                                int nBuyAnnotationIdx = curEa.myTradeManager.arrBuyedSlots[buyId].nBuyMinuteIdx;
                                string sRealBuyMessage = "";

                                if (curEa.myTradeManager.arrBuyedSlots[buyId].isCopied)
                                    sRealBuyMessage += "[복제본]";

                                sRealBuyMessage +=
                                     $"매수요청시간 : {curEa.myTradeManager.arrBuyedSlots[buyId].nRequestTime}  접수시간 : {curEa.myTradeManager.arrBuyedSlots[buyId].nReceiptTime}  체결시간 : {curEa.myTradeManager.arrBuyedSlots[buyId].nBuyEndTime}\n" +
                                     $"매수블록ID : {buyId}  주문가격(수량) : {curEa.myTradeManager.arrBuyedSlots[buyId].nOriginOrderPrice}(원)({curEa.myTradeManager.arrBuyedSlots[buyId].nOrderVolume}),  매수가격(수량) : {curEa.myTradeManager.arrBuyedSlots[buyId].nBuyPrice}({curEa.myTradeManager.arrBuyedSlots[buyId].nBuyVolume})\n" +
                                     "매수설명 : " + curEa.myTradeManager.arrBuyedSlots[buyId].sBuyDescription + "\n\n";


                                if (realDictionary.ContainsKey(nBuyAnnotationIdx)) // 해당위치(같은분봉) 에 값이 들어있다면
                                {
                                    realDictionary[nBuyAnnotationIdx].nCount++;
                                    realDictionary[nBuyAnnotationIdx].sTooltipMessage += sRealBuyMessage;
                                }
                                else // 값이 없다면 => 1번째 데이터
                                {
                                    realDictionary[nBuyAnnotationIdx] = new RealAnnotationInfo(1, sRealBuyMessage);
                                }

                                // 매수 어노테이션 삽입
                                ArrowAnnotation arrowBuy = new ArrowAnnotation();
                                if (!isArrowGrouping)
                                {
                                    arrowBuy.Width = -3;
                                }
                                else
                                {
                                    arrowBuy.Width = -1; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }
                                arrowBuy.AnchorOffsetY = +1.5;


                                arrowBuy.ToolTip =
                                    $"*실매수 총 갯수 : {realDictionary[nBuyAnnotationIdx].nCount}개\n" +
                                    $"=====================================================\n" + realDictionary[nBuyAnnotationIdx].sTooltipMessage;

                                if (realDictionary[nBuyAnnotationIdx].nCount == 1) // 해당데이터가 첫번째면은
                                    arrowBuy.Height = +4;
                                else
                                {
                                    for (int k = 0; k < historyChart.Annotations.Count; k++) // 어노테이션들 중
                                    {
                                        if (historyChart.Annotations[k].Name.Equals("M" + realDictionary[nBuyAnnotationIdx].nLastAnnotationLoc))  // M + 해당분봉의 최근삽입정보
                                        {
                                            historyChart.Annotations.RemoveAt(k);
                                        }
                                    }
                                    arrowBuy.Height = +7;
                                }

                                realDictionary[nBuyAnnotationIdx].nLastAnnotationLoc = nNumInjector;  // 최근 삽입시점 삽입

                                arrowBuy.BackColor = Color.HotPink;
                                arrowBuy.LineColor = Color.Black;

                                if (nBuyAnnotationIdx >= historyChart.Series["MinuteStick"].Points.Count)
                                    nBuyAnnotationIdx = historyChart.Series["MinuteStick"].Points.Count - 1;

                                arrowBuy.SetAnchor(historyChart.Series["MinuteStick"].Points[nBuyAnnotationIdx]);
                                arrowBuy.AnchorY = historyChart.Series["MinuteStick"].Points[nBuyAnnotationIdx].YValues[1]; // 고.저.시종
                                arrowBuy.Name = "M" + nNumInjector;

                                historyChart.Annotations.Add(arrowBuy);

                            } // END ---- 다 사졌다면
                        }// END ---- REAL BUY ARROW
                        realDictionary.Clear(); // 실매수 어노테이션 설정후 클리어

                        //START ---- REAL SELL ARROW
                        for (int sellId = 0; sellId < curEa.myTradeManager.arrBuyedSlots.Count; sellId++)
                        {
                            nNumInjector++;

                            if (curEa.myTradeManager.arrBuyedSlots[sellId].isAllSelled && isSellArrowVisible) // 다 팔렸다면
                            {
                                if (nCurRealBuyedId != -1 && sellId != nCurRealBuyedId)
                                    continue;

                                int nSellAnnotaionIdx = curEa.myTradeManager.arrBuyedSlots[sellId].nSellMinuteIdx;
                                string sRealSellMessage = "";

                                if (curEa.myTradeManager.arrBuyedSlots[sellId].isCopied)
                                    sRealSellMessage += "[복제본]";

                                sRealSellMessage +=
                                    $"매도시간 : {curEa.myTradeManager.arrBuyedSlots[sellId].nDeathTime}  총손익금 : {mainForm.GetProfitPrice(curEa.myTradeManager.arrBuyedSlots[sellId].nBuyedSumPrice, curEa.myTradeManager.arrBuyedSlots[sellId].nSelledSumPrice, curEa.nMarketGubun)} (원)  손익률 : {Math.Round(mainForm.GetProfitPercent(curEa.myTradeManager.arrBuyedSlots[sellId].nBuyedSumPrice, curEa.myTradeManager.arrBuyedSlots[sellId].nSelledSumPrice, curEa.nMarketGubun), 2)}(%)\n" +
                                    $"매도블록ID : {sellId}  주문가격(수량) : {curEa.myTradeManager.arrBuyedSlots[sellId].nBuyPrice}(원)({curEa.myTradeManager.arrBuyedSlots[sellId].nOrderVolume}),  매도가격(수량) : {curEa.myTradeManager.arrBuyedSlots[sellId].nDeathPrice}(원)({curEa.myTradeManager.arrBuyedSlots[sellId].nSellVolume})\n" +
                                    "매도설명 : " + curEa.myTradeManager.arrBuyedSlots[sellId].sSellDescription + "\n\n";



                                if (realDictionary.ContainsKey(nSellAnnotaionIdx))  // 해당 분봉에 정보가 있다면 => 덮어씌우기
                                {
                                    realDictionary[nSellAnnotaionIdx].nCount++;
                                    realDictionary[nSellAnnotaionIdx].sTooltipMessage += sRealSellMessage;
                                }
                                else // 해당 분봉에 정보가 없다면 => 처음 등장
                                {
                                    realDictionary[nSellAnnotaionIdx] = new RealAnnotationInfo(1, sRealSellMessage);
                                }


                                // 매도 어노테이션 삽입
                                ArrowAnnotation arrowSell = new ArrowAnnotation();
                                if (!isArrowGrouping)
                                {
                                    arrowSell.Width = 1;
                                }
                                else
                                {
                                    arrowSell.Width = +0.2; // -가 왼쪽으로 기움, + 가 오른쪽으로 기움 , 0이 수직자세
                                }

                                arrowSell.Height = +4;// -면 아래쪽방향, + 면 위쪽방향
                                arrowSell.AnchorOffsetY = +1.5;



                                arrowSell.ToolTip =
                                    $"*실매도 총 갯수 : {realDictionary[nSellAnnotaionIdx].nCount}개\n" +
                                   $"=====================================================\n" + realDictionary[nSellAnnotaionIdx].sTooltipMessage;

                                if (realDictionary[nSellAnnotaionIdx].nCount == 1)
                                    arrowSell.Height = +4;
                                else
                                {
                                    for (int k = 0; k < historyChart.Annotations.Count; k++)
                                    {
                                        if (historyChart.Annotations[k].Name.Equals("M" + realDictionary[nSellAnnotaionIdx].nLastAnnotationLoc))
                                        {
                                            historyChart.Annotations.RemoveAt(k);
                                        }
                                    }
                                    arrowSell.Height = +7;
                                }

                                realDictionary[nSellAnnotaionIdx].nLastAnnotationLoc = nNumInjector; // 최근 인덱스정보 삽입
                                arrowSell.BackColor = Color.LightSkyBlue;
                                arrowSell.LineColor = Color.Black;

                                if (nSellAnnotaionIdx >= historyChart.Series["MinuteStick"].Points.Count)
                                    nSellAnnotaionIdx = historyChart.Series["MinuteStick"].Points.Count - 1;

                                arrowSell.SetAnchor(historyChart.Series["MinuteStick"].Points[nSellAnnotaionIdx]);
                                arrowSell.AnchorY = historyChart.Series["MinuteStick"].Points[nSellAnnotaionIdx].YValues[1]; // 고.저.시종

                                arrowSell.Name = "M" + nNumInjector;
                                historyChart.Annotations.Add(arrowSell);
                            }// END ---- 다 팔렸다면
                        }//END ---- REAL SELL ARROW
                        realDictionary.Clear(); // 실매도 어노테이션 설정후 클리어


                        if (curEa.myTradeManager.nAppliedShowingRealBuyedId != -1)
                        {
                            curMyProfitLabel.Text = $"{curEa.myTradeManager.nAppliedShowingRealBuyedId}번째 블록 : {Math.Round(curEa.myTradeManager.arrBuyedSlots[curEa.myTradeManager.nAppliedShowingRealBuyedId].fPowerWithFee * 100, 2)}(%)";
                            curCheckLineIdxLabel.Text = $"블록 체크라인 : {(curEa.myTradeManager.arrBuyedSlots[curEa.myTradeManager.nAppliedShowingRealBuyedId].nCheckLineIdx - MainForm.MIDDLE_STEP > 0 ? "+" : "")}{curEa.myTradeManager.arrBuyedSlots[curEa.myTradeManager.nAppliedShowingRealBuyedId].nCheckLineIdx - MainForm.MIDDLE_STEP} ({curEa.myTradeManager.arrBuyedSlots[curEa.myTradeManager.nAppliedShowingRealBuyedId].nMaxCheckLineIdx - MainForm.MIDDLE_STEP} , {curEa.myTradeManager.arrBuyedSlots[curEa.myTradeManager.nAppliedShowingRealBuyedId].nMinCheckLineIdx - MainForm.MIDDLE_STEP})";
                        }
                        else
                        {
                            if (!curMyProfitLabel.Text.Equals(""))
                                curMyProfitLabel.Text = "";
                            if (!curCheckLineIdxLabel.Text.Equals(""))
                                curCheckLineIdxLabel.Text = "";
                        }

                        priceLabel.Text = $"현재가격 : ({curEa.nFs}, {curEa.nFb})";
                        powerLabel.Text = $"현재파워 : {Math.Round(curEa.fPower, 3)}";
                        curSpeedLabel.Text = $"속도 : {Math.Round(curEa.speedStatus.fCur, 2)}";
                        curPureTradePriceLabel.Text = $"매수 : {Math.Round(curEa.pureTradeStatus.fCur, 2)}";
                        curTradePriceLabel.Text = $"대금 : {Math.Round(curEa.tradeStatus.fCur, 2)}";
                        curHogaRatioLabel.Text = $"호가비 : {Math.Round(curEa.hogaRatioStatus.fCur, 2)}";
                        curHitNumLabel.Text = $"히트 : {curEa.fakeStrategyMgr.nCurHitNum}";

                        if (curEa.isViMode)
                            isViLabel.Text = "VI";
                        else
                            isViLabel.Text = "";

                        fTradeComparedLabel.Text = $"전일대비 : {Math.Round(curEa.fTradeRatioCompared, 2)}";
                        fTradeStrengthLabel.Text = $"체결강도 : {Math.Round(curEa.fTs, 2)}";

                        SetChartViewRange(0, nLastMinuteIdx + 2, curEa.nFs, curEa.nFs, "TotalArea");
                    } // END ---- if (curEa.timeLines1m.nRealDataIdx > 0)
                }
                catch
                { }
            } // END --- voidDelegate

            if (historyChart.InvokeRequired)
                historyChart.Invoke(new MethodInvoker(voidDelegate));
            else
                voidDelegate();
        }
        #endregion 

        #region 변수&현재상황 출력
        public void ShowVariables()
        {
            curEa = mainForm.ea[nCurIdx];
            string sTitle = $"[전체] {mainForm.nSharedTime} {curEa.sCode} {curEa.sCodeName} {curEa.sMarketGubunTag}";
            string sMessage =
                $"현재 매매횟수 : {curEa.paperBuyStrategy.nStrategyNum}{NEW_LINE}" +
                $"===================================================={NEW_LINE}" +

                $"시가총액 : {curEa.lMarketCap / (double)MainForm.HUNDRED_MILLION}(억원){NEW_LINE}" +
                $"* 총 거래대금 : {curEa.lTotalTradePrice / (double)MainForm.HUNDRED_MILLION}(억원){NEW_LINE}" +
                $"총 매수대금 : {curEa.lOnlyBuyPrice / (double)MainForm.HUNDRED_MILLION}(억원){NEW_LINE}" +
                $"총 매도대금 : {curEa.lOnlySellPrice / (double)MainForm.HUNDRED_MILLION}(억원){NEW_LINE}" +

                $"=====================  랭킹  ========================={NEW_LINE}" +
                $"----------------------- 총 -------------------------------{NEW_LINE}" +
                $"누적속도 순위 : {curEa.rankSystem.nAccumCountRanking}{NEW_LINE}" +
                $"시가총액 순위 : {curEa.rankSystem.nMarketCapRanking}{NEW_LINE}" +
                $"파워 랭킹 : {curEa.rankSystem.nPowerRanking}{NEW_LINE}" +
                $"총매수가 순위 : {curEa.rankSystem.nTotalBuyPriceRanking}{NEW_LINE}" +
                $"총매수량 순위 : {curEa.rankSystem.nTotalBuyVolumeRanking}{NEW_LINE}" +
                $"총체결가 순위 : {curEa.rankSystem.nTotalTradePriceRanking}{NEW_LINE}" +
                $"총체결량 순위 : {curEa.rankSystem.nTotalTradeVolumeRanking}{NEW_LINE}" +
                $"총 순위 : {curEa.rankSystem.nSummationRanking}위 ( {curEa.rankSystem.nSummationMove} ){NEW_LINE}" +
                $"----------------------- 분당 ------------------------------{NEW_LINE}" +
                $"분당 매수가 순위 : {curEa.rankSystem.nMinuteBuyPriceRanking}{NEW_LINE}" +
                $"분당 매수량 순위 : {curEa.rankSystem.nMinuteBuyVolumeRanking}{NEW_LINE}" +
                $"분당 속도 순위 : {curEa.rankSystem.nMinuteCountRanking}{NEW_LINE}" +
                $"분당 파워 순위 : {curEa.rankSystem.nMinutePowerRanking}{NEW_LINE}" +
                $"분당 체결가 순위 : {curEa.rankSystem.nMinuteTradePriceRanking}{NEW_LINE}" +
                $"분당 체결량 순위 : {curEa.rankSystem.nMinuteTradeVolumeRanking}{NEW_LINE}" +
                $"분당 업다운 순위 : {curEa.rankSystem.nMinuteUpDownRanking}{NEW_LINE}" +
                $"분당 순위 : {curEa.rankSystem.nMinuteSummationRanking}위{NEW_LINE}" +
                $"---------------------- 순위 홀드 --------------------{NEW_LINE}" +
                $"10홀드 : {curEa.rankSystem.nRankHold10}{NEW_LINE}" +
                $"20홀드 : {curEa.rankSystem.nRankHold20}{NEW_LINE}" +
                $"50홀드 : {curEa.rankSystem.nRankHold50}{NEW_LINE}" +
                $"100홀드 : {curEa.rankSystem.nRankHold100}{NEW_LINE}" +
                $"200홀드 : {curEa.rankSystem.nRankHold200}{NEW_LINE}{NEW_LINE}" +
                $"================= ARROW ============={NEW_LINE}" +
                $"페이크매수 : {curEa.fakeBuyStrategy.nStrategyNum}{NEW_LINE}" +
                $"페이크보조 : {curEa.fakeAssistantStrategy.nStrategyNum}{NEW_LINE}" +
                $"페이크저항 : {curEa.fakeResistStrategy.nStrategyNum}{NEW_LINE}" +
                $"페이크변동성 : {curEa.fakeVolatilityStrategy.nStrategyNum}{NEW_LINE}" +
                $"페이크다운 : {curEa.fakeDownStrategy.nStrategyNum}{NEW_LINE}" +
                $"총 Arrow : {curEa.fakeStrategyMgr.nTotalFakeCount}{NEW_LINE}" +
                $"총 ArrowMinute : {curEa.fakeStrategyMgr.nTotalFakeMinuteAreaNum}{NEW_LINE}{NEW_LINE}" +
                $"================= 분 봉 ============={NEW_LINE}" +
                $"1퍼캔들 : {curEa.timeLines1m.onePerCandleList.Count}{NEW_LINE}" +
                $"2퍼캔들 : {curEa.timeLines1m.twoPerCandleList.Count}{NEW_LINE}" +
                $"3퍼캔들 : {curEa.timeLines1m.threePerCandleList.Count}{NEW_LINE}" +
                $"4퍼캔들 : {curEa.timeLines1m.fourPerCandleList.Count}{NEW_LINE}" +
                $"아래캔들 : {curEa.timeLines1m.downCandleList.Count}{NEW_LINE}" +
                $"위꼬리 : {curEa.timeLines1m.upTailList.Count}{NEW_LINE}" +
                $"아래꼬리 : {curEa.timeLines1m.downTailList.Count}{NEW_LINE}" +

                $"=====================AI 점수========================={NEW_LINE}" +
                $"AI 5 시간 : {curEa.fakeStrategyMgr.nAI5Time}{NEW_LINE}" +
                $"AI 10 시간 : {curEa.fakeStrategyMgr.nAI10Time}{NEW_LINE}" +
                $"AI 15 시간 : {curEa.fakeStrategyMgr.nAI15Time}{NEW_LINE}" +
                $"AI 20 시간 : {curEa.fakeStrategyMgr.nAI20Time}{NEW_LINE}" +
                $"AI 30 시간 : {curEa.fakeStrategyMgr.nAI30Time}{NEW_LINE}" +
                $"AI 50 시간 : {curEa.fakeStrategyMgr.nAI50Time}{NEW_LINE}{NEW_LINE}" +

                $"=====================이동평균선========================={NEW_LINE}" +
                $"---------------------- 이평값 -------------------------{NEW_LINE}" +
                $"현재최저값 : {curEa.timeLines1m.arrTimeLine[curEa.timeLines1m.nRealDataIdx].nDownFs}{NEW_LINE}" +
                $"ma20m : {curEa.maOverN.fCurMa20m}{NEW_LINE}" +
                $"ma1h : {curEa.maOverN.fCurMa1h}{NEW_LINE}" +
                $"ma2h : {curEa.maOverN.fCurMa2h}{NEW_LINE}" +
                $"GapMa20m : {curEa.maOverN.fCurGapMa20m}{NEW_LINE}" +
                $"GapMa1h : {curEa.maOverN.fCurGapMa1h}{NEW_LINE}" +
                $"GapMa2h : {curEa.maOverN.fCurGapMa2h}{NEW_LINE}" +
                $"-------------------- 조건 --------------------{NEW_LINE}" +
                $"downFs > 20m : {(curEa.timeLines1m.arrTimeLine[curEa.timeLines1m.nRealDataIdx].nDownFs > curEa.maOverN.fCurMa20m)}{NEW_LINE}" +
                $"downFs > 1h : {(curEa.timeLines1m.arrTimeLine[curEa.timeLines1m.nRealDataIdx].nDownFs > curEa.maOverN.fCurMa1h)}{NEW_LINE}" +
                $"20m > 1h : {(curEa.maOverN.fCurMa20m > curEa.maOverN.fCurMa1h)}{NEW_LINE}" +
                $"1h > 2h : {(curEa.maOverN.fCurMa1h > curEa.maOverN.fCurMa2h)}{NEW_LINE}" +
                $"------------------ 다운 Good ----------------{NEW_LINE}" +
                $"다운20m : {curEa.maOverN.nDownCntMa20m}{NEW_LINE}" +
                $"다운1h : {curEa.maOverN.nDownCntMa1h}{NEW_LINE}" +
                $"다운2h : {curEa.maOverN.nDownCntMa2h}{NEW_LINE}" +
                $"------------------- 업 Bad ------------------{NEW_LINE}" +
                $"업20m : {curEa.maOverN.nUpCntMa20m}{NEW_LINE}" +
                $"업1h : {curEa.maOverN.nUpCntMa1h}{NEW_LINE}" +
                $"업2h : {curEa.maOverN.nUpCntMa2h}{NEW_LINE}{NEW_LINE}" +
                $"=====================  전고점  ========================={NEW_LINE}" +
                $"전고점 카운트 : {curEa.crushMinuteManager.nCurCnt}{NEW_LINE}" +
                $"전고점   업 : {curEa.crushMinuteManager.nUpCnt}{NEW_LINE}" +
                $"전고점 다운 : {curEa.crushMinuteManager.nDownCnt}{NEW_LINE}" +
                $"전고점 스페셜 다운 : {curEa.crushMinuteManager.nSpecialDownCnt}{NEW_LINE}{NEW_LINE}" +

                $"=====================  추세각도  ======================={NEW_LINE}" +
                $"초기각도 : {curEa.timeLines1m.fInitAngle}( {curEa.timeLines1m.fInitSlope} ){NEW_LINE}" +
                $"맥스각도 : {curEa.timeLines1m.fMaxAngle}( {curEa.timeLines1m.fMaxSlope} ){NEW_LINE}" +
                $"중위각도 : {curEa.timeLines1m.fTotalMedianAngle}( {curEa.timeLines1m.fTotalMedian} ){NEW_LINE}" +
                $"한시간각도 : {curEa.timeLines1m.fHourMedianAngle}( {curEa.timeLines1m.fHourMedian} ){NEW_LINE}" +
                $"최근각도 : {curEa.timeLines1m.fRecentMedianAngle}( {curEa.timeLines1m.fRecentMedian} ){NEW_LINE}" +
                $"차이각도 : {curEa.timeLines1m.fDAngle}{NEW_LINE}{NEW_LINE}" +

                $"===================== 실시간정보 ========================{NEW_LINE}" +
                $"최근 주식체결시간 : {curEa.nLastRecordTime}{NEW_LINE}" +
                $"접근가능 인덱스 : {curEa.timeLines1m.nRealDataIdx}{NEW_LINE}" +
                $"체결강도 : {curEa.fTs}{NEW_LINE}" +
                $"노무브 : {curEa.nNoMoveCount}{NEW_LINE}" +
                $"적은거래 : {curEa.nFewSpeedCount}{NEW_LINE}" +
                $"거래없음 : {curEa.nMissCount}{NEW_LINE}" +
                $"초기갭 : {curEa.fStartGap}{NEW_LINE}" +
                $"갭제외파워 : {curEa.fPowerWithoutGap}{NEW_LINE}" +
                $"호가비율 : {curEa.fHogaRatio}{NEW_LINE}" +
                $"마카07 : {curEa.fMinusCnt07}{NEW_LINE}" +
                $"마카09 : {curEa.fMinusCnt09}{NEW_LINE}" +
                $"플카07 : {curEa.fPlusCnt07}{NEW_LINE}" +
                $"플카09 : {curEa.fPlusCnt09}{NEW_LINE}" +
                $"현재파워 : {curEa.fPower}{NEW_LINE}" +
                $"파워항아리 : {curEa.fPowerJar}{NEW_LINE}" +
                $"유통 대 호가 : {curEa.fSharePerHoga}{NEW_LINE}" +
                $"유통 대 체결 : {curEa.fSharePerTrade}{NEW_LINE}" +
                $"호가 대 체결 : {curEa.fHogaPerTrade}{NEW_LINE}" +
                $"체결 대 순체결 : {curEa.fTradePerPure}{NEW_LINE}" +
                $"체결 누적빈도 : {curEa.nChegyulCnt}{NEW_LINE}" +
                $"호가 누적빈도 : {curEa.nHogaCnt}{NEW_LINE}" +
                $"----------------- 상태변수 --------------------{NEW_LINE}" +
                $"실시간 호가비율상태 : {curEa.hogaRatioStatus.fCur}{NEW_LINE}" +
                $"실시간 호가속도상태 : {curEa.hogaSpeedStatus.fCur}{NEW_LINE}" +
                $"실시간 순매수상태 : {curEa.pureBuyStatus.fCur}{NEW_LINE}" +
                $"실시간 순체결상태 : {curEa.pureTradeStatus.fCur}{NEW_LINE}" +
                $"실시간 속도상태 : {curEa.speedStatus.fCur}{NEW_LINE}" +
                $"실시간 호가총량상태 : {curEa.totalHogaVolumeStatus.fCur}{NEW_LINE}" +
                $"실시간 체결상태 : {curEa.tradeStatus.fCur}";
            new ScrollableMessageBox().Show(sMessage, sTitle);
        }


        public void ShowPaperBuy()
        {
            curEa = mainForm.ea[nCurIdx];
            string sTitle = "[모의매수 전략] " + mainForm.nSharedTime + " " + curEa.sCode + " " + curEa.sCodeName + " " + curEa.sMarketGubunTag;
            string sMessage = $"모의매수 전략 횟수 : {curEa.paperBuyStrategy.nStrategyNum}번{NEW_LINE}" +
                $"모의매수 전략 마지막 접근시각 : {curEa.paperBuyStrategy.nLastTouchTime}{NEW_LINE}" +
                $"모의매수 전략 최고 어깨점 : {curEa.paperBuyStrategy.nMaxShoulderPrice}(원){NEW_LINE}" +
                $"모의매수 전략 단순평균 어깨점 : {Math.Round(curEa.paperBuyStrategy.fEverageShoulderPrice, 2)}(원){NEW_LINE}" +
                $"모의매수 전략 어깨 상승횟수 : {curEa.paperBuyStrategy.nUpperCount}번{NEW_LINE}" +
                $"모의매수 전략 분포횟수 : {curEa.paperBuyStrategy.nMinuteLocationCount}번{NEW_LINE}" +
                $"공유 전략 분포횟수 : {curEa.fakeStrategyMgr.nSharedMinuteLocationCount}번{NEW_LINE}" +
                $"--------------------------------------------------{NEW_LINE}{NEW_LINE}";

            for (int i = 0; i < strategyNames.arrPaperBuyStrategyName.Count; i++)
            {
                sMessage += $"==================== {i}번째 모의매수 전략 ===================={NEW_LINE}" +
                    $"##전략명 : {strategyNames.arrPaperBuyStrategyName[i]}  !!!!!!!!!!!!{NEW_LINE}";

                if (curEa.paperBuyStrategy.arrStrategy[i] == 0)
                {
                    sMessage += $"--> 해당전략은 활성화되지 않았습니다.{NEW_LINE}";
                }
                else
                {
                    sMessage +=
                        $"--> 마지막 접근시각 : {curEa.paperBuyStrategy.arrLastTouch[i]}{NEW_LINE}" +
                        $"--> 접근 횟수 : {curEa.paperBuyStrategy.arrStrategy[i]}번{NEW_LINE}";
                }

                sMessage += $"{NEW_LINE}";
            }
            new ScrollableMessageBox().Show(sMessage, sTitle);
        }
        public void ShowFakeBuy()
        {
            curEa = mainForm.ea[nCurIdx];
            double fBuyFlexEverage = 0;
            if (curEa.fakeBuyStrategy.nStrategyNum > 0)
                fBuyFlexEverage = (double)curEa.fakeBuyStrategy.nSumShoulderPrice / curEa.fakeBuyStrategy.nStrategyNum;
            string sTitle = "[페이크매수 전략] " + mainForm.nSharedTime + " " + curEa.sCode + " " + curEa.sCodeName + " " + curEa.sMarketGubunTag;
            string sMessage = $"페이크매수 전략 횟수 : {curEa.fakeBuyStrategy.nStrategyNum}번{NEW_LINE}" +
                $"페이크매수 전략 마지막 접근시각 : {curEa.fakeBuyStrategy.nLastTouchTime}{NEW_LINE}" +
                $"페이크매수 전략 최고 어깨점 : {curEa.fakeBuyStrategy.nMaxShoulderPrice}(원){NEW_LINE}" +
                $"페이크매수 전략 단순평균 어깨점 : {Math.Round(curEa.fakeBuyStrategy.fEverageShoulderPrice, 2)}(원){NEW_LINE}" +
                $"페이크매수 전략 종합평균 어깨점 : {Math.Round(fBuyFlexEverage, 2)}(원){NEW_LINE}" +
                $"페이크매수 전략 어깨 상승횟수 : {curEa.fakeBuyStrategy.nUpperCount}번{NEW_LINE}" +
                $"페이크매수 전략 분포횟수 : {curEa.fakeBuyStrategy.nMinuteLocationCount}번{NEW_LINE}" +
                $"공유 전략 분포횟수 : {curEa.fakeStrategyMgr.nSharedMinuteLocationCount}번{NEW_LINE}" +
                $"--------------------------------------------------{NEW_LINE}{NEW_LINE}";

            for (int i = 0; i < strategyNames.arrFakeBuyStrategyName.Count; i++)
            {
                sMessage += $"==================== {i}번째 페이크매수 전략 ===================={NEW_LINE}" +
                    $"##전략명 : {strategyNames.arrFakeBuyStrategyName[i]}  !!!!!!!!!!!!{NEW_LINE}";

                if (curEa.fakeBuyStrategy.arrStrategy[i] == 0)
                {
                    sMessage += $"--> 해당전략은 활성화되지 않았습니다.{NEW_LINE}";
                }
                else
                {
                    sMessage +=
                        $"--> 마지막 접근시각 : {curEa.fakeBuyStrategy.arrLastTouch[i]}{NEW_LINE}" +
                        $"--> 접근 횟수 : {curEa.fakeBuyStrategy.arrStrategy[i]}번{NEW_LINE}";
                }

                sMessage += $"{NEW_LINE}";
            }


            new ScrollableMessageBox().Show(sMessage, sTitle);
        }
        public void ShowFakeAssistant()
        {
            curEa = mainForm.ea[nCurIdx];
            double fAssistantFlexEverage = 0;
            if (curEa.fakeAssistantStrategy.nStrategyNum > 0)
                fAssistantFlexEverage = (double)curEa.fakeAssistantStrategy.nSumShoulderPrice / curEa.fakeAssistantStrategy.nStrategyNum;
            string sTitle = "[페이크보조 전략] " + mainForm.nSharedTime + " " + curEa.sCode + " " + curEa.sCodeName + " " + curEa.sMarketGubunTag;
            string sMessage = $"페이크보조 전략 횟수 : {curEa.fakeAssistantStrategy.nStrategyNum}번{NEW_LINE}" +
                $"페이크보조 전략 마지막 접근시각 : {curEa.fakeAssistantStrategy.nLastTouchTime}{NEW_LINE}" +
                $"페이크보조 전략 최고 어깨점 : {curEa.fakeAssistantStrategy.nMaxShoulderPrice}(원){NEW_LINE}" +
                $"페이크보조 전략 단순평균 어깨점 : {Math.Round(curEa.fakeAssistantStrategy.fEverageShoulderPrice, 2)}(원){NEW_LINE}" +
                $"페이크보조 전략 종합평균 어깨점 : {Math.Round(fAssistantFlexEverage, 2)}(원){NEW_LINE}" +
                $"페이크보조 전략 어깨 상승횟수 : {curEa.fakeAssistantStrategy.nUpperCount}번{NEW_LINE}" +
                $"페이크보조 전략 분포횟수 : {curEa.fakeAssistantStrategy.nMinuteLocationCount}번{NEW_LINE}" +
                $"공유 전략 분포횟수 : {curEa.fakeStrategyMgr.nSharedMinuteLocationCount}번{NEW_LINE}" +
                $"--------------------------------------------------{NEW_LINE}{NEW_LINE}";

            for (int i = 0; i < strategyNames.arrFakeAssistantStrategyName.Count; i++)
            {
                sMessage += $"==================== {i}번째 페이크보조 전략 ===================={NEW_LINE}" +
                    $"##전략명 : {strategyNames.arrFakeAssistantStrategyName[i]}  !!!!!!!!!!!!{NEW_LINE}";

                if (curEa.fakeAssistantStrategy.arrStrategy[i] == 0)
                {
                    sMessage += $"--> 해당전략은 활성화되지 않았습니다.{NEW_LINE}";
                }
                else
                {
                    sMessage +=
                        $"--> 마지막 접근시각 : {curEa.fakeAssistantStrategy.arrLastTouch[i]}{NEW_LINE}" +
                        $"--> 접근 횟수 : {curEa.fakeAssistantStrategy.arrStrategy[i]}번{NEW_LINE}";
                }

                sMessage += $"{NEW_LINE}";
            }


            new ScrollableMessageBox().Show(sMessage, sTitle);
        }
        public void ShowFakeResist()
        {
            curEa = mainForm.ea[nCurIdx];
            double fResistFlexEverage = 0;
            if (curEa.fakeResistStrategy.nStrategyNum > 0)
                fResistFlexEverage = (double)curEa.fakeResistStrategy.nSumShoulderPrice / curEa.fakeResistStrategy.nStrategyNum;
            string sTitle = "[페이크저항 전략] " + mainForm.nSharedTime + " " + curEa.sCode + " " + curEa.sCodeName + " " + curEa.sMarketGubunTag;
            string sMessage = $"페이크저항 전략 횟수 : {curEa.fakeResistStrategy.nStrategyNum}번{NEW_LINE}" +
                $"페이크저항 전략 마지막 접근시각 : {curEa.fakeResistStrategy.nLastTouchTime}{NEW_LINE}" +
                $"페이크저항 전략 마지막 접근시각 : {curEa.fakeResistStrategy.nLastTouchTime}{NEW_LINE}" +
                $"페이크저항 전략 최고 어깨점 : {curEa.fakeResistStrategy.nMaxShoulderPrice}(원){NEW_LINE}" +
                $"페이크저항 전략 단순평균 어깨점 : {Math.Round(curEa.fakeResistStrategy.fEverageShoulderPrice, 2)}(원){NEW_LINE}" +
                $"페이크저항 전략 종합평균 어깨점 : {Math.Round(fResistFlexEverage, 2)}(원){NEW_LINE}" +
                $"페이크저항 전략 어깨 상승횟수 : {curEa.fakeResistStrategy.nUpperCount}번{NEW_LINE}" +
                $"페이크저항 전략 분포횟수 : {curEa.fakeResistStrategy.nMinuteLocationCount}번{NEW_LINE}" +
                $"공유 전략 분포횟수 : {curEa.fakeStrategyMgr.nSharedMinuteLocationCount}번{NEW_LINE}" +
                $"--------------------------------------------------{NEW_LINE}{NEW_LINE}";

            for (int i = 0; i < strategyNames.arrFakeResistStrategyName.Count; i++)
            {
                sMessage += $"==================== {i}번째 페이크저항 전략 ===================={NEW_LINE}" +
                    $"##전략명 : {strategyNames.arrFakeResistStrategyName[i]}  !!!!!!!!!!!!{NEW_LINE}";

                if (curEa.fakeResistStrategy.arrStrategy[i] == 0)
                {
                    sMessage += $"--> 해당전략은 활성화되지 않았습니다.{NEW_LINE}";
                }
                else
                {
                    sMessage +=
                        $"--> 마지막 접근시각 : {curEa.fakeResistStrategy.arrLastTouch[i]}{NEW_LINE}" +
                        $"--> 접근 횟수 : {curEa.fakeResistStrategy.arrStrategy[i]}번{NEW_LINE}";
                }

                sMessage += $"{NEW_LINE}";
            }


            new ScrollableMessageBox().Show(sMessage, sTitle);
        }
        public void ShowFakeVolatility()
        {
            curEa = mainForm.ea[nCurIdx];
            string sTitle = "[페이크 변동성 전략] " + mainForm.nSharedTime + " " + curEa.sCode + " " + curEa.sCodeName + " " + curEa.sMarketGubunTag;
            string sMessage = $"페이크 변동성 전략 횟수 : {curEa.fakeVolatilityStrategy.nStrategyNum}번{NEW_LINE}" +
                $"페이크 변동성 전략 마지막 접근시각 : {curEa.fakeVolatilityStrategy.nLastTouchTime}{NEW_LINE}" +
                $"페이크 변동성 전략 최고 어깨점 : {curEa.fakeVolatilityStrategy.nMaxShoulderPrice}(원){NEW_LINE}" +
                $"페이크 변동성 전략 단순평균 어깨점 : {Math.Round(curEa.fakeVolatilityStrategy.fEverageShoulderPrice, 2)}(원){NEW_LINE}" +
                $"페이크 변동성 전략 어깨 상승횟수 : {curEa.fakeVolatilityStrategy.nUpperCount}번{NEW_LINE}" +
                $"페이크 변동성 전략 분포횟수 : {curEa.fakeVolatilityStrategy.nMinuteLocationCount}번{NEW_LINE}" +
                $"공유 전략 분포횟수 : {curEa.fakeStrategyMgr.nSharedMinuteLocationCount}번{NEW_LINE}" +
                $"--------------------------------------------------{NEW_LINE}{NEW_LINE}";

            for (int i = 0; i < strategyNames.arrFakeVolatilityStrategyName.Count; i++)
            {
                sMessage += $"==================== {i}번째 페이크 변동성 전략 ===================={NEW_LINE}" +
                    $"##전략명 : {strategyNames.arrFakeVolatilityStrategyName[i]}  !!!!!!!!!!!!{NEW_LINE}";

                if (curEa.fakeVolatilityStrategy.arrStrategy[i] == 0)
                {
                    sMessage += $"--> 해당전략은 활성화되지 않았습니다.{NEW_LINE}";
                }
                else
                {
                    sMessage +=
                        $"--> 마지막 접근시각 : {curEa.fakeVolatilityStrategy.arrLastTouch[i]}{NEW_LINE}" +
                        $"--> 접근 횟수 : {curEa.fakeVolatilityStrategy.arrStrategy[i]}번{NEW_LINE}";
                }

                sMessage += $"{NEW_LINE}";
            }
            new ScrollableMessageBox().Show(sMessage, sTitle);
        }

        public void ShowFakeDown()
        {
            curEa = mainForm.ea[nCurIdx];
            string sTitle = "[페이크 다운 전략] " + mainForm.nSharedTime + " " + curEa.sCode + " " + curEa.sCodeName + " " + curEa.sMarketGubunTag;
            string sMessage = $"페이크 다운 전략 횟수 : {curEa.fakeDownStrategy.nStrategyNum}번{NEW_LINE}" +
                $"페이크 다운 전략 마지막 접근시각 : {curEa.fakeDownStrategy.nLastTouchTime}{NEW_LINE}" +
                $"페이크 다운 전략 최고 어깨점 : {curEa.fakeDownStrategy.nMaxShoulderPrice}(원){NEW_LINE}" +
                $"페이크 다운 전략 단순평균 어깨점 : {Math.Round(curEa.fakeDownStrategy.fEverageShoulderPrice, 2)}(원){NEW_LINE}" +
                $"페이크 다운 전략 어깨 상승횟수 : {curEa.fakeDownStrategy.nUpperCount}번{NEW_LINE}" +
                $"페이크 다운 전략 분포횟수 : {curEa.fakeDownStrategy.nMinuteLocationCount}번{NEW_LINE}" +
                $"공유 전략 분포횟수 : {curEa.fakeStrategyMgr.nSharedMinuteLocationCount}번{NEW_LINE}" +
                $"--------------------------------------------------{NEW_LINE}{NEW_LINE}";

            for (int i = 0; i < strategyNames.arrFakeDownStrategyName.Count; i++)
            {
                sMessage += $"==================== {i}번째 페이크 다운 전략 ===================={NEW_LINE}" +
                    $"##전략명 : {strategyNames.arrFakeDownStrategyName[i]}  !!!!!!!!!!!!{NEW_LINE}";

                if (curEa.fakeDownStrategy.arrStrategy[i] == 0)
                {
                    sMessage += $"--> 해당전략은 활성화되지 않았습니다.{NEW_LINE}";
                }
                else
                {
                    sMessage +=
                        $"--> 마지막 접근시각 : {curEa.fakeDownStrategy.arrLastTouch[i]}{NEW_LINE}" +
                        $"--> 접근 횟수 : {curEa.fakeDownStrategy.arrStrategy[i]}번{NEW_LINE}";
                }

                sMessage += $"{NEW_LINE}";
            }
            new ScrollableMessageBox().Show(sMessage, sTitle);
        }
        #endregion


        public void SelectedIndexChangedHandler(object sender, EventArgs e)
        {
            // Get the selected tab index
            int selectedIndex = tabControl1.SelectedIndex;

            // Perform actions based on the selected tab index
            switch (selectedIndex)
            {
                case 0:
                    // Code to handle the first tab
                    break;
                case 1:
                    // Code to handle the second tab
                    break;
                    // Add more cases for additional tabs if needed
            }
        }

        public void ToolTipItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            curEa = mainForm.ea[nCurIdx];

            if (menuItem.Name.Equals("showLogToolStripMenuItem"))
            {
                new ScrollableMessageBox().ShowLog(mainForm.ea[nCurIdx]);
            }
            else if (menuItem.Name.Equals("showVarToolStripMenuItem"))
            {
                ShowVariables();
            }

        }


        /// <summary>
        /// 1 부터 nEnd까지 모두 더해 반환한다.
        /// </summary>
        /// <param name="nEnd"></param>
        /// <returns></returns>
        public int SumTil(int nEnd)
        {
            int retVal = 0;
            for (int i = 1; i <= nEnd; i++)
                retVal += i;
            return retVal;

        }


        /// <summary>
        /// Series를 차트.시리즈 맨 앞에 삽입한다
        /// 드로잉 순서를 가장 첫번째로 만드는 작업이다.
        /// </summary>
        /// <param name="sSeriesName"></param>
        public void SendToBackSeriesName(string sSeriesName)
        {
            try
            {
                historyChart.Series.Remove(historyChart.Series[sSeriesName]);
                historyChart.Series.Insert(0, new Series(sSeriesName));

            }
            catch (Exception e)
            {
                historyChart.Series.Insert(0, new Series(sSeriesName));
            }

        }


        /// <summary>
        /// Series를 차트.시리즈 맨 뒤에 삽입한다
        /// 드로잉 순서를 가장 마지막으로 만드는 작업이다.
        /// </summary>
        /// <param name="sSeriesName"></param>
        public void SendToFrontSeriesName(string sSeriesName)
        {
            try
            {
                historyChart.Series.Remove(historyChart.Series[sSeriesName]);
                historyChart.Series.Add(sSeriesName);
            }
            catch (Exception e)
            {
                historyChart.Series.Add(sSeriesName);
            }

        }

        #region 마우스 이벤트 핸들러
        /// <summary>
        /// 차트에서 마우스가 움직였을때 히트테스트를 진행한 후 이벤트핸들러
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public int prevXPos = 0, prevYPos = 0;
        public int prevXMove = 0, prevYMove = 0; // 최근 마우스 무브의 움직인 x,y좌표
        public void ChartMouseMoveHandler(Object o, MouseEventArgs e) // 마우스가 안움직이더라도 차트 안에 있으면 호출된다.
        {
            if (e.X != prevXMove || e.Y != prevYMove)  // 움직임이 조금이라도 있으면?
            {
                prevXMove = e.X;
                prevYMove = e.Y;
            }
            else // 전혀 움직이지 않았다면
                return;

            double xCoord = 0, yCoord = 0;


            HitTestResult hit = historyChart.HitTest(e.X, e.Y);
            if (hit.ChartArea != null && historyChart.Series["MinuteStick"].Points.Count > 0)
            {
                xCoord = historyChart.ChartAreas["TotalArea"].AxisX.PixelPositionToValue(e.X);
                yCoord = historyChart.ChartAreas["TotalArea"].AxisY.PixelPositionToValue(e.Y);
                if (double.IsNaN(xCoord))
                    xCoord = 0;
                if (double.IsNaN(yCoord))
                    yCoord = 0;
                xCoord = Math.Round(xCoord, 3);
                yCoord = Math.Round(yCoord, 3);


                curLocLabel.Text = $"현재좌표 : {xCoord} {yCoord}";
                curLocPowerLabel.Text = $"커서파워 : {Math.Round((double)(yCoord - curEa.nYesterdayEndPrice) / curEa.nYesterdayEndPrice, 3)}";
            }

            if (isMouseCursorView)
            {
                if (hit.ChartArea == null)
                {
                    prevXPos = 0;
                    prevYPos = 0;
                    historyChart.ChartAreas["TotalArea"].CursorX.LineDashStyle = ChartDashStyle.NotSet;
                    historyChart.ChartAreas["TotalArea"].CursorY.LineDashStyle = ChartDashStyle.NotSet;
                }
                else
                {

                    if (isMinuteVisible && historyChart.Series["MinuteStick"].Points.Count > 0) // 전체 접근가능 조건
                    {
                        xCoord = historyChart.ChartAreas["TotalArea"].AxisX.PixelPositionToValue(e.X);
                        yCoord = historyChart.ChartAreas["TotalArea"].AxisY.PixelPositionToValue(e.Y);
                        historyChart.ChartAreas["TotalArea"].CursorX.LineDashStyle = ChartDashStyle.Solid;
                        historyChart.ChartAreas["TotalArea"].CursorY.LineDashStyle = ChartDashStyle.Solid;

                        if (isAutoBoundaryCheck) // 자동 체크 바운더리
                        {

                            int xIdx = 0;
                            int xLoc = GetCursorIdx(xCoord, nPadding, historyChart.Series["MinuteStick"].Points.Count, ref xIdx);

                            double fUpperVal;
                            //if (historyChart.Series["MinuteStick"].Points[xIdx].YValues[3] > historyChart.Series["MinuteStick"].Points[xIdx].YValues[2])
                            //    fUpperVal = historyChart.Series["MinuteStick"].Points[xIdx].YValues[3];
                            //else
                            //    fUpperVal = historyChart.Series["MinuteStick"].Points[xIdx].YValues[2];
                            fUpperVal = historyChart.Series["MinuteStick"].Points[xIdx].YValues[3]; // 0 1 2 3  => 고 저 시 종 
                            int nUpperVal = (int)fUpperVal; // 시가와 종가 중 고가(현재는 그냥 종가)

                            if (prevXPos != xIdx || prevYPos != nUpperVal) // xIdx를 벗어나거나 가격이 다르다면
                            {
                                prevXPos = xIdx;
                                prevYPos = nUpperVal;
                                System.Windows.Forms.Cursor.Position = new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y - (int)(e.Y - historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(fUpperVal))); // x는 움직여야하니 그대로 y는 현재위치에서 차트에서의 커서 위치와 그 위치에서의 값의 차이만큼 조정해준다.(더하고 빼고 복잡함)

                            }

                            historyChart.ChartAreas["TotalArea"].CursorX.SetCursorPosition(xLoc); // padding 있는 상태의 x좌표니까 xLoc으로 가고 idx필요할때는 xIdx로 간다
                            historyChart.ChartAreas["TotalArea"].CursorY.SetCursorPosition(fUpperVal);

                        }
                        else
                        {
                            historyChart.ChartAreas["TotalArea"].CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), false);
                            historyChart.ChartAreas["TotalArea"].CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), false);
                        }

                        if (hit.Series != null && hit.Series.Name.Equals("MinuteStick"))
                        {
                            historyChart.ChartAreas["TotalArea"].CursorX.LineColor = Color.Yellow;
                            historyChart.ChartAreas["TotalArea"].CursorY.LineColor = Color.Yellow;
                        }
                        else
                        {
                            historyChart.ChartAreas["TotalArea"].CursorX.LineColor = Color.Red;
                            historyChart.ChartAreas["TotalArea"].CursorY.LineColor = Color.Red;
                        }
                    }

                }
            }
        }

        Graphics gp = null;

        int prevGpCount;

        Pen lpen = new Pen(Color.Black, 2);
        Pen cPen = new Pen(Color.LightGreen, 2);
        Pen pPen = new Pen(Color.MediumPurple, 2);


        int nCircleDenom = 6;
        int nCircleSize = 10;

        // P 변수
        public int nMinuteFrontIdx;
        public int nMinuteBackIdx;

        public int nPadding = 1;

        public void ChartMouseClickHandler(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    HitTestResult hit = historyChart.HitTest(e.X, e.Y);


                    if (prevGpCount != historyChart.Series["MinuteStick"].Points.Count)
                    {
                        prevGpCount = historyChart.Series["MinuteStick"].Points.Count;
                        SetCurGraphics();
                    }

                    if (hit.ChartArea != null)
                    {
                        if (isRightPressed || isPreciselyCheck)
                        {
                            nPressed++;

                            if (nPressed == 1) // 한번 눌렸을때
                            {
                                xVal1 = historyChart.ChartAreas["TotalArea"].AxisX.PixelPositionToValue(e.X);
                                yVal1 = historyChart.ChartAreas["TotalArea"].AxisY.PixelPositionToValue(e.Y);

                                if (!CheckIsNormalChartYValue(yVal1)) // 차트가 이상해..
                                {
                                    xVal1 = yVal1 = 0;
                                    nPressed--;
                                }
                                else
                                {
                                    xPixel1 = (float)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xVal1);
                                    yPixel1 = (float)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(yVal1);

                                    gp.DrawEllipse(cPen, new Rectangle((int)(xPixel1 - Cursor.Size.Width / nCircleDenom), (int)(yPixel1 - Cursor.Size.Height / nCircleDenom), nCircleSize, nCircleSize));

                                }
                            }
                            else if (nPressed == 2) // 두번 눌렸을때
                            {
                                double newX = historyChart.ChartAreas["TotalArea"].AxisX.PixelPositionToValue(e.X);
                                double newY = historyChart.ChartAreas["TotalArea"].AxisY.PixelPositionToValue(e.Y);
                                if (!CheckIsNormalChartYValue(newY)) // 차트가 이상하면
                                {
                                    nPressed--;
                                }
                                else
                                {
                                    if (xVal1 > newX) // 순서가 반대로 찍혔다?
                                    {
                                        xVal2 = xVal1;
                                        yVal2 = yVal1;
                                        xVal1 = newX;
                                        yVal1 = newY;
                                    }
                                    else
                                    {
                                        xVal2 = newX;
                                        yVal2 = newY;
                                    }

                                    xPixel1 = (float)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xVal1);
                                    xPixel2 = (float)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xVal2);
                                    yPixel1 = (float)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(yVal1);
                                    yPixel2 = (float)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(yVal2);

                                    gp.DrawLine(lpen, xPixel1, yPixel1, xPixel2, yPixel2);

                                    if (isPreciselyCheck)
                                    {
                                        try
                                        {

                                            GetCursorIdx(xVal1, nPadding, historyChart.Series["MinuteStick"].Points.Count, ref xMinIdx1);
                                            GetCursorIdx(xVal2, nPadding, historyChart.Series["MinuteStick"].Points.Count, ref xMinIdx2);
                                            if (xMinIdx1 > xMinIdx2)
                                                Swap<int>(ref xMinIdx1, ref xMinIdx2);

                                            // --------------------------------------------
                                            // 여기서 계산을 쫙 다 해놓을거임
                                            nMinPositionX1 = (int)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xMinIdx1 + 1);
                                            nMinPositionY1 = (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(mainForm.ea[nCurIdx].timeLines1m.arrTimeLine[xMinIdx1].nLastFs);
                                            nMinPositionX2 = (int)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xMinIdx2 + 1);
                                            nMinPositionY2 = (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(mainForm.ea[nCurIdx].timeLines1m.arrTimeLine[xMinIdx2].nLastFs);
                                            if (isMinuteVisible && CheckIsNormalChartYValue(nMinPositionY1, nMinPositionY2))
                                                gp.DrawLine(pPen, nMinPositionX1, nMinPositionY1, nMinPositionX2, nMinPositionY2);

                                            pResult = mainForm.CalcFakeHistory(nCurIdx, xMinIdx1, xMinIdx2);
                                        }
                                        catch
                                        {
                                            pResult = default;
                                        }

                                    }
                                }
                            }
                            else if (nPressed > 2)
                            {
                                xVal2 = yVal2 = 0;
                                nPressed = 1;
                                xVal1 = historyChart.ChartAreas["TotalArea"].AxisX.PixelPositionToValue(e.X);
                                yVal1 = historyChart.ChartAreas["TotalArea"].AxisY.PixelPositionToValue(e.Y);

                                if (!CheckIsNormalChartYValue(yVal1)) // 차트가 이상하면
                                {
                                    xVal1 = yVal1 = 0;
                                    nPressed--;
                                }
                                else
                                {
                                    if (isPreciselyCheck)
                                    {
                                        xMinIdx2 = 0;
                                        nMinPositionX2 = nMinPositionY2 = 0;

                                        double xCoord = historyChart.ChartAreas["TotalArea"].AxisX.PixelPositionToValue(e.X);
                                        GetCursorIdx(xCoord, nPadding, historyChart.Series["MinuteStick"].Points.Count, ref xMinIdx1);

                                    }
                                }
                            }
                        }
                        else if (isPointsRecordCheck)
                        {
                            double xPos1 = historyChart.ChartAreas["TotalArea"].AxisX.PixelPositionToValue(e.X);
                            double yPos1 = historyChart.ChartAreas["TotalArea"].AxisY.PixelPositionToValue(e.Y);

                            if (CheckIsNormalChartYValue(yPos1)) // 차트가 정상일때만
                                curEa.myTradeManager.posRecordList.Add((xPos1, yPos1));
                        }
                    } // END ---- HIT 영역
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (eBuyMode == TRADE_MODE.BUY_MODE)
                    {
                        curEa = mainForm.ea[nCurIdx];
                        double yCoord = historyChart.ChartAreas["TotalArea"].AxisY.PixelPositionToValue(e.Y);
                        if (double.IsNaN(yCoord))
                            yCoord = 0;
                        else
                        {
                            int movingPrice = curEa.nFb;
                            int targetPrice = (int)yCoord;

                            if (movingPrice > targetPrice)
                            {
                                while (movingPrice > targetPrice)
                                    movingPrice -= GetIntegratedMarketGap(movingPrice);

                            }
                            else if (movingPrice < targetPrice)
                            {
                                while (movingPrice < targetPrice)
                                    movingPrice += GetIntegratedMarketGap(movingPrice);
                                movingPrice -= GetIntegratedMarketGap(movingPrice); // 한칸 아래서 살거야
                            }
                            mainForm.RequestHandBuy(nCurIdx, movingPrice, nMouseWheel);
                        };
                    }
                    else if (eBuyMode == TRADE_MODE.SELL_MODE)
                    {
                        double yCoord = historyChart.ChartAreas["TotalArea"].AxisY.PixelPositionToValue(e.Y);
                        if (double.IsNaN(yCoord))
                            yCoord = 0;
                        else
                        {
                            if (nCurRealBuyedId == -1)
                            {
                                int movingPrice = curEa.nFb;
                                int targetPrice = (int)yCoord;

                                if (movingPrice > targetPrice)
                                {
                                    while (movingPrice > targetPrice)
                                        movingPrice -= GetIntegratedMarketGap(movingPrice);
                                    movingPrice += GetIntegratedMarketGap(movingPrice);
                                }
                                else if (movingPrice < targetPrice)
                                {
                                    while (movingPrice < targetPrice)
                                        movingPrice += GetIntegratedMarketGap(movingPrice);
                                }
                                mainForm.RequestHandSell(nCurIdx, movingPrice, nMouseWheel);
                            }
                            else // 특정 블럭이 선택돼있다면
                            {
                                if (!curEa.myTradeManager.arrBuyedSlots[nCurRealBuyedId].isAllSelled &&
                                        !curEa.myTradeManager.arrBuyedSlots[nCurRealBuyedId].isSelling &&
                                        curEa.myTradeManager.arrBuyedSlots[nCurRealBuyedId].nCurVolume > 0)
                                {
                                    mainForm.RequestHandBlockSell(nCurIdx, nCurRealBuyedId);
                                }
                                else
                                    mainForm.PrintLog("블럭 손매도 불가", nCurIdx, nCurRealBuyedId);
                            }
                        }
                    }
                    else
                    {
                        if (historyChart.Series["MinuteStick"].Points.Count > 0)
                        {
                            double yCoord = historyChart.ChartAreas["TotalArea"].AxisY.PixelPositionToValue(e.Y);
                            if (double.IsNaN(yCoord))
                                yCoord = 0;
                            else
                            {
                                if (curEa.manualReserve.nCurReserve != MainForm.INIT_RESERVE)
                                {
                                    if (curEa.manualReserve.nCurReserve == MainForm.UP_RESERVE) //  N 이상
                                    {
                                        curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].Clear();
                                        curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].isSelected = true;
                                        curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].nSelectedTime = mainForm.nSharedTime;
                                        curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].fCritLine1 = yCoord;
                                    }
                                    else if (curEa.manualReserve.nCurReserve == MainForm.DOWN_RESERVE) // M 이하
                                    {
                                        curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].Clear();
                                        curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].isSelected = true;
                                        curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].nSelectedTime = mainForm.nSharedTime;
                                        curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].fCritLine1 = yCoord;
                                    }
                                    else if (curEa.manualReserve.nCurReserve == MainForm.MA_DOWN_RESERVE)
                                    {
                                        curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isSelected = true;
                                        curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].nSelectedTime = mainForm.nSharedTime;
                                    }
                                    else if (curEa.manualReserve.nCurReserve == MainForm.MA_RESERVE_POSITION_RESERVE)
                                    {
                                        curEa.manualReserve.reserveArr[MainForm.MA_RESERVE_POSITION_RESERVE].isSelected = true;
                                        curEa.manualReserve.reserveArr[MainForm.MA_RESERVE_POSITION_RESERVE].nSelectedTime = mainForm.nSharedTime;
                                    }
                                    else if (curEa.manualReserve.nCurReserve == MainForm.MA_UP_RESERVE)
                                    {
                                        curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isSelected = true;
                                        curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].nSelectedTime = mainForm.nSharedTime;
                                    }
                                }
                                else // 0으로 설정 가능
                                {
                                    if (curEa.myTradeManager.isTargetChoice)
                                    {
                                        if (curEa.myTradeManager.fBottomPriceTouch == 0)
                                        {
                                            curEa.myTradeManager.fBottomPriceTouch = yCoord;
                                        }
                                        else
                                        {
                                            if (curEa.myTradeManager.fTargetPriceTouch == 0)
                                            {
                                                curEa.myTradeManager.fTargetPriceTouch = yCoord;
                                                if (curEa.myTradeManager.fBottomPriceTouch > curEa.myTradeManager.fTargetPriceTouch)
                                                {
                                                    double tmpVal = curEa.myTradeManager.fBottomPriceTouch;
                                                    curEa.myTradeManager.fBottomPriceTouch = curEa.myTradeManager.fTargetPriceTouch;
                                                    curEa.myTradeManager.fTargetPriceTouch = tmpVal;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                else if (e.Button == MouseButtons.Middle)
                {
                    eBuyMode = SwitchTradeMode(eBuyMode);
                    if (eBuyMode == TRADE_MODE.NONE_MODE)
                        ClearBuyMode();
                    else
                        buyModeLabel.Text = $"buy : {eBuyMode}";
                }
            }
            catch { }
        }

        public void ChartResizeHandler(object sender, EventArgs e)
        {
            SetCurGraphics();
        }

        // 차트가 다시 그려질떄마다 발생하는 이벤트
        public void ChartOnPaintHandler(Object sender, PaintEventArgs e)
        {
            void DrawReserveLine()
            {
                try
                {
                    if (historyChart.Series["MinuteStick"].Points.Count > 0)
                    {
                        try
                        {

                            int reservationX1 = (int)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(historyChart.ChartAreas["TotalArea"].AxisX.Minimum);
                            int reservationX2 = (int)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(historyChart.ChartAreas["TotalArea"].AxisX.Maximum);

                            float reservationY1;

                            string sReserveMsg;
                            string sReserveChosenMsg;

                            if (curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].isSelected && curEa.manualReserve.nCurReserve == MainForm.UP_RESERVE)
                            {
                                if (curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].fCritLine1 > 0)
                                {
                                    reservationY1 = (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].fCritLine1);
                                    e.Graphics.DrawLine(new Pen(Color.BlueViolet, 3), reservationX1, reservationY1, reservationX2, reservationY1);
                                    priceViewLabel.Text = $"이상가격 : {Math.Round(curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].fCritLine1, 2)}";
                                }
                                sReserveMsg = (curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].isBuyReserved) ? $"이상 매수예약 : Yes({curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].nBuyReserveNumStock})" : "이상 매수예약 : No";
                                if (!realBuyReserveLabel.Text.Equals(sReserveMsg))
                                    realBuyReserveLabel.Text = sReserveMsg;

                                sReserveChosenMsg = $"이상채택 : {curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].isChosen1}";
                                if (!reserveChosenLabel.Text.Equals(sReserveChosenMsg))
                                    reserveChosenLabel.Text = sReserveChosenMsg;
                            }
                            else if (curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].isSelected && curEa.manualReserve.nCurReserve == MainForm.DOWN_RESERVE)
                            {
                                if (curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].fCritLine1 > 0)
                                {
                                    reservationY1 = (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].fCritLine1);
                                    e.Graphics.DrawLine(new Pen(Color.Gold, 3), reservationX1, reservationY1, reservationX2, reservationY1);
                                    priceViewLabel.Text = $"이하가격 : {Math.Round(curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].fCritLine1, 2)}";
                                }

                                sReserveMsg = (curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].isBuyReserved) ? $"이하 매수예약 : Yes({curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].nBuyReserveNumStock})" : "이하 매수예약 : No";
                                if (!realBuyReserveLabel.Text.Equals(sReserveMsg))
                                    realBuyReserveLabel.Text = sReserveMsg;

                                sReserveChosenMsg = $"이하채택 : {curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].isChosen1}";
                                if (!reserveChosenLabel.Text.Equals(sReserveChosenMsg))
                                    reserveChosenLabel.Text = sReserveChosenMsg;
                            }
                            else if (curEa.manualReserve.nCurReserve == MainForm.MA_DOWN_RESERVE)
                            {
                                sReserveMsg = (curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isBuyReserved) ? $"MA다운 매수예약 : Yes({curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].nBuyReserveNumStock})" : "MA다운 매수예약 : No";
                                if (!realBuyReserveLabel.Text.Equals(sReserveMsg))
                                    realBuyReserveLabel.Text = sReserveMsg;
                                sReserveChosenMsg = $"MA다운 채택 :{curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isChosen1}";
                                if (!reserveChosenLabel.Text.Equals(sReserveChosenMsg))
                                    reserveChosenLabel.Text = sReserveChosenMsg;
                            }
                            else if (curEa.manualReserve.nCurReserve == MainForm.MA_RESERVE_POSITION_RESERVE)
                            {
                                sReserveMsg = "";
                                if (!realBuyReserveLabel.Text.Equals(sReserveMsg))
                                    realBuyReserveLabel.Text = sReserveMsg;
                                sReserveChosenMsg = $"MA역배열 채택 :{curEa.manualReserve.reserveArr[MainForm.MA_RESERVE_POSITION_RESERVE].isChosen1}";
                                if (!reserveChosenLabel.Text.Equals(sReserveChosenMsg))
                                    reserveChosenLabel.Text = sReserveChosenMsg;
                            }
                            else if (curEa.manualReserve.nCurReserve == MainForm.MA_UP_RESERVE)
                            {
                                sReserveMsg = (curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isBuyReserved) ? $"MA업 매수예약 : Yes({curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].nBuyReserveNumStock})" : "MA업 매수예약 : No";
                                if (!realBuyReserveLabel.Text.Equals(sReserveMsg))
                                    realBuyReserveLabel.Text = sReserveMsg;
                                sReserveChosenMsg = $"MA업 채택 :{curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isChosen1}";
                                if (!reserveChosenLabel.Text.Equals(sReserveChosenMsg))
                                    reserveChosenLabel.Text = sReserveChosenMsg;
                            }

                            if (curEa.myTradeManager.isTargetChoice)
                            {
                                if (curEa.myTradeManager.fBottomPriceTouch > 0)
                                {
                                    reservationY1 = (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(curEa.myTradeManager.fBottomPriceTouch);
                                    e.Graphics.DrawLine(new Pen(Color.Black, 3), reservationX1, reservationY1, reservationX2, reservationY1);
                                }

                                if (curEa.myTradeManager.fTargetPriceTouch > 0)
                                {
                                    reservationY1 = (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(curEa.myTradeManager.fTargetPriceTouch);
                                    e.Graphics.DrawLine(new Pen(Color.Black, 3), reservationX1, reservationY1, reservationX2, reservationY1);
                                }
                            }
                        }
                        catch
                        { }
                    }
                }
                catch { }
            }
            void DrawPosCheck(Color color)
            {
                try
                {
                    if (historyChart.Series["MinuteStick"].Points.Count > 0)
                    {
                        using (Pen pen = new Pen(color, 2))
                        {
                            for (int posIdx = 0; posIdx < curEa.myTradeManager.posRecordList.Count; posIdx++)
                            {
                                var pos = curEa.myTradeManager.posRecordList[posIdx];
                                double x = historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(pos.Item1);
                                double y = historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(pos.Item2);

                                e.Graphics.DrawEllipse(pen, new Rectangle((int)(x - Cursor.Size.Width / nCircleDenom), (int)(y - Cursor.Size.Height / nCircleDenom), nCircleSize, nCircleSize));
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            void DrawOPLine()
            {
                try
                {
                    if (historyChart.Series["MinuteStick"].Points.Count > 0)
                    {
                        if (isRightPressed || isPreciselyCheck)
                        {
                            moveLabel.Text = $"{cPressed}\n{nPressed}\n( {Math.Round(xVal1, 2)}, {Math.Round(yVal1, 2)} )\n( {Math.Round(xVal2, 2)}, {Math.Round(yVal2, 2)} )\n";
                            if (nPressed == 1)
                            {
                                xPixel1 = (float)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xVal1);
                                yPixel1 = (float)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(yVal1);
                                e.Graphics.DrawEllipse(cPen, new Rectangle((int)(xPixel1 - Cursor.Size.Width / nCircleDenom), (int)(yPixel1 - Cursor.Size.Height / nCircleDenom), nCircleSize, nCircleSize));
                            }
                            else if (nPressed == 2)
                            {
                                xPixel1 = (float)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xVal1);
                                xPixel2 = (float)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xVal2);
                                yPixel1 = (float)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(yVal1);
                                yPixel2 = (float)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(yVal2);

                                e.Graphics.DrawLine(lpen, xPixel1, yPixel1, xPixel2, yPixel2);

                                if (CheckIsNormalChartYValue(yVal1, yVal2))
                                {
                                    moveLabel.Text += $"( {Math.Round(xVal1, 2)}, {Math.Round(yVal1, 2)} ) -> ( {Math.Round(xVal2, 2)},  {Math.Round(yVal2, 2)})\n" +
                                        $"손익(시초값기준) : {Math.Round((yVal2 - yVal1) / curEa.nTodayStartPrice * 100, 2)}(%)\n" +
                                        $"손익( 종가기준 ) : {Math.Round((yVal2 - yVal1) / curEa.nYesterdayEndPrice * 100, 2)}(%)\n" +
                                        $"손익(첫번째기준) : {Math.Round((yVal2 - yVal1) / yVal1 * 100, 2)}(%)\n" +
                                        $"x2 - x1 : {Math.Round(xVal2, 0) - Math.Round(xVal1, 0)}칸\n";
                                }
                                else
                                    moveLabel.Text += "포인트를 다시 지정하세요\n";

                                if (isPreciselyCheck)
                                {
                                    // 여기서는 mouseClick에서 계산해놓은것을 그대로 출력만 할거
                                    {
                                        nMinPositionX1 = (int)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xMinIdx1 + 1);
                                        nMinPositionY1 = (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(mainForm.ea[nCurIdx].timeLines1m.arrTimeLine[xMinIdx1].nLastFs);
                                        nMinPositionX2 = (int)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xMinIdx2 + 1);
                                        nMinPositionY2 = (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(mainForm.ea[nCurIdx].timeLines1m.arrTimeLine[xMinIdx2].nLastFs);

                                        if (isMinuteVisible && CheckIsNormalChartYValue(nMinPositionY1, nMinPositionY2))
                                            e.Graphics.DrawLine(pPen, nMinPositionX1, nMinPositionY1, nMinPositionX2, nMinPositionY2);
                                        moveLabel.Text += $"================== 분당 정보 ==================\n" +
                                            $"페이크 매수 : ( {pResult.nFakeBuyStrategyNum}, 분당 : {pResult.nFakeBuyStrategyMinuteNum} ){NEW_LINE}" +
                                            $"페이크 보조 : ( {pResult.nFakeAssistantStrategyNum}, 분당 : {pResult.nFakeAssistantStrategyMinuteNum} ){NEW_LINE}" +
                                            $"페이크 저항 : ( {pResult.nFakeResistStrategyNum}, 분당 : {pResult.nFakeResistStrategyMinuteNum} ){NEW_LINE}" +
                                            $"가격 업 :      ( {pResult.nFakeUpStrategyNum}, 분당 : {pResult.nFakeUpStrategyMinuteNum} ){NEW_LINE}" +
                                            $"가격다운 :    ( {pResult.nFakeDownStrategyNum}, 분당 : {pResult.nFakeDownStrategyMinuteNum} ){NEW_LINE}" +
                                            $"모의매수 :    ( {pResult.nPaperBuyStrategyNum}, 분당 : {pResult.nPaperBuyStrategyMinuteNum} ){NEW_LINE}" +
                                            $"총 애로우 :   ( {pResult.nTotalStrategyNum}, 분당 : {pResult.nTotalStrategyMinuteNum} ){NEW_LINE}{NEW_LINE}";

                                    }
                                }
                            }
                        }
                        else
                            moveLabel.Text = "";
                    }
                }
                catch
                {

                }
            }

            void DrawHitEdge(Dictionary<int, int> dict, Color color)
            {
                try
                {
                    if (historyChart.Series["MinuteStick"].Points.Count > 0)
                    {
                        foreach (var key in dict.Keys)
                        {
                            Series series = historyChart.Series["MinuteStick"];

                            if (series.Points.Count >= key + 1)
                            {
                                DataPoint point = series.Points[key];

                                double pixelPosition1 = historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(0.8);
                                double pixelPosition2 = historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(0);

                                double barLength = Math.Abs(pixelPosition2 - pixelPosition1);

                                double xLocation = key + 1;
                                double yStartValue = point.YValues[2]; // 시가
                                double yLastValue = point.YValues[3]; // 종가

                                double yMaxValue = Max(yLastValue, yStartValue);
                                double yMinValue = Min(yLastValue, yStartValue);

                                using (Pen pen = new Pen(color, 2))
                                {
                                    e.Graphics.DrawRectangle(pen,
                                        (float)(historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(xLocation) - barLength / 2),
                                        (float)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(yMaxValue),
                                        (float)barLength,
                                        (float)(historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(yMinValue) - historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(yMaxValue)));
                                }
                            }
                        }
                    }

                }
                catch { }
            }


            void WriteInfo()
            {
                try
                {
                    gapLabel.Text = $"현재갭 : {Math.Round(curEa.fStartGap, 3)}";
                    isAllSelledLabel.Text = $"매도완료 : {curEa.myTradeManager.nTotalSelled}";
                    isSellingLabel.Text = $"매도중 : {curEa.myTradeManager.nTotalSelling}";
                    isAllBuyedLabel.Text = $"총매수 : {curEa.myTradeManager.nTotalBuyed}";
                    restVolumeLabel.Text = $"잔량 : {curEa.myTradeManager.nTotalBuyed - (curEa.myTradeManager.nTotalSelling + curEa.myTradeManager.nTotalSelled)}";
                }
                catch
                { }
            }

            void WriteGraphicsSet()
            {
                try
                {
                    if (isHitView)
                    {
                        DrawHitEdge(mainForm.ea[nCurIdx].fakeStrategyMgr.hitDict25, Color.Orange);
                        DrawHitEdge(mainForm.ea[nCurIdx].fakeStrategyMgr.hitDict38, Color.Green);
                        DrawHitEdge(mainForm.ea[nCurIdx].fakeStrategyMgr.hitDict312, Color.Purple);
                        DrawHitEdge(mainForm.ea[nCurIdx].fakeStrategyMgr.hitDict410, Color.Black);
                    }

                    if (isPosView)
                    {
                        DrawPosCheck(Color.Gold);
                    }

                    DrawReserveLine();
                    DrawOPLine();
                    WriteInfo();
                }
                catch { }
            }

            WriteGraphicsSet();

            if (!isTradeCancelInit)
            {
                mainForm.ea[nCurIdx].eventMgr.cancelEachStockFormEventHandler?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool isTradeCancelInit = false;
        public int GetCursorIdx(double xCoord, int frontPad, int backPad, ref int xIdx)
        {
            xIdx = (int)xCoord - frontPad;
            int xLoc = xIdx + frontPad;

            if ((xCoord % 1) > 0.5) // ex) 25.52가 나오면 26이라 판단
            {
                xIdx++;
                xLoc++;
            }

            if (xIdx < frontPad)
            {
                xIdx = 0;
                xLoc = frontPad;
            }

            else if (xIdx >= backPad)
            {
                xIdx = backPad - 1;
                xLoc = xIdx + frontPad;
            }

            return xLoc;
        }
        #endregion

        public TRADE_MODE eBuyMode = TRADE_MODE.NONE_MODE;
        public const int NONE_MODE = 0;
        public const int BUY_MODE = 1;
        public const int SELL_MODE = 2;
        public enum TRADE_MODE
        {
            NONE_MODE,
            BUY_MODE,
            SELL_MODE,
        }
        public TRADE_MODE SwitchTradeMode(TRADE_MODE m)
        {
            if (m == TRADE_MODE.NONE_MODE)
                return TRADE_MODE.BUY_MODE;
            else if (m == TRADE_MODE.BUY_MODE)
                return TRADE_MODE.SELL_MODE;
            else
                return TRADE_MODE.NONE_MODE;
        }

        #region 키보드 이벤트 핸들러
        public void KeyUpHandler(object sender, KeyEventArgs e)
        {
            char cUp = (char)e.KeyValue;


            if (cUp == 38) // 위쪽 화살표
            {
                fMaxPlus += 0.025;
                expansionLabel.Text = $"확장 : {Math.Round(fMaxPlus, 3)}, {Math.Round(fMinPlus, 3)}";
                SetChartViewRange(0, curEa.timeLines1m.nRealDataIdx + 2, curEa.nFs, curEa.nFs, "TotalArea");
                mainForm.ea[nCurIdx].eventMgr.cancelEachStockFormEventHandler?.Invoke(this, EventArgs.Empty);
            }

            if (cUp == 40) // 아래쪽 화살표
            {
                fMinPlus += 0.025;
                expansionLabel.Text = $"확장 : {Math.Round(fMaxPlus, 3)}, {Math.Round(fMinPlus, 3)}";
                SetChartViewRange(0, curEa.timeLines1m.nRealDataIdx + 2, curEa.nFs, curEa.nFs, "TotalArea");
                mainForm.ea[nCurIdx].eventMgr.cancelEachStockFormEventHandler?.Invoke(this, EventArgs.Empty);
            }

            if (cUp == 37) // 왼쪽 화살표
            {
                fMaxPlus = 0;
                fMinPlus = 0;
                nXPlus = 0;
                expansionLabel.Text = $"확장 : {Math.Round(fMaxPlus, 3)}, {Math.Round(fMinPlus, 3)}";
                SetChartViewRange(0, curEa.timeLines1m.nRealDataIdx + 2, curEa.nFs, curEa.nFs, "TotalArea");
                mainForm.ea[nCurIdx].eventMgr.cancelEachStockFormEventHandler?.Invoke(this, EventArgs.Empty);
            }
            if (cUp == 39) // 오른쪽 화살표
            {
                nXPlus++;
                SetChartViewRange(0, curEa.timeLines1m.nRealDataIdx + 2, curEa.nFs, curEa.nFs, "TotalArea");
            }


            if (cUp == 192) // `
            {
                isHitView = !isHitView;
                UpdateMinuteHistoryData();
            }

            if (cUp == 'M')
            {
                isMouseCursorView = !isMouseCursorView;
            }

            if (cUp == 'O') // 각도기
            {
                isRightPressed = false;
            }

            if (cUp == 'C') // 오토 바운더리
            {
                isAutoBoundaryCheck = false;
            }
            if (cUp == 'P') // precisely
            {
                // 각도기의 확장버전
                // 지정된 X사이에 정보들을 모두 가져온다. ( max 각도 , min 각도, 이평선 침범 여부, 침범 횟수 등등을 계산해서 가져온다)
                // TODO.
                isPreciselyCheck = false;
            }
            if (cUp == 'V') // 변수출력
            {
                ShowVariables();
            }
            if (cUp == 'K') // 매매블럭
            {
                new ScrollableMessageBox().ShowEachBlock(mainForm.ea[nCurIdx]);
            }

            if (cUp == 'G')
            {
                isViewGapMa = !isViewGapMa;
                ResetMinuteChart();
            }

            if (cUp == 'L') // 로그
            {
                new ScrollableMessageBox().ShowLog(mainForm.ea[nCurIdx]);
            }


            if (cUp == 27) // esc
            {
                timer.Enabled = false;
                this.Close();
            }

            if (cUp == 17) // ctrl
            {
                isCtrlPushed = false;
            }

            if (cUp == 32) // space
            {
                isSpacePushed = false;
            }

            if (cUp == 16) // shift
            {
                isShiftPushed = false;
            }

            if (cUp == 'I')
            {
                if (isShiftPushed)
                {
                    isPosView = !isPosView;
                    UpdateMinuteHistoryData();
                }

                if (isCtrlPushed)
                {
                    curEa.myTradeManager.posRecordList.Clear();
                }

                isPointsRecordCheck = false;
            }

            if (!isCtrlPushed)
            {
                if (cUp == 191) // enter
                {
                    isQuestionPushed = false;
                }
            }

            if (cUp == 'U') // update
            {
                if (isCtrlPushed)
                    timer.Enabled = !timer.Enabled;
                updateDelegate();
            }

            if (cUp == 189) // -
            {
                isArrowGrouping = true;
                UpdateMinuteHistoryData();
            }
            if (cUp == 187) // +
            {
                isArrowGrouping = false;
                UpdateMinuteHistoryData();
            }

            if (cUp == 'Q') // q가 눌렸는데 실매수
            {
                if (isSpacePushed) // 스페이도 눌린상태라면 
                {
                    ShowPaperBuy(); // 전략현황을 출력해주고
                    isSpacePushed = false;
                }
                else // 아니라면
                {
                    isPaperBuyArrowVisible = !isPaperBuyArrowVisible;
                    UpdateMinuteHistoryData(); // 단순 Q이기 때문에 updateMinuteHistory만 해주면 된다.
                }
            }

            if (cUp == 'W') // 실매도
            {
                isPaperSellArrowVisible = !isPaperSellArrowVisible;
                UpdateMinuteHistoryData();
            }

            if (cUp == 'E') // 페이크매수
            {
                if (isSpacePushed) // 스페이도 눌린상태라면 
                {
                    ShowFakeBuy(); // 전략현황을 출력해주고
                    isSpacePushed = false;
                }
                else
                {
                    isFakeBuyArrowVisible = !isFakeBuyArrowVisible;
                    UpdateMinuteHistoryData();
                }
            }

            if (cUp == 'R') // 페이크저항
            {
                if (isSpacePushed) // 스페이도 눌린상태라면 
                {
                    ShowFakeResist(); // 전략현황을 출력해주고
                    isSpacePushed = false;
                }
                else
                {
                    isFakeResistArrowVisible = !isFakeResistArrowVisible;
                    UpdateMinuteHistoryData();
                }
            }

            if (cUp == 'S') // 페이크보조
            {
                if (isSpacePushed) // 스페이도 눌린상태라면 
                {
                    ShowFakeAssistant(); // 전략현황을 출력해주고
                    isSpacePushed = false;
                }
                else
                {
                    isFakeAssistantArrowVisible = !isFakeAssistantArrowVisible;
                    UpdateMinuteHistoryData();
                }
            }


            if (cUp == 'D') // 페이크 변동성
            {
                if (isSpacePushed) // 스페이도 눌린상태라면 
                {
                    ShowFakeVolatility(); // 전략현황을 출력해주고
                    isSpacePushed = false;
                }
                else
                {
                    isFakeVolatilityArrowVisible = !isFakeVolatilityArrowVisible;
                    UpdateMinuteHistoryData();
                }
            }

            if (cUp == 'F') // 페이크 다운
            {
                if (isSpacePushed) // 스페이도 눌린상태라면 
                {
                    ShowFakeDown(); // 전략현황을 출력해주고
                    isSpacePushed = false;
                }
                else
                {
                    isFakeDownArrowVisible = !isFakeDownArrowVisible;
                    UpdateMinuteHistoryData();
                }
            }

            if (cUp == 'Z') // 매수
            {
                isBuyArrowVisible = !isBuyArrowVisible;
                UpdateMinuteHistoryData();
            }

            if (cUp == 'X') // 매도
            {
                isSellArrowVisible = !isSellArrowVisible;
                UpdateMinuteHistoryData();
            }

            if (cUp == 'A')
            {
                ReverseAllArrowVisible();
                UpdateMinuteHistoryData();
            }

            if (cUp >= 49 && cUp <= 53)
            {

                if (isCtrlPushed) // 취소
                {
                    if (cUp == 49)
                    {
                        curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].Clear();
                        realBuyReserveLabel.Text = "이상예약 취소";
                        reserveChosenLabel.Text = "";
                        priceViewLabel.Text = "";
                    }
                    if (cUp == 50)
                    {
                        curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].Clear();
                        realBuyReserveLabel.Text = "이하예약 취소";
                        reserveChosenLabel.Text = "";
                        priceViewLabel.Text = "";
                    }
                    if (cUp == 51)
                    {
                        curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].Clear();
                        realBuyReserveLabel.Text = "MA다운예약 취소";
                        reserveChosenLabel.Text = "";
                        priceViewLabel.Text = "";
                    }
                    if (cUp == 52)
                    {
                        curEa.manualReserve.reserveArr[MainForm.MA_RESERVE_POSITION_RESERVE].Clear();
                        realBuyReserveLabel.Text = "MA역배열 취소";
                        reserveChosenLabel.Text = "";
                        priceViewLabel.Text = "";
                    }
                    if (cUp == 53)
                    {
                        curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].Clear();
                        realBuyReserveLabel.Text = "MA업예약 취소";
                        reserveChosenLabel.Text = "";
                        priceViewLabel.Text = "";
                    }
                }
                else
                {
                    realBuyReserveLabel.Text = "";
                    reserveChosenLabel.Text = "";
                    priceViewLabel.Text = "";
                }
                curEa.manualReserve.nCurReserve = MainForm.INIT_RESERVE;
            }

            if ((cUp >= 54 && cUp <= 57) || (cUp == 48))
            {
                if (isSpacePushed)
                {
                    curEa = mainForm.ea[nCurIdx];
                    for (int i = 0; i < curEa.myTradeManager.arrBuyedSlots.Count; i++)
                    {
                        if (!curEa.myTradeManager.arrBuyedSlots[i].isAllSelled && !curEa.myTradeManager.arrBuyedSlots[i].isSelling)
                        {
                            if (nCurRealBuyedId != -1 && nCurRealBuyedId != i)
                                continue;

                            void ResetCheckLinesBeforeApply()
                            {
                                if (isCtrlPushed)
                                {
                                    curEa.myTradeManager.arrBuyedSlots[i].nCurLineIdx = 0;

                                    curEa.myTradeManager.arrBuyedSlots[i].nCheckLineIdx = MainForm.MIDDLE_STEP;
                                    curEa.myTradeManager.arrBuyedSlots[i].nMinCheckLineIdx = MainForm.MIDDLE_STEP;
                                    curEa.myTradeManager.arrBuyedSlots[i].nMaxCheckLineIdx = MainForm.MIDDLE_STEP;
                                    curEa.myTradeManager.arrBuyedSlots[i].fCheckCeilingPer = 0;
                                    curEa.myTradeManager.arrBuyedSlots[i].fCheckBottomPer = -0.0025;
                                }
                            }

                            if (cUp == 48)
                            {
                                curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod = MainForm.TradeMethodCategory.None;
                            }
                            else if (cUp == 54) // 6
                            {
                                if (curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod != MainForm.TradeMethodCategory.RisingMethod || isCtrlPushed)
                                {
                                    ResetCheckLinesBeforeApply();

                                    curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod = MainForm.TradeMethodCategory.RisingMethod;
                                    curEa.myTradeManager.arrBuyedSlots[i].fTargetPer = mainForm.GetNextCeiling(curEa.myTradeManager.arrBuyedSlots[i].nCurLineIdx);
                                    curEa.myTradeManager.arrBuyedSlots[i].fBottomPer = mainForm.GetNextFloor(curEa.myTradeManager.arrBuyedSlots[i].nCurLineIdx, curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod);
                                }
                            }
                            else if (cUp == 55) // 7
                            {
                                if (curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod != MainForm.TradeMethodCategory.BottomUpMethod || isCtrlPushed)
                                {
                                    ResetCheckLinesBeforeApply();

                                    curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod = MainForm.TradeMethodCategory.BottomUpMethod;
                                    curEa.myTradeManager.arrBuyedSlots[i].fTargetPer = mainForm.GetNextCeiling(curEa.myTradeManager.arrBuyedSlots[i].nCurLineIdx);
                                    curEa.myTradeManager.arrBuyedSlots[i].fBottomPer = mainForm.GetNextFloor(curEa.myTradeManager.arrBuyedSlots[i].nCurLineIdx, curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod);
                                }

                            }
                            else if (cUp == 56) // 8
                            {
                                if (curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod != MainForm.TradeMethodCategory.ScalpingMethod || isCtrlPushed)
                                {
                                    ResetCheckLinesBeforeApply();

                                    curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod = MainForm.TradeMethodCategory.ScalpingMethod;
                                    curEa.myTradeManager.arrBuyedSlots[i].fTargetPer = mainForm.GetNextCeiling(curEa.myTradeManager.arrBuyedSlots[i].nCurLineIdx);
                                    curEa.myTradeManager.arrBuyedSlots[i].fBottomPer = mainForm.GetNextFloor(curEa.myTradeManager.arrBuyedSlots[i].nCurLineIdx, curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod);
                                }

                            }
                            else if (cUp == 57) // 9
                            {
                                ResetCheckLinesBeforeApply();

                                curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod = MainForm.TradeMethodCategory.FixedMethod;
                                if (curEa.myTradeManager.fBottomPriceTouch != 0 && curEa.myTradeManager.fTargetPriceTouch != 0)
                                {
                                    curEa.myTradeManager.arrBuyedSlots[i].fTargetPer = mainForm.GetProfitPercent(curEa.myTradeManager.arrBuyedSlots[i].nBuyedSumPrice, (int)(curEa.myTradeManager.fTargetPriceTouch * curEa.myTradeManager.arrBuyedSlots[i].nBuyVolume), curEa.nMarketGubun) / 100;
                                    curEa.myTradeManager.arrBuyedSlots[i].fBottomPer = mainForm.GetProfitPercent(curEa.myTradeManager.arrBuyedSlots[i].nBuyedSumPrice, (int)(curEa.myTradeManager.fBottomPriceTouch * curEa.myTradeManager.arrBuyedSlots[i].nBuyVolume), curEa.nMarketGubun) / 100;
                                }
                                else
                                {
                                    curEa.myTradeManager.arrBuyedSlots[i].fTargetPer = MainForm.DEFAULT_FIXED_CEILING;
                                    curEa.myTradeManager.arrBuyedSlots[i].fBottomPer = MainForm.DEFAULT_FIXED_BOTTOM;
                                }

                            }


                            if (nCurRealBuyedId == -1) // 공용 (그냥 반복해서 삽입)
                                curEa.myTradeManager.eDefaultTradeCategory = curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod;

                            if (nCurRealBuyedId == -1)
                                tradeMethodLabel.Text = $"전체 매매기법 : {curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod}";
                            else
                                tradeMethodLabel.Text = $"{nCurRealBuyedId}번째 매매기법 : {curEa.myTradeManager.arrBuyedSlots[i].eTradeMethod}";
                        }
                        else
                            tradeMethodLabel.Text = "전체 매매기법 : 변경불가";
                    }
                }
                else
                {
                    if (cUp == 57) // 9
                    {
                        curEa.myTradeManager.isTargetChoice = false;
                        if (curEa.myTradeManager.fTargetPriceTouch == 0) // 두번째 타겟이 입력 안됐으면 초기화
                        {
                            curEa.myTradeManager.fBottomPriceTouch = 0;
                            curEa.myTradeManager.fTargetPriceTouch = 0;
                        }

                        if (isCtrlPushed) // 초기화
                        {
                            curEa.myTradeManager.fBottomPriceTouch = 0;
                            curEa.myTradeManager.fTargetPriceTouch = 0;
                        }

                    }
                }
            }


            if (isCtrlPushed)
            {
                if (cUp == 191) // ?
                {
                    curEa.manualReserve.ClearAll();
                    realBuyReserveLabel.Text = "전체예약 취소";
                    reserveChosenLabel.Text = "";
                    priceViewLabel.Text = "";
                }
            }

        }


        public bool isCtrlPushed = false;
        public bool isShiftPushed = false;
        public bool isSpacePushed = false;
        public bool isQuestionPushed = false;

        public bool isHitView = true;
        public bool isPosView = true;

        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            cPressed = (char)e.KeyValue; // 대문자로 준다.

            if (cPressed == 'O') // 기본각도기
            {
                if (!isRightPressed)
                {
                    DeepClean();
                    isRightPressed = true;
                }
            }

            if (cPressed == 'I')
            {
                isPointsRecordCheck = true;
            }

            if (cPressed >= 49 && cPressed <= 53)
            {

                ClearBuyMode(false);

                if (cPressed == 49)
                {
                    if (isShiftPushed && !curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].isChosen1 && curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].isSelected)
                    {
                        curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].nBuyReserveNumStock = nMouseWheel;
                        curEa.manualReserve.reserveArr[MainForm.UP_RESERVE].isBuyReserved = true;
                    }
                    curEa.manualReserve.nCurReserve = MainForm.UP_RESERVE;
                }
                else if (cPressed == 50)
                {
                    if (isShiftPushed && !curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].isChosen1 && curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].isSelected)
                    {
                        curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].nBuyReserveNumStock = nMouseWheel;
                        curEa.manualReserve.reserveArr[MainForm.DOWN_RESERVE].isBuyReserved = true;
                    }
                    curEa.manualReserve.nCurReserve = MainForm.DOWN_RESERVE;
                }
                else if (cPressed == 51)
                {
                    if (isShiftPushed && !curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isChosen1 && curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isSelected )
                    {
                        curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].nBuyReserveNumStock = nMouseWheel;
                        curEa.manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isBuyReserved = true;
                    }
                    curEa.manualReserve.nCurReserve = MainForm.MA_DOWN_RESERVE;
                }
                else if ( cPressed == 52)
                {
                    curEa.manualReserve.nCurReserve = MainForm.MA_RESERVE_POSITION_RESERVE;
                }
                else if (cPressed == 53)
                {
                    if (isShiftPushed && !curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isChosen1 && curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isSelected)
                    {
                        curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].nBuyReserveNumStock = nMouseWheel;
                        curEa.manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isBuyReserved = true;
                    }
                    curEa.manualReserve.nCurReserve = MainForm.MA_UP_RESERVE;
                }

            }
            if (cPressed == 57) // 9
            {
                ClearBuyMode();
                curEa.myTradeManager.isTargetChoice = true;
            }

            if (cPressed == 'C') // 오토 바운더리
                isAutoBoundaryCheck = true;

            if (cPressed == 'P') // precisely
            {
                // 각도기의 확장버전
                // 지정된 X사이에 정보들을 모두 가져온다. ( max 각도 , min 각도, 이평선 침범 여부, 침범 횟수 등등을 계산해서 가져온다)
                // TODO.
                if (!isPreciselyCheck)
                {
                    DeepClean();
                    isPreciselyCheck = true;
                }
            }



            if (cPressed == 17) // ctrl
            {
                isCtrlPushed = true;
            }

            if (cPressed == 16) // shift
            {
                isShiftPushed = true;
            }

            if (cPressed == 32) // space 
            {
                isSpacePushed = true;
            }
            if (!isCtrlPushed)
            {
                if (cPressed == 191) // enter 
                {
                    isQuestionPushed = true;
                }
            }

        }

        #endregion

        public void ClearBuyMode(bool wheelInit = true)
        {
            eBuyMode = TRADE_MODE.NONE_MODE;
            buyModeLabel.Text = $"buy : {eBuyMode}";
            if (wheelInit)
            {
                nMouseWheel = 0;
                wheelLabel.Text = $"wheel : 0";
            }
        }

        public void ReverseAllArrowVisible()
        {
            isAllArrowVisible = !isAllArrowVisible;
            isBuyArrowVisible = isAllArrowVisible;
            isSellArrowVisible = isAllArrowVisible;
            isFakeBuyArrowVisible = isAllArrowVisible;
            isFakeResistArrowVisible = isAllArrowVisible;
            isFakeAssistantArrowVisible = isAllArrowVisible;
            isFakeVolatilityArrowVisible = isAllArrowVisible;
            isFakeDownArrowVisible = isAllArrowVisible;
            isPaperBuyArrowVisible = isAllArrowVisible;
            isPaperSellArrowVisible = isAllArrowVisible;
        }



        public char cPressed;
        public double xVal1, xVal2, yVal1, yVal2;
        public float xPixel1, xPixel2, yPixel1, yPixel2;


        public int xMinIdx1, xMinIdx2;


        public int nPressed;
        public bool isRightPressed;
        public bool isPreciselyCheck;
        public bool isPointsRecordCheck;

        public bool isMouseCursorView;

        public void DeepClean()
        {
            xVal1 = xVal2 = yVal1 = yVal2 = 0;
            xPixel1 = xPixel2 = yPixel1 = yPixel2 = 0;
            xMinIdx1 = xMinIdx2 = 0;
            nMinPositionX1 = nMinPositionX2 = nMinPositionY1 = nMinPositionY2 = 0;
            isRightPressed = false;
            isPreciselyCheck = false;
            nPressed = 0;

        }

        public bool CheckIsNormalChartYValue(params double[] yList)
        {
            bool ret = false;
            for (int i = 0; i < yList.Length; i++)
            {
                ret = (yList[i] > 0) && !double.IsNaN(yList[i]);
                if (!ret)
                    break;
            }
            return ret;
        }

        public int nMouseWheel = 0;


        public void MouseWheelEventHandler(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) // Scrolled up
            {
                nMouseWheel++;
            }
            else if (e.Delta < 0)
            {
                nMouseWheel--;
            }
            wheelLabel.Text = $"wheel : {nMouseWheel}";
        }

        public void OnTradeCancelArrowClicked(object sender, EventArgs e)
        {
            string[] sArrArrowName = ((ArrowControl)sender).Name.Split();

            if (sArrArrowName[1].Equals("B"))
                mainForm.RequestHandBuyCancel(nCurIdx, sArrArrowName[2]);
            else if (sArrArrowName[1].Equals("S"))
                mainForm.RequestHandSellCancel(nCurIdx, sArrArrowName[2]);

            mainForm.ea[nCurIdx].eventMgr.cancelEachStockFormEventHandler?.Invoke(this, EventArgs.Empty);
        }

        public void CancelEventHandler(object sender, EventArgs e)
        {
            void RefreshCancelConfirm()
            {
                try
                {
                    curEa = mainForm.ea[nCurIdx];

                    if (historyChart.Series["MinuteStick"].Points.Count > 0)
                    {

                        isTradeCancelInit = true;

                        // ======================================예약대기 출력===============================================
                        // 준비물 초기화
                        for (int i = 0; i < historyChart.Controls.Count; i++)
                        {
                            if (historyChart.Controls[i].Name != null && historyChart.Controls[i].Name[0] == '^')
                                historyChart.Controls.RemoveAt(i--);
                        }
                        toCancelDict.Clear();

                        List<ArrowControl> arrowList = new List<ArrowControl>();

                        if (double.IsNaN(historyChart.ChartAreas["TotalArea"].AxisX.Maximum)) // 매수취소애로우 놓을 수 없음
                            return;
                        // 매수예약
                        for (int buyCancel = 0; buyCancel < curEa.unhandledBuyOrderIdList.Count; buyCancel++)
                        {
                            string sOrderId = curEa.unhandledBuyOrderIdList[buyCancel];

                            if (!mainForm.buyCancelingByOrderIdDict.ContainsKey(sOrderId))
                            {
                                MainForm.BuyedSlot slot = mainForm.buySlotByOrderIdDict[sOrderId];
                                PrevCancel cancelInfo;
                                cancelInfo.sPrevOrderId = sOrderId;
                                cancelInfo.sAccumMsg = "";
                                arrowControl = new ArrowControl((int)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(historyChart.ChartAreas["TotalArea"].AxisX.Maximum), (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(slot.nOrderPrice), isBuy: true, le: OnTradeCancelArrowClicked);

                                if (toCancelDict.ContainsKey((BUY_RESERVE, slot.nOrderPrice)))
                                {
                                    List<PrevCancel> beforeList = toCancelDict[(BUY_RESERVE, slot.nOrderPrice)];
                                    int beforeCnt = beforeList.Count;

                                    arrowControl.ArrowColor = GetArrowStepColor(beforeCnt + 1);
                                    new ToolTip().SetToolTip(arrowControl, $"-----------------현재매수주문--------------{NEW_LINE} 물량 : {slot.nOrderVolume} 잔량 : {slot.nOrderVolume - slot.nCurVolume}  가격 : {slot.nOrderPrice}{NEW_LINE}===================== 대기물량: {beforeCnt}개 ====================={NEW_LINE}{NEW_LINE}{beforeList[beforeCnt - 1].sAccumMsg}");
                                    cancelInfo.sAccumMsg += $"물량 : {slot.nOrderVolume - slot.nCurVolume}  가격 : {slot.nOrderPrice}{NEW_LINE}{beforeList[beforeCnt - 1].sAccumMsg}";
                                }
                                else
                                {
                                    arrowControl.ArrowColor = GetArrowStepColor(1);
                                    new ToolTip().SetToolTip(arrowControl, $"-----------------현재매수주문--------------{NEW_LINE} 물량 : {slot.nOrderVolume} 잔량 : {slot.nOrderVolume - slot.nCurVolume} 가격 : {slot.nOrderPrice}{NEW_LINE}");
                                    toCancelDict[(BUY_RESERVE, slot.nOrderPrice)] = new List<PrevCancel>();
                                    cancelInfo.sAccumMsg += $"물량 : {slot.nOrderVolume - slot.nCurVolume}  가격 : {slot.nOrderPrice}{NEW_LINE}";
                                }

                                toCancelDict[(BUY_RESERVE, slot.nOrderPrice)].Add(cancelInfo);
                                arrowControl.Name = $"^ B {sOrderId}";
                                arrowList.Add(arrowControl);
                            }
                        }

                        if (double.IsNaN(historyChart.ChartAreas["TotalArea"].AxisX.Minimum)) // 매수취소애로우 놓을 수 없음
                            return;
                        // 매도예약
                        for (int sellCancel = 0; sellCancel < curEa.unhandledSellOrderIdList.Count; sellCancel++)
                        {
                            string sOrderId = curEa.unhandledSellOrderIdList[sellCancel];
                            if (!mainForm.sellCancelingByOrderIdDict.ContainsKey(sOrderId))
                            {
                                MainForm.VirtualSellBlock virtualSellBlock = mainForm.virtualSellBlockByOrderIdDict[sOrderId];
                                PrevCancel cancelInfo;
                                cancelInfo.sPrevOrderId = sOrderId;
                                cancelInfo.sAccumMsg = "";
                                arrowControl = new ArrowControl((int)historyChart.ChartAreas["TotalArea"].AxisX.ValueToPixelPosition(historyChart.ChartAreas["TotalArea"].AxisX.Minimum - 1), (int)historyChart.ChartAreas["TotalArea"].AxisY.ValueToPixelPosition(virtualSellBlock.nOrderPrice), isBuy: false, le: OnTradeCancelArrowClicked);

                                if (toCancelDict.ContainsKey((SELL_RESERVE, virtualSellBlock.nOrderPrice)))
                                {
                                    List<PrevCancel> beforeList = toCancelDict[(SELL_RESERVE, virtualSellBlock.nOrderPrice)];
                                    int beforeCnt = beforeList.Count;

                                    arrowControl.ArrowColor = GetArrowStepColor(beforeCnt + 1);
                                    new ToolTip().SetToolTip(arrowControl, $"-----------------현재매도주문--------------{NEW_LINE}시간 : {virtualSellBlock.nOrderTime} 가격 : {virtualSellBlock.nOrderPrice} 물량 : {virtualSellBlock.nOrderVolume} 잔량 : {virtualSellBlock.nOrderVolume - virtualSellBlock.nProcessedVolume}{NEW_LINE}===================== 대기물량 : {beforeCnt}개 ====================={NEW_LINE}{NEW_LINE}{beforeList[beforeCnt - 1].sAccumMsg}");
                                    cancelInfo.sAccumMsg = $"-----------------현재매도주문--------------{NEW_LINE}시간 : {virtualSellBlock.nOrderTime} 가격 : {virtualSellBlock.nOrderPrice} 물량 : {virtualSellBlock.nOrderVolume}{NEW_LINE}{beforeList[beforeCnt - 1].sAccumMsg}";
                                }
                                else
                                {
                                    arrowControl.ArrowColor = GetArrowStepColor(1);
                                    new ToolTip().SetToolTip(arrowControl, $"-----------------현재매도주문--------------{NEW_LINE}시간 : {virtualSellBlock.nOrderTime} 가격 : {virtualSellBlock.nOrderPrice} 물량 : {virtualSellBlock.nOrderVolume} 잔량 : {virtualSellBlock.nOrderVolume - virtualSellBlock.nProcessedVolume}{NEW_LINE}");
                                    toCancelDict[(SELL_RESERVE, virtualSellBlock.nOrderPrice)] = new List<PrevCancel>();
                                    cancelInfo.sAccumMsg = $"-----------------현재매도주문--------------{NEW_LINE}시간 : {virtualSellBlock.nOrderTime} 가격 : {virtualSellBlock.nOrderPrice} 물량 : {virtualSellBlock.nOrderVolume}{NEW_LINE}";
                                }

                                toCancelDict[(SELL_RESERVE, virtualSellBlock.nOrderPrice)].Add(cancelInfo);
                                arrowControl.Name = $"^ S {sOrderId}";
                                arrowList.Add(arrowControl);


                            }

                        }

                        for (int i = arrowList.Count - 1; i >= 0; i--)
                            historyChart.Controls.Add(arrowList[i]); // 컨트롤은 먼저 들어간것이 가장 나중에 그려진다.

                        //==============================================================================================================

                    }

                    isAllSelledLabel.Text = $"매도완료 : {curEa.myTradeManager.nTotalSelled}";
                    isSellingLabel.Text = $"매도중 : {curEa.myTradeManager.nTotalSelling}";
                    isAllBuyedLabel.Text = $"총매수 : {curEa.myTradeManager.nTotalBuyed}";
                    restVolumeLabel.Text = $"잔량 : {curEa.myTradeManager.nTotalBuyed - (curEa.myTradeManager.nTotalSelling + curEa.myTradeManager.nTotalSelled)}";
                }
                catch { }
            }

            if (historyChart.InvokeRequired)
                historyChart.Invoke(new MethodInvoker(RefreshCancelConfirm));
            else
                RefreshCancelConfirm();
        }

        public struct PrevCancel
        {
            public string sPrevOrderId;
            public string sAccumMsg;
        }

        public Color GetArrowStepColor(int n)
        {
            Color retC;
            switch (n)
            {
                case 1:
                    retC = Color.Red;
                    break;
                case 2:
                    retC = Color.Orange;
                    break;
                case 3:
                    retC = Color.Yellow;
                    break;
                case 4:
                    retC = Color.Green;
                    break;
                case 5:
                    retC = Color.Blue;
                    break;
                case 6:
                    retC = Color.Navy;
                    break;
                case 7:
                    retC = Color.Purple;
                    break;
                default:
                    retC = Color.Black;
                    break;
            }
            return retC;
        }

        // 

        public int nTimerMilliSec = 100;
        public const int TIMER_MOVING = 100;

        public void TimerButtonClickHandler(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(timerUpButton))
                {
                    nTimerMilliSec += TIMER_MOVING;
                    timer.Interval = nTimerMilliSec;
                    timerLabel.Text = nTimerMilliSec.ToString();
                }
                else if (sender.Equals(timerDownButton))
                {
                    if (nTimerMilliSec > 100)
                    {
                        nTimerMilliSec -= TIMER_MOVING;
                        timer.Interval = nTimerMilliSec;
                        timerLabel.Text = nTimerMilliSec.ToString();
                    }
                }
            }
            catch
            {
                nTimerMilliSec = 100;
                timer.Interval = nTimerMilliSec;
                timerLabel.Text = nTimerMilliSec.ToString();
            }
        }


        public void SetCurGraphics()
        {
            try
            {
                gp = historyChart.CreateGraphics();
            }
            catch { }
        }
    }


    #endregion
}
