using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.CallTagSearch;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class CallTagSearchController : Controller
    {
        // GET: ColdFusionReports/CallTagSearch
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CallTagSearch()
        {
            return View();
        }
        public ActionResult CallTagSearchData(CallTagSearchVM callTagSearchVM)
        {
            CallTagSearchVM _vm = new CallTagSearchVM();
            _vm.CallTagSearchType = callTagSearchVM.CallTagSearchType;
            _vm.CallTagSearchValue = callTagSearchVM.CallTagSearchValue;
            IList<CallTagSearchData> _list = new List<CallTagSearchData>();
            _list = USPS_Report.Areas.ColdFusionReports.Models.DataModels.CallTagSearch.GetCallTagSearchData(callTagSearchVM.CallTagSearchType, Convert.ToInt32(callTagSearchVM.CallTagSearchValue));
            _vm.Details = _list;
            return View("CallTagSearch", _vm);
        }
    }
}