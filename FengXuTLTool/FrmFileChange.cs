using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FengXuTLTool
{
    public partial class FrmFileChange : Form
    {
        public FrmFileChange()
        {
            InitializeComponent();
        }

        string[] files;//选择文件的集合
        FileInfo fi;//创建一个FileInfo对象，用于获取文件信息
        string[] lvFiles = new string[7];//向控件中添加的行信息
        Thread td;//处理批量更名方法的线程
        string _dictionary =string.Empty;//源文件位置
        string _newDictionary = string.Empty;//导出文件位置

        private void 添加文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            dialog.SelectedPath = "C:\\";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获得当前文件夹下的文件

                _dictionary = dialog.SelectedPath;

                List<string> fileWithPath  = getFiles(dialog.SelectedPath);
                SetFileToListView(fileWithPath);
            }
        }
        /// <summary>
        /// 获取最终需要的文件夹路径字符串集合
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private List<string> getFiles(string folderName, bool useOnlyName = false)
        {
            List<string> fileNameList = new List<string>() { };
            DirectoryInfo theFolder = new DirectoryInfo(folderName);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();

            fileNameList = getFileList(dirInfo, fileNameList, useOnlyName);
            FileInfo[] fileInfo = theFolder.GetFiles();
            for (int i = 0; i < fileInfo.Length; i++)
            {
                if (useOnlyName)
                {
                    fileNameList.Add(fileInfo[i].Name);
                }
                else
                {
                    fileNameList.Add(fileInfo[i].FullName);
                }
            }
            return fileNameList;
        }

        /// <summary>
        /// 递归获取文件夹里的所有文件（包括子文件夹内的文件）
        /// </summary>
        /// <param name="dirInfos"></param>
        /// <param name="fileNameList"></param>
        /// <returns></returns>
        private List<string> getFileList(DirectoryInfo[] dirInfos, List<string> fileNameList, bool useOnlyName = false)
        {
            foreach (DirectoryInfo sonFolder in dirInfos)
            {
                FileInfo[] fileInfo = sonFolder.GetFiles();
                for (int i = 0; i < fileInfo.Length; i++)
                {
                    if (useOnlyName)
                    {
                        fileNameList.Add(fileInfo[i].Name);
                    }
                    else
                    {
                        fileNameList.Add(fileInfo[i].FullName);
                    }

                }
                if (sonFolder.GetDirectories() != null)
                {
                    getFileList(sonFolder.GetDirectories(), fileNameList, useOnlyName);
                }
            }
            return fileNameList;
        }

        private void SetFileToListView(List<string> fileList)
        {

            listView1.GridLines = true;
            listView1.Items.Clear();
            foreach (var item in fileList)
            {
                string path = item;
                fi = new FileInfo(path);
                string name = path.Substring(path.LastIndexOf("\\") + 1, path.Length - 1 - path.LastIndexOf("\\"));
                string ftype = path.Substring(path.LastIndexOf("."), path.Length - path.LastIndexOf("."));
                string createTime = fi.CreationTime.ToShortDateString();
                double a = Convert.ToDouble(Convert.ToDouble(fi.Length) / Convert.ToDouble(1024));
                string fsize = a.ToString("0.0") + " KB";
                lvFiles[0] = name;
                lvFiles[1] = name;
                lvFiles[2] = ftype;
                lvFiles[3] = createTime;
                lvFiles[4] = path.Remove(path.LastIndexOf("\\") + 1);
                lvFiles[5] = fsize;

                ListViewItem lvi = new ListViewItem(lvFiles);
                lvi.UseItemStyleForSubItems = false;
                lvi.SubItems[1].BackColor = Color.AliceBlue;

                listView1.Items.Add(lvi);
            }

            tsslSum.Text = listView1.Items.Count.ToString();

            //for (var d = 0; d < dirstr.Length; d++)
            //{
            //    files = Directory.GetFiles(dirstr[d] + "\\");
            //    for (int i = 0; i < files.Length; i++)
            //    {
            //        string path = files[i].ToString();
            //        fi = new FileInfo(path);
            //        string name = path.Substring(path.LastIndexOf("\\") + 1, path.Length - 1 - path.LastIndexOf("\\"));
            //        string ftype = path.Substring(path.LastIndexOf("."), path.Length - path.LastIndexOf("."));
            //        string createTime = fi.CreationTime.ToShortDateString();
            //        double a = Convert.ToDouble(Convert.ToDouble(fi.Length) / Convert.ToDouble(1024));
            //        string fsize = a.ToString("0.0") + " KB";
            //        lvFiles[0] = name;
            //        lvFiles[1] = name;
            //        lvFiles[2] = ftype;
            //        lvFiles[3] = createTime;
            //        lvFiles[4] = path.Remove(path.LastIndexOf("\\") + 1);
            //        lvFiles[5] = fsize;
            //        lvFiles[7] = fi.Directory.Name;

            //        ListViewItem lvi = new ListViewItem(lvFiles);
            //        lvi.UseItemStyleForSubItems = false;
            //        lvi.SubItems[1].BackColor = Color.AliceBlue;

            //        listView1.Items.Add(lvi);
            //    }
            //}


        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)//文件名大写
        {
            if (listView1.Items.Count > 0)
            {
                if (radioButton1.Checked)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        string name = listView1.Items[i].SubItems[1].Text;
                        string name1 = name.Remove(name.LastIndexOf("."));
                        string newName = name.Replace(name1, name1.ToUpper());
                        listView1.Items[i].SubItems[1].Text = newName;
                    }
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)//文件名小写
        {
            if (listView1.Items.Count > 0)
            {
                if (radioButton2.Checked)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        string name = listView1.Items[i].SubItems[1].Text;
                        string name1 = name.Remove(name.LastIndexOf("."));
                        string newName = name.Replace(name1, name1.ToLower());
                        listView1.Items[i].SubItems[1].Text = newName;
                    }
                }
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)//第一个字母大写
        {
            if (listView1.Items.Count > 0)
            {
                if (radioButton3.Checked)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        string name = listView1.Items[i].SubItems[1].Text;
                        string name1 = name.Substring(0, 1);
                        string name2 = name.Substring(1);
                        string newName = name1.ToUpper() + name2;
                        listView1.Items[i].SubItems[1].Text = newName;
                    }
                }
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)//扩展名大写
        {
            if (listView1.Items.Count > 0)
            {
                if (radioButton4.Checked)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        string name = listView1.Items[i].SubItems[1].Text;
                        string name1 = name.Substring(name.LastIndexOf("."), name.Length - name.LastIndexOf("."));
                        string newName = name.Replace(name1, name1.ToUpper());
                        listView1.Items[i].SubItems[1].Text = newName;
                    }
                }
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                if (radioButton5.Checked)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        string name = listView1.Items[i].SubItems[1].Text;
                        string name1 = name.Substring(name.LastIndexOf("."), name.Length - name.LastIndexOf("."));
                        string newName = name.Replace(name1, name1.ToLower());
                        listView1.Items[i].SubItems[1].Text = newName;
                    }
                }
            }
        }


        private void StartNumAndAdd()//设置起始数字和增量值
        {
            if (listView1.Items.Count > 0)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    string name = listView1.Items[i].SubItems[1].Text;
                    string name1 = name.Remove(name.LastIndexOf("."));
                    // 截取后10位
                    try
                    {
                        int num = 0;
                        string name2 = string.Empty;
                        bool isContinue = true;
                        bool isChange = false;
                        while (isContinue)
                        {
                            // 尝试转换后10 位 成功的话结束循环
                            string endNum = name1.Substring(name1.Length - 10);
                            if (int.TryParse(endNum, out num))
                            {
                                num = Convert.ToInt32(endNum) + (int)nuAdd.Value;
                                name2 = name1.Remove(name1.Length - 10, 10) + num;
                                isContinue = false;
                                isChange = true;
                                continue;
                            }

                            // 尝试转换前10 位 成功的话结束循环
                            string fristNum = name1.Substring(0,10);
                            if (int.TryParse(fristNum, out num))
                            {
                                num = Convert.ToInt32(fristNum) + (int)nuAdd.Value;
                                name2 = num +  name1.Substring(10)  ;
                                isContinue = false;
                                isChange = true;

                                continue;
                            }

                            // 处理带_符号的 成功的话结束循环
                            string LNum = name1.Split('_')[0];
                            string endLNum = LNum.Substring(LNum.Length - 10);
                            if (int.TryParse(endLNum, out num))
                            {
                                num = Convert.ToInt32(endLNum) + (int)nuAdd.Value;
                                name2 = LNum.Substring(0, LNum.Length - 10)  + num+ "_"+ name1.Split('_')[1];
                                isContinue = false;
                                isChange = true;

                                continue;
                            }
                            isContinue = false;

                        }
                        string newName = name1;

                        if (isChange)
                        {
                            newName = name.Replace(name1, name2);
                        }
                        listView1.Items[i].SubItems[1].Text = newName;
                    }
                    catch
                    {
                    }
                }
            }
        }


        private void nuStart_ValueChanged(object sender, EventArgs e)//选择起始数字
        {
            StartNumAndAdd();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            StartNumAndAdd();
        }



        private void ChangeName()
        {
            int flag = 0;
            try
            {
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = listView1.Items.Count - 1;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    string path = listView1.Items[i].SubItems[4].Text;

                    string pathFile = path.Substring(_dictionary.Length);
                    string newDic = _newDictionary + pathFile;
                    if (!Directory.Exists(newDic))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(newDic);
                        directoryInfo.Create();
                    }


                    string sourcePath = path + listView1.Items[i].SubItems[0].Text;
                    string newPath = newDic + listView1.Items[i].SubItems[1].Text;
                    File.Copy(sourcePath, newPath);
                    toolStripProgressBar1.Value = i;
                    listView1.Items[i].SubItems[0].Text = listView1.Items[i].SubItems[1].Text;
                    listView1.Items[i].SubItems[6].Text = "√成功";
                }
            }
            catch (Exception ex)
            {
                flag++;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                tsslError.Text = flag.ToString() + " 个错误";
            }
        }

        private void 更名ToolStripMenuItem_Click(object sender, EventArgs e)//开始批量更名
        {


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //private static string TraditionalChineseToSimplifiedChinese(string str)//繁体转简体
        //{
        //    return (Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0));
        //}

        //private static string SimplifiedChineseToTraditionalChinese(string str)//简体转繁体
        //{
        //    return (Microsoft.VisualBasic.Strings.StrConv(str as string, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0));
        //}

        private void 繁体转简体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (listView1.Items.Count > 0)
            //{
            //    for (int i = 0; i < listView1.Items.Count; i++)
            //    {
            //        string name = listView1.Items[i].SubItems[1].Text;
            //        string name1 = TraditionalChineseToSimplifiedChinese(name);
            //        listView1.Items[i].SubItems[1].Text = name1;
            //    }
            //}
        }

        private void 简体转繁体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (listView1.Items.Count > 0)
            //{
            //    for (int i = 0; i < listView1.Items.Count; i++)
            //    {
            //        string name = listView1.Items[i].SubItems[1].Text;
            //        string name1 = SimplifiedChineseToTraditionalChinese(name);
            //        listView1.Items[i].SubItems[1].Text = name1;
            //    }
            //}
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (td != null)
            {
                td.Abort();
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AboutBox1 ab = new AboutBox1();
            //ab.ShowDialog();
        }

        private void 导出文件位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            dialog.SelectedPath = "C:\\";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获得当前文件夹下的文件

                _newDictionary = dialog.SelectedPath;
            }
        }

        private void 更名PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_newDictionary))
            {
                MessageBox.Show("请选择导出位置");
                return;
            }
            if (listView1.Items.Count > 0)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[6].Text = "";
                }
                tsslError.Text = "";
                td = new Thread(new ThreadStart(ChangeName));
                td.Start();
            }
        }
    }
}
