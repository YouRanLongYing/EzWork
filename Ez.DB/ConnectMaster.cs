using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Core.Interceptor;
using Ez.DBContract;

namespace Ez.DB
{
    /// <summary>
    /// 数据库配置信息管理器
    /// </summary>
    internal class ConnectMaster
    {
        /// <summary>
        /// 产品注册号
        /// </summary>
        public static int Proid { set; get; }
        /// <summary>
        /// 链接字符串集合
        /// </summary>
        private static IList<ConnectionEntity> connections = new List<ConnectionEntity>();

        /// <summary>
        /// 链接字符串集合
        /// </summary>
        private static IDictionary<string, IList<ConnectionEntity>> pluginConnections = new Dictionary<string, IList<ConnectionEntity>>();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        public static void Add(ConnectionEntity entity)
        {
            if (connections.Count(p => entity.Scope.Equals(p.Scope)) == 0)
            {
                connections.Add(entity);
            }
        }
        /// <summary>
        /// 添加插件的链接对象
        /// </summary>
        /// <param name="pluginName">插件名称(不区分大小写)</param>
        /// <param name="entity"></param>
        public static void Add(string pluginName,ConnectionEntity entity)
        {
            if (entity != null)
            {
                pluginName = pluginName.ToLower();
                IList<ConnectionEntity> conns = null;
                if (pluginConnections.ContainsKey(pluginName))
                {
                    conns = pluginConnections[pluginName];
                }
                else
                {
                    conns = new List<ConnectionEntity>();
                    pluginConnections.Add(pluginName, conns);
                }
                conns = conns??new List<ConnectionEntity>();
                if (conns.Count(p => entity.Scope.Equals(p.Scope)) == 0)
                {
                    conns.Add(entity);
                }
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="scope">域</param>
        /// <returns></returns>
        public static ConnectionEntity Get(string scope)
        {
            ConnectionEntity entity = connections.FirstOrDefault(p => scope.Equals(p.Scope));
            if (entity == null)
            {
                Log4NetManager.Output(new ExecuteInfo
                {
                    TargetType = typeof(ConnectMaster),
                    Exception = new Exception("The connection is not exits in current scope! " + scope),
                    LogLevel = LogLevel.Error,
                    MethodName = "ConnectionEntity[" + scope + "]"
                });
            }
            return entity;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="pluginName">插件名</param>
        /// <param name="scope">域</param>
        /// <returns></returns>
        public static ConnectionEntity Get(string pluginName, string scope)
        {
            ConnectionEntity entity = null;
            IList<ConnectionEntity> entities = pluginConnections[pluginName.ToLower()];
            if (entities != null && entities.Count()>0)
            {
                entity = entities.FirstOrDefault(p => scope.Equals(p.Scope));
            }
            if (entity == null)
            {
                Log4NetManager.Output(new ExecuteInfo
                {
                    TargetType = typeof(ConnectMaster),
                    Exception = new Exception("The plugin '" + pluginName + "' connection is not exits in current scope! " + scope),
                    LogLevel = LogLevel.Error,
                    MethodName = "ConnectionEntity[" + scope + "]"
                });
            }
            return entity;
        }

        /// <summary>
        /// 获取连接串集合
        /// </summary>
        /// <returns></returns>
        public static IList<ConnectionEntity> GetConnections()
        {
            return connections;
        }

        /// <summary>
        /// 获取插件连接串集合
        /// </summary>
        /// <param name="pluginName">插件名称(不区分大小写)</param>
        /// <returns></returns>
        public static IList<ConnectionEntity> GetConnections(string pluginName)
        {
            return pluginConnections[pluginName.ToLower()];
        }

        /// <summary>
        /// 检查是否配置了基础链接串
        /// </summary>
        /// <returns></returns>
        public static bool BaseConnectionExits()
        {
            return connections.Count(p => "fw".Equals(p.Scope)) > 0;
        }
    }
}
