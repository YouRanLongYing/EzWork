using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.Menus;

namespace Ez.XControls.Library
{
    public interface ICtrlSolution
    {

    }
    /// <summary>
    /// 自定义控件解决方案基类
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DesignerSerializer("System.Windows.Forms.Design.ControlCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [ToolboxItemFilter("System.Windows.Forms")]
    public class CtrlSolution : Control, ICtrlSolution
    {
        public CtrlSolution()
        { 
        
        }
        public CtrlSolution(bool Selectable =true)
        {
            if (Selectable)
            {
               // this.SetStyle(ControlStyles.Selectable, true);
            }
        }

        #region 内部属性
        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        protected bool IsMousePressd { set; get; }
        /// <summary>
        /// 鼠标是否进入控件区域
        /// </summary>
        protected bool IsMouseEnter { set; get; }
        /// <summary>
        /// 控件 是否获得焦点
        /// </summary>
        protected bool IsGotFocus { set; get; }

        #endregion

        #region 控制属性
        
        /// <summary>
        /// 当前的背景色
        /// </summary>
        protected virtual Color CurrentBgColor
        {
            get
            {
                if (this.IsMousePressd || this.IsGotFocus)
                {
                    return this.BgColorInfo.PressedBgColor;
                }
                else if (this.IsMouseEnter && !this.IsMousePressd)
                {
                    return this.BgColorInfo.EnterBgColor;
                }
                else
                {
                    return this.BgColorInfo.LeaveBgColor;
                }
            }
        }
         
        /// <summary>
        /// 当前的边框颜色
        /// </summary>
        protected virtual Color CurrentBorderColor
        {
            get
            {
                if (this.IsMousePressd)
                {
                    return this.BorderColorInfo.PressedBorderColor;
                }
                else if (this.IsMouseEnter && !this.IsMousePressd)
                {
                    return this.BorderColorInfo.EnterBorderColor;
                }
                else
                {
                    return this.BorderColorInfo.LeaveBorderColor;
                }
            }
        }
         
        /// <summary>
        /// 当前背景图
        /// </summary>
        protected virtual Image CurrentBg
        {
            get
            {
                if (this.IsMousePressd)
                {
                    return this.BgInfo.PressedBg;
                }
                else if (this.IsMouseEnter && !this.IsMousePressd)
                {
                    Console.WriteLine("进入...");
                    return this.BgInfo.EnterBg;
                }
                else
                {
                    return this.BgInfo.LeaveBg;
                }
            }
        }
         
        /// <summary>
        /// 当前背景图
        /// </summary>
        protected virtual Image CurrentArrow
        {
            get
            {
                return this.IsGotFocus || this.IsMouseEnter || this.IsMousePressd ? this.ArrowInfo.HightLightArrow : this.ArrowInfo.NormalArrow;
            }
        }

        /// <summary>
        /// 是否允许控件重绘
        /// </summary>
        protected bool Paintable
        {
            get{
                return true;
                //return this.IsMouseEnter || this.IsGotFocus || (this.IsMousePressd && this.MouseButtons == System.Windows.Forms.MouseButtons.Left);
            }
        }

        #endregion

        #region 配置方案
         
        /// <summary>
        /// 边框配色方案
        /// </summary>
        protected CtrlBorderColorInfo BorderColorInfo { private set; get; }

        /// <summary>
        /// 背景配图方案
        /// </summary>
        protected CtrlBgInfo BgInfo { private set; get; }

        /// <summary>
        /// 背景配色方案
        /// </summary>
        protected CtrlBgColorInfo BgColorInfo { private set; get; }

        /// <summary>
        /// 指示图标方案
        /// </summary>
        protected CtrlArrowInfo ArrowInfo { private set; get; }

        #endregion

        #region 行 为
        /// <summary>
        /// 设置边框方案
        /// </summary>
        /// <param name="BorderColorInfo"></param>
        protected void SetBorderColorSolution(CtrlBorderColorInfo BorderColorInfo)
        {
            this.BorderColorInfo = BorderColorInfo;
        }

        /// <summary>
        /// 设置背景配色方案
        /// </summary>
        /// <param name="BgColorInfo"></param>
        protected void SetBgColorSolution(CtrlBgColorInfo BgColorInfo)
        {
            this.BgColorInfo = BgColorInfo;
        }

        /// <summary>
        /// 设置背景配图方案
        /// </summary>
        /// <param name="BgInfo"></param>
        protected void SetBgSolution(CtrlBgInfo BgInfo)
        {
            this.BgInfo = BgInfo;
        }
        /// <summary>
        /// 设置配图指示图标方案
        /// </summary>
        /// <param name="ArrowInfo"></param>
        protected void SetArrowSolution(CtrlArrowInfo ArrowInfo)
        {
            this.ArrowInfo = ArrowInfo;
        }
        #endregion

        #region 事件重写
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
            this.IsMousePressd = true;
            if (this.Paintable)
            {
                this.Invalidate();
            }
        }
        /// <summary>
        /// 鼠标松开事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.IsMousePressd = false;
            if (this.Paintable)
            {
                this.Invalidate();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.IsMouseEnter = true;
            if (this.Paintable)
            {
                this.Invalidate();
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.IsMouseEnter = false;
            if (this.Paintable)
            {
                this.Invalidate();
            }
        }
        protected override void OnGotFocus(EventArgs e)
        {

            base.OnGotFocus(e);
            this.IsGotFocus = true;
            if (this.Paintable)
            {
                this.Invalidate();
            }
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.IsGotFocus = false;
            if (this.Paintable)
            {
                this.Invalidate();
            }
        }
        /// <summary>
        /// 菜单点击事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }
        /// <summary>
        /// 动作激活
        /// </summary>
        /// <param name="e">参数</param>
        protected virtual void Active(EventArgs e)
        {
        
        }
        #endregion

        private Point absLocation;
        /// <summary>
        /// 控件在顶级容器的绝对位置
        /// </summary>
        public Point AbsLocation
        {

            get
            {
                if (absLocation == Point.Empty)
                {
                    absLocation = GetAbsolutePoint(this);
                }
                return absLocation;
            }
        }
        private Point GetAbsolutePoint(Control target)
        {
            Point point = target.Location;

            if (target.Parent != null && target.Parent.Parent != null)
            {
                Point _point = GetAbsolutePoint(target.Parent);
                point.X = point.X + _point.X;
                point.Y = point.Y + _point.Y;
            }
            return point;
        }

        public new void Select()
        {
            this.IsMousePressd = true;
            this.Invalidate();
            base.Select();
        }
        public void UnSelect()
        {
            this.IsMousePressd = false;
            this.Invalidate();
        }

    }
}
