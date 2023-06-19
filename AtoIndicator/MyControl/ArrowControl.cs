using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtoIndicator.MyControl
{
    public partial class ArrowControl : UserControl
    {
        private Color arrowColor = Color.White;
        private bool isMouseOver;
        private bool isBuy;
        private float fSizeScale;

        public event EventHandler LeftArrowClicked;
        public event EventHandler RightArrowClicked;


        public Color ArrowColor
        {
            get { return arrowColor; }
            set { arrowColor = value; }
        }

        public bool IsMouseOver
        {
            get { return isMouseOver; }
            set
            {
                isMouseOver = value;
                Refresh(); // Refresh the control to update the arrow size
            }
        }

        public ArrowControl(int x, int y, float fSize = 1, bool isBuy = true, EventHandler le = null, EventHandler re = null)
        {
            InitializeComponent();

            this.isBuy = isBuy;
            this.fSizeScale = fSize;
            if (le != null)
                this.LeftArrowClicked += le;
            if (re != null)
                this.RightArrowClicked += re;
            Location = new Point(x, y - (int)(15 * fSize));

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserMouse, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);

            MouseEnter += ArrowControl_MouseEnter;
            MouseLeave += ArrowControl_MouseLeave;
            MouseClick += ArrowControl_MouseClick;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            Size = new Size((int)Math.Ceiling(40 * fSizeScale), (int)Math.Ceiling(26 * fSizeScale));

            PointF[] arrowPoints = new PointF[]
                   {
                        new PointF((!isBuy?10:40) * fSizeScale, (10) *fSizeScale),
                        new PointF((!isBuy?30:20) * fSizeScale, (10) *fSizeScale),
                        new PointF((!isBuy?30:20) * fSizeScale, (4) *fSizeScale),
                        new PointF((!isBuy?40:10) * fSizeScale, (15) *fSizeScale),
                        new PointF((!isBuy?30:20) * fSizeScale, (26) *fSizeScale),
                        new PointF((!isBuy?30:20) * fSizeScale, (20) *fSizeScale),
                        new PointF((!isBuy?10:40) * fSizeScale, (20) *fSizeScale),

                   };

            e.Graphics.FillPolygon(new SolidBrush(arrowColor), arrowPoints);
            if (isMouseOver)
            {

                PointF[] arrowPointsLine = new PointF[]
                {
                        new PointF((!isBuy?10:40) * fSizeScale, (10) *fSizeScale),
                        new PointF((!isBuy?30:20) * fSizeScale, (10) *fSizeScale),
                        new PointF((!isBuy?30:20) * fSizeScale, (4) *fSizeScale),
                        new PointF((!isBuy?40:10) * fSizeScale, (15) *fSizeScale),
                        new PointF((!isBuy?30:20) * fSizeScale, (26) *fSizeScale),
                        new PointF((!isBuy?30:20) * fSizeScale, (20) *fSizeScale),
                        new PointF((!isBuy?10:40) * fSizeScale, (20) *fSizeScale),
                };
                e.Graphics.DrawPolygon(new Pen(Color.Green), arrowPointsLine);
            }

        }

        private void ArrowControl_MouseEnter(object sender, EventArgs e)
        {
            IsMouseOver = true;
        }

        private void ArrowControl_MouseLeave(object sender, EventArgs e)
        {
            IsMouseOver = false;
        }

        private void ArrowControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OnLeftArrowClicked();
            }
            else if (e.Button == MouseButtons.Right)
            {
                OnRightArrowClicked();
            }
        }

        protected virtual void OnLeftArrowClicked()
        {
            LeftArrowClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnRightArrowClicked()
        {
            RightArrowClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
