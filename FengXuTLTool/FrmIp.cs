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
    public partial class FrmIp : Form
    {
        public FrmIp(FileModel fileModel)
        {
            InitializeComponent();
            _fileModel1 = fileModel;
        }

        private FileModel _fileModel1;


        private void FrmIp_Load(object sender, EventArgs e)
        {

        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            _fileModel1.Ip = this.edtIp.Text;
            _fileModel1.UserName = this.edtUserName.Text;
            _fileModel1.Pwd = this.edtPwd.Text;
            _fileModel1.FileName = this.edtFileName.Text;
            _fileModel1.FilePath = this.edtFilePath.Text;

            JsonFile.SetJsonFile(_fileModel1);
            this.Close();
        }
    }
}
