using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.Reports.Models
{
    public class Logistics
    {
    }
    public class WaitingToInterfaceVM
    {
        public IList<pumpsHoldvm> holdlist { get; set; }
    }

    public class Orders
    {
        public int ID { get; set; }
        public int? Account { get; set; }
        public string HoldFromShippingReason { get; set; }
        public int? ID_Payer { get; set; }
        public string PayerName { get; set; }
        public DateTime? Request_Date { get; set; }

    }
}