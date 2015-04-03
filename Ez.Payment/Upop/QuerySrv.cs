using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StrDict = System.Collections.Generic.Dictionary<string, string>;
namespace Ez.Payment.Upop
{
    /// <summary>
    /// 交易查询服务
    /// </summary>
    /// <remarks></remarks>
    public class QuerySrv : BackSrv
    {

        public const string QUERY_SUCCESS = "0";
        public const string QUERY_FAIL = "1";
        public const string QUERY_WAIT = "2";

        public const string QUERY_INVALID = "3";
        private string[] QueryParamPredefList = { "version", "charset", "merId" };
        public QuerySrv(StrDict args)
            : base(args)
        {
            m_API_URL = Config.queryURL;

            m_Args = args;

            foreach (string k in QueryParamPredefList)
            {
                m_Args[k] = Config.payParamsPredef[k];
            }

            m_Args["merReserved"] = "";
            if (!string.IsNullOrEmpty(Config.payParamsPredef["acqCode"]))
            {
                m_Args["merReserved"] = "{acqCode=" + Config.payParamsPredef["acqCode"] + "}";
            }
            else if (string.IsNullOrEmpty(Config.payParamsPredef["merId"]))
            {
                throw new Exception("config:[merId] and [acqCode] cannot both be empty");
            }

            // Init()??  merReserved may be covered!
            SignMe();
        }

    }
}
