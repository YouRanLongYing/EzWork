namespace Ez.WinForm
{
    partial class StartForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.lbl_starting = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_login = new Ez.XControls.Buttons.ButtonEx();
            this.tbx_uid = new Ez.XControls.TextBoxs.XTextBox();
            this.tbx_pwd = new Ez.XControls.TextBoxs.XTextBox();
            this.lbl_close = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_starting
            // 
            this.lbl_starting.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_starting.Location = new System.Drawing.Point(84, 430);
            this.lbl_starting.Name = "lbl_starting";
            this.lbl_starting.Size = new System.Drawing.Size(243, 12);
            this.lbl_starting.TabIndex = 0;
            this.lbl_starting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 305);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "账户";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 348);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码";
            // 
            // btn_login
            // 
            this.btn_login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.btn_login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_login.Location = new System.Drawing.Point(321, 485);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(84, 26);
            this.btn_login.TabIndex = 2;
            this.btn_login.Text = "登录";
            this.btn_login.UseVisualStyleBackColor = false;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // tbx_uid
            // 
            this.tbx_uid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbx_uid.HotColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.tbx_uid.Location = new System.Drawing.Point(121, 301);
            this.tbx_uid.Name = "tbx_uid";
            this.tbx_uid.Size = new System.Drawing.Size(206, 21);
            this.tbx_uid.TabIndex = 0;
            this.tbx_uid.Text = "administrator";
            // 
            // tbx_pwd
            // 
            this.tbx_pwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbx_pwd.HotColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.tbx_pwd.Location = new System.Drawing.Point(121, 344);
            this.tbx_pwd.Name = "tbx_pwd";
            this.tbx_pwd.PasswordChar = '*';
            this.tbx_pwd.Size = new System.Drawing.Size(206, 21);
            this.tbx_pwd.TabIndex = 1;
            this.tbx_pwd.Text = "123456";
            // 
            // lbl_close
            // 
            this.lbl_close.BackColor = System.Drawing.Color.Transparent;
            this.lbl_close.Image = global::Ez.WinForm.Properties.Resources.close;
            this.lbl_close.Location = new System.Drawing.Point(381, 9);
            this.lbl_close.Name = "lbl_close";
            this.lbl_close.Size = new System.Drawing.Size(26, 24);
            this.lbl_close.TabIndex = 3;
            this.lbl_close.Click += new System.EventHandler(this.lbl_close_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 523);
            this.Controls.Add(this.lbl_close);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.tbx_uid);
            this.Controls.Add(this.tbx_pwd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_starting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbl_starting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Ez.XControls.TextBoxs.XTextBox tbx_pwd;
        private Ez.XControls.TextBoxs.XTextBox tbx_uid;
        private Ez.XControls.Buttons.ButtonEx btn_login;
        private System.Windows.Forms.Label lbl_close;
    }
}

