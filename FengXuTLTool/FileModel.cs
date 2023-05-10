using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FengXuTLTool
{
    public class FileModel
    {
        public string Ip { get; set; }

        public string UserName { get; set; }

        public string Pwd { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// 登录标识
        /// </summary>
        public string LoginCode { get; set; }

    }


    public class LoginCode
    {
        public string Code { get; set; }
    }

    public class ArticleList
    {
        public string Index { get; set; }

        public string Name { get; set; }
    }


    public class CDKCode
    {
        public string Code { get; set; }

        public string Index { get; set; }

        public string Num { get; set; }

    }



    public class XYJShop
    {
        public string index { get; set; }
        public string 商店标识 { get; set; }
        public string nMenuIndex  { get; set; }
        public string nShopIndex { get; set; }
        public string 物品INDEX { get; set; }
        public string 物品ITEMID { get; set; }

        public string 物品数量 { get; set; }
        public string 物品价格 { get; set; }

        public string 物品折扣比 { get; set; }
        public string 商品特殊标记 { get; set; }
        public string 颜色显示 { get; set; }

    }
}
