using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace USPS_Report.Areas.Reports.Models
{
    public class HDMSPunctuationsService
    {
        public List<HDMSPunctuationsVM> GetDetails_Puncuation()
        {
            List<HDMSPunctuationsVM> records = new List<HDMSPunctuationsVM>();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                records = _db.Database.SqlQuery<HDMSPunctuationsVM>(@" Select AI.Account,AI.Reference as InsuranceID,AM.First_Name as FirstName,AM.Last_Name as LastName,AM.Sex as Gender,AM.BirthDate,AI.Billable_Party_Member, prov.OrganizationName
  ,CASE when AI.Reference LIKE '%[^a-z0-9-'']%' then 'Yes' else 'No' End as Is_IssueWith_InsuranceID
  ,CASE when (AM.First_Name LIKE ' %' or AM.First_Name LIKE '% ') then 'Yes' else 'No' End as Is_IssueWith_FirstName
  ,CASE when (AM.Last_Name LIKE ' %' or AM.Last_Name LIKE '% ') then 'Yes' else 'No' End as Is_IssueWith_LastName
  ,CASE when (AM.Sex is null or AM.Sex ='') then 'Yes' else 'No' End as Is_IssueWith_Gender
  ,CASE when (AM.BirthDate > DATEADD(yy,-18,GETDATE()) AND (AI.Billable_Party_Member is null  or AI.Billable_Party_Member = '')) then 'Yes' else 'No' End as AgeUnder18_ButNoBillableParty
  from tbl_Account_Information AI
  join tbl_Account_Member AM on AM.Account = AI.Account and AM.Member = 1
  join tbl_Provider_Table prov on prov.ID = AM.ID_Default_Provider
   where AI.InActiveAccount = 0
  and  (AI.Reference LIKE '%[^a-z0-9-'']%' or AM.First_Name LIKE ' %' or AM.First_Name LIKE '% ' or AM.Last_Name LIKE ' %' or AM.Last_Name LIKE '% ' or AM.Sex is null 
  or AM.Sex ='' or (AM.BirthDate > DATEADD(yy,-18,GETDATE()) AND (AI.Billable_Party_Member is null  or AI.Billable_Party_Member = ''))) and AM.ID_Default_Provider not in 
  ( Select ID from tbl_Provider_Table where (OrganizationName = 'J and B Medical - Drop Ship' or OrganizationName = 'J and B NETWORK SOLUTIONS' or 
  OrganizationName = 'J B DIS' or OrganizationName = 'J B DIS 2' or OrganizationName = 'WRIGHT AND FILIPPIS'))").ToList<HDMSPunctuationsVM>();
            }

            return records;
        }
    }

    public class HDMSPunctuationsVM
    {
        public int? Account { get; set; }
        public string InsuranceID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? BirthDate { get; set; }
        public string OrganizationName { get; set; }
        public Int16? Billable_Party_Member { get; set; }
        public string Is_IssueWith_InsuranceID { get; set; }
        public string Is_IssueWith_FirstName { get; set; }
        public string Is_IssueWith_LastName { get; set; }
        public string Is_IssueWith_Gender { get; set; }
        public string AgeUnder18_ButNoBillableParty { get; set; }
    }
}