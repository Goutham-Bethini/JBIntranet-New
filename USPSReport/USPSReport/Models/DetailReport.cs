using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPSReport.Models
{
    public class DetailReport
    {
        public string Acccount { get; set; }
        public string Tracknum { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string DDate { get; set; }

        public string Amount { get; set; }


    }
}