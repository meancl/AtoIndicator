using AtoTrader.View.EachStockHistory;
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
using static AtoTrader.KiwoomLib.TimeLib;

namespace AtoTrader.View
{

    public partial class FastInfo : Form
    {
        public MainForm mainForm;

        public int GetPassNum(bool[] pas)
        {
            int retval = 0;
            for (int i = 0; i < pas.Length; i++)
                if (pas[i])
                    retval++;
            return retval;
        }


        public FastInfo(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            string sString = "STRING";
            string sInt = "INT";
            string sDouble = "DOUBLE";

            listView1.Columns.Add(new ColumnHeader { Name = sString, Text = "종목코드" });
            listView1.Columns.Add(new ColumnHeader { Name = sString, Text = "종목명" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "총 랭크" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "분 랭크" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "페매수" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "페보조" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "페저항" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "페변동" });
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "총 페이크" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "초기갭" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "갭제외" });
            listView1.Columns.Add(new ColumnHeader { Name = sDouble, Text = "현재파워" });


            listView1.View = System.Windows.Forms.View.Details;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.MouseDoubleClick += RowDoubleClickHandler;
            listView1.ColumnClick += ColumnClickHandler;

            this.KeyPreview = true;
            this.KeyUp += KeyUpHandler;
            this.groupBox1.DoubleClick += DoubleClickHandler;
            this.DoubleBuffered = true;


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

        public void KeyUpHandler(object sender, KeyEventArgs k)
        {
            char cUp = (char)k.KeyValue;

            if (cUp == 27) // esc or space
                this.Close();

            if (cUp == 16)
            {
                if (sortColumn != -1)
                    listView1.Columns[sortColumn].Text = listView1.Columns[sortColumn].Text.Substring(0, listView1.Columns[sortColumn].Text.Length - nTipLen);
                sortColumn = -1;
                UpdateTable();
            }

            if (cUp == 17)
            {
                passNumTxtBox.Text = "";

                tCC1.Text = "";
                tCC2.Text = "";
                tFB1.Text = "";
                tFB2.Text = "";
                tFA1.Text = "";
                tFA2.Text = "";
                tFR1.Text = "";
                tFR2.Text = "";
                tFV1.Text = "";
                tFV2.Text = "";
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
                tCTC1.Text = "";
                tCTC2.Text = "";
                tCPC1.Text = "";
                tCPC2.Text = "";
                tCTH1.Text = "";
                tCTH2.Text = "";
                tCTBH1.Text = "";
                tCTBH2.Text = "";
                tCTBTD1.Text = "";
                tCTBTD2.Text = "";
                tCBCTD1.Text = "";
                tCBCTD2.Text = "";
                tCTCTD1.Text = "";
                tCTCTD2.Text = "";
                tBUT1.Text = "";
                tBUT2.Text = "";
                tBUB1.Text = "";
                tBUB2.Text = "";
                tBUJ1.Text = "";
                tBUJ2.Text = "";
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
            }
        }


        bool isUsing = false;
        public void UpdateTable()
        {
            if (isUsing)
                return;

            isUsing = true;

            // Clear the sorting criteria applied to the ListView control
            listView1.Sorting = SortOrder.None;
            listView1.Sort();

            listView1.Items.Clear();
            listView1.BeginUpdate();

            try
            {
                string sCC1 = tCC1.Text.Trim();
                string sCC2 = tCC2.Text.Trim();
                string sFB1 = tFB1.Text.Trim();
                string sFB2 = tFB2.Text.Trim();
                string sFA1 = tFA1.Text.Trim();
                string sFA2 = tFA2.Text.Trim();
                string sFR1 = tFR1.Text.Trim();
                string sFR2 = tFR2.Text.Trim();
                string sFV1 = tFV1.Text.Trim();
                string sFV2 = tFV2.Text.Trim();
                string sTF1 = tTF1.Text.Trim();
                string sTF2 = tTF2.Text.Trim();
                string sSG1 = tSG1.Text.Trim();
                string sSG2 = tSG2.Text.Trim();
                string sWOG1 = tWOG1.Text.Trim();
                string sWOG2 = tWOG2.Text.Trim();
                string sCP1 = tCP1.Text.Trim();
                string sCP2 = tCP2.Text.Trim();
                string sPD1 = tPD1.Text.Trim();
                string sPD2 = tPD2.Text.Trim();
                string sPJ1 = tPJ1.Text.Trim();
                string sPJ2 = tPJ2.Text.Trim();
                string sUPJ1 = tUPJ1.Text.Trim();
                string sUPJ2 = tUPJ2.Text.Trim();
                string sDPJ1 = tDPJ1.Text.Trim();
                string sDPJ2 = tDPJ2.Text.Trim();
                string sCTC1 = tCTC1.Text.Trim();
                string sCTC2 = tCTC2.Text.Trim();
                string sCPC1 = tCPC1.Text.Trim();
                string sCPC2 = tCPC2.Text.Trim();
                string sCTH1 = tCTH1.Text.Trim();
                string sCTH2 = tCTH2.Text.Trim();
                string sCTBH1 = tCTBH1.Text.Trim();
                string sCTBH2 = tCTBH2.Text.Trim();
                string sCTBTD1 = tCTBTD1.Text.Trim();
                string sCTBTD2 = tCTBTD2.Text.Trim();
                string sCBCTD1 = tCBCTD1.Text.Trim();
                string sCBCTD2 = tCBCTD2.Text.Trim();
                string sCTCTD1 = tCTCTD1.Text.Trim();
                string sCTCTD2 = tCTCTD2.Text.Trim();
                string sBUT1 = tBUT1.Text.Trim();
                string sBUT2 = tBUT2.Text.Trim();
                string sBUB1 = tBUB1.Text.Trim();
                string sBUB2 = tBUB2.Text.Trim();
                string sBUJ1 = tBUJ1.Text.Trim();
                string sBUJ2 = tBUJ2.Text.Trim();
                string sTTM1 = tTTM1.Text.Trim();
                string sTTM2 = tTTM2.Text.Trim();
                string sBM1 = tBM1.Text.Trim();
                string sBM2 = tBM2.Text.Trim();
                string sSM1 = tSM1.Text.Trim();
                string sSM2 = tSM2.Text.Trim();
                string sTA1 = tTA1.Text.Trim();
                string sTA2 = tTA2.Text.Trim();
                string sHA1 = tHA1.Text.Trim();
                string sHA2 = tHA2.Text.Trim();
                string sRA1 = tRA1.Text.Trim();
                string sRA2 = tRA2.Text.Trim();
                string sDA1 = tDA1.Text.Trim();
                string sDA2 = tDA2.Text.Trim();
                string s1P1 = t1P1.Text.Trim();
                string s1P2 = t1P2.Text.Trim();
                string s2P1 = t2P1.Text.Trim();
                string s2P2 = t2P2.Text.Trim();
                string s3P1 = t3P1.Text.Trim();
                string s3P2 = t3P2.Text.Trim();
                string s4P1 = t4P1.Text.Trim();
                string s4P2 = t4P2.Text.Trim();


                bool isCC1 = !sCC1.Equals("");
                bool isCC2 = !sCC2.Equals("");
                bool isFB1 = !sFB1.Equals("");
                bool isFB2 = !sFB2.Equals("");
                bool isFA1 = !sFA1.Equals("");
                bool isFA2 = !sFA2.Equals("");
                bool isFR1 = !sFR1.Equals("");
                bool isFR2 = !sFR2.Equals("");
                bool isFV1 = !sFV1.Equals("");
                bool isFV2 = !sFV2.Equals("");
                bool isTF1 = !sTF1.Equals("");
                bool isTF2 = !sTF2.Equals("");
                bool isSG1 = !sSG1.Equals("");
                bool isSG2 = !sSG2.Equals("");
                bool isWOG1 = !sWOG1.Equals("");
                bool isWOG2 = !sWOG2.Equals("");
                bool isCP1 = !sCP1.Equals("");
                bool isCP2 = !sCP2.Equals("");
                bool isPD1 = !sPD1.Equals("");
                bool isPD2 = !sPD2.Equals("");
                bool isPJ1 = !sPJ1.Equals("");
                bool isPJ2 = !sPJ2.Equals("");
                bool isUPJ1 = !sUPJ1.Equals("");
                bool isUPJ2 = !sUPJ2.Equals("");
                bool isDPJ1 = !sDPJ1.Equals("");
                bool isDPJ2 = !sDPJ2.Equals("");
                bool isCTC1 = !sCTC1.Equals("");
                bool isCTC2 = !sCTC2.Equals("");
                bool isCPC1 = !sCPC1.Equals("");
                bool isCPC2 = !sCPC2.Equals("");
                bool isCTH1 = !sCTH1.Equals("");
                bool isCTH2 = !sCTH2.Equals("");
                bool isCTBH1 = !sCTBH1.Equals("");
                bool isCTBH2 = !sCTBH2.Equals("");
                bool isCTBTD1 = !sCTBTD1.Equals("");
                bool isCTBTD2 = !sCTBTD2.Equals("");
                bool isCBCTD1 = !sCBCTD1.Equals("");
                bool isCBCTD2 = !sCBCTD2.Equals("");
                bool isCTCTD1 = !sCTCTD1.Equals("");
                bool isCTCTD2 = !sCTCTD2.Equals("");
                bool isBUT1 = !sBUT1.Equals("");
                bool isBUT2 = !sBUT2.Equals("");
                bool isBUB1 = !sBUB1.Equals("");
                bool isBUB2 = !sBUB2.Equals("");
                bool isBUJ1 = !sBUJ1.Equals("");
                bool isBUJ2 = !sBUJ2.Equals("");
                bool isTTM1 = !sTTM1.Equals("");
                bool isTTM2 = !sTTM2.Equals("");
                bool isBM1 = !sBM1.Equals("");
                bool isBM2 = !sBM2.Equals("");
                bool isSM1 = !sSM1.Equals("");
                bool isSM2 = !sSM2.Equals("");
                bool isTA1 = !sTA1.Equals("");
                bool isTA2 = !sTA2.Equals("");
                bool isHA1 = !sHA1.Equals("");
                bool isHA2 = !sHA2.Equals("");
                bool isRA1 = !sRA1.Equals("");
                bool isRA2 = !sRA2.Equals("");
                bool isDA1 = !sDA1.Equals("");
                bool isDA2 = !sDA2.Equals("");
                bool is1P1 = !s1P1.Equals("");
                bool is1P2 = !s1P2.Equals("");
                bool is2P1 = !s2P1.Equals("");
                bool is2P2 = !s2P2.Equals("");
                bool is3P1 = !s3P1.Equals("");
                bool is3P2 = !s3P2.Equals("");
                bool is4P1 = !s4P1.Equals("");
                bool is4P2 = !s4P2.Equals("");

                int nPass; // pass cnt
                int nPassLen = 0;
                int nFullMinusNum = GetPassNum(new bool[] {
                                        isCC1 || isCC2,
                                        isFB1 || isFB2,
                                        isFA1 || isFA2 ,
                                        isFR1 || isFR2 ,
                                        isFV1 || isFV2 ,
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
                                        isSM1 || isSM2 ,
                                        isTA1 || isTA2 ,
                                        isHA1 || isHA2 ,
                                        isRA1 || isRA2 ,
                                        isDA1 || isDA2 ,
                                        is1P1 || is1P2 ,
                                        is2P1 || is2P2 ,
                                        is3P1 || is3P2 ,
                                        is4P1 || is4P2
                                     });

                string sPassNum = passNumTxtBox.Text.Trim();
                int nPassMinusNum = 0;

                int nFinalPassNum = 0;

                List<ListViewItem> listViewItemList = new List<ListViewItem>();

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




                for (int i = 0; i < mainForm.nStockLength; i++)
                {
                    nPass = 0;

                    if (isCC1 || isCC2)
                        nPass += ((isCC1 ? int.Parse(sCC1) <= mainForm.ea[i].crushMgr.crushBoxes.Count : true) &&
                            (isCC2 ? mainForm.ea[i].crushMgr.crushBoxes.Count <= int.Parse(sCC2) : true)) ? 1 : 0;
                    if (isFB1 || isFB2)
                        nPass += ((isFB1 ? int.Parse(sFB1) <= mainForm.ea[i].fakeBuyStrategy.nStrategyNum : true) &&
                            (isFB2 ? mainForm.ea[i].fakeBuyStrategy.nStrategyNum <= int.Parse(sFB2) : true)) ? 1 : 0;
                    if (isFA1 || isFA2)
                        nPass += ((isFA1 ? int.Parse(sFA1) <= mainForm.ea[i].fakeAssistantStrategy.nStrategyNum : true) &&
                            (isFA2 ? mainForm.ea[i].fakeAssistantStrategy.nStrategyNum <= int.Parse(sFA2) : true)) ? 1 : 0;
                    if (isFR1 || isFR2)
                        nPass += ((isFR1 ? int.Parse(sFR1) <= mainForm.ea[i].fakeResistStrategy.nStrategyNum : true) &&
                            (isFR2 ? mainForm.ea[i].fakeResistStrategy.nStrategyNum <= int.Parse(sFR2) : true)) ? 1 : 0;
                    if (isFV1 || isFV2)
                        nPass += ((isFV1 ? int.Parse(sFV1) <= mainForm.ea[i].fakeVolatilityStrategy.nStrategyNum : true) &&
                            (isFV2 ? mainForm.ea[i].fakeVolatilityStrategy.nStrategyNum <= int.Parse(sFV2) : true)) ? 1 : 0;
                    if (isTF1 || isTF2)
                        nPass += ((isTF1 ? int.Parse(sTF1) <= mainForm.ea[i].fakeBuyStrategy.nStrategyNum : true) &&
                            (isTF2 ? mainForm.ea[i].fakeBuyStrategy.nStrategyNum <= int.Parse(sTF2) : true)) ? 1 : 0;
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
                        nPass += ((isPD1 ? double.Parse(sPD1) <= (double)(mainForm.ea[i].nFs - mainForm.ea[i].timeLines1m.nMaxUpFs) / mainForm.ea[i].nYesterdayEndPrice : true) &&
                            (isPD2 ? (double)(mainForm.ea[i].nFs - mainForm.ea[i].timeLines1m.nMaxUpFs) / mainForm.ea[i].nYesterdayEndPrice <= double.Parse(sPD2) : true)) ? 1 : 0;
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
                        nPass += ((isTTM1 ? long.Parse(sTTM1) * 100000000 <= mainForm.ea[i].lTotalTradePrice : true) &&
                            (isTTM2 ? mainForm.ea[i].lTotalTradePrice <= long.Parse(sTTM2) * 100000000 : true)) ? 1 : 0;
                    if (isBM1 || isBM2)
                        nPass += ((isBM1 ? long.Parse(sBM1) * 100000000 <= mainForm.ea[i].lOnlyBuyPrice : true) &&
                            (isBM2 ? mainForm.ea[i].lOnlyBuyPrice <= long.Parse(sBM2) * 100000000 : true)) ? 1 : 0;
                    if (isSM1 || isSM2)
                        nPass += ((isSM1 ? long.Parse(sSM1) * 100000000 <= mainForm.ea[i].lOnlySellPrice : true) &&
                            (isSM2 ? mainForm.ea[i].lOnlySellPrice <= long.Parse(sSM2) * 100000000 : true)) ? 1 : 0;
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


                    // 전고점 
                    bool isCrushPass = true;
                    int nTrial = 0;
                    int nPassed = 0;
                    for (int nCC = 0; nCC < mainForm.ea[i].crushMgr.crushBoxes.Count; i++)
                    {
                        if (isCTC1 || isCTC2)
                        {
                            if ((isCTC1 ? int.Parse(sCTC1) <= nCC : true) &&
                                (isCTC2 ? nCC <= int.Parse(sCTC2) : true)
                                )
                            {
                                nTrial++;

                                if ((isCTH1 || isCTH2) && mainForm.ea[i].nYesterdayEndPrice > 0) // 고점 높이
                                    if (!(isCTH1 ? double.Parse(sCTH1) <= (double)(mainForm.ea[i].crushMgr.crushBoxes[nCC].maxPoint.nPrice - mainForm.ea[i].nTodayStartPrice) / mainForm.ea[i].nYesterdayEndPrice : true) &&
                                        (isCTH2 ? (double)(mainForm.ea[i].crushMgr.crushBoxes[nCC].maxPoint.nPrice - mainForm.ea[i].nTodayStartPrice) / mainForm.ea[i].nYesterdayEndPrice <= double.Parse(sCTH2) : true))
                                        continue;
                                if ((isCTBH1 || isCTBH2) && mainForm.ea[i].nYesterdayEndPrice > 0) // 전고 높이차
                                    if (!((isCTBH1 ? double.Parse(sCTBH1) <= (double)(mainForm.ea[i].crushMgr.crushBoxes[nCC].maxPoint.nPrice - mainForm.ea[i].crushMgr.crushBoxes[nCC].minPoint.nPrice) / mainForm.ea[i].nYesterdayEndPrice : true) &&
                                        (isCTBH2 ? (double)(mainForm.ea[i].crushMgr.crushBoxes[nCC].maxPoint.nPrice - mainForm.ea[i].crushMgr.crushBoxes[nCC].minPoint.nPrice) / mainForm.ea[i].nYesterdayEndPrice <= double.Parse(sCTBH2) : true)))
                                        continue;
                                if (isCTBTD1 || isCTBTD2) // 전고 시간차
                                    if (!(isCTBTD1 ? int.Parse(sCTBTD1) <= SubTimeToTimeAndSec(mainForm.ea[i].crushMgr.crushBoxes[nCC].minPoint.nTime, mainForm.ea[i].crushMgr.crushBoxes[nCC].maxPoint.nTime) : true) &&
                                        (isCTBTD2 ? SubTimeToTimeAndSec(mainForm.ea[i].crushMgr.crushBoxes[nCC].minPoint.nTime, mainForm.ea[i].crushMgr.crushBoxes[nCC].maxPoint.nTime) <= int.Parse(sCTBTD2) : true))
                                        continue;
                                if (isCBCTD1 || isCBCTD2) // 저점 간 시간차
                                    if (!(isCBCTD1 ? int.Parse(sCBCTD1) <= SubTimeToTimeAndSec(mainForm.ea[i].crushMgr.crushBoxes[nCC].crushPoint.nTime, mainForm.ea[i].crushMgr.crushBoxes[nCC].minPoint.nTime) : true) &&
                                        (isCBCTD2 ? SubTimeToTimeAndSec(mainForm.ea[i].crushMgr.crushBoxes[nCC].crushPoint.nTime, mainForm.ea[i].crushMgr.crushBoxes[nCC].minPoint.nTime) <= int.Parse(sCBCTD2) : true))
                                        continue;
                                if (isCTCTD1 || isCTCTD2) // 고점 간 시간차
                                    if (!(isCTCTD1 ? int.Parse(sCTCTD1) <= SubTimeToTimeAndSec(mainForm.ea[i].crushMgr.crushBoxes[nCC].crushPoint.nTime, mainForm.ea[i].crushMgr.crushBoxes[nCC].maxPoint.nTime) : true) &&
                                        (isCTCTD2 ? SubTimeToTimeAndSec(mainForm.ea[i].crushMgr.crushBoxes[nCC].crushPoint.nTime, mainForm.ea[i].crushMgr.crushBoxes[nCC].maxPoint.nTime) <= int.Parse(sCTCTD2) : true))
                                        continue;

                                nPassed++;
                            }
                        }
                        else // crush count에 속하지 않는다면
                        {
                            break;
                        }
                    }
                    if (isCPC1 || isCPC2)
                        isCrushPass = ((isCPC1 ? int.Parse(sCPC1) <= nPassed : true) &&
                        (isCPC2 ? nPassed <= int.Parse(sCPC2) : true));
                    else
                        isCrushPass = nPassed >= 0;



                    // 봇업
                    bool isBotUpPass = true;
                    {
                        if ((isBUT1 || isBUT2) && mainForm.ea[i].nYesterdayEndPrice > 0)
                            isBotUpPass = isBotUpPass &&
                                ((isBUT1 ? double.Parse(sBUT1) <= (double)(mainForm.ea[i].crushMgr.maxPoint.nPrice - mainForm.ea[i].nTodayStartPrice) / mainForm.ea[i].nYesterdayEndPrice : true) &&
                                (isBUT2 ? (double)(mainForm.ea[i].crushMgr.maxPoint.nPrice - mainForm.ea[i].nTodayStartPrice) / mainForm.ea[i].nYesterdayEndPrice <= double.Parse(sBUT2) : true));
                        if ((isBUB1 || isBUB2) && mainForm.ea[i].nYesterdayEndPrice > 0)
                            isBotUpPass = isBotUpPass &&
                                ((isBUB1 ? double.Parse(sBUB1) <= (double)(mainForm.ea[i].crushMgr.maxPoint.nPrice - mainForm.ea[i].crushMgr.minPoint.nPrice) / mainForm.ea[i].nYesterdayEndPrice : true) &&
                                (isBUB2 ? (double)(mainForm.ea[i].crushMgr.maxPoint.nPrice - mainForm.ea[i].crushMgr.minPoint.nPrice) / mainForm.ea[i].nYesterdayEndPrice <= double.Parse(sBUB2) : true));
                        if ((isBUJ1 || isBUJ2) && mainForm.ea[i].nYesterdayEndPrice > 0)
                            isBotUpPass = isBotUpPass &&
                                ((isBUJ1 ? double.Parse(sBUJ1) <= (double)(mainForm.ea[i].nFs - mainForm.ea[i].crushMgr.minPoint.nPrice) / mainForm.ea[i].nYesterdayEndPrice : true) &&
                                (isBUJ2 ? (double)(mainForm.ea[i].nFs - mainForm.ea[i].crushMgr.minPoint.nPrice) / mainForm.ea[i].nYesterdayEndPrice <= double.Parse(sBUJ2) : true));
                    }



                    if (nPass >= nFinalPassNum && isBotUpPass && isCrushPass && isCrushPass)
                    {
                        nPassLen++;
                        ListViewItem listViewItem = new ListViewItem(new string[] {
                        mainForm.ea[i].sCode,
                        mainForm.ea[i].sCodeName,
                        mainForm.ea[i].rankSystem.nSummationRanking.ToString(),
                        mainForm.ea[i].rankSystem.nMinuteSummationRanking.ToString(),
                        mainForm.ea[i].fakeBuyStrategy.nStrategyNum.ToString(),
                        mainForm.ea[i].fakeAssistantStrategy.nStrategyNum.ToString(),
                        mainForm.ea[i].fakeResistStrategy.nStrategyNum.ToString(),
                        mainForm.ea[i].fakeVolatilityStrategy.nStrategyNum.ToString(),
                        mainForm.ea[i].fakeStrategyMgr.nTotalFakeCount.ToString(),
                        Math.Round(mainForm.ea[i].fStartGap, 3).ToString(),
                        Math.Round(mainForm.ea[i].fPowerWithoutGap, 3).ToString(),
                        Math.Round(mainForm.ea[i].fPower, 3).ToString(),
                            });

                        if (mainForm.ea[i].fPowerWithoutGap == 0) //  평균 이율
                            listViewItem.BackColor = Color.FromArgb(255, 255, 255);
                        else if (mainForm.ea[i].fPowerWithoutGap > 0)
                        {
                            int nColorStep = (int)(mainForm.ea[i].fPowerWithoutGap * 100 / 0.1 * 2);
                            if (nColorStep > 255)
                                nColorStep = 255;
                            listViewItem.BackColor = Color.FromArgb(255, 255 - nColorStep, 255 - nColorStep);
                        }
                        else
                        {
                            int nColorStep = (int)(Math.Abs(mainForm.ea[i].fPowerWithoutGap) * 100 / 0.1 * 2);
                            if (nColorStep > 255)
                                nColorStep = 255;
                            listViewItem.BackColor = Color.FromArgb(255 - nColorStep, 255 - nColorStep, 255);
                        }

                        listViewItemList.Add(listViewItem);

                    }

                }

                if (listViewItemList.Count > 0)
                    listView1.Items.AddRange(listViewItemList.ToArray());

                doneLabel.Text = $"done..{++nDoneCnt}";
                passLenLabel.Text = $"pass {nPassLen}";
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

        public int nDoneCnt = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.InvokeRequired)
            {
                if (sortColumn != -1)
                    listView1.Columns[sortColumn].Text = listView1.Columns[sortColumn].Text.Substring(0, listView1.Columns[sortColumn].Text.Length - nTipLen);
                sortColumn = -1;
                listView1.Invoke(new MethodInvoker(UpdateTable));
            }
            else
            {
                if (sortColumn != -1)
                    listView1.Columns[sortColumn].Text = listView1.Columns[sortColumn].Text.Substring(0, listView1.Columns[sortColumn].Text.Length - nTipLen);
                sortColumn = -1;
                UpdateTable();
            }


        }

        // =======================================
        // 마지막 편집일 : 2023-04-20
        // 1. 행을 더블클릭하면 해당 종목의 차트뷰 폼을 콜한다.
        // =======================================
        public void RowDoubleClickHandler(Object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
                CallThreadEachStockHistoryForm(mainForm.eachStockDict[listView1.FocusedItem.SubItems[0].Text.Trim()]);
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
            new Thread(() => new EachStockHistoryForm(mainForm, nCallIdx).ShowDialog()).Start();
        }



        #endregion

    }
}
