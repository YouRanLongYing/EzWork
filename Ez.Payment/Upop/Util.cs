using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using StrDict = System.Collections.Generic.Dictionary<string, string>;
using System.Xml.Serialization;
using System.Xml.Schema;
namespace Ez.Payment.Upop
{
    public class Util
    {

        /// <summary>
        /// 将NameValueCollection转换为Dictionary(Of String,String)
        /// </summary>
        /// <param name="nvcoll"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static StrDict NameValueCollection2StrDict(System.Collections.Specialized.NameValueCollection nvcoll)
        {
            StrDict dict = new StrDict();
            foreach (string k in nvcoll.AllKeys)
            {
                dict[k] = nvcoll[k];
            }
            return dict;
        }


        /// <summary>
        /// 合并两个Dictionary，如果有重名key，则后一个覆盖前一个的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict1"></param>
        /// <param name="dict2"></param>
        /// <returns>合并后的Dictionary</returns>
        /// <remarks></remarks>
        public static Dictionary<T, T2> DictMerge<T, T2>(Dictionary<T, T2> dict1, Dictionary<T, T2> dict2)
        {
            if (dict1 == null & dict2 == null)
            {
                return null;
            }
            else if (dict1 == null)
            {
                return new Dictionary<T, T2>(dict2);
            }

            Dictionary<T, T2> ret = new Dictionary<T, T2>(dict1);
            if ((dict2 != null))
            {
                foreach (T k in dict2.Keys)
                {
                    ret[k] = dict2[k];
                }
            }
            return ret;
        }


        static internal void DictInsertEmpty<T, T2>(Dictionary<T, T2> dict1, T[] keyList, T2 emptyVal)
        {
            foreach (T k in keyList)
            {
                if (dict1.ContainsKey(k))
                    continue;
                dict1[k] = emptyVal;
            }
        }


        /// <summary>
        /// 计算input字符串的MD5值
        /// </summary>
        /// <param name="input">要计算的字符串</param>
        /// <param name="enc">编码，默认是Encoding.Default</param>
        /// <returns>小写16进制表示的md5值</returns>
        /// <remarks></remarks>
        static internal string MD5Hash(string input, Encoding enc = null)
        {
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            if (enc == null)
                enc = Encoding.Default;

            byte[] data = md5Hasher.ComputeHash(enc.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            int i = 0;
            for (i = 0; i <= data.Length - 1; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();

        }


        static internal Encoding GetArgsEncoding(StrDict args)
        {
            if (!args.ContainsKey("charset"))
            {
                throw new Exception("args does not contain [charset] field!");
            }

            string strCharset = args["charset"].ToUpper();
            switch (strCharset)
            {
                case "UTF8":
                case "UTF-8":
                    return Encoding.UTF8;
                case "UNICODE":
                case "UTF-16":
                    return Encoding.Unicode;
                case "GBK":
                case "CP936":
                    return Encoding.GetEncoding("gb2312");
                case "ASCII":
                    return Encoding.ASCII;
                default:
                    return Encoding.GetEncoding(strCharset);
            }
        }

        /// <summary>
        /// 解析QueryString，将{}内的串作为一个整体来处理，不拆分。
        /// </summary>
        /// <param name="queryStr"></param>
        /// <returns>解析好的key/value对</returns>
        /// <remarks></remarks>
        static internal StrDict ParseQueryStrWithBranket(string queryStr)
        {
            queryStr = "&" + queryStr.TrimStart('&').TrimEnd('&') + "&";
            StrDict dict = new StrDict();
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("&(.*?)=((\\{.*?\\})*(.*?))(?=&)");
            foreach (System.Text.RegularExpressions.Match m in re.Matches(queryStr))
            {
                dict[m.Groups[1].Value] = m.Groups[2].Value;
            }
            return dict;
        }
    }
}