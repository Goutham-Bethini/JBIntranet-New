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
    
    public partial class sp_GetAcChanges_Result
    {
        public int Account { get; set; }
        public string First_Name { get; set; }
        public string Middle { get; set; }
        public string Last_name { get; set; }
        public Nullable<System.DateTime> AccountUpdated { get; set; }
        public string AccountUpdatedBy { get; set; }
        public Nullable<System.DateTime> AccountInfUpdated { get; set; }
        public string AccountInfUpdatedBy { get; set; }
        public Nullable<System.DateTime> InsuranceUpdated { get; set; }
        public string InsuranceUpdatedBy { get; set; }
        public string Payer { get; set; }
    }
}
