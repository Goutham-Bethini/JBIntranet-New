using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.Entity.SqlServer;
using USPS_Report.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.Entity;

namespace USPS_Report.Areas.Reports.Models
{
    public class PayerLinkReport
    {
        public IList<PayerLinkViewModel> GetDetails(string RecordsForPayer, string WhichRecords)
        {
            IList<PayerLinkViewModel> _list = new List<PayerLinkViewModel>();
            string sql = GetSqlQuery(RecordsForPayer, WhichRecords, "PayerLink");
            using (IntranetEntities _db = new IntranetEntities())
            {
                _list = _db.Database.SqlQuery<PayerLinkViewModel>(sql).ToList<PayerLinkViewModel>();
            }
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                foreach (var item in _list)
                {
                    if (item.eopEntityName.Trim().IndexOf("'") > -1)
                    {
                        int index = item.eopEntityName.Trim().IndexOf("'");
                        item.eopEntityName = item.eopEntityName.Insert(index + 1, "'");
                    }
                    string subsql = GetSqlQuery(RecordsForPayer, WhichRecords, "tbl_Payer_Table");
                    if (!string.IsNullOrEmpty(item.eopEntityName))
                    {
                        subsql = subsql.Replace("@[eopEntityName]", item.eopEntityName.Trim());
                    }
                    else
                    {
                        subsql = subsql.Replace("@[eopEntityName]", "");
                    }
                    if (!string.IsNullOrEmpty(item.eopPlanCode))
                    {
                        subsql = subsql.Replace("@[eopPlanCode]", item.eopPlanCode.Trim());
                    }
                    else
                    {
                        subsql = subsql.Replace("@[eopPlanCode]", "");
                    }

                    item.FoundResult = _db.Database.SqlQuery<tbl_Payer_Table>(subsql).ToList<tbl_Payer_Table>().Count().ToString() + " Found";
                }
            }

            return _list;
        }

        public void UpdateDatabase(PayerLinkViewModel vm)
        {
            if (vm.eopIgnoreTorF == false)
            {
                vm.eopIgnore = 0;
            }
            if (vm.eopIgnoreTorF == true)
            {
                vm.eopIgnore = 1;
            }
            using (IntranetEntities _callDB = new IntranetEntities())
            {
                //string sql = "select * from [Intranet].[dbo].[EmdeonOtherPayers] where [eopID] = '" + vm.eopID + "'";
                string sql = "update [Intranet].[dbo].[EmdeonOtherPayers] Set [eopHDMSids] = '" + vm.eopHDMSids.Trim() + "', [eopIgnore] = '" + vm.eopIgnore + "'  where [eopID] = '" + vm.eopID + "'";
                if (!string.IsNullOrEmpty(vm.eopEntityName))
                {
                    sql += " and [eopEntityName] = '" + vm.eopEntityName.Trim() + "'";
                }
                else
                {
                    sql += " and [eopEntityName] IS NULL";
                }
                if (!string.IsNullOrEmpty(vm.eopPlanCoverage))
                {
                    sql += " and [eopPlanCoverage] = '" + vm.eopPlanCoverage.Trim() + "'";
                }
                else
                {
                    sql += " and [eopPlanCoverage] IS NULL";
                }
                if (!string.IsNullOrEmpty(vm.eopPlanCode))
                {
                    sql += " and [eopPlanCode] = '" + vm.eopPlanCode.Trim() + "'";
                }
                else
                {
                    sql += " and [eopPlanCode] IS NULL";
                }
                if (vm.eopAddedWithPayer.GetValueOrDefault() == 0)
                {
                    sql += " and [eopType] = '" + vm.eopAddedWithPayer + "'";
                }
                else
                {
                    sql += " and [eopType] IS NULL";
                }
                try
                {
                    _callDB.Database.CommandTimeout = 900;
                    _callDB.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, sql);
                    _callDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }


        private string GetSqlQuery(string RecordsForPayer, string WhichRecords, string type)
        {
            string sql = "";
            if (type == "PayerLink")
            {
                if (!string.IsNullOrEmpty(WhichRecords) && !string.IsNullOrEmpty(RecordsForPayer))
                {
                    sql += @"SELECT *, Case when eopIgnore = 0 then CAST('FALSE' as bit) else CAST('TRUE' as bit) End as eopIgnoreTorF
                                FROM EmdeonOtherPayers
                                WHERE 1 = 1";

                    if (WhichRecords.Equals("UnLinked"))
                    {
                        sql += @" AND	(eopIgnore = 0 OR eopIgnore IS NULL)
                                    AND	(eopHDMSids IS NULL OR ltrim(rtrim(eopHDMSids)) = '')";
                    }
                    if (WhichRecords.Equals("Linked"))
                    {
                        sql += @" AND (eopIgnore=1
                                    OR	(eopHDMSids IS NOT NULL AND eopHDMSids <> ''))";
                    }
                    if (RecordsForPayer.Equals("7"))
                    {
                        sql += @" AND eopAddedWithPayer=7";
                    }
                    if (RecordsForPayer.Equals("3179"))
                    {
                        sql += @" AND eopAddedWithPayer=3179";
                    }
                    sql += @" ORDER BY 
				                        eopPlanCoverage";

                }
                else
                {
                    sql += @"SELECT *,  Case when eopIgnore = 0 then CAST('FALSE' as bit) else CAST('TRUE' as bit) End as eopIgnoreTorF
		                        FROM EmdeonOtherPayers
		                        WHERE 
				                        1=1
		                        ORDER BY 
				                        eopPlanCoverage";
                }
            }
            else if (type == "tbl_Payer_Table")
            {
                sql += @"SELECT * 
					        FROM tbl_payer_table
					        WHERE
							        Discontinued=1
						        AND	(Name='@[eopEntityName]'
						        OR (ltrim(rtrim(ExternalReference)) <> '' AND ExternalReference='@[eopPlanCode]'))";

            }


            return sql;
        }
    }

    public class PayerLinkViewModel
    {
        public int eopID { get; set; }
        public string eopType { get; set; }
        public string eopEntityName { get; set; }
        public string eopPlanCoverage { get; set; }
        public byte eopIgnore { get; set; }
        public bool eopIgnoreTorF { get; set; }
        //{
        //    get
        //    {
        //        if (eopIgnore == 1)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    set { }
        //}
        public string eopPlanCode { get; set; }
        public double? eopAddedWithPayer { get; set; }
        public string eopHDMSids { get; set; }
        public List<tbl_Payer_Table> PlayerData { get; set; }
        public string FoundResult { get; set; }
    }

    public class PayerLinkSearchModel
    {
        public string RecordsForPayer { get; set; }
        //   public SelectList RecordsForPayerList { get; set; }
        public string WhichRecords { get; set; }
        //   public SelectList WhichRecordsList { get; set; }
    }

}