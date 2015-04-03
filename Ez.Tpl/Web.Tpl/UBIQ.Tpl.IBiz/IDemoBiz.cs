using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Framework.IBiz;
using UBIQ.Tpl.Dto.Entity;
namespace UBIQ.Tpl.IBiz
{
    public interface IDemoBiz:IDefaultBiz
    {
        BizResult<Demo> TestFunction(Demo dto);
    }
}
