using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Ez.Helper;
using Ez.Core;

namespace Ez.Config
{
    public class UIConfig
    {
        private static readonly string configpath = Tools.GetMapPath(Constans.SYS_UICONFIG_FILE_PATH);
        private static System.Timers.Timer FileServersTimer = new System.Timers.Timer(60000);
        private static DateTime m_fileoldchange;
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object m_lockHelper = new object();
        private static UIConfigModel model = new UIConfigModel();
        public static UIConfigModel Model
        {
            get { return UIConfig.model; }
        }
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static UIConfig()
        {
            LoadConfig();
            FileServersTimer.AutoReset = true;
            FileServersTimer.Enabled = true;
            FileServersTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            FileServersTimer.Start();
        }
        /// <summary>
        /// 定时重置配置文件
        /// </summary>
        static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReLoadConfig(true);
        }
        static public void ReLoadConfig(bool checkTime)
        {
            if (checkTime)
            {
                lock (m_lockHelper)
                {
                    DateTime m_filenewchange = System.IO.File.GetLastWriteTime(configpath);
                    if (m_fileoldchange != m_filenewchange)
                    {
                        m_fileoldchange = m_filenewchange;
                        LoadConfig();
                    }
                }
            }
            else
            {
                LoadConfig();
            }
        }
        static public void LoadConfig()
        {
            m_fileoldchange = System.IO.File.GetLastWriteTime(configpath);
            XmlDocument xml = new XmlDocument();
            xml.Load(configpath);
            XmlNode xroot = xml.SelectSingleNode("configuration");
            XmlNodeList nodlist = xroot.ChildNodes;
            if (nodlist != null)
            {
                foreach (XmlNode n in nodlist)
                {
                    if (n.NodeType != XmlNodeType.Comment)
                    {

                        switch (n.Name.ToLower())
                        {
                            case "systemname":
                                {
                                    model.SystemName = n.InnerText;
                                }break;
                            case "layoutaction":
                                {
                                    model.LayoutAction = n.InnerText;
                                    XmlAttribute uistyle = n.Attributes["uistyle"];
                                    if (uistyle != null)
                                    {
                                        model.UIStyle = uistyle.Value;
                                    }
                                    XmlAttribute ctrldir = n.Attributes["ctrldir"];
                                    if(ctrldir!=null)
                                    {
                                        model.CtrlDir = ctrldir.Value;
                                    }
                                    XmlAttribute login = n.Attributes["login"];
                                    if (login != null)
                                    {
                                        model.Login = login.Value;
                                    }
                                    XmlAttribute regist = n.Attributes["regist"];
                                    if (regist != null)
                                    {
                                        model.Regist = regist.Value;
                                    }
                                    if (string.IsNullOrEmpty(model.UIStyle)) model.UIStyle = "default";
                                    if (string.IsNullOrEmpty(model.LayoutAction)) model.LayoutAction = "tradition";
                                    if (string.IsNullOrEmpty(model.CtrlDir)) model.LayoutAction = "window";
                                }
                                break;
                            case "pageSize":
                                {
                                    model.PageSize = n.InnerText.ToSafeInt(20, true);
                                }; break;
                            case "language":
                                {
                                    bool throwexp = false;
                                    string suportlanguages = n.InnerText;
                                    string[] language_names = suportlanguages.Replace("，", ",").Split(',');
                                    if (language_names != null && language_names.Length > 0)
                                    {
                                        foreach (string item in language_names)
                                        {
                                            var lang_name = item.Split('|');
                                            throwexp = lang_name.Length != 2 || (lang_name.Length == 2 && string.IsNullOrEmpty(lang_name[0]) && string.IsNullOrEmpty(lang_name[1]));
                                            if (!throwexp)
                                            {
                                                if(!model.AvailableCultures.ContainsKey(lang_name[0]))
                                                model.AvailableCultures.Add(lang_name[0], lang_name[1]); 
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    if (throwexp) throw new Exception("请正确配置UI.Config文件的language节点！");
                                    XmlAttribute attr = n.Attributes["current"];
                                    model.Language = attr != null ? attr.Value : "zh-CN";
                                }; break;
                        }
                        model[n.Name] = n.InnerText;
                    }
                }
            }
        }
    }

    public class UIConfigModel
    {
        private IDictionary<string, string> keyValue = new Dictionary<string, string>();
        /// <summary>
        /// 语言别名列表
        /// </summary>
        private IDictionary<string, string> availableCultures = new Dictionary<string, string>();

        private string language;
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { internal set; get; }
        /// <summary>
        /// 登录页面
        /// </summary>
        public string Login { internal set; get; }
        /// <summary>
        /// 注册页面
        /// </summary>
        public string Regist { internal set; get; }
        /// <summary>
        /// 控制器目录
        /// </summary>
        public string CtrlDir { internal set; get; }
        /// <summary>
        /// 布局页面对应Action也对应需要的样式和脚本文件夹
        /// </summary>
        public string LayoutAction { internal set; get; }
        /// <summary>
        /// UI配色风格
        /// </summary>
        public string UIStyle { internal set; get; }
        /// <summary>
        /// 分页所需页码
        /// </summary>
        public int PageSize { internal set; get; }
        /// <summary>
        /// 语言别名列表
        /// </summary>
        public IDictionary<string, string> AvailableCultures
        {
            internal set{availableCultures = value;}
            get{return availableCultures;}
        }

        /// <summary>
        /// 当前语言的简称
        /// </summary>
        public string Language
        {
            get
            {
                if (!AvailableCultures.Keys.Any(p => p.Equals(this.language, StringComparison.OrdinalIgnoreCase)))
                {
                    var df = this.AvailableCultures.FirstOrDefault();
                    if (df.Key == null)
                        throw new Exception("当前指定的系统语言:“" + this.language + "”不受支持!");
                    else
                        this.language = this.AvailableCultures.FirstOrDefault().Key;
                }
                return language;
            }
            internal set { language = value; }
        }
        /// <summary>
        /// 当前语言的别名
        /// </summary>
        public string LanagueName
        {

            get
            {
                string lname = AvailableCultures[this.language];
                if (string.IsNullOrEmpty(lname)) throw new Exception("请正确配置UI.Config文件的language节点！");
                return lname;
            }
        }
        /// <summary>
        /// 指定节点的配置信息
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public string this[string nodeName]
        {
            get
            {
                return this.keyValue[nodeName];
            }
            internal set
            {
                this.keyValue[nodeName] = value;
            }
        }
    }
}
