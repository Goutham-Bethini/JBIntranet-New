using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using DotNet.Highcharts;

namespace USPS_Report.Areas.Reports.Models
{

    public class AssessmentReports
    {

        public static AssessmentVM GetAssessmentsData(DateTime? _startDt, DateTime? _endDt)
        {
            try
            {
                AssessmentVM _rec = new AssessmentVM();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {



                    var _list = (from c in _db.tbl_Clinical_Assessments
                                 join t in _db.tbl_Operator_Table
                                 on c.ID_CreateBy equals t.ID
                                 where c.AssessmentDate >= _startDt && c.AssessmentDate <= _endDt
                                 select new 
                                 {
                                     t.OperatorName,c.AssessmentDate,
                                    

                                 }).ToList();

                    var _list1 = (from t in _list
                                  group t by new { t.OperatorName, t.AssessmentDate } into p
                                  select new AssessmentData
                                  {
                                      Name = p.Key.OperatorName,
                                      Type= "New",
                                      Date = p.Key.AssessmentDate,
                                      Qty  = p.Count() 

                                  }).ToList();

                    var _list2 = (from c in _db.tbl_Clinical_Assessments
                                 join t in _db.tbl_Operator_Table
                                 on c.ID_Changed equals t.ID
                                 where c.LastChange >= _startDt && c.LastChange <= _endDt
                                 select new
                                 {
                                     t.OperatorName,
                                     c.LastChange,


                                 }).ToList();

                    var _list3 = (from t in _list2
                                  group t by new { t.OperatorName, t.LastChange.Value.Date } into p
                                  select new AssessmentData
                                  {
                                      Name = p.Key.OperatorName,
                                      Type = "Update",
                                      Date = p.Key.Date,
                                      Qty = p.Count()

                                  }).ToList();

                    var _dataList = _list1.Concat(_list3).OrderBy(t=>t.Name).ThenBy(t=>t.Type).ThenBy(t=>t.Date).ToList();

                    var _total = (from d in _dataList where d.Type == "Update"
                                  group d by new { d.Name } into t
                                  select new totalAssessmentData
                                  {
                                      count = t.Count(),
                                      Total = t.Sum(d=>d.Qty),
                                      name = t.Key.Name

                                  }).ToList();

                    _rec.assessmentData = _dataList;
                    _rec.totalAssessmentList = _total;

                   
                    return _rec;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new AssessmentVM();
            }

        }
    }
    public class AssessmentData
    {
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public int? Qty { get; set; }
        public string Name { get; set; }
    }

    public class totalAssessmentData {
        public int? Total { get; set; }
        public int? count { get; set; }

        public string name { get; set; }
    }

    public class AssessmentVM
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<AssessmentData> assessmentData { get; set; }

        public IList<totalAssessmentData> totalAssessmentList { get; set; }


        public Highcharts TotalAssessmentBarChart { get; set; }
    }
}