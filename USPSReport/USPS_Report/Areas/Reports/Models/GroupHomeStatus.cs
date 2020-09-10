using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{
    public class GroupHomeStatusVM
    {
        public string GroupHome { get; set; }
        public DateTime? LastOrder { get; set; }
        public DateTime? NextOrder { get; set; }
        public int? IncompleteOrders { get; set; }

        public int? CompleteOrder { get; set; }
    }


    public class GroupHomeStatusReport
    {
        //public static IList<GroupHomeStatusVM> GetGroupHomeStatusReport()
        //{

        //    try
        //    {
        //        using (HHSQLDBEntities _db = new HHSQLDBEntities())
        //        {
        //            var Ids = new int[] { 5, 7,18 };

        //            var _loclist = (from dt in _db.tbl_DeliveryLocation_Table
        //                            where Ids.Contains(dt.ID)
        //                            select new { dt.DeliveryLocationName,dt.ID}).ToList();
        //                            )
        //            var _woList = (from wos in _db.tbl_PS_WorkOrder
        //                           join ops in _db.tbl_Operator_Table
        //                           on wos.ID_PrimaryAssignedUser equals ops.ID
        //                           where wos.Account == account
        //                           from wr in _db.WorkOrdersReleaseds.Where(w => w.worID_WorkOrder == wos.ID).DefaultIfEmpty()

                                  
        //            return _list;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return new List<GroupHomeStatusVM>();
        //    }


        //}


    }
}