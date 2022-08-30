using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Helper
{
    public class GetSafeData
    {
        public static int GetSafeInt(object dc)
        {
            if (dc == null || dc == DBNull.Value)
                return 0;
            else
                return Convert.ToInt32(dc);
        }

        public static DateTime GetSafeDate(object dc)
        {
            if (dc == null || dc == DBNull.Value)
                return DateTime.MinValue;
            else
                return Convert.ToDateTime(dc);
        }
        public static DateTime? GetSafeDate_Nullable(object dc)
        {
            if (dc == null || dc == DBNull.Value)
                return null;
            else
                return Convert.ToDateTime(dc);
        }

        public static decimal GetSafeDecimal(object dc)
        {
            if (dc == null || dc == DBNull.Value)
                return 0.0m;
            else
                return Convert.ToDecimal(dc);
        }
        public static string GetSafeString(object dc)
        {
            if (dc == null || dc == DBNull.Value)
                return "";
            else
                return dc.ToString();
        }
        public static bool GetSafeBoolean(object dc)
        {
            if (dc == null || dc == DBNull.Value)
                return false;
            else
                return Convert.ToBoolean(dc);
        }
    }
}