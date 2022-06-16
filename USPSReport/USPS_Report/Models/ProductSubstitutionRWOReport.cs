using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Models
{
    public class ProductSubstitutionRWOReport
    {
        public int Account { get; set; }        
        public string OldProdCode { get; set; }
        public Nullable<int> OldQuantity { get; set; }
        public string NewProdCode { get; set; }
        public Nullable<int> NewQuantity { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}