using AtoIndicator.View.EachStockHistory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AtoIndicator.KiwoomLib.TimeLib;


namespace AtoIndicator.View
{
    public partial class FastInfo : Form
    {
        public MainForm mainForm;
        public int[] targetTimeArr;

        public FastInfo(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            string sString = "STRING";
            string sInt = "INT";
            string sDouble = "DOUBLE";

            listView1.Columns.Add(new ColumnHeader { Name = sString, Text = "종목코드" });
            listView1.Columns.Add(new ColumnHeader { Name = sString, Text = "종목명" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "현재파워" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "초기갭" });

            listView1.Columns.Add(new ColumnHeader { Name = sString, Text = "RV" });
            listView1.Columns.Add(new ColumnHeader { Name = sString, Text = "VI" });

            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "이전분봉" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "현재분봉" });

            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "호가차이" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "체결속도" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "호가비" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "가격속도" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "대금정도" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "매수정도" });

            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "타겟 T" });
            
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "히트갯수" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "히트종류" });

            

            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "실매수" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "페매수" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "에브리" });

            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "총 페이크" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "총 애로우" });


            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "갭제외" });

            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "AI 점수" });



            listView1.View = System.Windows.Forms.View.Details;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.MouseClick += MouseClickHandler;
            listView1.MouseDoubleClick += RowDoubleClickHandler;
            listView1.ColumnClick += ColumnClickHandler;
            listView1.KeyUp += ListViewKeyUpHandller;

            targetTimeArr = new int[mainForm.nStockLength];
            for (int i = 0; i< mainForm.nStockLength; i++)
                targetTimeArr[i] = 0;

            this.KeyPreview = true;
            this.KeyUp += KeyUpHandler;
            this.KeyDown += KeyDownHandler;
            this.groupBox1.DoubleClick += DoubleClickHandler;
            this.DoubleBuffered = true;

            ToolTip tooltip1 = new ToolTip();
            ToolTip tooltip2 = new ToolTip();
            ToolTip tooltip3 = new ToolTip();
            ToolTip tooltip4 = new ToolTip();
            ToolTip tooltip5 = new ToolTip();
            ToolTip tooltip6 = new ToolTip();
            ToolTip tooltip7 = new ToolTip();
            ToolTip tooltip8 = new ToolTip();
            ToolTip tooltip9 = new ToolTip();
            ToolTip tooltip10 = new ToolTip();
            ToolTip tooltip11 = new ToolTip();


            timerDownButton.Click += TimerButtonClickHandler;
            timerUpButton.Click += TimerButtonClickHandler;

            this.write1Btn.Click += WriteButtonClickHandler;
            this.write2Btn.Click += WriteButtonClickHandler;
            this.write3Btn.Click += WriteButtonClickHandler;
            this.write4Btn.Click += WriteButtonClickHandler;
            this.write5Btn.Click += WriteButtonClickHandler;

            this.reserve1Btn.Click += ReserveButtonClickHandler;
            this.reserve2Btn.Click += ReserveButtonClickHandler;
            this.reserve3Btn.Click += ReserveButtonClickHandler;
            this.reserve4Btn.Click += ReserveButtonClickHandler;
            this.confirmButton.Click += ReserveButtonClickHandler;

            tooltip1.SetToolTip(reserve1Btn, "vi모드");
            tooltip2.SetToolTip(reserve2Btn, "페이크보조갯수 10 w지정 No");
            tooltip3.SetToolTip(reserve3Btn, "페이크매수갯수 10 W지정 No");
            tooltip4.SetToolTip(reserve4Btn, "페이크매수분포2 페이크갯수 50 맥스파워 0.1");


            tooltip7.SetToolTip(write1Btn, "실매수 10 실매수분포 2 공유분포 5");
            tooltip8.SetToolTip(write2Btn, "실매수분포 2 페이크매수분포3 공유분포 5");
            tooltip9.SetToolTip(write3Btn, "페이크매수분포2 페이크갯수 50 맥스파워 0.1");
            tooltip10.SetToolTip(write4Btn, "페이크 매수 30");
            tooltip11.SetToolTip(write5Btn, "AI 점수 10점");

            this.FormClosed += FormClosedHandler;

            timer = new System.Timers.Timer(nTimerMilliSec);
            timer.Elapsed += delegate (Object s, System.Timers.ElapsedEventArgs e)
            {
                UpdateTable();
            };
        }

        public void DoubleClickHandler(object sender, EventArgs e)
        {
            groupBox2.Visible = !groupBox2.Visible;
            if (!groupBox2.Visible)
            {
                groupBox1.Dock = DockStyle.Fill;
            }
            else
            {
                groupBox1.Dock = DockStyle.Left;
            }
        }
        public void FormClosedHandler(Object sender, FormClosedEventArgs e)
        {
            timer.Enabled = false;
            this.Dispose();
        }

        public bool isShiftMode;
        public bool isCtrlMode;
        public int nQWSharedNum = 0;
        public int nQWNum1 = 0;
        public int nQWNum2 = 0;

        public void ListViewKeyUpHandller(object sender, KeyEventArgs k)
        {
            char cUp = (char)k.KeyValue;

            if (listView1.FocusedItem != null)
            {
                if (k.KeyCode == Keys.Up || k.KeyCode == Keys.Down)
                {
                    nPrevEaIdx = mainForm.eachStockDict[listView1.FocusedItem.SubItems[0].Text.Trim()];
                }
                else
                {
                    nPrevEaIdx = mainForm.eachStockDict[listView1.FocusedItem.SubItems[0].Text.Trim()];

                    if (!isCtrlMode)
                    {
                        if (cUp == 'Q')
                        {
                            if (nQWNum1 == nQWSharedNum)
                            {
                                nQWNum1 = ++nQWSharedNum;

                                mainForm.ea[nPrevEaIdx].manualReserve.isChosenQ = !mainForm.ea[nPrevEaIdx].manualReserve.isChosenQ;
                                if (mainForm.ea[nPrevEaIdx].manualReserve.isChosenQ)
                                    registerLabel.Text = $"{listView1.FocusedItem.SubItems[0].Text.Trim()} Q창 등록됨";
                                else
                                    registerLabel.Text = $"{listView1.FocusedItem.SubItems[0].Text.Trim()} Q창 등록해제됨";
                            }
                            else
                                nQWNum1 = nQWSharedNum;
                        }

                        if (cUp == 'W')
                        {
                            if (nQWNum1 == nQWSharedNum)
                            {
                                nQWNum1 = ++nQWSharedNum;

                                mainForm.ea[nPrevEaIdx].manualReserve.isChosenW = !mainForm.ea[nPrevEaIdx].manualReserve.isChosenW;
                                if (mainForm.ea[nPrevEaIdx].manualReserve.isChosenW)
                                    registerLabel.Text = $"{listView1.FocusedItem.SubItems[0].Text.Trim()} W창 등록됨";
                                else
                                    registerLabel.Text = $"{listView1.FocusedItem.SubItems[0].Text.Trim()} W창 등록해제됨";
                            }
                            else
                                nQWNum1 = nQWSharedNum;
                        }

                        if (cUp == 'E')
                        {
                            if (nQWNum1 == nQWSharedNum)
                            {
                                nQWNum1 = ++nQWSharedNum;

                                mainForm.ea[nPrevEaIdx].manualReserve.isChosenE = !mainForm.ea[nPrevEaIdx].manualReserve.isChosenE;
                                if (mainForm.ea[nPrevEaIdx].manualReserve.isChosenE)
                                    registerLabel.Text = $"{listView1.FocusedItem.SubItems[0].Text.Trim()} E창 등록됨";
                                else
                                    registerLabel.Text = $"{listView1.FocusedItem.SubItems[0].Text.Trim()} E창 등록해제됨";
                            }
                            else
                                nQWNum1 = nQWSharedNum;
                        }

                        if (cUp == 'R')
                        {
                            if (nQWNum1 == nQWSharedNum)
                            {
                                nQWNum1 = ++nQWSharedNum;

                                mainForm.ea[nPrevEaIdx].manualReserve.isChosenR = !mainForm.ea[nPrevEaIdx].manualReserve.isChosenR;
                                if (mainForm.ea[nPrevEaIdx].manualReserve.isChosenR)
                                    registerLabel.Text = $"{listView1.FocusedItem.SubItems[0].Text.Trim()} R창 등록됨";
                                else
                                    registerLabel.Text = $"{listView1.FocusedItem.SubItems[0].Text.Trim()} R창 등록해제됨";
                            }
                            else
                                nQWNum1 = nQWSharedNum;
                        }

                        if (cUp == 191) // ?
                        {
                            mainForm.ea[nPrevEaIdx].manualReserve.ClearAll();
                            registerLabel.Text = $"{mainForm.ea[nPrevEaIdx].sCode} 전체예약 취소";
                        }
                    }

                }
            }
        }

        public void KeyDownHandler(object sender, KeyEventArgs k)
        {
            char cDown = (char)k.KeyValue;

            if (cDown == 17) // ctrl
            {
                isCtrlMode = true;
                zzimLabel.Text = "ctrl on";
                this.ActiveControl = this.passLenLabel;
            }
            if (cDown == 16) // shift
            {
                isShiftMode = true;
                zzimLabel.Text = "shift on";
                this.ActiveControl = this.passLenLabel;
            }
        }
        public void KeyUpHandler(object sender, KeyEventArgs k)
        {
            char cUp = (char)k.KeyValue;

            keyLabel.Text = $"{cUp}";

            if (isShiftMode && cUp == 27) // esc 
            {
                timer.Enabled = false;
                this.Close();
            }
               


            if (cUp == 16) // shift
            {
                isShiftMode = false;
                zzimLabel.Text = "shift off";
                this.ActiveControl = this.passLenLabel;
            }

            if (cUp == 17) // ctrl
            {
                isCtrlMode = false;
                zzimLabel.Text = "ctrl off";
                this.ActiveControl = this.passLenLabel;
            }

            if (cUp == 'T')
            {
                timerCheckBox.Checked = !timerCheckBox.Checked;
                if (timerCheckBox.Checked == false)
                    timer.Enabled = false;
            }

            if (isCtrlMode)
            {
                if (cUp == 'C')
                {
                    if (!isUsing)
                    {
                        timer.Enabled = false;
                        timerCheckBox.Checked = false;

                        wCheckBox.Checked = false;
                        qCheckBox.Checked = false;
                        eCheckBox.Checked = false;
                        rCheckBox.Checked = false;


                        passNumTxtBox.Text = "";

                        tTF1.Text = "";
                        tTF2.Text = "";
                        tSG1.Text = "";
                        tSG2.Text = "";
                        tWOG1.Text = "";
                        tWOG2.Text = "";
                        tCP1.Text = "";
                        tCP2.Text = "";
                        tPJ1.Text = "";
                        tPJ2.Text = "";
                        tUPJ1.Text = "";
                        tUPJ2.Text = "";
                        tDPJ1.Text = "";
                        tDPJ2.Text = "";
                        tTTM1.Text = "";
                        tTTM2.Text = "";
                        tBM1.Text = "";
                        tBM2.Text = "";
                        tSM1.Text = "";
                        tSM2.Text = "";
                        tTA1.Text = "";
                        tTA2.Text = "";
                        tHA1.Text = "";
                        tHA2.Text = "";
                        tRA1.Text = "";
                        tRA2.Text = "";
                        tDA1.Text = "";
                        tDA2.Text = "";
                        t1P1.Text = "";
                        t1P2.Text = "";
                        t2P1.Text = "";
                        t2P2.Text = "";
                        t3P1.Text = "";
                        t3P2.Text = "";
                        t4P1.Text = "";
                        t4P2.Text = "";
                        tPD1.Text = "";
                        tPD2.Text = "";
                        tRPD1.Text = "";
                        tRPD2.Text = "";
                        tVI1.Text = "";
                        tVI2.Text = "";
                        tRBP1.Text = "";
                        tRBP2.Text = "";
                        tTFP1.Text = "";
                        tTFP2.Text = "";
                        tEP1.Text = "";
                        tEP2.Text = "";
                        tAIS1.Text = "";
                        tAIS2.Text = "";
                        tSC1.Text = "";
                        tSC2.Text = "";
                        tSUDC1.Text = "";
                        tSUDC2.Text = "";
                        tSUP1.Text = "";
                        tSUP2.Text = "";
                        tSDP1.Text = "";
                        tSDP2.Text = "";
                        tSPD1.Text = "";
                        tSPD2.Text = "";
                        tEC1.Text = "";
                        tEC2.Text = "";
                        tCM1.Text = "";
                        tCM2.Text = "";
                        tPM1.Text = "";
                        tPM2.Text = "";
                        tCC1.Text = "";
                        tCC2.Text = "";
                        tCS1.Text = "";
                        tCS2.Text = "";
                        tAMU1.Text = "";
                        tAMU2.Text = "";
                        tAMD1.Text = "";
                        tAMD2.Text = "";
                        tHS1.Text = "";
                        tHS2.Text = "";
                        tPS1.Text = "";
                        tPS2.Text = "";
                        tPUS1.Text = "";
                        tPUS2.Text = "";
                        tTS1.Text = "";
                        tTS2.Text = "";
                        tTMAX1.Text = "";
                        tTMAX2.Text = "";
                        tTMIN1.Text = "";
                        tTMIN2.Text = "";
                        tTMD1.Text = "";
                        tTMD2.Text = "";
                        tAI5T1.Text = "";
                        tAI5T2.Text = "";
                        tAI10T1.Text = "";
                        tAI10T2.Text = "";
                        tAI15T1.Text = "";
                        tAI15T2.Text = "";
                        tAI20T1.Text = "";
                        tAI20T2.Text = "";
                        tAI30T1.Text = "";
                        tAI30T2.Text = "";
                        tAI50T1.Text = "";
                        tAI50T2.Text = "";
                        tCRC1.Text = "";
                        tCRC2.Text = "";
                        tUCRC1.Text = "";
                        tUCRC2.Text = "";
                        tT1501.Text = "";
                        tT1502.Text = "";
                        tC1501.Text = "";
                        tC1502.Text = "";
                        tRBA1.Text = "";
                        tRBA2.Text = "";
                        tRBD1.Text = "";
                        tRBD2.Text = "";
                        tFBA1.Text = "";
                        tFBA2.Text = "";
                        tFBD1.Text = "";
                        tFBD2.Text = "";
                        tFAA1.Text = "";
                        tFAA2.Text = "";
                        tFAD1.Text = "";
                        tFAD2.Text = "";
                        tFRA1.Text = "";
                        tFRA2.Text = "";
                        tFRD1.Text = "";
                        tFRD2.Text = "";
                        tPUA1.Text = "";
                        tPUA2.Text = "";
                        tPUD1.Text = "";
                        tPUD2.Text = "";
                        tPDA1.Text = "";
                        tPDA2.Text = "";
                        tPDD1.Text = "";
                        tPDD2.Text = "";
                        tSFD1.Text = "";
                        tSFD2.Text = "";
                        tHIT51.Text = "";
                        tHIT52.Text = "";
                        tHIT81.Text = "";
                        tHIT82.Text = "";
                        tHIT101.Text = "";
                        tHIT102.Text = "";
                        tHIT121.Text = "";
                        tHIT122.Text = "";
                        tCURFT1.Text = "";
                        tCURFT2.Text = "";
                        tCURFC1.Text = "";
                        tCURFC2.Text = "";
                        tFABNum1.Text = "";
                        tFABNum2.Text = "";
                        tFABPlusNum1.Text = "";
                        tFABPlusNum2.Text = "";
                        tTradeCompared1.Text = "";
                        tTradeCompared2.Text = "";
                        tTradeStrength1.Text = "";
                        tTradeStrength2.Text = "";

                    }
                }

                if (cUp == 'A')
                {
                    isReserved = false;
                    timer.Enabled = false;
                    ShowIndicator();
                }

                if (cUp == 'Q')
                {
                    CheckReserve();
                    isRZ = true;
                    nRZNum = 1;
                    ShowIndicator();
                }

                if (cUp == 'W')
                {
                    CheckReserve();
                    isRZ = true;
                    nRZNum = 2;
                    ShowIndicator();
                }

                if (cUp == 'E')
                {
                    CheckReserve();
                    isRZ = true;
                    nRZNum = 3;
                    ShowIndicator();
                }

                if (cUp == 'R')
                {
                    CheckReserve();
                    isRZ = true;
                    nRZNum = 4;
                    ShowIndicator();
                }

                if (cUp == 'P')
                {
                    CheckReserve();
                    isRZ = true;
                    nRZNum = 5;
                    ShowIndicator();
                }

                if (cUp == 'O')
                {
                    CheckReserve();
                    isRZ = true;
                    nRZNum = 6;
                    ShowIndicator();
                }

                if (cUp == 'I')
                {
                    CheckReserve();
                    isRZ = true;
                    nRZNum = 7;
                    ShowIndicator();
                }
                if (cUp == 'U')
                {
                    CheckReserve();
                    isRZ = true;
                    nRZNum = 8;
                    ShowIndicator();
                }
                if (cUp == 'Y')
                {
                    CheckReserve();
                    isRZ = true;
                    nRZNum = 9;
                    ShowIndicator();
                }
            }
            else
            {
                if (cUp == 'Q')
                {
                    if (nQWNum2 == nQWSharedNum)
                    {
                        nQWNum2 = ++nQWSharedNum;

                        mainForm.ea[nPrevEaIdx].manualReserve.isChosenQ = !mainForm.ea[nPrevEaIdx].manualReserve.isChosenQ;
                        if (mainForm.ea[nPrevEaIdx].manualReserve.isChosenQ)
                            registerLabel.Text = $"{ mainForm.ea[nPrevEaIdx].sCode} Q창 등록됨";
                        else
                            registerLabel.Text = $"{mainForm.ea[nPrevEaIdx].sCode} Q창 등록해제됨";
                    }
                    else
                        nQWNum2 = nQWSharedNum;
                }

                if (cUp == 'W')
                {
                    if (nQWNum2 == nQWSharedNum)
                    {
                        nQWNum2 = ++nQWSharedNum;

                        mainForm.ea[nPrevEaIdx].manualReserve.isChosenW = !mainForm.ea[nPrevEaIdx].manualReserve.isChosenW;
                        if (mainForm.ea[nPrevEaIdx].manualReserve.isChosenW)
                            registerLabel.Text = $"{mainForm.ea[nPrevEaIdx].sCode} W창 등록됨";
                        else
                            registerLabel.Text = $"{mainForm.ea[nPrevEaIdx].sCode} W창 등록해제됨";
                    }
                    else
                        nQWNum2 = nQWSharedNum;
                }


                if (cUp == 'E')
                {
                    if (nQWNum2 == nQWSharedNum)
                    {
                        nQWNum2 = ++nQWSharedNum;

                        mainForm.ea[nPrevEaIdx].manualReserve.isChosenE = !mainForm.ea[nPrevEaIdx].manualReserve.isChosenE;
                        if (mainForm.ea[nPrevEaIdx].manualReserve.isChosenE)
                            registerLabel.Text = $"{mainForm.ea[nPrevEaIdx].sCode} E창 등록됨";
                        else
                            registerLabel.Text = $"{mainForm.ea[nPrevEaIdx].sCode} E창 등록해제됨";
                    }
                    else
                        nQWNum2 = nQWSharedNum;
                }

                if (cUp == 'R')
                {
                    if (nQWNum2 == nQWSharedNum)
                    {
                        nQWNum2 = ++nQWSharedNum;

                        mainForm.ea[nPrevEaIdx].manualReserve.isChosenR = !mainForm.ea[nPrevEaIdx].manualReserve.isChosenR;
                        if (mainForm.ea[nPrevEaIdx].manualReserve.isChosenR)
                            registerLabel.Text = $"{mainForm.ea[nPrevEaIdx].sCode} R창 등록됨";
                        else
                            registerLabel.Text = $"{mainForm.ea[nPrevEaIdx].sCode} R창 등록해제됨";
                    }
                    else
                        nQWNum2 = nQWSharedNum;
                }

                if (cUp == 191) // ?
                {
                    mainForm.ea[nPrevEaIdx].manualReserve.ClearAll();
                    registerLabel.Text = $"{mainForm.ea[nPrevEaIdx].sCode} 전체예약 취소";
                }
            }

        }

        public void ShowIndicator()
        {
            if (sortColumn != -1)
                listView1.Columns[sortColumn].Text = listView1.Columns[sortColumn].Text.Substring(0, listView1.Columns[sortColumn].Text.Length - nTipLen);
            sortColumn = -1;
            listView1.Sorting = SortOrder.None;
            listView1.Sort();

            UpdateTable();
        }

        public int GetPassNum(bool[] pas)
        {
            int retval = 0;
            for (int i = 0; i < pas.Length; i++)
                if (pas[i])
                    retval++;
            return retval;
        }
        bool isUsing = false;

        public int nPrevTime;
        public void UpdateTable()
        {

            void Func()
            {
                DateTime startTime = DateTime.Now;
                if (isUsing)
                    return;

                isUsing = true;

                if (nPrevTime != mainForm.nSharedTime)
                {
                    nPrevTime = mainForm.nSharedTime;
                    timeLabel.Text = $"현재시간 : {nPrevTime}";
                }

                // Clear the sorting criteria applied to the ListView control


                listView1.Items.Clear();
                listView1.BeginUpdate();





                string sTF1 = "";
                string sTF2 = "";
                string sSG1 = "";
                string sSG2 = "";
                string sWOG1 = "";
                string sWOG2 = "";
                string sCP1 = "";
                string sCP2 = "";
                string sPD1 = "";
                string sPD2 = "";
                string sRPD1 = "";
                string sRPD2 = "";
                string sPJ1 = "";
                string sPJ2 = "";
                string sUPJ1 = "";
                string sUPJ2 = "";
                string sDPJ1 = "";
                string sDPJ2 = "";
                string sTTM1 = "";
                string sTTM2 = "";
                string sBM1 = "";
                string sBM2 = "";
                string sSM1 = "";
                string sSM2 = "";
                string sTA1 = "";
                string sTA2 = "";
                string sHA1 = "";
                string sHA2 = "";
                string sRA1 = "";
                string sRA2 = "";
                string sDA1 = "";
                string sDA2 = "";
                string s1P1 = "";
                string s1P2 = "";
                string s2P1 = "";
                string s2P2 = "";
                string s3P1 = "";
                string s3P2 = "";
                string s4P1 = "";
                string s4P2 = "";
                string sVI1 = "";
                string sVI2 = "";
                string sRBP1 = "";
                string sRBP2 = "";
                string sTFP1 = "";
                string sTFP2 = "";
                string sEP1 = "";
                string sEP2 = "";
                string sAIS1 = "";
                string sAIS2 = "";
                string sSC1 = "";
                string sSC2 = "";
                string sSUDC1 = "";
                string sSUDC2 = "";
                string sSUP1 = "";
                string sSUP2 = "";
                string sSDP1 = "";
                string sSDP2 = "";
                string sSPD1 = "";
                string sSPD2 = "";
                string sEC1 = "";
                string sEC2 = "";
                string sCM1 = "";
                string sCM2 = "";
                string sPM1 = "";
                string sPM2 = "";
                string sCC1 = "";
                string sCC2 = "";
                string sCS1 = "";
                string sCS2 = "";
                string sAMU1 = "";
                string sAMU2 = "";
                string sAMD1 = "";
                string sAMD2 = "";
                string sHS1 = "";
                string sHS2 = "";
                string sPS1 = "";
                string sPS2 = "";
                string sPUS1 = "";
                string sPUS2 = "";
                string sTS1 = "";
                string sTS2 = "";
                string sTMAX1 = "";
                string sTMAX2 = "";
                string sTMIN1 = "";
                string sTMIN2 = "";
                string sTMD1 = "";
                string sTMD2 = "";
                string sAI5T1 = "";
                string sAI5T2 = "";
                string sAI10T1 = "";
                string sAI10T2 = "";
                string sAI15T1 = "";
                string sAI15T2 = "";
                string sAI20T1 = "";
                string sAI20T2 = "";
                string sAI30T1 = "";
                string sAI30T2 = "";
                string sAI50T1 = "";
                string sAI50T2 = "";
                string sCRC1 = "";
                string sCRC2 = "";
                string sUCRC1 = "";
                string sUCRC2 = "";
                string sT1501 = "";
                string sT1502 = "";
                string sC1501 = "";
                string sC1502 = "";
                string sRBA1 = "";
                string sRBA2 = "";
                string sRBD1 = "";
                string sRBD2 = "";
                string sFBA1 = "";
                string sFBA2 = "";
                string sFBD1 = "";
                string sFBD2 = "";
                string sFAA1 = "";
                string sFAA2 = "";
                string sFAD1 = "";
                string sFAD2 = "";
                string sFRA1 = "";
                string sFRA2 = "";
                string sFRD1 = "";
                string sFRD2 = "";
                string sPUA1 = "";
                string sPUA2 = "";
                string sPUD1 = "";
                string sPUD2 = "";
                string sPDA1 = "";
                string sPDA2 = "";
                string sPDD1 = "";
                string sPDD2 = "";
                string sSFD1 = "";
                string sSFD2 = "";
                string sHIT51 = "";
                string sHIT52 = "";
                string sHIT81 = "";
                string sHIT82 = "";
                string sHIT101 = "";
                string sHIT102 = "";
                string sHIT121 = "";
                string sHIT122 = "";
                string sCURFT1 = "";
                string sCURFT2 = "";
                string sCURFC1 = "";
                string sCURFC2 = "";
                string sFABNum1 = "";
                string sFABNum2 = "";
                string sFABPlusNum1 = "";
                string sFABPlusNum2 = "";
                string sTradeCompared1 = "";
                string sTradeCompared2 = "";
                string sTradeStrength1 = "";
                string sTradeStrength2 = "";

                bool isTF1 = false;
                bool isTF2 = false;
                bool isSG1 = false;
                bool isSG2 = false;
                bool isWOG1 = false;
                bool isWOG2 = false;
                bool isCP1 = false;
                bool isCP2 = false;
                bool isPD1 = false;
                bool isPD2 = false;
                bool isRPD1 = false;
                bool isRPD2 = false;
                bool isPJ1 = false;
                bool isPJ2 = false;
                bool isUPJ1 = false;
                bool isUPJ2 = false;
                bool isDPJ1 = false;
                bool isDPJ2 = false;
                bool isTTM1 = false;
                bool isTTM2 = false;
                bool isBM1 = false;
                bool isBM2 = false;
                bool isSM1 = false;
                bool isSM2 = false;
                bool isTA1 = false;
                bool isTA2 = false;
                bool isHA1 = false;
                bool isHA2 = false;
                bool isRA1 = false;
                bool isRA2 = false;
                bool isDA1 = false;
                bool isDA2 = false;
                bool is1P1 = false;
                bool is1P2 = false;
                bool is2P1 = false;
                bool is2P2 = false;
                bool is3P1 = false;
                bool is3P2 = false;
                bool is4P1 = false;
                bool is4P2 = false;
                bool isVI1 = false;
                bool isVI2 = false;
                bool isRBP1 = false;
                bool isRBP2 = false;
                bool isTFP1 = false;
                bool isTFP2 = false;
                bool isEP1 = false;
                bool isEP2 = false;
                bool isAIS1 = false;
                bool isAIS2 = false;
                bool isSC1 = false;
                bool isSC2 = false;
                bool isSUDC1 = false;
                bool isSUDC2 = false;
                bool isSUP1 = false;
                bool isSUP2 = false;
                bool isSDP1 = false;
                bool isSDP2 = false;
                bool isSPD1 = false;
                bool isSPD2 = false;
                bool isEC1 = false;
                bool isEC2 = false;
                bool isCM1 = false;
                bool isCM2 = false;
                bool isPM1 = false;
                bool isPM2 = false;
                bool isCC1 = false;
                bool isCC2 = false;
                bool isCS1 = false;
                bool isCS2 = false;
                bool isAMU1 = false;
                bool isAMU2 = false;
                bool isAMD1 = false;
                bool isAMD2 = false;
                bool isHS1 = false;
                bool isHS2 = false;
                bool isPS1 = false;
                bool isPS2 = false;
                bool isPUS1 = false;
                bool isPUS2 = false;
                bool isTS1 = false;
                bool isTS2 = false;
                bool isTMAX1 = false;
                bool isTMAX2 = false;
                bool isTMIN1 = false;
                bool isTMIN2 = false;
                bool isTMD1 = false;
                bool isTMD2 = false;
                bool isAI5T1 = false;
                bool isAI5T2 = false;
                bool isAI10T1 = false;
                bool isAI10T2 = false;
                bool isAI15T1 = false;
                bool isAI15T2 = false;
                bool isAI20T1 = false;
                bool isAI20T2 = false;
                bool isAI30T1 = false;
                bool isAI30T2 = false;
                bool isAI50T1 = false;
                bool isAI50T2 = false;
                bool isCRC1 = false;
                bool isCRC2 = false;
                bool isUCRC1 = false;
                bool isUCRC2 = false;
                bool isT1501 = false;
                bool isT1502 = false;
                bool isC1501 = false;
                bool isC1502 = false;
                bool isRBA1 = false;
                bool isRBA2 = false;
                bool isRBD1 = false;
                bool isRBD2 = false;
                bool isFBA1 = false;
                bool isFBA2 = false;
                bool isFBD1 = false;
                bool isFBD2 = false;
                bool isFAA1 = false;
                bool isFAA2 = false;
                bool isFAD1 = false;
                bool isFAD2 = false;
                bool isFRA1 = false;
                bool isFRA2 = false;
                bool isFRD1 = false;
                bool isFRD2 = false;
                bool isPUA1 = false;
                bool isPUA2 = false;
                bool isPUD1 = false;
                bool isPUD2 = false;
                bool isPDA1 = false;
                bool isPDA2 = false;
                bool isPDD1 = false;
                bool isPDD2 = false;
                bool isSFD1 = false;
                bool isSFD2 = false;
                bool isHIT51 = false;
                bool isHIT52 = false;
                bool isHIT81 = false;
                bool isHIT82 = false;
                bool isHIT101 = false;
                bool isHIT102 = false;
                bool isHIT121 = false;
                bool isHIT122 = false;
                bool isCURFT1 = false;
                bool isCURFT2 = false;
                bool isCURFC1 = false;
                bool isCURFC2 = false;
                bool isFABNum1 = false;
                bool isFABNum2 = false;
                bool isFABPlusNum1 = false;
                bool isFABPlusNum2 = false;
                bool isTradeCompared1 = false;
                bool isTradeCompared2 = false;
                bool isTradeStrength1 = false;
                bool isTradeStrength2 = false;

                try
                {
                    int nPass = 0; // pass cnt
                    int nPassLen = 0;
                    int nFullMinusNum = 0;
                    int nFinalPassNum = 0;

                    List<ListViewItem> listViewItemList = new List<ListViewItem>();


                    sTF1 = tTF1.Text.Trim();
                    sTF2 = tTF2.Text.Trim();
                    sSG1 = tSG1.Text.Trim();
                    sSG2 = tSG2.Text.Trim();
                    sWOG1 = tWOG1.Text.Trim();
                    sWOG2 = tWOG2.Text.Trim();
                    sCP1 = tCP1.Text.Trim();
                    sCP2 = tCP2.Text.Trim();
                    sPD1 = tPD1.Text.Trim();
                    sPD2 = tPD2.Text.Trim();
                    sRPD1 = tRPD1.Text.Trim();
                    sRPD2 = tRPD2.Text.Trim();
                    sPJ1 = tPJ1.Text.Trim();
                    sPJ2 = tPJ2.Text.Trim();
                    sUPJ1 = tUPJ1.Text.Trim();
                    sUPJ2 = tUPJ2.Text.Trim();
                    sDPJ1 = tDPJ1.Text.Trim();
                    sDPJ2 = tDPJ2.Text.Trim();
                    sTTM1 = tTTM1.Text.Trim();
                    sTTM2 = tTTM2.Text.Trim();
                    sBM1 = tBM1.Text.Trim();
                    sBM2 = tBM2.Text.Trim();
                    sSM1 = tSM1.Text.Trim();
                    sSM2 = tSM2.Text.Trim();
                    sTA1 = tTA1.Text.Trim();
                    sTA2 = tTA2.Text.Trim();
                    sHA1 = tHA1.Text.Trim();
                    sHA2 = tHA2.Text.Trim();
                    sRA1 = tRA1.Text.Trim();
                    sRA2 = tRA2.Text.Trim();
                    sDA1 = tDA1.Text.Trim();
                    sDA2 = tDA2.Text.Trim();
                    s1P1 = t1P1.Text.Trim();
                    s1P2 = t1P2.Text.Trim();
                    s2P1 = t2P1.Text.Trim();
                    s2P2 = t2P2.Text.Trim();
                    s3P1 = t3P1.Text.Trim();
                    s3P2 = t3P2.Text.Trim();
                    s4P1 = t4P1.Text.Trim();
                    s4P2 = t4P2.Text.Trim();
                    sVI1 = tVI1.Text.Trim();
                    sVI2 = tVI2.Text.Trim();
                    sRBP1 = tRBP1.Text.Trim();
                    sRBP2 = tRBP2.Text.Trim();
                    sTFP1 = tTFP1.Text.Trim();
                    sTFP2 = tTFP2.Text.Trim();
                    sEP1 = tEP1.Text.Trim();
                    sEP2 = tEP2.Text.Trim();
                    sAIS1 = tAIS1.Text.Trim();
                    sAIS2 = tAIS2.Text.Trim();
                    sSC1 = tSC1.Text.Trim();
                    sSC2 = tSC2.Text.Trim();
                    sSUDC1 = tSUDC1.Text.Trim();
                    sSUDC2 = tSUDC2.Text.Trim();
                    sSUP1 = tSUP1.Text.Trim();
                    sSUP2 = tSUP2.Text.Trim();
                    sSDP1 = tSDP1.Text.Trim();
                    sSDP2 = tSDP2.Text.Trim();
                    sSPD1 = tSPD1.Text.Trim();
                    sSPD2 = tSPD2.Text.Trim();
                    sEC1 = tEC1.Text.Trim();
                    sEC2 = tEC2.Text.Trim();
                    sCM1 = tCM1.Text.Trim();
                    sCM2 = tCM2.Text.Trim();
                    sPM1 = tPM1.Text.Trim();
                    sPM2 = tPM2.Text.Trim();
                    sCC1 = tCC1.Text.Trim();
                    sCC2 = tCC2.Text.Trim();
                    sCS1 = tCS1.Text.Trim();
                    sCS2 = tCS2.Text.Trim();
                    sAMU1 = tAMU1.Text.Trim();
                    sAMU2 = tAMU2.Text.Trim();
                    sAMD1 = tAMD1.Text.Trim();
                    sAMD2 = tAMD2.Text.Trim();
                    sHS1 = tHS1.Text.Trim();
                    sHS2 = tHS2.Text.Trim();
                    sPS1 = tPS1.Text.Trim();
                    sPS2 = tPS2.Text.Trim();
                    sPUS1 = tPUS1.Text.Trim();
                    sPUS2 = tPUS2.Text.Trim();
                    sTS1 = tTS1.Text.Trim();
                    sTS2 = tTS2.Text.Trim();
                    sTMAX1 = tTMAX1.Text.Trim();
                    sTMAX2 = tTMAX2.Text.Trim();
                    sTMIN1 = tTMIN1.Text.Trim();
                    sTMIN2 = tTMIN2.Text.Trim();
                    sTMD1 = tTMD1.Text.Trim();
                    sTMD2 = tTMD2.Text.Trim();
                    sAI5T1 = tAI5T1.Text.Trim();
                    sAI5T2 = tAI5T2.Text.Trim();
                    sAI10T1 = tAI10T1.Text.Trim();
                    sAI10T2 = tAI10T2.Text.Trim();
                    sAI15T1 = tAI15T1.Text.Trim();
                    sAI15T2 = tAI15T2.Text.Trim();
                    sAI20T1 = tAI20T1.Text.Trim();
                    sAI20T2 = tAI20T2.Text.Trim();
                    sAI30T1 = tAI30T1.Text.Trim();
                    sAI30T2 = tAI30T2.Text.Trim();
                    sAI50T1 = tAI50T1.Text.Trim();
                    sAI50T2 = tAI50T2.Text.Trim();
                    sCRC1 = tCRC1.Text.Trim();
                    sCRC2 = tCRC2.Text.Trim();
                    sUCRC1 = tUCRC1.Text.Trim();
                    sUCRC2 = tUCRC2.Text.Trim();
                    sT1501 = tT1501.Text.Trim();
                    sT1502 = tT1502.Text.Trim();
                    sC1501 = tC1501.Text.Trim();
                    sC1502 = tC1502.Text.Trim();
                    sRBA1 = tRBA1.Text.Trim();
                    sRBA2 = tRBA2.Text.Trim();
                    sRBD1 = tRBD1.Text.Trim();
                    sRBD2 = tRBD2.Text.Trim();
                    sFBA1 = tFBA1.Text.Trim();
                    sFBA2 = tFBA2.Text.Trim();
                    sFBD1 = tFBD1.Text.Trim();
                    sFBD2 = tFBD2.Text.Trim();
                    sFAA1 = tFAA1.Text.Trim();
                    sFAA2 = tFAA2.Text.Trim();
                    sFAD1 = tFAD1.Text.Trim();
                    sFAD2 = tFAD2.Text.Trim();
                    sFRA1 = tFRA1.Text.Trim();
                    sFRA2 = tFRA2.Text.Trim();
                    sFRD1 = tFRD1.Text.Trim();
                    sFRD2 = tFRD2.Text.Trim();
                    sPUA1 = tPUA1.Text.Trim();
                    sPUA2 = tPUA2.Text.Trim();
                    sPUD1 = tPUD1.Text.Trim();
                    sPUD2 = tPUD2.Text.Trim();
                    sPDA1 = tPDA1.Text.Trim();
                    sPDA2 = tPDA2.Text.Trim();
                    sPDD1 = tPDD1.Text.Trim();
                    sPDD2 = tPDD2.Text.Trim();
                    sSFD1 = tSFD1.Text.Trim();
                    sSFD2 = tSFD2.Text.Trim();
                    sHIT51 = tHIT51.Text.Trim();
                    sHIT52 = tHIT52.Text.Trim();
                    sHIT81 = tHIT81.Text.Trim();
                    sHIT82 = tHIT82.Text.Trim();
                    sHIT101 = tHIT101.Text.Trim();
                    sHIT102 = tHIT102.Text.Trim();
                    sHIT121 = tHIT121.Text.Trim();
                    sHIT122 = tHIT122.Text.Trim();
                    sCURFT1 = tCURFT1.Text.Trim();
                    sCURFT2 = tCURFT2.Text.Trim();
                    sCURFC1 = tCURFC1.Text.Trim();
                    sCURFC2 = tCURFC2.Text.Trim();
                    sFABNum1 = tFABNum1.Text.Trim();
                    sFABNum2 = tFABNum2.Text.Trim();
                    sFABPlusNum1 = tFABPlusNum1.Text.Trim();
                    sFABPlusNum2 = tFABPlusNum2.Text.Trim();
                    sTradeCompared1 = tTradeCompared1.Text.Trim();
                    sTradeCompared2 = tTradeCompared2.Text.Trim();
                    sTradeStrength1 = tTradeStrength1.Text.Trim();
                    sTradeStrength2 = tTradeStrength2.Text.Trim();


                    isTF1 = !sTF1.Equals("");
                    isTF2 = !sTF2.Equals("");
                    isSG1 = !sSG1.Equals("");
                    isSG2 = !sSG2.Equals("");
                    isWOG1 = !sWOG1.Equals("");
                    isWOG2 = !sWOG2.Equals("");
                    isCP1 = !sCP1.Equals("");
                    isCP2 = !sCP2.Equals("");
                    isPD1 = !sPD1.Equals("");
                    isPD2 = !sPD2.Equals("");
                    isRPD1 = !sRPD1.Equals("");
                    isRPD2 = !sRPD2.Equals("");
                    isPJ1 = !sPJ1.Equals("");
                    isPJ2 = !sPJ2.Equals("");
                    isUPJ1 = !sUPJ1.Equals("");
                    isUPJ2 = !sUPJ2.Equals("");
                    isDPJ1 = !sDPJ1.Equals("");
                    isDPJ2 = !sDPJ2.Equals("");
                    isTTM1 = !sTTM1.Equals("");
                    isTTM2 = !sTTM2.Equals("");
                    isBM1 = !sBM1.Equals("");
                    isBM2 = !sBM2.Equals("");
                    isSM1 = !sSM1.Equals("");
                    isSM2 = !sSM2.Equals("");
                    isTA1 = !sTA1.Equals("");
                    isTA2 = !sTA2.Equals("");
                    isHA1 = !sHA1.Equals("");
                    isHA2 = !sHA2.Equals("");
                    isRA1 = !sRA1.Equals("");
                    isRA2 = !sRA2.Equals("");
                    isDA1 = !sDA1.Equals("");
                    isDA2 = !sDA2.Equals("");
                    is1P1 = !s1P1.Equals("");
                    is1P2 = !s1P2.Equals("");
                    is2P1 = !s2P1.Equals("");
                    is2P2 = !s2P2.Equals("");
                    is3P1 = !s3P1.Equals("");
                    is3P2 = !s3P2.Equals("");
                    is4P1 = !s4P1.Equals("");
                    is4P2 = !s4P2.Equals("");
                    isVI1 = !sVI1.Equals("");
                    isVI2 = !sVI2.Equals("");
                    isRBP1 = !sRBP1.Equals("");
                    isRBP2 = !sRBP2.Equals("");
                    isTFP1 = !sTFP1.Equals("");
                    isTFP2 = !sTFP2.Equals("");
                    isEP1 = !sEP1.Equals("");
                    isEP2 = !sEP2.Equals("");
                    isAIS1 = !sAIS1.Equals("");
                    isAIS2 = !sAIS2.Equals("");
                    isSC1 = !sSC1.Equals("");
                    isSC2 = !sSC2.Equals("");
                    isSUDC1 = !sSUDC1.Equals("");
                    isSUDC2 = !sSUDC2.Equals("");
                    isSUP1 = !sSUP1.Equals("");
                    isSUP2 = !sSUP2.Equals("");
                    isSDP1 = !sSDP1.Equals("");
                    isSDP2 = !sSDP2.Equals("");
                    isSPD1 = !sSPD1.Equals("");
                    isSPD2 = !sSPD2.Equals("");
                    isEC1 = !sEC1.Equals("");
                    isEC2 = !sEC2.Equals("");
                    isCM1 = !sCM1.Equals("");
                    isCM2 = !sCM2.Equals("");
                    isPM1 = !sPM1.Equals("");
                    isPM2 = !sPM2.Equals("");
                    isCC1 = !sCC1.Equals("");
                    isCC2 = !sCC2.Equals("");
                    isCS1 = !sCS1.Equals("");
                    isCS2 = !sCS2.Equals("");
                    isAMU1 = !sAMU1.Equals("");
                    isAMU2 = !sAMU2.Equals("");
                    isAMD1 = !sAMD1.Equals("");
                    isAMD2 = !sAMD2.Equals("");
                    isHS1 = !sHS1.Equals("");
                    isHS2 = !sHS2.Equals("");
                    isPS1 = !sPS1.Equals("");
                    isPS2 = !sPS2.Equals("");
                    isPUS1 = !sPUS1.Equals("");
                    isPUS2 = !sPUS2.Equals("");
                    isTS1 = !sTS1.Equals("");
                    isTS2 = !sTS2.Equals("");
                    isTMAX1 = !sTMAX1.Equals("");
                    isTMAX2 = !sTMAX2.Equals("");
                    isTMIN1 = !sTMIN1.Equals("");
                    isTMIN2 = !sTMIN2.Equals("");
                    isTMD1 = !sTMD1.Equals("");
                    isTMD2 = !sTMD2.Equals("");
                    isAI5T1 = !sAI5T1.Equals("");
                    isAI5T2 = !sAI5T2.Equals("");
                    isAI10T1 = !sAI10T1.Equals("");
                    isAI10T2 = !sAI10T2.Equals("");
                    isAI15T1 = !sAI15T1.Equals("");
                    isAI15T2 = !sAI15T2.Equals("");
                    isAI20T1 = !sAI20T1.Equals("");
                    isAI20T2 = !sAI20T2.Equals("");
                    isAI30T1 = !sAI30T1.Equals("");
                    isAI30T2 = !sAI30T2.Equals("");
                    isAI50T1 = !sAI50T1.Equals("");
                    isAI50T2 = !sAI50T2.Equals("");
                    isCRC1 = !sCRC1.Equals("");
                    isCRC2 = !sCRC2.Equals("");
                    isUCRC1 = !sUCRC1.Equals("");
                    isUCRC2 = !sUCRC2.Equals("");
                    isT1501 = !sT1501.Equals("");
                    isT1502 = !sT1502.Equals("");
                    isC1501 = !sC1501.Equals("");
                    isC1502 = !sC1502.Equals("");
                    isRBA1 = !sRBA1.Equals("");
                    isRBA2 = !sRBA2.Equals("");
                    isRBD1 = !sRBD1.Equals("");
                    isRBD2 = !sRBD2.Equals("");
                    isFBA1 = !sFBA1.Equals("");
                    isFBA2 = !sFBA2.Equals("");
                    isFBD1 = !sFBD1.Equals("");
                    isFBD2 = !sFBD2.Equals("");
                    isFAA1 = !sFAA1.Equals("");
                    isFAA2 = !sFAA2.Equals("");
                    isFAD1 = !sFAD1.Equals("");
                    isFAD2 = !sFAD2.Equals("");
                    isFRA1 = !sFRA1.Equals("");
                    isFRA2 = !sFRA2.Equals("");
                    isFRD1 = !sFRD1.Equals("");
                    isFRD2 = !sFRD2.Equals("");
                    isPUA1 = !sPUA1.Equals("");
                    isPUA2 = !sPUA2.Equals("");
                    isPUD1 = !sPUD1.Equals("");
                    isPUD2 = !sPUD2.Equals("");
                    isPDA1 = !sPDA1.Equals("");
                    isPDA2 = !sPDA2.Equals("");
                    isPDD1 = !sPDD1.Equals("");
                    isPDD2 = !sPDD2.Equals("");
                    isSFD1 = !sSFD1.Equals("");
                    isSFD2 = !sSFD2.Equals("");
                    isHIT51 = !sHIT51.Equals("");
                    isHIT52 = !sHIT52.Equals("");
                    isHIT81 = !sHIT81.Equals("");
                    isHIT82 = !sHIT82.Equals("");
                    isHIT101 = !sHIT101.Equals("");
                    isHIT102 = !sHIT102.Equals("");
                    isHIT121 = !sHIT121.Equals("");
                    isHIT122 = !sHIT122.Equals("");
                    isCURFT1 = !sCURFT1.Equals("");
                    isCURFT2 = !sCURFT2.Equals("");
                    isCURFC1 = !sCURFC1.Equals("");
                    isCURFC2 = !sCURFC2.Equals("");
                    isFABNum1 = !sFABNum1.Equals("");
                    isFABNum2 = !sFABNum2.Equals("");
                    isFABPlusNum1 = !sFABPlusNum1.Equals("");
                    isFABPlusNum2 = !sFABPlusNum2.Equals("");
                    isTradeCompared1 = !sTradeCompared1.Equals("");
                    isTradeCompared2 = !sTradeCompared2.Equals("");
                    isTradeStrength1 = !sTradeStrength1.Equals("");
                    isTradeStrength2 = !sTradeStrength2.Equals("");

                    nPass = 0; // pass cnt
                    nPassLen = 0;
                    nFullMinusNum = GetPassNum(new bool[] {
                                        isTF1 || isTF2 ,
                                        isSG1 || isSG2 ,
                                        isWOG1 || isWOG2 ,
                                        isCP1 || isCP2 ,
                                        isPJ1 || isPJ2 ,
                                        isUPJ1 || isUPJ2 ,
                                        isDPJ1 || isDPJ2 ,
                                        isTTM1 || isTTM2 ,
                                        isBM1 || isBM2 ,
                                        isPD1 || isPD2 ,
                                        isRPD1 || isRPD2 ,
                                        isSM1 || isSM2 ,
                                        isTA1 || isTA2 ,
                                        isHA1 || isHA2 ,
                                        isRA1 || isRA2 ,
                                        isDA1 || isDA2 ,
                                        is1P1 || is1P2 ,
                                        is2P1 || is2P2 ,
                                        is3P1 || is3P2 ,
                                        is4P1 || is4P2 ,
                                        isVI1 || isVI2,
                                        isRBP1 || isRBP2,
                                        isTFP1 || isTFP2,
                                        isEP1 || isEP2,
                                        isAIS1 || isAIS2,
                                        isSC1 || isSC2,
                                        isSUDC1 || isSUDC2,
                                        isSUP1 || isSUP2,
                                        isSDP1 || isSDP2,
                                        isSPD1 || isSPD2,
                                        isEC1 || isEC2,
                                        isCM1 || isCM2,
                                        isPM1 || isPM2,
                                        isCC1 || isCC2,
                                        isCS1 || isCS2,
                                        isAMU1 || isAMU2,
                                        isAMD1 || isAMD2,
                                        isHS1 || isHS2,
                                        isPS1 || isPS2,
                                        isPUS1 || isPUS2,
                                        isTS1 || isTS2,
                                        isTMAX1 || isTMAX2,
                                        isTMIN1 || isTMIN2,
                                        isTMD1 || isTMD2,
                                        isAI5T1 || isAI5T2 ,
                                        isAI10T1 || isAI10T2 ,
                                        isAI15T1 || isAI15T2 ,
                                        isAI20T1 || isAI20T2 ,
                                        isAI30T1 || isAI30T2 ,
                                        isAI50T1 || isAI50T2 ,
                                        isCRC1 || isCRC2 ,
                                        isUCRC1 || isUCRC2 ,
                                        isT1501 || isT1502,
                                        isC1501 || isC1502,
                                        isRBA1 || isRBA2,
                                        isFBA1 || isFBA2,
                                        isFAA1 || isFAA2,
                                        isFRA1 || isFRA2,
                                        isPUA1 || isPUA2,
                                        isPDA1 || isPDA2,
                                        isRBD1 || isRBD2,
                                        isFBD1 || isFBD2,
                                        isFAD1 || isFAD2,
                                        isFRD1 || isFRD2,
                                        isPUD1 || isPUD2,
                                        isPDD1 || isPDD2,
                                        isSFD1 || isSFD2,
                                        isHIT51 || isHIT52,
                                        isHIT81 || isHIT82,
                                        isHIT101 || isHIT102,
                                        isHIT121 || isHIT122,
                                        isCURFT1 || isCURFT2,
                                        isCURFC1 || isCURFC2,
                                        isFABNum1 || isFABNum2,
                                        isFABPlusNum1 || isFABPlusNum2,
                                        isTradeCompared1 || isTradeCompared2,
                                        isTradeStrength1 || isTradeStrength2,
                    });

                    string sPassNum = passNumTxtBox.Text.Trim();
                    int nPassMinusNum = 0;



                    if (!sPassNum.Equals(""))
                    {
                        nPassMinusNum = int.Parse(sPassNum);

                        if (nPassMinusNum > 0)
                        {
                            nFinalPassNum = nPassMinusNum;
                        }
                        else if (nPassMinusNum < 0)
                        {
                            nFinalPassNum = nFullMinusNum + nPassMinusNum;
                        }
                        else
                        {
                            nFinalPassNum = 0;
                        }
                    }
                    else
                        nFinalPassNum = nFullMinusNum;


                    bool isShow;
                    bool isReserveShow;

                    for (int i = 0; i < mainForm.nStockLength; i++)
                    {
                        try
                        {
                            nPass = 0;

                            isShow = false;
                            isReserveShow = true;

                            if (isTF1 || isTF2)
                                nPass += ((isTF1 ? int.Parse(sTF1) <= mainForm.ea[i].fakeStrategyMgr.nTotalFakeCount : true) &&
                                    (isTF2 ? mainForm.ea[i].fakeStrategyMgr.nTotalFakeCount <= int.Parse(sTF2) : true)) ? 1 : 0;
                            if (isSG1 || isSG2)
                                nPass += ((isSG1 ? double.Parse(sSG1) <= mainForm.ea[i].fStartGap : true) &&
                                    (isSG2 ? mainForm.ea[i].fStartGap <= double.Parse(sSG2) : true)) ? 1 : 0;
                            if (isWOG1 || isWOG2)
                                nPass += ((isWOG1 ? double.Parse(sWOG1) <= mainForm.ea[i].fPowerWithoutGap : true) &&
                                    (isWOG2 ? mainForm.ea[i].fPowerWithoutGap <= double.Parse(sWOG2) : true)) ? 1 : 0;
                            if (isCP1 || isCP2)
                                nPass += ((isCP1 ? double.Parse(sCP1) <= mainForm.ea[i].fPower : true) &&
                                    (isCP2 ? mainForm.ea[i].fPower <= double.Parse(sCP2) : true)) ? 1 : 0;
                            if ((isPD1 || isPD2) && mainForm.ea[i].nYesterdayEndPrice > 0)
                                nPass += ((isPD1 ? double.Parse(sPD1) <= (double)(mainForm.ea[i].timeLines1m.nMaxUpFs - mainForm.ea[i].nFs) / mainForm.ea[i].nYesterdayEndPrice : true) &&
                                    (isPD2 ? (double)(mainForm.ea[i].timeLines1m.nMaxUpFs - mainForm.ea[i].nFs) / mainForm.ea[i].nYesterdayEndPrice <= double.Parse(sPD2) : true)) ? 1 : 0;
                            if (isRPD1 || isRPD2)
                                nPass += ((isRPD1 ? double.Parse(sRPD1) <= mainForm.ea[i].fTodayMaxPower - mainForm.ea[i].fPower : true) &&
                                    (isRPD2 ? mainForm.ea[i].fTodayMaxPower - mainForm.ea[i].fPower <= double.Parse(sRPD2) : true)) ? 1 : 0;
                            if (isPJ1 || isPJ2)
                                nPass += ((isPJ1 ? double.Parse(sPJ1) <= mainForm.ea[i].fPowerJar : true) &&
                                    (isPJ2 ? mainForm.ea[i].fPowerJar <= double.Parse(sPJ2) : true)) ? 1 : 0;
                            if (isUPJ1 || isUPJ2)
                                nPass += ((isUPJ1 ? double.Parse(sUPJ1) <= mainForm.ea[i].fOnlyUpPowerJar : true) &&
                                    (isUPJ2 ? mainForm.ea[i].fOnlyUpPowerJar <= double.Parse(sUPJ2) : true)) ? 1 : 0;
                            if (isDPJ1 || isDPJ2)
                                nPass += ((isDPJ1 ? double.Parse(sDPJ1) <= mainForm.ea[i].fOnlyDownPowerJar : true) &&
                                    (isDPJ2 ? mainForm.ea[i].fOnlyDownPowerJar <= double.Parse(sDPJ2) : true)) ? 1 : 0;
                            if (isTTM1 || isTTM2)
                                nPass += ((isTTM1 ? double.Parse(sTTM1) * 100000000 <= mainForm.ea[i].lTotalTradePrice : true) &&
                                    (isTTM2 ? mainForm.ea[i].lTotalTradePrice <= double.Parse(sTTM2) * 100000000 : true)) ? 1 : 0;
                            if (isBM1 || isBM2)
                                nPass += ((isBM1 ? double.Parse(sBM1) * 100000000 <= mainForm.ea[i].lOnlyBuyPrice : true) &&
                                    (isBM2 ? mainForm.ea[i].lOnlyBuyPrice <= double.Parse(sBM2) * 100000000 : true)) ? 1 : 0;
                            if (isSM1 || isSM2)
                                nPass += ((isSM1 ? double.Parse(sSM1) * 100000000 <= mainForm.ea[i].lOnlySellPrice : true) &&
                                    (isSM2 ? mainForm.ea[i].lOnlySellPrice <= double.Parse(sSM2) * 100000000 : true)) ? 1 : 0;
                            if (isTA1 || isTA2)
                                nPass += ((isTA1 ? double.Parse(sTA1) <= mainForm.ea[i].timeLines1m.fTotalMedianAngle : true) &&
                                    (isTA2 ? mainForm.ea[i].timeLines1m.fTotalMedianAngle <= double.Parse(sTA2) : true)) ? 1 : 0;
                            if (isHA1 || isHA2)
                                nPass += ((isHA1 ? double.Parse(sHA1) <= mainForm.ea[i].timeLines1m.fHourMedianAngle : true) &&
                                    (isHA2 ? mainForm.ea[i].timeLines1m.fHourMedianAngle <= double.Parse(sHA2) : true)) ? 1 : 0;
                            if (isRA1 || isRA2)
                                nPass += ((isRA1 ? double.Parse(sRA1) <= mainForm.ea[i].timeLines1m.fRecentMedianAngle : true) &&
                                    (isRA2 ? mainForm.ea[i].timeLines1m.fRecentMedianAngle <= double.Parse(sRA2) : true)) ? 1 : 0;
                            if (isDA1 || isDA2)
                                nPass += ((isDA1 ? double.Parse(sDA1) <= mainForm.ea[i].timeLines1m.fDAngle : true) &&
                                    (isDA2 ? mainForm.ea[i].timeLines1m.fDAngle <= double.Parse(sDA2) : true)) ? 1 : 0;
                            if (is1P1 || is1P2)
                                nPass += ((is1P1 ? int.Parse(s1P1) <= mainForm.ea[i].timeLines1m.onePerCandleList.Count : true) &&
                                    (is1P2 ? mainForm.ea[i].timeLines1m.onePerCandleList.Count <= int.Parse(s1P2) : true)) ? 1 : 0;
                            if (is2P1 || is2P2)
                                nPass += ((is2P1 ? int.Parse(s2P1) <= mainForm.ea[i].timeLines1m.twoPerCandleList.Count : true) &&
                                    (is2P2 ? mainForm.ea[i].timeLines1m.twoPerCandleList.Count <= int.Parse(s2P2) : true)) ? 1 : 0;
                            if (is3P1 || is3P2)
                                nPass += ((is3P1 ? int.Parse(s3P1) <= mainForm.ea[i].timeLines1m.threePerCandleList.Count : true) &&
                                    (is3P2 ? mainForm.ea[i].timeLines1m.threePerCandleList.Count <= int.Parse(s3P2) : true)) ? 1 : 0;
                            if (is4P1 || is4P2)
                                nPass += ((is4P1 ? int.Parse(s4P1) <= mainForm.ea[i].timeLines1m.fourPerCandleList.Count : true) &&
                                    (is4P2 ? mainForm.ea[i].timeLines1m.fourPerCandleList.Count <= int.Parse(s4P2) : true)) ? 1 : 0;
                            if (isVI1 || isVI2)
                                nPass += ((isVI1 ? int.Parse(sVI1) <= mainForm.ea[i].nViCnt : true) &&
                                    (isVI2 ? mainForm.ea[i].nViCnt <= int.Parse(sVI2) : true)) ? 1 : 0;
                            if (isRBP1 || isRBP2)
                                nPass += ((isRBP1 ? int.Parse(sRBP1) <= mainForm.ea[i].fakeStrategyMgr.nAIPassed : true) &&
                                    (isRBP2 ? mainForm.ea[i].fakeStrategyMgr.nAIPassed <= int.Parse(sRBP2) : true)) ? 1 : 0;
                            if (isTFP1 || isTFP2)
                                nPass += ((isTFP1 ? int.Parse(sTFP1) <= mainForm.ea[i].fakeStrategyMgr.nFakeAccumPassed : true) &&
                                    (isTFP2 ? mainForm.ea[i].fakeStrategyMgr.nFakeAccumPassed <= int.Parse(sTFP2) : true)) ? 1 : 0;
                            if (isEP1 || isEP2)
                                nPass += ((isEP1 ? int.Parse(sEP1) <= mainForm.ea[i].fakeStrategyMgr.nEveryAIPassNum : true) &&
                                    (isEP2 ? mainForm.ea[i].fakeStrategyMgr.nEveryAIPassNum <= int.Parse(sEP2) : true)) ? 1 : 0;
                            if (isAIS1 || isAIS2)
                                nPass += ((isAIS1 ? double.Parse(sAIS1) <= mainForm.ea[i].fakeStrategyMgr.fAIScore : true) &&
                                    (isAIS2 ? mainForm.ea[i].fakeStrategyMgr.fAIScore <= double.Parse(sAIS2) : true)) ? 1 : 0;
                            if (isSC1 || isSC2)
                                nPass += ((isSC1 ? int.Parse(sSC1) <= mainForm.ea[i].nStopHogaCnt : true) &&
                                    (isSC2 ? mainForm.ea[i].nStopHogaCnt <= int.Parse(sSC2) : true)) ? 1 : 0;
                            if (isSUDC1 || isSUDC2)
                                nPass += ((isSUDC1 ? int.Parse(sSUDC1) <= mainForm.ea[i].nStopUpDownCnt : true) &&
                                    (isSUDC2 ? mainForm.ea[i].nStopUpDownCnt <= int.Parse(sSUDC2) : true)) ? 1 : 0;
                            if (isSUP1 || isSUP2)
                                nPass += ((isSUP1 ? double.Parse(sSUP1) <= mainForm.ea[i].fStopMaxPower : true) &&
                                    (isSUP2 ? mainForm.ea[i].fStopMaxPower <= double.Parse(sSUP2) : true)) ? 1 : 0;
                            if (isSDP1 || isSDP2)
                                nPass += ((isSDP1 ? double.Parse(sSDP1) <= mainForm.ea[i].fStopMinPower : true) &&
                                    (isSDP2 ? mainForm.ea[i].fStopMinPower <= double.Parse(sSDP2) : true)) ? 1 : 0;
                            if (isSPD1 || isSPD2)
                                nPass += ((isSPD1 ? double.Parse(sSPD1) <= mainForm.ea[i].fStopMaxMinDiff : true) &&
                                    (isSPD2 ? mainForm.ea[i].fStopMaxMinDiff <= double.Parse(sSPD2) : true)) ? 1 : 0;
                            if (isEC1 || isEC2)
                                nPass += ((isEC1 ? int.Parse(sEC1) <= mainForm.ea[i].fakeStrategyMgr.nEveryAICount : true) &&
                                    (isEC2 ? mainForm.ea[i].fakeStrategyMgr.nEveryAICount <= int.Parse(sEC2) : true)) ? 1 : 0;
                            if (isCM1 || isCM2)
                                nPass += ((isCM1 ? double.Parse(sCM1) <= (double)(mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nPrevTimeLineIdx].nLastFs - mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nPrevTimeLineIdx].nStartFs) / mainForm.ea[i].nYesterdayEndPrice : true) &&
                                    (isCM2 ? (double)(mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nPrevTimeLineIdx].nLastFs - mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nPrevTimeLineIdx].nStartFs) / mainForm.ea[i].nYesterdayEndPrice <= double.Parse(sCM2) : true)) ? 1 : 0;
                            if (isPM1 || isPM2)
                                nPass += ((isPM1 ? double.Parse(sPM1) <= (double)(mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nRealDataIdx].nLastFs - mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nRealDataIdx].nStartFs) / mainForm.ea[i].nYesterdayEndPrice : true) &&
                                    (isPM2 ? (double)(mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nRealDataIdx].nLastFs - mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nRealDataIdx].nStartFs) / mainForm.ea[i].nYesterdayEndPrice <= double.Parse(sPM2) : true)) ? 1 : 0;
                            if (isCC1 || isCC2)
                                nPass += ((isCC1 ? int.Parse(sCC1) <= mainForm.ea[i].nChegyulCnt : true) &&
                                    (isCC2 ? mainForm.ea[i].nChegyulCnt <= int.Parse(sCC2) : true)) ? 1 : 0;
                            if (isCS1 || isCS2)
                                nPass += ((isCS1 ? double.Parse(sCS1) <= mainForm.ea[i].speedStatus.fCur : true) &&
                                    (isCS2 ? mainForm.ea[i].speedStatus.fCur <= double.Parse(sCS2) : true)) ? 1 : 0;
                            if (isAMU1 || isAMU2)
                                nPass += ((isAMU1 ? double.Parse(sAMU1) <= mainForm.ea[i].fPositiveStickPower : true) &&
                                    (isAMU2 ? mainForm.ea[i].fPositiveStickPower <= double.Parse(sAMU2) : true)) ? 1 : 0;
                            if (isAMD1 || isAMD2)
                                nPass += ((isAMD1 ? double.Parse(sAMD1) <= mainForm.ea[i].fNegativeStickPower : true) &&
                                    (isAMD2 ? mainForm.ea[i].fNegativeStickPower <= double.Parse(sAMD2) : true)) ? 1 : 0;
                            if (isHS1 || isHS2)
                                nPass += ((isHS1 ? double.Parse(sHS1) <= mainForm.ea[i].hogaSpeedStatus.fCur : true) &&
                                    (isHS2 ? mainForm.ea[i].hogaSpeedStatus.fCur <= double.Parse(sHS2) : true)) ? 1 : 0;
                            if (isPS1 || isPS2)
                                nPass += ((isPS1 ? double.Parse(sPS1) <= mainForm.ea[i].priceMoveStatus.fCur : true) &&
                                    (isPS2 ? mainForm.ea[i].priceMoveStatus.fCur <= double.Parse(sPS2) : true)) ? 1 : 0;
                            if (isPUS1 || isPUS2)
                                nPass += ((isPUS1 ? double.Parse(sPUS1) <= mainForm.ea[i].priceUpMoveStatus.fCur : true) &&
                                    (isPUS2 ? mainForm.ea[i].priceUpMoveStatus.fCur <= double.Parse(sPUS2) : true)) ? 1 : 0;
                            if (isTS1 || isTS2)
                                nPass += ((isTS1 ? double.Parse(sTS1) <= mainForm.ea[i].tradeStatus.fCur : true) &&
                                    (isTS2 ? mainForm.ea[i].tradeStatus.fCur <= double.Parse(sTS2) : true)) ? 1 : 0;
                            if (isTMAX1 || isTMAX2)
                                nPass += ((isTMAX1 ? double.Parse(sTMAX1) <= mainForm.ea[i].fTodayMaxPower : true) &&
                                    (isTMAX2 ? mainForm.ea[i].fTodayMaxPower <= double.Parse(sTMAX2) : true)) ? 1 : 0;
                            if (isTMIN1 || isTMIN2)
                                nPass += ((isTMIN1 ? double.Parse(sTMIN1) <= mainForm.ea[i].fTodayMinPower : true) &&
                                    (isTMIN2 ? mainForm.ea[i].fTodayMinPower <= double.Parse(sTMIN2) : true)) ? 1 : 0;
                            if (isTMD1 || isTMD2)
                                nPass += ((isTMD1 ? double.Parse(sTMD1) <= mainForm.ea[i].fTodayMaxPower - mainForm.ea[i].fTodayBottomPower : true) &&
                                    (isTMD2 ? mainForm.ea[i].fTodayMaxPower - mainForm.ea[i].fTodayBottomPower <= double.Parse(sTMD2) : true)) ? 1 : 0;
                            if (isAI5T1 || isAI5T2)
                                nPass += ((isAI5T1 ? int.Parse(sAI5T1) <= mainForm.ea[i].fakeStrategyMgr.nAI5Time : true) &&
                                  (isAI5T2 ? mainForm.ea[i].fakeStrategyMgr.nAI5Time <= int.Parse(sAI5T2) : true)) ? 1 : 0;
                            if (isAI10T1 || isAI10T2)
                                nPass += ((isAI10T1 ? int.Parse(sAI10T1) <= mainForm.ea[i].fakeStrategyMgr.nAI10Time : true) &&
                                  (isAI10T2 ? mainForm.ea[i].fakeStrategyMgr.nAI10Time <= int.Parse(sAI10T2) : true)) ? 1 : 0;
                            if (isAI15T1 || isAI15T2)
                                nPass += ((isAI15T1 ? int.Parse(sAI15T1) <= mainForm.ea[i].fakeStrategyMgr.nAI15Time : true) &&
                                  (isAI15T2 ? mainForm.ea[i].fakeStrategyMgr.nAI15Time <= int.Parse(sAI15T2) : true)) ? 1 : 0;
                            if (isAI20T1 || isAI20T2)
                                nPass += ((isAI20T1 ? int.Parse(sAI20T1) <= mainForm.ea[i].fakeStrategyMgr.nAI20Time : true) &&
                                  (isAI20T2 ? mainForm.ea[i].fakeStrategyMgr.nAI20Time <= int.Parse(sAI20T2) : true)) ? 1 : 0;
                            if (isAI30T1 || isAI30T2)
                                nPass += ((isAI30T1 ? int.Parse(sAI30T1) <= mainForm.ea[i].fakeStrategyMgr.nAI30Time : true) &&
                                  (isAI30T2 ? mainForm.ea[i].fakeStrategyMgr.nAI30Time <= int.Parse(sAI30T2) : true)) ? 1 : 0;
                            if (isAI50T1 || isAI50T2)
                                nPass += ((isAI50T1 ? int.Parse(sAI50T1) <= mainForm.ea[i].fakeStrategyMgr.nAI50Time : true) &&
                                  (isAI50T2 ? mainForm.ea[i].fakeStrategyMgr.nAI50Time <= int.Parse(sAI50T2) : true)) ? 1 : 0;
                            if (isCRC1 || isCRC2)
                                nPass += ((isCRC1 ? int.Parse(sCRC1) <= mainForm.ea[i].crushMinuteManager.nCurCnt : true) &&
                                  (isCRC2 ? mainForm.ea[i].crushMinuteManager.nCurCnt <= int.Parse(sCRC2) : true)) ? 1 : 0;
                            if (isUCRC1 || isUCRC2)
                                nPass += ((isUCRC1 ? int.Parse(sUCRC1) <= mainForm.ea[i].crushMinuteManager.nUpCnt : true) &&
                                  (isUCRC2 ? mainForm.ea[i].crushMinuteManager.nUpCnt <= int.Parse(sUCRC2) : true)) ? 1 : 0;
                            if (isT1501 || isT1502)
                                nPass += ((isT1501 ? int.Parse(sT1501) <= mainForm.ea[i].sequenceStrategy.nSpeed150TotalSec : true) &&
                                  (isT1502 ? mainForm.ea[i].sequenceStrategy.nSpeed150TotalSec <= int.Parse(sT1502) : true)) ? 1 : 0;
                            if (isC1501 || isC1502)
                                nPass += ((isC1501 ? int.Parse(sC1501) <= mainForm.ea[i].sequenceStrategy.nSpeed150CurSec : true) &&
                                  (isC1502 ? mainForm.ea[i].sequenceStrategy.nSpeed150CurSec <= int.Parse(sC1502) : true)) ? 1 : 0;
                            if (isRBA1 || isRBA2)
                                nPass += ((isRBA1 ? int.Parse(sRBA1) <= mainForm.ea[i].paperBuyStrategy.nStrategyNum : true) &&
                                  (isRBA2 ? mainForm.ea[i].paperBuyStrategy.nStrategyNum <= int.Parse(sRBA2) : true)) ? 1 : 0;
                            if (isRBD1 || isRBD2)
                                nPass += ((isRBD1 ? int.Parse(sRBD1) <= mainForm.ea[i].paperBuyStrategy.nMinuteLocationCount : true) &&
                                  (isRBD2 ? mainForm.ea[i].paperBuyStrategy.nMinuteLocationCount <= int.Parse(sRBD2) : true)) ? 1 : 0;
                            if (isFBA1 || isFBA2)
                                nPass += ((isFBA1 ? int.Parse(sFBA1) <= mainForm.ea[i].fakeBuyStrategy.nStrategyNum : true) &&
                                  (isFBA2 ? mainForm.ea[i].fakeBuyStrategy.nStrategyNum <= int.Parse(sFBA2) : true)) ? 1 : 0;
                            if (isFBD1 || isFBD2)
                                nPass += ((isFBD1 ? int.Parse(sFBD1) <= mainForm.ea[i].fakeBuyStrategy.nMinuteLocationCount : true) &&
                                  (isFBD2 ? mainForm.ea[i].fakeBuyStrategy.nMinuteLocationCount <= int.Parse(sFBD2) : true)) ? 1 : 0;
                            if (isFAA1 || isFAA2)
                                nPass += ((isFAA1 ? int.Parse(sFAA1) <= mainForm.ea[i].fakeAssistantStrategy.nStrategyNum : true) &&
                                  (isFAA2 ? mainForm.ea[i].fakeAssistantStrategy.nStrategyNum <= int.Parse(sFAA2) : true)) ? 1 : 0;
                            if (isFAD1 || isFAD2)
                                nPass += ((isFAD1 ? int.Parse(sFAD1) <= mainForm.ea[i].fakeAssistantStrategy.nMinuteLocationCount : true) &&
                                  (isFAD2 ? mainForm.ea[i].fakeAssistantStrategy.nMinuteLocationCount <= int.Parse(sFAD2) : true)) ? 1 : 0;
                            if (isFRA1 || isFRA2)
                                nPass += ((isFRA1 ? int.Parse(sFRA1) <= mainForm.ea[i].fakeResistStrategy.nStrategyNum : true) &&
                                  (isFRA2 ? mainForm.ea[i].fakeResistStrategy.nStrategyNum <= int.Parse(sFRA2) : true)) ? 1 : 0;
                            if (isFRD1 || isFRD2)
                                nPass += ((isFRD1 ? int.Parse(sFRD1) <= mainForm.ea[i].fakeResistStrategy.nMinuteLocationCount : true) &&
                                  (isFRD2 ? mainForm.ea[i].fakeResistStrategy.nMinuteLocationCount <= int.Parse(sFRD2) : true)) ? 1 : 0;
                            if (isPUA1 || isPUA2)
                                nPass += ((isPUA1 ? int.Parse(sPUA1) <= mainForm.ea[i].fakeVolatilityStrategy.nStrategyNum : true) &&
                                  (isPUA2 ? mainForm.ea[i].fakeVolatilityStrategy.nStrategyNum <= int.Parse(sPUA2) : true)) ? 1 : 0;
                            if (isPUD1 || isPUD2)
                                nPass += ((isPUD1 ? int.Parse(sPUD1) <= mainForm.ea[i].fakeVolatilityStrategy.nMinuteLocationCount : true) &&
                                  (isPUD2 ? mainForm.ea[i].fakeVolatilityStrategy.nMinuteLocationCount <= int.Parse(sPUD2) : true)) ? 1 : 0;
                            if (isPDA1 || isPDA2)
                                nPass += ((isPDA1 ? int.Parse(sPDA1) <= mainForm.ea[i].fakeDownStrategy.nStrategyNum : true) &&
                                  (isPDA2 ? mainForm.ea[i].fakeDownStrategy.nStrategyNum <= int.Parse(sPDA2) : true)) ? 1 : 0;
                            if (isPDD1 || isPDD2)
                                nPass += ((isPDD1 ? int.Parse(sPDD1) <= mainForm.ea[i].fakeDownStrategy.nMinuteLocationCount : true) &&
                                  (isPDD2 ? mainForm.ea[i].fakeDownStrategy.nMinuteLocationCount <= int.Parse(sPDD2) : true)) ? 1 : 0;
                            if (isSFD1 || isSFD2)
                                nPass += ((isSFD1 ? int.Parse(sSFD1) <= mainForm.ea[i].fakeStrategyMgr.nSharedMinuteLocationCount : true) &&
                                  (isSFD2 ? mainForm.ea[i].fakeStrategyMgr.nSharedMinuteLocationCount <= int.Parse(sSFD2) : true)) ? 1 : 0;
                            if (isHIT51 || isHIT52)
                                nPass += ((isHIT51 ? int.Parse(sHIT51) <= mainForm.ea[i].fakeStrategyMgr.hitDict25.Count : true) &&
                                  (isHIT52 ? mainForm.ea[i].fakeStrategyMgr.hitDict25.Count <= int.Parse(sHIT52) : true)) ? 1 : 0;
                            if (isHIT81 || isHIT82)
                                nPass += ((isHIT81 ? int.Parse(sHIT81) <= mainForm.ea[i].fakeStrategyMgr.hitDict38.Count : true) &&
                                  (isHIT82 ? mainForm.ea[i].fakeStrategyMgr.hitDict38.Count <= int.Parse(sHIT82) : true)) ? 1 : 0;
                            if (isHIT101 || isHIT102)
                                nPass += ((isHIT101 ? int.Parse(sHIT101) <= mainForm.ea[i].fakeStrategyMgr.hitDict410.Count : true) &&
                                  (isHIT102 ? mainForm.ea[i].fakeStrategyMgr.hitDict410.Count <= int.Parse(sHIT102) : true)) ? 1 : 0;
                            if (isHIT121 || isHIT122)
                                nPass += ((isHIT121 ? int.Parse(sHIT121) <= mainForm.ea[i].fakeStrategyMgr.hitDict312.Count : true) &&
                                  (isHIT122 ? mainForm.ea[i].fakeStrategyMgr.hitDict312.Count <= int.Parse(sHIT122) : true)) ? 1 : 0;
                            if (isCURFT1 || isCURFT2)
                                nPass += ((isCURFT1 ? int.Parse(sCURFT1) <= mainForm.ea[i].fakeStrategyMgr.nCurHitType : true) &&
                                  (isCURFT2 ? mainForm.ea[i].fakeStrategyMgr.nCurHitType <= int.Parse(sCURFT2) : true)) ? 1 : 0;
                            if (isCURFC1 || isCURFC2)
                                nPass += ((isCURFC1 ? int.Parse(sCURFC1) <= mainForm.ea[i].fakeStrategyMgr.nCurHitNum : true) &&
                                  (isCURFC2 ? mainForm.ea[i].fakeStrategyMgr.nCurHitNum <= int.Parse(sCURFC2) : true)) ? 1 : 0;
                            if(isFABNum1 || isFABNum2)
                                nPass += ((isFABNum1 ? int.Parse(sFABNum1) <= mainForm.ea[i].fakeAssistantStrategy.nStrategyNum + mainForm.ea[i].fakeBuyStrategy.nStrategyNum : true) &&
                                  (isFABNum2 ? mainForm.ea[i].fakeAssistantStrategy.nStrategyNum + mainForm.ea[i].fakeBuyStrategy.nStrategyNum <= int.Parse(sFABNum2) : true)) ? 1 : 0;
                            if (isFABPlusNum1 || isFABPlusNum2)
                                nPass += ((isFABPlusNum1 ? int.Parse(sFABPlusNum1) <= mainForm.ea[i].fakeAssistantStrategy.nStrategyNum + mainForm.ea[i].fakeBuyStrategy.nStrategyNum + mainForm.ea[i].fakeBuyStrategy.nStrategyNum / 2 : true) &&
                                  (isFABPlusNum2 ? mainForm.ea[i].fakeAssistantStrategy.nStrategyNum + mainForm.ea[i].fakeBuyStrategy.nStrategyNum + mainForm.ea[i].fakeBuyStrategy.nStrategyNum / 2 <= int.Parse(sFABPlusNum2) : true)) ? 1 : 0;
                            if (isTradeCompared1 || isTradeCompared2)
                                nPass += ((isTradeCompared1 ? double.Parse(sTradeCompared1) <= mainForm.ea[i].fTradeRatioCompared : true) &&
                                  (isTradeCompared2 ? mainForm.ea[i].fTradeRatioCompared <= double.Parse(sTradeCompared2) : true)) ? 1 : 0;
                            if (isTradeStrength1 || isTradeStrength2)
                                nPass += ((isTradeStrength1 ? double.Parse(sTradeStrength1) <= mainForm.ea[i].fTs : true) &&
                                  (isTradeStrength2 ? mainForm.ea[i].fTs <= double.Parse(sTradeStrength2) : true)) ? 1 : 0;

                            isShow = nPass >= nFinalPassNum;


                            if (isReserved) // 예약ㅇ라면
                            {
                                isReserveShow = false;

                                if (isR1) // r1 조건
                                {
                                    isReserveShow = mainForm.ea[i].isViMode;
                                }
                                else if (isR2) // r2 조건
                                {
                                    isReserveShow = mainForm.ea[i].fakeAssistantStrategy.nStrategyNum >= 10 && !mainForm.ea[i].manualReserve.isChosenW;
                                }
                                else if (isR3) // r3 조건
                                {
                                    isReserveShow = mainForm.ea[i].fakeBuyStrategy.nStrategyNum >= 10 && !mainForm.ea[i].manualReserve.isChosenW;
                                }
                                else if (isR4) // r4 조건
                                {
                                    isReserveShow = mainForm.ea[i].fTodayMaxPower >= 0.1 &&
                                                mainForm.ea[i].fakeStrategyMgr.nTotalFakeCount >= 50 &&
                                                mainForm.ea[i].fakeBuyStrategy.nMinuteLocationCount >= 2;
                                }
                                else if (isRZ)
                                {
                                    if (nRZNum == 1)
                                        isReserveShow = mainForm.ea[i].manualReserve.isChosenQ;
                                    else if (nRZNum == 2)
                                        isReserveShow = mainForm.ea[i].manualReserve.isChosenW;
                                    else if (nRZNum == 3)
                                        isReserveShow = mainForm.ea[i].manualReserve.isChosenE;
                                    else if (nRZNum == 4)
                                        isReserveShow = mainForm.ea[i].manualReserve.isChosenR;
                                    else if (nRZNum == 5)
                                        isReserveShow = mainForm.ea[i].manualReserve.reserveArr[MainForm.UP_RESERVE].isSelected;
                                    else if (nRZNum == 6)
                                        isReserveShow = mainForm.ea[i].manualReserve.reserveArr[MainForm.DOWN_RESERVE].isSelected;
                                    else if (nRZNum == 7)
                                        isReserveShow = mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isSelected;
                                    else if (nRZNum == 8)
                                        isReserveShow = mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_RESERVE_POSITION_RESERVE].isSelected;
                                    else if (nRZNum == 9)
                                        isReserveShow = mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isSelected;

                                }
                                else // 에러
                                {

                                }
                            }

                            if (qCheckBox.Checked && mainForm.ea[i].manualReserve.isChosenQ)
                                continue;

                            if (wCheckBox.Checked && mainForm.ea[i].manualReserve.isChosenW)
                                continue;

                            if (eCheckBox.Checked && mainForm.ea[i].manualReserve.isChosenE)
                                continue;

                            if (rCheckBox.Checked && mainForm.ea[i].manualReserve.isChosenR)
                                continue;

                            if (isShow && isReserveShow)
                            {
                                targetTimeArr[i]++;
                                nPassLen++;
                                ListViewItem listViewItem = new ListViewItem(new string[] {
                                mainForm.ea[i].sCode,
                                mainForm.ea[i].sCodeName,
                                Math.Round(mainForm.ea[i].fPower, 3).ToString(),
                                Math.Round(mainForm.ea[i].fStartGap, 3).ToString(),
                                "", // 매수예약
                                mainForm.ea[i].isViMode.ToString(),

                                Math.Round((double)(mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nRealDataIdx].nLastFs - mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nRealDataIdx].nStartFs) / mainForm.ea[i].nYesterdayEndPrice, 3).ToString(),
                                Math.Round((double)(mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nPrevTimeLineIdx].nLastFs - mainForm.ea[i].timeLines1m.arrTimeLine[mainForm.ea[i].timeLines1m.nPrevTimeLineIdx].nStartFs) / mainForm.ea[i].nYesterdayEndPrice, 3).ToString(),

                                Math.Abs(Math.Round((double)(mainForm.ea[i].nTotalSellHogaVolume - mainForm.ea[i].nTotalBuyHogaVolume)  * mainForm.ea[i].nCurHogaPrice / MainForm.HUNDRED_MILLION, 2)).ToString(),
                                Math.Round(mainForm.ea[i].speedStatus.fCur, 2).ToString(),
                                Math.Round(mainForm.ea[i].fHogaRatio, 2).ToString(),
                                Math.Round(mainForm.ea[i].priceMoveStatus.fCur, 2).ToString(),
                                Math.Round(mainForm.ea[i].tradeStatus.fCur , 2).ToString(),
                                Math.Round(mainForm.ea[i].pureTradeStatus.fCur , 2).ToString(),

                                targetTimeArr[i].ToString(), 

                                mainForm.ea[i].fakeStrategyMgr.nCurHitNum.ToString(),
                                mainForm.ea[i].fakeStrategyMgr.nCurHitType.ToString(),

                                mainForm.ea[i].paperBuyStrategy.nStrategyNum.ToString(),
                                mainForm.ea[i].fakeBuyStrategy.nStrategyNum.ToString(),

                                mainForm.ea[i].fakeStrategyMgr.nEveryAICount.ToString(),
                                mainForm.ea[i].fakeStrategyMgr.nTotalFakeCount.ToString(),
                                mainForm.ea[i].fakeStrategyMgr.nTotalArrowCount.ToString(),


                                Math.Round(mainForm.ea[i].fPowerWithoutGap, 3).ToString(),

                                Math.Round(mainForm.ea[i].fakeStrategyMgr.fAIScore, 2).ToString(),

                            });


                                Color myColor;

                                if (mainForm.ea[i].fPowerJar == 0) //  평균 이율
                                    myColor = Color.FromArgb(255, 255, 255);
                                else if (mainForm.ea[i].fPowerJar > 0)
                                {
                                    int nColorStep = (int)(mainForm.ea[i].fPowerJar * 20000);
                                    if (nColorStep > 255)
                                        nColorStep = 255;
                                    myColor = Color.FromArgb(255, 255 - nColorStep, 255 - nColorStep);
                                }
                                else
                                {
                                    int nColorStep = (int)(Math.Abs(mainForm.ea[i].fPowerJar) * 20000);
                                    if (nColorStep > 255)
                                        nColorStep = 255;
                                    myColor = Color.FromArgb(255 - nColorStep, 255 - nColorStep, 255);
                                }


                                listViewItem.UseItemStyleForSubItems = false;

                                for (int restIdx = 0; restIdx < listViewItem.SubItems.Count; restIdx++)
                                {
                                    if (mainForm.ea[i].manualReserve.isChosenQ && restIdx == 0)
                                        listViewItem.SubItems[restIdx].BackColor = Color.Green;
                                    else if (mainForm.ea[i].manualReserve.isChosenW && restIdx == 1)
                                        listViewItem.SubItems[restIdx].BackColor = Color.Orange;
                                    else if (mainForm.ea[i].manualReserve.isChosenE && restIdx == 2)
                                        listViewItem.SubItems[restIdx].BackColor = Color.SkyBlue;
                                    else if (mainForm.ea[i].manualReserve.isChosenR && restIdx == 3)
                                        listViewItem.SubItems[restIdx].BackColor = Color.GreenYellow;
                                    else if ((mainForm.ea[i].manualReserve.reserveArr[MainForm.UP_RESERVE].isBuyReserved ||
                                              mainForm.ea[i].manualReserve.reserveArr[MainForm.DOWN_RESERVE].isBuyReserved ||
                                              mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isBuyReserved ||
                                              mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isBuyReserved) && restIdx == 4)
                                        listViewItem.SubItems[restIdx].BackColor = Color.Black;
                                    else if ((mainForm.ea[i].manualReserve.reserveArr[MainForm.UP_RESERVE].isSelected && restIdx == 5) ||
                                            (mainForm.ea[i].manualReserve.reserveArr[MainForm.UP_RESERVE].isChosen1 && restIdx == 6))
                                        listViewItem.SubItems[restIdx].BackColor = Color.BlueViolet;
                                    else if ((mainForm.ea[i].manualReserve.reserveArr[MainForm.DOWN_RESERVE].isSelected && restIdx == 7) ||
                                            (mainForm.ea[i].manualReserve.reserveArr[MainForm.DOWN_RESERVE].isChosen1 && restIdx == 8))
                                        listViewItem.SubItems[restIdx].BackColor = Color.Gold;
                                    else if ((mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isSelected && restIdx == 9) ||
                                            (mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_DOWN_RESERVE].isChosen1 && restIdx == 10))
                                        listViewItem.SubItems[restIdx].BackColor = Color.Turquoise;
                                    else if ((mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_RESERVE_POSITION_RESERVE].isSelected && restIdx == 11) ||
                                            (mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_RESERVE_POSITION_RESERVE].isChosen1 && restIdx == 12))
                                        listViewItem.SubItems[restIdx].BackColor = Color.Olive;
                                    else if ((mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isSelected && restIdx == 13) ||
                                            (mainForm.ea[i].manualReserve.reserveArr[MainForm.MA_UP_RESERVE].isChosen1 && restIdx == 14))
                                        listViewItem.SubItems[restIdx].BackColor = Color.Teal;
                                    else
                                        listViewItem.SubItems[restIdx].BackColor = myColor;
                                }


                                listViewItemList.Add(listViewItem);
                            }
                        }
                        catch
                        {
                            registerLabel.Text = "오류 발생";
                            timer.Enabled = false;
                            timerCheckBox.Checked = false;
                            return;
                        }

                    }

                    // 조건에 맞는 애들이 있다면 출력
                    if (listViewItemList.Count > 0)
                    {
                        listView1.Items.AddRange(listViewItemList.ToArray());
                        listViewItemList.Clear();
                    }

                    doneLabel.Text = $"done..{++nDoneCnt}";
                    passLenLabel.Text = $"pass {nPassLen}";

                    DateTime endTime = DateTime.Now;
                    // 타이머가 on이라면
                    if (timerCheckBox.Checked)
                    {
                        if (nPassLen > 200 || (endTime - startTime).TotalMilliseconds > 500)  //  타이머 무리무리
                        {
                            timer.Enabled = false;
                            timerCheckBox.Checked = false;
                        }
                        else
                        {

                            timer.Interval = nTimerMilliSec;
                            timer.Enabled = true;
                            timerCheckBox.Checked = true;
                        }
                    }
                    else
                    {
                        timer.Enabled = false;
                        timerCheckBox.Checked = false;
                    }

                    isReserved = isReserved && timer.Enabled;
                }
                catch
                {
                    doneLabel.Text = "Uncomplete";
                }
                finally
                {
                    isUsing = false;
                    listView1.EndUpdate();
                }
            }

            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new MethodInvoker(Func));
            }
            else
                Func();
        }

        public int nDoneCnt = 0;

        public System.Timers.Timer timer;

        public bool isReserved = false;
        public bool isR1 = false;
        public bool isR2 = false;
        public bool isR3 = false;
        public bool isR4 = false;

        public bool isRZ = false;
        public int nRZNum = -1;



        public void CheckReserve()
        {
            isReserved = true;
            isR1 = isR2 = isR3 = isR4 = isRZ = false;
        }

        private void WriteButtonClickHandler(object sender, EventArgs e)
        {
            if (sender.Equals(write1Btn))
            {
                tRBA1.Text = "10";
                tRBD1.Text = "2";
                tSFD1.Text = "5";
            }
            else if (sender.Equals(write2Btn))
            {
                tRBD1.Text = "2";
                tFBD1.Text = "3";
                tSFD1.Text = "5";
            }
            else if (sender.Equals(write3Btn))
            {
                tTF1.Text = "50";
                tFBD1.Text = "2";
                tTMAX1.Text = "0.1";
            }
            else if (sender.Equals(write4Btn))
            {
                tFBA1.Text = "30";
            }
            else if (sender.Equals(write5Btn))
            {
                tAIS1.Text = "10";
            }
        }

        public int nTimerMilliSec = 300;
        public const int TIMER_MOVING = 100;

        public void TimerButtonClickHandler(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(timerUpButton))
                {
                    nTimerMilliSec += TIMER_MOVING;
                    timer.Interval = nTimerMilliSec;
                    timerLabel.Text = nTimerMilliSec.ToString();
                }
                else if (sender.Equals(timerDownButton))
                {
                    if (nTimerMilliSec > 100)
                    {
                        nTimerMilliSec -= TIMER_MOVING;
                        timer.Interval = nTimerMilliSec;
                        timerLabel.Text = nTimerMilliSec.ToString();
                    }
                }
            }
            catch
            {
                nTimerMilliSec = 300;
                timer.Interval = nTimerMilliSec;
                timerLabel.Text = nTimerMilliSec.ToString();
            }
        }

        private void ReserveButtonClickHandler(object sender, EventArgs e)
        {
            CheckReserve();

            if (sender.Equals(reserve1Btn))
            {
                isR1 = true;
                ShowIndicator();
            }
            else if (sender.Equals(reserve2Btn))
            {
                isR2 = true;
                ShowIndicator();
            }
            else if (sender.Equals(reserve3Btn))
            {
                isR3 = true;
                ShowIndicator();
            }
            else if (sender.Equals(reserve4Btn))
            {
                isR4 = true;
                ShowIndicator();
            }
            else if (sender.Equals(confirmButton))
            {
                isReserved = false;
                timer.Enabled = false;
                ShowIndicator();
            }
        }



        // =======================================
        // 마지막 편집일 : 2023-04-20
        // 1. 행을 더블클릭하면 해당 종목의 차트뷰 폼을 콜한다.
        // =======================================
        public int nPrevEaIdx = 0;
        public void RowDoubleClickHandler(Object sender, MouseEventArgs e)
        {

            try
            {
                if (listView1.FocusedItem != null)
                {
                    nPrevEaIdx = mainForm.eachStockDict[listView1.FocusedItem.SubItems[0].Text.Trim()];
                    CallThreadEachStockHistoryForm(nPrevEaIdx);
                }
            }
            catch { }

        }

        public void MouseClickHandler(Object sender, EventArgs e)
        {
            try
            {
                if (listView1.FocusedItem != null)
                {
                    nPrevEaIdx = mainForm.eachStockDict[listView1.FocusedItem.SubItems[0].Text.Trim()];
                    if (timerCheckBox.Checked)
                        CallThreadEachStockHistoryForm(nPrevEaIdx);
                }
            }
            catch
            {

            }

        }


        public int sortColumn = -1;
        public const string UP_TIP = " ▲";
        public const string DOWN_TIP = " ▼";
        public int nTipLen = DOWN_TIP.Length;

        public void ColumnClickHandler(Object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                if (sortColumn != -1) // 처음이 아니라면
                    listView1.Columns[sortColumn].Text = listView1.Columns[sortColumn].Text.Substring(0, listView1.Columns[sortColumn].Text.Length - nTipLen);
                sortColumn = e.Column;
                // Set the sort ord6er to ascending by default.
                listView1.Sorting = SortOrder.Descending; // 초기에는 내림차순
                listView1.Columns[sortColumn].Text = listView1.Columns[sortColumn].Text + DOWN_TIP; // 콜롬명 ▼
            }
            else
            {
                listView1.Columns[sortColumn].Text = listView1.Columns[sortColumn].Text.Substring(0, listView1.Columns[sortColumn].Text.Length - nTipLen);
                // Determine what the last sort order was and change it.
                if (listView1.Sorting == SortOrder.Descending)
                {
                    listView1.Sorting = SortOrder.Ascending;
                    listView1.Columns[sortColumn].Text = listView1.Columns[sortColumn].Text + UP_TIP;
                }
                else
                {
                    listView1.Sorting = SortOrder.Descending;
                    listView1.Columns[sortColumn].Text = listView1.Columns[sortColumn].Text + DOWN_TIP;
                }
            }
            // Call the sort method to manually sort.
            listView1.ListViewItemSorter = new MyListViewComparer(listView1.Columns[e.Column].Name, e.Column, listView1.Sorting);
            listView1.Sort();
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        // 리스트뷰 비교 인스턴스
        class MyListViewComparer : IComparer
        {
            private int col;
            private SortOrder order;
            private string s;

            public MyListViewComparer()
            {
                col = 0;
                order = SortOrder.Ascending;
            }

            public MyListViewComparer(string s, int column, SortOrder order)
            {
                col = column;
                this.order = order;
                this.s = s;
            }

            public int Compare(object x, object y)
            {
                int returnVal = -1;

                if (s.Equals("STRING")) // string | boolean sort
                {
                    returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                }
                else if (s.Equals("INT")) // int sort
                {
                    int nX = int.Parse(((ListViewItem)x).SubItems[col].Text);
                    int nY = int.Parse(((ListViewItem)y).SubItems[col].Text);

                    if (nX == nY)
                        returnVal = 0;
                    else if (nX < nY)
                        returnVal = -1;
                    else
                        returnVal = 1;
                }
                else if (s.Equals("DOUBLE")) // double sort
                {
                    // 전량매수취소된경우 수익률이 0.0일텐데 이게 건들지 않아서  0.0이지 
                    // canceld는 다른애들과 대비되야 한다. 그래서 수익률 비교하기 전에 isCanceld를 먼저 비교해준다.
                    double fX = double.Parse(((ListViewItem)x).SubItems[col].Text);
                    double fY = double.Parse(((ListViewItem)y).SubItems[col].Text);

                    if (fX == fY)
                        returnVal = 0;
                    else if (fX < fY)
                        returnVal = -1;
                    else
                        returnVal = 1;
                }

                // Determine whether the sort order is descending.
                if (order == SortOrder.Descending)
                    // Invert the value returned by String.Compare.
                    returnVal *= -1;

                return returnVal;
            }
        }


        #region Thread Call Method
        public void CallThreadEachStockHistoryForm(int nCallIdx)
        {
            try
            {
                if ((DateTime.UtcNow - mainForm.ea[nCallIdx].myTradeManager.dLatestApproachTime).TotalSeconds >= 1 && !mainForm.ea[nCallIdx].myTradeManager.isEachStockHistoryExist)
                {
                    mainForm.ea[nCallIdx].myTradeManager.dLatestApproachTime = DateTime.UtcNow;
                    mainForm.ea[nCallIdx].myTradeManager.isEachStockHistoryExist = true;
                    new Thread(() => new EachStockHistoryForm(mainForm, nCallIdx).ShowDialog()).Start();
                }
            }
            catch { }
        }


        #endregion

        private void InitTargetButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < mainForm.nStockLength; i++)
                targetTimeArr[i] = 0;
        }
    }
}
