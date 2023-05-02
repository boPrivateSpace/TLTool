using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;


namespace FengXuTLTool
{
    public static class ModelTool
    {
        /// <summary>
        /// 用DataTable转换实体类
        /// </summary>
        public static List<T> DataTableConvertToModel<T>(DataTable dt)
        {
            List<T> modelList = new List<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return modelList;
            }

            int j  = 0;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;
                    T model = (T)Activator.CreateInstance(typeof(T));
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName, flag);
                        if (propertyInfo != null && dr[i] != DBNull.Value)
                        {
                            propertyInfo.SetValue(model, ChanageType(dr[i], propertyInfo.PropertyType), null);
                        }
                    }
                    j++;
                    modelList.Add(model);
                }
            }
            catch(Exception E)
            { 
            
            }


            return modelList;
        }

        public static object ChanageType(this object value, Type convertsionType)
        {
            //判断convertsionType类型是否为泛型，因为nullable是泛型类,
            if (convertsionType.IsGenericType &&

                //判断convertsionType是否为nullable泛型类
                convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }

                //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                NullableConverter nullableConverter = new NullableConverter(convertsionType);

                //将convertsionType转换为nullable对的基础基元类型
                convertsionType = nullableConverter.UnderlyingType;
            }

            return Convert.ChangeType(value, convertsionType);
        }


        /// <summary>
        /// List泛型转换DataTable.
        /// </summary>
        public static DataTable ModelsToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                tb.Rows.Add(values);
            }
            return tb;
        }

        /// <summary>
        /// 如果类型可空，则返回基础类型，否则返回类型
        /// </summary>
        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        /// <summary>
        /// 指定类型是否可为空
        /// </summary>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}
