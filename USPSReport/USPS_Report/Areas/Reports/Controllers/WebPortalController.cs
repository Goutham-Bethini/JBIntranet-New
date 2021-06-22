using Newtonsoft.Json;
using ReportsDatabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class WebPortalController : Controller
    {

        // GET: Reports/WebPortal
        public ActionResult Index(string username, int? Acct, bool? AcctExists, bool? AcctCreated, string msg, bool? IsEmail, bool? IsSumit, bool? decline)
        {

           WebAcctCreatorVm _vm = new WebAcctCreatorVm();
            if (AcctExists == true)
            {
                _vm.webUsername = username;
                _vm.Account = Acct;
                _vm.IsAcctExists = true;
            }
            else if (AcctCreated == true)
            {
                _vm.webUsername = username;
                _vm.Account = Acct;
                _vm.IsAcctCreated = true;
            }
            else if (AcctCreated == false)
            {
                _vm.errorMsg = msg;
                _vm.IsAcctCreated = false;
            }

            if (IsEmail == true)
            {
                _vm.IsEmail = true;
                _vm.MissingEmail = true;
            }
            else
            { _vm.IsEmail = false; }


            _vm.ISActive = true;

            if (IsSumit == true)
                _vm.IsSubmit = true;
            if (decline == true)
                _vm.decline = true;
            return View(_vm);
        }

        [HttpPost]
        public ActionResult Index(WebAcctCreatorVm _vm)
        {
            _vm.IsSubmit = true;
            _vm.firstTime = true;
            bool IsActive = Portal.GetActiveAcct(_vm.Account);
            _vm.ISActive = IsActive;
            if (IsActive == true)
            {

                _vm.details = AddCSRLog.GetDetails(_vm.Account);
            }
            if (_vm.details != null && (_vm.details.Email == null || _vm.details.Email == "" || !_vm.details.Email.Contains("@")))
            {
                _vm.IsEmail = true;
            }
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',33,GETDATE())";

                int rowsinsert = _db.Database.ExecuteSqlCommand(query);
            }

            return View(_vm);
        }
        [HttpPost]
        public ActionResult CreatAcct(WebAcctCreatorVm _vm)
        {
            _vm.webUsername = Portal.GetWebAcct(_vm.Account);
            bool? IsExists = false;
            bool? Created = false;
            string returnMsg = "";

            if ((_vm.webUsername == null || _vm.webUsername == "") && _vm.IsEmail != true)
            {
                webModel Model = new webModel();
                _vm.details = AddCSRLog.GetDetails(_vm.Account);
                Model.acct = _vm.Account;

                // string day = (_vm.details.DOB.Value.Day.ToString().Length < 2) ? "0" + _vm.details.DOB.Value.Day.ToString() : _vm.details.DOB.Value.Day.ToString();

                // string month = (_vm.details.DOB.Value.Month.ToString().Length < 2) ? "0" + _vm.details.DOB.Value.Month.ToString() : _vm.details.DOB.Value.Month.ToString();

                string lname = _vm.details.lastName.Count()>3 ? _vm.details.lastName.Substring(0,3) : _vm.details.lastName;

                string day = (DateTime.Today.Day.ToString().Length < 2) ? "0" + DateTime.Today.Day.ToString() : DateTime.Today.Day.ToString();

                string month = (DateTime.Today.Month.ToString().Length < 2) ? "0" + DateTime.Today.Month.ToString() : DateTime.Today.Month.ToString();

                //Model.Username = string.IsNullOrWhiteSpace(_vm.details.firstName) ? "" : _vm.details.firstName[0] + _vm.details.lastName + "@" + month + day + _vm.details.DOB.Value.Year.ToString();
                //  Model.password = string.IsNullOrWhiteSpace(_vm.details.firstName) ? "" : _vm.details.firstName[0] + _vm.details.lastName + "@" + month + day + _vm.details.DOB.Value.Year.ToString();


                // Model.Username = string.IsNullOrWhiteSpace(_vm.details.firstName) ? "" : _vm.details.firstName[0] + lname + "@" + month + day + DateTime.Today.Year.ToString();
                // Model.password = string.IsNullOrWhiteSpace(_vm.details.firstName) ? "" : _vm.details.firstName[0] + lname + "@" + month + day + DateTime.Today.Year.ToString();

                Model.Username =   _vm.details.Account2+"@"+_vm.details.firstName[0] + lname ;
                Model.password = _vm.details.Account2 + "@" + _vm.details.firstName[0] + lname ;



                // Model.Username= Model.Username.Replace(" ", "");
                //   Model.password= Model.password.Replace(" ", "");

                Model.Username= Regex.Replace(Model.Username, @"\s", "");

                Model.password = Regex.Replace(Model.password, @"\s", "");

                Model.Email = _vm.details.Email;
                returnMsg = WebAcctCreator(Model);
                if (returnMsg.Contains("Success"))
                {
                    Created = true;
                    Portal.WelcomeEmailforInteractiveWebSite(_vm.details.Email, Model.Username, _vm.details.firstName);
                    Portal.AddNoteforNewWebAcct(_vm.Account, Model.Username);
                }
            }

            else
            {
                if (_vm.IsEmail != true)
                {
                    IsExists = true;
                }
            }
            _vm.webUsername = Portal.GetWebAcct(_vm.Account);

            return RedirectToAction("Index", new { username = _vm.webUsername, Acct = _vm.Account, AcctExists = IsExists, AcctCreated = Created, msg = returnMsg, IsEmail = _vm.IsEmail, IsSumit = true });

            // return View(_vm);
        }

      
        public ActionResult DeclineWebAcc(int? Acct, string Email)
        {
            Portal.AddDecliningNoteforNewWebAcct(Acct);
            if (Email != null && Email != "" && Email.Contains("@"))
            {
                Portal.DeclineEmailforInteractiveWebSite(Email);
            }
            return RedirectToAction("Index", new { decline = true });
        }

        private class webModel
        {
            public int? acct { get; set; }
            public string Username { get; set; }
            public string password { get; set; }
            public string Email { get; set; }
        }
        private static string WebAcctCreator(webModel _vm)
        {
            HttpClient client = new HttpClient();
            string _value = ""; ;
            string err = ""; ;
            try
            {
                WebClient _web = new WebClient();
               //  client.BaseAddress = new Uri("http://localhost:6415/");
                client.BaseAddress = new Uri("https://portal.jandbmedical.com/");
                var content = JsonConvert.SerializeObject(_vm);

                var result2 = client.PostAsync("api/NewAccSetup?operatorId=925&password=WebAccount", new StringContent(content, Encoding.UTF8, "application/json")).Result;


                //   var ser = JsonConvert.SerializeObject(typeof(CoInsDetail)); 

                using (var stm1 = result2.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new StreamReader(stm1.Result))
                    {
                        _value = reader.ReadToEnd();

                    }
                }


            }
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return _value + err;

        }
    }
}