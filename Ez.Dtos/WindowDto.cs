using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Ez.UI.HtmlExtend;
using Ez.UI.Entities;

namespace Ez.Dtos
{
    [Serializable]
    public class WindowDto
    {
       public ShortCutCollection ShortCuts { set; get; }
       public TopBar TopBar { set; get; }
       public  LoginInfoDto LoginInfo { set; get; }
       public  UserInfoDto UserInfo { set; get; }
    }
}
