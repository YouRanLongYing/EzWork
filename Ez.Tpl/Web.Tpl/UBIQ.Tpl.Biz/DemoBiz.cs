using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Framework.Biz;
using UBIQ.Tpl.IBiz;
using UBIQ.Tpl.Dto.Entity;

namespace UBIQ.Tpl.Biz
{
    public class DemoBiz : DefaultBiz, IDemoBiz
    {
        public Framework.IBiz.BizResult<Demo> TestFunction(Demo dto)
        {
            return new Framework.IBiz.BizResult<Demo>(true, new Demo
            {
                Key = "verson",
                Value = "v2.0"

            });
        }
    }
}
