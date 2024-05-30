using Newtonsoft.Json;
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


        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            System.Environment.Exit(0);
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
            StringBuilder stringBuilder = new StringBuilder();
            var res = JsonConvert.DeserializeObject<StatusCodeRes>(NetHelper.Get(NetHelper.Url + "api/Default"));
            if (res.status == "200")
            {
                this.lyBl.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.lyCdk.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.lyHq.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.lySerach.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.LyShop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                stringBuilder.Append("工具支持生成上传CDK,可以直接通过物品名称搜索");
                stringBuilder.AppendLine();
                stringBuilder.Append("支持一键导出爆率");
                stringBuilder.AppendLine();
                stringBuilder.Append("新增逍遥记商店工具");
                stringBuilder.AppendLine();
                stringBuilder.Append("新增txt合区工具");
                stringBuilder.AppendLine();
                stringBuilder.Append("txt合区工具修复,带前缀后缀的数据处理,使用本工具需确认文本和GUID对应");
                stringBuilder.AppendLine();
                stringBuilder.Append("导出爆率支持导出html文件,感谢心语共享的网页资源");
                stringBuilder.AppendLine();
                stringBuilder.Append("新增游戏内查看爆率功能,工具适配功能请联系作者");
                stringBuilder.AppendLine();
                stringBuilder.Append("持续更新中有问题联系作者！！！！");
            }
            else
            {
                stringBuilder.Append(" 服务器连接失败");

            }
            this.edtMemo.Text = stringBuilder.ToString();

            HardwareInfo hardwareInfo = new HardwareInfo();
            string code = hardwareInfo.GetMaginCode();
            var res1 = JsonConvert.DeserializeObject<ExecuteResult<UserPermission>>(NetHelper.Get(NetHelper.Url + "UsrPremissionControler"));
            if (res1.Status == "200")
            {
                var list = res1.Data?.results.Where(x => x.MachineCode == code).Select(x=>x.Permission).FirstOrDefault();
                foreach (var item in list.Split(','))
                {
                    switch (item)
                    {
                        case "DropItem":
                            this.lyDrop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            break;
                    }
                }

            }
        }

        private void btnShop_Click(object sender, EventArgs e)
        {
            FrmXYJShop frmXYJShop = new FrmXYJShop();
            frmXYJShop.Show();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            FrmFileChange frmXYJShop = new FrmFileChange();
            frmXYJShop.Show();
        }

        private void btnImportSerach_Click(object sender, EventArgs e)
        {
           var articleLists = ArticleBase.ArticleLists;

            List<string> columns = new List<string> { "STRING", "INT", "INT" };
            var name = "Plugin_GameSearch.dll";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls|文本文件(*.txt)|*.txt|*.dll|";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = false;
            //saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "导出文件为...";
            saveFileDialog.FileName = name;
            DialogResult dr = saveFileDialog.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            DataTable dt = new DataTable();

            dt = ModelTool.ModelsToDataTable<ArticleList>(articleLists);
            dt.Columns["Name"].ColumnName = "名称";
            dt.Columns["Index"].ColumnName = "ID";
            dt.Columns["Num"].ColumnName = "叠加数量";
            Excel.ExportToExcelOrTxt(dt, saveFileDialog.FileName, columns);
        }

        private void btnBaoLv_Click(object sender, EventArgs e)
        {
            PackBase packBase = new PackBase();
            packBase.GetMonstorPackDropLua();

        }
    }
}
