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
    
    public partial class RWO_Product_Substitutions
    {
        public int subID { get; set; }
        public Nullable<int> subOldProd { get; set; }
        public Nullable<int> subNewProd { get; set; }
        public Nullable<int> subDL { get; set; }
        public Nullable<byte> subAddComment { get; set; }
        public Nullable<int> subRWOcount { get; set; }
        public Nullable<System.DateTime> subAdded { get; set; }
        public string subAddedBy { get; set; }
        public Nullable<System.DateTime> subApproved { get; set; }
        public string subApprovedBy { get; set; }
        public Nullable<System.DateTime> subDenied { get; set; }
        public string subDeniedBy { get; set; }
        public Nullable<System.DateTime> subDeleted { get; set; }
        public string subDeletedBy { get; set; }
        public Nullable<int> subOldProdQty { get; set; }
        public Nullable<int> subNewProdQty { get; set; }
        public Nullable<int> QtyOldProd { get; set; }
        public Nullable<int> QtyNewProd { get; set; }
        public bool allProds { get; set; }
    }
}
