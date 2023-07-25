
namespace AtoIndicator.View.EachStockHistory
{
    partial class EachStockHistoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.historyChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.totalClockLabel = new System.Windows.Forms.Label();
            this.curLocLabel = new System.Windows.Forms.Label();
            this.moveLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.메뉴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showVarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gapLabel = new System.Windows.Forms.Label();
            this.powerLabel = new System.Windows.Forms.Label();
            this.expansionLabel = new System.Windows.Forms.Label();
            this.paperBlockFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buyModeLabel = new System.Windows.Forms.Label();
            this.wheelLabel = new System.Windows.Forms.Label();
            this.curLocPowerLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.realBlockFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.isAllBuyedLabel = new System.Windows.Forms.Label();
            this.isSellingLabel = new System.Windows.Forms.Label();
            this.isAllSelledLabel = new System.Windows.Forms.Label();
            this.restVolumeLabel = new System.Windows.Forms.Label();
            this.depositLabel = new System.Windows.Forms.Label();
            this.tradeMethodLabel = new System.Windows.Forms.Label();
            this.realBuyReserveLabel = new System.Windows.Forms.Label();
            this.reserveChosenLabel = new System.Windows.Forms.Label();
            this.curMyProfitLabel = new System.Windows.Forms.Label();
            this.curSpeedLabel = new System.Windows.Forms.Label();
            this.curHogaRatioLabel = new System.Windows.Forms.Label();
            this.curHitNumLabel = new System.Windows.Forms.Label();
            this.curPureTradePriceLabel = new System.Windows.Forms.Label();
            this.curTradePriceLabel = new System.Windows.Forms.Label();
            this.curCheckLineIdxLabel = new System.Windows.Forms.Label();
            this.isViLabel = new System.Windows.Forms.Label();
            this.priceViewLabel = new System.Windows.Forms.Label();
            this.priceLabel = new System.Windows.Forms.Label();
            this.timerUpButton = new System.Windows.Forms.Button();
            this.timerDownButton = new System.Windows.Forms.Button();
            this.timerLabel = new System.Windows.Forms.Label();
            this.nPositiveBomb2000Label = new System.Windows.Forms.Label();
            this.fTradeComparedLabel = new System.Windows.Forms.Label();
            this.fTradeComparedWithTimeLabel = new System.Windows.Forms.Label();
            this.nPositiveBomb5000Label = new System.Windows.Forms.Label();
            this.nNegativeBomb2000Label = new System.Windows.Forms.Label();
            this.nNegativeBomb5000Label = new System.Windows.Forms.Label();
            this.fTradeStrengthLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.historyChart)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // historyChart
            // 
            this.historyChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.Name = "TotalArea";
            this.historyChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.historyChart.Legends.Add(legend1);
            this.historyChart.Location = new System.Drawing.Point(16, 94);
            this.historyChart.Name = "historyChart";
            series1.ChartArea = "TotalArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.Legend = "Legend1";
            series1.Name = "MinuteStick";
            series1.YValuesPerPoint = 4;
            series2.ChartArea = "TotalArea";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Red;
            series2.Legend = "Legend1";
            series2.Name = "Ma20m";
            series3.ChartArea = "TotalArea";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Purple;
            series3.Legend = "Legend1";
            series3.Name = "Ma1h";
            series4.ChartArea = "TotalArea";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.DarkOrange;
            series4.Legend = "Legend1";
            series4.Name = "Ma2h";
            series5.ChartArea = "TotalArea";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.RoyalBlue;
            series5.Enabled = false;
            series5.Legend = "Legend1";
            series5.Name = "Ma20mGap";
            series6.ChartArea = "TotalArea";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Color = System.Drawing.Color.LimeGreen;
            series6.Enabled = false;
            series6.Legend = "Legend1";
            series6.Name = "Ma1hGap";
            series7.ChartArea = "TotalArea";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.Navy;
            series7.Enabled = false;
            series7.Legend = "Legend1";
            series7.Name = "Ma2hGap";
            this.historyChart.Series.Add(series1);
            this.historyChart.Series.Add(series2);
            this.historyChart.Series.Add(series3);
            this.historyChart.Series.Add(series4);
            this.historyChart.Series.Add(series5);
            this.historyChart.Series.Add(series6);
            this.historyChart.Series.Add(series7);
            this.historyChart.Size = new System.Drawing.Size(803, 476);
            this.historyChart.TabIndex = 2;
            this.historyChart.Text = "chart1";
            // 
            // totalClockLabel
            // 
            this.totalClockLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalClockLabel.AutoSize = true;
            this.totalClockLabel.Font = new System.Drawing.Font("굴림", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalClockLabel.Location = new System.Drawing.Point(849, 7);
            this.totalClockLabel.Name = "totalClockLabel";
            this.totalClockLabel.Size = new System.Drawing.Size(0, 19);
            this.totalClockLabel.TabIndex = 3;
            // 
            // curLocLabel
            // 
            this.curLocLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.curLocLabel.AutoSize = true;
            this.curLocLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curLocLabel.Location = new System.Drawing.Point(680, 294);
            this.curLocLabel.Name = "curLocLabel";
            this.curLocLabel.Size = new System.Drawing.Size(7, 13);
            this.curLocLabel.TabIndex = 8;
            this.curLocLabel.Text = "\r\n";
            // 
            // moveLabel
            // 
            this.moveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.moveLabel.AutoSize = true;
            this.moveLabel.Location = new System.Drawing.Point(680, 380);
            this.moveLabel.Name = "moveLabel";
            this.moveLabel.Size = new System.Drawing.Size(5, 12);
            this.moveLabel.TabIndex = 11;
            this.moveLabel.Text = "\r\n";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.메뉴ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1050, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 메뉴ToolStripMenuItem
            // 
            this.메뉴ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showVarToolStripMenuItem,
            this.showLogToolStripMenuItem});
            this.메뉴ToolStripMenuItem.Name = "메뉴ToolStripMenuItem";
            this.메뉴ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.메뉴ToolStripMenuItem.Text = "메뉴";
            // 
            // showVarToolStripMenuItem
            // 
            this.showVarToolStripMenuItem.Name = "showVarToolStripMenuItem";
            this.showVarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showVarToolStripMenuItem.Text = "변수출력( V )";
            // 
            // showLogToolStripMenuItem
            // 
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            this.showLogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showLogToolStripMenuItem.Text = "로그 확인( L )";
            // 
            // gapLabel
            // 
            this.gapLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gapLabel.AutoSize = true;
            this.gapLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gapLabel.Location = new System.Drawing.Point(680, 270);
            this.gapLabel.Name = "gapLabel";
            this.gapLabel.Size = new System.Drawing.Size(7, 13);
            this.gapLabel.TabIndex = 17;
            this.gapLabel.Text = "\r\n";
            // 
            // powerLabel
            // 
            this.powerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.powerLabel.AutoSize = true;
            this.powerLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.powerLabel.Location = new System.Drawing.Point(680, 247);
            this.powerLabel.Name = "powerLabel";
            this.powerLabel.Size = new System.Drawing.Size(7, 13);
            this.powerLabel.TabIndex = 18;
            this.powerLabel.Text = "\r\n";
            // 
            // expansionLabel
            // 
            this.expansionLabel.AutoSize = true;
            this.expansionLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.expansionLabel.Location = new System.Drawing.Point(72, 40);
            this.expansionLabel.Name = "expansionLabel";
            this.expansionLabel.Size = new System.Drawing.Size(62, 11);
            this.expansionLabel.TabIndex = 20;
            this.expansionLabel.Text = "확장 : 0 0";
            // 
            // paperBlockFlowLayoutPanel
            // 
            this.paperBlockFlowLayoutPanel.AutoScroll = true;
            this.paperBlockFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paperBlockFlowLayoutPanel.Location = new System.Drawing.Point(3, 2);
            this.paperBlockFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.paperBlockFlowLayoutPanel.Name = "paperBlockFlowLayoutPanel";
            this.paperBlockFlowLayoutPanel.Size = new System.Drawing.Size(203, 328);
            this.paperBlockFlowLayoutPanel.TabIndex = 0;
            // 
            // buyModeLabel
            // 
            this.buyModeLabel.AutoSize = true;
            this.buyModeLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buyModeLabel.Location = new System.Drawing.Point(72, 13);
            this.buyModeLabel.Name = "buyModeLabel";
            this.buyModeLabel.Size = new System.Drawing.Size(121, 11);
            this.buyModeLabel.TabIndex = 22;
            this.buyModeLabel.Text = "buy : NONE_MODE";
            // 
            // wheelLabel
            // 
            this.wheelLabel.AutoSize = true;
            this.wheelLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.wheelLabel.Location = new System.Drawing.Point(210, 13);
            this.wheelLabel.Name = "wheelLabel";
            this.wheelLabel.Size = new System.Drawing.Size(64, 11);
            this.wheelLabel.TabIndex = 23;
            this.wheelLabel.Text = "wheel : 0";
            // 
            // curLocPowerLabel
            // 
            this.curLocPowerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.curLocPowerLabel.AutoSize = true;
            this.curLocPowerLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curLocPowerLabel.Location = new System.Drawing.Point(679, 316);
            this.curLocPowerLabel.Name = "curLocPowerLabel";
            this.curLocPowerLabel.Size = new System.Drawing.Size(7, 13);
            this.curLocPowerLabel.TabIndex = 24;
            this.curLocPowerLabel.Text = "\r\n";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(821, 213);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(217, 358);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.realBlockFlowLayoutPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(209, 332);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "실매매";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // realBlockFlowLayoutPanel
            // 
            this.realBlockFlowLayoutPanel.AutoScroll = true;
            this.realBlockFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realBlockFlowLayoutPanel.Location = new System.Drawing.Point(3, 2);
            this.realBlockFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.realBlockFlowLayoutPanel.Name = "realBlockFlowLayoutPanel";
            this.realBlockFlowLayoutPanel.Size = new System.Drawing.Size(203, 328);
            this.realBlockFlowLayoutPanel.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.paperBlockFlowLayoutPanel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(209, 332);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "모의매매";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // isAllBuyedLabel
            // 
            this.isAllBuyedLabel.AutoSize = true;
            this.isAllBuyedLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.isAllBuyedLabel.Location = new System.Drawing.Point(310, 12);
            this.isAllBuyedLabel.Name = "isAllBuyedLabel";
            this.isAllBuyedLabel.Size = new System.Drawing.Size(62, 11);
            this.isAllBuyedLabel.TabIndex = 26;
            this.isAllBuyedLabel.Text = "총매수 : 0";
            // 
            // isSellingLabel
            // 
            this.isSellingLabel.AutoSize = true;
            this.isSellingLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.isSellingLabel.Location = new System.Drawing.Point(415, 13);
            this.isSellingLabel.Name = "isSellingLabel";
            this.isSellingLabel.Size = new System.Drawing.Size(62, 11);
            this.isSellingLabel.TabIndex = 27;
            this.isSellingLabel.Text = "매도중 : 0";
            // 
            // isAllSelledLabel
            // 
            this.isAllSelledLabel.AutoSize = true;
            this.isAllSelledLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.isAllSelledLabel.Location = new System.Drawing.Point(519, 12);
            this.isAllSelledLabel.Name = "isAllSelledLabel";
            this.isAllSelledLabel.Size = new System.Drawing.Size(74, 11);
            this.isAllSelledLabel.TabIndex = 28;
            this.isAllSelledLabel.Text = "매도완료 : 0";
            // 
            // restVolumeLabel
            // 
            this.restVolumeLabel.AutoSize = true;
            this.restVolumeLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.restVolumeLabel.Location = new System.Drawing.Point(628, 13);
            this.restVolumeLabel.Name = "restVolumeLabel";
            this.restVolumeLabel.Size = new System.Drawing.Size(50, 11);
            this.restVolumeLabel.TabIndex = 29;
            this.restVolumeLabel.Text = "잔량 : 0";
            // 
            // depositLabel
            // 
            this.depositLabel.AutoSize = true;
            this.depositLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.depositLabel.Location = new System.Drawing.Point(725, 12);
            this.depositLabel.Name = "depositLabel";
            this.depositLabel.Size = new System.Drawing.Size(62, 11);
            this.depositLabel.TabIndex = 30;
            this.depositLabel.Text = "예수금 : 0";
            // 
            // tradeMethodLabel
            // 
            this.tradeMethodLabel.AutoSize = true;
            this.tradeMethodLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tradeMethodLabel.Location = new System.Drawing.Point(212, 40);
            this.tradeMethodLabel.Name = "tradeMethodLabel";
            this.tradeMethodLabel.Size = new System.Drawing.Size(99, 11);
            this.tradeMethodLabel.TabIndex = 31;
            this.tradeMethodLabel.Text = "매매기법 : None";
            // 
            // realBuyReserveLabel
            // 
            this.realBuyReserveLabel.AutoSize = true;
            this.realBuyReserveLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.realBuyReserveLabel.Location = new System.Drawing.Point(72, 69);
            this.realBuyReserveLabel.Name = "realBuyReserveLabel";
            this.realBuyReserveLabel.Size = new System.Drawing.Size(83, 11);
            this.realBuyReserveLabel.TabIndex = 35;
            this.realBuyReserveLabel.Text = "해당예약 : No";
            // 
            // reserveChosenLabel
            // 
            this.reserveChosenLabel.AutoSize = true;
            this.reserveChosenLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.reserveChosenLabel.Location = new System.Drawing.Point(212, 69);
            this.reserveChosenLabel.Name = "reserveChosenLabel";
            this.reserveChosenLabel.Size = new System.Drawing.Size(53, 11);
            this.reserveChosenLabel.TabIndex = 36;
            this.reserveChosenLabel.Text = "채택 : ()";
            // 
            // curMyProfitLabel
            // 
            this.curMyProfitLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.curMyProfitLabel.AutoSize = true;
            this.curMyProfitLabel.Location = new System.Drawing.Point(680, 339);
            this.curMyProfitLabel.Name = "curMyProfitLabel";
            this.curMyProfitLabel.Size = new System.Drawing.Size(5, 12);
            this.curMyProfitLabel.TabIndex = 37;
            this.curMyProfitLabel.Text = "\r\n";
            // 
            // curSpeedLabel
            // 
            this.curSpeedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curSpeedLabel.AutoSize = true;
            this.curSpeedLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curSpeedLabel.Location = new System.Drawing.Point(566, 42);
            this.curSpeedLabel.Name = "curSpeedLabel";
            this.curSpeedLabel.Size = new System.Drawing.Size(50, 11);
            this.curSpeedLabel.TabIndex = 38;
            this.curSpeedLabel.Text = "속도 : 0";
            // 
            // curHogaRatioLabel
            // 
            this.curHogaRatioLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curHogaRatioLabel.AutoSize = true;
            this.curHogaRatioLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curHogaRatioLabel.Location = new System.Drawing.Point(660, 41);
            this.curHogaRatioLabel.Name = "curHogaRatioLabel";
            this.curHogaRatioLabel.Size = new System.Drawing.Size(62, 11);
            this.curHogaRatioLabel.TabIndex = 39;
            this.curHogaRatioLabel.Text = "호가비 : 0";
            // 
            // curHitNumLabel
            // 
            this.curHitNumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curHitNumLabel.AutoSize = true;
            this.curHitNumLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curHitNumLabel.Location = new System.Drawing.Point(941, 40);
            this.curHitNumLabel.Name = "curHitNumLabel";
            this.curHitNumLabel.Size = new System.Drawing.Size(50, 11);
            this.curHitNumLabel.TabIndex = 40;
            this.curHitNumLabel.Text = "히트 : 0";
            // 
            // curPureTradePriceLabel
            // 
            this.curPureTradePriceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curPureTradePriceLabel.AutoSize = true;
            this.curPureTradePriceLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curPureTradePriceLabel.Location = new System.Drawing.Point(765, 41);
            this.curPureTradePriceLabel.Name = "curPureTradePriceLabel";
            this.curPureTradePriceLabel.Size = new System.Drawing.Size(50, 11);
            this.curPureTradePriceLabel.TabIndex = 41;
            this.curPureTradePriceLabel.Text = "매수 : 0";
            // 
            // curTradePriceLabel
            // 
            this.curTradePriceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curTradePriceLabel.AutoSize = true;
            this.curTradePriceLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curTradePriceLabel.Location = new System.Drawing.Point(855, 41);
            this.curTradePriceLabel.Name = "curTradePriceLabel";
            this.curTradePriceLabel.Size = new System.Drawing.Size(50, 11);
            this.curTradePriceLabel.TabIndex = 42;
            this.curTradePriceLabel.Text = "대금 : 0";
            // 
            // curCheckLineIdxLabel
            // 
            this.curCheckLineIdxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.curCheckLineIdxLabel.AutoSize = true;
            this.curCheckLineIdxLabel.Location = new System.Drawing.Point(680, 360);
            this.curCheckLineIdxLabel.Name = "curCheckLineIdxLabel";
            this.curCheckLineIdxLabel.Size = new System.Drawing.Size(5, 12);
            this.curCheckLineIdxLabel.TabIndex = 43;
            this.curCheckLineIdxLabel.Text = "\r\n";
            // 
            // isViLabel
            // 
            this.isViLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.isViLabel.AutoSize = true;
            this.isViLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.isViLabel.Location = new System.Drawing.Point(1017, 40);
            this.isViLabel.Name = "isViLabel";
            this.isViLabel.Size = new System.Drawing.Size(0, 11);
            this.isViLabel.TabIndex = 44;
            // 
            // priceViewLabel
            // 
            this.priceViewLabel.AutoSize = true;
            this.priceViewLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.priceViewLabel.Location = new System.Drawing.Point(415, 69);
            this.priceViewLabel.Name = "priceViewLabel";
            this.priceViewLabel.Size = new System.Drawing.Size(53, 11);
            this.priceViewLabel.TabIndex = 46;
            this.priceViewLabel.Text = "가격 : ()";
            // 
            // priceLabel
            // 
            this.priceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.priceLabel.AutoSize = true;
            this.priceLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.priceLabel.Location = new System.Drawing.Point(680, 226);
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size(7, 13);
            this.priceLabel.TabIndex = 47;
            this.priceLabel.Text = "\r\n";
            // 
            // timerUpButton
            // 
            this.timerUpButton.Location = new System.Drawing.Point(10, 25);
            this.timerUpButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.timerUpButton.Name = "timerUpButton";
            this.timerUpButton.Size = new System.Drawing.Size(43, 24);
            this.timerUpButton.TabIndex = 48;
            this.timerUpButton.Text = "▲";
            this.timerUpButton.UseVisualStyleBackColor = true;
            // 
            // timerDownButton
            // 
            this.timerDownButton.Location = new System.Drawing.Point(10, 66);
            this.timerDownButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.timerDownButton.Name = "timerDownButton";
            this.timerDownButton.Size = new System.Drawing.Size(43, 23);
            this.timerDownButton.TabIndex = 49;
            this.timerDownButton.Text = "▼";
            this.timerDownButton.UseVisualStyleBackColor = true;
            // 
            // timerLabel
            // 
            this.timerLabel.Location = new System.Drawing.Point(13, 51);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(39, 12);
            this.timerLabel.TabIndex = 0;
            this.timerLabel.Text = "100";
            // 
            // nPositiveBomb2000Label
            // 
            this.nPositiveBomb2000Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nPositiveBomb2000Label.AutoSize = true;
            this.nPositiveBomb2000Label.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nPositiveBomb2000Label.Location = new System.Drawing.Point(841, 102);
            this.nPositiveBomb2000Label.Name = "nPositiveBomb2000Label";
            this.nPositiveBomb2000Label.Size = new System.Drawing.Size(84, 11);
            this.nPositiveBomb2000Label.TabIndex = 50;
            this.nPositiveBomb2000Label.Text = "+폭탄2000 : 0";
            // 
            // fTradeComparedLabel
            // 
            this.fTradeComparedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fTradeComparedLabel.AutoSize = true;
            this.fTradeComparedLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.fTradeComparedLabel.Location = new System.Drawing.Point(566, 66);
            this.fTradeComparedLabel.Name = "fTradeComparedLabel";
            this.fTradeComparedLabel.Size = new System.Drawing.Size(74, 11);
            this.fTradeComparedLabel.TabIndex = 52;
            this.fTradeComparedLabel.Text = "전일대비 : 0";
            // 
            // fTradeComparedWithTimeLabel
            // 
            this.fTradeComparedWithTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fTradeComparedWithTimeLabel.AutoSize = true;
            this.fTradeComparedWithTimeLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.fTradeComparedWithTimeLabel.Location = new System.Drawing.Point(714, 66);
            this.fTradeComparedWithTimeLabel.Name = "fTradeComparedWithTimeLabel";
            this.fTradeComparedWithTimeLabel.Size = new System.Drawing.Size(91, 11);
            this.fTradeComparedWithTimeLabel.TabIndex = 53;
            this.fTradeComparedWithTimeLabel.Text = "동시간 대비 : 0";
            // 
            // nPositiveBomb5000Label
            // 
            this.nPositiveBomb5000Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nPositiveBomb5000Label.AutoSize = true;
            this.nPositiveBomb5000Label.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nPositiveBomb5000Label.Location = new System.Drawing.Point(841, 122);
            this.nPositiveBomb5000Label.Name = "nPositiveBomb5000Label";
            this.nPositiveBomb5000Label.Size = new System.Drawing.Size(84, 11);
            this.nPositiveBomb5000Label.TabIndex = 54;
            this.nPositiveBomb5000Label.Text = "+폭탄5000 : 0";
            // 
            // nNegativeBomb2000Label
            // 
            this.nNegativeBomb2000Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nNegativeBomb2000Label.AutoSize = true;
            this.nNegativeBomb2000Label.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nNegativeBomb2000Label.Location = new System.Drawing.Point(841, 154);
            this.nNegativeBomb2000Label.Name = "nNegativeBomb2000Label";
            this.nNegativeBomb2000Label.Size = new System.Drawing.Size(84, 11);
            this.nNegativeBomb2000Label.TabIndex = 55;
            this.nNegativeBomb2000Label.Text = "-폭탄2000 : 0";
            // 
            // nNegativeBomb5000Label
            // 
            this.nNegativeBomb5000Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nNegativeBomb5000Label.AutoSize = true;
            this.nNegativeBomb5000Label.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nNegativeBomb5000Label.Location = new System.Drawing.Point(841, 175);
            this.nNegativeBomb5000Label.Name = "nNegativeBomb5000Label";
            this.nNegativeBomb5000Label.Size = new System.Drawing.Size(84, 11);
            this.nNegativeBomb5000Label.TabIndex = 56;
            this.nNegativeBomb5000Label.Text = "-폭탄5000 : 0";
            // 
            // fTradeStrengthLabel
            // 
            this.fTradeStrengthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fTradeStrengthLabel.AutoSize = true;
            this.fTradeStrengthLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.fTradeStrengthLabel.Location = new System.Drawing.Point(880, 66);
            this.fTradeStrengthLabel.Name = "fTradeStrengthLabel";
            this.fTradeStrengthLabel.Size = new System.Drawing.Size(74, 11);
            this.fTradeStrengthLabel.TabIndex = 57;
            this.fTradeStrengthLabel.Text = "체결강도 : 0";
            // 
            // EachStockHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 582);
            this.Controls.Add(this.fTradeStrengthLabel);
            this.Controls.Add(this.nNegativeBomb5000Label);
            this.Controls.Add(this.nNegativeBomb2000Label);
            this.Controls.Add(this.nPositiveBomb5000Label);
            this.Controls.Add(this.fTradeComparedWithTimeLabel);
            this.Controls.Add(this.fTradeComparedLabel);
            this.Controls.Add(this.nPositiveBomb2000Label);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.timerDownButton);
            this.Controls.Add(this.timerUpButton);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.priceViewLabel);
            this.Controls.Add(this.isViLabel);
            this.Controls.Add(this.curCheckLineIdxLabel);
            this.Controls.Add(this.curTradePriceLabel);
            this.Controls.Add(this.curPureTradePriceLabel);
            this.Controls.Add(this.curHitNumLabel);
            this.Controls.Add(this.curHogaRatioLabel);
            this.Controls.Add(this.curSpeedLabel);
            this.Controls.Add(this.curMyProfitLabel);
            this.Controls.Add(this.reserveChosenLabel);
            this.Controls.Add(this.realBuyReserveLabel);
            this.Controls.Add(this.tradeMethodLabel);
            this.Controls.Add(this.depositLabel);
            this.Controls.Add(this.restVolumeLabel);
            this.Controls.Add(this.isAllSelledLabel);
            this.Controls.Add(this.isSellingLabel);
            this.Controls.Add(this.isAllBuyedLabel);
            this.Controls.Add(this.curLocPowerLabel);
            this.Controls.Add(this.moveLabel);
            this.Controls.Add(this.wheelLabel);
            this.Controls.Add(this.buyModeLabel);
            this.Controls.Add(this.curLocLabel);
            this.Controls.Add(this.expansionLabel);
            this.Controls.Add(this.powerLabel);
            this.Controls.Add(this.gapLabel);
            this.Controls.Add(this.totalClockLabel);
            this.Controls.Add(this.historyChart);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EachStockHistoryForm";
            this.Text = "EachStockHistory";
            ((System.ComponentModel.ISupportInitialize)(this.historyChart)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart historyChart;
        private System.Windows.Forms.Label totalClockLabel;
        private System.Windows.Forms.Label curLocLabel;
        private System.Windows.Forms.Label moveLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 메뉴ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showVarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
        private System.Windows.Forms.Label gapLabel;
        private System.Windows.Forms.Label powerLabel;
        private System.Windows.Forms.Label expansionLabel;
        private System.Windows.Forms.FlowLayoutPanel paperBlockFlowLayoutPanel;
        private System.Windows.Forms.Label buyModeLabel;
        private System.Windows.Forms.Label wheelLabel;
        private System.Windows.Forms.Label curLocPowerLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel realBlockFlowLayoutPanel;
        private System.Windows.Forms.Label isAllBuyedLabel;
        private System.Windows.Forms.Label isSellingLabel;
        private System.Windows.Forms.Label isAllSelledLabel;
        private System.Windows.Forms.Label restVolumeLabel;
        private System.Windows.Forms.Label depositLabel;
        private System.Windows.Forms.Label tradeMethodLabel;
        private System.Windows.Forms.Label realBuyReserveLabel;
        private System.Windows.Forms.Label reserveChosenLabel;
        private System.Windows.Forms.Label curMyProfitLabel;
        private System.Windows.Forms.Label curSpeedLabel;
        private System.Windows.Forms.Label curHogaRatioLabel;
        private System.Windows.Forms.Label curHitNumLabel;
        private System.Windows.Forms.Label curPureTradePriceLabel;
        private System.Windows.Forms.Label curTradePriceLabel;
        private System.Windows.Forms.Label curCheckLineIdxLabel;
        private System.Windows.Forms.Label isViLabel;
        private System.Windows.Forms.Label priceViewLabel;
        private System.Windows.Forms.Label priceLabel;
        private System.Windows.Forms.Button timerUpButton;
        private System.Windows.Forms.Button timerDownButton;
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Label nPositiveBomb2000Label;
        private System.Windows.Forms.Label fTradeComparedLabel;
        private System.Windows.Forms.Label fTradeComparedWithTimeLabel;
        private System.Windows.Forms.Label nPositiveBomb5000Label;
        private System.Windows.Forms.Label nNegativeBomb2000Label;
        private System.Windows.Forms.Label nNegativeBomb5000Label;
        private System.Windows.Forms.Label fTradeStrengthLabel;
    }
}