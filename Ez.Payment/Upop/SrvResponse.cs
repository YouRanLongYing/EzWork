using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using StrDict = System.Collections.Generic.Dictionary<string, string>;

namespace Ez.Payment.Upop
{
    /// <summary>
    /// 交易响应
    /// </summary>
    /// <remarks></remarks>
    public class SrvResponse
    {

        public const string RESP_SUCCESS = "00";

        protected StrDict m_Args;

        protected StrDict m_Reserved;

        protected string m_PostStr;
        public SrvResponse(StrDict postData)
        {
            Init(postData);
        }

        public SrvResponse(string postStr)
        {
            m_PostStr = postStr;
            Init(Util.ParseQueryStrWithBranket(postStr));
        }

        protected void Init(StrDict postData)
        {
            m_Args = new StrDict(postData);
            if (m_Args.ContainsKey("cupReserved"))
            {
                string cupReservedStr = m_Args["cupReserved"];
                if (!string.IsNullOrEmpty(cupReservedStr))
                {
                    cupReservedStr = cupReservedStr.Substring(2, cupReservedStr.Length - 2);//edited
                    //去掉{}
                    m_Reserved = Util.NameValueCollection2StrDict(System.Web.HttpUtility.ParseQueryString(cupReservedStr));
                }
            }


            if (!(m_Args.ContainsKey("signature") & m_Args.ContainsKey("signMethod")))
            {
                throw new Exception("param [signature] and [signMethod] not found");
            }

            string signature = m_Args["signature"];
            string signMethod = m_Args["signMethod"];
            m_Args.Remove("signature");
            m_Args.Remove("signMethod");


            if (signature != UPOPSrv.Sign(m_Args, signMethod))
            {
                throw new Exception("sign failed");
            }

            m_Args = Util.DictMerge(m_Args, m_Reserved);
            m_Args.Remove("cupReserved");
        }


        #region "Property"

        public bool HasField(string key)
        {
            return m_Args.ContainsKey(key);
        }

        public string Field(string key)
        {
            return m_Args[key];
        }
        public StrDict Fields
        {
            get { return m_Args; }
        }

        public string OrigPostString
        {
            get { return m_PostStr; }
        }

        public string ResponseCode
        {
            get { return m_Args["respCode"]; }
        }


        #endregion

    }
}