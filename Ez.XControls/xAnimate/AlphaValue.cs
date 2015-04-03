using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ez.XControls.xAnimate
{
    /// <summary>
    /// 用于颜色变化
    /// </summary>
    public class AlphaValue
    {
        public int Value { private set; get; }

        public static AlphaValue Get(int value)
        {
            AlphaValue alpah = new AlphaValue()
            {

                Value = value
            };
            return alpah;
        }
    }
}
