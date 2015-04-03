using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.UI.Entities;

namespace Ez.Dtos
{
   public class LayoutWinDto:WindowDto
    {
       public IList<MenuBar> TopBar { set; get; }
    }
}
