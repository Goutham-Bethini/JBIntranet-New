//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace USPS_Report.Areas.ColdFusionReports.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employees_AlphaSplit
    {
        public int alpID { get; set; }
        public Nullable<int> alpDeptID { get; set; }
        public Nullable<int> alpEmpID { get; set; }
        public string alpAlphaStart { get; set; }
        public string alpAlphaEnd { get; set; }
        public string alpComment { get; set; }
        public Nullable<System.DateTime> alpEdited { get; set; }
        public string alpEditedBy { get; set; }
        public Nullable<System.DateTime> alpAdded { get; set; }
        public string alpAddedBy { get; set; }
        public Nullable<System.DateTime> alpDeleted { get; set; }
        public string alpDeletedBy { get; set; }
    }
}
