using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using static USPS_Report.Areas.Reports.Models.InsulinPumpSuppliesNot;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class InsulinPumpSuppliesNotController : Controller
    {
        // GET: Reports/InsulinPumpSuppliesNot
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InsulinPumpSuppliesNot()
        {

            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',65,GETDATE())";

                int rowsinsert = _db.Database.ExecuteSqlCommand(query);
            }
            return View();
        }
        public ActionResult InsulinPumpSuppliesNotData([DataSourceRequest] DataSourceRequest request)
        {
            var jsonResult = Json(USPS_Report.Areas.Reports.Models.InsulinPumpSuppliesNot.GetInsulinPumpSupplies().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
       
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}