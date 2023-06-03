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
