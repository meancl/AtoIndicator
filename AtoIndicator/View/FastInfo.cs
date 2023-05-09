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

namespace AtoTrader.View
{
    public partial class FastInfo : Form
    {
        public MainForm mainForm;

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
            listView1.Columns.Add(new ColumnHeader { Name = sInt, Text = "라이트통과" });
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
            
            if (cUp == 27 ) // esc or space
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
                fakeBuyTxtBox.Text = "";
                fakeAssistantTxtBox.Text = "";
                fakeResistTxtBox.Text = "";
                totalFakeTxtBox.Text = "";
                totalTrialTxtBox.Text = "";
                totalFakePassTxtBox.Text = "";
                startGapTxtBox.Text = "";
                powerWithoutGapTxtBox.Text = "";
                powerTxtBox.Text = "";
                downDepthTxtBox.Text = "";
                minDownDepthTxtBox.Text = "";
                totalTradePriceTxtBox.Text = "";
                buyPriceTxtBox.Text = "";
                sellPriceTxtBox.Text = "";
                fakeBuyMinTxtBox.Text = "";
                fakeAssistantMinTxtBox.Text = "";
                fakeResistMinTxtBox.Text = "";
                totalFakeMinTxtBox.Text = "";
                powerJarTxtBox.Text = "";
                upPowerJarTxtBox.Text = "";
                downPowerJarTxtBox.Text = "";
                plusCnt07TxtBox.Text = "";
                minusCnt07TxtBox.Text = "";
                plusCnt09TxtBox.Text = "";
                minusCnt09TxtBox.Text = "";
                ma20mTxtBox.Text = "";
                ma1hTxtBox.Text = "";
                ma2hTxtBox.Text = "";
                tAngleTxtBox.Text = "";
                hAngleTxtBox.Text = "";
                rAngleTxtBox.Text = "";
                dAngleTxtBox.Text = "";
                iAngleTxtBox.Text = "";
                mAngleTxtBox.Text = "";
                hogaCntTxtBox.Text = "";
                chegyulCntTxtBox.Text = "";
                pureTradeTxtBox.Text = "";
                hogaTradeTxtBox.Text = "";
                shareTradeTxtBox.Text = "";
                shareHogaTxtBox.Text = "";
                fsTxtBox.Text = "";
                hogaGapTxtBox.Text = "";
                speedRankTxtBox.Text = "";
                marketCapRankTxtBox.Text = "";
                powerRankTxtBox.Text = "";
                buyPriceRankTxtBox.Text = "";
                buyVolumeRankTxtBox.Text = "";
                tradePriceRankTxtBox.Text = "";
                tradeVolumeRankTxtBox.Text = "";
                totalRankTxtBox.Text = "";
                minSpeedRankTxtBox.Text = "";
                minUpDownRankTxtBox.Text = "";
                minPowerRankTxtBox.Text = "";
                minBuyPriceTxtBox.Text = "";
                minBuyVolumeTxtBox.Text = "";
                minTradePriceRankTxtBox.Text = "";
                minTradeVolumeRankTxtBox.Text = "";
                minTotalRankTxtBox.Text = "";
               
            }
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
              
                string sFakeBuy = fakeBuyTxtBox.Text.Trim();
                string sFakeAssistant = fakeAssistantTxtBox.Text.Trim();
                string sFakeResist = fakeResistTxtBox.Text.Trim();
                string sFakeVolatility = fakeVolatilityTxtBox.Text.Trim();
                string sTotalFake = totalFakeTxtBox.Text.Trim();
                string sTotalTrial = totalTrialTxtBox.Text.Trim();
                string sTotalFakePass = totalFakePassTxtBox.Text.Trim();
                string sStartGap = startGapTxtBox.Text.Trim();
                string sWithoutGap = powerWithoutGapTxtBox.Text.Trim();
                string sPower = powerTxtBox.Text.Trim();
                string sDownDepth = downDepthTxtBox.Text.Trim();
                string sMinDownDepth = minDownDepthTxtBox.Text.Trim();
                string sTotalTradePrice = totalTradePriceTxtBox.Text.Trim();
                string sBuyPrice = buyPriceTxtBox.Text.Trim();
                string sSellPrice = sellPriceTxtBox.Text.Trim();
                string sMinFakeBuy = fakeBuyMinTxtBox.Text.Trim();
                string sMinFakeAssistant = fakeAssistantMinTxtBox.Text.Trim();
                string sMinFakeResist = fakeResistMinTxtBox.Text.Trim();
                string sMinFakeVolatility = fakeVolatilityMinTxtBox.Text.Trim();
                string sMinTotalFake = totalFakeMinTxtBox.Text.Trim();
                string sPowerJar = powerJarTxtBox.Text.Trim();
                string sUpPowerJar = upPowerJarTxtBox.Text.Trim();
                string sDownPowerJar = downPowerJarTxtBox.Text.Trim();
                string sPCnt07 = plusCnt07TxtBox.Text.Trim();
                string sMCnt07 = minusCnt07TxtBox.Text.Trim();
                string sPCnt09 = plusCnt09TxtBox.Text.Trim();
                string sMCnt09 = minusCnt09TxtBox.Text.Trim();
                string sMa20m = ma20mTxtBox.Text.Trim();
                string sMa1h = ma1hTxtBox.Text.Trim();
                string sMa2h = ma2hTxtBox.Text.Trim();
                string sTAngle = tAngleTxtBox.Text.Trim();
                string sHAngle = hAngleTxtBox.Text.Trim();
                string sRAngle = rAngleTxtBox.Text.Trim();
                string sDAngle = dAngleTxtBox.Text.Trim();
                string sIAngle = iAngleTxtBox.Text.Trim();
                string sMAngle = mAngleTxtBox.Text.Trim();
                string sHogaCnt = hogaCntTxtBox.Text.Trim();
                string sChegyulCnt = chegyulCntTxtBox.Text.Trim();
                string sPureTrade = pureTradeTxtBox.Text.Trim();
                string sHogaTrade = hogaTradeTxtBox.Text.Trim();
                string sShareTrade = shareTradeTxtBox.Text.Trim();
                string sShareHoga = shareHogaTxtBox.Text.Trim();
                string sFsPrice = fsTxtBox.Text.Trim();
                string sHogaGap = hogaGapTxtBox.Text.Trim();
                string sSpeedRank = speedRankTxtBox.Text.Trim();
                string sMarketCapRank = marketCapRankTxtBox.Text.Trim();
                string sPowerRank = powerRankTxtBox.Text.Trim();
                string sBuyPriceRank = buyPriceRankTxtBox.Text.Trim();
                string sBuyVolumeRank = buyVolumeRankTxtBox.Text.Trim();
                string sTradePriceRank = tradePriceRankTxtBox.Text.Trim();
                string sTradeVolumeRank = tradeVolumeRankTxtBox.Text.Trim();
                string sTotalRank = totalRankTxtBox.Text.Trim();
                string sMinSpeedRank = minSpeedRankTxtBox.Text.Trim();
                string sMinUpDownRank = minUpDownRankTxtBox.Text.Trim();
                string sMinPowerRank = minPowerRankTxtBox.Text.Trim();
                string sMinBuyPriceRank = minBuyPriceTxtBox.Text.Trim();
                string sMinBuyVolumeRank = minBuyVolumeTxtBox.Text.Trim();
                string sMinTradePriceRank = minTradePriceRankTxtBox.Text.Trim();
                string sMinTradeVolumeRank = minTradeVolumeRankTxtBox.Text.Trim();
                string sMinTotalRank = minTotalRankTxtBox.Text.Trim();



                bool isFakeBuy = !sFakeBuy.Equals("");
                bool isFakeAssistant = !sFakeAssistant.Equals("");
                bool isFakeResist = !sFakeResist.Equals("");
                bool isFakeVolatility = !sFakeVolatility.Equals("");
                bool isTotalFake = !sTotalFake.Equals("");
                bool isTotalTrial = !sTotalTrial.Equals("");
                bool isTotalFakePass = !sTotalFakePass.Equals("");
                bool isStartGap = !sStartGap.Equals("");
                bool isWithoutGap = !sWithoutGap.Equals("");
                bool isPower = !sPower.Equals("");
                bool isDownDepth = !sDownDepth.Equals("");
                bool isMinDownDepth = !sMinDownDepth.Equals("");
                bool isTotalTradePrice = !sTotalTradePrice.Equals("");
                bool isBuyPrice = !sBuyPrice.Equals("");
                bool isSellPrice = !sSellPrice.Equals("");
                bool isMinFakeBuy = !sMinFakeBuy.Equals("");
                bool isMinFakeAssistant = !sMinFakeAssistant.Equals("");
                bool isMinFakeResist = !sMinFakeResist.Equals("");
                bool isMinFakeVolatility = !sMinFakeVolatility.Equals("");
                bool isMinTotalFake = !sMinTotalFake.Equals("");
                bool isPowerJar = !sPowerJar.Equals("");
                bool isUpPowerJar = !sUpPowerJar.Equals("");
                bool isDownPowerJar = !sDownPowerJar.Equals("");
                bool isPCnt07 = !sPCnt07.Equals("");
                bool isMCnt07 = !sMCnt07.Equals("");
                bool isPCnt09 = !sPCnt09.Equals("");
                bool isMCnt09 = !sMCnt09.Equals("");
                bool isMa20m = !sMa20m.Equals("");
                bool isMa1h = !sMa1h.Equals("");
                bool isMa2h = !sMa2h.Equals("");
                bool isTAngle = !sTAngle.Equals("");
                bool isHAngle = !sHAngle.Equals("");
                bool isRAngle = !sRAngle.Equals("");
                bool isDAngle = !sDAngle.Equals("");
                bool isIAngle = !sIAngle.Equals("");
                bool isMAngle = !sMAngle.Equals("");
                bool isHogaCnt = !sHogaCnt.Equals("");
                bool isChegyulCnt = !sChegyulCnt.Equals("");
                bool isPureTrade = !sPureTrade.Equals("");
                bool isHogaTrade = !sHogaTrade.Equals("");
                bool isShareTrade = !sShareTrade.Equals("");
                bool isShareHoga = !sShareHoga.Equals("");
                bool isFsPrice = !sFsPrice.Equals("");
                bool isHogaGap = !sHogaGap.Equals("");
                bool isSpeedRank = !sSpeedRank.Equals("");
                bool isMarketCapRank = !sMarketCapRank.Equals("");
                bool isPowerRank = !sPowerRank.Equals("");
                bool isBuyPriceRank = !sBuyPriceRank.Equals("");
                bool isBuyVolumeRank = !sBuyVolumeRank.Equals("");
                bool isTradePriceRank = !sTradePriceRank.Equals("");
                bool isTradeVolumeRank = !sTradeVolumeRank.Equals("");
                bool isTotalRank = !sTotalRank.Equals("");
                bool isMinSpeedRank = !sMinSpeedRank.Equals("");
                bool isMinUpDownRank = !sMinUpDownRank.Equals("");
                bool isMinPowerRank = !sMinPowerRank.Equals("");
                bool isMinBuyPriceRank = !sMinBuyPriceRank.Equals("");
                bool isMinBuyVolumeRank = !sMinBuyVolumeRank.Equals("");
                bool isMinTradePriceRank = !sMinTradePriceRank.Equals("");
                bool isMinTradeVolumeRank = !sMinTradeVolumeRank.Equals("");
                bool isMinTotalRank = !sMinTotalRank.Equals("");

                int nPass; // pass cnt
                int nPassLen = 0;
                int nFullMinusNum = GetPassNum(new bool[] {
                                        isFakeBuy,
                                        isFakeAssistant,
                                        isFakeResist,
                                        isFakeVolatility,
                                        isTotalFake,
                                        isTotalTrial,
                                        isTotalFakePass,
                                        isStartGap,
                                        isWithoutGap,
                                        isPower,
                                        isDownDepth,
                                        isMinDownDepth,
                                        isTotalTradePrice,
                                        isBuyPrice,
                                        isSellPrice,
                                        isMinFakeBuy,
                                        isMinFakeAssistant,
                                        isMinFakeResist,
                                        isMinFakeVolatility,
                                        isMinTotalFake,
                                        isPowerJar,
                                        isUpPowerJar,
                                        isDownPowerJar,
                                        isPCnt07,
                                        isMCnt07,
                                        isPCnt09,
                                        isMCnt09,
                                        isMa20m,
                                        isMa1h,
                                        isMa2h,
                                        isTAngle,
                                        isHAngle,
                                        isRAngle,
                                        isDAngle,
                                        isIAngle,
                                        isMAngle,
                                        isHogaCnt,
                                        isChegyulCnt,
                                        isPureTrade,
                                        isHogaTrade,
                                        isShareTrade,
                                        isShareHoga,
                                        isFsPrice,
                                        isHogaGap,
                                        isSpeedRank,
                                        isMarketCapRank,
                                        isPowerRank,
                                        isBuyPriceRank,
                                        isBuyVolumeRank,
                                        isTradePriceRank,
                                        isTradeVolumeRank,
                                        isTotalRank,
                                        isMinSpeedRank,
                                        isMinUpDownRank,
                                        isMinPowerRank,
                                        isMinBuyPriceRank,
                                        isMinBuyVolumeRank,
                                        isMinTradePriceRank,
                                        isMinTradeVolumeRank,
                                        isMinTotalRank
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

                    if (isFakeBuy)
                        nPass += (int.Parse(sFakeBuy) <= mainForm.ea[i].fakeBuyStrategy.nStrategyNum) ? 1 : 0;
                    if (isFakeAssistant)
                        nPass += (int.Parse(sFakeAssistant) <= mainForm.ea[i].fakeAssistantStrategy.nStrategyNum) ? 1 : 0;
                    if (isFakeResist)
                        nPass += (int.Parse(sFakeResist) <= mainForm.ea[i].fakeResistStrategy.nStrategyNum) ? 1 : 0;
                    if (isFakeVolatility)
                        nPass += (int.Parse(sFakeVolatility) <= mainForm.ea[i].fakeVolatilityStrategy.nStrategyNum) ? 1 : 0;
                    if (isTotalFake)
                        nPass += (int.Parse(sTotalFake) <= mainForm.ea[i].fakeStrategyMgr.nTotalFakeCount) ? 1 : 0;
                    if (isTotalTrial)
                        nPass += (int.Parse(sTotalTrial) <= mainForm.ea[i].fakeStrategyMgr.nTotalFakeCount) ? 1 : 0;
                    if (isTotalFakePass)
                        nPass += (int.Parse(sTotalFakePass) <= mainForm.ea[i].fakeStrategyMgr.nFakeAccumPassed) ? 1 : 0;
                    if (isStartGap)
                        nPass += (double.Parse(sStartGap) <= mainForm.ea[i].fStartGap) ? 1 : 0;
                    if (isWithoutGap)
                        nPass += (double.Parse(sWithoutGap) <= mainForm.ea[i].fPowerWithoutGap) ? 1 : 0;
                    if (isPower)
                        nPass += (double.Parse(sPower) <= mainForm.ea[i].fPower) ? 1 : 0;
                    if (isDownDepth && mainForm.ea[i].nYesterdayEndPrice > 0)
                        nPass += (double.Parse(sDownDepth) <= (double)(mainForm.ea[i].nRealMaxPrice - mainForm.ea[i].nFs) / mainForm.ea[i].nYesterdayEndPrice ) ? 1 : 0;
                    if (isMinDownDepth && mainForm.ea[i].nYesterdayEndPrice > 0 )
                        nPass += (double.Parse(sMinDownDepth) <= (double)(mainForm.ea[i].timeLines1m.nMaxUpFs - mainForm.ea[i].nFs) / mainForm.ea[i].nYesterdayEndPrice) ? 1 : 0;
                    if (isTotalTradePrice)
                        nPass += (double.Parse(sTotalTradePrice) * 100000000 <= mainForm.ea[i].lTotalTradePrice) ? 1 : 0;
                    if (isBuyPrice)
                        nPass += (double.Parse(sBuyPrice) * 100000000 <= mainForm.ea[i].lOnlyBuyPrice) ? 1 : 0;
                    if (isSellPrice)
                        nPass += (double.Parse(sSellPrice) * 100000000 <= mainForm.ea[i].lOnlySellPrice) ? 1 : 0;
                    if (isMinFakeBuy)
                        nPass += (int.Parse(sMinFakeBuy) <= mainForm.ea[i].fakeBuyStrategy.nMinuteLocationCount) ? 1 : 0;
                    if (isMinFakeAssistant)
                        nPass += (int.Parse(sMinFakeAssistant) <= mainForm.ea[i].fakeAssistantStrategy.nMinuteLocationCount) ? 1 : 0;
                    if (isMinFakeResist)
                        nPass += (int.Parse(sMinFakeResist) <= mainForm.ea[i].fakeResistStrategy.nMinuteLocationCount) ? 1 : 0;
                    if (isMinFakeVolatility)
                        nPass += (int.Parse(sMinFakeVolatility) <= mainForm.ea[i].fakeVolatilityStrategy.nMinuteLocationCount) ? 1 : 0;
                    if (isMinTotalFake)
                        nPass += (int.Parse(sMinTotalFake) <= mainForm.ea[i].fakeStrategyMgr.nSharedMinuteLocationCount) ? 1 : 0;
                    if (isPowerJar)
                        nPass += (double.Parse(sPowerJar) <= mainForm.ea[i].fPowerJar) ? 1 : 0;
                    if (isUpPowerJar)
                        nPass += (double.Parse(sUpPowerJar) <= mainForm.ea[i].fOnlyUpPowerJar) ? 1 : 0;
                    if (isDownPowerJar)
                        nPass += (double.Parse(sDownPowerJar) * (-1) >= mainForm.ea[i].fOnlyDownPowerJar) ? 1 : 0;
                    if (isPCnt07)
                        nPass += (double.Parse(sPCnt07) <= mainForm.ea[i].fPlusCnt07) ? 1 : 0;
                    if (isMCnt07)
                        nPass += (double.Parse(sMCnt07) <= mainForm.ea[i].fMinusCnt07) ? 1 : 0;
                    if (isPCnt09)
                        nPass += (double.Parse(sPCnt09) <= mainForm.ea[i].fPlusCnt09) ? 1 : 0;
                    if (isMCnt09)
                        nPass += (double.Parse(sMCnt09) <= mainForm.ea[i].fMinusCnt09) ? 1 : 0;
                    if (isMa20m)
                        nPass += (int.Parse(sMa20m) <= mainForm.ea[i].maOverN.nDownCntMa20m) ? 1 : 0;
                    if (isMa1h)
                        nPass += (int.Parse(sMa1h) <= mainForm.ea[i].maOverN.nDownCntMa1h) ? 1 : 0;
                    if (isMa2h)
                        nPass += (int.Parse(sMa2h) <= mainForm.ea[i].maOverN.nDownCntMa2h) ? 1 : 0;
                    if (isTAngle)
                        nPass += (double.Parse(sTAngle) <= mainForm.ea[i].timeLines1m.fTotalMedianAngle) ? 1 : 0;
                    if (isHAngle)
                        nPass += (double.Parse(sHAngle) <= mainForm.ea[i].timeLines1m.fHourMedianAngle) ? 1 : 0;
                    if (isRAngle)
                        nPass += (double.Parse(sRAngle) <= mainForm.ea[i].timeLines1m.fRecentMedianAngle) ? 1 : 0;
                    if (isDAngle)
                        nPass += (double.Parse(sDAngle) * (-1) >= mainForm.ea[i].timeLines1m.fDAngle) ? 1 : 0;
                    if (isIAngle)
                        nPass += (double.Parse(sIAngle) <= mainForm.ea[i].timeLines1m.fInitAngle) ? 1 : 0;
                    if (isMAngle)
                        nPass += (double.Parse(sMAngle) <= mainForm.ea[i].timeLines1m.fMaxAngle) ? 1 : 0;
                    if (isHogaCnt)
                        nPass += (int.Parse(sHogaCnt) <= mainForm.ea[i].nHogaCnt) ? 1 : 0;
                    if (isChegyulCnt)
                        nPass += (int.Parse(sChegyulCnt) <= mainForm.ea[i].nChegyulCnt) ? 1 : 0;
                    if (isPureTrade)
                        nPass += (double.Parse(sPureTrade) <= mainForm.ea[i].fTradePerPure) ? 1 : 0;
                    if (isHogaTrade)
                        nPass += (double.Parse(sHogaTrade) >= mainForm.ea[i].fHogaPerTrade) ? 1 : 0;
                    if (isShareTrade)
                        nPass += (double.Parse(sShareTrade) >= mainForm.ea[i].fSharePerTrade) ? 1 : 0;
                    if (isShareHoga)
                        nPass += (double.Parse(sShareHoga) >= mainForm.ea[i].fSharePerHoga) ? 1 : 0;
                    if (isFsPrice)
                        nPass += (int.Parse(sFsPrice) <= mainForm.ea[i].nFs) ? 1 : 0;
                    if (isHogaGap && mainForm.ea[i].nYesterdayEndPrice > 0)
                        nPass += (double.Parse(sHogaGap) <= ((double)(mainForm.ea[i].nFs - mainForm.ea[i].nFb) / mainForm.ea[i].nYesterdayEndPrice)) ? 1 : 0;
                    if (isSpeedRank)
                        nPass += (int.Parse(sSpeedRank) >= mainForm.ea[i].rankSystem.nAccumCountRanking) ? 1 : 0;
                    if (isMarketCapRank)
                        nPass += (int.Parse(sMarketCapRank) >= mainForm.ea[i].rankSystem.nMarketCapRanking) ? 1 : 0;
                    if (isPowerRank)
                        nPass += (int.Parse(sPowerRank) >= mainForm.ea[i].rankSystem.nPowerRanking) ? 1 : 0;
                    if (isBuyPriceRank)
                        nPass += (int.Parse(sBuyPriceRank) >= mainForm.ea[i].rankSystem.nTotalBuyPriceRanking) ? 1 : 0;
                    if (isBuyVolumeRank)
                        nPass += (int.Parse(sBuyVolumeRank) >= mainForm.ea[i].rankSystem.nTotalBuyVolumeRanking) ? 1 : 0;
                    if (isTradePriceRank)
                        nPass += (int.Parse(sTradePriceRank) >= mainForm.ea[i].rankSystem.nTotalTradePriceRanking) ? 1 : 0;
                    if (isTradeVolumeRank)
                        nPass += (int.Parse(sTradeVolumeRank) >= mainForm.ea[i].rankSystem.nTotalTradeVolumeRanking) ? 1 : 0;
                    if (isTotalRank)
                        nPass += (int.Parse(sTotalRank) >= mainForm.ea[i].rankSystem.nSummationRanking) ? 1 : 0;
                    if (isMinSpeedRank)
                        nPass += (int.Parse(sMinSpeedRank) >= mainForm.ea[i].rankSystem.nMinuteCountRanking) ? 1 : 0;
                    if (isMinUpDownRank)
                        nPass += (int.Parse(sMinUpDownRank) >= mainForm.ea[i].rankSystem.nMinuteUpDownRanking) ? 1 : 0;
                    if (isMinPowerRank)
                        nPass += (int.Parse(sMinPowerRank) >= mainForm.ea[i].rankSystem.nMinutePowerRanking) ? 1 : 0;
                    if (isMinBuyPriceRank)
                        nPass += (int.Parse(sMinBuyPriceRank) >= mainForm.ea[i].rankSystem.nMinuteBuyPriceRanking) ? 1 : 0;
                    if (isMinBuyVolumeRank)
                        nPass += (int.Parse(sMinBuyVolumeRank) >= mainForm.ea[i].rankSystem.nMinuteBuyVolumeRanking) ? 1 : 0;
                    if (isMinTradePriceRank)
                        nPass += (int.Parse(sMinTradePriceRank) >= mainForm.ea[i].rankSystem.nMinuteTradePriceRanking) ? 1 : 0;
                    if (isMinTradeVolumeRank)
                        nPass += (int.Parse(sMinTradeVolumeRank) >= mainForm.ea[i].rankSystem.nMinuteTradeVolumeRanking) ? 1 : 0;
                    if (isMinTotalRank)
                        nPass += (int.Parse(sMinTotalRank) >= mainForm.ea[i].rankSystem.nMinuteSummationRanking) ? 1 : 0;



                    if (nPass >= nFinalPassNum)
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
                        mainForm.ea[i].fakeStrategyMgr.nFakeAccumPassed.ToString(),
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
