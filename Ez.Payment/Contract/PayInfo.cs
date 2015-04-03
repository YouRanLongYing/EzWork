using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Payment.Contract
{
    /// <summary>
    /// 付费信息模型
    /// </summary>
    public class PayInfo
    {
        private string service = "create_direct_pay_by_user";
        /// <summary>
        /// 付款信息模型
        /// </summary>
        /// <param name="service">服务（默认及时到账）</param>
        public PayInfo(string service = "create_direct_pay_by_user")
        {
            this.Service = service;
        }
       
        /// <summary>
        /// 接口服务,create_direct_pay_by_user
        /// </summary>
        public string Service { internal set; get; }
        //支付类型
        private string payment_type = "1";
        /// <summary>
        /// 支付类型
        /// </summary>
        public string Payment_type
        {
            get { return payment_type; }
        }
        /// <summary>
        ///商户订单号（必填）
        /// </summary>
        private string out_trade_no = "";
        /// <summary>
        ///商户网站订单系统中唯一订单号，必填
        /// </summary>
        public string Out_trade_no
        {
            get {
                if (string.IsNullOrEmpty(out_trade_no)) throw new Exception("订单号不允许为空！");
                return out_trade_no;
            }
            set { out_trade_no = value; }
        }
        /// <summary>
        /// 订单名称
        /// </summary>
        private string subject = "";
        /// <summary>
        /// 订单名称
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string total_fee = "";
        /// <summary>
        ///必填
        ///付款金额
        /// </summary>
        public string Total_fee
        {
            get {
                if (string.IsNullOrEmpty(total_fee)) throw new Exception("付款金额不允许费空！");
                return total_fee;
            }
            set { total_fee = value; }
        }
        /// <summary>
        ///必填
        ///订单描述
        /// </summary>
        private string body = "";
        /// <summary>
        ///必填
        ///订单描述
        /// </summary>
        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        /// <summary>
        /// 商品展示地址
        /// </summary>
        private string show_url = "";
        /// <summary>
        /// 商品展示地址 需以http://开头的完整路径，例如：http://www.xxx.com/myorder.html
        /// </summary>
        public string Show_url
        {
            get { return show_url; }
            set { show_url = value; }
        }

        private string anti_phishing_key = "";
        /// <summary>     
        ///防钓鱼时间戳 如若要使用请调用类文件AlipayPayment的query_timestamp函数
        /// </summary>
        public string Anti_phishing_key
        {
            get { return anti_phishing_key; }
            set { anti_phishing_key = value; }
        }

        private string exter_invoke_ip = "";
        /// <summary>
        /// 客户端的IP地址,非局域网的外网IP地址，如：221.0.0.1
        /// </summary>
        public string Exter_invoke_ip
        {
            get { return exter_invoke_ip; }
            set { exter_invoke_ip = value; }
        }

        /// <summary>
        /// 交易超时时间（银联在线） 后天交易用
        /// </summary>
        public string transTimeout { set; get; }
        /// <summary>
        /// 支付银行卡卡号（银联在线） 后天交易用
        /// </summary>
        public string cardNumber { set; get; }
        /// <summary>
        /// CVN2号 （银联在线） 后天交易用
        /// </summary>
        public string cardCvn2 { set; get; }
        /// <summary>
        /// 应用卡过期时间（银联在线） 后天交易用
        /// </summary>
        public string cardExpire { set; get; }

    }
}
