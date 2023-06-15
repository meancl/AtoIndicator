
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.realBlockFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.isAllBuyedLabel = new System.Windows.Forms.Label();
            this.isSellingLabel = new System.Windows.Forms.Label();
            this.isAllSelledLabel = new System.Windows.Forms.Label();
            this.restVolumeLabel = new System.Windows.Forms.Label();
            this.depositLabel = new System.Windows.Forms.Label();
            this.tradeMethodLabel = new System.Windows.Forms.Label();
            this.ctrlLabel = new System.Windows.Forms.Label();
            this.shiftLabel = new System.Windows.Forms.Label();
            this.spaceLabel = new System.Windows.Forms.Label();
            this.realBuyReserveLabel = new System.Windows.Forms.Label();
            this.reserveChosenLabel = new System.Windows.Forms.Label();
            this.curMyProfitLabel = new System.Windows.Forms.Label();
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
            chartArea2.CursorX.IsUserSelectionEnabled = true;
            chartArea2.Name = "TotalArea";
            this.historyChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.historyChart.Legends.Add(legend2);
            this.historyChart.Location = new System.Drawing.Point(14, 114);
            this.historyChart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.historyChart.Name = "historyChart";
            series8.ChartArea = "TotalArea";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series8.Legend = "Legend1";
            series8.Name = "MinuteStick";
            series8.YValuesPerPoint = 4;
            series9.ChartArea = "TotalArea";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Color = System.Drawing.Color.Red;
            series9.Legend = "Legend1";
            series9.Name = "Ma20m";
            series10.ChartArea = "TotalArea";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Color = System.Drawing.Color.Purple;
            series10.Legend = "Legend1";
            series10.Name = "Ma1h";
            series11.ChartArea = "TotalArea";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series11.Color = System.Drawing.Color.DarkOrange;
            series11.Legend = "Legend1";
            series11.Name = "Ma2h";
            series12.ChartArea = "TotalArea";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series12.Color = System.Drawing.Color.RoyalBlue;
            series12.Enabled = false;
            series12.Legend = "Legend1";
            series12.Name = "Ma20mGap";
            series13.ChartArea = "TotalArea";
            series13.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series13.Color = System.Drawing.Color.LimeGreen;
            series13.Enabled = false;
            series13.Legend = "Legend1";
            series13.Name = "Ma1hGap";
            series14.ChartArea = "TotalArea";
            series14.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series14.Color = System.Drawing.Color.Navy;
            series14.Enabled = false;
            series14.Legend = "Legend1";
            series14.Name = "Ma2hGap";
            this.historyChart.Series.Add(series8);
            this.historyChart.Series.Add(series9);
            this.historyChart.Series.Add(series10);
            this.historyChart.Series.Add(series11);
            this.historyChart.Series.Add(series12);
            this.historyChart.Series.Add(series13);
            this.historyChart.Series.Add(series14);
            this.historyChart.Size = new System.Drawing.Size(1091, 824);
            this.historyChart.TabIndex = 2;
            this.historyChart.Text = "chart1";
            // 
            // totalClockLabel
            // 
            this.totalClockLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalClockLabel.AutoSize = true;
            this.totalClockLabel.Font = new System.Drawing.Font("굴림", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalClockLabel.Location = new System.Drawing.Point(1138, 9);
            this.totalClockLabel.Name = "totalClockLabel";
            this.totalClockLabel.Size = new System.Drawing.Size(0, 24);
            this.totalClockLabel.TabIndex = 3;
            // 
            // curLocLabel
            // 
            this.curLocLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curLocLabel.AutoSize = true;
            this.curLocLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curLocLabel.Location = new System.Drawing.Point(946, 390);
            this.curLocLabel.Name = "curLocLabel";
            this.curLocLabel.Size = new System.Drawing.Size(8, 17);
            this.curLocLabel.TabIndex = 8;
            this.curLocLabel.Text = "\r\n";
            // 
            // moveLabel
            // 
            this.moveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveLabel.AutoSize = true;
            this.moveLabel.Location = new System.Drawing.Point(947, 499);
            this.moveLabel.Name = "moveLabel";
            this.moveLabel.Size = new System.Drawing.Size(7, 15);
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1368, 28);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 메뉴ToolStripMenuItem
            // 
            this.메뉴ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showVarToolStripMenuItem,
            this.showLogToolStripMenuItem});
            this.메뉴ToolStripMenuItem.Name = "메뉴ToolStripMenuItem";
            this.메뉴ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.메뉴ToolStripMenuItem.Text = "메뉴";
            // 
            // showVarToolStripMenuItem
            // 
            this.showVarToolStripMenuItem.Name = "showVarToolStripMenuItem";
            this.showVarToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.showVarToolStripMenuItem.Text = "변수출력( V )";
            // 
            // showLogToolStripMenuItem
            // 
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            this.showLogToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.showLogToolStripMenuItem.Text = "로그 확인( L )";
            // 
            // gapLabel
            // 
            this.gapLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gapLabel.AutoSize = true;
            this.gapLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gapLabel.Location = new System.Drawing.Point(946, 352);
            this.gapLabel.Name = "gapLabel";
            this.gapLabel.Size = new System.Drawing.Size(8, 17);
            this.gapLabel.TabIndex = 17;
            this.gapLabel.Text = "\r\n";
            // 
            // powerLabel
            // 
            this.powerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.powerLabel.AutoSize = true;
            this.powerLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.powerLabel.Location = new System.Drawing.Point(946, 314);
            this.powerLabel.Name = "powerLabel";
            this.powerLabel.Size = new System.Drawing.Size(8, 17);
            this.powerLabel.TabIndex = 18;
            this.powerLabel.Text = "\r\n";
            // 
            // expansionLabel
            // 
            this.expansionLabel.AutoSize = true;
            this.expansionLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.expansionLabel.Location = new System.Drawing.Point(82, 50);
            this.expansionLabel.Name = "expansionLabel";
            this.expansionLabel.Size = new System.Drawing.Size(79, 14);
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
            this.paperBlockFlowLayoutPanel.Size = new System.Drawing.Size(231, 791);
            this.paperBlockFlowLayoutPanel.TabIndex = 0;
            // 
            // buyModeLabel
            // 
            this.buyModeLabel.AutoSize = true;
            this.buyModeLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buyModeLabel.Location = new System.Drawing.Point(82, 16);
            this.buyModeLabel.Name = "buyModeLabel";
            this.buyModeLabel.Size = new System.Drawing.Size(150, 14);
            this.buyModeLabel.TabIndex = 22;
            this.buyModeLabel.Text = "buy : NONE_MODE";
            // 
            // wheelLabel
            // 
            this.wheelLabel.AutoSize = true;
            this.wheelLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.wheelLabel.Location = new System.Drawing.Point(240, 16);
            this.wheelLabel.Name = "wheelLabel";
            this.wheelLabel.Size = new System.Drawing.Size(76, 14);
            this.wheelLabel.TabIndex = 23;
            this.wheelLabel.Text = "wheel : 0";
            // 
            // curLocPowerLabel
            // 
            this.curLocPowerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curLocPowerLabel.AutoSize = true;
            this.curLocPowerLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curLocPowerLabel.Location = new System.Drawing.Point(946, 424);
            this.curLocPowerLabel.Name = "curLocPowerLabel";
            this.curLocPowerLabel.Size = new System.Drawing.Size(8, 17);
            this.curLocPowerLabel.TabIndex = 24;
            this.curLocPowerLabel.Text = "\r\n";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1111, 114);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(245, 824);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.paperBlockFlowLayoutPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(237, 795);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "모의매매";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.realBlockFlowLayoutPanel);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(237, 795);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "실매매";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // realBlockFlowLayoutPanel
            // 
            this.realBlockFlowLayoutPanel.AutoScroll = true;
            this.realBlockFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realBlockFlowLayoutPanel.Location = new System.Drawing.Point(3, 2);
            this.realBlockFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.realBlockFlowLayoutPanel.Name = "realBlockFlowLayoutPanel";
            this.realBlockFlowLayoutPanel.Size = new System.Drawing.Size(231, 791);
            this.realBlockFlowLayoutPanel.TabIndex = 0;
            // 
            // isAllBuyedLabel
            // 
            this.isAllBuyedLabel.AutoSize = true;
            this.isAllBuyedLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.isAllBuyedLabel.Location = new System.Drawing.Point(354, 15);
            this.isAllBuyedLabel.Name = "isAllBuyedLabel";
            this.isAllBuyedLabel.Size = new System.Drawing.Size(79, 14);
            this.isAllBuyedLabel.TabIndex = 26;
            this.isAllBuyedLabel.Text = "총매수 : 0";
            // 
            // isSellingLabel
            // 
            this.isSellingLabel.AutoSize = true;
            this.isSellingLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.isSellingLabel.Location = new System.Drawing.Point(474, 16);
            this.isSellingLabel.Name = "isSellingLabel";
            this.isSellingLabel.Size = new System.Drawing.Size(79, 14);
            this.isSellingLabel.TabIndex = 27;
            this.isSellingLabel.Text = "매도중 : 0";
            // 
            // isAllSelledLabel
            // 
            this.isAllSelledLabel.AutoSize = true;
            this.isAllSelledLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.isAllSelledLabel.Location = new System.Drawing.Point(593, 15);
            this.isAllSelledLabel.Name = "isAllSelledLabel";
            this.isAllSelledLabel.Size = new System.Drawing.Size(94, 14);
            this.isAllSelledLabel.TabIndex = 28;
            this.isAllSelledLabel.Text = "매도완료 : 0";
            // 
            // restVolumeLabel
            // 
            this.restVolumeLabel.AutoSize = true;
            this.restVolumeLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.restVolumeLabel.Location = new System.Drawing.Point(718, 16);
            this.restVolumeLabel.Name = "restVolumeLabel";
            this.restVolumeLabel.Size = new System.Drawing.Size(64, 14);
            this.restVolumeLabel.TabIndex = 29;
            this.restVolumeLabel.Text = "잔량 : 0";
            // 
            // depositLabel
            // 
            this.depositLabel.AutoSize = true;
            this.depositLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.depositLabel.Location = new System.Drawing.Point(829, 15);
            this.depositLabel.Name = "depositLabel";
            this.depositLabel.Size = new System.Drawing.Size(79, 14);
            this.depositLabel.TabIndex = 30;
            this.depositLabel.Text = "예수금 : 0";
            // 
            // tradeMethodLabel
            // 
            this.tradeMethodLabel.AutoSize = true;
            this.tradeMethodLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tradeMethodLabel.Location = new System.Drawing.Point(242, 50);
            this.tradeMethodLabel.Name = "tradeMethodLabel";
            this.tradeMethodLabel.Size = new System.Drawing.Size(123, 14);
            this.tradeMethodLabel.TabIndex = 31;
            this.tradeMethodLabel.Text = "매매기법 : None";
            // 
            // ctrlLabel
            // 
            this.ctrlLabel.AutoSize = true;
            this.ctrlLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ctrlLabel.Location = new System.Drawing.Point(714, 85);
            this.ctrlLabel.Name = "ctrlLabel";
            this.ctrlLabel.Size = new System.Drawing.Size(68, 14);
            this.ctrlLabel.TabIndex = 32;
            this.ctrlLabel.Text = "ctrl : No";
            // 
            // shiftLabel
            // 
            this.shiftLabel.AutoSize = true;
            this.shiftLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.shiftLabel.Location = new System.Drawing.Point(802, 85);
            this.shiftLabel.Name = "shiftLabel";
            this.shiftLabel.Size = new System.Drawing.Size(77, 14);
            this.shiftLabel.TabIndex = 33;
            this.shiftLabel.Text = "shift : No";
            // 
            // spaceLabel
            // 
            this.spaceLabel.AutoSize = true;
            this.spaceLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.spaceLabel.Location = new System.Drawing.Point(895, 85);
            this.spaceLabel.Name = "spaceLabel";
            this.spaceLabel.Size = new System.Drawing.Size(90, 14);
            this.spaceLabel.TabIndex = 34;
            this.spaceLabel.Text = "space : No";
            // 
            // realBuyReserveLabel
            // 
            this.realBuyReserveLabel.AutoSize = true;
            this.realBuyReserveLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.realBuyReserveLabel.Location = new System.Drawing.Point(242, 85);
            this.realBuyReserveLabel.Name = "realBuyReserveLabel";
            this.realBuyReserveLabel.Size = new System.Drawing.Size(105, 14);
            this.realBuyReserveLabel.TabIndex = 35;
            this.realBuyReserveLabel.Text = "해당예약 : No";
            // 
            // reserveChosenLabel
            // 
            this.reserveChosenLabel.AutoSize = true;
            this.reserveChosenLabel.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.reserveChosenLabel.Location = new System.Drawing.Point(82, 85);
            this.reserveChosenLabel.Name = "reserveChosenLabel";
            this.reserveChosenLabel.Size = new System.Drawing.Size(75, 14);
            this.reserveChosenLabel.TabIndex = 36;
            this.reserveChosenLabel.Text = "채택 : No";
            // 
            // curMyProfitLabel
            // 
            this.curMyProfitLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curMyProfitLabel.AutoSize = true;
            this.curMyProfitLabel.Location = new System.Drawing.Point(947, 460);
            this.curMyProfitLabel.Name = "curMyProfitLabel";
            this.curMyProfitLabel.Size = new System.Drawing.Size(7, 15);
            this.curMyProfitLabel.TabIndex = 37;
            this.curMyProfitLabel.Text = "\r\n";
            // 
            // EachStockHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1368, 966);
            this.Controls.Add(this.curMyProfitLabel);
            this.Controls.Add(this.reserveChosenLabel);
            this.Controls.Add(this.realBuyReserveLabel);
            this.Controls.Add(this.spaceLabel);
            this.Controls.Add(this.shiftLabel);
            this.Controls.Add(this.ctrlLabel);
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
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
        private System.Windows.Forms.Label ctrlLabel;
        private System.Windows.Forms.Label shiftLabel;
        private System.Windows.Forms.Label spaceLabel;
        private System.Windows.Forms.Label realBuyReserveLabel;
        private System.Windows.Forms.Label reserveChosenLabel;
        private System.Windows.Forms.Label curMyProfitLabel;
    }
}