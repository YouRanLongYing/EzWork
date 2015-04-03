using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Payment.Contract
{
    /// <summary>
    /// 支付配置模型
    /// </summary>
    public class PaymentConfig
    {
        /// <summary>
        /// 支付类型
        /// </summary>
        public PaymentType PaymentType { set; get; }
        /// <summary>
        /// 请求网关
        /// </summary>
        public string GetWay { internal set; get; }
        /// <summary>
        /// 支付宝消息验证地址
        /// </summary>
        public string Veryfy_url { internal set; get; }
        /// <summary>
        /// 支付名称
        /// </summary>
        public string Name { internal set; get; }
        /// <summary>
        /// 服务器同步通知页面
        /// 页面可在本机电脑测试
        /// 可放入HTML等美化页面的代码、商户业务逻辑程序代码
        /// 该页面可以使用ASP.NET开发工具调试，也可以使用写文本函数LogResult进行调试
        /// </summary>
        public string Return_url { internal set; get; }
        /// <summary>
        /// 服务器异步通知页面
        /// 创建该页面文件时，请留心该页面文件中无任何HTML代码及空格。
        /// 该页面不能在本机电脑测试，请到服务器上做测试。请确保外部可以访问该页面。
        /// 该页面调试工具请使用写文本函数logResult。
        /// 如果没有收到该页面返回的 success 信息，支付宝会在24小时内按一定的时间策略重发通知
        /// </summary>
        public string Notify_url { internal set; get; }
        /// <summary>
        /// 签名类型
        /// </summary>
        private string sing_type = "MD5";
        /// <summary>
        /// 签名类型
        /// </summary>
        public string Sing_type
        {
            get { return sing_type; }
            internal set { sing_type = value; }
        }
        /// <summary>
        /// 编码类型
        /// </summary>
        private string input_charset = "utf-8";
        /// <summary>
        /// 编码类型
        /// </summary>
        public string Input_charset
        {
            get { return input_charset; }
            set { input_charset = value; }
        }
        /// <summary>
        /// 图标地址
        /// </summary>
        public string Ico { internal set; get; }
        /// <summary>
        /// 合作人
        /// </summary>
        public string Partner { internal set; get; }
        /// <summary>
        /// 商家邮件地址
        /// </summary>
        public string Seller_email { internal set; get; }
        /// <summary>
        /// 私钥
        /// </summary>
        public string Secret { internal set; get; }
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicSecret
        {
            get;
            internal set;
        }
        /// <summary>
        /// 支付类型描述
        /// </summary>
        public string Description { internal set; get; }
    }
}
