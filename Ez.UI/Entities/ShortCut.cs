using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ez.UI.Entities
{
    /// <summary>
    /// 用于客户端Xdialog的尺寸模式
    /// </summary>
    public enum WinSizeEnum
    {
        /// <summary>
        /// 窗口最大化
        /// </summary>
        Max=0,
        /// <summary>
        /// 窗口最小化
        /// </summary>
        Min=1,
        /// <summary>
        /// 默认大小，设置此值后 需要指定窗口的尺寸
        /// </summary>
        Normal=2
    }
    /// <summary>
    /// 快捷按钮模型
    /// </summary>
    public class ShortCut:Menu
    {
        /// <summary>
        /// 菜单快捷按钮的DomElement元素在容器的Top值
        /// </summary>
        public double Top { set; get; }
        /// <summary>
        /// 菜单快捷按钮的DomElement元素在容器的Left值
        /// </summary>
        public double Left { set; get; }
    }
    /// <summary>
    /// 快捷按钮集合
    /// </summary>
    public class ShortCutCollection : IEnumerable
    {
        /// <summary>
        /// 起始快捷按钮的DomElement元素在容器的Top值,用于定义其他按钮的布局
        /// </summary>
        public double StartTop { private set; get; }
        /// <summary>
        /// 起始快捷按钮的DomElement元素在容器的Left值,用于定义其他按钮的布局
        /// </summary>
        public double StartLeft { private set; get; }
        /// <summary>
        /// 快捷按钮集合
        /// </summary>
        /// <param name="startop">起始快捷按钮的DomElement元素在容器的Top值,用于定义其他按钮的布局</param>
        /// <param name="startleft">起始快捷按钮的DomElement元素在容器的Left值,用于定义其他按钮的布局</param>
        public ShortCutCollection(double startop, double startleft)
        {
            this.StartLeft = startleft;
            this.StartTop = startop;
        }
        private IList<ShortCut> shortcuts = new List<ShortCut>();
        public IEnumerator GetEnumerator()
        {
            foreach (ShortCut item in shortcuts)
            {
                yield return item;
            }
        }
        /// <summary>
        /// 添加一个快捷按钮
        /// </summary>
        /// <param name="shortcut"></param>
        public void Add(ShortCut shortcut)
        {
            this.shortcuts.Add(shortcut);
        }
        /// <summary>
        /// 通过索引获取一个快捷按钮
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ShortCut this[int index]
        {
            get { return shortcuts[index]; }
        }
        /// <summary>
        /// 快捷按钮的Json格式数据
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.shortcuts.Count() > 0)
            {
                StringBuilder jsonstring = new StringBuilder();
                jsonstring.Append("{\"startLeft\":" + this.StartLeft + ",\"startTop\":" + this.StartTop + ",\"shortCuts\":");
                IList<string> jsonItem = new List<string>();
                foreach (ShortCut item in this)
                {
                    jsonItem.Add("{" + string.Format("\"name\":\"{0}\",\"ico\":\"{1}\",\"url\":\"{2}\"", item.Name, item.Ico, item.Url) + "}");
                }
                jsonstring.Append("[" + string.Join(",", jsonItem.ToArray()) + "]}");
                return jsonstring.ToString();
            }
            else
            {
                return "{}";
            }
        }
    }
}
