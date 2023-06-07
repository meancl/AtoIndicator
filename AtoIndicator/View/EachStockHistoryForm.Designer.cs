﻿
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
            this.label1 = new System.Windows.Forms.Label();
            this.moveLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.메뉴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showVarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gapLabel = new System.Windows.Forms.Label();
            this.powerLabel = new System.Windows.Forms.Label();
            this.reserveLabel = new System.Windows.Forms.Label();
            this.expansionLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.blockFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buyModeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.historyChart)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.historyChart.Location = new System.Drawing.Point(14, 55);
            this.historyChart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.historyChart.Size = new System.Drawing.Size(1043, 708);
            this.historyChart.TabIndex = 2;
            this.historyChart.Text = "chart1";
            // 
            // totalClockLabel
            // 
            this.totalClockLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalClockLabel.AutoSize = true;
            this.totalClockLabel.Font = new System.Drawing.Font("굴림", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalClockLabel.Location = new System.Drawing.Point(1090, 9);
            this.totalClockLabel.Name = "totalClockLabel";
            this.totalClockLabel.Size = new System.Drawing.Size(0, 24);
            this.totalClockLabel.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(898, 431);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "\r\n";
            // 
            // moveLabel
            // 
            this.moveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveLabel.AutoSize = true;
            this.moveLabel.Location = new System.Drawing.Point(898, 465);
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
            this.menuStrip1.Size = new System.Drawing.Size(1320, 28);
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
            this.gapLabel.Location = new System.Drawing.Point(898, 399);
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
            this.powerLabel.Location = new System.Drawing.Point(898, 368);
            this.powerLabel.Name = "powerLabel";
            this.powerLabel.Size = new System.Drawing.Size(8, 17);
            this.powerLabel.TabIndex = 18;
            this.powerLabel.Text = "\r\n";
            // 
            // reserveLabel
            // 
            this.reserveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reserveLabel.AutoSize = true;
            this.reserveLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.reserveLabel.Location = new System.Drawing.Point(897, 330);
            this.reserveLabel.Name = "reserveLabel";
            this.reserveLabel.Size = new System.Drawing.Size(8, 17);
            this.reserveLabel.TabIndex = 19;
            this.reserveLabel.Text = "\r\n";
            // 
            // expansionLabel
            // 
            this.expansionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.expansionLabel.AutoSize = true;
            this.expansionLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.expansionLabel.Location = new System.Drawing.Point(897, 297);
            this.expansionLabel.Name = "expansionLabel";
            this.expansionLabel.Size = new System.Drawing.Size(8, 17);
            this.expansionLabel.TabIndex = 20;
            this.expansionLabel.Text = "\r\n";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.blockFlowLayoutPanel);
            this.groupBox1.Location = new System.Drawing.Point(1063, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 695);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "매매블록 선택";
            // 
            // blockFlowLayoutPanel
            // 
            this.blockFlowLayoutPanel.AutoScroll = true;
            this.blockFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blockFlowLayoutPanel.Location = new System.Drawing.Point(3, 21);
            this.blockFlowLayoutPanel.Name = "blockFlowLayoutPanel";
            this.blockFlowLayoutPanel.Size = new System.Drawing.Size(239, 671);
            this.blockFlowLayoutPanel.TabIndex = 0;
            // 
            // buyModeLabel
            // 
            this.buyModeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buyModeLabel.AutoSize = true;
            this.buyModeLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buyModeLabel.Location = new System.Drawing.Point(880, 16);
            this.buyModeLabel.Name = "buyModeLabel";
            this.buyModeLabel.Size = new System.Drawing.Size(48, 17);
            this.buyModeLabel.TabIndex = 22;
            this.buyModeLabel.Text = "false";
            // 
            // EachStockHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 792);
            this.Controls.Add(this.buyModeLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.expansionLabel);
            this.Controls.Add(this.reserveLabel);
            this.Controls.Add(this.powerLabel);
            this.Controls.Add(this.gapLabel);
            this.Controls.Add(this.moveLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.totalClockLabel);
            this.Controls.Add(this.historyChart);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "EachStockHistoryForm";
            this.Text = "EachStockHistory";
            ((System.ComponentModel.ISupportInitialize)(this.historyChart)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart historyChart;
        private System.Windows.Forms.Label totalClockLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label moveLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 메뉴ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showVarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
        private System.Windows.Forms.Label gapLabel;
        private System.Windows.Forms.Label powerLabel;
        private System.Windows.Forms.Label reserveLabel;
        private System.Windows.Forms.Label expansionLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel blockFlowLayoutPanel;
        private System.Windows.Forms.Label buyModeLabel;
    }
}