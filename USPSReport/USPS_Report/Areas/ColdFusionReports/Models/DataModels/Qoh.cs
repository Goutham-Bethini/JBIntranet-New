using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class Qoh
    {
        public class QohVM
        {
            [Required]            
            public string ProductCode { get; set; }

            public IList<QohData> Details { get; set; }

        }
        public class QohData
        {            
            public string ProductCode { get; set; }
            public string Description { get; set; }
            public string Discontinued { get; set; }
            public int QtyAvailableInOrc { get; set; }
            public int BOdInHDMS { get; set; }           
        }

        public static IList<QohData> GetQohData(string productCode)
        {
            List<QohData> lstQohData = new List<QohData>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstQohData = (from item in _db.sp_GetQOH(productCode)
                                   select new QohData
                                   {
                                       ProductCode = item.ProductCode,
                                       Description = item.Description,
                                       Discontinued = item.Discontinued,
                                       QtyAvailableInOrc = item.QtyAvailableInOrc,
                                       BOdInHDMS = item.BOdInHDMS
                                   }
                               ).ToList();
                }
                return lstQohData;
            }
            catch (Exception ex)
            {
                return new List<QohData>();
            }
        }
    }
}