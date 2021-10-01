using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using USPS_Report.Models;

namespace USPS_Report.Areas.Reports.Models
{
    public class RWOsWith2099Dates
    {
        public class RWOsWith2099DatesVM
        {
            [Required]
            public string Team { get; set; }
            public IList<RWOsWith2099DatesData> Details { get; set; }
        }

        public class RWOsWith2099DatesData
        {
            public int Account { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }            
            public DateTime? NextRepeatDate { get; set; }
            public string ProductCategory { get; set; }
            public DateTime? LastChange { get; set; }
        }

        public class RWOsWith2099DatesAudit
        {
            public int RowId { get; set; }
            public string OperatorName { get; set; }
            public string TeamReport { get; set; }
            public DateTime? ReportDatetime { get; set; }
        }

        public static IList<RWOsWith2099DatesData> GetRWOsWith2099Dates(string operatorName,string team)
        {
            List<RWOsWith2099DatesData> lstRWOsWith2099DatesData = new List<RWOsWith2099DatesData>();
            try
            {
                using (ReportsEntities _db = new ReportsEntities())
                {
                    lstRWOsWith2099DatesData = (from item in _db.sp_GetRWOsWith2099Dates(operatorName, team)
                                          select new RWOsWith2099DatesData
                                          {
                                              Account = item.Account,
                                              LastName = item.Last_Name,
                                              FirstName = item.First_Name,
                                              NextRepeatDate = item.NextRepeatDate,
                                              ProductCategory = item.Product_Category,
                                              LastChange = item.LastChange
                                          }
                               ).ToList();
                }
                return lstRWOsWith2099DatesData;
            }
            catch (Exception ex)
            {
                return new List<RWOsWith2099DatesData>();
            }
        }

        public static IList<RWOsWith2099DatesAudit> GetRWOsWith2099DatesAudit()
        {
            List<RWOsWith2099DatesAudit> lstRWOsWith2099DatesAudit = new List<RWOsWith2099DatesAudit>();
            try
            {
                using (ReportsEntities _db = new ReportsEntities())
                {
                    lstRWOsWith2099DatesAudit = (from item in _db.sp_GetRWOs2099Audit()
                                                select new RWOsWith2099DatesAudit
                                                {
                                                    RowId = item.RowId,
                                                    OperatorName = item.OperatorName,
                                                    TeamReport = item.TeamReport,
                                                    ReportDatetime = item.ReportDatetime
                                                }
                               ).ToList();
                }
                return lstRWOsWith2099DatesAudit;
            }
            catch (Exception ex)
            {
                return new List<RWOsWith2099DatesAudit>();
            }
        }
    }
}