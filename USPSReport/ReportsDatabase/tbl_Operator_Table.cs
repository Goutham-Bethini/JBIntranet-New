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
    
    public partial class tbl_Operator_Table
    {
        public int ID { get; set; }
        public string OperatorName { get; set; }
        public string LegalName { get; set; }
        public Nullable<bool> DepartmentMgr { get; set; }
        public Nullable<int> ID_DeliveryLocation { get; set; }
        public Nullable<int> ID_Location { get; set; }
        public Nullable<int> ID_Dept { get; set; }
        public Nullable<int> ID_Group { get; set; }
        public string Comments { get; set; }
        public string Password { get; set; }
        public Nullable<bool> NoPSMessage { get; set; }
        public string Email { get; set; }
        public string OfficePhone { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> ID_DeletedBy { get; set; }
        public Nullable<System.DateTime> InactiveDate { get; set; }
        public string CurrentPassword { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string Password3 { get; set; }
        public string Password4 { get; set; }
        public string Password5 { get; set; }
        public string Password6 { get; set; }
        public string LastPasswordChangeDate { get; set; }
        public Nullable<short> ChangePasswordNextLogon { get; set; }
        public Nullable<int> ID_OperatorTeam { get; set; }
        public string OtherUserName { get; set; }
        public Nullable<short> PromptForLocation { get; set; }
        public string IntegrationUserName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ID_CreatedBy { get; set; }
        public Nullable<System.DateTime> LastChange { get; set; }
        public Nullable<int> ID_ChangedBy { get; set; }
        public Nullable<short> DisplayDashboardAtLogin { get; set; }
        public string Connect_ARA_UserName { get; set; }
        public string Connect_ARA_Password { get; set; }
        public string Connect_HDMSImaging_UserName { get; set; }
        public string Connect_HDMSImaging_Password { get; set; }
        public Nullable<short> TeamLeader { get; set; }
        public Nullable<int> Connect_StowPoint_ID_PermGroup { get; set; }
        public Nullable<short> Imaging_AskScanOption { get; set; }
        public Nullable<bool> NoPSWOPreview { get; set; }
        public Nullable<short> DisplayCollectionWksAtLogin { get; set; }
    }
}
