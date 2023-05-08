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
namespace AtoTrader
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

            
            realTimeLogStripMenuItem.Click += ToolTipItemClickHandler;

            checkChartButton.Click += Button_Click;

            axKHOpenAPI1.OnEventConnect += OnEventConnectHandler; // 로그인 event slot connect
            axKHOpenAPI1.OnReceiveRealData += OnReceiveRealDataHandler; // 실시간 event slot connect
            // END -- Windows Settings
            // ------------------------------------------------------

            InitAto(); // 초기화 메서드

            PrintLog("로그인 시도");
            axKHOpenAPI1.CommConnect(); 
            
        }
     
    }
}
