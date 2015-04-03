using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ez.XControls.Library
{
    /// <summary>
    /// 背景配图方案
    /// </summary>
    public class CtrlBgInfo
    {
        /// <summary>
        /// 按下时的背景
        /// </summary>
        public Image PressedBg { set; get; }
        /// <summary>
        /// 进入时的背景
        /// </summary>
        public Image EnterBg { set; get; }
        /// <summary>
        /// 离开时的背景
        /// </summary>
        public Image LeaveBg { set; get; }

    }
}
