using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using StrDict = System.Collections.Generic.Dictionary<string, string>;
namespace Ez.Payment.Upop
{
    /// <summary>
    /// 前台交易服务
    /// </summary>
    /// <remarks></remarks>
    public class FrontPaySrv : UPOPSrv
    {
        private static readonly string[] ms_supportedTrade = { TransType.CONSUME, TransType.PRE_AUTH };

        protected static bool IsTradeSupport(string tradeType)
        {
            return Array.BinarySearch<string>(ms_supportedTrade, tradeType) >= 0;
        }

        public FrontPaySrv(StrDict args)
            : base(args)
        {
            string transTypeVal = args["transType"];
            if (!IsTradeSupport(transTypeVal))
            {
                throw new Exception("front pay cannot support this trans_type");
            }

            m_API_URL = Config.frontPayURL;
            Init(args);

        }

        public string CreateHtml()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>").AppendLine("<head>");
            html.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", m_Args["charset"]).AppendLine();
            html.AppendLine("</head>");
            html.AppendLine("<body onload=\"javascript:document.getElementById('pay_form').submit();\">");
            html.AppendFormat("<form id=\"pay_form\" name=\"pay_form\" action=\"{0}\" method=\"POST\">", m_API_URL).AppendLine();
            foreach (string k in m_Args.Keys)
            {
                html.AppendFormat("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", k, m_Args[k]).AppendLine();
            }
            html.AppendLine("<input type=\"submit\" style=\"display:none;\" />");
            html.AppendLine("</form>").AppendLine("</body>").AppendLine("</html>");

            return html.ToString();
        }

        public Encoding Charset
        {
            get
            {
                Encoding e = Util.GetArgsEncoding(m_Args);
                return e;
            }
        }
    }
}