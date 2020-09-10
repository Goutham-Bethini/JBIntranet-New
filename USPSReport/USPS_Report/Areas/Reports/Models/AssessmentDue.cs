using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Globalization;

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
                    for (int count = -1;count <2; count ++)
                     {
                        Assesment_counts _assesment= new Assesment_counts();
                        IList<AssessmentDueData> _monthlyList = new List<AssessmentDueData>();
                        _monthlyList = _db.Database.SqlQuery<AssessmentDueData>("SELECT DISTINCT "+
                            " DateName( month , DateAdd( month , "+
            " datepart(mm, dateadd(mm, c.duration, c.AssessmentDate)), 0) - 1 ) AS MonthName,   "+
                           " datepart(mm, dateadd(mm, c.duration,c.AssessmentDate)) AS Month, " +
              "  COUNT(DISTINCT c.Account) AS MonthTotal, " +
              "  Year(DATEADD(year, 0, getdate()))+"+count+"  AS Year " +


          "  FROM tbl_clinical_assessments               c " +
                   "  JOIN tbl_PS_RepeatingOrders         wro ON wro.account = c.account " +
                  "  JOIN tbl_product_table              pro on pro.id = wro.ID_product " +
                    " JOIN tbl_Account_Member             am  ON c.account = am.account " +
                                                              "  AND am.member = 1 " +
                  "  JOIN tbl_Account_Insurance          ins ON ins.Account = am.Account " +
                  "   JOIN tbl_Account_Information        ai  ON c.account = ai.account  " +
                   " JOIN tbl_payer_table                pay ON pay.id = ins.ID_payer  " +
                                                               " and pay.id in (7, 3179, 3739)  " +
                  "  JOIN tbl_Account_Member_Insurance   mis on mis.Account = am.Account " +
                                                              "  AND mis.Member = 1 " +
                                                               " AND mis.Entry_Number = ins.Entry_Number " +


          "  WHERE datepart(yy, dateadd(mm, c.duration, c.assessmentDate)) = (Year(DATEADD(year, 0, getdate()))+"+count+" ) " +
             "    AND(effective_date is Null or effective_date < GetDate()) " +
               "   AND(expiration_date is Null or expiration_date > GetDate())  " +
               "   AND c.DeleteDate IS NULL  " +
                " AND  ai.inactiveaccount = 0  " +
                "  AND am.deceased is null " +
                " AND c.ID = (select top 1 ID  " +
                              "  from tbl_Clinical_Assessments " +
                               " where Account = c.Account " +
                                "        and Member = c.Member " +
                                " order by AssessmentDate desc )  " +


        "	GROUP BY datepart(mm, dateadd(mm, c.duration, c.AssessmentDate)) " +
         "   ORDER BY datepart(mm, dateadd(mm, c.duration, c.AssessmentDate))").ToList<AssessmentDueData>();

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


                    string Query = "SELECT  Distinct " +
          "  am.Account,  " +
           " CONVERT(varchar(12),  " +
          "  DATEADD([month], ISNULL(ca.Duration, 24), ca.AssessmentDate))AS Expires,  " +
         "  ca.ID AS Clin_Ass_ID,  " +
        "	pay.id,  " +
        "	pay.state,  " +
        "	cast(DateDiff(yy,am.BirthDate,GetDate()) as int) AS Age, " +
      "  cast(ca.duration as int) AS  Duration " +

       "   FROM   tbl_Clinical_Assessments ca " +
            "    JOIN tbl_PS_RepeatingOrders         wro ON wro.account = ca.account  " +
            "    JOIN tbl_product_table              pro on pro.id = wro.ID_product  " +
             "   JOIN tbl_Account_Member             am ON ca.account = am.account  " +
                                                           " AND am.member = 1  " +
             "   JOIN tbl_Account_Insurance          ins ON ins.Account = am.Account  " +
             "   JOIN tbl_Account_Information        ai ON ca.account = ai.account  " +
             "   JOIN tbl_payer_table                pay ON pay.id = ins.ID_payer  " +
                                                          "  and pay.id in (7,3179,3739)  " +
            "	JOIN tbl_Account_Member_Insurance   mis on mis.Account = am.Account  " +
                                                        "    AND mis.Member = 1  " +
                                                        "    AND mis.Entry_Number = ins.Entry_Number  " +

      "  WHERE  " +
         "   datepart(yy, dateadd(mm, ca.duration, ca.assessmentDate)) =  " + yr +
           "   AND(effective_date is Null or effective_date < GetDate())  " +
            "  AND(expiration_date is Null or expiration_date > GetDate())  " +
            "  AND ca.DeleteDate IS NULL  " +
            " AND  ai.inactiveaccount = 0  " +
            "  AND am.deceased is null  " +
            "  AND ca.ID = (select top 1 ID  " +
                          "  from tbl_Clinical_Assessments  " +
                          "  where Account = ca.Account  " +
                            "        and Member = ca.Member  " +
                            " order by AssessmentDate desc )  " +


            " AND DATEADD([month], ISNULL(ca.Duration, 6), ca.AssessmentDate) between '" + firstdayOftheMonth + "' and '" + lastdayoftheMonth+ "'"+

      "  order by  " +
          
        "	CONVERT(varchar(12),  " +
         "   DATEADD([month], ISNULL(ca.Duration, 24), ca.AssessmentDate) ), am.Account, " +
        "	ca.ID ,pay.id,pay.state,age, duration";

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
    }
}