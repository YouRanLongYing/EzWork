using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Framework.Biz;
using $saferootprojectname$.IBiz;
using $saferootprojectname$.Dto.Entity;
namespace $safeprojectname$
{
    /// <summary>
    /// clrversion:$clrversion$
    /// Guid:$guid1$
    /// itemname:$itemname$
    /// machinename:$machinename$
    /// projectname:$projectname$
    /// registeredorganization:$registeredorganization$
    /// safeprojectname:$safeprojectname$
    /// userdomain:$userdomain$
    /// username:$username$
    /// webnamespace:$webnamespace$
    /// time:$time$
    /// </summary>
    public class DemoBiz : DefaultBiz, IDemoBiz
    {
        //saferootprojectname=$saferootprojectname$
		//projcode=$projcode$
		//projname=$projname$
		//uistyle=$uistyle$
		//company=$company$
		
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
