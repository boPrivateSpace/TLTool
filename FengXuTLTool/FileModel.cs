using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FengXuTLTool
{
    public   class FileModel
    {
        public  string Ip { get; set; }

        public  string UserName { get; set; }

        public  string Pwd { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }
    }

    public class ArticleList
    {
        public string Index { get; set; }

        public string Name { get; set; }
    }
}
