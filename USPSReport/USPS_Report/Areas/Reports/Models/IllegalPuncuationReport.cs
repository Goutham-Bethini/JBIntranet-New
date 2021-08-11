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
    public class IllegalPuncuationReport
    {
            public static IList<IllegalPuncuationVM> GetDetails()
            {
                IList<IllegalPuncuationVM> _list = new List<IllegalPuncuationVM>();
                string sql = GetQuery();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    _list = _db.Database.SqlQuery<IllegalPuncuationVM>(sql).ToList<IllegalPuncuationVM>();
                }
                return _list;
            }

            private static string GetQuery()
            {
                string sql = @"SELECT 
			                            mem.Account, 
			                            Last_Name, 
			                            First_Name, 
			                            Address_1, 
			                            Address_2, 
			                            City, 
			                            State, 
			                            ShipToAddress_1, 
			                            ShipToAddress_2, 
			                            ShipToCity, 
			                            ShipToState, 
			                            op.legalname, 
			                            mem.last_updated_date
	                            FROM 
				                            tbl_Account_member			mem
		                            JOIN 	tbl_Account_information		inf 	ON inf.account=mem.account
		                            JOIN 	tbl_operator_table			op 		ON op.id=mem.last_updated_user
	                            WHERE 
			                            inf.InActiveAccount=0
		                            AND mem.member=1
		                            AND inf.id_pricing <> 3";

                return sql;
            }
    }

        public class IllegalPuncuationVM
        {
            public int Account { get; set; }
            public string Last_Name { get; set; }
            public string First_Name { get; set; }
            public string Address_1 { get; set; }
            public string Address_2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ShipToCity { get; set; }
            public string ShipToState { get; set; }
            public string ShipToAddress_1 { get; set; }
            public string ShipToAddress_2 { get; set; }
        }
}