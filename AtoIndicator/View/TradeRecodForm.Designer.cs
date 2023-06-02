
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
            this.sGubunTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sCodeName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nTradingNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nTradeNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nFakeBuyNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nFakeAssistantNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nFakeResistNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nPriceUpNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nPriceDownNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.isTradeStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fBuyPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nCurFb = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fEverageProfit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fTotalPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fCurPower = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fCurPowerWithOutGap = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.todayTotalResultTextBox = new System.Windows.Forms.TextBox();
            this.totalClockLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.메뉴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.수동매도ctrlSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tradeRecordListView
            // 
            this.tradeRecordListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tradeRecordListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.sGubunTag,
            this.sCode,
            this.sCodeName,
            this.nTradingNum,
            this.nTradeNum,
            this.nFakeBuyNum,
            this.nFakeAssistantNum,
            this.nFakeResistNum,
            this.nPriceUpNum,
            this.nPriceDownNum,
            this.isTradeStatus,
            this.fBuyPrice,
            this.nCurFb,
            this.fEverageProfit,
            this.fTotalPrice,
            this.fCurPower,
            this.fCurPowerWithOutGap});
            this.tradeRecordListView.FullRowSelect = true;
            this.tradeRecordListView.HideSelection = false;
            this.tradeRecordListView.Location = new System.Drawing.Point(12, 39);
            this.tradeRecordListView.Name = "tradeRecordListView";
            this.tradeRecordListView.Size = new System.Drawing.Size(1157, 481);
            this.tradeRecordListView.TabIndex = 0;
            this.tradeRecordListView.UseCompatibleStateImageBehavior = false;
            // 
            // sGubunTag
            // 
            this.sGubunTag.Text = "장구분";
            // 
            // sCode
            // 
            this.sCode.Text = "종목코드";
            // 
            // sCodeName
            // 
            this.sCodeName.Text = "종목명";
            // 
            // nTradingNum
            // 
            this.nTradingNum.Text = "매매중";
            // 
            // nTradeNum
            // 
            this.nTradeNum.Text = "실매매";
            // 
            // nFakeBuyNum
            // 
            this.nFakeBuyNum.Text = "가짜매수";
            // 
            // nFakeAssistantNum
            // 
            this.nFakeAssistantNum.Text = "가짜보조";
            // 
            // nFakeResistNum
            // 
            this.nFakeResistNum.Text = "가짜저항";
            // 
            // nPriceUpNum
            // 
            this.nPriceUpNum.Text = "가격업";
            // 
            // nPriceDownNum
            // 
            this.nPriceDownNum.Text = "가격다운";
            // 
            // isTradeStatus
            // 
            this.isTradeStatus.Text = "매매상태";
            // 
            // fBuyPrice
            // 
            this.fBuyPrice.Text = "매수가";
            // 
            // nCurFb
            // 
            this.nCurFb.Text = "현재가";
            // 
            // fEverageProfit
            // 
            this.fEverageProfit.Text = "평균이익(백분율)";
            // 
            // fTotalPrice
            // 
            this.fTotalPrice.Text = "총구매가격(백만원)";
            // 
            // fCurPower
            // 
            this.fCurPower.Text = "당일기세";
            // 
            // fCurPowerWithOutGap
            // 
            this.fCurPowerWithOutGap.Text = "갭제외";
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
            this.statisticToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.statisticToolStripMenuItem.Text = "전략현황기록( R )";
            // 
            // 수동매도ctrlSToolStripMenuItem
            // 
            this.수동매도ctrlSToolStripMenuItem.Enabled = false;
            this.수동매도ctrlSToolStripMenuItem.Name = "수동매도ctrlSToolStripMenuItem";
            this.수동매도ctrlSToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.수동매도ctrlSToolStripMenuItem.Text = "수동매도(ctrl + S)";
            // 
            // TradeRecodForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 645);
            this.Controls.Add(this.totalClockLabel);
            this.Controls.Add(this.todayTotalResultTextBox);
            this.Controls.Add(this.tradeRecordListView);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TradeRecodForm";
            this.Text = "TradeRecodForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView tradeRecordListView;
        private System.Windows.Forms.ColumnHeader sCode;
        private System.Windows.Forms.ColumnHeader sCodeName;
        private System.Windows.Forms.ColumnHeader isTradeStatus;
        private System.Windows.Forms.ColumnHeader nTradingNum;
        private System.Windows.Forms.ColumnHeader nTradeNum;
        private System.Windows.Forms.ColumnHeader nCurFb;
        private System.Windows.Forms.ColumnHeader fEverageProfit;
        private System.Windows.Forms.ColumnHeader fTotalPrice;
        private System.Windows.Forms.ColumnHeader sGubunTag;
        private System.Windows.Forms.ColumnHeader fBuyPrice;
        private System.Windows.Forms.TextBox todayTotalResultTextBox;
        private System.Windows.Forms.Label totalClockLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 메뉴ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader nFakeBuyNum;
        private System.Windows.Forms.ColumnHeader nFakeResistNum;
        private System.Windows.Forms.ToolStripMenuItem 수동매도ctrlSToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader nFakeAssistantNum;
        private System.Windows.Forms.ColumnHeader fCurPower;
        private System.Windows.Forms.ColumnHeader fCurPowerWithOutGap;
        private System.Windows.Forms.ColumnHeader nPriceUpNum;
        private System.Windows.Forms.ColumnHeader nPriceDownNum;
    }
}