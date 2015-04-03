using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Ez.Core;
using System.Xml;
using Ez.Payment.Contract;
using Ez.Payment.Upop;
using Ez.Helper;
namespace Ez.Payment
{
    /// <summary>
    /// 配置处理程序
    /// </summary>
    public class PaymentHandler : IConfigurationSectionHandler
    {
        private static PaymentCollect _payment;
        /// <summary>
        /// 获取配置信息
        /// </summary>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            if (_payment != null) return _payment;
            _payment = new PaymentCollect
            {
                DefaultPayment = null,
                Options = new List<PaymentConfigAdapter>()
            };

            int defaultindex = 0;
            var nodeattr = section.Attributes["default"];//defaultindex
            if (nodeattr != null && string.IsNullOrEmpty(nodeattr.Value))
            {
                defaultindex = nodeattr.Value.ToSafeInt();
            }

            var itemNode = section.SelectNodes("payment");

            foreach (XmlNode node in itemNode)
            {
                if (node.NodeType == XmlNodeType.Comment) continue;
                var attr_payment_type = node.Attributes["paytype"];
                if (attr_payment_type == null)
                {
                    throw new Exception("请检查你的配置文件Payment.config是否配置正确(paytype 属性丢失 0 ：支付宝 1为银联在线)!");
                }
                else
                {
                    PaymentType ptype = (PaymentType)attr_payment_type.Value.ToSafeInt();
                    switch (ptype)
                    {
                        case PaymentType.Alipay: AlipayConfigAnalize(node, ref _payment); break;
                        case PaymentType.Upop: UpopConfigAnalize(node, ref _payment); break;
                    }
                }
            }
            if (_payment.Options.Count() > 0)
            {
                _payment.DefaultPayment = _payment.Options[defaultindex];
            }
            return _payment;
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        public static PaymentCollect Config
        {
            get
            {
                if (_payment == null) _payment = ConfigurationManager.GetSection("payments") as PaymentCollect;
                return _payment;
            }
        }
        /// <summary>
        /// 根据类型获取支付配置信息
        /// </summary>
        /// <param name="paymenttype">支付类型</param>
        /// <returns></returns>
        public static PaymentConfigAdapter GetConfig(PaymentType paymenttype)
        {
            return Config.Options.FirstOrDefault(p => p.PaymentType == paymenttype);
        }

        /// <summary>
        /// Alipay 节点配置信息处理
        /// </summary>
        /// <param name="node">节点实例信息</param>
        /// <param name="_payment">配置信息集合</param>
        public static void AlipayConfigAnalize(XmlNode node, ref PaymentCollect _payment)
        {
            XmlNode node_getway = node.SelectSingleNode("getway");
            XmlNode node_ico = node.SelectSingleNode("ico");
            XmlNode node_des = node.SelectSingleNode("description");
            XmlNode node_partner = node.SelectSingleNode("partner");
            XmlNode node_secret = node.SelectSingleNode("secret");
            XmlNode node_public_secret = node.SelectSingleNode("public_secret");
            XmlNode node_return_url = node.SelectSingleNode("return_url");
            XmlNode node_notify_url = node.SelectSingleNode("notify_url");
            XmlNode node_veryfy_url = node.SelectSingleNode("veryfy_url");

            var attr_name = node.Attributes["name"];
            var attr_sing_type = node.Attributes["sing_type"];
            var attr_input_charset = node.Attributes["input_charset"];
            var attr_seller_email = node_partner.Attributes["seller_email"];
            if (node_getway == null || node_ico == null ||
                node_des == null || node_partner == null ||
                node_secret == null || attr_sing_type == null ||
                attr_input_charset == null || attr_name == null ||
                node_return_url == null || node_notify_url == null || node_veryfy_url == null)
            {
                throw new Exception("请检查你的配置文件Payment.config是否配置正确!");
            }
            string getway = node_getway.InnerText;
            string ico = node_ico.InnerText;
            string description = node_des.InnerText;
            string partner = node_partner.InnerText;
            string secret = node_secret.InnerText;
            string veryfy_url = node_veryfy_url.InnerText;
            string name = attr_name.Value;
            string sing_type = attr_sing_type.Value;
            string input_charset = attr_input_charset.Value;
            string seller_email = attr_seller_email.Value;
            string return_url = node_return_url.InnerText;
            string notify_url = node_notify_url.InnerText;
            _payment.Options.Add(new PaymentConfigAdapter
            {
                PaymentType = PaymentType.Alipay,
                Name = name,
                Partner = partner,
                Secret = secret,
                Seller_email = seller_email,
                Sing_type = sing_type,
                Input_charset = input_charset,
                Ico = ico,
                Description = description,
                GetWay = getway,
                Veryfy_url = veryfy_url,
                Notify_url = notify_url,
                Return_url = return_url,
                PublicSecret = (node_public_secret == null) ? "" : node_public_secret.InnerText
            });
        }
        /// <summary>
        /// 银联在线支付配置信息处理
        /// </summary>
        /// <param name="node">节点实例信息</param>
        /// <param name="_payment">配置信息集合</param>
        public static void UpopConfigAnalize(XmlNode node, ref PaymentCollect _payment)
        {

            var attr_configSource = node.Attributes["configSource"];
            if (attr_configSource != null && !string.IsNullOrEmpty(attr_configSource.Value))
            {
                if (UPOPSrv.LoadConf(Tools.GetMapPath("/Config/" + attr_configSource.Value)))
                {
                    XmlNode node_ico = node.SelectSingleNode("ico");
                    XmlNode node_des = node.SelectSingleNode("description");
                    if (node_ico == null || node_des == null)
                    {
                        throw new Exception("银联在线的配置文件配置不正确,请检查(ico,description不允许为空)！");
                    }
                    else
                    {
                        PaymentConfigAdapter adapter = new PaymentConfigAdapter
                        {
                            PaymentType = PaymentType.Upop,
                            Name = node_des.InnerText,
                            Description = node_des.InnerText,
                            Ico = node_ico.InnerText,
                            Partner = UPOPSrv.Config.payParamsPredef["merId"],
                            Secret = UPOPSrv.Config.securityKey,
                            Sing_type = UPOPSrv.Config.signMethod,
                            Input_charset = UPOPSrv.Config.payParamsPredef["charset"],
                            GetWay = UPOPSrv.Config.frontPayURL,
                            Notify_url = UPOPSrv.Config.notify_url,
                            Return_url = UPOPSrv.Config.return_url,
                            transType = UPOPSrv.TransType.CONSUME,
                            orderCurrency = UPOPSrv.CURRENCY_CNY
                        };
                        adapter.SetSpecialConfig(UPOPSrv.Config);
                        _payment.Options.Add(adapter);
                    }
                }
                else
                {
                    throw new Exception("银联在线的配置文件配置不正确,请检查！");
                }
            }
            else
            {
                throw new Exception("银联在线的配置文件配置不正确,请检查！");
            }
        }
    }
}
