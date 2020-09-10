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
using System.Net.Mail;
using System.Security.Principal;


namespace USPS_Report.Areas.Reports.Controllers
{
    public class ClaimAdjustmentController : Controller
    {
        // GET: Reports/ClaimAdjustment
        string dir = @"\\JBMMIWEB001\StateAudit$\Files\";
        // GET: Reports/ManageOrders
        public ActionResult ClaimAdjustment()
        {
            var components = User.Identity.Name.Split('\\');
            var userName = components.Last();
            bool IsAllowed = IsInGroup(userName, "ClaimAdjustments");
            ViewBag.IsAllowed = IsAllowed;
            return View();
        }



        [HttpPost]
        public ActionResult ClaimAdjustment(HttpPostedFileBase file, string AdjustmentReason)
        {
            try
            {

                // Check validations
                HttpFileCollectionBase files = Request.Files;
                var errorMsg = "";
                int ID_Adjustment_Reason = 0;


                var components = User.Identity.Name.Split('\\');
                var userName = components.Last();


                if (files.Count == 0 || (files[0] != null && files[0].ContentLength > 0 && System.IO.Path.GetExtension(files[0].FileName).ToLower() != ".xlsx"))
                {
                    errorMsg = "Please select file with .xlsx extension! <br/>";
                }
                if (string.IsNullOrEmpty(AdjustmentReason) || AdjustmentReason == "1")
                {
                    errorMsg = errorMsg + "Please select Claim Adjustment reason";
                }
                if (!int.TryParse(AdjustmentReason, out ID_Adjustment_Reason))
                {
                    errorMsg = errorMsg + "Adjustment reason is not correct , please select it again";
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

                 // load data to table to use it as identifier table valued in stored procedure

                    DataTable tbClaims = new DataTable();
                    DataColumn column;
                    DataRow row;
                    // Create new DataColumn, set DataType, ColumnName and add to DataTable.  

                    // first column
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.ColumnName = "Claim";
                    tbClaims.Columns.Add(column);

                    // second column
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.ColumnName = "Account";
                    tbClaims.Columns.Add(column);

                    // third column
                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Decimal");
                    column.ColumnName = "Amount";
                    tbClaims.Columns.Add(column);

                      //   tbClaims.Columns.Add("Claim", typeof(Int32));
                     //  tbClaims.Columns.Add("Account", typeof(Int32));
                    //  tbClaims.Columns.Add("Amount", typeof(decimal));

                    // Read the excel file
                    using (XLWorkbook workbook = new XLWorkbook(path))
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(1);
                        bool dataExist = false;
                        //Range for reading the cells based on the last cell used.  
                        //Skip Header row
                        foreach (var WKrow in worksheet.RangeUsed().RowsUsed().Skip(1))
                        {
                            if (WKrow.Cell(1).Value != null && WKrow.Cell(1).Value != "")//!string.IsNullOrEmpty(row.Cell(1).Value.ToString()))
                            {
                                if (!dataExist)
                                {
                                    dataExist = true;
                                }


                                row = tbClaims.NewRow();
                                row["Claim"] = Convert.ToInt32(WKrow.Cell(1).Value);
                                row["Account"] = Convert.ToInt32(WKrow.Cell(2).Value);
                                row["Amount"] = Convert.ToDecimal(WKrow.Cell(3).Value);
                                tbClaims.Rows.Add(row);

                             //   tbClaims.Rows.Add(row["Claim"], row["Account"], row["Amount"]);


                                //  list.Add(Convert.ToInt32(row.Cell(1).Value.ToString()));
                                //tbClaims.Rows.Add(
                                //    Convert.ToInt32(WKrow.Cell(1).Value),
                                //    Convert.ToInt32(WKrow.Cell(2).Value),
                                //    Convert.ToDecimal(WKrow.Cell(3).Value)
                                //    );
                            }
                        }
                        if (System.IO.File.Exists(path))
                        {
                            // If file found, delete it    
                            System.IO.File.Delete(path);
                        }
                        if (tbClaims != null && tbClaims.Rows.Count > 0)
                        {
                          

                          //  DataTable Restable = new DataTable();
                            string ClaimsAmtNotMatched = string.Empty;

                            var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ReportsDbconnectionstring"].ConnectionString;
                            using (SqlConnection connection = new SqlConnection(conns))
                            {
                                connection.Open();
                               

                                using (SqlCommand command = new SqlCommand("sp_ClaimAdjLessthan25", connection) { CommandType = CommandType.StoredProcedure })
                                {
                                    command.CommandTimeout = 0;
                                    command.Parameters.AddWithValue("@AdjClaims", tbClaims);
                                    command.Parameters.AddWithValue("@OperatorName", userName);
                                    command.Parameters.AddWithValue("@ID_Adjustment_Reason", ID_Adjustment_Reason);

                                   var ReturnParameter = command.Parameters.Add("@ClaimsNotMatched", SqlDbType.VarChar, 250);

                                    ReturnParameter.Direction = ParameterDirection.Output;
                                    command.ExecuteNonQuery();

                                    ClaimsAmtNotMatched = Convert.ToString(ReturnParameter.Value);

                                    //using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                                    //{
                                    //    dataAdapter.Fill(Restable);
                                    //}
                                }
                            }

                          //  if (Restable.Rows.Count != 0)
                          //  {
                               // foreach (DataRow Resrow in Restable.Rows )
                               // {
                                   // var result = Convert.ToString(Resrow[0]).Length >2 ? Convert.ToString(Resrow[0]) : null;
                                    if (ClaimsAmtNotMatched != null && ClaimsAmtNotMatched != "")
                                    {
                                        SendEmail(false,  "Claim ADjustment <25", "Claims amount do not match with excel sheet for workorder :" + ClaimsAmtNotMatched, "grani@jandbmedical.com");
                                    }
                               // }

                             
                            //}

                           

                            return Json(new { Success = true, Message = "Successfully completed claim adjustments" }, JsonRequestBehavior.AllowGet);
                          //  }


                        }

                        //If no data in Excel file  
                        if (!dataExist)
                        {
                            return Json(new { Success = false, errorMsg = errorMsg +"Empty Excel File!" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                else
                {
                    //If file extension of the uploaded file is different then .xlsx  
                    return Json(new { Success = false, errorMsg = errorMsg+"Please select file with .xlsx extension!" }, JsonRequestBehavior.AllowGet);
                }

                return View();
            }
            catch (Exception ex)
            {
                SendEmail(true, "Claim ADjustment <25", "Claims adjustment having some issues , error :" +ex.Message , "grani@jandbmedical.com");
                throw;
            }

        }

        bool IsInGroup(string user, string group)
        {
            using (var identity = new WindowsIdentity(user))
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(group);
            }
        }


        public static void SendEmail(bool error, string  Subject , string Body, string Receipent )
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("jandbmedical-com.mail.protection.outlook.com");
            mail.To.Add(Receipent);
            mail.From = new MailAddress("noreply@jandbmedical.com");
         
            mail.Subject = Subject;
            if (error == true)
            {
                Body = "<font style='color:red ; font-size: 30px' >" + Body + "</font>";
            }
            else
            { Body = "<font style='color:blue ; font-size: 30px' >" + Body + "</font>"; }
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }
    }
}