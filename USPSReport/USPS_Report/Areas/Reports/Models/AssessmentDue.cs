using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Globalization;
using System.Text;
using System.Net.Http;
using System.IO;

namespace USPS_Report.Areas.Reports.Models
{

    public class PrintableReports
    {
        public static IList<Assesment_counts> GetAssessmentDueData()
        {
            try
            {             
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                   IList<Assesment_counts> _list = new List<Assesment_counts>();
                    for (int count = 0;count <2; count ++)
                     {
                        Assesment_counts _assesment= new Assesment_counts();
                        IList<AssessmentDueData> _monthlyList = new List<AssessmentDueData>();
                        _db.Database.CommandTimeout = 180;
                        _monthlyList = _db.Database.SqlQuery<AssessmentDueData>(@"SELECT DISTINCT 
DateName(month, DateAdd(month, datepart(mm, dateadd(mm, c.duration, c.AssessmentDate)), 0) - 1) AS MonthName,
datepart(mm, dateadd(mm, c.duration, c.AssessmentDate)) AS Month,
COUNT(DISTINCT c.Account) AS MonthTotal,
Year(DATEADD(year, 0, getdate())) + " + count+@"  AS Year,
(case when(count(case when pc.CategoryDescription = 'Pull Ons' then 1 else null end)) > 0 then 'Yes' else 'No' end) as 'Is Pull Ons?'
FROM HHSQLDB.dbo.tbl_clinical_assessments c
JOIN HHSQLDB.dbo.tbl_PS_RepeatingOrders wro ON wro.account = c.account
JOIN HHSQLDB.dbo.tbl_product_table pro on pro.id = wro.ID_product
join HHSQLDB.dbo.tbl_ProductCategory_Table pc on pro.ID_ProductCategory = pc.ID
JOIN HHSQLDB.dbo.tbl_Account_Member am  ON c.account = am.account
AND am.member = 1
JOIN HHSQLDB.dbo.tbl_Account_Insurance ins ON ins.Account = am.Account
JOIN HHSQLDB.dbo.tbl_Account_Information ai  ON c.account = ai.account
JOIN HHSQLDB.dbo.tbl_payer_table pay ON pay.id = ins.ID_payer
and pay.id in (7, 3179, 3739)
JOIN HHSQLDB.dbo.tbl_Account_Member_Insurance mis on mis.Account = am.Account
AND mis.Member = 1
AND mis.Entry_Number = ins.Entry_Number
WHERE datepart(yy, dateadd(mm, c.duration, c.assessmentDate)) = (Year(DATEADD(year, 0, getdate())) + "+count+@")
AND(effective_date is Null or effective_date < GetDate())
AND(expiration_date is Null or expiration_date > GetDate())
AND c.DeleteDate IS NULL
AND  ai.inactiveaccount = 0
AND am.deceased is null
AND c.ID = (select top 1 ID
from HHSQLDB.dbo.tbl_Clinical_Assessments
where Account = c.Account
and Member = c.Member
order by AssessmentDate desc)
GROUP BY datepart(mm, dateadd(mm, c.duration, c.AssessmentDate))
ORDER BY datepart(mm, dateadd(mm, c.duration, c.AssessmentDate))").ToList<AssessmentDueData>();
                        _assesment.assessmentDueData = _monthlyList;
                        _list.Add(_assesment);                       
                    }                  
                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<Assesment_counts>();
            }
        }

        public static IList<AssessmentDueByMonthVM> GetAssessmentDueByMonth(int yr, DateTime firstdayOftheMonth, DateTime lastdayoftheMonth)
        {            
            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    IList<AssessmentDueByMonthVM> _list = new List<AssessmentDueByMonthVM>();
                    string Query = @"declare @year int
declare @firstdayOftheMonth date
declare @lastdayoftheMonth date

set @year="+yr.ToString()
+ @" set @firstdayOftheMonth='" + firstdayOftheMonth.ToShortDateString()
+ @"' set @lastdayoftheMonth='" + lastdayoftheMonth.ToShortDateString()

+ @"' IF OBJECT_ID('tempdb..#test') IS NOT NULL DROP TABLE  #test
IF OBJECT_ID('tempdb..#test2') IS NOT NULL DROP TABLE  #test2
SELECT   
distinct 
am.Account,  
CONVERT(varchar(12),  
DATEADD([month], ISNULL(ca.Duration, 24), ca.AssessmentDate))AS Expires,  
ca.ID AS Clin_Ass_ID,  
pay.id,  
pay.state,  
cast(DateDiff(yy,am.BirthDate,GetDate()) as int) AS Age, 
cast(ca.duration as int) AS  Duration,
pc.CategoryDescription 
into #test
FROM  HHSQLDB.dbo.tbl_Clinical_Assessments ca 
JOIN HHSQLDB.dbo.tbl_PS_RepeatingOrders         wro ON wro.account = ca.account  
JOIN HHSQLDB.dbo.tbl_product_table              pro on pro.id = wro.ID_product  
join HHSQLDB.dbo.tbl_ProductCategory_Table pc on pro.ID_ProductCategory=pc.ID 
JOIN HHSQLDB.dbo.tbl_Account_Member             am ON ca.account = am.account  
AND am.member = 1  
JOIN HHSQLDB.dbo.tbl_Account_Insurance          ins ON ins.Account = am.Account  
JOIN HHSQLDB.dbo.tbl_Account_Information        ai ON ca.account = ai.account  
JOIN HHSQLDB.dbo.tbl_payer_table                pay ON pay.id = ins.ID_payer  
and pay.id in (7,3179,3739) 
JOIN HHSQLDB.dbo.tbl_Account_Member_Insurance   mis on mis.Account = am.Account  
AND mis.Member = 1  
AND mis.Entry_Number = ins.Entry_Number 
WHERE  
datepart(yy, dateadd(mm, ca.duration, ca.assessmentDate)) =  @year 
AND(effective_date is Null or effective_date < GetDate())  
AND(expiration_date is Null or expiration_date > GetDate())  
AND ca.DeleteDate IS NULL  
AND  ai.inactiveaccount = 0  
AND am.deceased is null  
AND ca.ID = (select top 1 ID  
from HHSQLDB.dbo.tbl_Clinical_Assessments  
where Account = ca.Account  
and Member = ca.Member 
order by AssessmentDate desc) 
AND DATEADD([month], ISNULL(ca.Duration, 6), ca.AssessmentDate) between @firstdayOftheMonth and @lastdayoftheMonth
order by  
CONVERT(varchar(12),  
DATEADD([month], ISNULL(ca.Duration, 24), ca.AssessmentDate)), 
am.Account, 
ca.ID ,pay.id,pay.state,age, duration 

select  T1.Account,T1.Expires,T1.Clin_Ass_ID,T1.ID,T1.State,T1.Age,T1.Duration
,RTRIM(LTRIM( STUFF(  
        (  
        SELECT ',' + T2.CategoryDescription  
        FROM #test T2  
        WHERE T1.Account = T2.Account  
        FOR XML PATH ('')  
        ),1,1,'')  )) 'ProductCategories'
,(case when max(case when RTRIM(LTRIM( T1.CategoryDescription))='Pull Ons' then 1 else 0 end)>0 then 'Yes' else 'No' end) 'HasPullOns'
into #test2 
from #test T1
group by T1.Account,T1.Expires,T1.Clin_Ass_ID,T1.ID,T1.State,T1.Age,T1.Duration

select res.Account,res.Expires,res.Clin_Ass_ID,res.ID,res.State,res.Age,res.Duration
,res.ProductCategories,res.HasPullOns,cast((case when aa.Attempt1st=1 then 1 else 0 end) as bit) Attempt1st,
aa.CreatedBy Attempted1stBy,
ope.OperatorName Attempted1stName,
cast(aa.CreatedDate as date) Attempted1stDate,
cast((case when aa.Attempt2nd=1 then 1 else 0 end) as bit) Attempt2nd,
(case when aa.Attempt2nd=1 then isnull(aa.UpdatedBy, aa.CreatedBy) else null end) Attempted2ndBy,
(case when aa.Attempt2nd=1 then ISNULL(ope2.OperatorName,ope.OperatorName) else null end) Attempted2ndName,
(case when aa.Attempt2nd=1 then isnull(aa.UpdatedDate, aa.CreatedDate) else null end) Attempted2ndDate,
cast((case when aa.Attempt3rd=1 then 1 else 0 end) as bit) Attempt3rd,
(case when aa.Attempt3rd=1 then isnull(aa.UpdatedBy, aa.CreatedBy) else null end) Attempted3rdBy,
(case when aa.Attempt3rd=1 then isnull(aa.UpdatedDate, aa.CreatedDate) else null end) Attempted3rdDate 
from #test2 res 
left join HHSQLDB.dbo.tbl_AssessmentDue_Attempts aa on res.Account=aa.Account
left join HHSQLDB.dbo.tbl_Operator_Table ope on aa.CreatedBy=ope.ID 
left join HHSQLDB.dbo.tbl_Operator_Table ope2 on aa.UpdatedBy=ope2.ID";
//                    string Query = @"SELECT   
//am.Account,  
//CONVERT(varchar(12),
//DATEADD([month], ISNULL(ca.Duration, 24), ca.AssessmentDate))AS Expires,
//ca.ID AS Clin_Ass_ID,  
//pay.id,  
//pay.state,  
//cast(DateDiff(yy, am.BirthDate, GetDate()) as int) AS Age,
//  cast(ca.duration as int) AS Duration,
//  pc.CategoryDescription
//  FROM  HHSQLDB.dbo.tbl_Clinical_Assessments ca
//JOIN HHSQLDB.dbo.tbl_PS_RepeatingOrders wro ON wro.account = ca.account
//JOIN HHSQLDB.dbo.tbl_product_table pro on pro.id = wro.ID_product
//join HHSQLDB.dbo.tbl_ProductCategory_Table pc on pro.ID_ProductCategory = pc.ID
//JOIN HHSQLDB.dbo.tbl_Account_Member am ON ca.account = am.account
//AND am.member = 1
//JOIN HHSQLDB.dbo.tbl_Account_Insurance ins ON ins.Account = am.Account
//JOIN HHSQLDB.dbo.tbl_Account_Information ai ON ca.account = ai.account
//JOIN HHSQLDB.dbo.tbl_payer_table pay ON pay.id = ins.ID_payer
//and pay.id in (7, 3179, 3739)

//JOIN HHSQLDB.dbo.tbl_Account_Member_Insurance mis on mis.Account = am.Account
//AND mis.Member = 1
//AND mis.Entry_Number = ins.Entry_Number

//WHERE
//datepart(yy, dateadd(mm, ca.duration, ca.assessmentDate)) = "+yr+@" 
//AND(effective_date is Null or effective_date < GetDate())
//AND(expiration_date is Null or expiration_date > GetDate())
//AND ca.DeleteDate IS NULL
//AND  ai.inactiveaccount = 0
//AND am.deceased is null
//AND ca.ID = (select top 1 ID
//from HHSQLDB.dbo.tbl_Clinical_Assessments
//where Account = ca.Account
//and Member = ca.Member
//order by AssessmentDate desc) 
//AND DATEADD([month], ISNULL(ca.Duration, 6), ca.AssessmentDate) between '"+firstdayOftheMonth+"' and '"+lastdayoftheMonth+@"' 
//order by
//CONVERT(varchar(12),
//DATEADD([month], ISNULL(ca.Duration, 24), ca.AssessmentDate)), am.Account, 
//ca.ID ,pay.id,pay.state,age, duration";

                    _list = _db.Database.SqlQuery<AssessmentDueByMonthVM>(Query).ToList<AssessmentDueByMonthVM>();
                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<AssessmentDueByMonthVM>();
            }


        }

        public static IList<RemovedAccount> GetRemovedAcs()
        {
            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    IList<RemovedAccount> _list = new List<RemovedAccount>();
                    string Query = @"select Account,ISNULL(UpdatedDate,CreatedDate) as RWOsSetHoldDate,State,
                                    (case when HasPullOns=1 then 'Yes' else 'No' end) HasPullOns 
                                    from HHSQLDB.dbo.tbl_AssessmentDue_Attempts where Attempt3rd = 1 or isSetToHold=1";
                    _list = _db.Database.SqlQuery<RemovedAccount>(Query).ToList<RemovedAccount>();
                    return _list;

                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return new List<RemovedAccount>();
            }
        }

        public static void InsertAssessmentDueNote(AssessmentDueByMonth _vm, int account, string operatorName, bool attempt1st, bool attempt2nd, bool attempt3rd)
        {
            ID_VM id_op = new ID_VM();
            tbl_Account_Note _note = new tbl_Account_Note();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                id_op = (from ope in _db.tbl_Operator_Table
                         where ope.OperatorName.ToUpper() == operatorName.ToUpper() && ope.DeletedDate == null && ope.InactiveDate == null
                         select new ID_VM
                         {
                             ID = ope.ID
                         }).Take(1).SingleOrDefault();
                Int32? id = Convert.ToInt32(id_op.ID);
                _note = _db.tbl_Account_Note.Where(t => t.Account == account && t.NoteHeading == "COMMUNICATION").FirstOrDefault(); 
                if (_note == null)
                {
                    tbl_Account_Note _tbl = new tbl_Account_Note();
                    _tbl.Account = Convert.ToInt32(account);
                    _tbl.Member = 1;
                    _tbl.NoteHeading = "COMMUNICATION";
                    _tbl.NoteCreateDate = DateTime.Now;
                    _tbl.NoteCreatedBy = id;
                    _tbl.SystemRecordType = 100;
                    _tbl.ID_NoteLibrary = 9;
                    _db.tbl_Account_Note.Add(_tbl);
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                    }
                    _note = _db.tbl_Account_Note.Where(t => t.Account == account && t.NoteHeading == "COMMUNICATION").FirstOrDefault(); 
                }
                for(int i=1;i<=3;i++)
                {
                    if (_note != null)
                    {
                        StringBuilder _msgStr1 = new StringBuilder();
                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);
                        tbl_Account_Note_History _noteHistory1 = _db.tbl_Account_Note_History.Where(t => t.ID_Note == _note.ID && t.NoteText== "Attempted to reach member/caregiver to update the assessment. Attempt #1.").FirstOrDefault();
                        tbl_Account_Note_History _noteHistory2 = _db.tbl_Account_Note_History.Where(t => t.ID_Note == _note.ID && t.NoteText == "Attempted to reach member/caregiver to update the assessment. Attempt #2.").FirstOrDefault();
                        if (i==1&&attempt1st&&_noteHistory1==null)
                        {
                            _msgStr1.Append("Attempted to reach member/caregiver to update the assessment. Attempt #1.");
                            _tHist.NoteText = _msgStr1.ToString();
                            _db.tbl_Account_Note_History.Add(_tHist);
                        }
                        else if (i == 2 && attempt2nd && _noteHistory2 == null)
                        {
                            _msgStr1.Append("Attempted to reach member/caregiver to update the assessment. Attempt #2.");
                            _tHist.NoteText = _msgStr1.ToString();
                            _db.tbl_Account_Note_History.Add(_tHist);
                        }
                        else if (i == 3 && (attempt3rd || _vm.isSetToHold))
                        {
                            _msgStr1.Append("Unable to reach the member/caregiver due to invalid contact information.  Sending contact letter to obtain updated contact information.  Placing RWO on 2/1/2099 hold until assessment can be completed.");
                            _tHist.NoteText = _msgStr1.ToString();
                            _db.tbl_Account_Note_History.Add(_tHist);
                            tbl_Clinical_Assessments ass= _db.tbl_Clinical_Assessments.Where(t => t.ID == _vm.Clin_Ass_ID).FirstOrDefault();
                            if (ass != null)
                            {
                                ass.ID_DeletedBy = id;
                                ass.DeleteDate= DateTime.Now;
                            }
                            string locQuery = @"select rwo.ID from HHSQLDB.dbo.tbl_PS_RepeatingOrders rwo 
                                                join HHSQLDB.dbo.tbl_Product_Table pro on rwo.ID_Product = pro.ID
                                                join HHSQLDB.dbo.tbl_ProductCategory_Table cat on pro.ID_ProductCategory = cat.ID
                                                where pro.ID_ProductCategory in (1, 2, 3, 6, 7, 9, 10, 13, 14, 24, 43, 84)
                                                and Account = " + account.ToString();
                            List<int> lstIds = _db.Database.SqlQuery<int>(locQuery).ToList<int>();
                            
                            List<tbl_PS_RepeatingOrders> rwos= _db.tbl_PS_RepeatingOrders.Where(t => t.Account == account).ToList();
                            //_db.tbl_PS_RepeatingOrders.Join(_db.tbl_PS_RepeatingOrders, rwo => rwo.ID_Product, pro => pro.ID, (rwo, pro) => new { rwo, pro })
                            //    .Join(_db.tbl_ProductCategory_Table, cat => cat.pro.ID_Product, pc => pc.ID, (cat, pc) => new { cat, pc })
                            //    .Where(res => lst.Contains(res.pc.ID)).ToList();
                            
                            foreach (tbl_PS_RepeatingOrders item in rwos)
                            {
                                if(lstIds.Contains(item.ID))
                                {
                                    item.NextRepeatDate = Convert.ToDateTime("2/1/2099");
                                }                                
                            }
                            //SendContactLetter(account);
                        }
                    }
                }
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
                tbl_AssessmentDue_Attempts _attempt = _db.tbl_AssessmentDue_Attempts.Where(t => t.Account == account).FirstOrDefault();
                if(_attempt==null)
                {
                    tbl_AssessmentDue_Attempts _attempts = new tbl_AssessmentDue_Attempts();
                    _attempts.Account = Convert.ToInt32(account);
                    _attempts.Expires = Convert.ToDateTime(_vm.Expires);
                    _attempts.State = _vm.state;
                    _attempts.Age = _vm.Age;
                    _attempts.Duration = _vm.Duration;
                    _attempts.HasPullOns = _vm.HasPullOns == "Yes" ? true : false;
                    _attempts.Attempt1st = attempt1st;
                    _attempts.Attempt2nd = attempt2nd;
                    _attempts.Attempt3rd = attempt3rd;
                    _attempts.CreatedBy = id;
                    _attempts.CreatedDate = DateTime.Now;
                    _attempts.isSetToHold = _vm.isSetToHold;
                    _db.tbl_AssessmentDue_Attempts.Add(_attempts);
                }
                else
                {
                    _attempt.Attempt1st = attempt1st;
                    _attempt.Attempt2nd = attempt2nd;
                    _attempt.Attempt3rd = attempt3rd;
                    _attempt.UpdatedBy = id;
                    _attempt.UpdatedDate = DateTime.Now;
                    _attempt.isSetToHold = _vm.isSetToHold;
                }
                
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
        }

        //Sends the contact letter to the member, through email if exists, if not through mail.
        public static bool SendContactLetter(int Account)
        {
            Account = 7802;
            bool IsContactLetterSent = false;
            // string BaseAddress = "http://localhost:61129";  //https://jbreports2.jandbmedical.com
            string BaseAddress = "https://jbreports2.jandbmedical.com";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);
            var result2 = client.GetAsync("/LetterGenService/api/values/" + Account.ToString() + "/NursingOrDeact/Email").Result;
            //var result2 = client.GetAsync("/LetterGenService/api/values/" + Account.ToString() + "/cont_letter_RWOUpdate/dynamiccheck").Result;
            //var result2 = client.GetAsync("/api/values/" + Account + "/cont_letter_RWOUpdate/dynamiccheck").Result;//local
            string _value;
            using (var stm1 = result2.Content.ReadAsStreamAsync())
            {
                using (StreamReader reader = new StreamReader(stm1.Result))
                {
                    _value = reader.ReadToEnd();
                    if (_value == "true")
                    {
                        IsContactLetterSent = true;
                        
                    }
                    else
                    {
                        //send that the program failed to send contact letter.
                        throw new Exception();
                    }
                }
            }
            return IsContactLetterSent;
        }
    }
    public class AssessmentDueData
    {
        public int? Month { get; set; }
        public int? MonthTotal { get; set; }
        public int? Year { get; set; }

        public string MonthName { get; set; }
    }


    public class Assesment_counts
    {
        public IList<AssessmentDueData> assessmentDueData { get; set; }
    }

    public class AssessmentDueVM
    {
        public IList<Assesment_counts> assesment_counts { get; set; }
    }


    public class AssessmentDueByMonth
    {
        public DateTime firstDate { get; set; }
        public DateTime lastDate { get; set; }
        public IList<AssessmentDueByMonthVM> assessmentDueByMonth { get; set; }
        public int AssessmentsCount { get; set; }
        public int? Account { get; set; }
        public string Expires { get; set; }       
        public string state { get; set; }
        public int? Age { get; set; }
        public int? Duration { get; set; }
        public string ProductCategories { get; set; }
        public string HasPullOns { get; set; }
        public bool Attempt1st { get; set; }
        public int? Attempted1stBy { get; set; }
        public string Attempted1stName { get; set; }
        public DateTime? Attempted1stDate { get; set; }
        public bool Attempt2nd { get; set; }
        public int? Attempted2ndBy { get; set; }
        public string Attempted2ndName { get; set; }
        public DateTime? Attempted2ndDate { get; set; }
        public bool Attempt3rd { get; set; }
        public int Clin_Ass_ID { get; set; }
        public bool isSetToHold { get; set; }
    }
    public class AssessmentDueByMonthVM
    {
        public int Account { get; set; }
        public string Expires { get; set; }
        public int Clin_Ass_ID { get; set; }
        public int id { get; set; }
        public string state { get; set; }
        public int Age { get; set; }
        public int Duration { get; set; }
        public string ProductCategories { get; set; }
        public string HasPullOns { get; set; }        
        public bool Attempt1st { get; set; }
        public int? Attempted1stBy { get; set; }
        public string Attempted1stName { get; set; }
        public DateTime? Attempted1stDate { get; set; }
        public bool Attempt2nd { get; set; }
        public int? Attempted2ndBy { get; set; }
        public string Attempted2ndName { get; set; }
        public DateTime? Attempted2ndDate { get; set; }
        public bool Attempt3rd { get; set; }
        public bool isSetToHold { get; set; }
    }

    public class RemovedAccountsVM
    {
        public List<RemovedAccount> Details { get; set; }
    }

    public class RemovedAccount
    {
        public int Account { get; set; }
        public DateTime? RWOsSetHoldDate { get; set; }
        public string State { get; set; }
        public string HasPullOns { get; set; }
    }
}