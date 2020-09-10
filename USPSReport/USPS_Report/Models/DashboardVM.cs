using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USPS_Report.Areas.Reports.Models;
using DotNet.Highcharts;

namespace USPS_Report.Models
{
    public class DashboardVM
    {
        //  public IList<Highcharts> Charts { get; set; }

       
      
        public HoldReasonVM holdReason { get; set; }
        public HoldPayerVM holdPayer { get; set; }
    }
}