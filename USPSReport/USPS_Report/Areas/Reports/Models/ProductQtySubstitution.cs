using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using USPS_Report.Models;

namespace USPS_Report.Areas.Reports.Models
{
    public class ProductQtySubstitution
    {
        public class ProductQtySubstitutionVM
        {
            [Required]
            public string Type { get; set; }
            [Required]
            public string OldProdCode { get; set; }
            [Required]
            public string NewProdCode { get; set; }
            public IList<ProductOldNewQty> PreViewProductOldNewQtys { get; set; }
            public IList<AllProdSubstitution> AllProdSubstitutions { get; set; }
            public IList<AllProdQtySubstitution> AllProdQtySubstitutions { get; set; }
            public IList<AllRWOProdQtySubstitution> AllRWOProdQtySubstitutions { get; set; }
        }
        public class ProductOldNewQty
        {
            public string OldProdCode { get; set; }
            public string NewProdCode { get; set; }
            public int? OldQty { get; set; }
            public int? NewQty { get; set; }
            public bool NeedUpdate { get; set; }
        }
        public static IList<AllProdSubstitution> GetPSData()
        {
            List<AllProdSubstitution> lstPSData = new List<AllProdSubstitution>();
            try
            {
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    lstPSData = (from item in _db.sp_GetAllProdQtySubs()
                               select new AllProdSubstitution
                               {
                                   OldProdCode = item.oldProdCode,
                                   NewProdCode = item.newProdCode,
                                   UpdatedDate = item.Updated_Date,
                                   UpdatedBy = item.Updated_By,
                                   NoOfAccounts=item.No_of_Accounts.Value,
                                   RepeatingOrExisting=item.Repeating_or_Existing
                               }
                               ).ToList();
                }
                return lstPSData;
            }
            catch (Exception ex)
            {
                return new List<AllProdSubstitution>();
            }
        }

        public static IList<ProductOldNewQty> GetData(string oldProdCode, string newProdCode)
        {
            List<ProductOldNewQty> lstData = new List<ProductOldNewQty>();
            try
            {
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    lstData = (from item in _db.sp_ProdSubToDoList(oldProdCode, newProdCode)
                               select new ProductOldNewQty
                               {
                                   OldProdCode = item.OldProdCode,
                                   OldQty = item.OldQty,
                                   NewProdCode = item.NewProdCode,
                                   NewQty = item.NewQty
                               }
                               ).ToList();
                }
                return lstData;
            }
            catch (Exception ex)
            {
                return new List<ProductOldNewQty>();
            }
        }
        public static IList<ProductOldNewQty> GetRWOData(string oldProdCode, string newProdCode)
        {
            List<ProductOldNewQty> lstData = new List<ProductOldNewQty>();
            try
            {
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    lstData = (from item in _db.sp_RWOProdSubToDoList(oldProdCode, newProdCode)
                               select new ProductOldNewQty
                               {
                                   OldProdCode = item.OldProdCode,
                                   OldQty = item.OldQty,
                                   NewProdCode = item.NewProdCode,
                                   NewQty = item.NewQty
                               }
                               ).ToList();
                }
                return lstData;
            }
            catch (Exception ex)
            {
                return new List<ProductOldNewQty>();
            }
        }
        public static ProductQtySubstitutionVM SelectedProducts(ProductQtySubstitutionVM _vm, string operatorName, string Type)
        {
            ProductQtySubstitutionVM _vmobj = new ProductQtySubstitutionVM();
            try
            {
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    int selected = 0;
                    if (Type == "Existing")
                    {
                        foreach (ProductOldNewQty item in _vm.PreViewProductOldNewQtys)
                        {
                            if (item.NeedUpdate)
                            {
                                ProdQtySubstitution_table prodQtySubstitution_table = new ProdQtySubstitution_table();
                                prodQtySubstitution_table.oldProdCode = item.OldProdCode;
                                prodQtySubstitution_table.oldQuantity = item.OldQty;
                                prodQtySubstitution_table.newProdCode = item.NewProdCode;
                                prodQtySubstitution_table.newQuantity = item.NewQty;
                                _db.ProdQtySubstitution_table.Add(prodQtySubstitution_table);
                                selected++;
                            }
                        }
                        _db.Database.ExecuteSqlCommand("TRUNCATE TABLE Reports.[dbo].ProdQtySubstitution_table");
                        _db.SaveChanges();
                        List<AllProdSubstitution> substitutions = new List<AllProdSubstitution>();
                        if (selected > 0)
                        {
                            substitutions = ApplySubstitution(operatorName, _vm);
                        }
                        _vmobj.Type = Type;
                        _vmobj.AllProdSubstitutions = substitutions;
                    }
                    else if (Type == "Repeating")
                    {
                        foreach (ProductOldNewQty item in _vm.PreViewProductOldNewQtys)
                        {
                            if (item.NeedUpdate)
                            {
                                RWOProdQtySubstitution_table objRWOProdQtySubstitution_table = new RWOProdQtySubstitution_table();
                                objRWOProdQtySubstitution_table.oldProdCode = item.OldProdCode;
                                objRWOProdQtySubstitution_table.oldQuantity = item.OldQty;
                                objRWOProdQtySubstitution_table.newProdCode = item.NewProdCode;
                                objRWOProdQtySubstitution_table.newQuantity = item.NewQty;
                                _db.RWOProdQtySubstitution_table.Add(objRWOProdQtySubstitution_table);
                                selected++;
                            }
                        }
                        _db.Database.ExecuteSqlCommand("TRUNCATE TABLE Reports.[dbo].RWOProdQtySubstitution_table");
                        _db.SaveChanges();
                        List<AllProdSubstitution> substitutions = new List<AllProdSubstitution>();
                        if (selected > 0)
                        {
                            substitutions = ApplyRWOSubstitution(operatorName, _vm);
                        }
                        _vmobj.Type = Type;
                        _vmobj.AllProdSubstitutions = substitutions;
                    }
                }
                return _vmobj;
            }
            catch (Exception ex)
            {
                return new ProductQtySubstitutionVM();
            }
        }

        public static List<AllProdSubstitution> ApplySubstitution(string operatorName, ProductQtySubstitutionVM _vm)
        {
            List<AllProdSubstitution> lstAllProdSubstitution = new List<AllProdSubstitution>();
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_DoProdQtySubstitution", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        command.Parameters.AddWithValue("@operatorName", operatorName);
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(ds);
                        DataTable dt = new DataTable();                        
                        dt = ds.Tables[0];
                        DataTable dtPS = new DataTable();
                        dtPS = ds.Tables[2];
                        AllProdSubstitution allProdSubstitution;
                        if (dtPS != null && dtPS.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtPS.Rows)
                            {
                                allProdSubstitution = new AllProdSubstitution();
                                allProdSubstitution.OldProdCode = dr[0].ToString();
                                allProdSubstitution.NewProdCode = dr[1].ToString();
                                allProdSubstitution.UpdatedDate = Convert.ToDateTime(dr[2].ToString());
                                allProdSubstitution.UpdatedBy = dr[3].ToString();
                                allProdSubstitution.NoOfAccounts = Convert.ToInt32(dr[4].ToString());
                                allProdSubstitution.RepeatingOrExisting = dr[5].ToString();
                                lstAllProdSubstitution.Add(allProdSubstitution);
                            }
                        }
                        //DataTable dt2 = new DataTable();
                        //dt2 = ds.Tables[1];
                        //AllProdQtySubstitution allProdQtySubstitution;
                        //if (dt2 != null && dt2.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt2.Rows)
                        //    {
                        //        allProdQtySubstitution = new AllProdQtySubstitution();
                        //        allProdQtySubstitution.Account = Convert.ToInt32(dr[0].ToString());
                        //        allProdQtySubstitution.WorkOrder = Convert.ToInt32(dr[1].ToString());
                        //        allProdQtySubstitution.oldProdCode = dr[2].ToString();
                        //        allProdQtySubstitution.oldQuantity = Convert.ToInt32(dr[3].ToString());
                        //        allProdQtySubstitution.newProdCode = dr[4].ToString();
                        //        allProdQtySubstitution.newQuantity = Convert.ToInt32(dr[5].ToString());
                        //        allProdQtySubstitution.UpdatedDate = Convert.ToDateTime(dr[6].ToString());
                        //        allProdQtySubstitution.UpdatedBy = dr[7].ToString();
                        //        lstAllProdQtySubstitution.Add(allProdQtySubstitution);
                        //    }
                        //}
                    }
                }
                
                //MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("jandbmedical-com.mail.protection.outlook.com");
                //mail.From = new MailAddress("noreply@jandbmedical.com");
                ////mail.To.Add("grani@jandbmedical.com");
                ////// mail.To.Add("grani@jandbmedical.com");
                ////mail.To.Add("ProdSubs@jandbmedical.com");
                ////mail.To.Add("disteamleader@jandbmedical.com");
                ////mail.To.Add("mott@jandbmedical.com");
                //mail.To.Add("maheshkattamuribpl@jandbmedical.com");
                //mail.Subject = "Product Substitution.";
                //mail.Body += "<html>";
                //mail.Body += "<head>";
                //mail.Body += "<style>";
                //mail.Body += "table, th, td { border: 1px solid black; }";
                //mail.Body += "</style>";
                //mail.Body += "</head>";
                //mail.Body += "<body>";
                //mail.Body += "Request for Product Substitution is done for below product quantities:";
                //mail.Body += "<br />";
                //mail.Body += "<table>"; /*style = 'border:1; border-style: solid;'*/
                //foreach (ProductOldNewQty item in _vm.PreViewProductOldNewQtys)
                //{
                //    if (item.NeedUpdate)
                //    {
                //        mail.Body += "<tr>";
                //        mail.Body += "<td>" + item.OldProdCode + "  </td> <td>" + item.OldQty + " </td><td>" + item.NewProdCode + "  </td> <td>" + item.NewQty + " </td>";
                //        mail.Body += "</tr>";
                //    }
                //}
                //mail.Body += "</table>";
                //mail.Body += "<br />";
                //mail.Body += "Thank You!";
                //mail.Body += "</body>";
                //mail.Body += "</html>";
                //mail.IsBodyHtml = true;
                //SmtpServer.Send(mail);
                
                return lstAllProdSubstitution;
            }
            catch (Exception ex)
            {
                return new List<AllProdSubstitution>();
            }
        }

        public static List<AllProdSubstitution> ApplyRWOSubstitution(string operatorName, ProductQtySubstitutionVM _vm)
        {
            List<AllProdSubstitution> lstAllProdSubstitution = new List<AllProdSubstitution>();
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_DoRWOProdQtySubstitution", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        command.Parameters.AddWithValue("@operatorName", operatorName);
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(ds);
                        DataTable dt = new DataTable();                        
                        dt = ds.Tables[0];
                        DataTable dtrwoPS = new DataTable();
                        dtrwoPS = ds.Tables[2];
                        AllProdSubstitution allProdSubstitution;
                        if (dtrwoPS != null && dtrwoPS.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtrwoPS.Rows)
                            {
                                allProdSubstitution = new AllProdSubstitution();
                                allProdSubstitution.OldProdCode =dr[0].ToString();
                                allProdSubstitution.NewProdCode = dr[1].ToString();
                                allProdSubstitution.UpdatedDate = Convert.ToDateTime(dr[2].ToString());
                                allProdSubstitution.UpdatedBy = dr[3].ToString();
                                allProdSubstitution.NoOfAccounts = Convert.ToInt32(dr[4].ToString());
                                allProdSubstitution.RepeatingOrExisting = dr[5].ToString();
                                lstAllProdSubstitution.Add(allProdSubstitution);
                            }
                        }
                        //DataTable dt2 = new DataTable();
                        //dt2 = ds.Tables[1];
                        //AllRWOProdQtySubstitution allRWOProdQtySubstitution;
                        //if (dt2 != null && dt2.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt2.Rows)
                        //    {
                        //        allRWOProdQtySubstitution = new AllRWOProdQtySubstitution();
                        //        allRWOProdQtySubstitution.Account = Convert.ToInt32(dr[0].ToString());
                        //        allRWOProdQtySubstitution.oldProdCode = dr[1].ToString();
                        //        allRWOProdQtySubstitution.oldQuantity = Convert.ToInt32(dr[2].ToString());
                        //        allRWOProdQtySubstitution.newProdCode = dr[3].ToString();
                        //        allRWOProdQtySubstitution.newQuantity = Convert.ToInt32(dr[4].ToString());
                        //        allRWOProdQtySubstitution.UpdatedDate = Convert.ToDateTime(dr[5].ToString());
                        //        allRWOProdQtySubstitution.UpdatedBy = dr[6].ToString();
                        //        lstAllRWOProdQtySubstitution.Add(allRWOProdQtySubstitution);
                        //    }
                        //}

                    }
                }
                
                //MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("jandbmedical-com.mail.protection.outlook.com");
                //mail.From = new MailAddress("noreply@jandbmedical.com");
                ////mail.To.Add("grani@jandbmedical.com");
                ////// mail.To.Add("grani@jandbmedical.com");
                ////mail.To.Add("ProdSubs@jandbmedical.com");
                ////mail.To.Add("disteamleader@jandbmedical.com");
                ////mail.To.Add("mott@jandbmedical.com");
                //mail.To.Add("maheshkattamuribpl@jandbmedical.com");
                //mail.Subject = "Product Substitution for RWO's.";
                //mail.Body += "<html>";
                //mail.Body += "<head>";
                //mail.Body += "<style>";
                //mail.Body += "table, th, td { border: 1px solid black; }";
                //mail.Body += "</style>";
                //mail.Body += "</head>";
                //mail.Body += "<body>";
                //mail.Body += "Request for Product Substitution is done for below product quantities:";
                //mail.Body += "<br />";
                //mail.Body += "<table>"; /*style = 'border:1; border-style: solid;'*/
                //foreach (ProductOldNewQty item in _vm.PreViewProductOldNewQtys)
                //{
                //    if (item.NeedUpdate)
                //    {
                //        mail.Body += "<tr>";
                //        mail.Body += "<td>" + item.OldProdCode + "  </td> <td>" + item.OldQty + " </td><td>" + item.NewProdCode + "  </td> <td>" + item.NewQty + " </td>";
                //        mail.Body += "</tr>";
                //    }
                //}
                //mail.Body += "</table>";
                //mail.Body += "<br />";
                //mail.Body += "Thank You!";
                //mail.Body += "</body>";
                //mail.Body += "</html>";
                //mail.IsBodyHtml = true;
                //SmtpServer.Send(mail);
                
                return lstAllProdSubstitution;
            }
            catch (Exception ex)
            {
                return new List<AllProdSubstitution>();
            }
        }
        public static List<ProductSubstitutionReport> GetProdSubReportTest(string oldProdCode, string newProdCode, string updatedDate, string updatedBy, string repeatingOrExisting)
        {
            List<ProductSubstitutionReport> lstProductSubstitutionReport = new List<ProductSubstitutionReport>();
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetProdSubAcDetails", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        command.Parameters.AddWithValue("@oldProdCode", oldProdCode);
                        command.Parameters.AddWithValue("@newProdCode", newProdCode);
                        command.Parameters.AddWithValue("@updatedDate", updatedDate);
                        command.Parameters.AddWithValue("@updatedBy", updatedBy);
                        command.Parameters.AddWithValue("@repeatingOrExisting", repeatingOrExisting);
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        //DataTable dtrwoPS = new DataTable();
                        //dtrwoPS = ds.Tables[2];
                        ProductSubstitutionReport productSubstitutionReport;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                productSubstitutionReport = new ProductSubstitutionReport();
                                productSubstitutionReport.Account = Convert.ToInt32(dr[0].ToString());
                                productSubstitutionReport.WorkOrder = Convert.ToInt32(dr[1].ToString());
                                productSubstitutionReport.OldProdCode = dr[2].ToString();
                                productSubstitutionReport.OldQuantity = Convert.ToInt32(dr[3].ToString());
                                productSubstitutionReport.NewProdCode = dr[4].ToString();
                                productSubstitutionReport.NewQuantity = Convert.ToInt32(dr[5].ToString());
                                productSubstitutionReport.UpdatedDate = Convert.ToDateTime(dr[6].ToString());
                                productSubstitutionReport.UpdatedBy = dr[7].ToString();                               
                                lstProductSubstitutionReport.Add(productSubstitutionReport);
                            }
                        }                     
                    }
                }
                return lstProductSubstitutionReport;
            }
            catch(Exception ex)
            {
                return new List<ProductSubstitutionReport>();
            }
        }

        public static DataTable GetProdSubReport(string oldProdCode, string newProdCode, string updatedDate, string updatedBy, string repeatingOrExisting)
        {
            //List<ProductSubstitutionReport> lstProductSubstitutionReport = new List<ProductSubstitutionReport>();
            DataTable dt = new DataTable();
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetProdSubAcDetails", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        command.Parameters.AddWithValue("@oldProdCode", oldProdCode);
                        command.Parameters.AddWithValue("@newProdCode", newProdCode);
                        command.Parameters.AddWithValue("@updatedDate", updatedDate);
                        command.Parameters.AddWithValue("@updatedBy", updatedBy);
                        command.Parameters.AddWithValue("@repeatingOrExisting", repeatingOrExisting);
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(ds);                        
                        dt = ds.Tables[0];
                        //DataTable dtrwoPS = new DataTable();
                        //dtrwoPS = ds.Tables[2];
                        //ProductSubstitutionReport productSubstitutionReport;
                        //if (dt != null && dt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        productSubstitutionReport = new ProductSubstitutionReport();
                        //        productSubstitutionReport.Account = Convert.ToInt32(dr[0].ToString());
                        //        productSubstitutionReport.WorkOrder = Convert.ToInt32(dr[1].ToString());
                        //        productSubstitutionReport.OldProdCode = dr[2].ToString();
                        //        productSubstitutionReport.OldQuantity = Convert.ToInt32(dr[3].ToString());
                        //        productSubstitutionReport.NewProdCode = dr[4].ToString();
                        //        productSubstitutionReport.NewQuantity = Convert.ToInt32(dr[5].ToString());
                        //        productSubstitutionReport.UpdatedDate = Convert.ToDateTime(dr[6].ToString());
                        //        productSubstitutionReport.UpdatedBy = dr[7].ToString();
                        //        lstProductSubstitutionReport.Add(productSubstitutionReport);
                        //    }
                        //}
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public static List<ProductSubstitutionRWOReport> GetProdSubRWOReportTest(string oldProdCode, string newProdCode, string updatedDate, string updatedBy, string repeatingOrExisting)
        {
            List<ProductSubstitutionRWOReport> lstProductSubstitutionRWOReport = new List<ProductSubstitutionRWOReport>();
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetProdSubAcDetails", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        command.Parameters.AddWithValue("@oldProdCode", oldProdCode);
                        command.Parameters.AddWithValue("@newProdCode", newProdCode);
                        command.Parameters.AddWithValue("@updatedDate", updatedDate);
                        command.Parameters.AddWithValue("@updatedBy", updatedBy);
                        command.Parameters.AddWithValue("@repeatingOrExisting", repeatingOrExisting);
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        //DataTable dtrwoPS = new DataTable();
                        //dtrwoPS = ds.Tables[2];
                        ProductSubstitutionRWOReport productSubstitutionRWOReport;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                productSubstitutionRWOReport = new ProductSubstitutionRWOReport();
                                productSubstitutionRWOReport.Account = Convert.ToInt32(dr[0].ToString());
                                productSubstitutionRWOReport.OldProdCode = dr[1].ToString();
                                productSubstitutionRWOReport.OldQuantity = Convert.ToInt32(dr[2].ToString());
                                productSubstitutionRWOReport.NewProdCode = dr[3].ToString();
                                productSubstitutionRWOReport.NewQuantity = Convert.ToInt32(dr[4].ToString());
                                productSubstitutionRWOReport.UpdatedDate = Convert.ToDateTime(dr[5].ToString());
                                productSubstitutionRWOReport.UpdatedBy = dr[6].ToString();
                                lstProductSubstitutionRWOReport.Add(productSubstitutionRWOReport);
                            }
                        }
                    }
                }
                return lstProductSubstitutionRWOReport;
            }
            catch (Exception ex)
            {
                return new List<ProductSubstitutionRWOReport>();
            }
        }

        public static DataTable GetProdSubRWOReport(string oldProdCode, string newProdCode, string updatedDate, string updatedBy, string repeatingOrExisting)
        {
            //List<ProductSubstitutionRWOReport> lstProductSubstitutionRWOReport = new List<ProductSubstitutionRWOReport>();
            DataTable dt = new DataTable();
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetProdSubAcDetails", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        command.Parameters.AddWithValue("@oldProdCode", oldProdCode);
                        command.Parameters.AddWithValue("@newProdCode", newProdCode);
                        command.Parameters.AddWithValue("@updatedDate", updatedDate);
                        command.Parameters.AddWithValue("@updatedBy", updatedBy);
                        command.Parameters.AddWithValue("@repeatingOrExisting", repeatingOrExisting);
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(ds);
                        dt = ds.Tables[0];
                        ////DataTable dtrwoPS = new DataTable();
                        ////dtrwoPS = ds.Tables[2];
                        //ProductSubstitutionRWOReport productSubstitutionRWOReport;
                        //if (dt != null && dt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        productSubstitutionRWOReport = new ProductSubstitutionRWOReport();
                        //        productSubstitutionRWOReport.Account = Convert.ToInt32(dr[0].ToString());
                        //        productSubstitutionRWOReport.OldProdCode = dr[1].ToString();
                        //        productSubstitutionRWOReport.OldQuantity = Convert.ToInt32(dr[2].ToString());
                        //        productSubstitutionRWOReport.NewProdCode = dr[3].ToString();
                        //        productSubstitutionRWOReport.NewQuantity = Convert.ToInt32(dr[4].ToString());
                        //        productSubstitutionRWOReport.UpdatedDate = Convert.ToDateTime(dr[5].ToString());
                        //        productSubstitutionRWOReport.UpdatedBy = dr[6].ToString();
                        //        lstProductSubstitutionRWOReport.Add(productSubstitutionRWOReport);
                        //    }
                        //}
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

    }

}