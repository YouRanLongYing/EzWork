using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ez.XControls.xAnimate
{
    public class AnimateTimer : IDisposable
    {
        public delegate void EventAnimateHandler();
        public Control target;
        public int sleep;
        public AnimateTimer(Control target, int sleep)
        {
            this.target = target;
            this.sleep = sleep;
        }
        public event EventAnimateHandler Tick;
        private Thread thread;
        public void Start()
        {
            thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (this.Tick == null)
                    {
                        break;
                    }
                    if (!this.target.IsDisposed && this.target.IsHandleCreated)
                    {
                        Thread.Sleep(this.sleep);

                        try
                        {
                            this.target.BeginInvoke(Tick);
                        }
                        catch (InvalidOperationException)
                        {
                            this.Dispose();
                        }
                    }
                    else
                    {
                        Stop();
                    }

                }
            }));
            thread.Start();
        }
        public void Stop()
        {
            if (thread != null)
            {
                thread.Abort();
            }
        }

        public void Dispose()
        {
            if (this.thread != null)
            {
                this.thread.Abort();
                this.Dispose();
            }
        }
    }
}
