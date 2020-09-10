//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportsDatabase
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Pricing_Table
    {
        public int ID_Product { get; set; }
        public int ID_PricingNo { get; set; }
        public Nullable<decimal> Purchase { get; set; }
        public Nullable<decimal> Rental { get; set; }
        public Nullable<double> Cost_Plus_Percent { get; set; }
        public string HCPC { get; set; }
        public string PricingComment { get; set; }
        public Nullable<System.DateTime> SaleStartDate { get; set; }
        public Nullable<System.DateTime> SaleEndDate { get; set; }
        public Nullable<double> SalePercentOff { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<decimal> Rental_Daily { get; set; }
        public Nullable<decimal> Rental_Weekly { get; set; }
        public Nullable<short> UseAWP { get; set; }
        public Nullable<double> AWPMultiplier { get; set; }
    }
}
