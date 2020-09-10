using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.ComponentModel.DataAnnotations;

namespace USPS_Report.Areas.Reports.Models
{
    public class ReportWoNotPrinted
    {
        public static IList<WONotPrinted> GetWO_NotPrinted()
        {


            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {

                var _list = (from wos in _db.tbl_PS_WorkOrder
                             where ((!wos.StatusComments.Contains("%walk-in%") && wos.Completed_Date == null && wos.ConfirmationNumber == null
                             && wos.LastPrintDate == null && wos.Cancel_Date == null
                             && wos.HoldFromShipping == 0 && wos.Account != null)
                             || (wos.StatusComments == null && wos.Completed_Date == null
                             && wos.ConfirmationNumber == null && wos.LastPrintDate == null
                             && wos.Cancel_Date == null && wos.HoldFromShipping == 0
                             && wos.Account != null))
                             select new WONotPrinted
                             {
                                 Account = wos.Account,
                                 WorkOrderID = wos.ID,
                                 Request_Date = wos.Request_Date,
                                 StatusComments = wos.StatusComments,
                                 LastPrintDate = wos.LastPrintDate
                             }).OrderBy(t => t.Request_Date).OrderBy(t => t.Account).ToList();


                return _list;


            }




        }

      
    }
    public class WONotPrinted
    {
        public int? Account { get; set; }

        [Display(Name = "WorkOrder#")]
        public int WorkOrderID { get; set; }
        public DateTime? Request_Date { get; set; }
        public string StatusComments { get; set; }
        public DateTime? LastPrintDate { get; set; }

        // public int? count { get; set; }


    }


    public class ReportWoLost
    {
        public static IList<WOLost> GetWO_Lost()
        {

            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                DateTime? LastEOD = (_db.Eligibility_EOD_Process.Where(a => a.eodDeleted == null).Max(t => t.eodClosed));


               

                var _list = (from wos in _db.tbl_PS_WorkOrder
                             where wos.Cancel_Date == null && wos.Completed_Date == null
                              && wos.LastPrintDate >= new DateTime(2011, 10, 3) && wos.LastPrintDate < LastEOD
                              && wos.ID_LastPrint_User != null && wos.ID_LastPrint_User.Value.Equals(488)
                             select new
                             {
                                 wos.ID,
                                 wos.Account,
                                 wos.Request_Date,
                                 wos.LastPrintDate,
                                 wos.StatusComments,
                                 wos.ID_LastPrint_User,
                                 wos.DateMovedToUser,
                                 wos.ID_PrimaryAssignedUser
                             }).ToList();


                var _list1 = (from a in _list
                              group a by new
                              {
                                  a.ID,
                                  a.Account,
                                  a.Request_Date,
                                  a.LastPrintDate,
                                  a.StatusComments,
                                  a.ID_LastPrint_User
                              } into t
                              select new WOLost
                              {
                                  Account = t.Key.Account,
                                  WorkOrderID = t.Key.ID,
                                  Request_Date = t.Key.Request_Date,
                                  LastPrintDate = t.Key.LastPrintDate,
                                  StatusComments = t.Key.StatusComments,
                                  NextPrintDate = (_db.tbl_PS_RepeatingOrders.Where(a => a.Account == t.Key.Account).Max(p => p.NextRepeatDate))

                              }).OrderBy(o => o.LastPrintDate).OrderBy(o => o.WorkOrderID).Take(10).ToList();

                return _list1;


            }

        }
    }


    public class WOLost
    {
        public int? Account { get; set; }

        [Display(Name = "WorkOrder")]
        public int WorkOrderID { get; set; }
        public DateTime? Request_Date { get; set; }

        public DateTime? LastPrintDate { get; set; }
        public DateTime? NextPrintDate { get; set; }
        public string Status { get; set; }
        public string StatusComments { get; set; }

        public string OracleStatus { get; set; }

        public DateTime? ShippedInOracle { get; set; }

        public int? TrackingNoFounnd { get; set; }
    }
}