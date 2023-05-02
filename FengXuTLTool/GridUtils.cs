#region Copyright (c) 2007 xQuant Company Ltd.(Hangzhou)

/*********************************************************************
 * 名称： GridUtils.cs
 * 功能： 提供表格的辅助函数
 * 作者： LiaoHongzhou
 * 公司： xQuant
 * 日期： 2007-12-04
 * 版本： 1.0.0
 * 修订：
 *********************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace xQuant.UI.Assist
{
    public class GridUtils
    {

        /// <summary>
        /// 判断指定的BandedGridView是否为空Band
        /// </summary>
        /// <param name="bandedView">指定的BandedGridView</param>
        /// <returns></returns>
        public static bool IsEmptyBand(BandedGridView bandedView)
        {
            if (bandedView == null || bandedView.Bands.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < bandedView.Bands.Count; i++)
            {
                GridBand band = bandedView.Bands[i];
                if (band.Visible && (band.HasChildren || !string.IsNullOrEmpty(band.Caption)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取表格列的全名
        /// </summary>
        /// <param name="gridColumn"></param>
        /// <returns></returns>
        public static string GetGolumnFullCaption(GridColumn gridColumn)
        {
            if (gridColumn == null)
            {
                return string.Empty;
            }

            string caption = gridColumn.Caption;
            BandedGridColumn bandColumn = gridColumn as BandedGridColumn;
            if (bandColumn != null)
            {
                // 加上Band的标题路径
                string bandPath = GetBandPath(bandColumn.OwnerBand);
                if (!string.IsNullOrEmpty(bandPath))
                {
                    caption = bandPath + "|" + caption;
                }
            }

            return caption;
        }

        /// <summary>
        /// 获取Band的标题路径
        /// </summary>
        /// <param name="aBand"></param>
        /// <returns></returns>
        private static string GetBandPath(GridBand aBand)
        {
            string path = string.Empty;
            GridBand band = aBand;
            while (band != null)
            {
                string bandCaption = band.Caption.Trim();
                if (!string.IsNullOrEmpty(bandCaption))
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        path = bandCaption + "|" + path;
                    }
                    else
                    {
                        path = bandCaption;
                    }
                }

                band = band.ParentBand;
            }

            return path;
        }

        /// <summary>
        /// 获取表格列的边界框
        /// </summary>
        /// <param name="column"></param>
        /// <param name="viewInfo"></param>
        /// <returns></returns>
        public static Rectangle GetColumnBounds(GridColumn column, GridViewInfo viewInfo)
        {
            if (column == null || column.View == null || viewInfo == null)
            {
                return Rectangle.Empty;
            }

            GridColumnInfoArgs colInfo = viewInfo.ColumnsInfo[column];
            if (colInfo == null)
            {
                return Rectangle.Empty;
            }

            Rectangle rect = colInfo.Bounds;
            int fixedLeftPos = viewInfo.ViewRects.FixedLeft.Right;
            int fixedRightPos = viewInfo.ViewRects.FixedRight.Left;

            BandedGridViewInfo bandViewInfo = viewInfo as BandedGridViewInfo;
            if (bandViewInfo != null)
            {
                GridBand band = (column as BandedGridColumn).OwnerBand.RootBand;
                if (bandViewInfo.HasFixedLeft && band.VisibleIndex <= bandViewInfo.FixedLeftBand.VisibleIndex)
                {
                    return rect;
                }

                if (bandViewInfo.HasFixedRight && band.VisibleIndex >= bandViewInfo.FixedRightBand.VisibleIndex)
                {
                    if (fixedRightPos > fixedLeftPos)
                    {
                        return rect;
                    }
                    else if (rect.Left < fixedLeftPos && rect.Right <= fixedLeftPos)
                    {
                        return Rectangle.Empty;
                    }
                    else
                    {
                        rect.Width -= fixedLeftPos - rect.X;
                        rect.X = fixedLeftPos;
                        return rect;
                    }
                }
            }
            else
            {
                if (viewInfo.HasFixedLeft && column.VisibleIndex <= viewInfo.FixedLeftColumn.VisibleIndex)
                {
                    return rect;
                }

                if (viewInfo.HasFixedRight && column.VisibleIndex >= viewInfo.FixedRightColumn.VisibleIndex)
                {
                    if (fixedRightPos > fixedLeftPos)
                    {
                        return rect;
                    }
                    else if (rect.Left < fixedLeftPos && rect.Right <= fixedLeftPos)
                    {
                        rect.Width -= fixedLeftPos - rect.X;
                        rect.X = fixedLeftPos;
                        return rect;
                    }
                }
            }

            if (fixedLeftPos == fixedRightPos && fixedLeftPos > 0)
            {
                return Rectangle.Empty;
            }

            if (viewInfo.HasFixedLeft && rect.Left < fixedLeftPos)
            {
                if (rect.Right > fixedLeftPos)
                {
                    rect.Width -= fixedLeftPos - rect.X;
                    rect.X = fixedLeftPos;
                }
                else
                {
                    return Rectangle.Empty;
                }
            }

            if (viewInfo.HasFixedRight && rect.Right > fixedRightPos)
            {
                if (rect.Left < fixedRightPos)
                {
                    rect.Width -= rect.Right - fixedRightPos;
                }
                else
                {
                    return Rectangle.Empty;
                }
            }

            return rect;
        }

        /// <summary>
        /// 转化对象的值
        /// </summary>
        /// <param name="val"></param>
        /// <param name="destType"></param>
        /// <returns></returns>
        public static object ConvertValue(object val, Type destType)
        {
            if (val == null)
            {
                return null;
            }

            Type valType = val.GetType();
            if (object.ReferenceEquals(valType, destType))
            {
                return val;
            }

            if (val is IConvertible)
            {
                try
                {
                    return Convert.ChangeType(val, destType, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// 尝试转换字符的值，如果成功，则返回Decimal类型的值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object TryParseValue(string val)
        {
            if (val == null)
            {
                return null;
            }

            string ss = val;
            decimal d2 = 1.0M;
            for (int i = val.Length - 1; i >= 0; i--)
            {
                string symbol = new string(val[i], 1);
                if (symbol == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentSymbol)
                {
                    ss = ss.Remove(i);
                    d2 /= 100.0M;
                }
                else if (symbol == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PerMilleSymbol)
                {
                    ss = ss.Remove(i);
                    d2 /= 1000.0M;
                }
                else
                {
                    break;
                }
            }

            try
            {
                if (decimal.TryParse(ss, System.Globalization.NumberStyles.Any, null, out decimal d1))
                {
                    return d1 * d2;
                }
                else
                {
                    return val;
                }
            }
            catch
            {
                return val;
            }
        }

        /// <summary>
        /// 判断某类型是为数字类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);
            if (typeCode == TypeCode.SByte || typeCode == TypeCode.Byte ||
                typeCode == TypeCode.Int16 || typeCode == TypeCode.UInt16 ||
                typeCode == TypeCode.Int32 || typeCode == TypeCode.UInt32 ||
                typeCode == TypeCode.Int64 || typeCode == TypeCode.UInt64 ||
                typeCode == TypeCode.Single || typeCode == TypeCode.Double ||
                typeCode == TypeCode.Decimal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查找GridBand
        /// </summary>
        /// <param name="bands">GridBand集合</param>
        /// <param name="bandName">GridBand名称</param>
        /// <returns>查找到的GridBand</returns>
        public static GridBand FindGridBand(GridBandCollection bands, string bandName)
        {
            foreach (GridBand band in bands)
            {
                if (band.Name == bandName)
                {
                    return band;
                }

                GridBand child = FindGridBand(band.Children, bandName);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        /// <summary>
        /// 创建GridBand，如果在aParentBand下存在aCaption的Band，则直接返回该Band
        /// </summary>
        /// <param name="aView"></param>
        /// <param name="aParentBand"></param>
        /// <param name="aCaption"></param>
        /// <returns></returns>
        public static GridBand CreateGridBand(BandedGridView aView, GridBand aParentBand, string aCaption)
        {
            GridBandCollection bandList = aParentBand == null ? aView.Bands : aParentBand.Children;

            // 检查band是否存在
            for (int i = 0; i < bandList.Count; i++)
            {
                if (bandList[i].Caption == aCaption)
                {
                    return bandList[i];
                }
            }

            // 如果不存在则创建
            GridBand band = bandList.AddBand(aCaption);
            band.Name = GetBandPath(band);
            band.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            return band;
        }

        /// <summary>
        /// 删除没有拥有列的GridBand
        /// </summary>
        /// <param name="bands"></param>
        public static void RemoveBandsOfNotColumn(GridBandCollection bands)
        {
            for (int i = bands.Count - 1; i >= 0; i--)
            {
                GridBand band = bands[i];

                RemoveBandsOfNotColumn(band.Children);

                if (band.Children.Count == 0 && band.Columns.Count == 0)
                {
                    bands.Remove(band);
                }
            }
        }

        /// <summary>
        /// 创建菜单项DXMenuItem
        /// </summary>
        /// <param name="owner">GridViewMenu</param>
        /// <param name="caption">菜单标题</param>
        /// <param name="image">菜单图片</param>
        /// <param name="clickEvent">Click 事件</param>
        /// <param name="beginGroup"></param>
        public  static DXMenuItem CreatePopupMenuItem(DXMenuItemCollection owner, string caption, Image image, bool beginGroup, EventHandler click)
        {
            DXMenuItem item = new DXMenuItem(caption, click);
            item.Image = image;
            item.BeginGroup = beginGroup;
            owner.Add(item);
            return item;
        }

        #region 创建并初始化列接口

        /// <summary>
        /// 创建表格列，如果表格列已存在，则不创建表格列，但会根据aPath、aWidth、aFormatString参数来修改相关属性
        /// </summary>
        /// <param name="aView">可以是GridView，也可以是BandedGridView</param>
        /// <param name="aPath">以“|”分隔的完整路径，例如：Greeks|Delta</param>
        /// <param name="aFieldName">字段名，不允许为空</param>
        /// <param name="aWidth">宽度，0为默认宽度</param>
        /// <param name="aFormatString">格式化字符串</param>
        /// <returns>返回已找到或已创建的表格列</returns>
        public static GridColumn CreateGridColumn(GridView aView, string aPath, string aFieldName, int aWidth, string aFormatString)
        {
            // 如果字段名为空，则不创建表格列
            if (string.IsNullOrEmpty(aFieldName))
            {
                return null;
            }

            // 查找是不是已存在该字段
            GridColumn colResult = aView.Columns.ColumnByFieldName(aFieldName);
            string tmpPath = string.IsNullOrEmpty(aPath) ? aFieldName : aPath;

            // 如果是BandedGridView，则需要创建GridBand
            if (aView is BandedGridView bandedView)
            {
                GridBand band = null;
                string[] paths = tmpPath.Split('|');
                if (paths.Length > 1)
                {
                    // 最后一个token为表格列的Caption
                    for (int i = 0; i < paths.Length - 1; i++)
                    {
                        band = CreateGridBand(bandedView, band, paths[i]);
                    }
                }

                if (band == null)
                {
                    band = CreateGridBand(bandedView, null, string.Empty);
                }

                BandedGridColumn colBand = colResult as BandedGridColumn;
                if (colBand == null)
                {
                    colBand = bandedView.Columns.AddField(aFieldName);
                    band.Columns.Add(colBand);
                    colBand.Visible = true;
                }
                else
                {
                    colBand.OwnerBand = band;
                }

                colResult = colBand;
                colResult.Caption = paths[paths.Length - 1];
            }
            else
            {
                if (colResult == null)
                {
                    colResult = aView.Columns.AddField(aFieldName);
                    colResult.Visible = true;
                }

                colResult.Caption = tmpPath;
            }

            if (colResult != null)
            {

                colResult.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                if (aWidth > 0)
                {
                    colResult.Width = aWidth;
                }

                if (string.IsNullOrWhiteSpace(aFormatString))
                {
                    colResult.DisplayFormat.FormatType = FormatType.None;
                    colResult.DisplayFormat.FormatString = aFormatString;
                }
                else
                {
                    colResult.DisplayFormat.FormatType = FormatType.Numeric;
                    colResult.DisplayFormat.FormatString = aFormatString;
                }

                colResult.OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
            }

            // 2016-06-16 ww 设置Column.Name
            if (colResult != null && string.IsNullOrEmpty(colResult.Name))
            {
                colResult.Name = aFieldName;
            }

            return colResult;
        }

        /// <summary>
        /// 处理Grid的非绑定列的显示（最基本的设置）
        /// </summary>
        /// <param name="aView">目标GridView</param>
        /// <param name="aPath">显示的列名，如【姓名】</param>
        /// <param name="aFieldName">定义的字段名，如【Name】</param>
        /// <returns></returns>
        public static GridColumn CreateGridColumn(GridView aView, string aPath, string aFieldName)
        {
            return CreateGridColumn(aView, aPath, aFieldName, 200,"");
        }
        #endregion



        /// <summary>
        /// 获取选中的数据行
        /// </summary>
        /// <param name="aView">可以是GridView，也可以是BandedGridView</param>
        /// <param name="aGroupReturnChild">合计行是否返回对应的子行</param>
        /// <returns></returns>
        public static List<int> GetSelectRowHandleList(GridView aView, bool aGroupReturnChild)
        {
            int[] selLst = aView.GetSelectedRows();
            List<int> re = new List<int>();
            if (selLst.Length > 0)
            {
                for (int i = 0; i < selLst.Length; i++)
                {
                    AddRowHandle(aView, re, selLst[i], aGroupReturnChild);
                }
            }
            else
            {
                AddRowHandle(aView, re, aView.FocusedRowHandle, aGroupReturnChild);
            }

            re.Sort();
            return re;
        }

        /// <summary>
        /// 选择选种的数据行
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="aView">可以是GridView，也可以是BandedGridView</param>
        /// <param name="aGroupReturnChild">合计行是否返回对应的子行</param>
        /// <returns></returns>
        public static List<T> GetSelectRowHandleList<T>(GridView aView, bool aGroupReturnChild)
            where T : class
        {
            List<int> selLst = GetSelectRowHandleList(aView, aGroupReturnChild);
            List<T> re = new List<T>();
            foreach (int i in selLst)
            {
                T obj = aView.GetRow(i) as T;
                if (obj != null)
                {
                    re.Add(obj);
                }
            }

            return re;
        }

        //增加aRowID到选中的数据行列表
        private static void AddRowHandle(GridView aView, List<int> aList, int aRowID, bool aGroupReturnChild)
        {
            if (aView.IsGroupRow(aRowID) && aGroupReturnChild)
            {
                for (int i = 0; i < aView.GetChildRowCount(aRowID); i++)
                {
                    AddRowHandle(aView, aList, aView.GetChildRowHandle(aRowID, i), aGroupReturnChild);
                }
            }
            else
            {
                aList.Add(aRowID);
            }
        }

        /// <summary>
        /// 设置表格默认的一些参数
        /// </summary>
        /// <param name="aView"></param>
        public static void SetDefualtOption(GridView aView)
        {
            aView.OptionsDetail.EnableMasterViewMode = false;
            aView.OptionsView.ColumnAutoWidth = false;
            aView.OptionsView.ShowFooter = false;
            aView.OptionsView.ShowGroupPanel = false;
            aView.OptionsView.AllowCellMerge = false;
            aView.OptionsBehavior.Editable = false;
        }
        /// <summary>
        /// 设置GridView列金额单位
        /// </summary>
        /// <param name="aViewView">目标GridView</param>
        /// <param name="aFieldName">列名</param>
        /// <param name="aMoneyUnit">金额单位，如元、千元等</param>
        public static void SetGridColMoneyUnit(GridView aViewView, string aFieldName, string aMoneyUnit)
        {
            GridColumn col = aViewView.Columns.ColumnByFieldName(aFieldName);
            if (col == null)
            {
                return;
            }

            string colCaption = col.Caption;
            int startIndex = colCaption.IndexOf("(");
            if (startIndex <= 0)
            {
                startIndex = colCaption.Length;
            }

            colCaption = colCaption.Substring(0, startIndex) + "(" + aMoneyUnit + ")";
            col.Caption = colCaption;
        }

        /// <summary>
        /// 以阻止内部数据更新事件下设置数据源
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="data"></param>
        public static void SetGridDataSource(GridView gridView, object data)
        {
            gridView.BeginDataUpdate();
            gridView.GridControl.DataSource = data;
            gridView.EndDataUpdate();
        }

        /// <summary>
        /// 创建单行删除列
        /// </summary>
        /// <param name="gridView">表格视图</param>
        /// <param name="colName">显示列名</param>
        /// <param name="fieldName">定义的字段名</param>
        public static void CreateDeleteColunm(GridView gridView, string colName, string fieldName)
        {
            var deleteStr = "删除";
            var confirmDeleteStr = "确认删除";
            var deleteState = new Dictionary<object, int>();
            var actionColumn = CreateGridColumn(gridView, colName, fieldName);
            actionColumn.OptionsColumn.AllowEdit = false;
            actionColumn.UnboundType = UnboundColumnType.String;

            // 确定列最小宽度
            gridView.CustomDrawColumnHeader += (sender, e) =>
            {
                if (e.Column == actionColumn)
                {
                    e.Column.MinWidth = Convert.ToInt32(e.Graphics.MeasureString(confirmDeleteStr, e.Appearance.Font).Width) + 20;
                }
            };

            // 根据删除状态设置单元格内容
            gridView.CustomUnboundColumnData += (sender, e) =>
            {
                if (e.Column == actionColumn && e.IsGetData)
                {
                    e.Value = deleteState[e.Row] == 0 ? deleteStr : confirmDeleteStr;
                }
            };

            // 数据源改变时，清空删除状态列表
            gridView.DataSourceChanged += (sender, e) =>
            {
                deleteState.Clear();
                foreach (var row in (System.Collections.IEnumerable)((GridView)sender).DataSource)
                {
                    deleteState.Add(row, 0);
                }
            };

            // 注册行点击事件
            gridView.RowCellClick += (sender, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Column == actionColumn)
                {
                    var grid = (GridView)sender;
                    var row = grid.GetFocusedRow();

                    if (deleteState[row] == 0)
                    {
                        deleteState[row]++;
                        grid.RefreshRowCell(e.RowHandle, actionColumn);
                    }
                    else
                    {
                        ((System.Collections.IList)grid.DataSource).Remove(row);
                        deleteState.Remove(row);
                        grid.RefreshData();
                    }
                }
            };

            // 根据删除状态设置单元格样式
            gridView.RowCellStyle += (sender, e) =>
            {
                if (e.Column == actionColumn)
                {
                    var row = ((GridView)sender).GetRow(e.RowHandle);
                    if (row == null)
                    {
                        return;
                    }

                    //2018-11-07 杨倩 增加字典是否包含key值的判断，避免key值不在字典中时报错
                    e.Appearance.ForeColor = deleteState.ContainsKey(row) && deleteState[row] == 0 ? Color.Blue : Color.Red;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                }
            };

            // 检点行改变重置删除状态
            gridView.FocusedRowChanged += (sender, e) =>
            {
                var grid = (GridView)sender;
                var row = grid.GetRow(e.PrevFocusedRowHandle);
                if (row == null)
                {
                    return;
                }

                deleteState[row] = 0;
                grid.RefreshRowCell(grid.FocusedRowHandle, actionColumn);
            };

            // 焦点列改变重置删除状态
            gridView.FocusedColumnChanged += (sender, e) =>
            {
                if (e.PrevFocusedColumn == actionColumn)
                {
                    var grid = (GridView)sender;
                    deleteState[grid.GetFocusedRow()] = 0;
                    grid.RefreshRowCell(grid.FocusedRowHandle, actionColumn);
                }
            };
        }

        public static void GridBeginUpdate(params GridView[] views)
        {
            if (views == null || views.Length == 0)
            {
                return;
            }

            foreach (GridView view in views)
            {
                if (view == null)
                {
                    continue;
                }

                if (view is BandedGridView && view.GridControl != null)
                {
                    view.GridControl.BeginUpdate();
                }

                view.BeginUpdate();

                // 2021-11-04 王伟 P008XIR-31988 创建汇总列时，避免重复触发集合变化事件
                view.GroupSummary.BeginUpdate();

                // 2022-01-25 俞德水 P008XIR-37214 这里关闭自动生成列功能，导致直接赋值datatable会无法生成列
                //view.OptionsBehavior.AutoPopulateColumns = false;
            }
        }

        public static void GridEndUpdate(params GridView[] views)
        {
            if (views == null || views.Length == 0)
            {
                return;
            }

            foreach (GridView view in views)
            {
                if (view == null)
                {
                    continue;
                }

                if (view is BandedGridView && view.GridControl != null)
                {
                    view.GridControl.EndUpdate();
                }

                view.GroupSummary.EndUpdate();
                view.EndUpdate();
            }
        }


        public static void InitGridColumnAllowEdit(GridView gridView, List<string> columnNameList)
        {
            foreach (GridColumn column in gridView.Columns)
            {
                if (columnNameList.Contains(column.FieldName))
                {
                    column.OptionsColumn.AllowEdit = true;
                }
                else
                {
                    column.OptionsColumn.AllowEdit = false;
                }
            }
        }


    }
}
