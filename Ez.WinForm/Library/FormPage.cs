using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ez.WinForm.Library
{
    public class FormPage:FormDefault
    {
        public FormPage() {
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }
    }

}
