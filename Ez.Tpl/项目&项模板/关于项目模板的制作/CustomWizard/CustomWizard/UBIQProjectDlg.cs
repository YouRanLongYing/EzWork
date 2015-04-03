using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UBIQProjTemplateWizard
{
    public partial class UBIQProjectDlg : Form
    {
        private string proj_code;

        public string Proj_code
        {
            get { return proj_code; }
        }
        private string proj_name;

        public string Proj_name
        {
            get { return proj_name; }
        }
        private string proj_uistyle;

        public string Proj_uistyle
        {
            get { return proj_uistyle; }
        }
        private string proj_company;

        public string Proj_company
        {
            get { return proj_company; }
        }

        public UBIQProjectDlg(string code)
        {
            InitializeComponent();
            tbx_proj_code.Text = code;
            cb_pro_style.DataSource = new List<object>{new { text = "mac桌面风格", value = "desktop" }, new { text = "传统管理框架", value = "tradition" }};
            cb_pro_style.DisplayMember = "text";
            cb_pro_style.ValueMember = "value";
            cb_pro_style.SelectedIndex = 0;
            
        }

        private void btn_create_proj_Click(object sender, EventArgs e)
        {
            proj_code = tbx_proj_code.Text.Trim();
            proj_name = tbx_proj_name.Text.Trim();
            proj_company = tbx_compay_name.Text.Trim();
            proj_uistyle = cb_pro_style.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(proj_code) && !string.IsNullOrEmpty(proj_name) && !string.IsNullOrEmpty(proj_company) && !string.IsNullOrEmpty(proj_uistyle))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Dispose();
            }
            else
            {
                lbl_error.Text = "请按照要求填写指定内容！";
            }
        }
    }
}
