﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HHSQLDBEntities : DbContext
    {
        public HHSQLDBEntities()
            : base("name=HHSQLDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<JBCCServiceProductLine> JBCCServiceProductLine { get; set; }
        public virtual DbSet<JBCCServiceProductLine_ProductCategory> JBCCServiceProductLine_ProductCategory { get; set; }
        public virtual DbSet<JBCCServiceSmartActionProductsToConfirm> JBCCServiceSmartActionProductsToConfirm { get; set; }
        public virtual DbSet<tbl_Account_Note> tbl_Account_Note { get; set; }
        public virtual DbSet<tbl_Billing_Code_Table> tbl_Billing_Code_Table { get; set; }
        public virtual DbSet<tbl_Clinical_Assessments> tbl_Clinical_Assessments { get; set; }
        public virtual DbSet<tbl_DeliveryLocation_Table> tbl_DeliveryLocation_Table { get; set; }
        public virtual DbSet<tbl_DeliveryMethod_Table> tbl_DeliveryMethod_Table { get; set; }
        public virtual DbSet<tbl_DeliveryTimes_Table> tbl_DeliveryTimes_Table { get; set; }
        public virtual DbSet<tbl_Inv_VendorProduct_Table> tbl_Inv_VendorProduct_Table { get; set; }
        public virtual DbSet<ClaimsToSubmit> ClaimsToSubmit { get; set; }
        public virtual DbSet<ClaimsToSubmit_Fixed> ClaimsToSubmit_Fixed { get; set; }
        public virtual DbSet<ERP_OrderLines> ERP_OrderLines { get; set; }
        public virtual DbSet<ERP_OrdersSent> ERP_OrdersSent { get; set; }
        public virtual DbSet<ExpiredProduct> ExpiredProduct { get; set; }
        public virtual DbSet<QuantityOnHand> QuantityOnHand { get; set; }
        public virtual DbSet<JBCCServicePayers> JBCCServicePayers { get; set; }
        public virtual DbSet<JBCCServiceTypes> JBCCServiceTypes { get; set; }
        public virtual DbSet<tbl_MedDoc_Type_Table> tbl_MedDoc_Type_Table { get; set; }
        public virtual DbSet<tbl_Procedure_Groups_Table> tbl_Procedure_Groups_Table { get; set; }
        public virtual DbSet<tbl_ProductCategory_Table> tbl_ProductCategory_Table { get; set; }
        public virtual DbSet<tbl_Transaction_File> tbl_Transaction_File { get; set; }
        public virtual DbSet<tbl_UPS_WorkOrders> tbl_UPS_WorkOrders { get; set; }
        public virtual DbSet<WorkOrdersReleased> WorkOrdersReleased { get; set; }
        public virtual DbSet<Eligibility_EOD_Process> Eligibility_EOD_Process { get; set; }
        public virtual DbSet<tbl_Pricing_Table> tbl_Pricing_Table { get; set; }
        public virtual DbSet<BCN_Claims_FTP> BCN_Claims_FTP { get; set; }
        public virtual DbSet<tbl_Account_Insurance> tbl_Account_Insurance { get; set; }
        public virtual DbSet<tbl_Account_Member> tbl_Account_Member { get; set; }
        public virtual DbSet<tbl_Account_Information> tbl_Account_Information { get; set; }
        public virtual DbSet<FedExLogins> FedExLogins { get; set; }
        public virtual DbSet<tbl_PS_WorkOrder> tbl_PS_WorkOrder { get; set; }
        public virtual DbSet<tbl_Account_Note_History> tbl_Account_Note_History { get; set; }
        public virtual DbSet<tbl_Claims> tbl_Claims { get; set; }
        public virtual DbSet<tbl_Encounters> tbl_Encounters { get; set; }
        public virtual DbSet<tbl_Inv_UOM_Table> tbl_Inv_UOM_Table { get; set; }
        public virtual DbSet<tbl_Inv_Vendor_Table> tbl_Inv_Vendor_Table { get; set; }
        public virtual DbSet<tbl_Name_Frequency> tbl_Name_Frequency { get; set; }
        public virtual DbSet<tbl_PS_RepeatingOrders> tbl_PS_RepeatingOrders { get; set; }
        public virtual DbSet<tbl_Referral_Source_Table> tbl_Referral_Source_Table { get; set; }
        public virtual DbSet<v__AccountMemberEffectiveInsurance_Ins1> v__AccountMemberEffectiveInsurance_Ins1 { get; set; }
        public virtual DbSet<tbl_Allowable_Amounts> tbl_Allowable_Amounts { get; set; }
        public virtual DbSet<tbl_MedDoc_History> tbl_MedDoc_History { get; set; }
        public virtual DbSet<tbl_Operator_Table> tbl_Operator_Table { get; set; }
        public virtual DbSet<tbl_Name_PayerTypes> tbl_Name_PayerTypes { get; set; }
        public virtual DbSet<tbl_Payer_Table> tbl_Payer_Table { get; set; }
        public virtual DbSet<tbl_Product_Table> tbl_Product_Table { get; set; }
        public virtual DbSet<tbl_Account_Member_Insurance> tbl_Account_Member_Insurance { get; set; }
        public virtual DbSet<tbl_PS_WorkOrderLine> tbl_PS_WorkOrderLine { get; set; }
        public virtual DbSet<tbl_AssessmentDue_Attempts> tbl_AssessmentDue_Attempts { get; set; }
    }
}
