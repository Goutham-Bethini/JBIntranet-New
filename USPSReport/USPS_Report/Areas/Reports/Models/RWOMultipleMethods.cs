using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Linq.Expressions;
using System.Data.SqlClient;

namespace USPS_Report.Areas.Reports.Models
{
    public class RWOMultipleMethodsReport
    {
        public static IList<RWOMultipleMethodsData> GetRWOMultipleMethods()
        {

            try
            {
                IList<RWOMultipleMethodsData> _list = new List<RWOMultipleMethodsData>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                   

                     _list =   _db.Database.SqlQuery<RWOMultipleMethodsData>("SELECT rwo.Account, mem.First_Name, mem.Last_Name, COUNT(DISTINCT mth.DeliveryMethod) AS Methods,  " +
                        "MIN(mth.DeliveryMethod)AS Method1," +
    "MAX(mth.DeliveryMethod) AS Method2 FROM tbl_ps_repeatingorders  rwo" +

    " JOIN    tbl_DeliveryMethod_Table                    mth ON mth.id = rwo.id_deliveryMethod" +
    " JOIN    tbl_account_member                          mem ON rwo.account = mem.account" +
                                                           " and rwo.member = mem.member" +
    " JOIN    tbl_Product_Table                           prd ON prd.ID = rwo.ID_Product" +
    " JOIN    JBCCServiceProductLine_ProductCategory      jsp ON jsp.ProductCategoryId = prd.ID_ProductCategory" +

" WHERE       jsp.ProductLineId != 3" +

" GROUP BY" +
    " rwo.Account, mem.First_Name, mem.Last_Name HAVING COUNT(DISTINCT mth.DeliveryMethod) > 1" +
" ORDER BY" +
    " rwo.Account").ToList<RWOMultipleMethodsData>();


                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<RWOMultipleMethodsData>();
            }


        }


    }
    public class RWOMultipleMethodsData
    {
        public int Account { get; set; }


        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public int? Methods{ get; set; }

        public string Method1 { get; set; }

        public string Method2 { get; set; }

    }


    public class RWOMultipleMethodsVM
    {
        public IList<RWOMultipleMethodsData> rwoMultipleMethods{ get; set; }
    }
}