using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Web.Mvc;

namespace USPS_Report.Areas.Reports.Models
{
    public class CSR
    {
        public static IList<ExpiredLMNs> GetExpiredLMNs()
        {

            try
            {
                IList<ExpiredLMNs> _list = new List<ExpiredLMNs>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {


                    _list = _db.Database.SqlQuery<ExpiredLMNs>("SELECT "+
             "   typ.id,  " +
              "  doctypedescription, " +
               " doc.Account,   " +
               " mem.FullName,  " +
               " doc.DocDescription,  " +
               " his.EffectiveDate,  " +
               " DateAdd(m, his.Duration, his.EffectiveDate) AS Expiration,  " +
               " DatePart(m, DateAdd(m, his.Duration, his.EffectiveDate)) AS ExpMonth,  " +
               " DatePart(yyyy, DateAdd(m, his.Duration, his.EffectiveDate)) AS ExpYear  " +
           " FROM  " +
            "            tbl_meddocuments        doc  " +
             "   JOIN    tbl_meddoc_type_table   typ ON typ.id = doc.id_typeofdoc  " +
              "  JOIN    tbl_meddoc_history      his ON his.id_cmn = doc.id  " +
                                               "        AND his.id = (  " +
                                                "        SELECT max(id)  " +
                                                 "       FROM tbl_MedDoc_History  " +
                                                  "      WHERE id_CMN = doc.id)  " +
                 " JOIN tbl_account_information inf ON inf.account = doc.account  " +
                 " JOIN dbo.tbl_Account_Member mem on mem.account = doc.account  " +
             " WHERE  " +
               "     (DocDescription LIKE '%LMN%' OR typ.doctypedescription LIKE '%LMN%')  " +
                " AND inf.inactiveaccount = 0  " +
               " order by Account").ToList<ExpiredLMNs>();


                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<ExpiredLMNs>();
            }


        }


        public static IList<ExpiredLMNs> GetExpiredLMNsFiltered(int? LMNid, int? mon, int? yr)
        {
           
            try
            {
                IList<ExpiredLMNs> _list = new List<ExpiredLMNs>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {


                    _list = _db.Database.SqlQuery<ExpiredLMNs>("SELECT " +
             "   typ.id,  " +
              "  doctypedescription, " +
               " doc.Account,   " +
               " mem.FullName,  " +
               " doc.DocDescription,  " +
               " his.EffectiveDate,  " +
               " DateAdd(m, his.Duration, his.EffectiveDate) AS Expiration,  " +
               " DatePart(m, DateAdd(m, his.Duration, his.EffectiveDate)) AS ExpMonth,  " +
               " DatePart(yyyy, DateAdd(m, his.Duration, his.EffectiveDate)) AS ExpYear  " +
           " FROM  " +
            "            tbl_meddocuments        doc  " +
             "   JOIN    tbl_meddoc_type_table   typ ON typ.id = doc.id_typeofdoc  " +
              "  JOIN    tbl_meddoc_history      his ON his.id_cmn = doc.id  " +
                                               "        AND his.id = (  " +
                                                "        SELECT max(id)  " +
                                                 "       FROM tbl_MedDoc_History  " +
                                                  "      WHERE id_CMN = doc.id)  " +
                 " JOIN tbl_account_information inf ON inf.account = doc.account  " +
                 " JOIN dbo.tbl_Account_Member mem on mem.account = doc.account  " +
             " WHERE  " +
               "     (DocDescription LIKE '%LMN%' OR typ.doctypedescription LIKE '%LMN%')  " +
                " AND inf.inactiveaccount = 0  " +
               " order by Account").ToList<ExpiredLMNs>();


                    var _reqlist = (from lst in _list
                                    where (LMNid == null || LMNid == lst.id)
                                    && (mon == null || lst.ExpMonth == mon)
                                    && (yr == null || lst.ExpYear == yr)
                                    select lst
                                    ).ToList();

                    return _reqlist;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<ExpiredLMNs>();
            }


        }

        public static IList<LMNTypeVm> GetLMNTypes(IList<ExpiredLMNs> Exlist)
        {

            try
            {
                LMNTypeVm vn = new LMNTypeVm();

                var _list = (from ex in Exlist
                              group ex by new { ex.id, ex.doctypedescription } into t
                              select new LMNTypeVm
                              {
                                  LMNType = t.Key.doctypedescription,
                                  LMNTypeid = t.Key.id
                              }).Distinct().OrderBy(t => t.LMNTypeid).ToList();
             

                    _list.Insert(0, vn);

                    return _list;

                
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<LMNTypeVm>();
            }
        }
        public static IList<Months> GetMonthList()
        {
            Months m = new Months();
            try
            {
               
                   
                    IList<Months> _list = new List<Months>();
                    for (int i = 1; i <= 12; i++)
                    {
                        Months mon = new Months();
                        mon.month = i;
                    _list.Add(mon);
                    }

                    _list.Insert(0, m);

                    return _list;

              
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<Months>();
            }
        }

        public static IList<Years> GetYearList(IList<ExpiredLMNs> Exlist)
        {
            Years yr = new Years();
            try
            {
                var _yrRange = (from lst in Exlist
                                group lst by new { lst.Expiration } into t
                                select new YearRange
                                {
                                    SmallestYear = t.Min(e => e.Expiration),
                                    LargestYear = t.Max(e => e.Expiration)

                                }).Take(1).SingleOrDefault();

                var smallestYear = (from lst in Exlist
                                    orderby lst.Expiration
                                    select lst.Expiration
                                     ).Take(1).SingleOrDefault();

                var largestYear = (from lst in Exlist
                                    orderby lst.Expiration descending
                                    select lst.Expiration
                                  ).Take(1).SingleOrDefault();

                IList<Years> _list = new List<Years>();
                    for (int i = smallestYear.Value.Year; i <= largestYear.Value.Year; i++)
                    {
                        Years Year = new Years();
                        Year.year = i;
                    _list.Add(Year);
                    }

                    _list.Insert(0, yr);

                    return _list;

               
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<Years>();
            }
        }
    }


    public class ExpiredLMNs
    {
        public int id { get; set; }
        public string doctypedescription { get; set; }

        public int account { get; set; }
        public string FullName { get; set; }
        public string DocDescription { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public DateTime? Expiration { get; set; }

        public int? ExpMonth { get; set; }
        public int? ExpYear { get; set; }
    }

    public class ExpiredLMNs_VM
    {
        public int? LMNTypeid { get; set; }
       public SelectList LMNTypeList { get; set; }
        public int? Month { get; set; }
        public SelectList MonthList { get; set; }
        public int? Year { get; set; }
        public SelectList YearList { get; set; }
        public IList<ExpiredLMNs> expiredLMNs { get; set; }
    }

public class LMNTypeVm
{
    public Int32? LMNTypeid { get; set; }
    public string LMNType { get; set; }

}
    public class Months
    {
        public Int32? month { get; set; }
    }
    public class Years
    {
        public Int32? year { get; set; }
    }

    public class YearRange
    {
        public DateTime? SmallestYear { get; set; }
        public DateTime? LargestYear { get; set; }
    }

}