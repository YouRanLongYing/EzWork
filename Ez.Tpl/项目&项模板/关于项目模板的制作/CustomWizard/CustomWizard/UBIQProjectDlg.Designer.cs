namespace UBIQProjTemplateWizard
{
    partial class UBIQProjectDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbx_proj_name = new System.Windows.Forms.TextBox();
            this.btn_create_proj = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_pro_style = new System.Windows.Forms.ComboBox();
            this.tbx_proj_code = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbx_compay_name = new System.Windows.Forms.TextBox();
            this.lbl_error = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbx_proj_name
            // 
            this.tbx_proj_name.Location = new System.Drawing.Point(42, 96);
            this.tbx_proj_name.Name = "tbx_proj_name";
            this.tbx_proj_name.Size = new System.Drawing.Size(229, 21);
            this.tbx_proj_name.TabIndex = 0;
            // 
            // btn_create_proj
            // 
            this.btn_create_proj.Location = new System.Drawing.Point(184, 291);
            this.btn_create_proj.Name = "btn_create_proj";
            this.btn_create_proj.Size = new System.Drawing.Size(87, 31);
            this.btn_create_proj.TabIndex = 1;
            this.btn_create_proj.Text = "创建";
            this.btn_create_proj.UseVisualStyleBackColor = true;
            this.btn_create_proj.Click += new System.EventHandler(this.btn_create_proj_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "项目名称(全称)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "UI风格";
            // 
            // cb_pro_style
            // 
            this.cb_pro_style.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_pro_style.FormattingEnabled = true;
            this.cb_pro_style.Location = new System.Drawing.Point(42, 155);
            this.cb_pro_style.Name = "cb_pro_style";
            this.cb_pro_style.Size = new System.Drawing.Size(140, 20);
            this.cb_pro_style.TabIndex = 4;
            // 
            // tbx_proj_code
            // 
            this.tbx_proj_code.Location = new System.Drawing.Point(42, 38);
            this.tbx_proj_code.Name = "tbx_proj_code";
            this.tbx_proj_code.Size = new System.Drawing.Size(142, 21);
            this.tbx_proj_code.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "项目代号";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "公司";
            // 
            // tbx_compay_name
            // 
            this.tbx_compay_name.Location = new System.Drawing.Point(42, 212);
            this.tbx_compay_name.Name = "tbx_compay_name";
            this.tbx_compay_name.Size = new System.Drawing.Size(229, 21);
            this.tbx_compay_name.TabIndex = 8;
            this.tbx_compay_name.Text = "优碧特软件(西安)有限公司";
            // 
            // lbl_error
            // 
            this.lbl_error.AutoSize = true;
            this.lbl_error.ForeColor = System.Drawing.Color.Red;
            this.lbl_error.Location = new System.Drawing.Point(42, 263);
            this.lbl_error.Name = "lbl_error";
            this.lbl_error.Size = new System.Drawing.Size(0, 12);
            this.lbl_error.TabIndex = 9;
            // 
            // UBIQProjectDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 334);
            this.Controls.Add(this.lbl_error);
            this.Controls.Add(this.tbx_compay_name);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbx_proj_code);
            this.Controls.Add(this.cb_pro_style);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_create_proj);
            this.Controls.Add(this.tbx_proj_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UBIQProjectDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "新建项目";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbx_proj_name;
        private System.Windows.Forms.Button btn_create_proj;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_pro_style;
        private System.Windows.Forms.TextBox tbx_proj_code;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbx_compay_name;
        private System.Windows.Forms.Label lbl_error;
    }
}