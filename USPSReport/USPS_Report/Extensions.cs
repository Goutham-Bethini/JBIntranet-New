using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace USPS_Report
{
    public static class Extensions
    {
        public static DataTable ToDataTable<TSource>(this IList<TSource> data)
        {
            DataTable dataTable = new DataTable(typeof(TSource).Name);
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                    prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public static object GetDBNullOrValue<T>(this T val)
        {
            bool isDbNull = true;
            Type t = typeof(T);

            if (Nullable.GetUnderlyingType(t) != null)
                isDbNull = EqualityComparer<T>.Default.Equals(default(T), val);
            else if (t.IsValueType)
                isDbNull = false;
            else
                isDbNull = val == null;

            return isDbNull ? DBNull.Value : (object)val;
        }
    }
}