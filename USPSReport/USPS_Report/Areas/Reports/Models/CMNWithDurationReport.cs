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
    public class CMNWithDurationReport
    {
        public IList<CMNWithDurationReportViewModel> GetDetails()
        {
            IList<CMNWithDurationReportViewModel> _list = new List<CMNWithDurationReportViewModel>();
            string sql = GetSqlQuery();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _list = _db.Database.SqlQuery<CMNWithDurationReportViewModel>(sql).ToList<CMNWithDurationReportViewModel>();
            }
            return _list;
        }


        private string GetSqlQuery()
        {
            string sql = @"SELECT 
		                        t1.Account,
		                        t1.DocTypeDescription, 
		                        t1.EffectiveDate, 
		                        t1.Duration
	                        FROM
					                        [dbo].[v_CMN_CurrentCMN]			t1
		                        LEFT JOIN	[dbo].[tbl_Account_Information]		t2	ON	t2.Account = t1.Account

	                        WHERE		(t1.EffectiveDate+(30*t1.Duration))>=GETDATE()	
		                        AND		t1.Duration = 99
		                        AND		t2.InActiveAccount != 1
	
	                        ORDER BY
		
			                        t1.Account";

            return sql;
        }
    }

    public class CMNWithDurationReportViewModel
    {
        public string DocType { get; set; }

        public int Account { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EffectiveDate { get; set; }
        public Int16 Duration { get; set; }
    }

}