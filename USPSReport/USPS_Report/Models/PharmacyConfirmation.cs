//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace USPS_Report.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PharmacyConfirmation
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductDescription { get; set; }
        public Nullable<bool> NeedsProduct { get; set; }
        public Nullable<int> Qty_Number { get; set; }
    }
}
