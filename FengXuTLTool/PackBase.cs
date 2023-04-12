using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FengXuTLTool
{
    public class PackBase
    {
        public

        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        private string UsedFileName
        {
            get 
            {
                return "DropBoxContent.txt,MonsterDropBoxs.txt,CommonItem.txt,EquipBase.txt,MonsterAttrExTable.txt";
            }
        }

        public void GetPackDrop()
        {
            var articleLists = ArticleBase.ArticleLists;

            string fileName = "DropBoxContent.txt";
             DataTable dt = new DataTable();
            string path = Directory.GetCurrentDirectory() + "\\" + fileName;
            if (!File.Exists(path))
            {
                return;
            }

            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记錄
            string strLine = "";

            //記錄每行記錄中的各字段内容
            string[] aryLine;
            //标示列数           
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;

            if (IsFirst == true)
            {
                sr.ReadLine(); //说明txt包含列名时，不传入列名，用过读取获得列名,一般用tab分隔
                strLine = sr.ReadLine();
                aryLine = strLine.Split('\t');//tab分隔符
                IsFirst = false;
                columnCount = aryLine.Length;
            }
            // 去掉空余行
            sr.ReadLine();
            //逐行读取txt中的数據
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split('\t');//tab分隔符
                string Id = string.Empty;
                StringBuilder str = new StringBuilder();
                for (int j = 0; j < columnCount; j++)
                {
                    if (j == 0)
                    {
                        Id = aryLine[j].ToString();
                    }
                    else
                    {
                        if (aryLine[j] != null && aryLine[j].ToString() != "-1")
                        {
                            var ss = articleLists.Where(x => x.Index == aryLine[j].ToString()).FirstOrDefault();
                            if (ss!=null)
                            {
                                str.Append(ss.Name + '\t');
                            }
                        }
                    }
                }

                if (!keyValuePairs.ContainsKey(Id))
                {
                    keyValuePairs.Add(Id, str.ToString());
                }
            }

            sr.Close();
            fs.Close();


        }

        public void GetMonstorPackDrop()
        {
            string message = string.Empty;
            if (!ArticleBase.Check(UsedFileName,ref message))
            {
                MessageBox.Show(message);
                return;
            }
            GetPackDrop();
            var articleLists = ArticleBase.ArticleLists;

            string fileName = "MonsterDropBoxs.txt";
            DataTable dt = new DataTable();
            string path = Directory.GetCurrentDirectory() + "\\" + fileName;
            if (!File.Exists(path))
            {
                return;
            }
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记錄
            string strLine = "";

            //記錄每行記錄中的各字段内容
            string[] aryLine;
            //标示列数           
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;

            if (IsFirst == true)
            {
                sr.ReadLine(); //说明txt包含列名时，不传入列名，用过读取获得列名,一般用tab分隔
                strLine = sr.ReadLine();
                aryLine = strLine.Split('\t');//tab分隔符
                IsFirst = false;
                columnCount = aryLine.Length;
            }
            // 去掉空余行
            sr.ReadLine();
            //逐行读取txt中的数據
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split('\t');//tab分隔符
                if (strLine.Contains('#'))
                {
                    continue;
                }
                StringBuilder str = new StringBuilder();
                try
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (j == 1 || j == 2)
                        {
                            continue;
                        }
                        if (aryLine[j] != null && aryLine[j].ToString() != "-1")
                        {
                            var ss = ArticleBase.MonsterAttrExs.Where(x => x.Index == aryLine[j].ToString()).FirstOrDefault();
                            if (ss != null && j == 0)
                            {
                                str.Append(ss.Name + '\t');
                                continue;
                            }
                            var cx = keyValuePairs.Where(x => x.Key == aryLine[j]).Select(x => x.Value).FirstOrDefault();
                            if (cx != null)
                            {
                                str.Append(cx + '\t');
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show(strLine);
                }


                if (!string.IsNullOrEmpty(str.ToString()))
                {
                    //第二种方法，比较简单
                    //\r\n要加在前面才会换行！
                    if (!File.Exists("爆率.txt"))
                    {
                        File.AppendAllText("爆率.txt", str.ToString());
                    }
                    else
                    {
                        File.AppendAllText("爆率.txt", "\r\n" + str.ToString());
                    }
                }

            }

            sr.Close();
            fs.Close();

            MessageBox.Show("导出完成");

        }
    }

}
