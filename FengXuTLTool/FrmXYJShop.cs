using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xQuant.UI.Assist;

namespace FengXuTLTool
{
    public partial class FrmXYJShop : Form
    {
        public FrmXYJShop()
        {
            InitializeComponent();
            Init();
        }
        private List<ArticleList> _articleLists = new List<ArticleList>();
        private List<XYJShop> _xyjShop = new List<XYJShop>();
        private string _Index = "1";
        private string _MenueIndex = "1";
        private string _ShopIndex = "1";

        private string UsedFileName
        {
            get
            {
                return "XYJ_ShopTable.txt,CommonItem.txt,EquipBase.txt,GemInfo.txt";
            }
        }


        private void Init()
        {
            textEdit1.Text = "";
            textEdit1.Properties.ReadOnly = true;
            GridUtils.GridBeginUpdate(this.grdvList);
            try
            {
                this.grdvList.Columns.Clear();
                this.grdvList.OptionsView.ShowFooter = false;
                this.grdvList.OptionsView.ShowGroupPanel = false;
                this.grdvList.OptionsView.ColumnAutoWidth = true;
                this.grdvList.OptionsBehavior.Editable = false;

                GridUtils.CreateGridColumn(this.grdvList, "物品INDEX", "物品INDEX");
                GridUtils.CreateGridColumn(this.grdvList, "物品ITEMID", "物品ITEMID");
                GridUtils.CreateGridColumn(this.grdvList, "物品名称", "物品名称");
                GridUtils.CreateGridColumn(this.grdvList, "物品数量", "物品数量");
                GridUtils.CreateGridColumn(this.grdvList, "物品价格", "物品价格");

                this.grdvList.CustomColumnDisplayText += new CustomColumnDisplayTextEventHandler(this.grdvList_CustomColumnDisplayText);
            }
            finally
            {
                GridUtils.GridEndUpdate(this.grdvList);
            }


            GridUtils.GridBeginUpdate(this.grdvEq);
            try
            {
                this.grdvEq.Columns.Clear();
                this.grdvEq.OptionsView.ShowFooter = false;
                this.grdvEq.OptionsView.ShowGroupPanel = false;
                this.grdvEq.OptionsView.ColumnAutoWidth = true;
                this.grdvEq.OptionsBehavior.Editable = false;

                GridUtils.CreateGridColumn(this.grdvEq, "物品编号", "Index");
                GridUtils.CreateGridColumn(this.grdvEq, "物品名称", "Name");

            }
            finally
            {
                GridUtils.GridEndUpdate(this.grdvEq);
            }

        }

        /// <summary>
        /// 界面优化默认显示的保证金为-1时 显示计算中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdvList_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            XYJShop xYJShop = this.grdvList.GetRow(e.RowHandle) as XYJShop;
            if (xYJShop == null)
            {
                return;
            }

            if (e.Column.FieldName == "物品名称")
            {
                e.DisplayText = _articleLists.Where(x => x.Index == xYJShop.物品ITEMID).Select(x=>x.Name).FirstOrDefault();
            }
        }


        private void GridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            GridView view = sender as GridView;

            //用户在表格列头、资料行、分组区域点击鼠标弹出菜单
            if (GridMenuType.Column == e.MenuType || GridMenuType.Row == e.MenuType || GridMenuType.Group == e.MenuType)
            {
                var item1 = GridUtils.CreatePopupMenuItem(e.Menu.Items, "删除", null, true, btnDelete_Click);
                var item2 = GridUtils.CreatePopupMenuItem(e.Menu.Items, "向上移动一行", null, false, btnMove_Clice);

            }
        }



        private void FrmXYJShop_Load(object sender, EventArgs e)
        {
            grdvList.PopupMenuShowing += GridView1_PopupMenuShowing;

            this.Index_1.Click += new System.EventHandler(this.btnCheck_Click);
            this.Index_2.Click += new System.EventHandler(this.btnCheck_Click);
            this.Index_3.Click += new System.EventHandler(this.btnCheck_Click);
            this.Index_4.Click += new System.EventHandler(this.btnCheck_Click);
            this.Index_5.Click += new System.EventHandler(this.btnCheck_Click);
            this.MenuIndex_1.Click += new System.EventHandler(this.btnCheck_Click);
            this.MenuIndex_2.Click += new System.EventHandler(this.btnCheck_Click);
            this.MenuIndex_3.Click += new System.EventHandler(this.btnCheck_Click);
            this.Layout4.Click += new System.EventHandler(this.btnCheck_Click);
            this.MenuIndex_5.Click += new System.EventHandler(this.btnCheck_Click);
            this.MenuIndex_6.Click += new System.EventHandler(this.btnCheck_Click);
            this.MenuIndex_7.Click += new System.EventHandler(this.btnCheck_Click);
            this.MenuIndex_8.Click += new System.EventHandler(this.btnCheck_Click);
            this.ShopIndex_1.Click += new System.EventHandler(this.btnCheck_Click);
            this.ShopIndex_2.Click += new System.EventHandler(this.btnCheck_Click);
            this.ShopIndex_3.Click += new System.EventHandler(this.btnCheck_Click);
            this.ShopIndex_4.Click += new System.EventHandler(this.btnCheck_Click);
            this.ShopIndex_5.Click += new System.EventHandler(this.btnCheck_Click);
            this.ShopIndex_6.Click += new System.EventHandler(this.btnCheck_Click);
            this.ShopIndex_7.Click += new System.EventHandler(this.btnCheck_Click);
            this.ShopIndex_8.Click += new System.EventHandler(this.btnCheck_Click);

        }

        private void btnAddW_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.ShowDialog();
            this.edtWpIndex.EditValue = form1.Index;
            this.edtName.EditValue = form1.Name;
            this.edtNum.EditValue = form1.Num;
        }


        private void btnCheck_Click(object sender, EventArgs e)
        {
            var btnName = ((SimpleButton)sender).Name;
            var type = btnName.Split('_')[0];
            var value = btnName.Split('_')[1];

            List<XYJShop> shops = new List<XYJShop>();
            switch (type)
            {
                case "Index":
                    _Index = value;
                    IndexCheck(((SimpleButton)sender));
                    break;
                case "MenuIndex":
                    _MenueIndex = value;
                    MenIndexCheck(((SimpleButton)sender));

                    break;
                case "ShopIndex":
                    _ShopIndex = value;
                    ShopCheck(((SimpleButton)sender));
                    break;
            }
            RefreshDataSource();
        }


        private void IndexCheck(SimpleButton button)
        {
            this.Index_1.ForeColor = Color.Black;
            this.Index_2.ForeColor = Color.Black;
            this.Index_3.ForeColor = Color.Black;
            this.Index_4.ForeColor = Color.Black;
            this.Index_5.ForeColor = Color.Black;
            this.Index_6.ForeColor = Color.Black;

            button.ForeColor = Color.Red;
        }

        private void MenIndexCheck(SimpleButton button)
        {
            this.MenuIndex_1.ForeColor = Color.Black;
            this.MenuIndex_2.ForeColor = Color.Black;
            this.MenuIndex_3.ForeColor = Color.Black;
            this.MenuIndex_4.ForeColor = Color.Black;
            this.MenuIndex_5.ForeColor = Color.Black;
            this.MenuIndex_6.ForeColor = Color.Black;
            this.MenuIndex_7.ForeColor = Color.Black;
            this.MenuIndex_8.ForeColor = Color.Black;



            button.ForeColor = Color.Red;
        }
        
        private void ShopCheck(SimpleButton button)
        {
            this.ShopIndex_1.ForeColor = Color.Black;
            this.ShopIndex_2.ForeColor = Color.Black;
            this.ShopIndex_3.ForeColor = Color.Black;
            this.ShopIndex_4.ForeColor = Color.Black;
            this.ShopIndex_5.ForeColor = Color.Black;
            this.ShopIndex_6.ForeColor = Color.Black;
            this.ShopIndex_7.ForeColor = Color.Black;
            this.ShopIndex_8.ForeColor = Color.Black;

            button.ForeColor = Color.Red;

        }


        private void RefreshEqDataSource(List<ArticleList> articleLists)
        {
            grdcEq.DataSource = articleLists;
            grdcEq.RefreshDataSource();
        }
        private void RefreshDataSource()
        {
            grdcList.DataSource = _xyjShop.Where(x => x.nMenuIndex == _MenueIndex && x.nShopIndex == _ShopIndex && x.商店标识 == _Index).ToList();
            grdcList.RefreshDataSource();
        }



        private void btnMove_Clice(object sender, EventArgs e)
        {
            XYJShop xYJShop = (XYJShop)this.grdvList.GetFocusedRow();
            if (xYJShop == null)
            {
                return;
            }

            var num = Convert.ToInt32(xYJShop.物品INDEX);
            var nextxyjShop = _xyjShop.Where(x => x.商店标识 == xYJShop.商店标识 && x.nMenuIndex == xYJShop.nMenuIndex && x.nShopIndex == xYJShop.nShopIndex && Convert.ToInt32(xYJShop.物品INDEX) < num).OrderByDescending(x => x.物品INDEX).FirstOrDefault();
            xYJShop.物品INDEX = (Convert.ToInt32(xYJShop.物品INDEX) - 1).ToString();
            nextxyjShop.物品INDEX = (Convert.ToInt32(xYJShop.物品INDEX) + 1).ToString();
            RefreshDataSource();
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            XYJShop xYJShop = (XYJShop)this.grdvList.GetFocusedRow();
            if (xYJShop == null)
            {
                return;
            }
            //xYJShop.商店标识 = edtIndex1.EditValue.ToString();
            //xYJShop.nMenuIndex = edtnMenuIndex.EditValue.ToString();
            //xYJShop.nShopIndex = edtnShopIndex.EditValue.ToString();
            //xYJShop.物品INDEX = edtSortNum.EditValue.ToString();
            xYJShop.物品ITEMID = edtWpIndex.EditValue.ToString();
            xYJShop.物品数量 = edtNum.EditValue.ToString();
            xYJShop.物品价格 = edtPrice.EditValue.ToString();

            RefreshDataSource();

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            string message = string.Empty;
            if (!ArticleBase.Check(UsedFileName, ref message))
            {
                MessageBox.Show(message);
                return;
            }


            DataTable dt = Excel.TXTToDataTable("XYJ_ShopTable.txt", string.Empty);
            _articleLists = ArticleBase.ArticleLists;
            _xyjShop = ModelTool.DataTableConvertToModel<XYJShop>(dt);
            this.Index_1.ForeColor = Color.Red;
            this.MenuIndex_1.ForeColor = Color.Red;
            this.ShopIndex_1.ForeColor = Color.Red;


            RefreshDataSource();

        }


        private void grdvList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            XYJShop dr = (XYJShop)this.grdvList.GetFocusedRow();
            if (dr == null)
            {
                return;
            }


            edtWpIndex.EditValue = dr.物品ITEMID;
            edtName.EditValue = _articleLists.Where(x => x.Index == dr.物品ITEMID.ToString()).Select(x => x.Name).FirstOrDefault();
            edtNum.EditValue = dr.物品数量;
            edtPrice.EditValue = dr.物品价格;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            XYJShop dr = (XYJShop)this.grdvList.GetFocusedRow();
            if (dr == null)
            {
                return;
            }

            _xyjShop.Remove(dr);

            int i = 1;
            foreach (var item in _xyjShop.Where(x => x.商店标识 == dr.商店标识 && x.nMenuIndex == dr.nMenuIndex && x.nShopIndex == dr.nShopIndex))
            {
                item.物品INDEX = i.ToString();
                i++;
            }

            RefreshDataSource();

        }


        private void btnSaveFile_Click(object sender, EventArgs e)
        {

            List<string> columns = new List<string> { "INT", "INT", "INT", "INT", "INT", "INT", "INT", "INT", "INT", "STRING", "INT" };
            var name = "XYJ_ShopTable.txt";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls|文本文件(*.txt)|*.txt";
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
            // 在导出的时候重新对Index 进行个排序
            int i = 1;
            foreach (var item in _xyjShop.OrderBy(x=>x.商店标识).ThenBy(x=>x.nMenuIndex).ThenBy(x=>x.nShopIndex))
            {
                item.index =i.ToString();
                i++;
            }

            DataTable xyjShop = ModelTool.ModelsToDataTable<XYJShop>(_xyjShop.OrderBy(x => x.商店标识).ThenBy(x => x.nMenuIndex).ThenBy(x => x.nShopIndex).ToList());


            Excel.ExportToExcelOrTxt(xyjShop, saveFileDialog.FileName, columns);

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            var list = _articleLists.Where(x => x.Name.Contains(this.edtEqName.Text)).ToList();

            RefreshEqDataSource(list);
        }

        private void btnEqAdd_Click(object sender, EventArgs e)
        {

            ArticleList dr = (ArticleList)this.grdvEq.GetFocusedRow();
            if (dr == null)
            {
                return;
            }
            XYJShop xYJShop = new XYJShop();


            xYJShop.index = (Convert.ToInt32(_xyjShop.Count()) + 1).ToString();
            xYJShop.商店标识 = _Index;
            xYJShop.nMenuIndex = _MenueIndex;
            xYJShop.nShopIndex = _ShopIndex;
            xYJShop.物品INDEX = (Convert.ToInt32(_xyjShop.Where(x => x.商店标识 == _Index && x.nMenuIndex == _MenueIndex && x.nShopIndex == _ShopIndex).Count()) + 1).ToString();
            xYJShop.物品ITEMID = dr.Index;
            xYJShop.物品数量 = "1";
            xYJShop.物品价格 = "0";
            xYJShop.物品折扣比 = "100";
            xYJShop.颜色显示 = "0";

            MessageBox.Show("添加成功");

            _xyjShop.Add(xYJShop);
            RefreshDataSource();
        }
    }
}
