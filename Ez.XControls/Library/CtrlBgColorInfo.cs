using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ez.XControls.Library
{
    /// <summary>
    /// 背景配色方案
    /// </summary>
    public class CtrlBgColorInfo
    {
        /// <summary>
        /// 按下时背景色
        /// </summary>
        public Color PressedBgColor { set; get; }
        /// <summary>
        /// 进入时背景色
        /// </summary>
        public Color EnterBgColor { set; get; }
        /// <summary>
        /// 离开时背景色
        /// </summary>
        public Color LeaveBgColor { set; get; }
    }
}
