using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StrDict = System.Collections.Generic.Dictionary<string, string>;
namespace Ez.Payment.Upop
{
    /// <summary>
    /// 后台交易服务
    /// </summary>
    /// <remarks></remarks>
    public class BackPaySrv : BackSrv
    {

        public BackPaySrv(StrDict args)
            : base(args)
        {
            string transTypeVal = args["transType"];
            if (transTypeVal == TransType.CONSUME | transTypeVal == TransType.PRE_AUTH)
            {
                if (!(args.ContainsKey("cardNumber") | args.ContainsKey("pan")))
                {
                    throw new Exception("trans_type CONSUME or PRE_AUTH need field[cardNumber] ");
                }
            }

            m_API_URL = Config.backPayURL;

            Init(args);
        }



    }
}
