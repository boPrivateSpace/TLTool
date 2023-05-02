#region Copyright (c) 2007 xQuant Company Ltd.(Hangzhou)

/*********************************************************************
 * ���ƣ� GridUtils.cs
 * ���ܣ� �ṩ���ĸ�������
 * ���ߣ� LiaoHongzhou
 * ��˾�� xQuant
 * ���ڣ� 2007-12-04
 * �汾�� 1.0.0
 * �޶���
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
        /// �ж�ָ����BandedGridView�Ƿ�Ϊ��Band
        /// </summary>
        /// <param name="bandedView">ָ����BandedGridView</param>
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
        /// ��ȡ����е�ȫ��
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
                // ����Band�ı���·��
                string bandPath = GetBandPath(bandColumn.OwnerBand);
                if (!string.IsNullOrEmpty(bandPath))
                {
                    caption = bandPath + "|" + caption;
                }
            }

            return caption;
        }

        /// <summary>
        /// ��ȡBand�ı���·��
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
        /// ��ȡ����еı߽��
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
        /// ת�������ֵ
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
        /// ����ת���ַ���ֵ������ɹ����򷵻�Decimal���͵�ֵ
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
        /// �ж�ĳ������Ϊ��������
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
        /// ����GridBand
        /// </summary>
        /// <param name="bands">GridBand����</param>
        /// <param name="bandName">GridBand����</param>
        /// <returns>���ҵ���GridBand</returns>
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
        /// ����GridBand�������aParentBand�´���aCaption��Band����ֱ�ӷ��ظ�Band
        /// </summary>
        /// <param name="aView"></param>
        /// <param name="aParentBand"></param>
        /// <param name="aCaption"></param>
        /// <returns></returns>
        public static GridBand CreateGridBand(BandedGridView aView, GridBand aParentBand, string aCaption)
        {
            GridBandCollection bandList = aParentBand == null ? aView.Bands : aParentBand.Children;

            // ���band�Ƿ����
            for (int i = 0; i < bandList.Count; i++)
            {
                if (bandList[i].Caption == aCaption)
                {
                    return bandList[i];
                }
            }

            // ����������򴴽�
            GridBand band = bandList.AddBand(aCaption);
            band.Name = GetBandPath(band);
            band.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            return band;
        }

        /// <summary>
        /// ɾ��û��ӵ���е�GridBand
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
        /// �����˵���DXMenuItem
        /// </summary>
        /// <param name="owner">GridViewMenu</param>
        /// <param name="caption">�˵�����</param>
        /// <param name="image">�˵�ͼƬ</param>
        /// <param name="clickEvent">Click �¼�</param>
        /// <param name="beginGroup"></param>
        public  static DXMenuItem CreatePopupMenuItem(DXMenuItemCollection owner, string caption, Image image, bool beginGroup, EventHandler click)
        {
            DXMenuItem item = new DXMenuItem(caption, click);
            item.Image = image;
            item.BeginGroup = beginGroup;
            owner.Add(item);
            return item;
        }

        #region ��������ʼ���нӿ�

        /// <summary>
        /// ��������У����������Ѵ��ڣ��򲻴�������У��������aPath��aWidth��aFormatString�������޸��������
        /// </summary>
        /// <param name="aView">������GridView��Ҳ������BandedGridView</param>
        /// <param name="aPath">�ԡ�|���ָ�������·�������磺Greeks|Delta</param>
        /// <param name="aFieldName">�ֶ�����������Ϊ��</param>
        /// <param name="aWidth">��ȣ�0ΪĬ�Ͽ��</param>
        /// <param name="aFormatString">��ʽ���ַ���</param>
        /// <returns>�������ҵ����Ѵ����ı����</returns>
        public static GridColumn CreateGridColumn(GridView aView, string aPath, string aFieldName, int aWidth, string aFormatString)
        {
            // ����ֶ���Ϊ�գ��򲻴��������
            if (string.IsNullOrEmpty(aFieldName))
            {
                return null;
            }

            // �����ǲ����Ѵ��ڸ��ֶ�
            GridColumn colResult = aView.Columns.ColumnByFieldName(aFieldName);
            string tmpPath = string.IsNullOrEmpty(aPath) ? aFieldName : aPath;

            // �����BandedGridView������Ҫ����GridBand
            if (aView is BandedGridView bandedView)
            {
                GridBand band = null;
                string[] paths = tmpPath.Split('|');
                if (paths.Length > 1)
                {
                    // ���һ��tokenΪ����е�Caption
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

            // 2016-06-16 ww ����Column.Name
            if (colResult != null && string.IsNullOrEmpty(colResult.Name))
            {
                colResult.Name = aFieldName;
            }

            return colResult;
        }

        /// <summary>
        /// ����Grid�ķǰ��е���ʾ������������ã�
        /// </summary>
        /// <param name="aView">Ŀ��GridView</param>
        /// <param name="aPath">��ʾ���������硾������</param>
        /// <param name="aFieldName">������ֶ������硾Name��</param>
        /// <returns></returns>
        public static GridColumn CreateGridColumn(GridView aView, string aPath, string aFieldName)
        {
            return CreateGridColumn(aView, aPath, aFieldName, 200,"");
        }
        #endregion



        /// <summary>
        /// ��ȡѡ�е�������
        /// </summary>
        /// <param name="aView">������GridView��Ҳ������BandedGridView</param>
        /// <param name="aGroupReturnChild">�ϼ����Ƿ񷵻ض�Ӧ������</param>
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
        /// ѡ��ѡ�ֵ�������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="aView">������GridView��Ҳ������BandedGridView</param>
        /// <param name="aGroupReturnChild">�ϼ����Ƿ񷵻ض�Ӧ������</param>
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

        //����aRowID��ѡ�е��������б�
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
        /// ���ñ��Ĭ�ϵ�һЩ����
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
        /// ����GridView�н�λ
        /// </summary>
        /// <param name="aViewView">Ŀ��GridView</param>
        /// <param name="aFieldName">����</param>
        /// <param name="aMoneyUnit">��λ����Ԫ��ǧԪ��</param>
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
        /// ����ֹ�ڲ����ݸ����¼�����������Դ
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
        /// ��������ɾ����
        /// </summary>
        /// <param name="gridView">�����ͼ</param>
        /// <param name="colName">��ʾ����</param>
        /// <param name="fieldName">������ֶ���</param>
        public static void CreateDeleteColunm(GridView gridView, string colName, string fieldName)
        {
            var deleteStr = "ɾ��";
            var confirmDeleteStr = "ȷ��ɾ��";
            var deleteState = new Dictionary<object, int>();
            var actionColumn = CreateGridColumn(gridView, colName, fieldName);
            actionColumn.OptionsColumn.AllowEdit = false;
            actionColumn.UnboundType = UnboundColumnType.String;

            // ȷ������С���
            gridView.CustomDrawColumnHeader += (sender, e) =>
            {
                if (e.Column == actionColumn)
                {
                    e.Column.MinWidth = Convert.ToInt32(e.Graphics.MeasureString(confirmDeleteStr, e.Appearance.Font).Width) + 20;
                }
            };

            // ����ɾ��״̬���õ�Ԫ������
            gridView.CustomUnboundColumnData += (sender, e) =>
            {
                if (e.Column == actionColumn && e.IsGetData)
                {
                    e.Value = deleteState[e.Row] == 0 ? deleteStr : confirmDeleteStr;
                }
            };

            // ����Դ�ı�ʱ�����ɾ��״̬�б�
            gridView.DataSourceChanged += (sender, e) =>
            {
                deleteState.Clear();
                foreach (var row in (System.Collections.IEnumerable)((GridView)sender).DataSource)
                {
                    deleteState.Add(row, 0);
                }
            };

            // ע���е���¼�
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

            // ����ɾ��״̬���õ�Ԫ����ʽ
            gridView.RowCellStyle += (sender, e) =>
            {
                if (e.Column == actionColumn)
                {
                    var row = ((GridView)sender).GetRow(e.RowHandle);
                    if (row == null)
                    {
                        return;
                    }

                    //2018-11-07 ��ٻ �����ֵ��Ƿ����keyֵ���жϣ�����keyֵ�����ֵ���ʱ����
                    e.Appearance.ForeColor = deleteState.ContainsKey(row) && deleteState[row] == 0 ? Color.Blue : Color.Red;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                }
            };

            // ����иı�����ɾ��״̬
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

            // �����иı�����ɾ��״̬
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

                // 2021-11-04 ��ΰ P008XIR-31988 ����������ʱ�������ظ��������ϱ仯�¼�
                view.GroupSummary.BeginUpdate();

                // 2022-01-25 ���ˮ P008XIR-37214 ����ر��Զ������й��ܣ�����ֱ�Ӹ�ֵdatatable���޷�������
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
