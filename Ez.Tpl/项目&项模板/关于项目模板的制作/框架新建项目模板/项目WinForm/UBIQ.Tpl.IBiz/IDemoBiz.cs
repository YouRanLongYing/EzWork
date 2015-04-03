using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Framework.IBiz;
using $saferootprojectname$.Dto.Entity;
namespace $safeprojectname$
{
    public interface IDemoBiz:IDefaultBiz
    {
        BizResult<Demo> TestFunction(Demo dto);
    }
}
