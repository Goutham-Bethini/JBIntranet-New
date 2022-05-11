using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{
    public class ManageOrder
    {
        public class CancelOrdersVM
        {
            public IList<ExcludedOrder> ExcludedOrders { get; set; }
        }
        public class ExcludedOrder
        {
            public int WorkOrder { get; set; }
            public int? Account { get; set; }
            public string Reason { get; set; }
        }

        public class MassCancelHistoryVM
        {
            public IList<CanceledItem> CanceledItems { get; set; }
        }
        public class CanceledItem
        {
            public string FileName { get; set; }
            public string ActionBy { get; set; }
            public DateTime? ActionDate { get; set; }
            public string CancelNote { get; set; }

        }

        public static IList<CanceledItem> GetData()
        {
            List<CanceledItem> canceledItems = new List<CanceledItem>();
            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    canceledItems = (from item in _db.tbl_MassCancel_History 
                                     select new CanceledItem
                                     {
                                         FileName=item.FileName,
                                         ActionBy = item.ActionBy,
                                         ActionDate = item.ActionDate,
                                         CancelNote = item.CancelNote
                                     }
                               ).ToList();
                }
                return canceledItems;
            }
            catch (Exception ex)
            {
                return new List<CanceledItem>();
            }
        }
    }
}