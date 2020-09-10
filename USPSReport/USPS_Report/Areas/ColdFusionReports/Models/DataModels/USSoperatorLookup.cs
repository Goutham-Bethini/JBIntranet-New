using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class USSoperatorLookup
    {
        public class USSoperatorLookupVM
        {
            public IList<USSoperatorLookupData> Details { get; set; }
            
        }
        public class USSoperatorLookupData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Department { get; set; }
            public string Active { get; set; }
        }
        public static IList<USSoperatorLookupData> GetUSSoperatorLookupData()
        {
            List<USSoperatorLookupData> lstUSSoperatorLookupData = new List<USSoperatorLookupData>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstUSSoperatorLookupData = (from item in _db.sp_ussOperatorLookup()
                                   select new USSoperatorLookupData
                                   {
                                       Id = item.id,
                                       Name = item.LegalName,
                                       Department = item.DeptName,
                                       Active = item.Active
                                   }
                               ).ToList();
                }
                return lstUSSoperatorLookupData;
            }
            catch (Exception ex)
            {
                return new List<USSoperatorLookupData>();
            }
        }
    }
}