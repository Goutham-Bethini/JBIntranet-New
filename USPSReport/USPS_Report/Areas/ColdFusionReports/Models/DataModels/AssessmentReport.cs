using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class AssessmentReport
    {
        public class AssessmentReportVM
        {
            //[RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Invalid date format.")]
            //[DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
            public DateTime? Start { get; set; }
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
            public DateTime? End { get; set; }
            public int GrandTotal { get; set; }
            public int DIScount { get; set; }
            public IList<AssessmentReportData> Details { get; set; }            

        }
        public class AssessmentReportData
        {
            public string OperatorName { get; set; }
            public int Total { get; set; }
            public IList<OperatorData> OperatorsData { get; set; }            

        }
        public class OperatorDataFromDB
        {
            public string OperatorName { get; set; }
            public string Type { get; set; }
            public string DateWorked { get; set; }
            public int Qty { get; set; }
        }

        public class OperatorData
        {
            public string Type { get; set; }
            public string DateWorked { get; set; }
            public int Qty { get; set; }
        }

        public static AssessmentReportVM GetAssessmentReport(DateTime? start,DateTime? end)
        {
            AssessmentReportVM assessmentReportVM = new AssessmentReportVM();
            assessmentReportVM.Start = start;
            assessmentReportVM.End = end;
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetAssessmentReport", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        command.Parameters.AddWithValue("@start", start);
                        command.Parameters.AddWithValue("@end", end);
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(ds);
                        DataTable dt = new DataTable();
                        DataTable dt2 = new DataTable();
                        //DataTable dt3 = new DataTable();
                        dt = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        //dt3 = ds.Tables[2];
                        OperatorDataFromDB operatorDataFromDB;
                        List<OperatorDataFromDB> lstOperatorDataFromDB = new List<OperatorDataFromDB>();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                operatorDataFromDB = new OperatorDataFromDB();
                                operatorDataFromDB.OperatorName = dr[2].ToString();
                                operatorDataFromDB.Type = dr[1].ToString();
                                operatorDataFromDB.DateWorked = dr[0].ToString() != "" ? Convert.ToDateTime(dr[0].ToString()).ToShortDateString() : dr[0].ToString(); 
                                operatorDataFromDB.Qty = Convert.ToInt32(dr[3].ToString());
                                lstOperatorDataFromDB.Add(operatorDataFromDB);
                            }
                        }
                        assessmentReportVM.GrandTotal = lstOperatorDataFromDB.Sum(i => i.Qty);
                        OperatorDataFromDB operatorDataFromDB2;
                        List<OperatorDataFromDB> lstOperatorDataFromDB2 = new List<OperatorDataFromDB>();
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt2.Rows)
                            {
                                operatorDataFromDB2 = new OperatorDataFromDB();
                                operatorDataFromDB2.OperatorName = dr[2].ToString();
                                operatorDataFromDB2.Type = dr[1].ToString();
                                operatorDataFromDB2.DateWorked = dr[0].ToString() != "" ? Convert.ToDateTime(dr[0].ToString()).ToShortDateString() : dr[0].ToString(); 
                                operatorDataFromDB2.Qty = Convert.ToInt32(dr[3].ToString());
                                lstOperatorDataFromDB2.Add(operatorDataFromDB2);
                            }
                        }
                        assessmentReportVM.DIScount = lstOperatorDataFromDB2.Sum(i => i.Qty);
                        List<string> lstOperatorName= lstOperatorDataFromDB.Select(i => i.OperatorName).Distinct().ToList();
                        List<AssessmentReportData> lstAssessmentReportData = new List<AssessmentReportData>();
                        AssessmentReportData assessmentReportData; 
                        foreach(string s in lstOperatorName)
                        {
                            assessmentReportData = new AssessmentReportData();
                            //List<OperatorData> lstOperatorData = new List<OperatorData>();
                            //assessmentReportData.OperatorsData=new List<>
                            assessmentReportData.OperatorName = s;
                            List<OperatorData> lstOperatorData= lstOperatorDataFromDB.Where(t => t.OperatorName == s).Select(i => new OperatorData { DateWorked = i.DateWorked, Type = i.Type, Qty = i.Qty }).ToList();
                            //OperatorData operatorData;
                            //foreach(OperatorData item in lstOperatorData)
                            //{

                            //    operatorData = new OperatorData();
                            //    operatorData.DateWorked = item.DateWorked;
                            //    operatorData.Type = item.Type;
                            //    operatorData.Qty = item.Qty;
                            //    lstOperatorData.Add(operatorData);
                            //}
                            assessmentReportData.OperatorsData = lstOperatorData;
                            assessmentReportData.Total = lstOperatorData.Sum(i => i.Qty);
                            lstAssessmentReportData.Add(assessmentReportData);
                        }                 
                        assessmentReportVM.Details = lstAssessmentReportData;                        
                    }
                }
                return assessmentReportVM;
            }
            catch (Exception ex)
            {
                return new AssessmentReportVM();
            }
        }

    }
}