using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Payment.Alipay;
using Ez.Payment.Contract;
using Ez.Payment.Upop;

namespace Ez.Payment
{
    /// <summary>
    /// 服务器通知委托
    /// </summary>
    /// <param name="out_trade_no">商户订单号</param>
    /// <param name="trade_no">支付宝交易号</param>
    /// <param name="issync">是否为同步通知</param>
    /// <param name="trade_status">通知状态码</param>
    /// <returns></returns>
    public delegate bool BizNotifyHandler(string out_trade_no, string trade_no, bool issync, string trade_status);
    /// <summary>
    /// 支付工厂
    /// </summary>
    public class PaymentFactory
    {
        private BizNotifyHandler bizProcessFunction;
        /// <summary>
        /// 构造支付模块
        /// </summary>
        /// <param name="bizProcessFunction">服务器通知处理逻辑</param>
        public PaymentFactory(BizNotifyHandler bizProcessFunction)
        {
            this.bizProcessFunction = bizProcessFunction;
        }
        public PaymentFactory()
        {
        }
        private IDictionary<PaymentType, IPayment> instanceDic = new Dictionary<PaymentType, IPayment>();
        /// <summary>
        /// 获取支付请求对象
        /// </summary>
        /// <param name="paymentType">支付类型</param>
        /// <param name="recordlog">日否记录日志（默认记录）</param>
        /// <returns>支付处理对象</returns>
        public IPayment Instance(PaymentType paymentType,bool recordlog=true)
        {
            IPayment instance = null;
            if (this.instanceDic.ContainsKey(paymentType))
            {
                instance = this.instanceDic[paymentType];
            }
            else
            {
                switch (paymentType)
                {
                    case PaymentType.Alipay: {
                        instance = new AlipayPayment(recordlog);
                        if (this.bizProcessFunction != null)
                        {
                            instance.BizNotify_Event += new BizNotifyHandler(this.bizProcessFunction);
                        }
                        instanceDic.Add(paymentType, instance);
                    }
                    break;
                    case PaymentType.Upop: {
                        instance = new UpopPayment(recordlog);
                        if (this.bizProcessFunction != null)
                        {
                            instance.BizNotify_Event += new BizNotifyHandler(this.bizProcessFunction);
                        }
                        instanceDic.Add(paymentType, instance);
                    }
                    break;
                    default:
                    throw new Exception("支付类型不存在:" + paymentType.GetHashCode());
                }
            }
            return instance;
        }
    }
}
