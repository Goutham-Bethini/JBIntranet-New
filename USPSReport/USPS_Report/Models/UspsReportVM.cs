using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Models
{
    public class UspsReportVM
    {

        public string uID { get; set; }
        public Int32? WoID { get; set; }
        public string ConfirmationNum { get; set; }
        public DateTime? PostDate { get; set; }

        public Int32? Account { get; set; }


    }

    public class FedExReportVM
    {

       // public string uID { get; set; }
        public string WoID { get; set; }
        public string ConfirmationNum { get; set; }
        public string DateShipped { get; set; }

        //public Int32? Account { get; set; }


    }
}