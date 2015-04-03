using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Memcached.ClientLibrary;
using System.Collections;

namespace Ez.Cache
{
    /// <summary>
    /// Memcached 的缓存代理
    /// </summary>
    public class MemcachedProxy : IMemCached
    {

        #region 单例实现
        private static MemcachedProxy instance = null;
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static MemcachedProxy Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MemcachedProxy();
                }

                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 静态构造器，用于初始化系统缓存的配置
        /// </summary>
        static MemcachedProxy()
        {
            ConfigurationManager.GetSection("memcached");
        }

        /// <summary>
        /// 获取缓存数据的对象索引
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return Get(key);
            }
            set {
                Set(key, value);
            }
        }
        private MemcachedClient client;
        /// <summary>
        /// 缓存管理客户端
        /// </summary>
        public MemcachedClient Client
        {
            get {
                if (client == null)
                {
                    client = new MemcachedClient();
                    client.PoolName = "default";
                }
                return client;
            }
        }

        /// <summary>
        /// 实例指定缓存池名称的实例
        /// </summary>
        /// <param name="poolName">p缓存池名称</param>
        public MemcachedProxy(string poolName)
        {
            client = new Memcached.ClientLibrary.MemcachedClient();
            client.PoolName = poolName;
        }

        /// <summary>
        /// 构造器初始化缓存运行参数
        /// </summary>
        private  MemcachedProxy()
        {
            var cfg = MemcachedHandler.Config;
            if (cfg == null)
            {
                throw new Exception("Memcached 未初始化或未正确配置！");
            }
            else if (cfg.Services.Count() > 0)
            {
                SockIOPool pool = SockIOPool.GetInstance("default");
                pool.SetServers(cfg.Services.Select(p => p.ToString()).ToArray());
                pool.InitConnections = cfg.initconns;
                pool.MinConnections = cfg.minconns;
                pool.MaxConnections = cfg.maxconns;
                pool.SocketConnectTimeout = cfg.connect_timeout;
                pool.SocketTimeout = cfg.socket_timeout;
                pool.MaintenanceSleep = cfg.maintenance_sleep;
                pool.Failover = cfg.failover;
                pool.Nagle = cfg.nagle;
                pool.HashingAlgorithm = cfg.hashing_algorithm;
                pool.MaxIdle = cfg.maxidle;
                pool.MaxBusy = cfg.max_busy;
                pool.SetWeights(cfg.Services.Select(p => p.weight).ToArray());
                pool.Initialize();

                IEnumerable<string> keys = cfg.Services.Select(p => p.Key).Distinct();
                foreach (var key in keys)
                {
                    IEnumerable<Service> svrs = cfg.Services.Where(p => key.Equals(p.Key));
                    SockIOPool pool2 = SockIOPool.GetInstance(key);
                    pool2.SetServers((from p in svrs select p.ToString()).ToArray());
                    pool2.InitConnections = cfg.initconns;
                    pool2.MinConnections = cfg.minconns;
                    pool2.MaxConnections = cfg.maxconns;
                    pool2.SocketConnectTimeout = cfg.connect_timeout;
                    pool2.SocketTimeout = cfg.socket_timeout;
                    pool2.MaintenanceSleep = cfg.maintenance_sleep;
                    pool2.Failover = cfg.failover;
                    pool2.Nagle = cfg.nagle;
                    pool2.HashingAlgorithm = cfg.hashing_algorithm;
                    pool2.MaxIdle = cfg.maxidle;
                    pool2.MaxBusy = cfg.max_busy;
                    pool2.SetWeights(svrs.Select(p => p.weight).ToArray());
                    pool2.Initialize();

                }
                Client.EnableCompression = cfg.enable_compression;
            }
            else
            {
                //throw new Exception("不存在缓存服务器地址，请检查相关配置！");
            }
        }

        #region Clear Func
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        public void Clear(string key)
        {
            Client.Delete(key);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="expiry">指定时间作为过期时间来清除缓存</param>
        public void Clear(string key,DateTime expiry)
        {
            Client.Delete(key, expiry);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="hashcode">指定缓存键的哈希值</param>
        /// <param name="expiry">指定时间作为过期时间来清除缓存</param>
        public void Clear(string key, object hashcode, DateTime expiry)
        {
            Client.Delete(key, hashcode, expiry);
        }
        #endregion

        #region Get Func
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <returns>缓存数据</returns>
        public Object Get(string key)
        {
            if (this.KeyExists(key))
            {
                return Client.Get(key);
            }
            else
                return null;
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="hashCode">指定缓存键的哈希值</param>
        /// <returns>缓存数据</returns>
        public Object Get(string key, int hashCode)
        {
            if (this.KeyExists(key))
            {
                return Client.Get(key, hashCode);
            }
            else
                return null;
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="hashCode">指定缓存键的哈希值</param>
        /// <param name="asstring">作为字符串类型返回</param>
        /// <returns>缓存数据</returns>
        public Object Get(string key, int hashCode,bool asstring)
        {
            if (this.KeyExists(key))
            {
                return Client.Get(key, hashCode, asstring);
            }
            else
                return null;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="value">缓存数据的值</param>
        public void Set(string key, object value)
        {
                Client.Set(key,value);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="value">缓存数据的值</param>
        /// <param name="expriy">设置缓存过期的时间</param>
        public void Set(string key, object value, DateTime expriy)
        {
            Client.Set(key, value,expriy);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="value">缓存数据的值</param>
        /// <param name="expriy">设置缓存过期的时间</param>
        /// <param name="hashCode">指定缓存键的哈希值</param>
        public void Set(string key, object value,DateTime expriy, int hashCode)
        {
            Client.Set(key, value, expriy, hashCode);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="value">缓存数据的值</param>
        /// <param name="hashCode">指定缓存键的哈希值</param>
        public void Set(string key, object value, int hashCode)
        {
            Client.Set(key, value, hashCode);
        }
        #endregion

        /// <summary>
        /// 清除所有缓冲区
        /// </summary>
        public void FlushAll()
        {
            Client.FlushAll(null);
        }
        /// <summary>
        /// 清除指定缓存服务的冲区
        /// </summary>
        [Obsolete("内部存在索引出界的bug，暂不允许使用")]
        public void FlushAll(ArrayList servers)
        {
            Client.FlushAll(servers);
        }

        /// <summary>
        /// 检测缓存是否存在
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <returns>true:存在，false:不存在</returns>
        public bool KeyExists(string key,params object[] args)
        {
            return Client.KeyExists(string.Format(key, args));
        }

        /// <summary>
        /// 获取多个指定键的缓存数据
        /// </summary>
        /// <param name="keys">缓存键集合</param>
        /// <returns>键值对应的哈希表</returns>
        public Hashtable GetMultiple(params string[] keys)
        {
            IList<string> keylist = new List<string>();

            foreach (string key in keys)
            {
                if (this.KeyExists(key))
                {
                    keylist.Add(key);
                }
            }
            if (keylist.Count > 0)
            {
                return Client.GetMultiple(keylist.ToArray());
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取运行的信息
        /// </summary>
        /// <returns>键值对应的参数</returns>
        public IDictionary<string, string> GetStatInfo()
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            Hashtable ht = Client.Stats();
            foreach (DictionaryEntry de in ht)
            {
                Hashtable info = (Hashtable)de.Value;
                foreach (DictionaryEntry de2 in info)
                {
                    if (!dic.ContainsKey(de2.Key.ToString()))
                    dic.Add(de2.Key.ToString(), de2.Value.ToString());
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取缓存的代理对象
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns></returns>
        public MemcachedProxy GetProxy(string poolName)
        {
            return new MemcachedProxy(poolName);
        }
    }
}
