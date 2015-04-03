using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Cache
{
    /// <summary>
    /// Session键定义
    /// </summary>
    public static class SessionKeys
    {
        /// <summary>
        /// 用户信息存储键:COM_USERINFO
        /// </summary>
        public const string FRM_LOGININFO = "FRM_LOGININFO";
        /// <summary>
        /// 存储当前语种信息
        /// </summary>
        public const string FRM_LANAGUAGE = "FRM_LANAGUAGE";
    }
    /// <summary>
    /// 缓存键key,可作用于 memcahced、Cache
    /// </summary>
    public static class CacheKeys
    {
        /// <summary>
        /// 系统用户信息,{0}:用户编号
        /// </summary>
        public const string FRM_USER_INFORMATION = "FRM_USER{0}_INFORMATION";

        /// <summary>
        /// 系统用户登录信息,{0}:登录id
        /// </summary>
        public const string FRM_USER_LOGIN_INFO = "FRM_USER{0}_LOGIN_INFO";

        /// <summary>
        /// 用户角色权限信息,{0}:用户编号 {1}:系统代号 {2}:角色编号
        /// </summary>
        public const string FRM_USER_ROLE_INFO = "FRM_USER{0}_SYS{1}_ROLE{2}_INFO";

        /// <summary>
        /// 部门简要信息
        /// </summary>
        public const string FRM_DEP_SHORT_INFO = "FRM_DEP_SHORT_INFO";
        /// <summary>
        /// 部门信息,{0}:部门编号
        /// </summary>
        public const string FRM_DEP_INFORMATION = "FRM_DEP{0}_INFO";

    }
}
