using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtoTrader.View.ScrollableMsgBox
{
    public partial class ScrollableMessageBox : Form
    {
        public ScrollableMessageBox()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyUp += KeyUpHandler;
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
        
    }
}
