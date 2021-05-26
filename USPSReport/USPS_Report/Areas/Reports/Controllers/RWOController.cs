using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class RWOController : Controller
    {
        // GET: Reports/RWO
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductSub()
        {
            ProductSubModel _vm = new ProductSubModel();
            IList<RwoProSub> proSublist = new List<RwoProSub>();
            _vm = ProdSubstitution.displayProdSubsList(_vm, User.Identity.Name.Split('\\').Last().ToLower());
          
            _vm.ValidOldProd = true;
            _vm.ValidNewProd = true;
            return View(_vm);
        }

        [HttpPost]
        public ActionResult ProductSub(ProductSubModel _vm)
        {
            _vm = ProdSubstitution.checkProdCode(_vm);
         
            IList<RwoProSub> proSublist = new List<RwoProSub>();
            _vm = ProdSubstitution.displayProdSubsList(_vm, User.Identity.Name.Split('\\').Last().ToLower());
            return View(_vm);
        }

       
        public ActionResult ProceedRWO(ProductSubModel _vm)
        {
            //update rwo product substitution table accordingly and change the tbl_ps_repeating orders
            ProdSubstitution.updateProdSubTable(_vm);


           return RedirectToAction("ProductSub");
        }
    }
}