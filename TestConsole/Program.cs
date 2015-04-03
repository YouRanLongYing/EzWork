using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Ez.Cache;
using Ez.BizContract;

namespace TestConsole
{
    [Serializable]
    class Person
    {
        public string name = "andy";
        public string sex = "man";
    }
    class Program
    { 
        
        static void Main(string[] args)
        {
            //new NetTcpBinding();
            using (ChannelFactory<IAccountBiz> channelFactory = new ChannelFactory<IAccountBiz>(new WSHttpBinding(), "http://127.0.0.1:8802/AccountBizService"))
            {
                IAccountBiz proxy = channelFactory.CreateChannel();
                using (proxy as IDisposable)
                {

                    var f = proxy.GetUserInfo(10);
                  Console.WriteLine(f);
                  Console.Read();
                }
            }



            return;

            Console.WriteLine("执行命令->Flush:清除所有缓冲区,Clear:清除缓存,Test:测试,Get:获取缓存\n,SetSvr:设置指定服务器缓存,GetSvr:获取指定服务器缓存");
            string input = "";
            while (true) {
                Console.Write("请输入命令:");
                input = Console.ReadLine();
                switch (input)
                { 
                    case "Flush":
                        FlushAll();
                    break;
                    case "Clear":
                        Clear();
                    break;
                    case "SClear":
                        ServiceClear();
                    break;
                    case "Test":
                        Test();
                    break;
                    case "Get":
                        Test_Get();
                    break;
                    case "SetSvr":
                        Test_Set_Svr();
                    break;
                    case "GetSvr":
                        Test_Get_Svr();
                    break;
                    case "GetSvr2":
                        Test_Get_Svr2();
                    break;
                }
            }
        }

        static void FlushAll()
        {
            MemcachedProxy.Instance.FlushAll();
            Console.WriteLine("缓冲区清除成功！");
        }

        static void Clear()
        {
            Console.WriteLine("请输入缓存数据的Key:");
            string key = Console.ReadLine();
            if (string.IsNullOrEmpty(key)) return;
            MemcachedProxy.Instance.Clear(key);
            Console.WriteLine("清楚成功！");
        }

        static void Test()
        {

            DateTime time = DateTime.Now;
            DateTime outtime = DateTime.Now.AddMinutes(1);
            Console.WriteLine(string.Format("测试写入一个缓存字符串，键为key1>>>", time, outtime));
            MemcachedProxy.Instance.Set("key1", DateTime.Now.ToString(), outtime);
            Console.WriteLine(string.Format("成功向缓存写入数据:{0},过期时间为:{1}", time, outtime));


            Console.WriteLine(string.Format("测试写入一个对象，键位key2>>>", time, outtime));
            Person p = new Person();
            MemcachedProxy.Instance.Set("key2", p, outtime);
            Console.WriteLine(string.Format("成功向缓存写入类实例person.name={0}&&person.sex={1},过期时间为:{2}", p.name, p.sex, outtime));


            Console.WriteLine(string.Format("测试一次写入多个数据键为key3,key4>>>", time, outtime));
            MemcachedProxy.Instance.Set("key3", "endfalse0", outtime);
            MemcachedProxy.Instance.Set("key4", "endfalse1", outtime);
            System.Collections.Hashtable htb = MemcachedProxy.Instance.GetMultiple("key3", "kye4");
            Console.WriteLine("成功一次写入多个数据:"+htb["kye3"] + "," + htb["key4"]);


            Console.WriteLine("将数据缓存到指定的服务器上,选择v4服务器,键为xkey>>>");
            MemcachedProxy instance = MemcachedProxy.Instance.GetProxy("v4");
            instance.Set("xkey", "这是指定到v4缓存服务器上的数据", outtime);

        }

        static void Test_Get()
        {
            string time = DateTime.Now.ToString();
            Console.WriteLine("请输入缓存数据的Key:");
            string key =   Console.ReadLine();
            if (string.IsNullOrEmpty(key)) return;
            object obj = MemcachedProxy.Instance.Get(key);
            if (obj != null)
            {
                TypeCode tcode =  Type.GetTypeCode(obj.GetType());
                if (tcode == TypeCode.String)
                {
                    Console.WriteLine("未过期:"+obj.ToString());
                }
                else
                {
                    Person p = obj as Person;
                    Console.WriteLine(string.Format("未过期:成功获取到类实例person.name={0}&&person.sex={1}", p.name, p.sex));
                }
            }
            else 
            {
                Console.WriteLine("数据已经失效！");
            }
        }

        static void Test_Set_Svr()
        {
            DateTime outtime = DateTime.Now.AddMinutes(3);

            Console.WriteLine("请选择一个服务器( v1,v2,v3,v4):");
            string s = Console.ReadLine();

            MemcachedProxy instance = MemcachedProxy.Instance.GetProxy(s);
            if (instance == null)
            {
                Console.WriteLine("服务器不存在！");
            }
            else {
                Console.WriteLine("请输入数据的key");
                string k = Console.ReadLine();
                Console.WriteLine("请输入数据的值");
                string v = Console.ReadLine();
                instance.Set(k,v, outtime);
            }
            
        }

        static void Test_Get_Svr()
        {
            DateTime outtime = DateTime.Now.AddMinutes(1);

            Console.WriteLine("请选择一个服务器( v1,v2,v3,v4):");
            string s = Console.ReadLine();

            MemcachedProxy instance = MemcachedProxy.Instance.GetProxy(s);
            if (instance == null)
            {
                Console.WriteLine("服务器不存在！");
            }
            else
            {
                Console.WriteLine("请输入数据的key");
                string k = Console.ReadLine();
               
                object v=  instance.Get(k);
                if (v == null)
                {
                    Console.WriteLine("在缓存服务器" + s + "上不存在任何缓存数据");
                }
                else
                {
                    Console.WriteLine("在缓存服务器" + s + "上的缓存数据为:" + v);
                }
            }

        }

        static void Test_Get_Svr2()
        {
           
            Console.WriteLine("请输入数据的key");
            object obj = Console.Read();
            if (obj != null )
            {
                int svrindex = obj.ToString().GetHashCode() % 4;
                MemcachedProxy instance = MemcachedProxy.Instance.GetProxy("v"+svrindex);
                object v = instance.Get(obj.ToString());
                if (v == null)
                {
                    Console.WriteLine("在缓存服务器v" + svrindex + "上不存在任何缓存数据");
                }
                else
                {
                    Console.WriteLine("缓存数据在服务器v" + svrindex + "上,数据为:" + v);
                }
            }
        }

        static void ServiceClear()
        {
            Console.WriteLine("请选择一个服务器( v1,v2,v3,v4):");
            string s = Console.ReadLine();
            MemcachedProxy instance = MemcachedProxy.Instance.GetProxy(s);
            Console.WriteLine("是否清除" + s + "服务器上的所有缓存，1:是,0:否>>");
            string b = Console.ReadLine();
            if ("1" == b)
            {
                instance.FlushAll();
                Console.WriteLine(s+"服务器上的缓存已被全部清除！");
            }
            else {
                Console.Write("请输入要清除的键：");
                string k = Console.ReadLine();
                instance.FlushAll(new System.Collections.ArrayList() { k });
                Console.WriteLine(s+"."+k+"的缓存清除成功！");
            }
        }
    }
}
