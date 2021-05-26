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
    
    public partial class tbl_Claims
    {
        public int ID { get; set; }
        public int Account { get; set; }
        public short Member { get; set; }
        public Nullable<short> AIns_Entry_Number { get; set; }
        public Nullable<int> ID_Payer { get; set; }
        public string Type_Of_Claim { get; set; }
        public Nullable<short> StatusIndicator { get; set; }
        public Nullable<System.DateTime> DateChanged { get; set; }
        public Nullable<short> SubmitAStatusQuery { get; set; }
        public Nullable<short> DontSendEDI { get; set; }
        public Nullable<bool> AcceptAssignment { get; set; }
        public int ID_HoldStatus { get; set; }
        public Nullable<int> ID_HoldEnteredBy { get; set; }
        public Nullable<short> LastUBReason { get; set; }
        public Nullable<System.DateTime> UBDateChanged { get; set; }
        public Nullable<short> ClaimFrequencyType { get; set; }
        public string ReplacementClaimRef { get; set; }
        public string PrimaryRejectionIdentifier { get; set; }
        public string ExternalReference { get; set; }
        public Nullable<int> ID_ClaimAdjudicateSubmit { get; set; }
        public Nullable<int> ID_ClaimAdjudicateRespond { get; set; }
        public Nullable<int> NCPDPServiceReferenceNumber { get; set; }
        public Nullable<int> ID_CollectionStatus { get; set; }
        public Nullable<System.DateTime> Collection_DueDate { get; set; }
        public Nullable<System.DateTime> CollectionStatusChangedDate { get; set; }
        public Nullable<int> ID_CollectionStatusChangedBy { get; set; }
        public Nullable<int> ID_CurrentClaimsWorksheet { get; set; }
        public Nullable<int> DelayReasonIdentifier { get; set; }
        public Nullable<short> UB_5 { get; set; }
        public Nullable<short> UB_7 { get; set; }
        public Nullable<short> UB_9 { get; set; }
        public Nullable<short> UB_10 { get; set; }
        public Nullable<short> UB_11 { get; set; }
        public Nullable<short> UB_12 { get; set; }
        public Nullable<short> UB_13 { get; set; }
        public Nullable<short> UB_14 { get; set; }
        public Nullable<short> UB_15 { get; set; }
        public Nullable<short> UB_18 { get; set; }
        public Nullable<short> UB_21 { get; set; }
        public Nullable<short> UB_30 { get; set; }
        public Nullable<short> UB_31 { get; set; }
        public Nullable<short> UB_32 { get; set; }
        public Nullable<short> UB_40 { get; set; }
        public Nullable<short> UB_45 { get; set; }
        public Nullable<short> UB_51 { get; set; }
        public Nullable<short> UB_52 { get; set; }
        public Nullable<short> UB_60 { get; set; }
        public Nullable<short> UB_61 { get; set; }
        public Nullable<short> UB_70 { get; set; }
        public Nullable<short> UB_90 { get; set; }
        public Nullable<short> UB_92 { get; set; }
    }
}
