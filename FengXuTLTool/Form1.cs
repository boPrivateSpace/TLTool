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

        public string Index = "";
        public string Name = "";
        public int Num = 0;
        private FileModel _fileModel;
        private List<ArticleList> _articleLists = new List<ArticleList>();

        public void InitColumn()
        {
            this.grdvList.OptionsView.ShowGroupPanel = false;
            this.grdvList.OptionsBehavior.Editable = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _fileModel = new FileModel();
            var fileModel = JsonFile.ReadJsonFile();
            if (fileModel != null)
            {
                _fileModel = fileModel;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_fileModel.Ip))
                {
                    MessageBox.Show("未设置数据库连接");
                }
                SftpClient sftp = new SftpClient(_fileModel.Ip, 22, _fileModel.UserName, _fileModel.Pwd);//创建sftp对象
                sftp.Connect();
                var path = _fileModel.FileName;
                FileInfo file = new FileInfo(path);
                Stream uploda = new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                sftp.UploadFile(uploda, _fileModel.FilePath + "/" + path);
                sftp.Dispose();//释放连接对象

                file.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show("上传失败");
            }


        }


        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (_fileModel == null || _fileModel.FileName == null)
            {
                MessageBox.Show("未设置文件名称");
                return;
            }

            ArticleList dr = (ArticleList)this.grdvList.GetFocusedRow();
            if (dr == null)
            {
                return;
            }
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(getRandomDom());
            stringBuilder.Append("             ");
            stringBuilder.Append(dr.Index);
            stringBuilder.Append("             ");
            stringBuilder.Append(string.IsNullOrEmpty(this.edtNum.Text)?"1": this.edtNum.Text);


            //第二种方法，比较简单
            //\r\n要加在前面才会换行！
            if (!File.Exists(_fileModel.FileName))
            {
                File.AppendAllText(_fileModel.FileName, stringBuilder.ToString());
            }
            else
            {
                File.AppendAllText(_fileModel.FileName, "\r\n" + stringBuilder.ToString());
            }
        }


        /// <summary>
        /// 生成不重复随机字符串
        /// </summary>
        /// <param name="count">输入字符串长度</param>
        /// <returns>字符串</returns>
        public static string getRandomDom(int count = 11)
        {
            string t62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            long ticks = DateTime.Now.Ticks;

            string gen = "";
            int ind = 0;
            while (ind < count)
            {
                byte low = (byte)((ticks >> ind * 6) & 61);
                gen += t62[low];
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
    }




}
