using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.UI
{
    /// <summary>
    /// 资源，用户名 等价于 [CDisplayName(LangKey = "COM_N_UserName", DefaultName = "COM_N_UserName")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_UserName : CDisplayNameAttribute
    {
        public COM_N_UserName()
            : base("COM_N_UserName")
        {

        }

    }
    /// <summary>
    /// 资源，用户性别 等价于 [CDisplayName(LangKey = "COM_N_Gender", DefaultName = "COM_N_Gender")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_Gender : CDisplayNameAttribute
    {
        public COM_N_Gender()
            : base("COM_N_Gender")
        {
        }
    }
    /// <summary> 
    /// 资源，年龄 等价于 [CDisplayName(LangKey = "COM_N_Age", DefaultName = "COM_N_Age")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_Age : CDisplayNameAttribute
    {
        public COM_N_Age()
            : base("COM_N_Age")
        {
        }
    }
    /// <summary>
    /// 资源，身份证 等价于 [CDisplayName(LangKey = "COM_N_IdCard", DefaultName = "COM_N_IdCard")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_IdCard : CDisplayNameAttribute
    {
        public COM_N_IdCard()
            : base("COM_N_IdCard")
        {
        }
    }
    /// <summary>
    /// 资源，座机号 等价于 [CDisplayName(LangKey = "COM_N_Tel", DefaultName = "COM_N_Tel")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_Tel : CDisplayNameAttribute
    {
        public COM_N_Tel()
            : base("COM_N_Tel")
        {
        }
    }
    /// <summary>
    /// 资源，手机号 等价于 [CDisplayName(LangKey = "COM_N_Mobile", DefaultName = "COM_N_Mobile")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_Mobile : CDisplayNameAttribute
    {
        public COM_N_Mobile()
            : base("COM_N_Mobile")
        {
        }
    }
    /// <summary>
    ///  资源，QQ号 等价于 [CDisplayName(LangKey = "COM_N_QQ", DefaultName = "COM_N_QQ")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_QQ : CDisplayNameAttribute
    {
        public COM_N_QQ()
            : base("COM_N_QQ")
        {
        }
    }
    /// <summary>
    ///  资源，邮箱地址 等价于 [CDisplayName(LangKey = "COM_N_Email", DefaultName = "COM_N_Email")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_Email : CDisplayNameAttribute
    {
        public COM_N_Email()
            : base("COM_N_Email")
        {
        }
    }
    /// <summary>
    /// 资源，住址或地址 等价于 [CDisplayName(LangKey = "COM_N_Address", DefaultName = "COM_N_Address")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_Address : CDisplayNameAttribute
    {
        public COM_N_Address()
            : base("COM_N_Address")
        {
        }
    }
    /// <summary>
    /// 资源，登录名 等价于 [CDisplayName(LangKey = "COM_N_LoginName", DefaultName = "COM_N_LoginName")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_LoginName : CDisplayNameAttribute
    {
        public COM_N_LoginName()
            : base("COM_N_LoginName")
        {
        }
    }
    /// <summary>
    /// 资源，登录密码 等价于 [CDisplayName(LangKey = "COM_N_Password", DefaultName = "COM_N_Password")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_Password : CDisplayNameAttribute
    {
        public COM_N_Password()
            : base("COM_N_Password")
        {
        }
    }
    /// <summary>
    /// 资源，登录次数 等价于 [CDisplayName(LangKey = "COM_N_LoginNum", DefaultName = "COM_N_LoginNum")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_LoginNum : CDisplayNameAttribute
    {
        public COM_N_LoginNum()
            : base("COM_N_LoginNum")
        {
        }
    }
    /// <summary>
    /// 资源，最后登录时间 等价于 [CDisplayName(LangKey = "COM_N_LastLoginTime", DefaultName = "COM_N_LastLoginTime")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_LastLoginTime : CDisplayNameAttribute
    {
        public COM_N_LastLoginTime()
            : base("COM_N_LastLoginTime")
        {
        }
    }
    /// <summary>
    /// 资源，注册时间 等价于 [CDisplayName(LangKey = "COM_N_RegTime", DefaultName = "COM_N_RegTime")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_RegTime : CDisplayNameAttribute
    {
        public COM_N_RegTime()
            : base("COM_N_RegTime")
        {
        }
    }
    /// <summary>
    /// 资源，注册IP 等价于 [CDisplayName(LangKey = "COM_N_RegIP", DefaultName = "COM_N_RegIP")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_RegIP : CDisplayNameAttribute
    {
        public COM_N_RegIP()
            : base("COM_N_RegIP")
        {
        }
    }
    /// <summary>
    /// 资源，最后登录IP 等价于 [CDisplayName(LangKey = "COM_N_LastLoginIP", DefaultName = "COM_N_LastLoginIP")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class COM_N_LastLoginIP : CDisplayNameAttribute
    {
        public COM_N_LastLoginIP()
            : base("COM_N_LastLoginIP")
        {
        }
    }

}
