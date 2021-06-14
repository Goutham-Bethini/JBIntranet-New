using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ReportsDatabase;
using USPS_Report.Models;



namespace USPS_Report.Areas.Reports.Controllers
{
    public class OutboundCallsController : Controller
    {
        // GET: Reports/OutboundCalls
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reassessment()
        {
            ReassessmentCallsVM _vm = new ReassessmentCallsVM();
            IList<ReassmentCalls> _list = new List<ReassmentCalls>();
            _vm.StartDate = DateTime.Today;
            _vm.EndDate = DateTime.Today.AddDays(1);
            _list = SMOutbound.getOutboundReassessmentCalls(_vm.StartDate, _vm.EndDate);
            _vm.details = _list;
            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',52,GETDATE())";

                int rowsinsert = _callDb.Database.ExecuteSqlCommand(query);
            }
            return View(_vm);
        }

        [HttpPost]
        public ActionResult Reassessment(ReassessmentCallsVM _vm)
        {

            IList<ReassmentCalls> _list = new List<ReassmentCalls>();

            _list = SMOutbound.getOutboundReassessmentCalls(_vm.StartDate, _vm.EndDate);
            _vm.details = _list;
            return View(_vm);
        }


        public ActionResult SmartActionVictorCalls()
        {
            SAVictorCalls _list = new SAVictorCalls();
            IList<SACallsVM> _vm = new List<SACallsVM>();
            _vm = SMOutbound.getSAVictorCalls(User.Identity.Name.Split('\\').Last().ToLower());
            _list.victorCalls = _vm;
            return View(_list);
        }

        public ActionResult SAVictorCallsWithOnlyLancetsContolSol()
        {
         

            SAcallCount _vm = new SAcallCount();
            _vm.OrderDate = DateTime.Today.AddDays(1);
          
            return View(_vm);
        }

        [HttpPost]
        public ActionResult SAVictorCallsWithOnlyLancetsContolSol(SAcallCount _vm)
        {
             

            return View(_vm);
        }


        
        [HttpPost]
        public ActionResult OnlyLancetsControlSol(DateTime OrderDate, [DataSourceRequest] DataSourceRequest request)
        {
            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',54,GETDATE())";
                int rowsinsert = _callDb.Database.ExecuteSqlCommand(query);
            }

            return Json(GetOnlyLancetsControlSolList(OrderDate).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public static IList<SACallBackVM> GetOnlyLancetsControlSolList(DateTime OrderDate)
        {
            IList<SACallBackVM> _list = new List<SACallBackVM>();

            
            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {

                
                    string query = "with table1 as( "+
" select c.AccountNumber, c.Id ,count(distinct  p.ProductCategoryId) as proCount from call c "+
" left join ProductConfirmation p on c.Id = p.CallId "+
" where OrderDate ='"+ OrderDate.ToShortDateString() + "'   and p.NeedsProduct = 1 "+
 
" group by c.Id, c.AccountNumber )"+

" select distinct c.Id,t.AccountNumber,c.OrderDate,c.CallStatus,  c.Calltime,c.SpokeWith, c.IsOrderConfirmed from table1 t " +
 " left join ProductConfirmation pc on t.Id = pc.CallId "+
" left join Call c on c.id = pc.CallId "+
" where t.proCount = 1 "+
" and pc.ProductCategoryId in (66,67) and pc.NeedsProduct = 1  " +
" Order by t.AccountNumber";


                    _list = _callDb.Database.SqlQuery<SACallBackVM>(query).ToList<SACallBackVM>();
                

            }
            

           
            return _list;
        }

        public ActionResult ProductListByCallID(int IDNum, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetProdAddListCallID(IDNum).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public static IEnumerable<ProdReq> GetProdAddListCallID(int Callid)
        {
            IList<ProdReq> _list = new List<ProdReq>();

            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {



                string query = " select distinct p.CallId, p.ProductCategoryId, c.CategoryDescription, p.NeedsProduct from ProductConfirmation p " +

    " left join HHSQLDB.dbo.tbl_ProductCategory_Table c " +

   "  on p.ProductCategoryId = c.ID " +

"     where p.CallId = " + Callid + "   ";


                _list = _callDb.Database.SqlQuery<ProdReq>(query).ToList<ProdReq>();


            }
            return _list;
        }

        public ActionResult SmartActionNotConfirmedCalls()
        {
            SAVictorCalls _list = new SAVictorCalls();
            IList<SACallsVM> _vm = new List<SACallsVM>();
            _vm = SMOutbound.getSAVictorNotConfirmedCalls(User.Identity.Name.Split('\\').Last().ToLower());
            _list.victorCalls = _vm;
            return View(_list);
        }


        public ActionResult SANotConfirmedCalls_RequireCallBack()
        {
            DateTime nextdt = DateTime.Today.AddDays(1);
            

            SAcallCount _vm = new SAcallCount();
            _vm.OrderDate = nextdt;
            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {
                _vm.count = (from c in _callDb.Calls
                             where c.OrderDate == nextdt
                             //   && c.IsOrderConfirmed != true
                                && (c.CallStatus.Contains("Reached Human - Requires Callback")
                            || c.IsInFacility == true || c.ReceivesHomeCare == true || c.ReceivesHospiceCare == true
                            || c.HasUpdatedAddress == true)
                             select c.AccountNumber).Distinct().Count();


            }
            return View(_vm);
        }
        [HttpPost]
        public ActionResult SANotConfirmedCalls_RequireCallBack(SAcallCount _vm)
        { 
            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {
                _vm.count = (from c in _callDb.Calls
                             where c.OrderDate == _vm.OrderDate
                            // && c.IsOrderConfirmed != true
                             && (c.CallStatus.Contains("Reached Human - Requires Callback")
                            || c.IsInFacility == true || c.ReceivesHomeCare == true || c.ReceivesHospiceCare == true
                            || c.HasUpdatedAddress == true)
                             select c.AccountNumber).Distinct().Count();


            }
            return View(_vm);
        }

        [HttpPost]
        public ActionResult RequiredCallBackList(DateTime OrderDate, [DataSourceRequest] DataSourceRequest request)
        {
            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',53,GETDATE())";
                int rowsinsert = _callDb.Database.ExecuteSqlCommand(query);
            }

            return Json(GetRequiredCallBackList(OrderDate).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public static IList<SACallBackVM> GetRequiredCallBackList(DateTime OrderDate)
        {
            IList<SACallBackVM> _list = new List<SACallBackVM>();
            //DateTime nextdt = DateTime.Today.AddDays(1);
            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {

                var list = (from c in _callDb.Calls
                            where c.OrderDate == OrderDate
                           // && c.IsOrderConfirmed != true
                            && (c.CallStatus.Contains("Reached Human - Requires Callback")
                            || c.IsInFacility == true || c.ReceivesHomeCare == true || c.ReceivesHospiceCare == true
                            || c.HasUpdatedAddress == true  )
                            select c.AccountNumber).Distinct().ToList();
                
                foreach (var acc in list)
                {
                    var _accounts = (from c in _callDb.Calls
                                     where c.OrderDate == OrderDate
                                     && c.IsOrderConfirmed != true
                                     && c.AccountNumber == acc

                                     select new SACallBackVM
                                     {
                                         id = c.Id,
                                         AccountNumber = c.AccountNumber,
                                         OrderDate = c.OrderDate,
                                         CallStatus = c.CallStatus,
                                         CallTime = c.CallTime,
                                         ISOrderConfirmed = c.IsOrderConfirmed,
                                         SpokeWith = c.SpokeWith,
                                         Reason = c.CallStatus.Contains("Reached Human - Requires Callback") ? "Requires Callback" : c.HasUpdatedAddress == true ?
                                         "Updated Address" : c.IsInFacility == true ? "In Facility" : c.ReceivesHomeCare == true ? "Receive Home Care" :
                                         c.ReceivesHospiceCare == true ? "Receive Hospice Care" : " "

                                     }).ToList();

                    _list = _list.Concat(_accounts).ToList();
                }
            }

            _list = _list.OrderBy(t => t.AccountNumber).ToList();
            return _list;
        }

      
       



    }
}