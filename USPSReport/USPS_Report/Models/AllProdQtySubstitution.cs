using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Models
{
    public class AllProdQtySubstitution
    {
        public int Account { get; set; }
        public int WorkOrder { get; set; }
        public string oldProdCode { get; set; }
        public Nullable<int> oldQuantity { get; set; }
        public string newProdCode { get; set; }
        public Nullable<int> newQuantity { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}