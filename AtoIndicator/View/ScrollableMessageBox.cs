using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AtoIndicator.KiwoomLib.TimeLib;

namespace AtoIndicator.View.ScrollableMsgBox
{
    public partial class ScrollableMessageBox : Form
    {
        public ScrollableMessageBox()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyUp += KeyUpHandler;
            this.FormClosed += FormClosedHandler;
        }
        public void FormClosedHandler(Object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }


        public void Show(string sText)
        {
            textBox1.Text = sText;
            groupBox1.Visible = false;

            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("내용이 없습니다.");
                return;
            }
            else
            {
                Size size = TextRenderer.MeasureText(textBox1.Text, textBox1.Font);
                this.Width = (int)(size.Width * 1.2);
                this.Height = size.Height;
            }
            this.Text = "";
            this.ActiveControl = label1; // 처음에 액티브컨트롤이 textBox가 되면 눌러진 상태에서 시작하니(focused) 별로라 액티브 컨트롤을 텍스트박스 뒤 라벨로 맞춰버린다.
            this.Show();
        }

        public void KeyUpHandler(Object sender, KeyEventArgs e)
        {
            char cUp = (char)e.KeyValue;
            if (cUp == 27 || cUp == 32) // esc or space
                this.Close();
        }

        public void Show(string sText, string sTitle)
        {
            textBox1.Text = sText;
            groupBox1.Visible = false;

            Size size = TextRenderer.MeasureText(textBox1.Text, textBox1.Font);
            this.Width = (int)(size.Width * 1.2);
            this.Height = size.Height;

            this.Text = sTitle;
            this.ActiveControl = label1; // 처음에 액티브컨트롤이 textBox가 되면 눌러진 상태에서 시작하니(focused) 별로라 액티브 컨트롤을 텍스트박스 뒤 라벨로 맞춰버린다.
            this.Show();
        }

        public string NEW_LINE = Environment.NewLine;
        public void ShowEachBlock(MainForm.EachStock curEa)
        {

            groupBox1.Visible = true;
            blockFlowLayoutPanel.Controls.Clear();

            this.Text = "[개별] " + curEa.nLastRecordTime + " " + curEa.sCode + " " + curEa.sCodeName + " " + curEa.sMarketGubunTag + " " + curEa.myTradeManager.arrBuyedSlots.Count;

            void WriteFunC(Object o, EventArgs e)
            {
                textBox1.Clear();
                RadioButton rd = (RadioButton)o;
                int nCheck = int.Parse(rd.Name);
                string sMessage;

                sMessage = $"========================  {nCheck}번째 매매블록정보 ====================={NEW_LINE}";

                if (curEa.paperBuyStrategy.paperTradeSlot[nCheck].nBuyedVolume == curEa.paperBuyStrategy.paperTradeSlot[nCheck].nTargetRqVolume)
                {


                    sMessage +=
                        $"종목코드 : {curEa.sCode}{NEW_LINE}" +
                        $"매매블럭 : {nCheck}{NEW_LINE}" +
                        $"종목명 : {curEa.sCodeName}{NEW_LINE}" +
                        $"-------------매수--------------{NEW_LINE}" +
                        $"신청시간 : {curEa.paperBuyStrategy.paperTradeSlot[nCheck].nRqTime}{NEW_LINE}" +
                        $"체결시간 : {curEa.paperBuyStrategy.paperTradeSlot[nCheck].nBuyEndTime}{NEW_LINE}" +
                        $"체결수량 : {curEa.paperBuyStrategy.paperTradeSlot[nCheck].nBuyedVolume}{NEW_LINE}" +
                        $"체결가 : {curEa.paperBuyStrategy.paperTradeSlot[nCheck].nBuyedPrice}{NEW_LINE}";

                    if (curEa.paperBuyStrategy.paperTradeSlot[nCheck].isAllSelled)
                    {
                        if (curEa.paperBuyStrategy.paperTradeSlot[nCheck].nBuyedVolume > 0)
                            sMessage +=
                                $"-------------매도--------------{NEW_LINE}" +
                                $"매도시간 : {curEa.paperBuyStrategy.paperTradeSlot[nCheck].nSellEndTime}{NEW_LINE}" +
                                $"매도가 : {curEa.paperBuyStrategy.paperTradeSlot[nCheck].nSellEndPrice}{NEW_LINE}" +
                                $"매매시간 : {SubTimeToTime(curEa.paperBuyStrategy.paperTradeSlot[nCheck].nSellEndTime, curEa.paperBuyStrategy.paperTradeSlot[nCheck].nBuyEndTime)}{NEW_LINE}" +
                                $"매매결과 : {Math.Round(((double)(curEa.paperBuyStrategy.paperTradeSlot[nCheck].nSellEndPrice - curEa.paperBuyStrategy.paperTradeSlot[nCheck].nBuyedPrice) / curEa.paperBuyStrategy.paperTradeSlot[nCheck].nBuyedPrice - (MainForm.KOSDAQ_STOCK_TAX + MainForm.KIWOOM_STOCK_FEE * 2)) * 100, 2)}(%){NEW_LINE}";
                        else
                            sMessage += $"전량 매수취소됐습니다.{NEW_LINE}";

                    }


                    sMessage += curEa.paperBuyStrategy.paperTradeSlot[nCheck].sFixedMsg;
                }

                textBox1.Text = sMessage;

            };

            int rdCnt = curEa.paperBuyStrategy.nStrategyNum;
            if (rdCnt <= 0)
            {
                textBox1.Text = "현재 매매블록이 없습니다." + NEW_LINE;
            }
            else
            {
                textBox1.Text = "매매블록을 선택하십시오." + NEW_LINE;

                RadioButton newRd;
                // 개인
                for (int i = 0; i < rdCnt; i++)
                {
                    newRd = new RadioButton();

                    if (curEa.paperBuyStrategy.paperTradeSlot[i].nBuyedVolume == curEa.paperBuyStrategy.paperTradeSlot[i].nTargetRqVolume) // 다 사졌으면
                    {
                        if (curEa.paperBuyStrategy.paperTradeSlot[i].nBuyedVolume > 0) // 체결이 일부라도 됐으면
                            newRd.Text = $"{i}번째 매매블록 정보";
                        else
                            newRd.Text = $"{i}번째 매매블록(매수취소) 정보";

                        newRd.Name = i.ToString();

                        newRd.CheckedChanged += new EventHandler(WriteFunC);

                    }
                    else
                    {
                        newRd.Text = $"{i}번째 매매블록(매매중..)";
                        newRd.Enabled = false;
                    }
                    newRd.Width = (TextRenderer.MeasureText(newRd.Text, newRd.Font)).Width + 20;
                    blockFlowLayoutPanel.Controls.Add(newRd);
                    blockFlowLayoutPanel.SetFlowBreak(newRd, true); // 각행마다 너비가 차이나면 개행이 잘 안돼서 강제하는 코드
                }
            }

            this.ActiveControl = label1; // 처음에 액티브컨트롤이 textBox가 되면 눌러진 상태에서 시작하니(focused) 별로라 액티브 컨트롤을 텍스트박스 뒤 라벨로 맞춰버린다.
            this.Show();

        }
        public void ShowLog(MainForm.EachStock curEa)
        {
            groupBox1.Visible = true;
            blockFlowLayoutPanel.Controls.Clear();
            this.Text = curEa.sCode + " " + curEa.sCodeName;

            void WriteFunC(Object o, EventArgs e)
            {
                textBox1.Clear();
                RadioButton rd = (RadioButton)o;
                int nCheck = int.Parse(rd.Name);
                if (nCheck == 0)
                {
                    textBox1.Text = curEa.myTradeManager.sTotalLog.ToString();
                }
                else
                {
                    nCheck--;
                    textBox1.Text = curEa.myTradeManager.arrBuyedSlots[nCheck].sEachLog.ToString();
                }
            };

            int rdCnt = curEa.myTradeManager.arrBuyedSlots.Count;
            RadioButton newRd;

            // 전체 0번 확정
            {
                newRd = new RadioButton();
                newRd.Text = "전체로그";
                newRd.Name = (0).ToString();
                newRd.CheckedChanged += new EventHandler(WriteFunC);
                newRd.Width = (TextRenderer.MeasureText(newRd.Text, newRd.Font)).Width + 20;
                blockFlowLayoutPanel.Controls.Add(newRd);
                blockFlowLayoutPanel.SetFlowBreak(newRd, true); // 각행마다 너비가 차이나면 개행이 잘 안돼서 강제하는 코드

                newRd.Checked = true;
            }

            // 개인 1번부터 시작
            for (int idx = 0; idx < rdCnt; idx++)
            {
                newRd = new RadioButton();

                if (curEa.myTradeManager.arrBuyedSlots[idx].isAllBuyed || curEa.myTradeManager.arrBuyedSlots[idx].isBuyBanned) // 다 사졌으면
                {
                    if (curEa.myTradeManager.arrBuyedSlots[idx].nBuyVolume > 0) // 체결이 일부라도 됐으면
                        newRd.Text = idx.ToString() + "번째 로그";
                    else
                    {
                        if (curEa.myTradeManager.arrBuyedSlots[idx].isBuyBanned)
                            newRd.Text = idx.ToString() + "번째(불능) 로그";
                        else
                            newRd.Text = idx.ToString() + "번째(매수취소) 로그";
                    }

                    newRd.Name = (idx + 1).ToString();
                    newRd.CheckedChanged += new EventHandler(WriteFunC);

                }
                else
                {
                    newRd.Text = idx.ToString() + "번째(매매중..)";
                    newRd.Enabled = false;
                }
                newRd.Width = (TextRenderer.MeasureText(newRd.Text, newRd.Font)).Width + 20;
                blockFlowLayoutPanel.Controls.Add(newRd);
                blockFlowLayoutPanel.SetFlowBreak(newRd, true); // 각행마다 너비가 차이나면 개행이 잘 안돼서 강제하는 코드
            }


            this.ActiveControl = label1; // 처음에 액티브컨트롤이 textBox가 되면 눌러진 상태에서 시작하니(focused) 별로라 액티브 컨트롤을 텍스트박스 뒤 라벨로 맞춰버린다.
            this.Show();
        }
    }
}
