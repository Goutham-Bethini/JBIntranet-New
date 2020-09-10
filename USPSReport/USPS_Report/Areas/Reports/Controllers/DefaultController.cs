using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Reports/Default
        public ActionResult Index()
        {
            return View();
        }
    }
}