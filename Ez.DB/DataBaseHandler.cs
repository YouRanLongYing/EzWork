using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using Ez.DBContract;
using Ez.Core;
namespace Ez.DB
{

    /// <summary>
    /// 数据库配置节点处理程序
    /// </summary>
    public class DataBaseHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// 创建配置信息对象
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            XmlAttribute attr_proid = section.Attributes["proid"];
            if (ConnectMaster.Proid <= 0 && attr_proid != null)
            {
                ConnectMaster.Proid = attr_proid.Value.ToSafeInt();
            }
            if (ConnectMaster.Proid <= 0) throw new Exception("请检查根节点是否配置了说明产品注册号的属性如:proid=2");
            //读入宿主站链接串
            ReadConfig("host/connection", section);
            var nodes = section.SelectNodes("plugins/plugin");
            bool isPlugin = nodes.Count>0;
            if (isPlugin)
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.NodeType == XmlNodeType.Comment) continue;
                    var pluginName = node.Attributes["name"];
                    if (pluginName != null && !string.IsNullOrEmpty(pluginName.Value))
                    {
                        ReadConfig("connection", node, pluginName.Value);
                    }
                }
            }
            if (!isPlugin&&!ConnectMaster.BaseConnectionExits())
            { 
               throw new Exception("请配置用户中(框架)的数据库链接串,要求scope='fw'");
            }
            return ConnectMaster.GetConnections();
        }


        private void ReadConfig(string nodepath, System.Xml.XmlNode section,string pluginname=null)
        {
            bool isplugin = !string.IsNullOrEmpty(pluginname);
 
            var itemNode = section.SelectNodes(nodepath);
            foreach (XmlNode node in itemNode)
            {
                if (node.NodeType == XmlNodeType.Comment) continue;
                var scope = node.Attributes["scope"];
                var type = node.Attributes["type"];
                string connection = node.InnerText;

                if (scope == null || string.IsNullOrEmpty(connection) || type == null && (scope != null && string.IsNullOrEmpty(scope.Value)) || (type != null && string.IsNullOrEmpty(type.Value)))
                {
                    throw new Exception("请检查你的数据库配置文件是否配置正确!");
                }
                else
                {
                    ConnectionEntity entity = new ConnectionEntity(scope.Value.ToLower(), (DBTypeEnum)Enum.Parse(typeof(DBTypeEnum), type.Value, true), connection);
                    if (isplugin)
                    {
                         ConnectMaster.Add(pluginname,entity);
                    }
                    else
                    {
                        ConnectMaster.Add(entity);
                    }
                }
            }
        }
    }
}
