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
    
    public partial class tbl_Account_Information
    {
        public int Account { get; set; }
        public string Reference { get; set; }
        public Nullable<int> ID_Status_Code_1 { get; set; }
        public Nullable<int> ID_Status_Code_2 { get; set; }
        public Nullable<int> ID_Status_Code_3 { get; set; }
        public Nullable<int> ID_Status_Code_4 { get; set; }
        public Nullable<int> ID_Status_Code_5 { get; set; }
        public Nullable<int> ID_Status_Code_6 { get; set; }
        public Nullable<int> ID_Status_Code_7 { get; set; }
        public Nullable<int> ID_Status_Code_8 { get; set; }
        public Nullable<int> ID_Status_Code_9 { get; set; }
        public Nullable<int> ID_Status_Code_10 { get; set; }
        public Nullable<int> ID_Referral_Source { get; set; }
        public Nullable<System.DateTime> Referral_Source_Date { get; set; }
        public string ReferralContactPerson { get; set; }
        public Nullable<short> Statements { get; set; }
        public Nullable<int> ID_Pricing { get; set; }
        public Nullable<short> Billable_Party_Member { get; set; }
        public Nullable<int> ID_Terms { get; set; }
        public string ServiceComments { get; set; }
        public Nullable<decimal> CreditLimit { get; set; }
        public string ThirdPartyStatusCodes { get; set; }
        public short InActiveAccount { get; set; }
        public Nullable<int> IDAccountContractHost { get; set; }
        public Nullable<short> TaxExemptAccount { get; set; }
        public Nullable<short> NoServiceCharges { get; set; }
        public string Notes { get; set; }
        public Nullable<int> IDOperator_Reoccur { get; set; }
        public Nullable<int> ID_AccountCCBatchPayment { get; set; }
        public Nullable<int> ID_CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> Last_Updated_Date { get; set; }
        public Nullable<int> Last_Updated_User { get; set; }
        public Nullable<short> PreferredContactMethod { get; set; }
    }
}
