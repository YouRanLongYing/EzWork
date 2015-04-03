using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Payment.Upop;

namespace Ez.Payment.Contract
{
    /// <summary>
    /// 支付配置信息适配器
    /// </summary>
    public class PaymentConfigAdapter : PaymentConfig
    {
        /// <summary>
        /// 其他支付类型配置
        /// </summary>
        internal object SpecialConfig { set; get; }

        /// <summary>
        /// 设置其他配置信息实例
        /// </summary>
        /// <param name="SpecialConfig"></param>
        internal void SetSpecialConfig(object SpecialConfig)
        {
            this.SpecialConfig = SpecialConfig;
        }

        /// <summary>
        /// 银联在线配置信息
        /// </summary>
        internal ConfigInf UpopConfig
        {
            get
            {
                return this.SpecialConfig as ConfigInf;
            }
        }

        #region 银联在线 特有配置信息
        /// <summary>
        ///  银联在线，交易类型，前台只支持CONSUME 和 PRE_AUTH
        /// </summary>
        public string transType
        {
            get;
            set;
        }

        /// <summary>
        /// 银联在线，币种
        /// </summary>
        public string orderCurrency
        {
            get;set;
        }
        #endregion
    }
}
