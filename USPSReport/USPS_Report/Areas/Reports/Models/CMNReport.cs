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
    public class CMNReport
    {
        public IList<ExpiringCMNDetails> GetDetails(int _month, int _year)
        {
            IList<ExpiringCMNDetails> _list = new List<ExpiringCMNDetails>();
            string sql = GetExpringCMNDataQuery(_month, _year);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _list = _db.Database.SqlQuery<ExpiringCMNDetails>(sql).ToList<ExpiringCMNDetails>();
            }
            return _list;
        }

        private string GetExpringCMNDataQuery(int _month, int _year)
        {
            string sql = @"SELECT 
	                            doc.Account,
	                            CASE
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'A'
			                            OR  LEFT(mem.Last_Name, 1) = 'B')
		                            THEN 'A-B'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'C'
			                            OR  LEFT(mem.Last_Name, 1) = 'D')
		                            THEN 'C-D'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'E'
			                            OR  LEFT(mem.Last_Name, 1) = 'F')
		                            THEN 'E-F'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'G'
			                            OR  LEFT(mem.Last_Name, 1) = 'H')
		                            THEN 'G-H'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'I'
			                            OR  LEFT(mem.Last_Name, 1) = 'J')
		                            THEN 'I-J'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'K'
			                            OR  LEFT(mem.Last_Name, 1) = 'L')
		                            THEN 'K-L'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'M'
			                            OR  LEFT(mem.Last_Name, 1) = 'N')
		                            THEN 'M-N'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'O'
			                            OR  LEFT(mem.Last_Name, 1) = 'P')
		                            THEN 'O-P'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'Q'
			                            OR  LEFT(mem.Last_Name, 1) = 'R')
		                            THEN 'Q-R'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'S')
		                            THEN 'S'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'T'
			                            OR  LEFT(mem.Last_Name, 1) = 'U'
			                            OR  LEFT(mem.Last_Name, 1) = 'V')
		                            THEN 'T-V'
		                            WHEN (
				                            LEFT(mem.Last_Name, 1) = 'W'
			                            OR  LEFT(mem.Last_Name, 1) = 'X'
			                            OR  LEFT(mem.Last_Name, 1) = 'Y'
			                            OR  LEFT(mem.Last_Name, 1) = 'Z'			)
		                            THEN 'W-Z'
	                            ELSE '???'
	                            END AS 'AlphaSplit',
	                            mem.Last_Name+', '+mem.First_Name as 'PatientName',
	                            typ.DocTypeDescription as 'ProductLine',
	                            CONVERT(DATE, DateAdd(m,his.Duration,his.EffectiveDate)) AS 'ExpirationDate',
	                            '' as 'DueDate',
	                            '' as 'FirstAttempt',
	                            '' as 'SecondAttempt',
	                            '' AS 'ThirdAttempt',
	                            '' as 'DateCMNEntered',
	                            '' as 'AdvActionLetter',
	                            '' as 'UnableToSVCLetter',
	                            '' as 'Completed',
	                            '' as 'NotNeeded',
	                            '' as 'ComplianceCheck',
	                            '' as 'DateCMNEntered'

                            FROM
				                            tbl_MedDocuments			doc
	                            JOIN		tbl_MedDoc_Type_Table		typ	ON typ.ID=doc.ID_TypeOfDoc
	                            JOIN		tbl_MedDoc_History			his	ON his.ID_CMN=doc.ID												
	                            JOIN		tbl_account_member			mem	ON mem.account=doc.Account
											                            AND mem.member=doc.member
	                            JOIN		tbl_account_information		inf	ON inf.account=mem.Account
	
                            WHERE
		                            inf.InactiveAccount=0
		                            AND MONTH(DateAdd(m,his.Duration,his.EffectiveDate)) = @[month]
		                            AND YEAR(DateAdd(m,his.Duration,his.EffectiveDate)) = @[year]
		                            AND his.CMNReturnDate IS NOT NULL

                            ORDER BY
		
			                            mem.Last_Name

                            ";
            sql = sql.Replace("@[month]", _month.ToString());
            sql = sql.Replace("@[year]", _year.ToString());

            return sql;
        }
    }

    public class ExpiringCMNDetails
    {
        public int Account { get; set; }
        public string AlphaSplit { get; set; }
        public string PatientName { get; set; }
        public string ProductLine { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ExpirationDate { get; set; }
        public string DueDate { get; set; }
        public string FirstAttempt { get; set; }
        public string SecondAttempt { get; set; }
        public string ThirdAttempt { get; set; }
        public string AdvActionLetter { get; set; }
        public string UnableToSVCLetter { get; set; }
        public string Completed { get; set; }
        public string NotNeeded { get; set; }
        public string ComplianceCheck { get; set; }
        public string DateCMNEntered { get; set; }
    }
    public class CMNReportModel
    {
        public string _pickedDate { get; set; }
        public string pickedDate
        {
            get
            {
                if (string.IsNullOrEmpty(_pickedDate))
                {
                    _pickedDate = DateTime.Now.ToString("MMMM yyyy");
                    return _pickedDate;
                }
                else
                {
                    return _pickedDate;
                };
            }
            set
            {
                _pickedDate = value;
            }
        }
        public int pickedMonth
        {
            get
            {
                if (!string.IsNullOrEmpty(pickedDate))
                {
                    if (pickedDate.Trim().ToUpper().StartsWith("JANUARY"))
                    {
                        return 1;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("FEBRUARY"))
                    {
                        return 2;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("MARCH"))
                    {
                        return 3;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("APRIL"))
                    {
                        return 4;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("MAY"))
                    {
                        return 5;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("JUNE"))
                    {
                        return 6;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("JULY"))
                    {
                        return 7;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("AUGUST"))
                    {
                        return 8;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("SEPTEMBER"))
                    {
                        return 9;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("OCTOBER"))
                    {
                        return 10;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("NOVEMBER"))
                    {
                        return 11;
                    }
                    else if (pickedDate.Trim().ToUpper().StartsWith("DECEMBER"))
                    {
                        return 12;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        public int pickedYear
        {
            get
            {
                if (!string.IsNullOrEmpty(pickedDate))
                {
                    string[] date = pickedDate.Split(' ');
                    return Convert.ToInt32(date[1]);
                }
                else
                {
                    return 0;
                }
            }
        }
        public IList<ExpiringCMNDetails> ExpiringCMNDetails { get; set; }
    }

}