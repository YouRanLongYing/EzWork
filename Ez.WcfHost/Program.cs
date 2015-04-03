using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Ez.Biz;
using Ez.BizContract;

namespace Ez.WcfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(AccountBiz)))
            {
                //
                host.AddServiceEndpoint(typeof(IAccountBiz), new WSHttpBinding(), "http://127.0.0.1:8802/AccountBizService");

                if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = new Uri("http://127.0.0.1:8802/AccountBizService/metadata");
                    host.Description.Behaviors.Add(behavior);
                }
                host.Opened += delegate
                {
                    Console.WriteLine("CalculaorService已经启动，按任意键终止服务！");
                };
                host.Open();
                Console.Read();
            }
        }
    }
}
