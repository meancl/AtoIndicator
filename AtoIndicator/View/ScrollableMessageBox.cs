using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            
            this.Text = "[개별] " + curEa.nLastRecordTime + " " + curEa.sCode + " " + curEa.sCodeName + " " + curEa.sMarketGubunTag + " " + curEa.myTradeManager.nIdx; 

            void WriteFunC(Object o, EventArgs e)
            {
                textBox1.Clear();
                RadioButton rd = (RadioButton)o;
                int nCheck = int.Parse(rd.Name);
                string sMessage;

                sMessage = $"========================  {nCheck}번째 매매블록정보 ====================={NEW_LINE}";

                if (curEa.myTradeManager.arrBuyedSlots[nCheck].isAllBuyed)
                {
                    sMessage += 
                        $"-------------------------- 추세각도 -------------------------{NEW_LINE}" +
                        $"분당각도 : {curEa.timeLines1m.fRecentMedianAngle}{NEW_LINE}{NEW_LINE}" +
                        $"--------------------------- 이평선 --------------------------{NEW_LINE}" +
                        $"ma2m : {curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.fCurMa20m}{NEW_LINE}" +
                        $"ma10m : {curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.fCurMa1h}{NEW_LINE}" +
                        $"ma20m : {curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.fCurMa2h}{NEW_LINE}" +
                        $"2m > 10m : {(curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.fCurMa20m > curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.fCurMa1h)}{NEW_LINE}" +
                        $"10m > 20m : {(curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.fCurMa1h > curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.fCurMa2h)}{NEW_LINE}" +
                        $"다운2m : {curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.nDownCntMa20m}{NEW_LINE}" +
                        $"다운10m : {curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.nDownCntMa1h}{NEW_LINE}" +
                        $"다운20m : {curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.nDownCntMa2h}{NEW_LINE}" +
                        $"업2m : {curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.nUpCntMa20m}{NEW_LINE}" +
                        $"업10m : {curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.nUpCntMa1h}{NEW_LINE}" +
                        $"업20m : {curEa.myTradeManager.arrBuyedSlots[nCheck].maOverN.nUpCntMa2h}{NEW_LINE}{NEW_LINE}" ;

                    sMessage += 
                            $"# -----------------------<< 매수정보 >>  ------------------------{NEW_LINE}" +
                            $"스텝  : {curEa.myTradeManager.arrBuyedSlots[nCheck].nCurLineIdx}{NEW_LINE}"+
                            $"매수요청시간 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nRequestTime}{NEW_LINE}"+
                            $"매수접수시간 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nReceiptTime}{NEW_LINE}"+
                            $"매수체결시간 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nBuyEndTime}{NEW_LINE}"+
                            $"주문수량 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nOrderVolume}{NEW_LINE}"+
                            $"매수수량 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nBuyVolume}{NEW_LINE}"+
                            $"주문가격 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nOriginOrderPrice}{NEW_LINE}"+
                            $"매수가격 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nBuyPrice}{NEW_LINE}"+
                            $"매수설명 : {curEa.myTradeManager.arrBuyedSlots[nCheck].sBuyDescription}{NEW_LINE}";

                    if (curEa.myTradeManager.arrBuyedSlots[nCheck].isAllSelled)
                    {
                        double profit = 0;
                        if (curEa.myTradeManager.arrBuyedSlots[nCheck].nBuyVolume > 0)
                            profit = ((double)(curEa.myTradeManager.arrBuyedSlots[nCheck].nDeathPrice - curEa.myTradeManager.arrBuyedSlots[nCheck].nBuyPrice) / curEa.myTradeManager.arrBuyedSlots[nCheck].nBuyPrice - MainForm.REAL_STOCK_COMMISSION) * 100;
                        else
                            profit = 0;

                        sMessage +=
                                $"# ---------------------- << 매도정보  >> ------------------------{NEW_LINE}" +
                                $"스텝  : {curEa.myTradeManager.arrBuyedSlots[nCheck].nCurLineIdx}{ NEW_LINE}" +
                                $"매도시간 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nDeathTime}{NEW_LINE}" +
                                $"매도물량 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nTotalSelledVolume}(주){NEW_LINE}" +
                                $"매도가격 : {curEa.myTradeManager.arrBuyedSlots[nCheck].nDeathPrice}(원){NEW_LINE}" +
                                $"(매수가격) : {curEa.myTradeManager.arrBuyedSlots[nCheck].nBuyPrice}{NEW_LINE}" +
                                $"손익률 : {profit}(%){NEW_LINE}" +
                                $"총손익금 : {(curEa.myTradeManager.arrBuyedSlots[nCheck].nDeathPrice - curEa.myTradeManager.arrBuyedSlots[nCheck].nBuyPrice) * curEa.myTradeManager.arrBuyedSlots[nCheck].nTotalSelledVolume}(원){NEW_LINE}" +
                                $"매도설명 : {curEa.myTradeManager.arrBuyedSlots[nCheck].sSellDescription}{NEW_LINE}";
                    }
                }
                
                textBox1.Text = sMessage;

            };

            int rdCnt = curEa.myTradeManager.nIdx;
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

                    if (curEa.myTradeManager.arrBuyedSlots[i].isAllBuyed) // 다 사졌으면
                    {
                        if (curEa.myTradeManager.arrBuyedSlots[i].nBuyVolume > 0) // 체결이 일부라도 됐으면
                            newRd.Text = $"{i}번째 매매블록 정보";
                        else                            
                            newRd.Text = $"{i}번째 매매블록(매수취소) 정보"; 

                        newRd.Name = i.ToString();
                        
                        newRd.CheckedChanged += new EventHandler(WriteFunC);

                    }
                    else
                    {
                        if (curEa.myTradeManager.arrBuyedSlots[i].isBuyBanned)
                            newRd.Text = $"{i}번째 매매블록(불능)";
                        else
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

            void WriteFunC (Object o, EventArgs e)
            {
                textBox1.Clear();
                RadioButton rd = (RadioButton)o;
                int nCheck = int.Parse(rd.Name);
                if(nCheck == 0)
                {
                    textBox1.Text = curEa.myTradeManager.sTotalLog.ToString();
                }
                else
                {
                    nCheck--;
                    textBox1.Text = curEa.myTradeManager.arrBuyedSlots[nCheck].sEachLog.ToString();
                }
            };
            
            int rdCnt = curEa.myTradeManager.nIdx;
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
