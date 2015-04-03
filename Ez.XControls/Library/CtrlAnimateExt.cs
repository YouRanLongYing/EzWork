using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.xAnimate;

namespace Ez.XControls.Library
{
    public static class CtrlAnimateExt
    {
        private static Hashtable animateparams = new Hashtable();
        public static Control Stop(this Control ctrl)
        {
           IAnimateFunc fun = animateparams[ctrl.GetHashCode()] as IAnimateFunc;
           if (fun != null)
           {
               fun.Stop();
               animateparams.Remove(ctrl.GetHashCode());
           }
           return ctrl;
        }
        public static Animate<T> Animate<T>(this Control ctrl, T toValue, int needtime, EventAnimateHandler<T> animationFinished=null)
        {
            Animate<T> animate = new Animate<T>(ctrl, needtime, toValue);
            if (animationFinished != null)
            {
                animate.AnimationFinished += animationFinished;
            }
            if (!animateparams.ContainsKey(ctrl.GetHashCode()))
            {
                animateparams.Add(ctrl.GetHashCode(), animate);
            }

            return animate;
        }
    }
}
