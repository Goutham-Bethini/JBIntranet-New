using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class ExpiringCMNs
    {
        public class ReportsVM
        {
            public IList<CMNreport> Details { get; set; }
        }

        public class CMNreport
        {
            public int ReportId { get; set; }
            public string ReportName { get; set; }
        }

        public class ExpiringCMNsVM
        {
            public int SelectedYear { get; set; }
            public int PreviousYear { get; set; }
            public int NextYear { get; set; }
            public string ReportName { get; set; }
            public IList<CMNreport> Reports { get; set; }
            public IList<ReportMonthWise> Details { get; set; }
        }
        public class ReportMonthWise
        {
            public string Month { get; set; }
            public int Expiring { get; set; }
            public int Recertified { get; set; }
            public int NotRecertified { get; set; }

        }
        public class CMNsReportVM
        {
            public string Month { get; set; }
            public string ReportName { get; set; }
            public int Count { get; set; }
            public IList<CMNreport> Reports { get; set; }
            public IList<ReportData> Details { get; set; }
        }

        public class ReportData
        {
            public int Account { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string EffectiveDate { get; set; }
            public Int16? Duration { get; set; }
            public string Expires { get; set; }
            public string RecertReturned { get; set; }
            public string DocType { get; set; }
            public string Doctor { get; set; }
            public string Phone { get; set; }
            public string isSpecialDoctor { get; set; }

        }
        public static IList<CMNreport> GetCMNreports()
        {            
            List<CMNreport> lstCMNreport = new List<CMNreport>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstCMNreport = (from item in _db.sp_GetCMNreports()
                                    select new CMNreport
                                    {
                                        ReportId = item.repID,
                                        ReportName = item.repName
                                    }
                               ).ToList();
                }                
                return lstCMNreport;
            }
            catch (Exception ex)
            {
                return new List<CMNreport>();
            }
        }

        public static IList<ReportMonthWise> GetData(int year, string reportName)
        {
            List<ReportMonthWise> lstReportData = new List<ReportMonthWise>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstReportData = (from item in _db.sp_CMNreportMonthsWise(year, reportName)
                                     select new ReportMonthWise
                                     {
                                         Month = item.Month,
                                         Expiring = item.Expiring.Value,
                                         Recertified = item.Recertified.Value,
                                         NotRecertified = item.NotRecertified.Value
                                     }
                               ).ToList();
                }
                return lstReportData;
            }
            catch (Exception ex)
            {
                return new List<ReportMonthWise>();
            }
        }

        public static CMNsReportVM GetReportData(string team, string month, string recert)
        {
            var arr = month.Split(' ');
            int monthNum = DateTime.ParseExact(arr[0], "MMMM", CultureInfo.CurrentCulture).Month;
            CMNsReportVM locCMNsReportVM = new CMNsReportVM();
            locCMNsReportVM.ReportName = team;
            locCMNsReportVM.Month = month;
            locCMNsReportVM.Reports = GetCMNreports();
            List<ReportData> lstReportData = new List<ReportData>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstReportData = (from item in _db.sp_GetCMNs_ReportData(Convert.ToInt32(arr[1]), monthNum, team, recert)
                                     select new ReportData
                                     {
                                         Account = item.Account,
                                         LastName = item.Last_Name,
                                         FirstName = item.First_Name,
                                         EffectiveDate = item.EffectiveDate.ToShortDateString(),
                                         Duration = item.Duration,
                                         Expires = item.Expiration.ToShortDateString(),
                                         RecertReturned = item.Recertified != null ? item.Recertified.Value.ToShortDateString() : null,
                                         DocType = item.DocTypeDescription,
                                         Doctor = item.DoctorName,
                                         Phone = item.Office_Phone,
                                         isSpecialDoctor = item.SpecialDoctor == 1 ? "Yes" : "No"
                                     }
                               ).ToList();
                }
                locCMNsReportVM.Details = lstReportData;
                locCMNsReportVM.Count = lstReportData.Count;
                return locCMNsReportVM;
            }
            catch (Exception ex)
            {
                return new CMNsReportVM();
            }
        }

        public class AlphaSplitVM
        {
            public IList<Department> DepartmentDetails { get; set; }
        }
        public class AlphaSplitUpdateVM
        {
            public int DeptId { get; set; }
            public string DeptName { get; set; }
            public string Message { get; set; }
            public IList<DeptEmployee> EmployeeDetails { get; set; }
        }
        public class Department
        {
            public int DeptId { get; set; }
            public string DeptName { get; set; }
            public IList<DeptEmployee> EmployeeDetails { get; set; }
        }

        public class DeptEmployee
        {
            public int EmpId { get; set; }
            public string EmpFullName { get; set; }
            public int? AlpId { get; set; }
            public string AlphaStart { get; set; }
            public string AlphaEnd { get; set; }
        }

        public class DeptsEmployees
        {
            public int DeptId { get; set; }
            public string DeptName { get; set; }
            public int EmpId { get; set; }
            public string EmpFullName { get; set; }
            public int AlpId { get; set; }
            public string AlphaStart { get; set; }
            public string AlphaEnd { get; set; }
        }

        public static AlphaSplitVM GetDeptsEmpsDetails()
        {
            AlphaSplitVM alphaSplitVM = new AlphaSplitVM();
            List<Department> lstDepartments = new List<Department>();
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetASdeptsemps", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(ds);
                        DataTable dt = new DataTable();
                        DataTable dt2 = new DataTable();
                        dt = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        Department department;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                department = new Department();
                                department.DeptId = Convert.ToInt32(dr[0].ToString());
                                department.DeptName = dr[1].ToString();
                                lstDepartments.Add(department);
                            }
                        }

                        DeptsEmployees deptsEmployeesFromDB;
                        List<DeptsEmployees> lstDeptsEmployees = new List<DeptsEmployees>();
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dt2.Rows)
                            {
                                deptsEmployeesFromDB = new DeptsEmployees();
                                deptsEmployeesFromDB.DeptId = Convert.ToInt32(dr[0].ToString());
                                deptsEmployeesFromDB.DeptName = dr[1].ToString();
                                deptsEmployeesFromDB.EmpId = Convert.ToInt32(dr[2].ToString());
                                deptsEmployeesFromDB.EmpFullName = dr[3].ToString();
                                deptsEmployeesFromDB.AlpId = Convert.ToInt32(dr[4].ToString());
                                deptsEmployeesFromDB.AlphaStart = dr[5].ToString();
                                deptsEmployeesFromDB.AlphaEnd = dr[6].ToString();
                                lstDeptsEmployees.Add(deptsEmployeesFromDB);
                            }

                            foreach (Department item in lstDepartments)
                            {
                                var res = lstDeptsEmployees.Where(i => i.DeptId == item.DeptId).ToList();
                                if (res != null && res.Count > 0)
                                {
                                    DeptEmployee deptEmployee;
                                    List<DeptEmployee> lstDeptEmployee = new List<DeptEmployee>();
                                    foreach (DeptsEmployees locitem in res)
                                    {
                                        deptEmployee = new DeptEmployee();
                                        deptEmployee.EmpId = locitem.EmpId;
                                        deptEmployee.EmpFullName = locitem.EmpFullName;
                                        deptEmployee.AlpId = locitem.AlpId;
                                        deptEmployee.AlphaStart = locitem.AlphaStart;
                                        deptEmployee.AlphaEnd = locitem.AlphaEnd;
                                        lstDeptEmployee.Add(deptEmployee);
                                    }
                                    item.EmployeeDetails = lstDeptEmployee;
                                }
                            }
                        }
                    }
                }
                alphaSplitVM.DepartmentDetails = lstDepartments;
                return alphaSplitVM;
            }
            catch (Exception ex)
            {
                return new AlphaSplitVM();
            }
        }

        public static AlphaSplitUpdateVM GetDeptEmployees(int deptId, string dept)
        {
            AlphaSplitUpdateVM alphaSplitUpdateVM = new AlphaSplitUpdateVM();
            alphaSplitUpdateVM.DeptId = deptId;
            alphaSplitUpdateVM.DeptName = dept;
            List<DeptEmployee> lstDeptEmployee = new List<DeptEmployee>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstDeptEmployee = (from item in _db.sp_GetDeptEmployees(deptId)
                                       select new DeptEmployee
                                       {
                                           EmpId = item.empID,
                                           EmpFullName = item.empFullName,
                                           AlpId = item.alpID,
                                           AlphaStart = item.alpAlphaStart,
                                           AlphaEnd = item.alpAlphaEnd
                                       }
                               ).ToList();
                }
                alphaSplitUpdateVM.EmployeeDetails = lstDeptEmployee;
                return alphaSplitUpdateVM;
            }
            catch (Exception ex)
            {
                return new AlphaSplitUpdateVM();
            }
        }

        public static AlphaSplitUpdateVM UpdateDeptEmployees(AlphaSplitUpdateVM alphaSplitUpdateVM, string currentUser)
        {
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionIntranetEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionIntranetEntities())
                {
                    foreach (DeptEmployee item in alphaSplitUpdateVM.EmployeeDetails)
                    {
                        var res = _db.Employees_AlphaSplit.Where(i => i.alpEmpID == item.EmpId && i.alpDeptID == alphaSplitUpdateVM.DeptId).FirstOrDefault();

                        if (res != null)
                        {
                            res.alpAlphaStart = item.AlphaStart;
                            res.alpAlphaEnd = item.AlphaEnd;
                            res.alpEdited = DateTime.Now;
                            res.alpEditedBy = currentUser;
                        }
                        else
                        {
                            Employees_AlphaSplit employees_AlphaSplit = new Employees_AlphaSplit();
                            employees_AlphaSplit.alpDeptID = alphaSplitUpdateVM.DeptId;
                            employees_AlphaSplit.alpEmpID = item.EmpId;
                            employees_AlphaSplit.alpAlphaStart = item.AlphaStart;
                            employees_AlphaSplit.alpAlphaEnd = item.AlphaEnd;
                            employees_AlphaSplit.alpAdded = DateTime.Now;
                            employees_AlphaSplit.alpAddedBy = currentUser;
                            _db.Employees_AlphaSplit.Add(employees_AlphaSplit);
                        }
                    }
                    _db.SaveChanges();
                    alphaSplitUpdateVM.Message = "Updated Successfully";
                }
                return alphaSplitUpdateVM;
            }
            catch (Exception ex)
            {
                alphaSplitUpdateVM.Message = "Sorry, Not Updated!";
                return alphaSplitUpdateVM;
            }
        }

        public class ReportsRulesVM
        {
            public IList<CMNreport> Details { get; set; }
        }
        public class ReportRulesDataVM
        {
            public int ReportId { get; set; }
            public string Report { get; set; }
            public string Message { get; set; }
            public int? SelPayer { get; set; }
            public int? SelPayer2 { get; set; }
            public int? SelPayer3 { get; set; }
            public int? SelPayerType { get; set; }
            public int? SelPayerType2 { get; set; }
            public int? SelPayerType3 { get; set; }
            public int? SelDocType { get; set; }
            public int? SelDocType2 { get; set; }
            public int? SelDocType3 { get; set; }
            public int? SelProvider { get; set; }
            public int? SelProvider2 { get; set; }
            public int? SelProvider3 { get; set; }
            public int? SelStatusCode { get; set; }
            public int? SelStatusCode2 { get; set; }
            public int? SelStatusCode3 { get; set; }
            public int? SelCategory { get; set; }
            public int? SelCategory2 { get; set; }
            public int? SelCategory3 { get; set; }
            public string SelIncExc { get; set; }
            public string SelIncExc2 { get; set; }
            public string SelIncExc3 { get; set; }
            public string SelPTIncExc { get; set; }
            public string SelPTIncExc2 { get; set; }
            public string SelPTIncExc3 { get; set; }
            public string SelDTIncExc { get; set; }
            public string SelDTIncExc2 { get; set; }
            public string SelDTIncExc3 { get; set; }
            public string SelPIncExc { get; set; }
            public string SelPIncExc2 { get; set; }
            public string SelPIncExc3 { get; set; }
            public string SelSCIncExc { get; set; }
            public string SelSCIncExc2 { get; set; }
            public string SelSCIncExc3 { get; set; }
            public string SelCIncExc { get; set; }
            public string SelCIncExc2 { get; set; }
            public string SelCIncExc3 { get; set; }
            public SelectList Payers { get; set; }
            public SelectList PayerTypes { get; set; }
            public SelectList DocTypes { get; set; }
            public SelectList Providers { get; set; }
            public SelectList StatusCodes { get; set; }
            public SelectList Categories { get; set; }
            public IList<ReportRule> Details { get; set; }

        }
        public class Payer
        {
            public int PayerId { get; set; }
            public string PayerName { get; set; }

        }

        public class PayerType
        {
            public int PayerTypeId { get; set; }
            public string PayerTypeTitle { get; set; }

        }
        public class DocType
        {
            public int Id { get; set; }
            public string Description { get; set; }

        }
        public class Provider
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }
        public class StatusCode
        {
            public int Id { get; set; }
            public string StatusDescription { get; set; }

        }
        public class Category
        {
            public int Id { get; set; }
            public string Description { get; set; }

        }
        public class ReportRule
        {
            public int? cmrId { get; set; }
            public string DataType { get; set; }
            public string value { get; set; }
            public string ExcInc { get; set; }

        }

        public static IList<ReportRule> GetReportRules(int reportId)
        {            
            List<ReportRule> lstReportRule = new List<ReportRule>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstReportRule = (from item in _db.sp_GetReportRules(reportId)
                                     select new ReportRule
                                     {
                                         cmrId = item.cmrId,
                                         DataType = item.DataType,
                                         value = item.value,
                                         ExcInc = item.ExcInc
                                     }
                               ).ToList();
                }                
                return lstReportRule;
            }
            catch (Exception ex)
            {
                return new List<ReportRule>();
            }
        }

        public static string DeleteReportRule(int cmrId, string currentUser)
        {           
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionIntranetEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionIntranetEntities())
                {

                    var res = _db.CMN_Rules.Where(i => i.cmrID == cmrId).FirstOrDefault();

                    if (res != null)
                    {
                        res.cmrDeleted = DateTime.Now;
                        res.cmrDeletedBy = currentUser;
                    }
                    _db.SaveChanges();                   
                }
                return "Rule Deleted";
            }
            catch (Exception ex)
            {                
                return "Sorry, Rule Not Deleted";
            }
        }

        public static IList<Payer> GetPayers()
        {
            List<Payer> lstPayer = new List<Payer>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstPayer = (from item in _db.sp_GetPayers()
                                select new Payer
                                {
                                    PayerId = item.ID,
                                    PayerName = item.Name
                                }
                               ).ToList();
                }
                return lstPayer;
            }
            catch (Exception ex)
            {
                return new List<Payer>();
            }
        }

        public static string AddRules(ReportRulesDataVM reportRulesDataVM,string currentUser)
        {
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionIntranetEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionIntranetEntities())
                {
                    CMN_Rules objCMN_Rules;
                    if (reportRulesDataVM.SelPayer != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrPayerID = reportRulesDataVM.SelPayer;
                        if (reportRulesDataVM.SelIncExc == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelIncExc == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelPayer2 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrPayerID = reportRulesDataVM.SelPayer2;
                        if (reportRulesDataVM.SelIncExc2 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelIncExc2 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelPayer3 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrPayerID = reportRulesDataVM.SelPayer3;
                        if (reportRulesDataVM.SelIncExc3 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelIncExc3 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelPayerType != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrPayerTypeID = reportRulesDataVM.SelPayerType;
                        if (reportRulesDataVM.SelPTIncExc == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelPTIncExc == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelPayerType2 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrPayerTypeID = reportRulesDataVM.SelPayerType2;
                        if (reportRulesDataVM.SelPTIncExc2 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelPTIncExc2 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelPayerType3 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrPayerTypeID = reportRulesDataVM.SelPayerType3;
                        if (reportRulesDataVM.SelPTIncExc3 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelPTIncExc3 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelDocType != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrDocTypeID = reportRulesDataVM.SelDocType;
                        if (reportRulesDataVM.SelDTIncExc == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelDTIncExc == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelDocType2 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrDocTypeID = reportRulesDataVM.SelDocType2;
                        if (reportRulesDataVM.SelDTIncExc2 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelDTIncExc2 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelDocType3 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrDocTypeID = reportRulesDataVM.SelDocType3;
                        if (reportRulesDataVM.SelDTIncExc3 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelDTIncExc3 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelProvider != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrProviderID = reportRulesDataVM.SelProvider;
                        if (reportRulesDataVM.SelPIncExc == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelPIncExc == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelProvider2 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrProviderID = reportRulesDataVM.SelProvider2;
                        if (reportRulesDataVM.SelPIncExc2 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelPIncExc2 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelProvider3 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrProviderID = reportRulesDataVM.SelProvider3;
                        if (reportRulesDataVM.SelPIncExc3 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelPIncExc3 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelStatusCode != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrStatusCodeID = reportRulesDataVM.SelStatusCode;
                        if (reportRulesDataVM.SelSCIncExc == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelSCIncExc == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelStatusCode2 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrStatusCodeID = reportRulesDataVM.SelStatusCode2;
                        if (reportRulesDataVM.SelSCIncExc2 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelSCIncExc2 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelStatusCode3 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrStatusCodeID = reportRulesDataVM.SelStatusCode3;
                        if (reportRulesDataVM.SelSCIncExc3 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelSCIncExc3 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelCategory != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrCategoryID = reportRulesDataVM.SelCategory;
                        if (reportRulesDataVM.SelCIncExc == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelCIncExc == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelCategory2 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrCategoryID = reportRulesDataVM.SelCategory2;
                        if (reportRulesDataVM.SelCIncExc2 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelCIncExc2 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    if (reportRulesDataVM.SelCategory3 != null)
                    {
                        objCMN_Rules = new CMN_Rules();
                        objCMN_Rules.cmrCreated = DateTime.Now;
                        objCMN_Rules.cmrCreatedBy = currentUser;
                        objCMN_Rules.cmrReportType = reportRulesDataVM.ReportId;
                        objCMN_Rules.cmrCategoryID = reportRulesDataVM.SelCategory3;
                        if (reportRulesDataVM.SelCIncExc3 == "Include")
                            objCMN_Rules.cmrInclude = 1;
                        else if (reportRulesDataVM.SelCIncExc3 == "Exclude")
                            objCMN_Rules.cmrExclude = 1;
                        _db.CMN_Rules.Add(objCMN_Rules);
                    }
                    _db.SaveChanges();
                }
                return "Rules Added";
            }
            catch (Exception ex)
            {                
                return "Sorry, Rules not Added";
            }
        }

        public static IList<PayerType> GetPayerTypes()
        {
            List<PayerType> lstPayerType = new List<PayerType>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstPayerType = (from item in _db.sp_GetPayerTypes()
                                select new PayerType
                                {
                                    PayerTypeId = item.ID,
                                    PayerTypeTitle = item.Title
                                }
                               ).ToList();
                }
                return lstPayerType;
            }
            catch (Exception ex)
            {
                return new List<PayerType>();
            }
        }

        public static IList<DocType> GetDocTypes()
        {
            List<DocType> lstDocType = new List<DocType>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstDocType = (from item in _db.sp_GetDocTypes()
                                    select new DocType
                                    {
                                        Id = item.ID,
                                        Description = item.DocTypeDescription
                                    }
                               ).ToList();
                }
                return lstDocType;
            }
            catch (Exception ex)
            {
                return new List<DocType>();
            }
        }
        public static IList<Provider> GetProviders()
        {
            List<Provider> lstProvider = new List<Provider>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstProvider = (from item in _db.sp_GetProviders()
                                  select new Provider
                                  {
                                      Id = item.ID,
                                      Name = item.OrganizationName
                                  }
                               ).ToList();
                }
                return lstProvider;
            }
            catch (Exception ex)
            {
                return new List<Provider>();
            }
        }

        public static IList<StatusCode> GetStatusCodes()
        {
            List<StatusCode> lstStatusCode = new List<StatusCode>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstStatusCode = (from item in _db.sp_GetStatusCodes()
                                   select new StatusCode
                                   {
                                       Id = item.ID,
                                       StatusDescription = item.StatusDescription
                                   }
                               ).ToList();
                }
                return lstStatusCode;
            }
            catch (Exception ex)
            {
                return new List<StatusCode>();
            }
        }
        public static IList<Category> GetCategories()
        {
            List<Category> lstCategory = new List<Category>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstCategory = (from item in _db.sp_GetCategories()
                                     select new Category
                                     {
                                         Id = item.ID,
                                         Description = item.Description
                                     }
                               ).ToList();
                }
                return lstCategory;
            }
            catch (Exception ex)
            {
                return new List<Category>();
            }
        }
    }
}