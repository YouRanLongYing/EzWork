namespace Ez.WinForm
{
    partial class Test2
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
            this.xButton1 = new Ez.XControls.Buttons.XButton();
            this.SuspendLayout();
            // 
            // xButton1
            // 
            this.xButton1.Child = null;
            this.xButton1.Image = global::Ez.WinForm.Properties.Resources.versioncue;
            this.xButton1.ImgPos = Ez.XControls.Buttons.ButtonImgPos.Normal;
            this.xButton1.Location = new System.Drawing.Point(308, 212);
            this.xButton1.Name = "xButton1";
            this.xButton1.Size = new System.Drawing.Size(44, 32);
            this.xButton1.Src = null;
            this.xButton1.TabIndex = 0;
            this.xButton1.TargetContainer = null;
            this.xButton1.TargetCtrl = null;
            this.xButton1.Text = null;
            this.xButton1.TipMsg = "设置一个按钮提示！";
            // 
            // Test2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 623);
            this.Controls.Add(this.xButton1);
            this.Name = "Test2";
            this.Text = "Test2";
            this.ResumeLayout(false);

        }

        #endregion

        private XControls.Buttons.XButton xButton1;
    }
}