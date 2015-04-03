using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Ez.Helper
{
    public enum PowerMode
    {
        OodEven,
        Default
    }
    public class CustomMD5
    {
        /// <summary>
        /// 执行MD5加密
        /// </summary>
        /// <param name="powerString">需要加密的字符串</param>
        /// <param name="powerType">加密方式</param>
        /// <param name="loop">加密次数</param>
        /// <param name="len">获取签名长度</param>
        /// <param name="start">获取签名开始位置(此参数在随即迷失下可缺省其他模式可根据情况赋值)</param>
        public static string Powered(string powerString, PowerMode powerMode, int loop, int len, int start = 0)
        {
            string result = "";
            if (string.IsNullOrEmpty(powerString)) return "";
            #region 处理可能导致异常的操作
            if (powerString.Length - 2 < start)
            {
                start = 0;
            }
            if ((len + start) > powerString.Length)
            {
                len = powerString.Length - start;
            }
            #endregion
            string startStr = "";
            string spitStr = "";
            string endStr = "";
            string[] partString = null;

            bool Initformat = false;
            switch (powerMode)
            {

                case PowerMode.OodEven:
                    {
                        #region 奇偶方式
                        startStr = powerString.Substring(0, start);
                        spitStr = powerString.Substring(start, len);
                        endStr = powerString.Substring(start + len - 1);
                        partString = new string[] { startStr, spitStr, endStr };
                        for (int i = 0; i < loop; i++)
                        {
                            if (i % 2 == 0)
                            {
                                if (!Initformat)
                                {
                                    result = partString[2] + partString[0] + partString[1];
                                    Initformat = true;
                                }
                                result = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(result, "MD5").ToLower();
                            }
                            else
                            {
                                if (!Initformat)
                                {
                                    result = partString[1] + partString[0] + partString[2];
                                    Initformat = true;
                                }
                                result = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(result, "MD5").ToLower();
                            }
                        }
                        #endregion
                    }; break;
                default:
                    {
                        for (int i = 0; i < loop; i++)
                        {
                            if (!Initformat)
                            {
                                result = powerString;
                                Initformat = true;
                            }
                            result = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(result, "MD5").ToLower();
                        }
                    }; break;
            }

            return result;
        }

        /// <summary>
        ///执行MD5加密,不同MD5加密方式
        /// </summary>
        /// <param name="powerString">需要加密的字符串</param>
        public static string Powered(string powerString)
        {
            return Powered(powerString, PowerMode.Default, 1, powerString.Length - 1,0);
        }
    }
}
