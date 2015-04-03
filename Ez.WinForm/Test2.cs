using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ez.WinForm
{
    public partial class Test2 : Form
    {
        public Test2()
        {
            InitializeComponent();
            
        }
        public double drawdot(int t, int endTime, int endValue)
        {
            var y = -(endValue / Math.Pow(endTime, 2)) * Math.Pow(t - endTime, 2) + endValue;
            return y;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics g = this.CreateGraphics())
            {
                int t = 0;
                while (t < 1000)
                {
                    var y = drawdot(t, 400, 1000);
                    g.DrawRectangle(new Pen(Color.Red), new Rectangle((int)t, (int)y, 2, 2));
                    using (SolidBrush sb = new SolidBrush(Color.Red))
                    {
                        using (GraphicsPath gp = new GraphicsPath())
                        {
                            gp.AddRectangle(new Rectangle((int)t, (int)y, 2, 2));
                            g.FillPath(sb, gp);
                        }
                    }
                    t++;
                }
            }
        }
    }
}
