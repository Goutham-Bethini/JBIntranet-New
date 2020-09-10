using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Models;
using ReportsDatabase;
using System.Data.Entity;
using USPS_Report.Areas.Reports.Models;
using System.Net.Mail;
using System.Text;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class PumpTrackerController : Controller
    {
        // GET: Reports/PumpTracker
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomerAccountList([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetCustomerAcctList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<PumpTrackerVM> GetCustomerAcctList()

        {
            IList<PumpTrackerVM> _AccList = new List<PumpTrackerVM>();

            using (IntranetEntities _DB = new IntranetEntities())
            {
         

                _AccList = _DB.Database.SqlQuery<PumpTrackerVM>("  with table1 as ( select Account, max(id) as id, min(CreatedOn) as Created, "+
 " max(CreatedOn) as Modified from tbl_BCNCallLog bcn group by Account ) "+

                    "select bcn.id,bcn.Account,t.Created, t.Modified, Manufacturer, OrderStatus, TypeSupplies_1 as Supplies1 , TypeSupplies_2 as Supplies2, TypeSuppliesOther as SuppliesOther, " +
   " Assmnt_manufacturerCGM as Model, Assmnt_neworreplacementCGM as NewReplacement, Assmnt_receiver_ProductCode as ReceiverProductCode, Assmnt_receiver_serialnoCGM as ReceiverSerial, "+
 " Assmnt_transmitter_ProductCode as TransmitterProductCode, Assmnt_transmitter_serialnoCGM as TransmitterSerial, " +
 "  SentReqPurchasing as SentRequestPurchasing, getdate() as ShipDate, DocumentationTxt as InNeedOf, "+
   " OrderStatusTxt as AdditionalInformation from tbl_BCNCallLog bcn "+
   " join table1 t on bcn.id = t.id").ToList<PumpTrackerVM>();

            }
            return _AccList;
        }

      

        public ActionResult AccountDetailByAccount(int AccountNum, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetAccDetails(AccountNum).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<OrderDetail> GetAccDetails(int _acct)
        {
            IList<OrderDetail> _addReq = new List<OrderDetail>();

            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _addReq = (from t in _db.tbl_Account_Member
                           join r in _db.tbl_Referral_Source_Table on t.ID_Default_Referring_Doctor equals r.ID
                           join i in _db.v__AccountMemberEffectiveInsurance_Ins1 on t.Account equals i.Account
                           join p in _db.tbl_Payer_Table on i.ID_Payer equals p.ID
                           join pt in _db.tbl_Name_PayerTypes on p.ID_PayerType equals pt.ID
                           where t.Account == _acct && t.Member == 1
                           select new OrderDetail
                           {
                               Account = t.Account,
                               First_Name = t.First_Name,
                               Last_Name = t.Last_Name,
                               Middle = t.Middle,
                               Gender = t.Sex,
                               EmailAddress = t.EmailAddress,
                               Phone = t.Phone,
                               BirthDate = t.BirthDate,
                               Address = t.Address_1 + " " + t.Address_2,
                               City = t.City,
                               State = t.State,
                               Zip = t.Zip,
                               PrimaryIns = i.PayerName,
                               InsType = pt.Title,
                               PhysicianFN = r.First_Name,
                               PhysicianLN = r.Last_Name,
                               PhysicianAddress = r.Address_1 + " " + r.Address_2,
                               PhysicianCity = r.City,
                               PhysicianState = r.State,
                               PhysicianZip = r.Zip,
                               PhysicianNPI = r.NPI

                            }
                           ).Take(1).ToList();



            }

           

            return _addReq;
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AccountDetailUpdate([DataSourceRequest] DataSourceRequest request, PumpTrackerVM _rec)
        {
           
            if (_rec != null && ModelState.IsValid)
            {
                // Inbound.AddNoteAddressChange(_rec);
                using (IntranetEntities _DB = new IntranetEntities())
                {

                    //var _data = _callDB.InboundChangeAddresses.Where(t => (t.UpdateTime == _rec.UpdateTime && t.Account == _rec.Account)).OrderByDescending(t=>t.UpdateTime).Take(1);
                    var _data = (from t in _DB.tbl_BCNCallLog
                                 where t.id == _rec.id 
                                 select t).Take(1).SingleOrDefault();

                    _data.Manufacturer = _rec.Manufacturer;
                    _data.OrderStatus = _rec.OrderStatus;
                    _data.TypeSupplies_1 = _rec.Supplies1;
                    _data.TypeSupplies_2 = _rec.Supplies2;
                    _data.TypeSuppliesOther = _rec.SuppliesOther;
                   
                   

                        _DB.Entry(_data).State = EntityState.Modified;
                     




                    try
                    {
                        _DB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }

               
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }

    }
}