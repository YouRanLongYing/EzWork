using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using Memcached.ClientLibrary;

namespace Ez.Cache
{
    /// <summary>
    /// 缓存配置信息的处理对象
    /// </summary>
    public class MemcachedHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public static MemcachedCfg Config { set; get; }
        /// <summary>
        /// 读取配置信息并配置到配置信息中
        /// </summary>
        /// <param name="parent">父级节点</param>
        /// <param name="configContext"></param>
        /// <param name="section">配置节点信息</param>
        /// <returns>配置信息</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            if (Config == null)
            {
                Config = new MemcachedCfg
                {
                    Services = new List<Service>()
                };
            }
            var openNode = section.SelectSingleNode("opencache");
            if (openNode != null && openNode.InnerText!=null)
            {
                Config.opencache = "true".Equals(openNode.InnerText.ToLower());
            }
            var servicesNode = section.SelectNodes("services/add");
            foreach (XmlNode node in servicesNode)
            {
                if (node.NodeType == XmlNodeType.Comment) continue;
                var address = node.Attributes["address"];
                var port = node.Attributes["port"];
                var key = node.Attributes["key"];
                var weight = node.Attributes["weight"];
                if (address == null || port == null || weight==null)
                {
                    throw new Exception("请检查你的数据库配置文件是否配置正确!");
                }
                else
                {
                    int int_weight = 0;
                    int.TryParse(weight.Value, out int_weight);
                    Config.Services.Add(new Service
                    {
                        Address = address.Value,
                        Port = port.Value,
                        Key = key.Value,
                        weight = int_weight
                    });
                }
            }
            var initconnsNode = section.SelectSingleNode("initconns");
            int initconns = 0;
            if (initconnsNode != null && int.TryParse(initconnsNode.InnerText, out initconns) && initconns > 0)
            {
                Config.initconns = initconns;
            }
            else
            {
                throw new Exception("初始连接数不存在！");
            }
            var minconnsNode = section.SelectSingleNode("minconns");
            int minconnsconns = 0;
            if (minconnsNode != null && int.TryParse(minconnsNode.InnerText, out minconnsconns) && minconnsconns > 0)
            {
                Config.minconns = minconnsconns;
            }
            else
            {
                throw new Exception("最小连接数不存在！");
            }
            var maxconnsNode = section.SelectSingleNode("maxconns");
            int maxconnsconns = 0;
            if (maxconnsNode != null && int.TryParse(maxconnsNode.InnerText, out maxconnsconns) && maxconnsconns > 0)
            {
                Config.maxconns = maxconnsconns;
            }
            else
            {
                throw new Exception("最大连接数不存在！");
            }
            var socket_timeoutNode = section.SelectSingleNode("socket_timeout");
            int socket_timeout = 0;
            if (socket_timeoutNode != null && int.TryParse(socket_timeoutNode.InnerText, out socket_timeout) && socket_timeout > 0)
            {
                Config.socket_timeout = socket_timeout;
            }
            else
            {
                throw new Exception("套接字超时时间未设置！");
            }
            var connect_timeoutNode = section.SelectSingleNode("socket_timeout");
            int connect_timeout = 0;
            if (connect_timeoutNode != null && int.TryParse(connect_timeoutNode.InnerText, out connect_timeout) && connect_timeout > 0)
            {
                Config.connect_timeout = connect_timeout;
            }
            else
            {
                throw new Exception("链接超时时间未设置！");
            }
            var maintenance_sleepNode = section.SelectSingleNode("maintenance_sleep");
            int maintenance_sleep = 0;
            if (maintenance_sleepNode != null && int.TryParse(maintenance_sleepNode.InnerText, out maintenance_sleep) && maintenance_sleep > 0)
            {
                Config.maintenance_sleep = maintenance_sleep;
            }
            else
            {
                throw new Exception("维护睡眠时间未设置！");
            }
            var failoverNode = section.SelectSingleNode("failover");
            if (failoverNode != null)
            {
                Config.failover = failoverNode.InnerText.ToLower() == "true";
            }
            var nagleNode = section.SelectSingleNode("nagle");
            if (nagleNode != null)
            {
                Config.nagle = nagleNode.InnerText.ToLower() == "true";
            }
            var enable_compressionNode = section.SelectSingleNode("enable_compression");
            if (enable_compressionNode != null)
            {
                Config.enable_compression = enable_compressionNode.InnerText.ToLower() == "true";
            }

            var hashing_algorithm_sleepNode = section.SelectSingleNode("maintenance_sleep");
            if (maintenance_sleepNode != null)
            {
                if (maintenance_sleepNode.InnerText.ToLower() == "native")
                {
                    Config.hashing_algorithm = HashingAlgorithm.Native;
                }
                else if (maintenance_sleepNode.InnerText.ToLower() == "oldcompatiblehash")
                {
                    Config.hashing_algorithm = HashingAlgorithm.OldCompatibleHash;
                }
                else
                {
                    Config.hashing_algorithm = HashingAlgorithm.NewCompatibleHash;
                }
            }
            else
            {
                throw new Exception("hash算法未设置！");
            }
            var maxidleNode = section.SelectSingleNode("maxidle");
            int maxidleint = 0;
            if (maxidleNode != null && int.TryParse(maxidleNode.InnerText, out maxidleint) && maxidleint > 0)
            {
                Config.maxidle = maxidleint;
            }
            else
            {
                throw new Exception("链接最大空闲时间未设置！");
            }
            var max_busyNode = section.SelectSingleNode("maxidle");
            int max_busy = 0;
            if (max_busyNode != null && int.TryParse(max_busyNode.InnerText, out max_busy) && max_busy > 0)
            {
                Config.max_busy = max_busy;
            }
            else
            {
                throw new Exception("单词任务最大时间未设置！");
            }

            return Config;
        }
    }
}
