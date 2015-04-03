using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.BizContract;
using Ez.DBContract;
using Ez.Core;
using System.Reflection;
using System.IO;
using Ez.Dtos.Entities;
using Ez.Dtos;
using Ez.Cache;
using System.Runtime.Serialization;

namespace Ez.Biz
{
    /// <summary>
    /// 业务模块默认基类
    /// </summary>
    public abstract class DefaultBiz : BaseBiz
    {
        private IDbMaster dbMaster;
        /// <summary>
        /// 框架用户中心数据库操作对象
        /// </summary>
        public IDbMaster DbMaster
        {
            get
            {
                if (this.dbMaster == null)
                {
                   this.dbMaster =Utils.GetSpringObject<IDbMaster>("DbMasterDaoTarget");
                }
                return this.dbMaster;
            }
        }

        /// <summary>
        /// 用户中心数据库管理对象
        /// </summary>
        public IDefaultDao UcDb
        {
            get {
                return this.DbMaster.Get("fw");
            }
        }
        /// <summary>
        /// 产品数据库管理对象
        /// </summary>
        public IDefaultDao ProDb
        {
            get
            {
                return this.DbMaster.Get("pro");
            }
        }
        /// <summary>
        /// 当前语言
        /// </summary>
        public string CurrentLang
        {
            get
            {
                return Helper.Tools.GetSessionValue(Constans.SYS_LANAGUE_KEY).ToSafeString("zh-CN");
            }
        }

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        public LoginInfoDto CurrentUser
        {
            get
            {
                LoginInfoDto dto = null;
                var instance = SessionProxy.Instance;
                if (instance != null)
                {
                    object obj = instance.GetLoginInfo();
                    if (obj != null)
                    {
                        dto = obj as LoginInfoDto;
                    }
                }
                return dto;
            }
        }

        /// <summary>
        /// 账户管理业务模块实例
        /// </summary>
        public IAccountBiz AccountBiz
        {
            get
            {
                if (this is IAccountBiz)
                {
                    return this as IAccountBiz;
                }
                else
                {
                    return Utils.GetSpringObject<IAccountBiz>("AccountBiz");
                }
            }
        }

        /// <summary>
        /// 角色管理业务模块实例
        /// </summary>
        public IRoleBiz RoleBiz {
            get
            {
                if (this is IRoleBiz)
                {
                    return this as IRoleBiz;
                }
                else
                {
                    return Utils.GetSpringObject<IRoleBiz>("RoleBiz");
                }
            }
        }
    }

}
