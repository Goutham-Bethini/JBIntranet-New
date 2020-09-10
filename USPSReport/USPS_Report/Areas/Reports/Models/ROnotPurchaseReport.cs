using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.Entity.SqlServer;
using USPS_Report.Models;
using System.ComponentModel.DataAnnotations;

namespace USPS_Report.Areas.Reports.Models
{
    public class ROnotPurchaseReport
    {
        public IList<ROnotPurchaseViewModel> GetDetails()
        {
            IList<ROnotPurchaseViewModel> _list = new List<ROnotPurchaseViewModel>();
            string sql = GetQuery();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _list = _db.Database.SqlQuery<ROnotPurchaseViewModel>(sql).ToList<ROnotPurchaseViewModel>();
            }
            return _list;
        }

        private string GetQuery()
        {
            string sql = @"select ro.Account,ID_CreateBy,O.LegalName AS LegalName_CreatedBy,ID_Changed,O2.LegalName AS LegalName_ChangedBy   
                                    from tbl_PS_RepeatingOrders ro     
                                    join tbl_Operator_Table        O  
                                    on ID_CreateBy=O.ID     
                                    join tbl_Operator_Table        O2 
                                    on ID_Changed=O2.ID     
                                    where WorkOrderType<>1";

            return sql;
        }
    }

    public class ROnotPurchaseViewModel
    {
        public int Account { get; set; }
        public int ID_CreateBy { get; set; }
        public string LegalName_CreatedBy { get; set; }
        public int ID_Changed { get; set; }
        public string LegalName_ChangedBy { get; set; }
    }


}