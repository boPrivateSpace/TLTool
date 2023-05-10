using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FengXuTLTool
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            var code =MachineCode.GetMachineCodeString();
            // 机器码写入配置文件  下次登录判断已经写入的直接登录即可
            var codes = JsonFile.ReadJsonLoginFile();
            var demo = codes.Where(x => x.Code == this.edtMemo.Text).FirstOrDefault();
            if (demo == null)
            {
                MessageBox.Show("输入验证码不正确,请联系QQ 464141564");
                return;
            }

            FileModel fileModel = new FileModel();
            fileModel.LoginCode = code;
            JsonFile.SetJsonFile(fileModel);

            this.Hide();
            FrmMain frmMain = new FrmMain();
            frmMain.Show();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            var fileMode = JsonFile.ReadJsonConfigFile();
            var code = MachineCode.GetMachineCodeString();

            if (fileMode != null && !string.IsNullOrEmpty(fileMode.LoginCode) && code == fileMode.LoginCode)
            {
                this.ShowInTaskbar = false;
                this.Opacity = 0;
                FrmMain frmMain = new FrmMain();
                frmMain.Show();
            }
        }
    }
}
