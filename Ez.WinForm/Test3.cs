using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.xAnimate;
using Ez.XControls.Library;
namespace Ez.WinForm
{
    public partial class Test3 : Form
    {
        Point O= Point.Empty;
        public Test3()
        {
            InitializeComponent();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (O == Point.Empty)
            {
                O = new Point(panel1.Location.X + panel1.Size.Width / 2, panel1.Location.Y + panel1.Size.Height / 2);
                panel1.Location = O;
            }
            AnimateStart();
        }
        void AnimateStart()
        {
            Animate<Size> animate = panel1.Stop().Animate<Size>(new Size(300, 300), 100, animate1_AnimationFinished);
            animate.StepSizeChanged += animate2_StepSizeChanged;
            animate.Start();
        }
        void animate1_AnimationFinished(object obj, AnimateArgs<Size> args)
        {
            Animate<Size> animate = panel1.Stop().Animate<Size>(new Size(100, 100), 100, animate2_AnimationFinished);
            animate.StepSizeChanged += animate2_StepSizeChanged;
            animate.Start();
        }
        void animate2_AnimationFinished(object obj, AnimateArgs<Size> args)
        {
            Animate<Size> animate = panel1.Stop().Animate<Size>(new Size(300, 300), 100, animate1_AnimationFinished);
            animate.StepSizeChanged += animate2_StepSizeChanged;
            animate.Start();
        }
        void animate2_StepSizeChanged(object obj, AnimateArgs<Size> args)
        {
            panel1.Location = new Point(O.X - (args.CurrentValue.Width / 2), O.Y - (args.CurrentValue.Height / 2));
        }
    }
}
