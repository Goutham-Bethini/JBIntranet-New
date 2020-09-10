using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPSReport.Models
{
    public class MainVM
    {
        public string trackNum { get; set; }
        public IList<UspsReportVM> dataList { get; set; }
    }
}