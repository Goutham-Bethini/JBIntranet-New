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
    public class RwosReport
    {
        public IList<RwosViewModel> GetDetails(string AccountToSearch)
        {
            IList<RwosViewModel> _list = new List<RwosViewModel>();
            string sql = GetDataQuery(AccountToSearch);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _list = _db.Database.SqlQuery<RwosViewModel>(sql).ToList<RwosViewModel>();
            }
            return _list;
        }

        private string GetDataQuery(string AccountToSearch)
        {
            string sql = "";
            if (string.IsNullOrEmpty(AccountToSearch))
            {
                sql = @"SELECT rwo.Account, ProductCode, op1.LegalName AS CreatedBy, rwo.CreateDate, op2.LegalName AS ChangedBy, rwo.LastChange, rwo.NextRepeatDate
				FROM tbl_PS_Repeatingorders				rwo
					LEFT JOIN tbl_Account_Information	inf on inf.Account=rwo.Account
					LEFT JOIN tbl_Product_Table			prod on prod.id=rwo.ID_Product
					LEFT JOIN tbl_Operator_Table		op1 on op1.id=rwo.ID_CreateBy
					LEFT JOIN tbl_Operator_Table		op2 on op2.id=rwo.ID_Changed
				WHERE  NextRepeatDate < convert(varchar,GETDATE(),101)   
					AND inf.InActiveAccount=0
				ORDER BY rwo.Account";

            }
            else
            {
                sql = @"SELECT rwo.Account, ProductCode, op1.LegalName AS CreatedBy, rwo.CreateDate, op2.LegalName AS ChangedBy, rwo.LastChange, rwo.NextRepeatDate
				FROM tbl_PS_Repeatingorders				rwo
					LEFT JOIN tbl_Account_Information	inf on inf.Account=rwo.Account
					LEFT JOIN tbl_Product_Table			prod on prod.id=rwo.ID_Product
					LEFT JOIN tbl_Operator_Table		op1 on op1.id=rwo.ID_CreateBy
					LEFT JOIN tbl_Operator_Table		op2 on op2.id=rwo.ID_Changed
				WHERE rwo.Account=@[Account]";
                sql = sql.Replace("@[Account]", AccountToSearch);
            }

            return sql;
        }
    }

    public class RwosViewModel
    {
        public string AccountToSearch { get; set; }
        public int Account { get; set; }
        public string ProductCode { get; set; }
        public string CreatedBy { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreateDate { get; set; }
        public string ChangedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? LastChange { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? NextRepeatDate { get; set; }
    }


}