using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtoTrader.View.TextLog
{
    public partial class TextLogForm : Form
    {
        public MainForm mainForm;
        public TextLogForm(MainForm parentForm)
        {

            InitializeComponent();

            mainForm = parentForm;
            Print();

            this.KeyPreview = true;
            this.KeyUp += KeyUpHandler;
            this.Text = "텍스트 로그 기록";
            this.DoubleBuffered = true;
        }
        public void Print()
        {
            textBox1.Text = mainForm.sbLogTxtBx.ToString();
        }

        public void KeyUpHandler(object sender, KeyEventArgs e)
        {
            char cUp = (char)e.KeyValue;
            if (cUp == 'U')
                Print();

            if (cUp == 27 || cUp == 32) // esc
                this.Close();

        }
    }
}
