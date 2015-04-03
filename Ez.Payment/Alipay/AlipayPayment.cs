using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Collections.Specialized;
using Ez.Payment.Contract;

namespace Ez.Payment.Alipay
{
    /// <summary>
    /// 类名：Submit
    /// 功能：支付宝各接口请求提交类
    /// 详细：构造支付宝各接口表单HTML文本，获取远程HTTP数据
    /// 版本：3.3
    /// 修改日期：2011-07-05
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考
    /// </summary>
    public partial class AlipayPayment : IPayment
    {
        /// <summary>
        /// 日否记录日志
        /// </summary>
        public bool Recordlog{set;get;}
        /// <summary>
        /// 支付宝模块
        /// </summary>
        /// <param name="recordlog">日否记录日志（默认记录）</param>
        public AlipayPayment(bool recordlog=true)
        {
            this.Recordlog = recordlog;
        }
        /// <summary>
        ///判断该笔订单是否在商户网站中已经做过处理
        ///如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
        ///如果有做过处理，不执行商户的业务程序
        /// </summary>
        public event BizNotifyHandler BizNotify_Event;

        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）
        /// </summary>
        /// <param name="payinfo">付费信息模型</param>
        /// <param name="ispost">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public string BuildRequest(PayInfo payinfo, bool ispost, string strButtonValue="submit")
        {
            //待请求参数数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(GetFormParams(payinfo));

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + GATEWAY_NEW + "_input_charset=" + _input_charset + "' method='" + (ispost ? "POST" : "GET") + "'>");

            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");
            return sbHtml.ToString();
        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <returns>支付宝处理结果</returns>
        public string BuildRequest(PayInfo payinfo)
        {
            Encoding code = Encoding.GetEncoding(_input_charset);

            //待请求参数数组字符串
            string strRequestData = BuildRequestParaToString(GetFormParams(payinfo), code);

            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = code.GetBytes(strRequestData);

            //构造请求地址
            string strUrl = GATEWAY_NEW + "_input_charset=" + _input_charset;

            //请求远程HTTP
            string strResult = "";
            try
            {
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";

                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();

                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();

                //获取服务器返回信息
                StreamReader reader = new StreamReader(myStream, code);
                StringBuilder responseData = new StringBuilder();
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    responseData.Append(line);
                }

                //释放
                myStream.Close();

                strResult = responseData.ToString();
            }
            catch (Exception exp)
            {
                strResult = "报错："+exp.Message;
            }

            return strResult;
        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果，带文件上传功能
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="data">文件数据</param>
        /// <param name="contentType">文件内容类型</param>
        /// <param name="lengthFile">文件长度</param>
        /// <returns>支付宝处理结果</returns>
        public string BuildRequest(PayInfo payinfo, string strMethod, string fileName, byte[] data, string contentType, int lengthFile)
        {

            //待请求参数数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(GetFormParams(payinfo));

            //构造请求地址
            string strUrl = GATEWAY_NEW + "_input_charset=" + _input_charset;

            //设置HttpWebRequest基本信息
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            //设置请求方式：get、post
            request.Method = strMethod;
            //设置boundaryValue
            string boundaryValue = DateTime.Now.Ticks.ToString("x");
            string boundary = "--" + boundaryValue;
            request.ContentType = "\r\nmultipart/form-data; boundary=" + boundaryValue;
            //设置KeepAlive
            request.KeepAlive = true;
            //设置请求数据，拼接成字符串
            StringBuilder sbHtml = new StringBuilder();
            foreach (KeyValuePair<string, string> key in dicPara)
            {
                sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"" + key.Key + "\"\r\n\r\n" + key.Value + "\r\n");
            }
            sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"withhold_file\"; filename=\"");
            sbHtml.Append(fileName);
            sbHtml.Append("\"\r\nContent-Type: " + contentType + "\r\n\r\n");
            string postHeader = sbHtml.ToString();
            //将请求数据字符串类型根据编码格式转换成字节流
            Encoding code = Encoding.GetEncoding(_input_charset);
            byte[] postHeaderBytes = code.GetBytes(postHeader);
            byte[] boundayBytes = Encoding.ASCII.GetBytes("\r\n" + boundary + "--\r\n");
            //设置长度
            long length = postHeaderBytes.Length + lengthFile + boundayBytes.Length;
            request.ContentLength = length;

            //请求远程HTTP
            Stream requestStream = request.GetRequestStream();
            Stream myStream;
            try
            {
                //发送数据请求服务器
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Write(data, 0, lengthFile);
                requestStream.Write(boundayBytes, 0, boundayBytes.Length);
                HttpWebResponse HttpWResp = (HttpWebResponse)request.GetResponse();
                myStream = HttpWResp.GetResponseStream();
            }
            catch (WebException e)
            {
                return e.ToString();
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
            }

            //读取支付宝返回处理结果
            StreamReader reader = new StreamReader(myStream, code);
            StringBuilder responseData = new StringBuilder();

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                responseData.Append(line);
            }
            myStream.Close();
            return responseData.ToString();
        }

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <returns>时间戳字符串</returns>
        public string Query_timestamp()
        {
            string url = GATEWAY_NEW + "service=query_timestamp&partner=" + _partner + "&_input_charset=" + _input_charset;
            string encrypt_key = "";

            XmlTextReader Reader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Reader);

            encrypt_key = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;

            return encrypt_key;
        }

        /// <summary>
        /// 启动页面加载模块
        /// </summary>
        /// <param name="issync">是否为同步通知</param>
        public bool NotifyPageLoadStart(bool issync)
        {
            if (this.BizNotify_Event == null) return false;

            if (issync)
            {
               return SyncNottfy();
            }
            else
            {
                return AsyncNotify();
            }
        }



    }
    public partial class AlipayPayment
    {  
        #region 私有字段
        //支付宝网关地址（新）
        private static string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";
        //商户的私钥
        private static string _key = "";
        //编码格式
        private static string _input_charset = "";
        //签名方式
        private static string _sign_type = "";
        private static string _partner = "";
        #endregion
        /// <summary>
        /// 支付宝各接口请求提交类
        /// </summary>
        /// <param name="key">商户的私钥</param>
        /// <param name="charset">编码格式</param>
        /// <param name="sign_type">签名方式</param>
        static AlipayPayment()
        {
            PaymentConfig pinfo = PaymentHandler.GetConfig(PaymentType.Alipay);
            GATEWAY_NEW = pinfo.GetWay;
            _key = pinfo.Secret;
            _sign_type = pinfo.Sing_type;
            _input_charset = pinfo.Input_charset;
            _partner = pinfo.Partner;
        }

        /// <summary>
        /// 生成请求时的签名
        /// </summary>
        /// <param name="sPara">请求给支付宝的参数数组</param>
        /// <returns>签名结果</returns>
        private static string BuildRequestMysign(Dictionary<string, string> sPara)
        {
            string text = Core.CreateLinkString(sPara);
            string sign_type = AlipayPayment._sign_type;
            string result;
            if (sign_type != null)
            {
                if (sign_type == "MD5")
                {
                    result = MD5Lib.Sign(text, AlipayPayment._key, AlipayPayment._input_charset);
                    return result;
                }
                if (sign_type == "RSA")
                {
                    result = RSAFromPkcs8.Sign(text, AlipayPayment._key, AlipayPayment._input_charset);
                    return result;
                }
            }
            result = "";
            return result;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            //签名结果
            string mysign = "";

            //过滤签名参数数组
            sPara = Core.FilterPara(sParaTemp);

            //获得签名结果
            mysign = BuildRequestMysign(sPara);

            //签名结果与签名方式加入请求提交参数组中
            sPara.Add("sign", mysign);
            sPara.Add("sign_type", _sign_type);

            return sPara;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        private static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara = BuildRequestPara(sParaTemp);

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = Core.CreateLinkStringUrlencode(sPara, code);

            return strRequestData;
        }
        /// <summary>
        /// 生成表单信息
        /// </summary>
        /// <param name="payinfo">付费信息模型</param>
        private SortedDictionary<string, string> GetFormParams(PayInfo payinfo)
        {
            PaymentConfig pinfo = PaymentHandler.GetConfig(PaymentType.Alipay);
            ////把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", pinfo.Partner);
            sParaTemp.Add("_input_charset", pinfo.Input_charset.ToLower());
            sParaTemp.Add("service", payinfo.Service);
            sParaTemp.Add("payment_type", payinfo.Payment_type);
            sParaTemp.Add("notify_url", pinfo.Notify_url);
            sParaTemp.Add("return_url", pinfo.Return_url);
            sParaTemp.Add("seller_email", pinfo.Seller_email);
            sParaTemp.Add("out_trade_no", payinfo.Out_trade_no);
            sParaTemp.Add("subject", payinfo.Subject);
            sParaTemp.Add("total_fee", payinfo.Total_fee);
            sParaTemp.Add("body", payinfo.Body);
            sParaTemp.Add("show_url", payinfo.Show_url);
            sParaTemp.Add("anti_phishing_key", payinfo.Anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", payinfo.Exter_invoke_ip);
            return sParaTemp;
        }

        /// <summary>
        /// 获取支付宝过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <param name="post">true获取post值 false 获取Get值</param>
        /// <returns>request回来的信息组成的数组</returns>
        private SortedDictionary<string, string> GetRequestParameters(bool post)
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = post ? HttpContext.Current.Request.Form : HttpContext.Current.Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], coll[requestItem[i]]);
            }

            return sArray;
        }

        private bool AsyncNotify()
        {
            bool processok = false;
            HttpRequest Request = HttpContext.Current.Request;
            HttpResponse Response = HttpContext.Current.Response;
            SortedDictionary<string, string> sPara = GetRequestParameters(true);

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);
                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码
                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表
                    //商户订单号
                    string out_trade_no = Request.Form["out_trade_no"];

                    //支付宝交易号
                    string trade_no = Request.Form["trade_no"];

                    //交易状态
                    string trade_status = Request.Form["trade_status"];
                    processok = this.BizNotify_Event(out_trade_no,trade_no, false, trade_status);
                }
                Response.Write(processok ? "success" : "fail"); 
            }
            else
            {
                Response.Write("无通知参数");
            }
            return processok;
        }

        private bool SyncNottfy()
        {
            bool processok = false;
            HttpRequest Request = HttpContext.Current.Request;
            HttpResponse Response = HttpContext.Current.Response;
            SortedDictionary<string, string> sPara = GetRequestParameters(false);
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码
                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                    //商户订单号

                    string out_trade_no = Request.QueryString["out_trade_no"];

                    //支付宝交易号

                    string trade_no = Request.QueryString["trade_no"];

                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];

                    processok = this.BizNotify_Event(out_trade_no, trade_no, false, trade_status);

                }
            }
            return processok;
        }
    }
}