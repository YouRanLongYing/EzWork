using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ez.XControls.Library
{
    /// <summary>
    /// 边框配色方案
    /// </summary>
    public struct CtrlBorderColorInfo
    {
        /// <summary>
        /// 按下时边框颜色
        /// </summary>
        public Color PressedBorderColor { set; get; }
        /// <summary>
        /// 松开时边框颜色
        /// </summary>
        public Color PressUpBorderColor { set; get; }
        /// <summary>
        /// 进入是边框颜色
        /// </summary>
        public Color EnterBorderColor { set; get; }
        /// <summary>
        /// 离开时的边框颜色，即默认颜色
        /// </summary>
        public Color LeaveBorderColor { set; get; }
    }
}
