using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using USPS_Report.Models;

namespace USPS_Report.Areas.Reports.Models
{
    public class RWOsChanges
    {
        public class RWOsChangesVM
        {
            public DateTime StartDt { get; set; }
            public DateTime EndDt { get; set; }
            public IList<RWOsChangesData> Details { get; set; }

        }
        public class RWOsChangesData
        {
            public int Account { get; set; }
            public string ProductCode { get; set; }
            public string ProductDescription { get; set; }
            public string LastChangedBy { get; set; }
            public DateTime? ChangedDate { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? NextRepeatDate { get; set; }
            public int? Qty { get; set; }

        }
        public static IList<RWOsChangesData> GetRWOsData(DateTime startDt,DateTime endDt)
        {
            List<RWOsChangesData> lstRWOsData = new List<RWOsChangesData>();
            try
            {
                using (ReportsEntities _db = new ReportsEntities())
                {
                    lstRWOsData = (from item in _db.sp_GetRWOChanges(startDt, endDt)
                                   select new RWOsChangesData
                                   {
                                       Account = item.Account,
                                       ProductCode = item.ProductCode,
                                       ProductDescription = item.productdescription,
                                       LastChangedBy = item.LastChangedBy,
                                       ChangedDate = item.ChangedDate,
                                       CreatedBy = item.CreatedBy,
                                       CreatedDate = item.CreatedDate,
                                       NextRepeatDate = item.NextRepeatDate,
                                       Qty = item.Qty
                                   }
                               ).ToList();
                }
                return lstRWOsData;
            }
            catch (Exception ex)
            {
                return new List<RWOsChangesData>();
            }
        }
    }
}