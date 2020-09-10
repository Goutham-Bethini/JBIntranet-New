using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{

    public class RWOwithOlderDates
    {

        public static IList<RWOwithOlderDatesVM> GetRWOwithOlderDates()
        {

            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    var _list = (from rwo in _db.tbl_PS_RepeatingOrders
                                 from inf in _db.tbl_Account_Information.Where(w => w.Account == rwo.Account).DefaultIfEmpty()
                                 from prod in _db.tbl_Product_Table.Where(p => p.ID == rwo.ID_Product).DefaultIfEmpty()
                                 from op1 in _db.tbl_Operator_Table.Where(o => o.ID == rwo.ID_CreateBy).DefaultIfEmpty()
                                 from op2 in _db.tbl_Operator_Table.Where(op => op.ID == rwo.ID_Changed).DefaultIfEmpty()
                                 where(inf.InActiveAccount == 0 && rwo.NextRepeatDate.Value.Year >= 2020)
                                 select new RWOwithOlderDatesVM
                                 {
                                  
                                     Account = rwo.Account,
                                     ProductCode = prod.ProductCode,
                                     NextRepeatDate = rwo.NextRepeatDate,
                                     CreatedBy = op1.LegalName,
                                     CreateDate = rwo.CreateDate,
                                     ChangedBy = op2.LegalName,
                                     LastChangedDate = rwo.LastChange
                                 }).OrderBy(t=>t.Account).ToList();

                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<RWOwithOlderDatesVM>();
            }


        }

    }

    public class RWOwithOlderDatesModel
    {
        public int? account { get; set; }
        public IList<RWOwithOlderDatesVM> rwoWithOlderDates { get; set; }
    }
    public class RWOwithOlderDatesVM
    {
     
        public int? Account { get; set; }

        public string ProductCode { get; set; }

        public DateTime? NextRepeatDate { get; set; }

        public string CreatedBy { get; set; }


        public DateTime? CreateDate { get; set; }


        public string ChangedBy { get; set; }

        public DateTime? LastChangedDate { get; set; }


    }
    
}