using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using StrDict = System.Collections.Generic.Dictionary<string, string>;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace Ez.Payment.Upop
{
    public abstract class UPOPSrv
    {
        public static class TransType
        {
            /// <Class1.cssummary>
            /// 消费 01
            /// </summary>
            public const string CONSUME = "01";
            /// <summary>
            /// 消费撤销 31
            /// </summary>
            public const string CONSUME_VOID = "31";
            /// <summary>
            /// 预授权 02
            /// </summary>
            public const string PRE_AUTH = "02";
            /// <summary>
            /// 预授权撤销 32
            /// </summary>
            public const string PRE_AUTH_VOID = "32";
            /// <summary>
            /// 预授权完成 03
            /// </summary>
            public const string PRE_AUTH_COMPLETE = "03";
            /// <summary>
            /// 预授权完成撤销 33
            /// </summary>
            public const string PRE_AUTH_VOID_COMPLETE = "33";
            /// <summary>
            /// 退货 04
            /// </summary>
            public const string REFUND = "04";
            /// <summary>
            /// 实名认证 71
            /// </summary>
            public const string REGISTRATION = "71";

        }
        public const string CURRENCY_CNY = "156";

        public static ConfigInf Config;
        /// <summary>
        /// 提供对https认证策略的多种支持
        /// </summary>
        /// <remarks></remarks>
        protected class SSLCertPolicy
        {
            protected static List<X509Certificate> trustCerts = new List<X509Certificate>();
            public static System.Net.Security.RemoteCertificateValidationCallback CurrentPolicy;
            public static void InitFromConfig()
            {
                if (UPOPSrv.Config.SSLCertPolicy.ToUpper() == "IGNORE")
                {
                    SSLCertPolicy.CurrentPolicy = SSLCertPolicy.IgnoreAllValidate;
                }
                else if (UPOPSrv.Config.SSLCertPolicy.ToUpper() == "TRUSTSTORE")
                {
                    if (UPOPSrv.Config.SSLCertStorePath == null || string.IsNullOrEmpty(UPOPSrv.Config.SSLCertPolicy))
                    {
                        throw new Exception("Config: SSLCertPolicy=TrustStore, valid SSLCertStorePath needed!");
                    }
                    SSLCertPolicy.LaodTrustCerts(UPOPSrv.Config.SSLCertStorePath);
                    SSLCertPolicy.CurrentPolicy = SSLCertPolicy.CheckTrustStoreValidate;
                }
                else
                {
                    SSLCertPolicy.CurrentPolicy = null;
                }
            }

            public static void LaodTrustCerts(string trustStorePath)
            {
                if (string.IsNullOrEmpty(trustStorePath.Trim()))
                    return;

                trustCerts.Clear();
                string[] strFiles = System.IO.Directory.GetFiles(trustStorePath);
                foreach (string strFile_loopVariable in strFiles)
                {
                    string strFile = strFile_loopVariable;
                    if (!strFile.EndsWith(".cer"))
                        continue;
                    trustCerts.Add(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(strFile));
                }

            }

            public static bool IgnoreAllValidate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }

            public static bool CheckTrustStoreValidate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                foreach (X509Certificate cert_loopVariable in trustCerts)
                {
                    X509Certificate cert = cert_loopVariable;
                    if (cert.Equals(certificate))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        protected string m_API_URL;

        protected StrDict m_Args;

        public UPOPSrv(StrDict args) { }

        protected void Init(StrDict args)
        {

            this.m_Args = Util.DictMerge(Config.payParamsPredef, args);
            Util.DictInsertEmpty(this.m_Args, Config.payParams, "");

            Debug.Assert(this.m_Args.ContainsKey("commodityUrl"));
            this.m_Args["commodityUrl"] = System.Uri.EscapeUriString(args["commodityUrl"]);

            //merReserverd field:
            List<string> merReservedParamList = new List<string>();

            foreach (string k in Config.merReservedParams)
            {
                if (this.m_Args.ContainsKey(k))
                {
                    string item = k + "=" + m_Args[k];
                    merReservedParamList.Add(item);
                    this.m_Args.Remove(k);
                }

            }

            string merReservedStr = string.Join("&", merReservedParamList.ToArray());
            if (!string.IsNullOrEmpty(merReservedStr))
                merReservedStr = "{" + merReservedStr + "}";
            this.m_Args["merReserved"] = merReservedStr;


            //param check:
            foreach (string k in Config.payParamsNotEmpty)
            {
                if (string.IsNullOrEmpty(m_Args[k]))
                {
                    throw new Exception("key [" + k + "] cannot be empty");
                }
            }

            //signature
            SignMe();
        }

        protected void SignMe()
        {
            m_Args["signature"] = Sign(m_Args, Config.signMethod);
            m_Args["signMethod"] = Config.signMethod;
        }

        static internal string Sign(StrDict args, string Method)
        {

            string signResult = null;
            if (Method.ToUpper() == "MD5")
            {
                StringBuilder signStr = new StringBuilder();
                Encoding enc = Util.GetArgsEncoding(args);

                string[] keys = new string[args.Keys.Count];
                args.Keys.CopyTo(keys, 0);
                Array.Sort(keys, StringComparer.Ordinal);
                foreach (string k in keys)
                {
                    signStr.AppendFormat("{0}={1}&", k, args[k]);
                }
                Console.WriteLine("Sign:" + signStr.ToString());
                signResult = signStr.ToString();
                signResult = Util.MD5Hash(signResult + Util.MD5Hash(Config.securityKey, enc), enc);
            }
            else
            {
                throw new Exception("unsupported signMethod");
            }
            return signResult;

        }

        public static bool LoadConf(System.IO.Stream xmlStream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ConfigInf));
            Config = (ConfigInf)xs.Deserialize(xmlStream);
            SSLCertPolicy.InitFromConfig();
            return true;
        }
        public static bool LoadConf(string confFileName)
        {
            System.IO.FileStream FS = new System.IO.FileStream(confFileName, System.IO.FileMode.Open);
            bool ret = LoadConf(FS);
            FS.Close();
            return ret;
        }

        #region "Propertys"
        public object Param(string key)
        {
            return this.m_Args[key];
        }
        public object Params
        {
            get { return m_Args; }
        }
        public bool HasParam(string key)
        {
            return m_Args.ContainsKey(key);
        }
        #endregion
    }
}