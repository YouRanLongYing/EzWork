using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ez.XControls.xAnimate
{
    public class AnimateArgs<T> : EventArgs
    {
        /// <summary>
        /// 当前的值
        /// </summary>
        public T CurrentValue { private set; get; }

        public AnimateArgs(T currentValue)
        {
            this.CurrentValue = currentValue;
        }
    }
}
