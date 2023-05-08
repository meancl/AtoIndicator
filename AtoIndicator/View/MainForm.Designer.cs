namespace AtoTrader
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
            this.marketGubunLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.myNameLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            this.realTimeLogStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logTxtBx = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).BeginInit();
            this.manualGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.marketGubunLabel);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.myNameLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.accountComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(7, 58);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(362, 167);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "내 정보";
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
            this.axKHOpenAPI1.Location = new System.Drawing.Point(92, 279);
            this.axKHOpenAPI1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.axKHOpenAPI1.Name = "axKHOpenAPI1";
            this.axKHOpenAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI1.OcxState")));
            this.axKHOpenAPI1.Size = new System.Drawing.Size(476, 243);
            this.axKHOpenAPI1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "종목 ID";
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
            this.checkChartButton.Location = new System.Drawing.Point(206, 81);
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
            this.manualGroupBox.Location = new System.Drawing.Point(7, 254);
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
            this.realTimeLogStripMenuItem});
            this.메뉴ToolStripMenuItem.Name = "메뉴ToolStripMenuItem";
            this.메뉴ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.메뉴ToolStripMenuItem.Text = "메뉴";
            // 
            // realTimeLogStripMenuItem
            // 
            this.realTimeLogStripMenuItem.Name = "realTimeLogStripMenuItem";
            this.realTimeLogStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.realTimeLogStripMenuItem.Text = "실시간 로그( L )";
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
            this.panel1.Controls.Add(this.axKHOpenAPI1);
            this.panel1.Location = new System.Drawing.Point(390, 56);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(909, 595);
            this.panel1.TabIndex = 24;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 666);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.manualGroupBox);
            this.Controls.Add(this.totalClockLabel);
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
        private System.Windows.Forms.Label myNameLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox sCodeToBuyTextBox;
        private System.Windows.Forms.Label totalClockLabel;
        private System.Windows.Forms.Button checkChartButton;
        private System.Windows.Forms.GroupBox manualGroupBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 메뉴ToolStripMenuItem;
        private System.Windows.Forms.Label marketGubunLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem realTimeLogStripMenuItem;
        private System.Windows.Forms.TextBox logTxtBx;
        private System.Windows.Forms.Panel panel1;
    }
}

