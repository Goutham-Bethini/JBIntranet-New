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
    
    public partial class ClaimsToSubmit
    {
        public int ctsID { get; set; }
        public Nullable<int> ctsCSBid { get; set; }
        public Nullable<int> ctsClaim { get; set; }
        public Nullable<int> ctsAccount { get; set; }
        public string ctsUBReason { get; set; }
        public string ctsUBFixedBy { get; set; }
        public Nullable<System.DateTime> ctsUBFixedDate { get; set; }
        public string ctsHoldReason { get; set; }
        public Nullable<decimal> ctsOpenBalance { get; set; }
        public Nullable<int> ctsOtherClaimsQty { get; set; }
        public string ctsOtherClaims { get; set; }
        public Nullable<int> ctsPaymentsReceived { get; set; }
        public string ctsPaymentPostedBy { get; set; }
        public Nullable<int> ctsTimesSubmitted { get; set; }
        public string ctsResubmittedBy { get; set; }
        public Nullable<int> ctsDaysSinceCrossOver { get; set; }
        public Nullable<byte> ctsWillSubmit { get; set; }
        public Nullable<byte> ctsIssues { get; set; }
        public string ctsTCN { get; set; }
        public string ctsManuallyCreatedBy { get; set; }
        public string ctsReleasedBy { get; set; }
        public Nullable<System.DateTime> ctsEligibilityDate { get; set; }
        public Nullable<System.DateTime> ctsWarning { get; set; }
        public string ctsNotes { get; set; }
    }
}
