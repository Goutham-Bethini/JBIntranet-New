using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using ReportsDatabase;
using USPS_Report.Models;

namespace USPS_Report.Areas.Reports.Models
{
    public class MIStateAudit
    {
        public class MIStateAuditVM
        {
            [Required]
            public string Report { get; set; }
            [Required]
            public string Quarter { get; set; }
            [Required]
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The Year field is only intergers.")]
            public string Year { get; set; }
            public string FileName { get; set; }
            public IList<MIStateAuditData> Details { get; set; }

        }
        public class MIStateAuditData
        {
            public string Report { get; set; }
            public int? Quarter { get; set; }
            public double? Year { get; set; }
            public DateTime? DateCreated { get; set; }
            public string FileName { get; set; }

        }

        public class MINewAccounts
        {
            public int Account { get; set; }
            public string Last_Name { get; set; }
            public string First_Name { get; set; }
            public DateTime? AccountCreated { get; set; }
            public DateTime? FirstShipment { get; set; }

        }
        public class MIPRODUCTSADDED
        {
            public int Account { get; set; }
            public string Last_Name { get; set; }
            public string First_Name { get; set; }
            public string ProductCode { get; set; }
            public DateTime? ProductAdded { get; set; }
            public DateTime? ProductFirstShipped { get; set; }

        }
        public class MIDeactivatedAccounts
        {
            public int Account { get; set; }
            public string Last_Name { get; set; }
            public string First_Name { get; set; }
            public string Reference { get; set; }
            public DateTime? DateDeactivated { get; set; }
            public DateTime? Deceased { get; set; }
            public DateTime? Expiration_Date { get; set; }
            public int? Orders { get; set; }

        }

        public static IList<MIStateAuditData> GetMIStateAuditData()
        {
            List<MIStateAuditData> lstMIStateAuditData = new List<MIStateAuditData>();
            try
            {                
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {                    
                    lstMIStateAuditData = (from item in _db.Database.SqlQuery<sp_MIStateAuditData_Result>("exec sp_MIStateAuditData").ToList<sp_MIStateAuditData_Result>()
                                           select new MIStateAuditData
                                           {
                                               Report = item.Report,
                                               Quarter = item.Quarter,
                                               Year = item.Year,
                                               //DateCreated = item.DateCreated == null ? "" : item.DateCreated.Value.ToString("MMMM dd, yyyy"),
                                               DateCreated = item.DateCreated,
                                               FileName = item.FileName
                                           }
                               ).ToList();
                }                
                return lstMIStateAuditData;
            }
            catch(Exception ex)
            {
                return new List<MIStateAuditData>();
            } 
        }         

        public static IList<MINewAccounts> GetMINewAccounts(DateTime startDate, DateTime endDate)
        {
            List<MINewAccounts> lstMINewAccounts = new List<MINewAccounts>();
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                _db.Database.CommandTimeout = 180;
                lstMINewAccounts = (from item in _db.Database.SqlQuery<sp_GetMINewAccounts_Result>("exec sp_GetMINewAccounts @startDate,@endDate", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate)).ToList<sp_GetMINewAccounts_Result>()
                                    select new MINewAccounts
                                    {
                                        Account = item.Account,
                                        Last_Name = item.Last_Name,
                                        First_Name = item.First_Name,
                                        AccountCreated = item.AccountCreated,
                                        FirstShipment = item.FirstShipment
                                    }
                           ).ToList();
            }
            return lstMINewAccounts;
        }
        public static IList<MIPRODUCTSADDED> GetMIPRODUCTSADDED(DateTime startDate, DateTime endDate)
        {
            List<MIPRODUCTSADDED> lstMIPRODUCTSADDED = new List<MIPRODUCTSADDED>();
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                _db.Database.CommandTimeout = 180;
                lstMIPRODUCTSADDED = (from item in _db.Database.SqlQuery<sp_GetMIPRODUCTSADDED_Result>("exec sp_GetMIPRODUCTSADDED @startDate,@endDate", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate)).ToList<sp_GetMIPRODUCTSADDED_Result>()
                                      select new MIPRODUCTSADDED
                                      {
                                          Account = item.Account,
                                          Last_Name = item.Last_Name,
                                          First_Name = item.First_Name,
                                          ProductCode = item.ProductCode,
                                          ProductAdded = item.ProductAdded,
                                          ProductFirstShipped = item.ProductFirstShipped
                                      }
                           ).ToList();
            }
            return lstMIPRODUCTSADDED;
        }
        public static IList<MIDeactivatedAccounts> GetMIDeactivatedAccounts(DateTime startDate, DateTime endDate)
        {
            List<MIDeactivatedAccounts> lstMIDeactivatedAccounts = new List<MIDeactivatedAccounts>();
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                _db.Database.CommandTimeout = 180;
                lstMIDeactivatedAccounts = (from item in _db.Database.SqlQuery<sp_GetMIDeactivatedAccounts_Result>("exec sp_GetMIDeactivatedAccounts @startDate,@endDate", new SqlParameter("startDate", startDate), new SqlParameter("endDate", endDate)).ToList<sp_GetMIDeactivatedAccounts_Result>()
                                            select new MIDeactivatedAccounts
                                      {
                                          Account = item.Account,
                                          Last_Name = item.Last_Name,
                                          First_Name = item.First_Name,
                                          Reference = item.Reference,
                                          DateDeactivated = item.DateDeactivated,
                                          Deceased = item.Deceased,
                                          Expiration_Date = item.Expiration_Date,
                                          Orders = item.Orders
                                      }
                           ).ToList();
            }
            return lstMIDeactivatedAccounts;
        }
    }
}