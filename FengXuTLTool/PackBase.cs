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
        public Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

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
                if (strLine.Contains('#'))
                {
                    continue;
                }
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
                        if (aryLine[j] != null && aryLine[j].ToString() != "-1" && !string.IsNullOrEmpty(aryLine[j]))
                        {
                            var ss = articleLists.Where(x => x.Index == aryLine[j].ToString()).FirstOrDefault();
                            if (ss != null)
                            {
                                string value = $"({Math.Round(100 / Convert.ToDouble(aryLine[1].ToString()) * 100, 2)}%)";

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

        /// <summary>
        /// 导出html文件
        /// </summary>
        public void GetMonstorPackDrop()
        {
            string message = string.Empty;
            if (!ArticleBase.Check(UsedFileName, ref message))
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
            List<string> mon = new List<string>();
            //逐行读取txt中的数據
            StringBuilder str = new StringBuilder();
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split('\t');//tab分隔符
                if (strLine.Contains('#'))
                {
                    continue;
                }


                var ss = ArticleBase.MonsterAttrExs.Where(x => x.Index == aryLine[0].ToString()).FirstOrDefault();
                if (ss == null)
                {
                    continue;
                }
                if (mon.Contains(ss.Name))
                {
                    continue;
                }

                str.Append($"<tr><td>{ss.Name}</td>");
                mon.Add(ss.Name);

                str.Append("<td>");
                try
                {
                    for (int j = 3; j < columnCount; j++)
                    {
                        if (aryLine[j] != null && aryLine[j].ToString() != "-1")
                        {
                            var cx = keyValuePairs.Where(x => x.Key == aryLine[j]).Select(x => x.Value).FirstOrDefault();
                            if (cx != null)
                            {
                                str.Append(cx +'\t');
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(strLine);
                }

                str.Append("</td></tr>");

                str.AppendLine();

            }

            sr.Close();
            fs.Close();
            Logmsg(str.ToString());
            MessageBox.Show("导出完成");

        }


        /// <summary>
        /// 创建HTML
        /// </summary>
        /// <param name="html">内容</param>
        /// <param name="title">标题</param>
        /// <param name="path">路径</param>
        public void Logmsg(string baolv)
        {
            string title = "爆率";
            string path = Application.StartupPath.ToString();
            string html1 = @"
<!DOCTYPE html >
 <html lang = ""en"" >
    <head >
      <meta http-equiv=""Content-Type"" content = ""text/html; charset=UTF-8"" />     
           <meta name = ""toTop"" content = ""true"" >      
              <meta name = ""viewport"" content = ""width=device-width, initial-scale=1.0"" />
                 <meta http - equiv = ""X-UA-Compatible"" content = ""ie=edge"" />
                      <title> 爆率查看工具 </title>
                      <link rel = ""stylesheet""  href = ""https://www.jq22.com/jquery/bootstrap-4.2.1.css""/>
    <style>
      #toTop {
        line-height: 50px;
        border-radius: 50%; 
        background-color: #fff;
        text-align: center;
        color: black;
        font-size: 25px;              
        box-shadow: 0 3px 5px 0 rgba(0,0,0,.1);
      }
      .content1 {
        margin: 1% auto;
        width: 90%;
      }
      .content1 h2{
        text-align: center;
      }
      .content1 h6{
        text-align: right;
      }
      #lin {
        margin: 20px 0;
        height: 30px;
        padding: 5px;
        box-sizing: border-box;
        display: inherit;
      }

      .names {
        width: 400px;
        margin-bottom: 30px;
      }
      .ad{
        position: sticky;
        top: 0;
      }
    </style>
  </head>
  <body style = """" >
     <div class= ""content1"" >
        <h2> 爆率查看工具 </h2>
           <h6>风絮爆率工具生成</h6>
           <span>
           快捷搜索：
      <input type = ""text"" class= ""form-control names"" id = ""lin"" placeholder = ""输入任意怪物名、怪物ID、物品名模糊搜索"" />
        </span>
        <table class= ""table table-striped table-bordered"" style = ""position: relative;"" id = ""table-1"">
                 <thead class= ""thead-dark ad"" >
                    <tr>
                      <th width = ""20%"" > 怪物名称 </th>
                        <th> 掉落物品 </th>
                      </tr>
                    </thead>
                    <tbody>";
            
            
            
            string html2 = @"</tbody>
                  </table>
       </div>
                <script src = ""https://www.jq22.com/jquery/jquery-1.10.2.js"" ></script>
                 <script src = ""https://www.jq22.com/jquery/bootstrap-4.2.1.js"" ></script>
                  <script src = ""https://www.jq22.com/demo/jquerytablesearch202003060001/lin_search.js"" ></script>
                   <script >
                     jQuery(document).ready(function($) {
                if ($(""meta[name = toTop]"").attr(""content"") == ""true"") {
              $("" <div id = 'toTop' title = '返回顶部' >▲</div> "").appendTo('body');
              $(""#toTop"").css({
        width: '50px',
                  height: '50px',
                  bottom: '10px',
                  right: '15px',
                  position: 'fixed',
                  cursor: 'pointer',
                  zIndex: '999999',
              });
                if ($(this).scrollTop() == 0) {
                  $(""#toTop"").hide();
        }
              $(window).scroll(function(event) {
            /* Act on the event */
            if ($(this).scrollTop() == 0) {
                      $(""#toTop"").hide();
            }
            if ($(this).scrollTop() != 0) {
                      $(""#toTop"").show();
            }
        });
              $(""#toTop"").click(function(event) {
                  /* Act on the event */
                  $(""html,body"").animate({
        scrollTop: ""0px""
                      },
                      666
                  )
              });
    }
});
      $(""#lin"").on(""keyup"", function() {
    var table1 = $(""#table-1"");
    var input = $(this);
new Search(table1, input);
});
    </script>
  </body>
</html>";


            string htmlString = html1 + baolv  + html2;
            #region 创建HTMLif (Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }

            FileStream fs1 = new FileStream(path + "/" + title + DateTime.Now.ToString("yyyyMMddhhmmss") + ".html", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);

            sw.WriteLine(htmlString);//开始写入值
            sw.Close();
            fs1.Close();
            #endregion
        }

        #region 导出爆率文件配合查看爆率

        /// <summary>
        /// 导出爆率文件配合查看爆率
        /// </summary>
        public void GetMonstorPackDropLua()
        {
            string message = string.Empty;
            if (!ArticleBase.Check(UsedFileName, ref message))
            {
                MessageBox.Show(message);
            }
            GetPackDropByItemId();
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
            StringBuilder str = new StringBuilder();
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split('\t');//tab分隔符
                if (strLine.Contains('#'))
                {
                    continue;
                }

                MonsterDrop drop = new MonsterDrop();
                var ss = ArticleBase.MonsterAttrExs.Where(x => x.Index == aryLine[0].ToString()).FirstOrDefault();
                if (ss == null)
                {
                    continue;
                }
                str.Append($"[{ss.Index}] = {{");

                try
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int j = 3; j < columnCount; j++)
                    {
                        if (aryLine[j] != null && aryLine[j].ToString() != "-1")
                        {
                            var cx = keyValuePairs.Where(x => x.Key == aryLine[j]).Select(x => x.Value).FirstOrDefault();
                            if (cx != null )
                            {
                                stringBuilder.Append($"{cx}|");
                            }
                        }
                    }

                    var itemLIst = GetDropItem(stringBuilder.ToString().TrimEnd('|'));
                    int i = 0;
                    foreach(var item in itemLIst)
                    {
                        str.Append(item);
                        if (i < itemLIst.Count - 1)
                        {
                            str.Append(",");
                        }
                        i++;
                    }
                    str.Append("},\n");
                }
                catch (Exception e)
                {
                    MessageBox.Show(strLine);
                }

            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls|文本文件(*.txt)|*.txt|*.dll|";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = false;
            //saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "导出文件为...";
            saveFileDialog.FileName = "FengXu_DropItem.txt";
            DialogResult dr = saveFileDialog.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }

            FileStream fs1 = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);

            sw.WriteLine(str);//开始写入值
            sw.Close();
            fs1.Close();
            sr.Close();
            fs.Close();

            MessageBox.Show("导出成功");


        }

        private HashSet<string> GetDropItem(string dropItem)
        {
            string[] items = dropItem.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> uniqueItems = new HashSet<string>(items);

            return uniqueItems;
        }

        public Dictionary<string, string> DropItem = new Dictionary<string, string>();


        public void GetPackDropByItemId()
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
                if (strLine.Contains('#'))
                {
                    continue;
                }
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
                        if (aryLine[j] != null && aryLine[j].ToString() != "-1" && !string.IsNullOrEmpty(aryLine[j]))
                        {
                            var ss = articleLists.Where(x => x.Index == aryLine[j].ToString()).FirstOrDefault();
                            if (ss != null)
                            {
                                str.Append(ss.Index +'|');
                            }
                        }
                    }
                }

                if (!keyValuePairs.ContainsKey(Id))
                {
                    keyValuePairs.Add(Id, str.ToString().TrimEnd('|'));
                }
            }

            sr.Close();
            fs.Close();

        }
        #endregion
    }

}
