#define UseStyle
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UBIQ.Framework.XControls.Library;
using UBIQ.Framework.XControls.Menus;

namespace UBIQ.Framework.XControls.Menus
{
    public class XSideMenuBar : CtrlSolution
    {
        private const int space = 1,sidex=1;
        /// <summary>
        /// 控件集合
        /// </summary>
        private CtrlCollection controls;
        public XSideMenuBar()
        {
            InitializeComponent();
            this.Paint += XSideMenuBar_Paint;
            this.SetStyle(ControlStyles.Opaque, false);
        }
        void XSideMenuBar_Paint(object sender, PaintEventArgs e)
        {



        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // XSideMenuBar
            // 
            //this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Name = "XSideMenuBar";
            this.Size = new System.Drawing.Size(230, 550);
            this.ResumeLayout(false);

        }
        private IList<SideMenuTitle> _innerItem = new List<SideMenuTitle>();
        /// <summary>
        /// 菜单集合
        /// </summary>
        private IList<MenuBlock> items { set; get; }
        /// <summary>
        /// 左侧菜单框菜单列表项
        /// </summary>
        public IList<MenuBlock> Items { get { return items ?? new List<MenuBlock>(); } }
        /// <summary>
        /// 子控件
        /// </summary>
        public new CtrlCollection Controls
        {
            get
            {
                if (controls == null) controls = new CtrlCollection(this, new List<Type> { typeof(SideMenuTitle), typeof(SideMenuChild) });
                return controls;
            }
        }
        /// <summary>
        /// 添加一个菜单项
        /// </summary>
        /// <param name="block"></param>
        public void AddItem(MenuBlock block)
        {
            this.SuspendLayout();
            this.Items.Add(block);

            SideMenuChild sidemenu = new SideMenuChild() { Parent = this, Width = this.Width - 2 };
            int h = 0;
            foreach (XMenuItem item in block.Children)
            {
                sidemenu.AddItem(item);
                h += item.Height;
            }
            sidemenu.Height = h;
           
           
            SideMenuTitle titlebar = new SideMenuTitle()
            {
                Parent =this,
                Text = block.MenuTitle,
                Width = sidemenu.Width,
                Child = sidemenu,
                Height = 35
            };

            SetPosition(ref titlebar);
            this.ResumeLayout();
        }
        /// <summary>
        /// 获取子控件容器在容器中的位置
        /// </summary>
        /// <returns></returns>
        private void SetPosition(ref SideMenuTitle titlebar)
        {
            Point childPoint = Point.Empty;
            if (this.Controls.Count == 0)
            {
                titlebar.Location = new Point(sidex, space);
            }
            else
            {
                int y = 0;
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is SideMenuChild) continue;
                    y += ctrl.Height;
                }
                y += space * this.Controls.Count;
                titlebar.Location = new Point(sidex, y);
            }
            if (titlebar.HasChild)
            {
                titlebar.Child.ViewPoint = titlebar.Child.Location = new Point(sidex, titlebar.Location.Y + titlebar.Height);// + space
                titlebar.Child.Hide();
            }
            this.Controls.Add(titlebar);
            this.Controls.Add(titlebar.Child);
            _innerItem.Add(titlebar);
        }


        public int PerIndex = -1;
        public int CurrentIndex = 0;
        /// <summary>
        /// 重新定位可见的菜单项
        /// </summary>
        public void DisplayViewMenus(SideMenuTitle viewMenu)
        {
           CurrentIndex = _innerItem.IndexOf(viewMenu);
           int nextY = 1;
           for (int i = 0; i < _innerItem.Count; i++)
           {
              SideMenuTitle cur = _innerItem[i];
              cur.Location = new Point(sidex, nextY);
#if UseStyle
              int y = cur.Location.Y + cur.Height;
#else
              int y = cur.Location.Y + cur.Height + space;
#endif
              cur.Child.Location = new Point(sidex,y);
              if (i == CurrentIndex)
              {
                  nextY = y + cur.Child.Height+space;
                  cur.Child.Show();
              }
              else
              {
                  cur.Child.Hide();
                  nextY = y + space;
              }
           }

        }
    }

    public class MenuBlock
    {
        /// <summary>
        /// 菜单的标题
        /// </summary>
        public string MenuTitle { set; get; }
        /// <summary>
        /// 对应的子模块
        /// </summary>
        public MenuBlockChildren Children { set; get; }
    }

    public class MenuBlockChildren : IEnumerable
    {
        public Control TargetContiner { set; get; }
        private IList<XMenuItem> items;
        public IList<XMenuItem> Items
        {
            get
            {
                if (items == null) items = new List<XMenuItem>();
                return items;
            }
        }
        public IEnumerator GetEnumerator()
        {
            foreach (XMenuItem item in Items)
            {
                yield return item;
            }
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="item">xmenuitem</param>
        public void Add(XMenuItem item)
        {
            Items.Add(item);
        }
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="name">item名称</param>
        /// <returns></returns>
        public XMenuItem Find(string name)
        {
            return this.Items.FirstOrDefault(p => p.Name.Equals(name));
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="item">项目</param>
        public void Remove(XMenuItem item)
        {
            this.Items.Remove(item);
        }
    }
}
