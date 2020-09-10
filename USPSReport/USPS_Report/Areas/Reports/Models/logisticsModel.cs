using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{
    public class logisticsModel
    {
        public static IList<WIList> GetWaitingToInterface()
        {

            IList<WIList> list = new List<WIList>();
            using (ReportsEntities _db = new ReportsEntities())
            {
               
                list = _db.v_waitingToInterface.Select()

                return list;
            }
            //   return _rec;
        }
    }

    public class WIVM
    {
        public IList<WIList> WaitingOrders { get; set; }
    }

public class WIList
{
        public int ID { get; set; }
        public int Account { get; set; }
        public DateTime? DateMovedToUser { get; set; }
        public DateTime? Request_Date { get; set; }

        public DateTime? Last_Updated_Date { get; set; }
        public string Last_Updated_By { get; set; }
}

   
}