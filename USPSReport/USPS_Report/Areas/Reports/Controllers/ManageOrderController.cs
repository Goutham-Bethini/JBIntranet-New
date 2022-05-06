using ClosedXML.Excel;
using ReportsDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.Reports.Models.ManageOrder;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class ManageOrderController : Controller
    {

        string dir = @"\\jbmwix-azfs01\IT\IntranetDocuments\StateAudit$\Files\";
        // GET: Reports/ManageOrders
        public ActionResult CancelOrders()
        {
            return View();
        }



        [HttpPost]
        public ActionResult CancelOrders(HttpPostedFileBase file, string Note)
        {
            try
            {

                // Check validations
                HttpFileCollectionBase files = Request.Files;
                var errorMsg = "";
                if (files.Count == 0 || (files[0] != null && files[0].ContentLength > 0 && System.IO.Path.GetExtension(files[0].FileName).ToLower() != ".xlsx"))
                {
                    errorMsg = "Please select file with .xlsx extension! <br/>";
                }
                if (string.IsNullOrEmpty(Note))
                {
                    errorMsg = errorMsg + "Please enter Cancel Note";
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return Json(new { Success = false, Message = errorMsg }, JsonRequestBehavior.AllowGet);
                }

                //Checking file content length and Extension must be .xlsx  
                if (files.Count > 0 && (files[0] != null && files[0].ContentLength > 0 && System.IO.Path.GetExtension(files[0].FileName).ToLower() == ".xlsx"))
                {

                    string path = Path.Combine(dir, Path.GetFileName(files[0].FileName));
                    //Saving the file  
                    file.SaveAs(path);

                    // Load data to SQL Server
                    var list = new List<Int32>();

                    // Read the excel file
                    using (XLWorkbook workbook = new XLWorkbook(path))
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(1);
                        bool dataExist = false;
                        //Range for reading the cells based on the last cell used.  
                        //Skip Header row
                        foreach (var row in worksheet.RangeUsed().RowsUsed().Skip(1))
                        {
                            if (row.Cell(1).Value != null && row.Cell(1).Value != "")//!string.IsNullOrEmpty(row.Cell(1).Value.ToString()))
                            {
                                if (!dataExist)
                                {
                                    dataExist = true;
                                }
                                list.Add(Convert.ToInt32(row.Cell(1).Value.ToString()));
                            }
                        }
                        var cancelList = new List<Int32>();
                        List<ExcludedOrder> excludeList = new List<ExcludedOrder>();
                        //var excludeList = new List<Int32>();
                        using (HHSQLDBEntities _db = new HHSQLDBEntities())
                        {
                            IList<tbl_PS_WorkOrder> _woListTemp = new List<tbl_PS_WorkOrder>();
                            _woListTemp = _db.tbl_PS_WorkOrder.Where(t => list.Contains(t.ID)).OrderByDescending(t => t.ID).ToList();

                            foreach (tbl_PS_WorkOrder item in _woListTemp)
                            {
                                string Status = item.Cancel_Date != null ? "Cancelled" : item.Completed_Date != null ? "Completed" : item.LastPrintDate != null ? "Printed/Sent to oracle" :
                                                (item.HoldFromShipping == 1 && item.HoldFromShippingReason == null) ? "Created" :
                                                (item.HoldFromShipping == 1 && item.HoldFromShippingReason != null) ? "Holding" :
                                                (item.HoldFromShipping == 1 && item.HoldFromShippingReason.Contains("%Back Order%")) ? "Back Ordered and Holding" :
                                                (item.HoldFromShipping == 1 && item.HoldFromShippingReason.Contains("Back Order ~")) ? "Back Ordered" : "Waiting to Interface";
                                //string Status = "Printed/Sent to oracle";
                                if (Status.Contains("Completed") || Status.Contains("Printed/Sent to oracle") || Status.Contains("Waiting to Interface"))
                                {
                                    excludeList.Add(new ExcludedOrder { WorkOrder = item.ID, Account = item.Account, Reason = Status });
                                }
                                else
                                {
                                    cancelList.Add(item.ID);
                                }
                            }
                        }
                        if (System.IO.File.Exists(path))
                        {
                            // If file found, delete it    
                            System.IO.File.Delete(path);
                        }
                        if (cancelList != null && cancelList.Any())
                        {
                            var components = User.Identity.Name.Split('\\');
                            var userName = components.Last();
                            using (var _db = new USPS_Report.Models.ReportsEntities())
                            {
                                _db.sp_CancelOrders(string.Join(",", cancelList.Select(n => n.ToString()).ToArray()), Note, userName);
                                //_db.sp_CancelOrders(string.Join(",", list.Select(n => n.ToString()).ToArray()), Note, userName);
                                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',29,GETDATE())";
                                int rowsinsert = _db.Database.ExecuteSqlCommand(query);
                                return Json(new { Success = true, ExcludedData = excludeList, Message = "Successfully completed cancel orders" }, JsonRequestBehavior.AllowGet);
                            }


                        }

                        //If no data in Excel file  
                        if (!dataExist)
                        {
                            return Json(new { Success = false, Message = "Empty Excel File!" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                else
                {
                    //If file extension of the uploaded file is different then .xlsx  
                    return Json(new { Success = false, Message = "Please select file with .xlsx extension!" }, JsonRequestBehavior.AllowGet);
                }

                return View();
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public ActionResult MassReleases()
        {
            return View();
        }


        [HttpPost]
        public ActionResult MassReleases(HttpPostedFileBase file, string Note)
        {
            try
            {
                // Check validations
                HttpFileCollectionBase files = Request.Files;
                var errorMsg = "";
                if (files.Count == 0 || (files[0] != null && files[0].ContentLength > 0 && System.IO.Path.GetExtension(files[0].FileName).ToLower() != ".xlsx"))
                {
                    errorMsg = "Please select file with .xlsx extension! <br/>";
                }
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return Json(new { Success = false, Message = errorMsg }, JsonRequestBehavior.AllowGet);
                }

                //Checking file content length and Extension must be .xlsx  
                if (files.Count > 0 && (files[0] != null && files[0].ContentLength > 0 && System.IO.Path.GetExtension(files[0].FileName).ToLower() == ".xlsx"))
                {

                    string path = Path.Combine(dir, Path.GetFileName(files[0].FileName));
                    //Saving the file  
                    file.SaveAs(path);
                    // Load data to SQL Server
                    var list = new List<Int32>();

                    // Read the excel file
                    using (XLWorkbook workbook = new XLWorkbook(path))
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(1);
                        bool dataExist = false;
                        //Range for reading the cells based on the last cell used.  
                        //Skip Header row
                        foreach (var row in worksheet.RangeUsed().RowsUsed().Skip(1))
                        {
                            if (row.Cell(1).Value != null && row.Cell(1).Value != "")//!string.IsNullOrEmpty(row.Cell(1).Value.ToString()))
                            {
                                if (!dataExist)
                                {
                                    dataExist = true;
                                }
                                list.Add(Convert.ToInt32(row.Cell(1).Value.ToString()));
                            }
                        }
                        if (System.IO.File.Exists(path))
                        {
                            // If file found, delete it    
                            System.IO.File.Delete(path);
                        }
                        if (list != null && list.Any())
                        {

                            var components = User.Identity.Name.Split('\\');
                            var userName = components.Last();
                            using (var _db = new USPS_Report.Models.ReportsEntities())
                            {
                                _db.sp_MassReleaseOrders(string.Join(",", list.Select(n => n.ToString()).ToArray()), userName);

                                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',30,GETDATE())";
                                int rowsinsert = _db.Database.ExecuteSqlCommand(query);
                                return Json(new { Success = true, Message = "Successfully released orders" }, JsonRequestBehavior.AllowGet);
                            }


                        }

                        //If no data in Excel file  
                        if (!dataExist)
                        {
                            //  ViewBag.Message = "Empty Excel File!";
                            return Json(new { Success = false, Message = "Empty Excel File!" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                else
                {
                    //If file extension of the uploaded file is different then .xlsx  
                    return Json(new { Success = false, Message = "Please select file with .xlsx extension!" }, JsonRequestBehavior.AllowGet);
                }

                return View();

            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public ActionResult UnCancelOrders()
        {
            return View();
        }



        [HttpPost]
        public ActionResult UnCancelOrders(HttpPostedFileBase file, string Note)
        {
            try
            {

                // Check validations
                HttpFileCollectionBase files = Request.Files;
                var errorMsg = "";
                if (files.Count == 0 || (files[0] != null && files[0].ContentLength > 0 && System.IO.Path.GetExtension(files[0].FileName).ToLower() != ".xlsx"))
                {
                    errorMsg = "Please select file with .xlsx extension! <br/>";
                    return Json(new { Success = false, Message = errorMsg }, JsonRequestBehavior.AllowGet);
                }

                //Checking file content length and Extension must be .xlsx  
                if (files.Count > 0 && (files[0] != null && files[0].ContentLength > 0 && System.IO.Path.GetExtension(files[0].FileName).ToLower() == ".xlsx"))
                {

                    string path = Path.Combine(dir, Path.GetFileName(files[0].FileName));
                    //Saving the file  
                    file.SaveAs(path);
                    // Load data to SQL Server
                    var list = new List<Int32>();

                    // Read the excel file
                    using (XLWorkbook workbook = new XLWorkbook(path))
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(1);
                        bool dataExist = false;
                        //Range for reading the cells based on the last cell used.  
                        //Skip Header row
                        foreach (var row in worksheet.RangeUsed().RowsUsed().Skip(1))
                        {
                            if (row.Cell(1).Value != null && row.Cell(1).Value != "")//!string.IsNullOrEmpty(row.Cell(1).Value.ToString()))
                            {
                                if (!dataExist)
                                {
                                    dataExist = true;
                                }
                                list.Add(Convert.ToInt32(row.Cell(1).Value.ToString()));
                            }
                        }
                        if (System.IO.File.Exists(path))
                        {
                            // If file found, delete it    
                            System.IO.File.Delete(path);
                        }
                        if (list != null && list.Any())
                        {

                            var components = User.Identity.Name.Split('\\');
                            var userName = components.Last();
                            using (var _db = new USPS_Report.Models.ReportsEntities())
                            {

                                _db.sp_UnCancelOrders(string.Join(",", list.Select(n => n.ToString()).ToArray()));
                                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',31,GETDATE())";
                                int rowsinsert = _db.Database.ExecuteSqlCommand(query);
                                return Json(new { Success = true, Message = "Successfully uncanceled orders" }, JsonRequestBehavior.AllowGet);
                            }


                        }

                        //If no data in Excel file  
                        if (!dataExist)
                        {
                            return Json(new { Success = false, Message = "Empty Excel File!" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                else
                {
                    //If file extension of the uploaded file is different then .xlsx  
                    return Json(new { Success = false, Message = "Please select file with .xlsx extension!" }, JsonRequestBehavior.AllowGet);
                }

                return View();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}