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
    
    public partial class v__AccountMemberEffectiveInsurance_Ins1
    {
        public int Account { get; set; }
        public short Member { get; set; }
        public short Sequence_Number { get; set; }
        public short EntryNumber { get; set; }
        public int ID_Payer { get; set; }
        public string PayerName { get; set; }
        public string PayerAddress1 { get; set; }
        public string PayerAddress2 { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZip { get; set; }
        public string PayerPhone { get; set; }
        public Nullable<int> ID_PayerType { get; set; }
        public Nullable<int> ID_Procedure_Group { get; set; }
        public Nullable<int> ID_Allowable { get; set; }
        public string MemberPolicyNumber { get; set; }
        public Nullable<System.DateTime> Effective_Date { get; set; }
        public Nullable<System.DateTime> Expiration_Date { get; set; }
        public double CoveragePercent { get; set; }
        public decimal DeductibleAmount { get; set; }
        public Nullable<System.DateTime> LastVerified_Date { get; set; }
        public Nullable<System.DateTime> LastVerifiedReceived_DateTime { get; set; }
        public string LastVerified_Details { get; set; }
        public Nullable<System.DateTime> LastRealTimeVerified_DateTime { get; set; }
        public int ID_ManagedCareHealthPlan { get; set; }
        public int ID_ManagedCareMedicalGroup { get; set; }
        public string SubscriberFirstName { get; set; }
        public string SubscriberMiddle { get; set; }
        public string SubscriberLastName { get; set; }
        public Nullable<System.DateTime> SubscriberBirthDate { get; set; }
        public short DeductibleMet { get; set; }
        public Nullable<short> SubscriberRelationship { get; set; }
        public string ExternalReference { get; set; }
    }
}
