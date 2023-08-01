using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

// ========================================================================
// 철학 : Being simple is the best.
// ========================================================================
namespace AtoIndicator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {

            // 현 프로그램 우선순위 최상위로 지정
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

            // ================================================
            // Windows Settings
            // ================================================
            InitializeComponent(); // c# 고유 고정메소드  

            this.Text = "Ato";

            this.KeyPreview = true;
            this.KeyDown += KeyDownHandler;
            this.KeyUp += KeyUpHandler;

            this.DoubleBuffered = true;
            this.FormClosed += FormClosedHandler;

            onMarketToolStripMenuItem.Click += ToolTipItemClickHandler;
            onMarketWithBuyAccToolStripMenuItem.Click += ToolTipItemClickHandler;
            onManualToolStripMenuItem.Click += ToolTipItemClickHandler;
            offManualToolStripMenuItem.Click += ToolTipItemClickHandler;
            curRecordToolStripMenuItem.Click += ToolTipItemClickHandler;
            indicatorToolStripMenuItem.Click += ToolTipItemClickHandler;
            depositToolStripMenuItem.Click += ToolTipItemClickHandler;
            holdingsToolStripMenuItem.Click += ToolTipItemClickHandler;
            todayResultStripMenuItem.Click += ToolTipItemClickHandler;
            realTimeLogStripMenuItem.Click += ToolTipItemClickHandler;
            configStripMenuItem.Click += ToolTipItemClickHandler;
            offMarketToolStripMenuItem.Click += ToolTipItemClickHandler;
            risingToolStripMenuItem.Click += ToolTipItemClickHandler;
            bottomUpToolStripMenuItem.Click += ToolTipItemClickHandler;
            scalpingToolStripMenuItem.Click += ToolTipItemClickHandler;
            fixedToolStripMenuItem.Click += ToolTipItemClickHandler;
            noneToolStripMenuItem.Click += ToolTipItemClickHandler;


            checkChartButton.Click += Button_Click;

            axKHOpenAPI1.OnEventConnect += OnEventConnectHandler; // 로그인 event slot connect
            axKHOpenAPI1.OnReceiveTrData += OnReceiveTrDataHandler; // TR event slot connect
            axKHOpenAPI1.OnReceiveRealData += OnReceiveRealDataHandler; // 실시간 event slot connect
            axKHOpenAPI1.OnReceiveChejanData += OnReceiveChejanDataHandler; // 체결,접수,잔고 event slot connect
            axKHOpenAPI1.OnReceiveMsg += OnReceiveMsgHandler;
            // END -- Windows Settings
            // ------------------------------------------------------

            InitAto(); // 초기화 메서드

            PrintLog("로그인 시도");
            axKHOpenAPI1.CommConnect();

        }
        public void FormClosedHandler(Object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

    }
}
