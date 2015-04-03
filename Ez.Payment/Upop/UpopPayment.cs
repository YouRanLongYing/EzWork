using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Payment.Contract;
using System.Web;
using Ez.Helper;

namespace Ez.Payment.Upop
{
    public partial class UpopPayment:IPayment
    {
        /// <summary>
        /// 日否记录日志
        /// </summary>
        public bool Recordlog{get;set;}

        /// <summary>
        /// 在线银联支付
        /// </summary>
        /// <param name="recordlog">日否记录日志（默认记录）</param>
        public UpopPayment(bool recordlog = true)
        {
            this.Recordlog = recordlog;
        }
        /// <summary>
        /// 支付平台通知处理事件
        /// </summary>
        public event BizNotifyHandler BizNotify_Event;

        /// <summary>
        /// HTML
        /// </summary>
        /// <param name="payinfo"></param>
        /// <param name="ispost"></param>
        /// <param name="strButtonValue"></param>
        /// <returns></returns>
        public string BuildRequest(PayInfo payinfo, bool ispost, string strButtonValue = "submit")
        {

            decimal titalfee = 0M;

            decimal.TryParse(payinfo.Total_fee, out titalfee);
            int fee = (int)(titalfee * 100);
            // 使用Dictionary保存参数
            System.Collections.Generic.Dictionary<string, string> param = new System.Collections.Generic.Dictionary<string, string>();
            // 填写参数
            param["transType"] = UPOPSrv.TransType.CONSUME;                         // 交易类型，前台只支持CONSUME 和 PRE_AUTH
            param["commodityUrl"] = payinfo.Show_url;                               // 商品URL
            param["commodityName"] = payinfo.Subject;                               // 商品名称
            param["commodityUnitPrice"] = fee.ToString();                           // 商品单价，分为单位
            param["commodityQuantity"] = "1";                                       // 商品数量
            param["orderNumber"] = payinfo.Out_trade_no;                            // 订单号，必须唯一
            param["orderAmount"] = fee.ToString();                               // 交易金额
            param["orderCurrency"] = UPOPSrv.CURRENCY_CNY;                          // 币种
            param["orderTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");           // 交易时间
            param["customerIp"] = payinfo.Exter_invoke_ip;                          // 用户IP
            param["frontEndUrl"] = configAdapter.Return_url;  // 前台回调URL
            param["backEndUrl"] = configAdapter.Notify_url;   // 后台回调URL（前台请求时可为空）
            FrontPaySrv srv = new FrontPaySrv(param);
            return srv.CreateHtml();
        }

        /// <summary>
        ///  建立请求，以模拟远程HTTP的POST请求方式构造并获取处理结果
        /// </summary>
        /// <param name="payinfo">请求参数数组</param>
        /// <returns>处理结果</returns>
        public string BuildRequest(PayInfo payinfo)
        {

            // 使用Dictionary保存参数
            System.Collections.Generic.Dictionary<string, string> param = new System.Collections.Generic.Dictionary<string, string>();

            // 随机构造一个订单号和订单时间（演示用）
            Random rnd = new Random();
            DateTime orderTime = DateTime.Now;
            string orderID = orderTime.ToString("yyyyMMddHHmmss") + (rnd.Next(900) + 100).ToString().Trim();


            // 填写参数
            param["transType"] = UPOPSrv.TransType.CONSUME;                             // 交易类型
            param["commodityUrl"] = payinfo.Show_url;                                   // 商品URL
            param["commodityName"] = payinfo.Subject;                                   // 商品名称
            param["commodityUnitPrice"] = payinfo.Total_fee;                            // 商品单价，分为单位
            param["commodityQuantity"] = "1";                                           // 商品数量
            param["orderNumber"] = payinfo.Out_trade_no;                                // 订单号，必须唯一
            param["orderAmount"] = payinfo.Total_fee;                                   // 交易金额
            param["orderCurrency"] = UPOPSrv.CURRENCY_CNY;                              // 币种
            param["orderTime"] = orderTime.ToString("yyyyMMddHHmmss");                  // 交易时间
            param["customerIp"] = payinfo.Exter_invoke_ip;                              // 用户IP
            param["frontEndUrl"] = configAdapter.Notify_url;                            // 前台回调URL（后台请求时可为空）
            param["backEndUrl"] = configAdapter.Notify_url;                             // 后台回调URL

            param["transTimeout"] = payinfo.transTimeout;                               // 交易超时时间，毫秒
            param["cardNumber"] = payinfo.cardNumber;                                   // 卡号
            param["cardCvn2"] = payinfo.cardCvn2;                                       // CVN2号
            param["cardExpire"] = payinfo.cardExpire;                                   // 信用卡过期时间


            // 创建后台交易服务对象
            BackSrv srv = new BackPaySrv(param);

            // 请求服务，得到SrvResponse回应对象
            SrvResponse resp = srv.Request();
            HttpResponse Response = HttpContext.Current.Response;


            // 根据回应对象的ResponseCode判断是否请求成功
            //（请求成功但交易不一定处理完成，需用Query接口查询交易状态或等Notify回调通知）
            if (resp.ResponseCode != SrvResponse.RESP_SUCCESS)
            {
                return String.Format("Pay Failed! Error:[{0}] : {1} ", resp.Fields["respCode"], resp.HasField("respMsg") ? resp.Fields["respMsg"] : "");
            }
            else
            {
                return "Pay Success!";
            }
        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取处理结果，带文件上传功能
        /// </summary>
        /// <param name="payinfo">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="data">文件数据</param>
        /// <param name="contentType">文件内容类型</param>
        /// <param name="lengthFile">文件长度</param>
        /// <returns>处理结果</returns>
        public string BuildRequest(PayInfo payinfo, string strMethod, string fileName, byte[] data, string contentType, int lengthFile)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <returns>时间戳字符串</returns>
        public string Query_timestamp()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 初始化页面加载初始化模块
        /// </summary>
        /// <param name="issync">是否为同步请求</param>
        public bool NotifyPageLoadStart(bool issync)
        {
            if (this.BizNotify_Event == null) return false;

            if (issync)
            {
                return SyncNotify();
            }
            else
            {
                HttpContext.Current.Response.StatusCode = AsyncNotify() ? 200 : 403;
                return HttpContext.Current.Response.StatusCode == 200;
            }
            
        }
    }

    public partial class UpopPayment
    {
        #region 私有字段
        private static PaymentConfigAdapter configAdapter = null;
        #endregion

        static UpopPayment()
        {
            configAdapter = PaymentHandler.GetConfig(PaymentType.Upop);
        }
        /// <summary>
        /// 异步请求处理
        /// </summary>
        private bool AsyncNotify()
        {
            bool processok = false;
            HttpRequest Request = HttpContext.Current.Request;
            HttpResponse Response = HttpContext.Current.Response;

            // 使用Post过来的内容构造SrvResponse
            SrvResponse resp = new SrvResponse(Util.NameValueCollection2StrDict(Request.Form));
            if (resp.ResponseCode == SrvResponse.RESP_SUCCESS)
            {
                string orderno = resp.Fields["orderNumber"];

                processok = this.BizNotify_Event(orderno, orderno, false, "TRADE_FINISHED");
                
                StringBuilder msglist = new StringBuilder();
                foreach (string k in resp.Fields.Keys)
                {
                    msglist.Append(k + "\t = " + resp.Fields[k]);
                }
                if (processok)
                {
                    Log(true, "单号[AsyncNotify]:" + orderno + "\r\n商户网站业务处理成功\r\n" + msglist.ToString());
                }
                else
                {
                    Log(false, "单号[AsyncNotify]:" + orderno + "\r\n商户网站业务处理失败\r\n" + msglist.ToString());
                }
            }
            else
            {
                Log(false,"error in parsing response message! code:" + resp.ResponseCode );
            }
            return processok;
        }

        /// <summary>
        /// 同步请求处理
        /// </summary>
        private bool SyncNotify()
        {
            bool processok = false;
            HttpRequest Request = HttpContext.Current.Request;
            HttpResponse Response = HttpContext.Current.Response;

            // 使用Post过来的内容构造SrvResponse
            SrvResponse resp = new SrvResponse(Util.NameValueCollection2StrDict(Request.Form));
            if (resp.ResponseCode == SrvResponse.RESP_SUCCESS)
            {
                string orderno = resp.Fields["orderNumber"];

                processok = this.BizNotify_Event(orderno, orderno, false, "TRADE_FINISHED");

                StringBuilder msglist = new StringBuilder();
                foreach (string k in resp.Fields.Keys)
                {
                    msglist.Append(k + "\t = " + resp.Fields[k]);
                }
                if (processok)
                {
                    Log(true, "单号[SyanNottfy]:" + orderno + "\r\n商户网站业务处理成功\r\n" + msglist.ToString());
                }
                else
                {
                    Log(false, "单号[SyanNottfy]:" + orderno + "\r\n商户网站业务处理失败\r\n" + msglist.ToString());
                }
            }
            else
            {
                Log(false, "error in parsing response message! code:" + resp.ResponseCode);
            }
            return processok;
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="msg">日志内容</param>
        private void Log(bool success,string msg)
        {
            // 收到回应后做后续处理（这里写入文件，仅供演示）
            System.IO.StreamWriter sw = new System.IO.StreamWriter(Tools.GetMapPath("/upop_notify_data.txt"));
            sw.WriteLine(string.Format("[{2}]{0}->{1}", success ? "ok" : "fail", msg,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            sw.Close();
        }
    }
}
