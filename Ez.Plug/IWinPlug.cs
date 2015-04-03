using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ez.Plug
{
    public class TransData
    {
        /// <summary>
        /// 当前用户编号
        /// </summary>
        public int CurrentUserID { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string CurrentUserName { set; get; }
    }

    public interface IWinPlug
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        string PlugName { get; }
        /// <summary>
        /// 插件描述
        /// </summary>
        string PlugDesc { get; }
        /// <summary>
        /// 插件启动的入口
        /// </summary>
        Form Instance(TransData data,params string[] flag);
        /// <summary>
        /// 插件版本
        /// </summary>
        string Version { get; }
        /// <summary>
        /// 开发人
        /// </summary>
        string DevPerson { get; }
        /// <summary>
        /// 是否完全填充
        /// </summary>
        bool DockFull { get; }
    }
}
