using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Models
{
    public class MainVM
    {
        public string trackNum { get; set; }
        public IList<UspsReportVM> dataList { get; set; }
    }


    public class MainVMFedEx
    {
        public string trackNum { get; set; }
        public IList<FedExReportVM> dataList { get; set; }
    }
}