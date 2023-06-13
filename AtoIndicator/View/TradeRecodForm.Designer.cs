
namespace AtoIndicator.View.TradeRecod
{
    partial class TradeRecodForm
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
            this.tradeRecordListView = new System.Windows.Forms.ListView();
            this.todayTotalResultTextBox = new System.Windows.Forms.TextBox();
            this.totalClockLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.메뉴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.수동매도ctrlSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.realTradeListView = new System.Windows.Forms.ListView();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tradeRecordListView
            // 
            this.tradeRecordListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tradeRecordListView.FullRowSelect = true;
            this.tradeRecordListView.HideSelection = false;
            this.tradeRecordListView.Location = new System.Drawing.Point(3, 3);
            this.tradeRecordListView.Name = "tradeRecordListView";
            this.tradeRecordListView.Size = new System.Drawing.Size(1165, 473);
            this.tradeRecordListView.TabIndex = 0;
            this.tradeRecordListView.UseCompatibleStateImageBehavior = false;
            // 
            // todayTotalResultTextBox
            // 
            this.todayTotalResultTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.todayTotalResultTextBox.Location = new System.Drawing.Point(13, 535);
            this.todayTotalResultTextBox.Multiline = true;
            this.todayTotalResultTextBox.Name = "todayTotalResultTextBox";
            this.todayTotalResultTextBox.ReadOnly = true;
            this.todayTotalResultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.todayTotalResultTextBox.Size = new System.Drawing.Size(1156, 97);
            this.todayTotalResultTextBox.TabIndex = 5;
            // 
            // totalClockLabel
            // 
            this.totalClockLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalClockLabel.AutoSize = true;
            this.totalClockLabel.Font = new System.Drawing.Font("굴림", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalClockLabel.Location = new System.Drawing.Point(997, 7);
            this.totalClockLabel.Name = "totalClockLabel";
            this.totalClockLabel.Size = new System.Drawing.Size(0, 19);
            this.totalClockLabel.TabIndex = 6;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.메뉴ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1179, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 메뉴ToolStripMenuItem
            // 
            this.메뉴ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statisticToolStripMenuItem,
            this.수동매도ctrlSToolStripMenuItem});
            this.메뉴ToolStripMenuItem.Name = "메뉴ToolStripMenuItem";
            this.메뉴ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.메뉴ToolStripMenuItem.Text = "메뉴";
            // 
            // statisticToolStripMenuItem
            // 
            this.statisticToolStripMenuItem.Name = "statisticToolStripMenuItem";
            this.statisticToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.statisticToolStripMenuItem.Text = "전략현황기록( R )";
            // 
            // 수동매도ctrlSToolStripMenuItem
            // 
            this.수동매도ctrlSToolStripMenuItem.Enabled = false;
            this.수동매도ctrlSToolStripMenuItem.Name = "수동매도ctrlSToolStripMenuItem";
            this.수동매도ctrlSToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.수동매도ctrlSToolStripMenuItem.Text = "수동매도(ctrl + S)";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1179, 505);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tradeRecordListView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1171, 479);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "모의매매";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.realTradeListView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(341, 272);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "실매매";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // realTradeListView
            // 
            this.realTradeListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realTradeListView.FullRowSelect = true;
            this.realTradeListView.HideSelection = false;
            this.realTradeListView.Location = new System.Drawing.Point(3, 3);
            this.realTradeListView.Name = "realTradeListView";
            this.realTradeListView.Size = new System.Drawing.Size(335, 266);
            this.realTradeListView.TabIndex = 0;
            this.realTradeListView.UseCompatibleStateImageBehavior = false;
            // 
            // TradeRecodForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 645);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.totalClockLabel);
            this.Controls.Add(this.todayTotalResultTextBox);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TradeRecodForm";
            this.Text = "TradeRecodForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView tradeRecordListView;
        private System.Windows.Forms.TextBox todayTotalResultTextBox;
        private System.Windows.Forms.Label totalClockLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 메뉴ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 수동매도ctrlSToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView realTradeListView;
    }
}