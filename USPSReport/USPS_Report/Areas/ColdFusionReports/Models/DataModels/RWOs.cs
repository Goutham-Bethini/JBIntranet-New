using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class RWOs
    {
        public class RWOsVM
        {
            [Required]
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The Account Number field is only intergers.")]
            public string AccountNumber { get; set; }           
            
            public IList<RWOsData> Details { get; set; }

        }
        public class RWOsData
        {
            public int Account { get; set; }
            public string ProductCode { get; set; }
            public string ProductDescription { get; set; }
            public string ChangedBy { get; set; }
            public DateTime? LastChanged { get; set; }
            public string AddedBy { get; set; }
            public DateTime? Added { get; set; }

        }     

        public static IList<RWOsData> GetRWOsData(int acNumber)
        {
            List<RWOsData> lstRWOsData = new List<RWOsData>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstRWOsData = (from item in _db.sp_GetAcRWOs(acNumber)
                                           select new RWOsData
                                           {
                                               Account = item.Account,
                                               ProductCode = item.ProductCode,
                                               ProductDescription = item.productdescription,                                               
                                               ChangedBy = item.LastChangedBy,
                                               LastChanged = item.LastChange,
                                               AddedBy = item.LegalName,
                                               Added = item.CreatedBy
                                           }
                               ).ToList();
                }           
                return lstRWOsData;
            }
            catch (Exception ex)
            {
                return new List<RWOsData>();
            }
        }
    }
}