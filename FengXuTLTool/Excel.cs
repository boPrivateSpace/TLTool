using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace FengXuTLTool
{
    public partial class Excel 
    {
        public DataSet getData()
        {
            //判断文件后缀
            var path = "CommonItem.xls";
            string fileSuffix = System.IO.Path.GetExtension(path);
            if (string.IsNullOrEmpty(fileSuffix))
                return null;
            using (DataSet ds = new DataSet())
            {
                //判断Excel文件是2003版本还是2007版本
                string connString = "";
                if (fileSuffix == ".xls")
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                else
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
                //读取文件
                string sql_select = " SELECT * FROM [Sheet1$]";
                using (OleDbConnection conn = new OleDbConnection(connString))
                using (OleDbDataAdapter cmd = new OleDbDataAdapter(sql_select, conn))
                {
                    conn.Open();
                    cmd.Fill(ds);
                }
                if (ds == null || ds.Tables.Count <= 0) return null;
                return ds;
            }
        }

        //删除空行
        public static void RemoveEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool IsNull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {
                        IsNull = false;
                    }
                }
                if (IsNull)
                {
                    removelist.Add(dt.Rows[i]);
                }
            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
        }



        public static DataTable TXTToDataTable(string fileName, string columnName)
        {
            DataTable dt = new DataTable();
            string path = Directory.GetCurrentDirectory() + "\\" + fileName;
            if (!File.Exists(path))
            {
                return new DataTable();
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
                if (columnName == "")
                {
                    sr.ReadLine(); // skip
                    //说明txt包含列名时，不传入列名，用过读取获得列名,一般用tab分隔
                    strLine = sr.ReadLine();
                    aryLine = strLine.Split('\t');//tab分隔符
                }
                else
                {
                    strLine = columnName;
                    aryLine = strLine.Split(',');//传入列名用逗号分隔
                }

                IsFirst = false;
                columnCount = aryLine.Length;
                //创建列
                for (int i = 0; i < columnCount; i++)
                {
                    if (!dt.Columns.Contains(aryLine[i].ToUpper()))
                    {
                        DataColumn dc = new DataColumn(aryLine[i].ToUpper());
                        dt.Columns.Add(dc);
                    }
                }
            }
            // 去掉空余行
            sr.ReadLine();
            //逐行读取txt中的数據
            try
            {
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (strLine.StartsWith("#"))
                    {
                        continue;
                    }
                    aryLine = strLine.Split('\t');//tab分隔符
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j].ToUpper();
                    }
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(strLine);
            }


            sr.Close();
            fs.Close();
            return dt;
        }


    }
}