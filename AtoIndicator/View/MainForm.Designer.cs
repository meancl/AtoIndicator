namespace AtoIndicator
{
    public partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.screenNumLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.marketGubunLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.depositCalcLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.myNameLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.myDepositLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.accountComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.axKHOpenAPI1 = new AxKHOpenAPILib.AxKHOpenAPI();
            this.label3 = new System.Windows.Forms.Label();
            this.sCodeToBuyTextBox = new System.Windows.Forms.TextBox();
            this.totalClockLabel = new System.Windows.Forms.Label();
            this.checkChartButton = new System.Windows.Forms.Button();
            this.manualGroupBox = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.메뉴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.수동매수ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depositToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.holdingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.curRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.강제장시작ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.onMarketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onMarketWithBuyAccToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offMarketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.todayResultStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indicatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.realTimeLogStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.디폴트매도ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.risingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scalpingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.라스트업데이트ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logTxtBx = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.isMarketLabel = new System.Windows.Forms.Label();
            this.isHoldingsLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).BeginInit();
            this.manualGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.screenNumLabel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.marketGubunLabel);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.depositCalcLabel);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.myNameLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.myDepositLabel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.accountComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(7, 58);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(362, 270);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "내 정보";
            // 
            // screenNumLabel
            // 
            this.screenNumLabel.AutoSize = true;
            this.screenNumLabel.Location = new System.Drawing.Point(139, 226);
            this.screenNumLabel.Name = "screenNumLabel";
            this.screenNumLabel.Size = new System.Drawing.Size(15, 15);
            this.screenNumLabel.TabIndex = 18;
            this.screenNumLabel.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "화면번호 갯수";
            // 
            // marketGubunLabel
            // 
            this.marketGubunLabel.AutoSize = true;
            this.marketGubunLabel.Location = new System.Drawing.Point(139, 38);
            this.marketGubunLabel.Name = "marketGubunLabel";
            this.marketGubunLabel.Size = new System.Drawing.Size(0, 15);
            this.marketGubunLabel.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "투자구분";
            // 
            // depositCalcLabel
            // 
            this.depositCalcLabel.AutoSize = true;
            this.depositCalcLabel.Location = new System.Drawing.Point(139, 194);
            this.depositCalcLabel.Name = "depositCalcLabel";
            this.depositCalcLabel.Size = new System.Drawing.Size(42, 15);
            this.depositCalcLabel.TabIndex = 14;
            this.depositCalcLabel.Text = "0(원)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "계산용예수금";
            // 
            // myNameLabel
            // 
            this.myNameLabel.AutoSize = true;
            this.myNameLabel.Location = new System.Drawing.Point(139, 116);
            this.myNameLabel.Name = "myNameLabel";
            this.myNameLabel.Size = new System.Drawing.Size(52, 15);
            this.myNameLabel.TabIndex = 10;
            this.myNameLabel.Text = "아무개";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "예금주";
            // 
            // myDepositLabel
            // 
            this.myDepositLabel.AutoSize = true;
            this.myDepositLabel.Location = new System.Drawing.Point(139, 158);
            this.myDepositLabel.Name = "myDepositLabel";
            this.myDepositLabel.Size = new System.Drawing.Size(42, 15);
            this.myDepositLabel.TabIndex = 8;
            this.myDepositLabel.Text = "0(원)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "예수금";
            // 
            // accountComboBox
            // 
            this.accountComboBox.FormattingEnabled = true;
            this.accountComboBox.Location = new System.Drawing.Point(142, 78);
            this.accountComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.accountComboBox.Name = "accountComboBox";
            this.accountComboBox.Size = new System.Drawing.Size(138, 23);
            this.accountComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "계좌번호";
            // 
            // axKHOpenAPI1
            // 
            this.axKHOpenAPI1.Enabled = true;
            this.axKHOpenAPI1.Location = new System.Drawing.Point(378, 359);
            this.axKHOpenAPI1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.axKHOpenAPI1.Name = "axKHOpenAPI1";
            this.axKHOpenAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI1.OcxState")));
            this.axKHOpenAPI1.Size = new System.Drawing.Size(930, 475);
            this.axKHOpenAPI1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "코드 or 이름";
            // 
            // sCodeToBuyTextBox
            // 
            this.sCodeToBuyTextBox.Location = new System.Drawing.Point(135, 30);
            this.sCodeToBuyTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sCodeToBuyTextBox.Name = "sCodeToBuyTextBox";
            this.sCodeToBuyTextBox.Size = new System.Drawing.Size(114, 25);
            this.sCodeToBuyTextBox.TabIndex = 18;
            // 
            // totalClockLabel
            // 
            this.totalClockLabel.AutoSize = true;
            this.totalClockLabel.Font = new System.Drawing.Font("굴림", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalClockLabel.Location = new System.Drawing.Point(1099, 11);
            this.totalClockLabel.Name = "totalClockLabel";
            this.totalClockLabel.Size = new System.Drawing.Size(0, 24);
            this.totalClockLabel.TabIndex = 20;
            // 
            // checkChartButton
            // 
            this.checkChartButton.Location = new System.Drawing.Point(194, 79);
            this.checkChartButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkChartButton.Name = "checkChartButton";
            this.checkChartButton.Size = new System.Drawing.Size(122, 42);
            this.checkChartButton.TabIndex = 21;
            this.checkChartButton.Text = "개별진행확인";
            this.checkChartButton.UseVisualStyleBackColor = true;
            // 
            // manualGroupBox
            // 
            this.manualGroupBox.Controls.Add(this.label3);
            this.manualGroupBox.Controls.Add(this.checkChartButton);
            this.manualGroupBox.Controls.Add(this.sCodeToBuyTextBox);
            this.manualGroupBox.Location = new System.Drawing.Point(10, 508);
            this.manualGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.manualGroupBox.Name = "manualGroupBox";
            this.manualGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.manualGroupBox.Size = new System.Drawing.Size(362, 144);
            this.manualGroupBox.TabIndex = 22;
            this.manualGroupBox.TabStop = false;
            this.manualGroupBox.Text = "수동작업";
            this.manualGroupBox.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.메뉴ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1312, 28);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 메뉴ToolStripMenuItem
            // 
            this.메뉴ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.수동매수ToolStripMenuItem,
            this.depositToolStripMenuItem,
            this.holdingsToolStripMenuItem,
            this.curRecordToolStripMenuItem,
            this.강제장시작ToolStripMenuItem1,
            this.todayResultStripMenuItem,
            this.indicatorToolStripMenuItem,
            this.realTimeLogStripMenuItem,
            this.configStripMenuItem,
            this.디폴트매도ToolStripMenuItem,
            this.라스트업데이트ToolStripMenuItem});
            this.메뉴ToolStripMenuItem.Name = "메뉴ToolStripMenuItem";
            this.메뉴ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.메뉴ToolStripMenuItem.Text = "메뉴";
            // 
            // 수동매수ToolStripMenuItem
            // 
            this.수동매수ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onManualToolStripMenuItem,
            this.offManualToolStripMenuItem});
            this.수동매수ToolStripMenuItem.Name = "수동매수ToolStripMenuItem";
            this.수동매수ToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.수동매수ToolStripMenuItem.Text = "수동작업( M )";
            // 
            // onManualToolStripMenuItem
            // 
            this.onManualToolStripMenuItem.Name = "onManualToolStripMenuItem";
            this.onManualToolStripMenuItem.Size = new System.Drawing.Size(111, 26);
            this.onManualToolStripMenuItem.Text = "on";
            // 
            // offManualToolStripMenuItem
            // 
            this.offManualToolStripMenuItem.Name = "offManualToolStripMenuItem";
            this.offManualToolStripMenuItem.Size = new System.Drawing.Size(111, 26);
            this.offManualToolStripMenuItem.Text = "off";
            // 
            // depositToolStripMenuItem
            // 
            this.depositToolStripMenuItem.Name = "depositToolStripMenuItem";
            this.depositToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.depositToolStripMenuItem.Text = "예수금확인( D )";
            // 
            // holdingsToolStripMenuItem
            // 
            this.holdingsToolStripMenuItem.Name = "holdingsToolStripMenuItem";
            this.holdingsToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.holdingsToolStripMenuItem.Text = "보유종목확인( H )";
            // 
            // curRecordToolStripMenuItem
            // 
            this.curRecordToolStripMenuItem.Name = "curRecordToolStripMenuItem";
            this.curRecordToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.curRecordToolStripMenuItem.Text = "현황기록( R )";
            // 
            // 강제장시작ToolStripMenuItem1
            // 
            this.강제장시작ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onMarketToolStripMenuItem,
            this.onMarketWithBuyAccToolStripMenuItem,
            this.offMarketToolStripMenuItem});
            this.강제장시작ToolStripMenuItem1.Name = "강제장시작ToolStripMenuItem1";
            this.강제장시작ToolStripMenuItem1.Size = new System.Drawing.Size(284, 26);
            this.강제장시작ToolStripMenuItem1.Text = "강제장시작 여부";
            // 
            // onMarketToolStripMenuItem
            // 
            this.onMarketToolStripMenuItem.Name = "onMarketToolStripMenuItem";
            this.onMarketToolStripMenuItem.Size = new System.Drawing.Size(242, 26);
            this.onMarketToolStripMenuItem.Text = "매수 금지 장시작";
            // 
            // onMarketWithBuyAccToolStripMenuItem
            // 
            this.onMarketWithBuyAccToolStripMenuItem.Name = "onMarketWithBuyAccToolStripMenuItem";
            this.onMarketWithBuyAccToolStripMenuItem.Size = new System.Drawing.Size(242, 26);
            this.onMarketWithBuyAccToolStripMenuItem.Text = "매수 허용 장시작";
            // 
            // offMarketToolStripMenuItem
            // 
            this.offMarketToolStripMenuItem.Name = "offMarketToolStripMenuItem";
            this.offMarketToolStripMenuItem.Size = new System.Drawing.Size(242, 26);
            this.offMarketToolStripMenuItem.Text = "매수 금지 임시 장마감";
            // 
            // todayResultStripMenuItem
            // 
            this.todayResultStripMenuItem.Name = "todayResultStripMenuItem";
            this.todayResultStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.todayResultStripMenuItem.Text = "테스트( T )";
            // 
            // indicatorToolStripMenuItem
            // 
            this.indicatorToolStripMenuItem.Name = "indicatorToolStripMenuItem";
            this.indicatorToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.indicatorToolStripMenuItem.Text = "지표( I )";
            // 
            // realTimeLogStripMenuItem
            // 
            this.realTimeLogStripMenuItem.Name = "realTimeLogStripMenuItem";
            this.realTimeLogStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.realTimeLogStripMenuItem.Text = "실시간 로그( L )";
            // 
            // configStripMenuItem
            // 
            this.configStripMenuItem.Name = "configStripMenuItem";
            this.configStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.configStripMenuItem.Text = "설정( C )";
            // 
            // 디폴트매도ToolStripMenuItem
            // 
            this.디폴트매도ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.risingToolStripMenuItem,
            this.bottomUpToolStripMenuItem,
            this.scalpingToolStripMenuItem,
            this.fixedToolStripMenuItem,
            this.noneToolStripMenuItem});
            this.디폴트매도ToolStripMenuItem.Name = "디폴트매도ToolStripMenuItem";
            this.디폴트매도ToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.디폴트매도ToolStripMenuItem.Text = "디폴트 매도 설정";
            // 
            // risingToolStripMenuItem
            // 
            this.risingToolStripMenuItem.Name = "risingToolStripMenuItem";
            this.risingToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.risingToolStripMenuItem.Text = "라이징 매도";
            // 
            // bottomUpToolStripMenuItem
            // 
            this.bottomUpToolStripMenuItem.Name = "bottomUpToolStripMenuItem";
            this.bottomUpToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.bottomUpToolStripMenuItem.Text = "바텀업 매도";
            // 
            // scalpingToolStripMenuItem
            // 
            this.scalpingToolStripMenuItem.Name = "scalpingToolStripMenuItem";
            this.scalpingToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.scalpingToolStripMenuItem.Text = "스캘핑 매도";
            // 
            // fixedToolStripMenuItem
            // 
            this.fixedToolStripMenuItem.Name = "fixedToolStripMenuItem";
            this.fixedToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.fixedToolStripMenuItem.Text = "고정형 매도";
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.noneToolStripMenuItem.Text = "None";
            // 
            // 라스트업데이트ToolStripMenuItem
            // 
            this.라스트업데이트ToolStripMenuItem.Name = "라스트업데이트ToolStripMenuItem";
            this.라스트업데이트ToolStripMenuItem.Size = new System.Drawing.Size(284, 26);
            this.라스트업데이트ToolStripMenuItem.Text = "Latest Updated : 2023-08-10";
            // 
            // logTxtBx
            // 
            this.logTxtBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTxtBx.Location = new System.Drawing.Point(0, 0);
            this.logTxtBx.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.logTxtBx.Multiline = true;
            this.logTxtBx.Name = "logTxtBx";
            this.logTxtBx.ReadOnly = true;
            this.logTxtBx.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTxtBx.Size = new System.Drawing.Size(909, 595);
            this.logTxtBx.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.logTxtBx);
            this.panel1.Location = new System.Drawing.Point(390, 56);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(909, 595);
            this.panel1.TabIndex = 24;
            // 
            // isMarketLabel
            // 
            this.isMarketLabel.AutoSize = true;
            this.isMarketLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.isMarketLabel.Location = new System.Drawing.Point(17, 341);
            this.isMarketLabel.Name = "isMarketLabel";
            this.isMarketLabel.Size = new System.Drawing.Size(106, 14);
            this.isMarketLabel.TabIndex = 25;
            this.isMarketLabel.Text = "장시작 : false";
            // 
            // isHoldingsLabel
            // 
            this.isHoldingsLabel.AutoSize = true;
            this.isHoldingsLabel.Font = new System.Drawing.Font("굴림", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.isHoldingsLabel.Location = new System.Drawing.Point(230, 341);
            this.isHoldingsLabel.Name = "isHoldingsLabel";
            this.isHoldingsLabel.Size = new System.Drawing.Size(121, 14);
            this.isHoldingsLabel.TabIndex = 26;
            this.isHoldingsLabel.Text = "잔고확인 : false";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 666);
            this.Controls.Add(this.isHoldingsLabel);
            this.Controls.Add(this.isMarketLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.manualGroupBox);
            this.Controls.Add(this.totalClockLabel);
            this.Controls.Add(this.axKHOpenAPI1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Enabled = false;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).EndInit();
            this.manualGroupBox.ResumeLayout(false);
            this.manualGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox accountComboBox;
        private System.Windows.Forms.Label label1;
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label myNameLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label myDepositLabel;
        private System.Windows.Forms.Label depositCalcLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox sCodeToBuyTextBox;
        private System.Windows.Forms.Label totalClockLabel;
        private System.Windows.Forms.Button checkChartButton;
        private System.Windows.Forms.GroupBox manualGroupBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 메뉴ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 수동매수ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onManualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offManualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem curRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depositToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem holdingsToolStripMenuItem;
        private System.Windows.Forms.Label marketGubunLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem 강제장시작ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem onMarketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onMarketWithBuyAccToolStripMenuItem;
        private System.Windows.Forms.Label screenNumLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripMenuItem todayResultStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem realTimeLogStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offMarketToolStripMenuItem;
        private System.Windows.Forms.TextBox logTxtBx;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label isMarketLabel;
        private System.Windows.Forms.Label isHoldingsLabel;
        private System.Windows.Forms.ToolStripMenuItem 라스트업데이트ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 디폴트매도ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem risingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bottomUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scalpingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indicatorToolStripMenuItem;
    }
}

