using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtoIndicator.View.TextLog
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
            this.FormClosed += FormClosedHandler;
        }
        public void Print()
        {
            textBox1.Text = mainForm.sbLogTxtBx.ToString();
        }
        public void FormClosedHandler(Object sender, FormClosedEventArgs e)
        {
            this.Dispose();
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
