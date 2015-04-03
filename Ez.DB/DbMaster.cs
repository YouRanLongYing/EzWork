using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.DBContract;
using System.Configuration;

namespace Ez.DB
{
    /// <summary>
    /// 数据库持久层对象管理器
    /// </summary>
    public class DbMaster : IDbMaster
    {
        static IDictionary<string, IDefaultDao> DbDic=null;
        public string PluginName { set; get; }
        /// <summary>
        /// 实例化宿主的数据库持久层对象管理器
        /// </summary>
        public DbMaster():this(null)
        {
        }
        /// <summary>
        /// 实例化插件的数据库持久层对象管理器
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        public DbMaster(string pluginName)
        {
            this.PluginName = pluginName;
            if (DbDic == null)
            {
                DbDic = new Dictionary<string, IDefaultDao>();
                ConfigurationManager.GetSection("database");
                IList<ConnectionEntity> connections = null;
                if (string.IsNullOrEmpty(pluginName))
                {
                    connections = ConnectMaster.GetConnections();
                }
                else
                {
                    connections = ConnectMaster.GetConnections(this.PluginName);
                }
                foreach (ConnectionEntity item in connections)
                {
                    if (!DbDic.ContainsKey(item.Scope))
                    {
                        DbDic.Add(item.Scope, new DefaultDao(item.Scope, pluginName));
                    }
                }
            }
        }

        /// <summary>
        /// 获取宿主的数据库管理器
        /// </summary>
        /// <param name="scope">指定索引</param>
        /// <returns></returns>
        public IDefaultDao Get(string scope)
        {
            if (DbDic.ContainsKey(scope))
            {
                return DbDic[scope];
            }
            else
            {
                throw new Exception("不存在" + scope + "对应的数据库操作实例");
            }
        }


        public IDefaultDao FistOrDefault()
        {
            ConnectionEntity connection = null;
            if (!string.IsNullOrEmpty(this.PluginName))
            {
                connection = ConnectMaster.GetConnections(this.PluginName).FirstOrDefault();
            }
            else
            {
                connection = ConnectMaster.GetConnections().FirstOrDefault();
            }
            if (connection != null)
                return DbDic[connection.Scope];
            else
                throw new Exception("默认数据库操作实例不存在！");
        }

        public IDefaultDao this[string scope]
        {
            get
            {
                return Get(scope);
            }
        }

        /// <summary>
        /// 产品注册号
        /// </summary>
        public int Proid
        {
            get { return ConnectMaster.Proid; }
        }
    }
}
