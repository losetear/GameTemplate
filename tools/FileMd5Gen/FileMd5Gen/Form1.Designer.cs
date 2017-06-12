namespace FileMd5Gen
{
    partial class Form1
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
            this.info = new System.Windows.Forms.ListBox();
            this.btnGenAndroidMd5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // info
            // 
            this.info.FormattingEnabled = true;
            this.info.ItemHeight = 12;
            this.info.Location = new System.Drawing.Point(5, 12);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(561, 280);
            this.info.TabIndex = 1;
            // 
            // btnGenAndroidMd5
            // 
            this.btnGenAndroidMd5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenAndroidMd5.Location = new System.Drawing.Point(199, 326);
            this.btnGenAndroidMd5.Name = "btnGenAndroidMd5";
            this.btnGenAndroidMd5.Size = new System.Drawing.Size(150, 23);
            this.btnGenAndroidMd5.TabIndex = 3;
            this.btnGenAndroidMd5.Text = "生成MD5";
            this.btnGenAndroidMd5.UseVisualStyleBackColor = true;
            this.btnGenAndroidMd5.Click += new System.EventHandler(this.GenMd5);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 389);
            this.Controls.Add(this.btnGenAndroidMd5);
            this.Controls.Add(this.info);
            this.Name = "Form1";
            this.Text = " 文件Md5生成器";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox info;
        private System.Windows.Forms.Button btnGenAndroidMd5;
    }
}

