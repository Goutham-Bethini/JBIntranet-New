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
    
    public partial class IntranetEntities : DbContext
    {
        public IntranetEntities()
            : base("name=IntranetEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<assessment_log> assessment_log { get; set; }
        public virtual DbSet<Employees_New> Employees_New { get; set; }
        public virtual DbSet<CHAMPS_Adj_Codes> CHAMPS_Adj_Codes { get; set; }
        public virtual DbSet<CHAMPS_Error_Codes> CHAMPS_Error_Codes { get; set; }
        public virtual DbSet<CHAMPS_Remit_Codes> CHAMPS_Remit_Codes { get; set; }
        public virtual DbSet<CHAMPS_Ignore_Errors> CHAMPS_Ignore_Errors { get; set; }
        public virtual DbSet<RWO_Product_Substitutions> RWO_Product_Substitutions { get; set; }
        public virtual DbSet<IndianaMedicaidSurvey> IndianaMedicaidSurveys { get; set; }
        public virtual DbSet<Eligibility_Orders> Eligibility_Orders { get; set; }
        public virtual DbSet<tbl_CSRComplaintLog> tbl_CSRComplaintLog { get; set; }
        public virtual DbSet<AOB_track> AOB_track { get; set; }
        public virtual DbSet<tbl_CSRInsDetail> tbl_CSRInsDetail { get; set; }
        public virtual DbSet<BCBS_ProviderList> BCBS_ProviderList { get; set; }
        public virtual DbSet<tbl_BCNCallLog> tbl_BCNCallLog { get; set; }
        public virtual DbSet<tbl_CSRCallLog> tbl_CSRCallLog { get; set; }
    }
}
