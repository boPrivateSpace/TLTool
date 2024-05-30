using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FengXuTLTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitColumn();
        }
        private string UsedFileName
        {
            get
            {
                return "CommonItem.txt,EquipBase.txt";
            }
        }

        public string path = string.Empty;
        public string Index = "";
        public string Name = "";
        public int Num = 0;
        private FileModel _fileModel;
        private List<ArticleList> _articleLists = new List<ArticleList>();
        private List<CDKCode> cDKCodes = new List<CDKCode>();
        public void InitColumn()
        {
            this.grdvList.OptionsView.ShowGroupPanel = false;
            this.grdvList.OptionsBehavior.Editable = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _fileModel = new FileModel();
            var fileModel = JsonFile.ReadJsonConfigFile();
            if (fileModel != null)
            {
                _fileModel = fileModel;
            }
            this.edtNum.EditValue = 100;
        }


        private void btnCreate_Click(object sender, EventArgs e)
        {
            ArticleList dr = (ArticleList)this.grdvList.GetFocusedRow();
            if (dr == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.edtcdkNum.Text))
            {
                MessageBox.Show("未设置CDK数量");
                return;
            }


            List<string> randow = GetRandom.GetRandString(8, Convert.ToInt32(this.edtcdkNum.Text), "TL");
            List<CDKCode> cDKCodes = new List<CDKCode>();
            foreach (var item in randow)
            {
                CDKCode cDKCode = new CDKCode();
                cDKCode.Code = item;
                cDKCode.Index = "39940003";
                cDKCode.Num = this.edtNum.Text;
                cDKCodes.Add(cDKCode);
            }

            if (string.IsNullOrEmpty(path))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Execl files (*.xls)|*.xls|文本文件(*.txt)|*.txt|*.dll|";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = false;
                //saveFileDialog.CreatePrompt = true;
                saveFileDialog.Title = "导出文件为...";
                saveFileDialog.FileName = "CDK-ItemInfo.txt";
                DialogResult dswr = saveFileDialog.ShowDialog();
                if (dswr != DialogResult.OK)
                {
                    return;
                }

                path = saveFileDialog.FileName;
            }

            DataTable xyjShop = ModelTool.ModelsToDataTable<CDKCode>(cDKCodes);
            Excel.ExportToExcelOrTxt(xyjShop, path, new List<string>(), false,true);

        }


        /// <summary>
        /// 生成不重复随机字符串
        /// </summary>
        /// <param name="count">输入字符串长度</param>
        /// <returns>字符串</returns>
        public static string getRandomDom(int count = 16)
        {
            string t62 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            long ticks = DateTime.Now.Ticks;

            string gen = "";
            int ind = 0;
            while (ind < count)
            {

                Random random = new Random();
                gen += t62[random.Next(t62.Length-1)];
                ind++;
            }
            return gen;
        }


        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.edtQueryText.Text))
            {
                return;
            }

            var queryList = this._articleLists.Where(x => x.Name.Contains(this.edtQueryText.Text)).ToList();


            if (queryList != null)
            {
                grdcList.DataSource = queryList;
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmIp frmIp = new FrmIp(_fileModel);
            frmIp.StartPosition = FormStartPosition.CenterParent;
            frmIp.ShowDialog();
            frmIp.Dispose();
        }

        private void 载入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grdcList.DataSource = null; //每次打开清空内容
            string message = string.Empty;
            if (!ArticleBase.Check(UsedFileName, ref message))
            {
                MessageBox.Show(message);
                return;
            }
            _articleLists = ArticleBase.ArticleLists;
            if (_articleLists != null)
            {
                grdcList.DataSource = _articleLists;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ArticleList dr = (ArticleList)this.grdvList.GetFocusedRow();
            if (dr == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(edtNum.Text))
            {
                this.Num = Convert.ToInt32(edtNum.Text);
            }
            this.Index = dr.Index;
            this.Name = dr.Name;
            
            this.Close();
        }


        private void 财富卡ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<string> randow = GetRandom.GetRandString(8, 200, "TL");
            List<CDKCode> cDKCodes = new List<CDKCode>();
            foreach (var item in randow)
            {
                CDKCode cDKCode = new CDKCode();
                cDKCode.Code = item;
                cDKCode.Num = "1";
                cDKCode.Index = "1";
                cDKCodes.Add(cDKCode);
            }
            DataTable xyjShop = ModelTool.ModelsToDataTable<CDKCode>(cDKCodes);
            Excel.ExportToExcelOrTxt(xyjShop, "CfCard.txt", new List<string>(), false);

        }

        private void 武圣卡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CDKCode> cDKCodes = new List<CDKCode>();

            List<string> randow = GetRandom.GetRandString(11, 200, "WS");
            foreach (var item in randow)
            {
                CDKCode cDKCode = new CDKCode();
                cDKCode.Code = item;
                cDKCode.Num = "1";
                cDKCode.Index = "1";
                cDKCodes.Add(cDKCode);
            }
            DataTable xyjShop = ModelTool.ModelsToDataTable<CDKCode>(cDKCodes);
            Excel.ExportToExcelOrTxt(xyjShop, "WsCard.txt", new List<string>(), false);

        }

        private void 随机CDKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CDKCodeT> cDKCodes = new List<CDKCodeT>();
            for(var i = 0;i<100; i++)
            {
                CDKCodeT cDKCode = new CDKCodeT();
                cDKCode.Code = getRandomDom(); ;
            }
            DataTable xyjShop = ModelTool.ModelsToDataTable<CDKCodeT>(cDKCodes);
            Excel.ExportToExcelOrTxt(xyjShop, "CDK.txt", new List<string>(), false);
        }
    }
}
