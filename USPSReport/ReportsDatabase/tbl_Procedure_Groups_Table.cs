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
    
    public partial class tbl_Procedure_Groups_Table
    {
        public int ID_Billing_Code { get; set; }
        public int ID_Group_No { get; set; }
        public string Procedure_Code { get; set; }
        public string Modifier_1 { get; set; }
        public string Modifier_2 { get; set; }
        public string Modifier_3 { get; set; }
        public string Modifier_4 { get; set; }
        public Nullable<double> Multiplier { get; set; }
        public short PA_Required { get; set; }
        public bool Bill_At_End_Of_Month { get; set; }
        public bool Two_Dates_Needed { get; set; }
        public bool Capped_Rental_Item { get; set; }
        public Nullable<short> HtWtRequired { get; set; }
        public Nullable<short> HoldForManualReview { get; set; }
        public Nullable<short> DontCombineSameHCPCs { get; set; }
        public Nullable<decimal> Professional_Fee { get; set; }
        public string NDC { get; set; }
        public string Comments_old { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastChange { get; set; }
        public Nullable<short> DontSendCMNbyEDI { get; set; }
        public Nullable<int> ID_ChangedBy { get; set; }
        public string Comments { get; set; }
        public Nullable<short> DontAddQtyIfCombined { get; set; }
        public Nullable<short> Capped_Rental_Months { get; set; }
        public string RevenueCode { get; set; }
        public Nullable<short> DontSendClaimEDI { get; set; }
        public Nullable<short> Capped_AllowForMaintenance { get; set; }
        public Nullable<int> ID_HoldReason { get; set; }
        public Nullable<short> Compliance_Required_Months { get; set; }
        public Nullable<short> MeasurementQualifier { get; set; }
        public Nullable<short> DontBillComponents { get; set; }
        public Nullable<double> Capped_4thMonthReductionRate { get; set; }
        public Nullable<System.DateTime> Capped_RuleEffectiveDate { get; set; }
        public Nullable<short> RequiresFTF { get; set; }
        public Nullable<System.DateTime> Compliance_EffectiveDate { get; set; }
        public Nullable<short> EPSDT { get; set; }
        public Nullable<System.DateTime> FTF_EffectiveDate { get; set; }
        public Nullable<short> RUL_Months { get; set; }
        public Nullable<int> ID_DefaultCMNFreq { get; set; }
    }
}
