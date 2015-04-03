using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using log4net;
using System.Threading;
using System.Net;
using System.Web;
using System.Reflection;
using AopAlliance.Intercept;

namespace Ez.Core.Interceptor
{
    #region 异步输出委托
    /// <summary>
    /// 日志异步输出委托 coding...
    /// </summary>
    /// <param name="executeInfo">业务方法执行信息</param>
    public delegate void AsyncOutputDelegate(ExecuteInfo executeInfo);
    #endregion

    #region Log4net封装类
    /// <summary>
    /// Log4net封装类
    /// </summary>
    public static class Log4NetManager
    {
        public static ILog defaultLogger = null;

        /// <summary>
        /// 默认使用的日志对象
        /// </summary>
        public static ILog DefaultLogger 
        {
            get
            {
                return defaultLogger;
            }
        }

        /// <summary>
        /// 静态构造器
        /// </summary>
        static Log4NetManager()
        {
            defaultLogger = LogManager.GetLogger("DefaultLogger");
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="executeInfo">调用目标执行信息</param>
        public static void Output(ExecuteInfo executeInfo)
        {
            string outputString = CreateLogMessage(executeInfo);
            switch (executeInfo.LogLevel)
            {
                case LogLevel.Off:
                    {
                        break;
                    }
                case LogLevel.Error:
                    {
                        DefaultLogger.Error(outputString.ToString(), executeInfo.Exception);
                        break;
                    }
                case LogLevel.Info:
                    {
                        DefaultLogger.Info(outputString.ToString());
                        
                        break;
                    }
                case LogLevel.Debug:
                    {
                        DefaultLogger.Debug(outputString.ToString());
                        break;
                    }
            }
        }

        private static string CreateLogMessage(ExecuteInfo executeInfo)
        {
            StringBuilder message = new StringBuilder();
            if (executeInfo.IsEndPoint)
            {
                message.AppendFormat("{0}结束:{1}.{2}",Environment.NewLine,executeInfo.TargetType.FullName, executeInfo.MethodName);
                return message.ToString();
            }
            message.AppendFormat("{0}开始:{1}.{2}", Environment.NewLine, executeInfo.TargetType.FullName, executeInfo.MethodName);

            message.Append("(");
            if (executeInfo.Args != null && executeInfo.Args.Length > 0)
            {
                if (DefaultLogger.IsInfoEnabled)         
                {
                    for (int i = 0; i < executeInfo.Args.Length; i++)
                    {
                        object arg = executeInfo.Args[i];
                        message.AppendFormat("Arg{0}:{1}", i, arg != null ? arg.ToString() : "null");
                    }
                }
                else
                {
                    message.Append("......");
                }
            }
            message.Append(")");

            if (DefaultLogger.IsDebugEnabled && executeInfo.StackTrace != null)
            {
                message.Append(executeInfo.StackTrace);
                message.Append(Environment.NewLine);
            }

            return message.ToString();
        }
    }
    #endregion

    #region 方法执行信息
    /// <summary>
    /// 方法执行信息
    /// </summary>
    public struct ExecuteInfo
    {
        /// <summary>
        /// 调用目标类型信息
        /// </summary>
        public Type TargetType { get; set; }
        /// <summary>
        /// 调用方法名称
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 参数列表
        /// </summary>
        public object[] Args { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogLevel LogLevel { get; set; }
        /// <summary>
        /// 是否结束标记
        /// </summary>
        public bool IsEndPoint { get; set; }
        /// <summary>
        /// 异常对象
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// 调试堆栈
        /// </summary>
        public StackTrace StackTrace { get; set; }

        /// <summary>
        /// 构造子
        /// </summary>
        public ExecuteInfo(Type targetType, MethodInfo method, object[] args, LogLevel logLevel, bool isEndPoint, Exception exception)
            : this()
        {
            this.TargetType = targetType;
            this.MethodName = method.Name;
            this.Args = args;
            this.LogLevel = logLevel;
            this.IsEndPoint = isEndPoint;
            this.Exception = exception;
        }
        /// <summary>
        /// 构造子
        /// </summary>
        public ExecuteInfo(IMethodInvocation invocation, bool isEndPoint, LogLevel logLevel)
            : this(invocation.Target.GetType(), invocation.Method, invocation.Arguments, logLevel, isEndPoint, null)
        {
        }
    }
    #endregion

    #region 日志级别枚举
    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 关闭
        /// </summary>
        Off=1,
        /// <summary>
        /// 错误
        /// </summary>
        Error=3,
        /// <summary>
        /// 信息
        /// </summary>
        Info=5,
        /// <summary>
        /// 调试
        /// </summary>
        Debug = 6,

        /// <summary>
        /// 严重故障
        /// </summary>
        Fatal = 2,
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 4,
        /// <summary>
        /// 所有
        /// </summary>
        All=7
    }
    #endregion
}

