using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPSReport.Models
{
    public class UspsReportVM
    {
        public string uID { get; set; }
        public Int32? WoID { get;set;}
        public string ConfirmationNum {get;set;}
        public DateTime? PostDate { get; set; }
     //   public string USPSReply { get; set; }
        public DetailReport Report { get; set; }
        public IDictionary<string, object> USPSVal { get; set; }
        public string USPSXml { get; set; }
    }
}