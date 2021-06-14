using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using static USPS_Report.Areas.Reports.Models.MIStateAudit;
using System.Data;
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Configuration;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class MIStateAuditController : Controller
    {
        string  stateAuditFolderPath = ConfigurationManager.AppSettings["StateAuditFolderDirectory"]; 
        // GET: Reports/MIStateAudit
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MIStateAuditData()
        {
            ViewBag.message = null;
            MIStateAuditVM _vm = new MIStateAuditVM();
            IList<MIStateAuditData> _list = new List<MIStateAuditData>();
            _list = MIStateAudit.GetMIStateAuditData();
            _vm.Details = _list;
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',55,GETDATE())";
                int rowsinsert = _db.Database.ExecuteSqlCommand(query);
            }
            return View(_vm);
        }
        public ActionResult MIStateAuditGenerateNewReport(MIStateAuditVM objMIStateAuditVM)
        {            
            try
            {
                ViewBag.message = null;
                DateTime startDate = DateTime.Now, endDate = startDate.AddDays(1);
                if (objMIStateAuditVM.Quarter == "1stQtr")
                {
                    startDate = Convert.ToDateTime("1/1/" + objMIStateAuditVM.Year);
                    endDate = Convert.ToDateTime("3/31/" + objMIStateAuditVM.Year);
                }
                else if (objMIStateAuditVM.Quarter == "2ndQtr")
                {
                    startDate = Convert.ToDateTime("4/1/" + objMIStateAuditVM.Year);
                    endDate = Convert.ToDateTime("6/30/" + objMIStateAuditVM.Year);
                }
                else if (objMIStateAuditVM.Quarter == "3rdQtr")
                {
                    startDate = Convert.ToDateTime("7/1/" + objMIStateAuditVM.Year);
                    endDate = Convert.ToDateTime("9/30/" + objMIStateAuditVM.Year);
                }
                else if (objMIStateAuditVM.Quarter == "4thQtr")
                {
                    startDate = Convert.ToDateTime("10/1/" + objMIStateAuditVM.Year);
                    endDate = Convert.ToDateTime("12/31/" + objMIStateAuditVM.Year);
                }
                dynamic lst = null;
                string reportFileName = "";
                if (objMIStateAuditVM.Report == "New Accounts")
                {
                    lst = GetMINewAccounts(startDate, endDate);
                    reportFileName = "NewAccounts";
                }
                else if (objMIStateAuditVM.Report == "Products Added")
                {
                    lst = GetMIPRODUCTSADDED(startDate, endDate);
                    reportFileName = "ProductAdded";
                }
                else if (objMIStateAuditVM.Report == "Deactivated Accounts")
                {
                    lst = GetMIDeactivatedAccounts(startDate, endDate);
                    reportFileName = "DeactivatedAccounts";
                }

                if (lst != null && lst.Count > 0)
                {
                    DateTime curDateTime = DateTime.Now;
                    reportFileName = reportFileName + "_" + objMIStateAuditVM.Quarter + "_" + objMIStateAuditVM.Year.ToString();
                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt = Extensions.ToDataTable(lst);
                   
                   
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                       
                        string dir = stateAuditFolderPath;
                        wb.Worksheets.Add(dt, reportFileName);
                        reportFileName = reportFileName + "_" + curDateTime.ToString("MMddyyyyhhmmss");
                        string filePath = Path.Combine(dir, reportFileName + ".xlsx");
                        wb.SaveAs(filePath);
                        using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                        {
                            _db.Database.ExecuteSqlCommand("Insert into HHSQLDB_Pharmacy.dbo.StateAuditReports Values(@report,@quarter,@year,@dateCreated,@fileName)", new SqlParameter("report", objMIStateAuditVM.Report), new SqlParameter("quarter", Convert.ToInt32(objMIStateAuditVM.Quarter.Substring(0, 1))), new SqlParameter("year", Convert.ToDouble(objMIStateAuditVM.Year)), new SqlParameter("dateCreated", curDateTime), new SqlParameter("fileName", reportFileName));
                            //_db.sp_InsertStateAuditReports(objMIStateAuditVM.Report, Convert.ToInt32(objMIStateAuditVM.Quarter.Substring(0, 1)),
                            //Convert.ToDouble(objMIStateAuditVM.Year), reportFileName);
                        }
                        MemoryStream stream = GetStream(wb);// The method is defined below
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition",
                        "attachment; filename=" + reportFileName + ".xlsx");
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.BinaryWrite(stream.ToArray());                        
                        Response.End();
                    }
                                        
                }
            }
            catch (Exception ex)
            { }          
            return RedirectToAction("MIStateAuditData", "MIStateAudit");            
        }
        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
        public ActionResult DownloadReportFile(string fileName)
        {
            try
            {
                ViewBag.message = null;

                 
                var path = stateAuditFolderPath;
                fileName = fileName.Trim();
                if (System.IO.File.Exists(stateAuditFolderPath + fileName + ".csv"))
                {
                    fileName = fileName   + ".csv";

                }
                else if (System.IO.File.Exists(stateAuditFolderPath + fileName + ".xlsx"))
                {
                    fileName = fileName  + ".xlsx";

                }
                byte[] fileByteArray = System.IO.File.ReadAllBytes(path+fileName);
                return new FileContentResult(fileByteArray, "application/vnd.ms-excel") { FileDownloadName = fileName };

            }
            catch(Exception ex)
            {
                ViewBag.message = "Oops! The system cannot find the file, you can regenerate it.";                
                MIStateAuditVM _vm = new MIStateAuditVM();
                IList<MIStateAuditData> _list = new List<MIStateAuditData>();
                _list = MIStateAudit.GetMIStateAuditData();
                _vm.Details = _list;
                return View("MIStateAuditData",_vm);                
            }           
        }        
    }
}