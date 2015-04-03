using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Payment.Contract;

namespace Ez.Payment
{
    /// <summary>
    /// 框架支付信息集合
    /// </summary>
    public class PaymentCollect
    {
        /// <summary>
        /// 默认支付模型
        /// </summary>
        public PaymentConfigAdapter DefaultPayment { internal set; get; }
        /// <summary>
        /// 可用于支付的模型选项
        /// </summary>
        public IList<PaymentConfigAdapter> Options { internal set; get; }

    }
}
