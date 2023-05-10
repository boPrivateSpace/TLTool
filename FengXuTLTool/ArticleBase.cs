using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FengXuTLTool
{
    public static class ArticleBase
    {

        #region check
        public static bool Check(string fileName , ref string msg)
        {
            bool isSure = true;
            StringBuilder str = new StringBuilder();
            str.Append("请将以下文件:");
            foreach (var item in fileName.Split(','))
            {
                string path = Directory.GetCurrentDirectory() + "\\" + item;
                if (!File.Exists(path))
                {
                    str.Append($"\r\n{item}");
                    isSure = false;
                }
            }
            str.Append("放到跟目录下");
            msg = str.ToString();
            return isSure;
        }
        #endregion

        #region wupin zb
        public static List<ArticleList> ArticleLists
        {
            get
            {
                if (_articleLists == null)
                {
                    GetArticleBase();
                }
                return _articleLists;
            }
        }

        private static List<ArticleList> _articleLists;

        private static void GetArticleBase()
        {

            _articleLists = new List<ArticleList>();
            DataTable dt = Excel.TXTToDataTable("CommonItem.txt", string.Empty);
            //循环遍历所有的行，将值赋值给List
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["NAME$1$"].ToString().Contains("未使用") || dt.Rows[i]["NAME$1$"].ToString().Contains("未知") || dt.Rows[i]["NAME$1$"].ToString().Contains("废弃") || string.IsNullOrEmpty(dt.Rows[i]["NAME$1$"].ToString()))
                {
                    continue;
                }
                _articleLists.Add(new ArticleList
                {
                    Index = dt.Rows[i]["INDEX"].ToString(),
                    Name = dt.Rows[i]["NAME$1$"].ToString(),
                });
            }


            DataTable dt1 = Excel.TXTToDataTable("EquipBase.txt", string.Empty);
            //循环遍历所有的行，将值赋值给List
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                try
                {
                    if (dt1.Rows[i].IsNull("NAME(名称)$1$"))
                    {
                        continue;
                    }
                    if (dt1.Rows[i]["NAME(名称)$1$"].ToString().Contains("未使用"))
                    {
                        continue;
                    }

                    if (dt1.Rows[i]["NAME(名称)$1$"].ToString().Contains("废弃"))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(dt1.Rows[i]["NAME(名称)$1$"].ToString()))
                    {
                        continue;
                    }

                    _articleLists.Add(new ArticleList
                    {
                        Index = dt1.Rows[i]["INDEX"].ToString(),
                        Name = dt1.Rows[i]["NAME(名称)$1$"].ToString(),
                    });
                }
                catch
                {

                }
                finally
                {

                }

            }



            Task task2 = Task.Run(() =>
            {
                DataTable dt2 = Excel.TXTToDataTable("GemInfo.txt", string.Empty);
                //循环遍历所有的行，将值赋值给List
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    try
                    {
                        if (dt2.Rows[i].IsNull("名称$1$"))
                        {
                            continue;
                        }

                        _articleLists.Add(new ArticleList
                        {
                            Index = dt2.Rows[i]["INDEX"].ToString(),
                            Name = dt2.Rows[i]["名称$1$"].ToString(),
                        });
                    }
                    catch
                    {

                    }
                    finally
                    {

                    }

                }

            });

        }

        #endregion

        #region guaiw
        public static List<ArticleList> MonsterAttrExs
        {
            get
            {
                if (_monsterAttrExs == null)
                {
                    GetMonsterAttrExBase();
                }
                return _monsterAttrExs;
            }
        }

        private static List<ArticleList> _monsterAttrExs;


        private static void GetMonsterAttrExBase()
        {
            _monsterAttrExs = new List<ArticleList>();
            DataTable dt = Excel.TXTToDataTable("MonsterAttrExTable.txt", string.Empty);
            //循环遍历所有的行，将值赋值给List
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["名称$1$"].ToString().Contains("未使用") || dt.Rows[i]["名称$1$"].ToString().Contains("废弃") || string.IsNullOrEmpty(dt.Rows[i]["名称$1$"].ToString()))
                {
                    continue;
                }
                _monsterAttrExs.Add(new ArticleList
                {
                    Index = dt.Rows[i]["怪物编号"].ToString(),
                    Name = dt.Rows[i]["名称$1$"].ToString(),
                });
            }

        }

        #endregion
    }
}
