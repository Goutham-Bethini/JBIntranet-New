using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{
    public class RWOAsRushOrders
    {
        public static IList<Rwo_RushOrdersVM> GetRWOSetAsRushOrders()
        {

            try
            {
                IList<Rwo_RushOrdersVM> _list = new List<Rwo_RushOrdersVM>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {


                    _list = _db.Database.SqlQuery<Rwo_RushOrdersVM>("SELECT  rwo.ID,   rwo.Account,  pro.ProductCode,  rwo.NextRepeatDate, " +
                                           " rwo.CreateDate, op1.LegalName AS CreatedBy, rwo.LastChange,  op2.LegalName AS ChangedBy,  altSent "+
                                             " FROM tbl_ps_repeatingorders      rwo "+
              "  JOIN        tbl_deliverymethod_table    del ON rwo.id_deliverymethod = del.id "+
                " JOIN        tbl_product_table           pro ON pro.id = rwo.id_product "+
               "  LEFT JOIN   tbl_operator_table          op1 ON op1.id = rwo.id_createby "+
               " LEFT JOIN   tbl_operator_table          op2 ON op2.id = rwo.id_changed "+
               " LEFT JOIN   Intranet..RWO_SetAsRush     alt ON altRWOid = rwo.id "+
            " WHERE "+
                    " del.Deliverymethod = 'FedEx - Rush Order'").ToList<Rwo_RushOrdersVM>();


                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<Rwo_RushOrdersVM>();
            }


        }
    }


    public class Rwo_RushOrdersVM
    {
        public int ID { get; set; }
        public int Account { get; set; }
        public string ProductCode { get; set; }
        public DateTime? NextRepeatDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastChange { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? altSent { get; set; }
    }

    public class RwoRushOrders
    {
        public IList<Rwo_RushOrdersVM> rwo_RushOrdersVM { get; set; }
    }
}