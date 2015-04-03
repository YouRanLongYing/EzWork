using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ez.XControls.xAnimate
{
    public interface IAnimateFunc
    {
        /// <summary>
        /// 启动动画
        /// </summary>
        void Start();
        /// <summary>
        /// 停止动画
        /// </summary>
        void Stop();
    }
}
