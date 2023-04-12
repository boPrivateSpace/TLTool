
namespace FengXuTLTool
{
    partial class FrmIp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmIp));
            this.edtIp = new DevExpress.XtraEditors.TextEdit();
            this.edtUserName = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.edtPwd = new DevExpress.XtraEditors.TextEdit();
            this.edtFilePath = new DevExpress.XtraEditors.TextEdit();
            this.edtFileName = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSure = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.edtIp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFilePath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFileName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // edtIp
            // 
            this.edtIp.Location = new System.Drawing.Point(124, 22);
            this.edtIp.Name = "edtIp";
            this.edtIp.Size = new System.Drawing.Size(169, 24);
            this.edtIp.TabIndex = 0;
            // 
            // edtUserName
            // 
            this.edtUserName.Location = new System.Drawing.Point(124, 63);
            this.edtUserName.Name = "edtUserName";
            this.edtUserName.Size = new System.Drawing.Size(169, 24);
            this.edtUserName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "用户名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 5;
            this.label3.Tag = "";
            this.label3.Text = "密码";
            // 
            // edtPwd
            // 
            this.edtPwd.Location = new System.Drawing.Point(124, 106);
            this.edtPwd.Name = "edtPwd";
            this.edtPwd.Properties.UseSystemPasswordChar = true;
            this.edtPwd.Size = new System.Drawing.Size(169, 24);
            this.edtPwd.TabIndex = 4;
            // 
            // edtFilePath
            // 
            this.edtFilePath.Location = new System.Drawing.Point(124, 150);
            this.edtFilePath.Name = "edtFilePath";
            this.edtFilePath.Size = new System.Drawing.Size(169, 24);
            this.edtFilePath.TabIndex = 7;
            // 
            // edtFileName
            // 
            this.edtFileName.Location = new System.Drawing.Point(124, 203);
            this.edtFileName.Name = "edtFileName";
            this.edtFileName.Size = new System.Drawing.Size(169, 24);
            this.edtFileName.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "文件路径";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "文件名";
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(103, 256);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(92, 30);
            this.btnSure.TabIndex = 11;
            this.btnSure.Text = "确认";
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // FrmIp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 310);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.edtFileName);
            this.Controls.Add(this.edtFilePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.edtPwd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edtUserName);
            this.Controls.Add(this.edtIp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmIp";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.FrmIp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.edtIp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFilePath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFileName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit edtIp;
        private DevExpress.XtraEditors.TextEdit edtUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit edtPwd;
        private DevExpress.XtraEditors.TextEdit edtFilePath;
        private DevExpress.XtraEditors.TextEdit edtFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton btnSure;
    }
}