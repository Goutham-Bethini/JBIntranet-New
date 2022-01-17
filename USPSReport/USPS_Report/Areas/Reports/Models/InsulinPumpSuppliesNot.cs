using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.Reports.Models
{
    public class InsulinPumpSuppliesNot
    {
        public class InsulinPumpSuppliesNotVM
        {           
            public IList<InsulinPumpSupply> InsulinPumpSupplies { get; set; }
       
        }
        public class InsulinPumpSupply
        {
            public int Account { get; set; }
            public DateTime? NextRepeatDate { get; set; }
            public int? ID_ProductCategory { get; set; }
            public string ProductCode { get; set; }
            public int? Qty { get; set; }
            public string PayerName { get; set; }
            public int? ID_Payer { get; set; }
        }

        public static IList<InsulinPumpSupply> GetInsulinPumpSupplies()
        {
            List<InsulinPumpSupply> lstInsulinPumpSupplies = new List<InsulinPumpSupply>();
            try
            {
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    lstInsulinPumpSupplies = (from item in _db.usp_GetInsulinPumpSupplies()
                                                select new InsulinPumpSupply
                                                {
                                                    Account = item.Account,
                                                    NextRepeatDate = item.NextRepeatDate,
                                                    ID_ProductCategory = item.ID_ProductCategory,
                                                    ProductCode = item.ProductCode,
                                                    Qty = item.Qty,
                                                    PayerName = item.PayerName,
                                                    ID_Payer = item.ID_Payer
                                                }
                               ).ToList();
                }
                return lstInsulinPumpSupplies;
            }
            catch (Exception ex)
            {
                return new List<InsulinPumpSupply>();
            }
        }
    }
}