using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Dtos.Framework;
using UBIQ.Cache.Framework;

namespace UBIQ.Controllers.Framework.Lib
{
    public abstract class DefaultController<T> : DefaultController
    {
        public T BizManager { set; get; }
    }

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
