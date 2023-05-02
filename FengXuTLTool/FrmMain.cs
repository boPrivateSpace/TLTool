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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                PackBase packBase = new PackBase();
                packBase.GetMonstorPackDrop();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
 
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.edtTItle.Text = "欢迎使用本工具,作者:风絮 QQ 464141564";

            this.edtMemo.Text = "工具支持生成上传CDK,可以直接通过物品名称搜索\r\n支持一键导出爆率\r\n持续更新中有问题联系作者,看心情修复！！！！";
        }

        private void btnShop_Click(object sender, EventArgs e)
        {
            FrmXYJShop frmXYJShop = new FrmXYJShop();
            frmXYJShop.Show();
        }
    }
}
