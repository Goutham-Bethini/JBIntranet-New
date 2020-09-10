using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{
    public class ExpiredProductReport
    {
        public static IList<ExpiredProductVM> GetExpiredProduct()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    var _list = (from ex in _db.ExpiredProduct
                                 join uom in _db.tbl_Inv_UOM_Table
                                 on ex.ID_UOM equals uom.ID
                                 select new ExpiredProductVM
                                 {
                                     Product_Code = ex.ProductCode,
                                     Quantity = ex.Qty,
                                     UOM = uom.UOMName,
                                     CountedBy = ex.CountedBy,
                                     RemovedBy = ex.RemovedBy == null ? "Item not Removed" : ex.RemovedBy,
                                     DateExpired = ex.DateExpired,
                                     DateAdded = ex.DateAdded,
                                     DateRemoved = ex.DateRemoved == null ? "Item not Removed" : ex.DateRemoved.ToString()
                                 }
                                 ).ToList();

                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<ExpiredProductVM>();
            }


        }


    }
    public class ExpiredProductVM
    {
        public string Product_Code { get; set; }

        public int? Quantity { get; set; }

        public string UOM { get; set; }

        public string CountedBy { get; set; }

        public string RemovedBy { get; set; }


        public DateTime? DateExpired { get; set; }
        public DateTime? DateAdded { get; set; }

        public string DateRemoved { get; set; }


    }
}