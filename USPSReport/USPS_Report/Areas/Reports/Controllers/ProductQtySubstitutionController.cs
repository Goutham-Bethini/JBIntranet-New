using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using USPS_Report.Areas.Reports.Models;
using USPS_Report.Models;
using static USPS_Report.Areas.Reports.Models.ProductQtySubstitution;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class ProductQtySubstitutionController : Controller
    {
        //ProductQtySubstitutionVM _vm
        public ActionResult ProductQtySubstitution()
        {
            ViewBag.message = null;
            ProductQtySubstitutionVM _vm = new ProductQtySubstitutionVM();
            _vm.AllProdSubstitutions= USPS_Report.Areas.Reports.Models.ProductQtySubstitution.GetPSData();
            var sub= TempData["FromSub"] as string;
            if (sub == "FromSub")
            {
                ViewBag.message = "Substitution is done successfully.";
            }
            //_vm.AllProdSubstitutions = TempData["AllPS"] as IList<AllProdSubstitution>;
            //_vm.AllProdQtySubstitutions = TempData["Existing"] as IList<AllProdQtySubstitution>;
            //_vm.AllRWOProdQtySubstitutions= TempData["Repeating"] as IList<AllRWOProdQtySubstitution>;            
            return View(_vm);
        }
        [HttpPost]
        public ActionResult ProdQtySubPreView(ProductQtySubstitutionVM _vm)
        {
            ViewBag.message = null;
            ProductQtySubstitution.ProductQtySubstitutionVM _locvm = new ProductQtySubstitution.ProductQtySubstitutionVM();
            IList<ProductOldNewQty> _list = new List<ProductOldNewQty>();
            if (_vm.Type == "Existing")
            {
                _list = USPS_Report.Areas.Reports.Models.ProductQtySubstitution.GetData(_vm.OldProdCode, _vm.NewProdCode);
            }
            else if (_vm.Type == "Repeating")
            {
                _list = USPS_Report.Areas.Reports.Models.ProductQtySubstitution.GetRWOData(_vm.OldProdCode, _vm.NewProdCode);
            }
            Session["Type"] = _vm.Type;
            _locvm.PreViewProductOldNewQtys = _list;
            _locvm.AllProdSubstitutions = USPS_Report.Areas.Reports.Models.ProductQtySubstitution.GetPSData();
            return View("ProductQtySubstitution", _locvm);
        }
        public ActionResult SelectedProdQtySub(ProductQtySubstitutionVM _vm)
        {
            var components = User.Identity.Name.Split('\\');
            var userName = components.Last();
            var Type = Session["Type"].ToString();
            USPS_Report.Areas.Reports.Models.ProductQtySubstitution.SelectedProducts(_vm, userName, Type);
            TempData["FromSub"] = "FromSub";
            return RedirectToAction("ProductQtySubstitution");
            //TempData["AllPS"] = _locvm.AllProdSubstitutions;
            //if(Type== "Existing")
            //{
            //    TempData["Existing"] = _locvm.AllProdQtySubstitutions;
            //}
            //else if (Type == "Repeating")
            //{
            //    TempData["Repeating"] = _locvm.AllRWOProdQtySubstitutions;
            //}  
            //ProductQtySubstitutionVM _locvm = new ProductQtySubstitutionVM();
            //_locvm.AllProdSubstitutions = USPS_Report.Areas.Reports.Models.ProductQtySubstitution.GetPSData();
            //ViewBag.message = "Substitution is done successfully.";
            //return View("ProductQtySubstitution", _locvm);
        }
        public ActionResult ExportReport(string oldProdCode, string newProdCode, string updatedDate, string updatedBy, string repeatingOrExisting)
        {
            string dir = @"\\JBMMIWEB001\StateAudit$\Files\";
            string reportFileName = string.Empty;
            string filePath = string.Empty;
            //try
            //{
            if (repeatingOrExisting == "Existing")
            {
                var rep = USPS_Report.Areas.Reports.Models.ProductQtySubstitution.GetProdSubReport(oldProdCode, newProdCode, updatedDate, updatedBy, repeatingOrExisting);
                reportFileName = "Product Sub_" + oldProdCode + "_" + newProdCode + "_" + Convert.ToDateTime(updatedDate).ToString("MMddyyyy") + "_" + updatedBy;
                filePath = Path.Combine(dir, reportFileName + ".xlsx");
                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Sheet1");
                    ws.Cell(1, 1).InsertTable(rep, false);
                    ws.Columns().AdjustToContents();
                    wb.SaveAs(filePath);
                }
                //byte[] fileByteArray = System.IO.File.ReadAllBytes(filePath);
                //return new FileContentResult(fileByteArray, "application/vnd.ms-excel") { FileDownloadName = reportFileName + ".xlsx" };
            }
            else if (repeatingOrExisting == "Repeating")
            {
                var rworep = USPS_Report.Areas.Reports.Models.ProductQtySubstitution.GetProdSubRWOReport(oldProdCode, newProdCode, updatedDate, updatedBy, repeatingOrExisting);
                reportFileName = "RWOs Product Sub_" + oldProdCode + "_" + newProdCode + "_" + Convert.ToDateTime(updatedDate).ToString("MMddyyyy") + "_" + updatedBy;
                filePath = Path.Combine(dir, reportFileName + ".xlsx");
                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Sheet1");
                    ws.Cell(1, 1).InsertTable(rworep, false);
                    ws.Columns().AdjustToContents();
                    wb.SaveAs(filePath);
                }
                //byte[] fileByteArray = System.IO.File.ReadAllBytes(filePath);
                //return new FileContentResult(fileByteArray, "application/vnd.ms-excel") { FileDownloadName = reportFileName + ".xlsx" };
            }
            byte[] fileByteArray = System.IO.File.ReadAllBytes(filePath);
            return new FileContentResult(fileByteArray, "application/vnd.ms-excel") { FileDownloadName = reportFileName + ".xlsx" };
            //}
            //catch (Exception ex)
            //{              
            //    return null;
            //}
        }
    }
}