using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{
    public class DropShipOrderReports
    {
        public static IList<DropShipOrderReceivedData> GetDropShipOrderReceived(DateTime? _startDt, DateTime? _endDt)
        {
            try
            {
               
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    var _list = (from wos in _db.tbl_PS_WorkOrder
                                 join ins in _db.tbl_Account_Insurance
                                 on wos.Account equals ins.Account
                                 join pay in _db.tbl_Payer_Table
                                 on ins.ID_Payer equals pay.ID
                                 join ops in _db.tbl_Operator_Table
                                 on wos.ID_PrimaryAssignedUser equals ops.ID
                                 where wos.ID_DeliveryLocation == 16 &&
                                 wos.Cancel_Note == null &&
                                ((wos.DateMovedToUser != null && wos.DateMovedToUser >= _startDt) || (wos.DateMovedToUser == null && wos.Request_Date >= _startDt))
                                && ((wos.DateMovedToUser != null && wos.DateMovedToUser <= _endDt) || (wos.DateMovedToUser == null && wos.Request_Date <= _endDt))


                                 select new DropShipOrderReceivedData
                                 {
                                     Date = wos.DateMovedToUser != null ? wos.DateMovedToUser : wos.Request_Date,
                                     Provider = pay.Name,
                                     VendorLink = wos.ID_PrimaryAssignedUser == 1 ? "X" : " "


                                 }
                                ).ToList();

                    var _list2 = (from t in _list
                                  group t by new { t.Provider, t.Date, t.VendorLink } into p
                                  select new DropShipOrderReceivedData
                                  {
                                      Date = p.Key.Date,
                                      Provider = p.Key.Provider,
                                      VendorLink = p.Key.VendorLink,
                                      Qty = p.Count()
                                  }).ToList();

                    return _list2;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<DropShipOrderReceivedData>();
            }

        }
    }
    public class DropShipOrderReceivedData
    {
        public string  Provider { get; set; }
        public DateTime? Date { get; set; }
        public int Qty { get; set; }
        public string VendorLink { get; set; }

    }

    public class DropshipOrderVM
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<DropShipOrderReceivedData> dropShipOrderReceived { get; set; }
    }
}