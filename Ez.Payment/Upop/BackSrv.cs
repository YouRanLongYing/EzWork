using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using StrDict = System.Collections.Generic.Dictionary<string, string>;
using System.Net;
namespace Ez.Payment.Upop
{
    /// <summary>
    /// 后台服务
    /// </summary>
    /// <remarks></remarks>
    public abstract class BackSrv : UPOPSrv
    {

        public BackSrv(StrDict args)
            : base(args)
        {
        }

        public string Post()
        {
            string reqStr = null;

            System.Text.Encoding encoding = Util.GetArgsEncoding(m_Args);

            //make req string
            StringBuilder sb = new StringBuilder();
            foreach (string k in m_Args.Keys)
            {
                sb.Append(k + "=" + HttpUtility.UrlEncode(m_Args[k], encoding));
                sb.Append("&");
            }
            reqStr = sb.ToString();

            ServicePointManager.Expect100Continue = Config.PostExpect100Continue;
            ServicePointManager.ServerCertificateValidationCallback = SSLCertPolicy.CurrentPolicy;
            HttpWebRequest req = System.Net.WebRequest.Create(m_API_URL) as HttpWebRequest;



            byte[] reqBuf = System.Text.Encoding.UTF8.GetBytes(reqStr);
            // reqStr is Only ASCII

            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = reqBuf.Length;

            System.IO.Stream reqStream = req.GetRequestStream();
            reqStream.Write(reqBuf, 0, reqBuf.Length);
            reqStream.Flush();
            reqStream.Close();

            System.Net.HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), encoding);
            string respStr = sr.ReadToEnd();
            sr.Close();
            resp.Close();
            return respStr;

        }

        public virtual SrvResponse Request()
        {
            return new SrvResponse(Post());
        }
    }
}