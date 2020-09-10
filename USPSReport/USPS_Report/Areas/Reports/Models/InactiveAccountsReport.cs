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
    public class InactiveAccountsReport
    {
        public IList<InactiveAccountsReportViewModel> GetDetails()
        {
            IList<InactiveAccountsReportViewModel> _list = new List<InactiveAccountsReportViewModel>();
            string sql = GetSqlQuery();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _list = _db.Database.SqlQuery<InactiveAccountsReportViewModel>(sql).ToList<InactiveAccountsReportViewModel>();
            }
            return _list;
        }


        private string GetSqlQuery()
        {
            string sql = @"select distinct rwo.account
                                from tbl_PS_RepeatingOrders rwo
                                where rwo.account in (select account from tbl_Account_Information inf
                                where inf.InActiveAccount = 1) ";

            return sql;
        }
    }

    public class InactiveAccountsReportViewModel
    {
        public int Account { get; set; }
    }

}