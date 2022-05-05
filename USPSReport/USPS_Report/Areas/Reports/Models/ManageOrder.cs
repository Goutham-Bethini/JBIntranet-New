using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}