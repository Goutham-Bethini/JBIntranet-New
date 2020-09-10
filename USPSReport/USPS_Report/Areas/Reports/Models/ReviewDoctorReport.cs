using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.Entity.SqlServer;
using USPS_Report.Models;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace USPS_Report.Areas.Reports.Models
{
    public class ReviewDoctorReport
    {
        public IList<DoctorsInfoViewModel> GetDetails()
        {
            IList<DoctorsInfoViewModel> _dlist = new List<DoctorsInfoViewModel>();
            string sql = GetSqlDataQuery("DoctorsInfoViewModel");
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _dlist = _db.Database.SqlQuery<DoctorsInfoViewModel>(sql).GroupBy(x => new { x.ID, x.DocFirst, x.DocLast, x.DocReview_Date, x.DocReview_ID_User, x.DocDegree }).Select(v => new DoctorsInfoViewModel() { ID = v.Key.ID, DocFirst = v.Key.DocFirst, DocLast = v.Key.DocLast, DocReview_Date = v.Key.DocReview_Date, DocReview_ID_User = v.Key.DocReview_ID_User, DocDegree = v.Key.DocDegree }).ToList();
            }
            return _dlist;
        }

        public IList<ReviewDoctorsViewModel> GetHierarchyDetails(int dID)
        {
            IList<ReviewDoctorsViewModel> _list = new List<ReviewDoctorsViewModel>();
            string sql = GetSqlDataQuery("ReviewDoctorsViewModel");
            sql = sql.Replace("@[DID]", dID.ToString());
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _list = _db.Database.SqlQuery<ReviewDoctorsViewModel>(sql).ToList<ReviewDoctorsViewModel>();
            }
            //_list = _list.Where(x => x.ID == dID).ToList();
            return _list;
        }


        private string GetSqlDataQuery(string type)
        {
            string sql = "";
            if (type == "DoctorsInfoViewModel")
            {
                sql = @"SELECT DISTINCT 
			rs.ID AS ID_Physician, 
			ai.Account, 
			am.Last_Name, 
			am.First_Name, 
			rs.discontinued, 
			rs.First_Name AS DocFirst, 
			rs.Last_Name AS DocLast, 
			rs.Degree AS DocDegree, 
			DocReview_Date, 
			DocReview_ID_User, 
			rs.ID
		FROM tbl_Referral_Source_Table		rs 
			JOIN tbl_MedDocuments			md ON rs.ID=md.ID_Physician
			JOIN tbl_MedDoc_Type_Table		mt ON mt.ID=md.ID_TypeOfDoc
			LEFT JOIN DocReview				dr ON dr.DocReview_ID_Physician=md.ID_Physician 
				AND dr.DocReview_ID IN 	(SELECT MAX(DocReview_ID)
										FROM DocReview
										GROUP BY DocReview_ID_Physician)
			JOIN tbl_Account_Member			am ON am.Account=md.account
			JOIN tbl_Account_Information	ai ON ai.Account=am.Account
		WHERE am.member=1
			AND rs.DeletedDate IS NULL
			AND mt.DocTypeDescription NOT LIKE 'LMN%'
			AND md.ID=(	SELECT max(md2.ID)
						FROM tbl_MedDocuments md2
							JOIN tbl_MedDoc_Type_Table mt2 ON mt2.ID=md2.ID_TypeOfDoc
						WHERE Account=am.Account
							AND mt2.DocTypeDescription NOT LIKE 'LMN%')
			AND (ai.InActiveAccount IS NULL OR ai.InActiveAccount=0)
		ORDER BY rs.ID, ai.Account, am.Last_Name, am.First_Name";
            }
            else if (type == "ReviewDoctorsViewModel")
            {
                sql = @"SELECT DISTINCT 
			rs.ID AS ID_Physician, 
			ai.Account, 
			am.Last_Name, 
			am.First_Name, 
			rs.discontinued, 
			rs.First_Name AS DocFirst, 
			rs.Last_Name AS DocLast, 
			rs.Degree AS DocDegree, 
			DocReview_Date, 
			DocReview_ID_User, 
			rs.ID
		FROM tbl_Referral_Source_Table		rs 
			JOIN tbl_MedDocuments			md ON rs.ID=md.ID_Physician
			JOIN tbl_MedDoc_Type_Table		mt ON mt.ID=md.ID_TypeOfDoc
			LEFT JOIN DocReview				dr ON dr.DocReview_ID_Physician=md.ID_Physician 
				AND dr.DocReview_ID IN 	(SELECT MAX(DocReview_ID)
										FROM DocReview
										GROUP BY DocReview_ID_Physician)
			JOIN tbl_Account_Member			am ON am.Account=md.account
			JOIN tbl_Account_Information	ai ON ai.Account=am.Account
		WHERE am.member=1
			AND rs.DeletedDate IS NULL
			AND mt.DocTypeDescription NOT LIKE 'LMN%'
			AND md.ID=(	SELECT max(md2.ID)
						FROM tbl_MedDocuments md2
							JOIN tbl_MedDoc_Type_Table mt2 ON mt2.ID=md2.ID_TypeOfDoc
						WHERE Account=am.Account
							AND mt2.DocTypeDescription NOT LIKE 'LMN%')
			AND (ai.InActiveAccount IS NULL OR ai.InActiveAccount=0)
            AND ID_Physician = @[DID]
		ORDER BY rs.ID, ai.Account, am.Last_Name, am.First_Name";
            }

            return sql;
        }
    }

    public class ReviewDoctorsViewModel
    {
        public int ID_Physician { get; set; }
        public int Account { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public Int16 discontinued { get; set; }
        public string DocFirst { get; set; }
        public string DocLast { get; set; }
        public string DocDegree { get; set; }
        public virtual string LastChecked
        {
            get
            {
                return DocReview_Date.ToString() + " - " + DocReview_ID_User;
            }
        }
        public virtual string DocInfo
        {
            get
            {
                return ID.ToString() + " - " + DocFirst + " " + DocLast + "* " + DocDegree;
            }
        }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DocReview_Date { get; set; }
        public string DocReview_ID_User { get; set; }
        public int ID { get; set; }
    }
    public class DoctorsInfoViewModel
    {
        public int ID { get; set; }
        public string DocFirst { get; set; }
        public string DocLast { get; set; }
        public string DocDegree { get; set; }
        public DateTime? DocReview_Date { get; set; }
        public string DocReview_ID_User { get; set; }
        public string LastChecked
        {
            get
            {
                return DocReview_Date.ToString() + " - " + DocReview_ID_User;
            }
        }
        public string DocInfo
        {
            get
            {
                return ID.ToString() + " - " + DocFirst + " " + DocLast + "* " + DocDegree;
            }
        }
    }

    public class ReviewPatientssViewModel
    {
        public int ID_Physician { get; set; }
        public int Account { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public Int16 discontinued { get; set; }
        public int ID { get; set; }
    }

}