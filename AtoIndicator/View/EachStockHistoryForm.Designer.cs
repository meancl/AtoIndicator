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
            this.label1 = new System.Windows.Forms.Label();
            this.moveLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.메뉴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showVarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buyedBlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gapLabel = new System.Windows.Forms.Label();
            this.powerLabel = new System.Windows.Forms.Label();
            this.reserveLabel = new System.Windows.Forms.Label();
            this.expansionLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.blockFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
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
            chartArea2.CursorX.IsUserSelectionEnabled = true;
            chartArea2.Name = "TotalArea";
            this.historyChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.historyChart.Legends.Add(legend2);
            this.historyChart.Location = new System.Drawing.Point(14, 55);
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
            this.label1.Location = new System.Drawing.Point(891, 378);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "\r\n";
            // 
            // moveLabel
            // 
            this.moveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveLabel.AutoSize = true;
            this.moveLabel.Location = new System.Drawing.Point(891, 412);
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
            this.buyedBlockToolStripMenuItem,
            this.showLogToolStripMenuItem});
            this.메뉴ToolStripMenuItem.Name = "메뉴ToolStripMenuItem";
            this.메뉴ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.메뉴ToolStripMenuItem.Text = "메뉴";
            // 
            // showVarToolStripMenuItem
            // 
            this.showVarToolStripMenuItem.Name = "showVarToolStripMenuItem";
            this.showVarToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.showVarToolStripMenuItem.Text = "변수출력( V )";
            // 
            // buyedBlockToolStripMenuItem
            // 
            this.buyedBlockToolStripMenuItem.Name = "buyedBlockToolStripMenuItem";
            this.buyedBlockToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.buyedBlockToolStripMenuItem.Text = "매매블럭 확인( B )";
            // 
            // showLogToolStripMenuItem
            // 
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            this.showLogToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.showLogToolStripMenuItem.Text = "로그 확인( L )";
            // 
            // gapLabel
            // 
            this.gapLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gapLabel.AutoSize = true;
            this.gapLabel.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gapLabel.Location = new System.Drawing.Point(891, 346);
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
            this.powerLabel.Location = new System.Drawing.Point(891, 315);
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
            this.reserveLabel.Location = new System.Drawing.Point(890, 277);
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
            this.expansionLabel.Location = new System.Drawing.Point(890, 244);
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
            this.blockFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blockFlowLayoutPanel.Location = new System.Drawing.Point(3, 21);
            this.blockFlowLayoutPanel.Name = "blockFlowLayoutPanel";
            this.blockFlowLayoutPanel.Size = new System.Drawing.Size(239, 671);
            this.blockFlowLayoutPanel.TabIndex = 0;
            // 
            // EachStockHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 792);
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
        private System.Windows.Forms.ToolStripMenuItem buyedBlockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
        private System.Windows.Forms.Label gapLabel;
        private System.Windows.Forms.Label powerLabel;
        private System.Windows.Forms.Label reserveLabel;
        private System.Windows.Forms.Label expansionLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel blockFlowLayoutPanel;
    }
}