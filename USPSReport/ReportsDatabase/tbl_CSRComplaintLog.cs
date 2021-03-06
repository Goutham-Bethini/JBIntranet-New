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
    
    public partial class tbl_CSRComplaintLog
    {
        public int id { get; set; }
        public Nullable<int> Account { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string PayerType { get; set; }
        public string TrackingNumber { get; set; }
        public Nullable<bool> Damaged { get; set; }
        public Nullable<bool> Driver { get; set; }
        public Nullable<bool> WrongProductShipped { get; set; }
        public Nullable<bool> QualityOfProdut { get; set; }
        public Nullable<bool> WrongArea { get; set; }
        public Nullable<bool> MissingProduct { get; set; }
        public string FedExTxt { get; set; }
        public Nullable<bool> ProductIncrease { get; set; }
        public Nullable<bool> ProductMispick { get; set; }
        public Nullable<bool> ProductDefective { get; set; }
        public string ProductTxt { get; set; }
        public Nullable<bool> ImpoliteORoffensive { get; set; }
        public Nullable<bool> HoldTimes { get; set; }
        public string customerServiceTxt { get; set; }
        public string OtherTxt { get; set; }
        public string ComplaintHasBeen { get; set; }
        public string Product { get; set; }
        public string DeliveryCompany { get; set; }
        public string BCNProvider { get; set; }
        public bool Other { get; set; }
        public bool BCNProviderIssue { get; set; }
        public bool InsLimitGuidelines { get; set; }
        public bool PhysicianIssue { get; set; }
        public bool NeverRecivedSupplies { get; set; }
        public bool NoFollowUpWithMem { get; set; }
        public bool ReturnedFromVM { get; set; }
        public bool NoFollowUp { get; set; }
        public bool Website { get; set; }
        public bool VirtualCallBack { get; set; }
        public bool SAJamesSelfService { get; set; }
        public bool SAJamesPhonePromts { get; set; }
        public bool VPaymentCalles { get; set; }
        public bool VConfirmationCalls { get; set; }
        public bool DidntFollowDelIns { get; set; }
        public Nullable<System.DateTime> ComplaintDate { get; set; }
        public Nullable<System.DateTime> ResolutionDate { get; set; }
        public Nullable<System.DateTime> InitialRespDate { get; set; }
        public Nullable<System.DateTime> WrittenRespDate { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<bool> Call { get; set; }
        public Nullable<bool> Email { get; set; }
        public Nullable<bool> Fax { get; set; }
        public Nullable<bool> Mail { get; set; }
        public Nullable<bool> CallRcvdWebsite { get; set; }
        public Nullable<bool> SocialMedia { get; set; }
        public Nullable<bool> InsCompany { get; set; }
        public Nullable<bool> CallRcvdOther { get; set; }
        public Nullable<bool> ComplaintProduct { get; set; }
        public Nullable<bool> ComplaintShipping { get; set; }
        public Nullable<bool> ComplaintService { get; set; }
        public Nullable<bool> ComplaintSmartAction { get; set; }
    }
}
