using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Dtos;
using Ez.Cache;

namespace Ez.Controllers.Library
{
    /// <summary>
    /// 泛型默认控制器基类
    /// created by kongjing
    /// </summary>
    /// <typeparam name="T">默认业务接口，请以后属性DefaultBiz可用</typeparam>
    public abstract class DefaultController<T> : DefaultController
    {
        public T DefaultBiz { set; get; }
    }
    /// <summary>
    /// 默认控制基类
    /// </summary>
    public abstract class DefaultController : BaseController
    {
        private LoginInfoDto currentUser;
        /// <summary>
        /// 身份认证后注入的当前用户身份实例信息
        /// </summary>
        public LoginInfoDto CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    object obj = SessionProxy.Instance.GetLoginInfo();
                    if (obj != null)
                    {
                        currentUser = SessionProxy.Instance.GetLoginInfo() as LoginInfoDto;
                    }
                }
                return currentUser ?? new LoginInfoDto {  id=0};
            }
            set { currentUser = value; }
        }
    }
}
