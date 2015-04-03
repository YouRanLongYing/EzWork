using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Memcached.ClientLibrary;

namespace Ez.Cache
{
    public class Service
    {
        public string Address { set; get; }
        public string Port { set; get; }
        public string Key { set; get; }
        /// <summary>
        /// 权重
        /// </summary>
        public int weight { set; get; }
        public override string ToString()
        {
            return string.Format("{0}:{1}", Address, Port);
        }
    }
    public class MemcachedCfg
    {
        /// <summary>
        /// 缓存开启或关闭
        /// </summary>
        public bool opencache { set; get; }
        /// <summary>
        /// 缓存服务器
        /// </summary>
        public IList<Service> Services { set; get; }
        /// <summary>
        /// 初始连接数
        /// </summary>
        public int initconns { set; get; }
        /// <summary>
        /// 最小连接数
        /// </summary>
        public int minconns { set; get; }
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int maxconns { set; get; }
        /// <summary>
        /// 通讯的超时时间,设置为3秒（单位ms）.NET版本没有实现
        /// </summary>
        public int socket_timeout { set; get; }
         /// <summary>
        /// 连接超时时间
        /// </summary>
        public int connect_timeout { set; get; }
        /// <summary>
        /// 维护线程的间隔激活时间，下面设置为60秒（单位s），设置为0表示不启用维护线程
        /// </summary>
        public int maintenance_sleep { set; get; }
        /// <summary>
        /// 故障切换
        /// </summary>
        public bool failover { set; get; }
        /// <summary>
        /// ?
        /// </summary>
        public bool nagle { set; get; }

        /// <summary>
        /// 启用压缩
        /// </summary>
        public bool enable_compression { set; get; }
        /// <summary>
        /// Native,OldCompatibleHash,NewCompatibleHash
        /// </summary>
        public HashingAlgorithm hashing_algorithm { set; get; }
        /// <summary>
        /// 连接的最大空闲时间，下面设置为6个小时（单位ms），超过这个设置时间，连接会被释放掉
        /// </summary>
        public int maxidle { set; get; }
        /// <summary>
        /// socket单次任务的最大时间，超过这个时间socket会被强行中断掉（当前任务失败）
        /// </summary>
        public int max_busy { set; get; }

    }
}
