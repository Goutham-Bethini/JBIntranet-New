using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USPS_Report.Models
{
    public class AllProdSubstitution
    {       
        public string OldProdCode { get; set; }        
        public string NewProdCode { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int NoOfAccounts { get; set; }
        public string RepeatingOrExisting { get; set; }
    }
}