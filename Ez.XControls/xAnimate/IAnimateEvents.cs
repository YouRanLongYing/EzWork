using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ez.XControls.Library;

namespace Ez.XControls.xAnimate
{
    public interface IAnimateEvents<T>
    {
        /// <summary>
        /// 启动动画后的回调
        /// </summary>
        event EventAnimateHandler<T> AnimationStarted;
        /// <summary>
        /// 动画停止后的回调
        /// </summary>
        event EventAnimateHandler<T> AnimationStopped;

        /// <summary>
        /// 动画执行完事的回调
        /// </summary>
        event EventAnimateHandler<T> AnimationFinished;

        /// <summary>
        /// 动画执行中的回调
        /// </summary>
        event EventAnimateHandler<T> StepSizeChanged;

    }
}
