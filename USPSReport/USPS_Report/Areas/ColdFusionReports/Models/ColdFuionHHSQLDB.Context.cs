﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace USPS_Report.Areas.ColdFusionReports.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ColdFuionHHSQLDBEntities : DbContext
    {
        public ColdFuionHHSQLDBEntities()
            : base("name=ColdFuionHHSQLDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<FedExMeterNumber> FedExMeterNumbers { get; set; }
        public virtual DbSet<FedExLogin> FedExLogins { get; set; }
    }
}
