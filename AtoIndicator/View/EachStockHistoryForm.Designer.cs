
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series21 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            chartArea3.CursorX.IsUserSelectionEnabled = true;
            chartArea3.Name = "TotalArea";
            this.historyChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.historyChart.Legends.Add(legend3);
            this.historyChart.Location = new System.Drawing.Point(14, 55);
            this.historyChart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.historyChart.Name = "historyChart";
            series15.ChartArea = "TotalArea";
            series15.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series15.Legend = "Legend1";
            series15.Name = "MinuteStick";
            series15.YValuesPerPoint = 4;
            series16.ChartArea = "TotalArea";
            series16.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series16.Color = System.Drawing.Color.Red;
            series16.Legend = "Legend1";
            series16.Name = "Ma20m";
            series17.ChartArea = "TotalArea";
            series17.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series17.Color = System.Drawing.Color.Purple;
            series17.Legend = "Legend1";
            series17.Name = "Ma1h";
            series18.ChartArea = "TotalArea";
            series18.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series18.Color = System.Drawing.Color.DarkOrange;
            series18.Legend = "Legend1";
            series18.Name = "Ma2h";
            series19.ChartArea = "TotalArea";
            series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series19.Color = System.Drawing.Color.RoyalBlue;
            series19.Enabled = false;
            series19.Legend = "Legend1";
            series19.Name = "Ma20mGap";
            series20.ChartArea = "TotalArea";
            series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series20.Color = System.Drawing.Color.LimeGreen;
            series20.Enabled = false;
            series20.Legend = "Legend1";
            series20.Name = "Ma1hGap";
            series21.ChartArea = "TotalArea";
            series21.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series21.Color = System.Drawing.Color.Navy;
            series21.Enabled = false;
            series21.Legend = "Legend1";
            series21.Name = "Ma2hGap";
            this.historyChart.Series.Add(series15);
            this.historyChart.Series.Add(series16);
            this.historyChart.Series.Add(series17);
            this.historyChart.Series.Add(series18);
            this.historyChart.Series.Add(series19);
            this.historyChart.Series.Add(series20);
            this.historyChart.Series.Add(series21);
            this.historyChart.Size = new System.Drawing.Size(1067, 744);
            this.historyChart.TabIndex = 2;
            this.historyChart.Text = "chart1";
            // 
            // totalClockLabel
            // 
            this.totalClockLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalClockLabel.AutoSize = true;
            this.totalClockLabel.Font = new System.Drawing.Font("굴림", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalClockLabel.Location = new System.Drawing.Point(1114, 9);
            this.totalClockLabel.Name = "totalClockLabel";
            this.totalClockLabel.Size = new System.Drawing.Size(0, 24);
            this.totalClockLabel.TabIndex = 3;
            // 
            // curLocLabel
            // 
            this.curLocLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curLocLabel.AutoSize = true;
            this.curLocLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.curLocLabel.Location = new System.Drawing.Point(922, 317);
            this.curLocLabel.Name = "curLocLabel";
            this.curLocLabel.Size = new System.Drawing.Size(8, 17);
            this.curLocLabel.TabIndex = 8;
            this.curLocLabel.Text = "\r\n";
            // 
            // moveLabel
            // 
            this.moveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveLabel.AutoSize = true;
            this.moveLabel.Location = new System.Drawing.Point(922, 488);
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
            this.menuStrip1.Size = new System.Drawing.Size(1344, 28);
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
            this.gapLabel.Location = new System.Drawing.Point(921, 285);
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
            this.powerLabel.Location = new System.Drawing.Point(922, 257);
            this.powerLabel.Name = "powerLabel";
            this.powerLabel.Size = new System.Drawing.Size(8, 17);
            this.powerLabel.TabIndex = 18;
            this.powerLabel.Text = "\r\n";
            // 
            // expansionLabel
            // 
            this.expansionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.expansionLabel.AutoSize = true;
            this.expansionLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.expansionLabel.Location = new System.Drawing.Point(82, 34);
            this.expansionLabel.Name = "expansionLabel";
            this.expansionLabel.Size = new System.Drawing.Size(8, 17);
            this.expansionLabel.TabIndex = 20;
            this.expansionLabel.Text = "\r\n";
            // 
            // paperBlockFlowLayoutPanel
            // 
            this.paperBlockFlowLayoutPanel.AutoScroll = true;
            this.paperBlockFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paperBlockFlowLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.paperBlockFlowLayoutPanel.Name = "paperBlockFlowLayoutPanel";
            this.paperBlockFlowLayoutPanel.Size = new System.Drawing.Size(231, 709);
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
            this.curLocPowerLabel.Location = new System.Drawing.Point(921, 347);
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
            this.tabControl1.Location = new System.Drawing.Point(1087, 55);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(245, 744);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.paperBlockFlowLayoutPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(237, 715);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "모의매매";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.realBlockFlowLayoutPanel);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(237, 715);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "실매매";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // realBlockFlowLayoutPanel
            // 
            this.realBlockFlowLayoutPanel.AutoScroll = true;
            this.realBlockFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realBlockFlowLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.realBlockFlowLayoutPanel.Name = "realBlockFlowLayoutPanel";
            this.realBlockFlowLayoutPanel.Size = new System.Drawing.Size(231, 709);
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
            // EachStockHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 828);
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
    }
}