using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Payment.Contract
{
    public enum PaymentType
    {
        /// <summary>
        /// 支付宝支付 0
        /// </summary>
        Alipay=0,
        /// <summary>
        /// 在线银联支付 UnionPay Online Pay 1
        /// </summary>
        Upop=1
    }
}
