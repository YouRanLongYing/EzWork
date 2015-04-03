using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Payment.Contract
{
    /// <summary>
    /// 统一支付模块接口
    /// </summary>
    public interface IPayment
    {
        /// <summary>
        /// 是否记录日志
        /// </summary>
        bool Recordlog { set; get; }
        /// <summary>
        /// 通知处理事件
        ///判断该笔订单是否在商户网站中已经做过处理
        ///如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
        ///如果有做过处理，不执行商户的业务程序
        /// </summary>
        event BizNotifyHandler BizNotify_Event;
        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）
        /// </summary>
        /// <param name="payinfo">付费信息模型</param>
        /// <param name="ispost">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        string BuildRequest(PayInfo payinfo, bool ispost, string strButtonValue = "submit");

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果
        /// </summary>
        /// <param name="payinfo">付费信息模型</param>
        /// <returns>如支付宝处理结果</returns>
        string BuildRequest(PayInfo payinfo);

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果，带文件上传功能
        /// </summary>
        /// <param name="payinfo">付费信息模型</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="data">文件数据</param>
        /// <param name="contentType">文件内容类型</param>
        /// <param name="lengthFile">文件长度</param>
        /// <returns>如支付宝处理结果</returns>
        string BuildRequest(PayInfo payinfo, string strMethod, string fileName, byte[] data, string contentType, int lengthFile);

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <returns>时间戳字符串</returns>
        string Query_timestamp();
        /// <summary>
        /// 启动页面加载模块
        /// </summary>
        /// <param name="issync">是否为同步通知</param>
        bool NotifyPageLoadStart(bool issync);

    }
}
