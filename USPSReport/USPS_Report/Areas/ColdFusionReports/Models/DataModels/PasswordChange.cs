using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class PasswordChange
    {
        public class PasswordChangeVM
        {
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
            public string Message { get; set; }
        }
        public static PasswordChangeVM AddOrUpdate(PasswordChangeVM passwordChangeVM, string currentUser)
        {
            PasswordChangeVM locPasswordChangeVM = new PasswordChangeVM();
            using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionIntranetEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionIntranetEntities())
            {                
                var res = _db.FedEx_Password.Where(i => i.logInId == currentUser).FirstOrDefault();
                if (res != null)
                {
                    res.pwd = passwordChangeVM.NewPassword;
                    res.lastModDt = DateTime.Now;
                    locPasswordChangeVM.Message = "Password Reset Successful!";
                }
                else
                {
                    FedEx_Password fedEx_Password = new FedEx_Password();
                    fedEx_Password.logInId = currentUser;
                    fedEx_Password.pwd = passwordChangeVM.NewPassword;
                    fedEx_Password.lastModDt = DateTime.Now;
                    _db.FedEx_Password.Add(fedEx_Password);                    
                    locPasswordChangeVM.Message = "Password Reset Successful!";
                }
                _db.SaveChanges();               
            }            
            return locPasswordChangeVM;
        }
    }
}