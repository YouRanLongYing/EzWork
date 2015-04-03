using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using UBIQ.Tpl.IBiz;
using UBIQ.Framework.Biz;


namespace UBIQ.Tpl.Biz
{
    public class DemoBiz : DefaultBiz, IDemoBiz
    {

        public UBIQ.Framework.IBiz.BizResult<Dto.Entity.Demo> TestFunction(Dto.Entity.Demo dto)
        {
            throw new NotImplementedException();
        }
    }
}
