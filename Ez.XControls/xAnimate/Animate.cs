#define UseFun
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.xAnimate;

namespace Ez.XControls.xAnimate
{

    public delegate void EventAnimateHandler<T>(object obj, AnimateArgs<T> args);
    /// <summary>
    /// 动画类
    /// </summary>
    /// <typeparam name="T">只接受 Point Color</typeparam>
    public class Animate<T> : IAnimateEvents<T>, IAnimateFunc, IDisposable
    {
        private AnimateTimer timer;
        private int time;

        /// <summary>
        /// 启动动画后的回调
        /// </summary>
        public event EventAnimateHandler<T> AnimationStarted;
        /// <summary>
        /// 动画停止后的回调
        /// </summary>
        public event EventAnimateHandler<T> AnimationStopped;

        /// <summary>
        /// 动画执行完事的回调
        /// </summary>
        public event EventAnimateHandler<T> AnimationFinished;

        /// <summary>
        /// 动画执行中的回调
        /// </summary>
        public event EventAnimateHandler<T> StepSizeChanged;

        /// <summary>
        /// 要执行到的动画值
        /// </summary>
        private T ToValue { set; get; }
        /// <summary>
        /// 执行动画的目标
        /// </summary>
        public Control Target { private set; get; }

        /// <summary>
        /// 初始化动画
        /// </summary>
        /// <param name="target">执行动画的控件</param>
        /// <param name="needTime">执行所需要的时间,毫秒</param>
        /// <param name="toValue">变化的值</param>
        public Animate(Control target, int needTime, T toValue)
        {
            this.Target = target;
            this.time = needTime;
            this.ToValue = toValue;
            this.timer = new AnimateTimer(this.Target, 2);
        }

        private T current;
        /// <summary>
        /// 使用初始值执行动画
        /// </summary>
        public void Start()
        {
            this.Target.SuspendLayout();
            if (this.AnimationStarted != null)
            {
                this.AnimationFinished(this.Target, new AnimateArgs<T>(this.ToValue));
            }
            if (this.ToValue.GetType() == typeof(Point))
            {
                LocationChange();
            }
            else if (this.ToValue.GetType() == typeof(Color))
            {
                ColorChange();
            }
            else if (this.ToValue.GetType() == typeof(AlphaValue))
            {
                AlhpaChange();
            }
            else if (this.ToValue.GetType() == typeof(Size))
            {
                SizeChange();
            }
            this.Target.ResumeLayout();
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            if (this.AnimationStopped != null)
            {
                this.AnimationStopped(this.Target, new AnimateArgs<T>(current));
            }
        }
        /// <summary>
        /// 位置变化
        /// </summary>
        private void LocationChange()
        {

#if UseFun
            Point point = (Point)Convert.ChangeType(this.ToValue, typeof(Point));
            Point curPos = this.Target.Location;
            Point offset = new Point(point.X - curPos.X, point.Y - curPos.Y);

            int fun_x = 0;
            int yy = this.Target.Location.Y;
            int xx = this.Target.Location.X;
            bool xFinish = false;
            bool yFinish = false;

            timer.Tick += delegate
            {
                if (this.Target.IsDisposed)
                {
                    this.timer.Stop();
                    return;
                }

                #region 配置执行所需参数
                bool is_y_increace_fun = offset.Y > 0;
                bool is_x_increace_fun = offset.X > 0;
                int asix_x = fun_x++;

                double x = GetAnimateY(asix_x, time, Math.Abs(offset.X)) * (is_x_increace_fun ? 1 : -1) + xx;
                double y = GetAnimateY(asix_x, time, Math.Abs(offset.Y)) * (is_y_increace_fun ? 1 : -1) + yy;
                if (is_y_increace_fun)
                {
                    yFinish = y >= offset.Y + yy;
                }
                else
                {
                    yFinish = (y - yy) <= offset.Y;
                }

                if (is_x_increace_fun)
                {
                    xFinish = (x - xx) >= offset.X;
                }
                else
                {
                    xFinish = (x - xx) <= offset.X;
                }

                if (xFinish && yFinish)
                {
                    this.timer.Stop();
                    if (this.AnimationFinished != null)
                    {
                        this.AnimationFinished(this.Target, new AnimateArgs<T>(current));
                    }
                    return;
                }
                #endregion

                #region 执行动画
                if (!xFinish && !yFinish)
                {
                    this.Target.Location = new Point((int)x, (int)y);
                }
                else if (!xFinish)
                {
                    this.Target.Location = new Point((int)x, this.Target.Location.Y);
                }
                else if (!yFinish)
                {
                    this.Target.Location = new Point(this.Target.Size.Width, (int)y);
                }
                if (!xFinish || !yFinish)
                {
                    if (this.StepSizeChanged != null)
                    {
                        current = (T)Convert.ChangeType(this.Target.Location, typeof(T));
                        this.StepSizeChanged(this.Target, new AnimateArgs<T>(current));
                    }
                }
                #endregion
             };
            timer.Start();
#else

            Point point = (Point)Convert.ChangeType(this.ToValue, typeof(Point));
            Point curPos = this.Target.Location;
            Point offset = new Point(point.X - curPos.X, point.Y - curPos.Y);
            int offsetX_step = offset.X / time;
            int offsetY_step = offset.Y / time;
            Point recorder = offset;
            timer.Tick += delegate
            {

                if (offsetX_step == 0 && offsetY_step != 0)
                {
                    this.Target.Location = new Point(this.Target.Location.X + offset.X, this.Target.Location.Y + offsetY_step);
                    recorder.X += -offset.X;
                    recorder.Y += -offsetY_step;
                }
                else if (offsetX_step != 0 && offsetY_step == 0)
                {
                    this.Target.Location = new Point(this.Target.Location.X + offset.X, this.Target.Location.Y + offset.Y);
                    recorder.X += -offsetX_step;
                    recorder.Y += -offset.Y;
                }
                else if (offsetX_step != 0 && offsetY_step != 0)
                {

                    this.Target.Location = new Point(this.Target.Location.X + offsetX_step, this.Target.Location.Y + offsetY_step);
                    recorder.X += -offsetX_step;
                    recorder.Y += -offsetY_step;
                }
                else
                {
                    this.Target.Location = new Point(this.Target.Location.X + offset.X, this.Target.Location.Y + offset.Y);
                    recorder.X += -offset.X;
                    recorder.Y += -offset.Y;
                }
                current = (T)Convert.ChangeType(this.Target.Location, typeof(T));
                if (recorder == Point.Empty)
                {
                    this.timer.Stop();
                    if (this.AnimationFinished != null)
                    {
                        this.AnimationFinished(this.Target, new AnimateArgs<T>(current));
                    }
                }
                else
                {
                    if (this.StepSizeChanged != null)
                    {
                        this.StepSizeChanged(this.Target, new AnimateArgs<T>(current));
                    }
                }
            };
            timer.Start();
#endif
        }

        /// <summary>
        /// 颜色变化函数 TODO:
        /// </summary>
        private void ColorChange()
        {
            //TODO:
        }
        /// <summary>
        /// 透明度变化 TODO: 
        /// </summary>
        private void AlhpaChange()
        {
            //TODO:
        }
        /// <summary>
        /// 颜色变化
        /// </summary>
        private void SizeChange()
        {
            Size size = (Size)Convert.ChangeType(this.ToValue, typeof(Size));
            Size curSize = this.Target.Size;
            Size offset = new Size(size.Width - curSize.Width, size.Height - curSize.Height);
#if UseFun
            int fun_x = 0;
            int yy = this.Target.Size.Height;
            int xx = this.Target.Size.Width;
            bool wFinish = false;
            bool hFinish = false;
#else
            int offsetW_step = offset.Width / time;
            int offsetH_step = offset.Height / time;
            Size recorder = offset;
#endif
            timer.Tick += delegate
            {
#if UseFun
                if (this.Target.IsDisposed)
                {
                    this.timer.Stop();
                    return;
                }

                #region 配置执行所需参数
                bool is_h_increace_fun = offset.Height > 0;
                bool is_w_increace_fun = offset.Width > 0;
                int asix_x = fun_x++;

                double x = GetAnimateY(asix_x, time,Math.Abs(offset.Width))  * (is_w_increace_fun ? 1 : -1) + xx;
                double y = GetAnimateY(asix_x, time,Math.Abs(offset.Height)) * (is_h_increace_fun ? 1 : -1) + yy;
                if (is_h_increace_fun)
                {
                    hFinish = y >= offset.Height + yy;
                }
                else
                {
                    hFinish = (y - yy) <= offset.Height;
                }

                if (is_w_increace_fun)
                {
                    wFinish = (x - xx) >= offset.Width;
                }
                else
                {
                    wFinish = (x - xx) <= offset.Width;
                }

                if (wFinish && hFinish)
                {
                    this.timer.Stop();
                    if (this.AnimationFinished != null)
                    {
                        this.AnimationFinished(this.Target, new AnimateArgs<T>(current));
                    }
                    return;
                }
#endregion

                #region 执行动画
                Size tosize = Size.Empty;
                if (!wFinish && !hFinish)
                {
                    tosize = new Size((int)x, (int)y);
                }
                else if (!wFinish)
                {
                    tosize = new Size((int)x, this.Target.Size.Height);
                }
                else if (!hFinish)
                {
                    tosize = new Size(this.Target.Size.Width, (int)y);
                }
                if ((!wFinish || !hFinish) && tosize!=Size.Empty)
                {
                    if (this.StepSizeChanged != null)
                    {
                        current = (T)Convert.ChangeType(tosize, typeof(T));
                        this.StepSizeChanged(this.Target, new AnimateArgs<T>(current));
                    }
                }
                this.Target.Size = tosize;
                #endregion
#else
                if (offsetW_step == 0 && offsetH_step != 0)
                {
                    this.Target.Size = new Size(this.Target.Size.Width + offset.Width, this.Target.Size.Height + offsetH_step);
                    recorder.Width += -offset.Width;
                    recorder.Height += -offsetH_step;
                }
                else if (offsetW_step != 0 && offsetH_step == 0)
                {
                    this.Target.Size = new Size(this.Target.Size.Width + offset.Width, this.Target.Size.Height + offset.Height);
                    recorder.Width += -offsetW_step;
                    recorder.Height += -offset.Height;
                }
                else if (offsetW_step != 0 && offsetH_step != 0)
                {

                    this.Target.Size = new Size(this.Target.Size.Width + offsetW_step, this.Target.Size.Height + offsetH_step);
                    recorder.Width += -offsetW_step;
                    recorder.Height += -offsetH_step;
                }
                else
                {
                    this.Target.Size = new Size(this.Target.Size.Width + offset.Width, this.Target.Size.Height + offset.Height);
                    recorder.Width += -offset.Width;
                    recorder.Height += -offset.Height;
                }
                current = (T)Convert.ChangeType(this.Target.Size, typeof(T));
                if (recorder == Size.Empty)
                {
                    this.timer.Stop();
                    if (this.AnimationFinished != null)
                    {
                        this.AnimationFinished(this.Target, new AnimateArgs<T>(current));
                    }
                }
                else
                {
                    if (this.StepSizeChanged != null)
                    {
                        this.StepSizeChanged(this.Target, new AnimateArgs<T>(current));
                    }
                }
#endif
            };
            timer.Start();
        }

        public double GetAnimateY(int t, int endTime, int endValue)
        {
            var y = -(endValue / Math.Pow(endTime, 2)) * Math.Pow(t - endTime, 2) + endValue;
            return y;
        }

        public void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Dispose();
                this.Dispose();
            }
        }
    }
}
