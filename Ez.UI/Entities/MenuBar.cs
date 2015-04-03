using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.UI.Entities
{
    public class MenuBar : Menu
    {
        public MenuBar(string name)
        {
            this.Name = name;
        }
        /// <summary>
        /// 分组
        /// </summary>
        public new IList<MGroup> Children { set; get; }
    }
    public class MGroup : Menu
    {
        public MGroup(string name)
        {
            this.Name = name;
        }
        /// <summary>
        /// 分组
        /// </summary>
        public new IList<Menu> Children { set; get; }
    }
}
