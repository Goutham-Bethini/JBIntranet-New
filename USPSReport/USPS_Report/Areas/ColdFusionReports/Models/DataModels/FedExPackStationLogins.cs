using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class FedExPackStationLogins
    {
        public class FedExPackStationLoginsVM
        {
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string UserName { get; set; }
            public string ResetPassword { get; set; }
            public string ResetPasswordCheck { get; set; }
            public bool NeedToDeleteLogin { get; set; }            
            public IList<FedExPackStationLoginsData> Details { get; set; }
            public string Message { get; set; }
        }
        public class FedExPackStationLoginsData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string UserName { get; set; }
            public string HasPassword { get; set; }
            public DateTime? Updated { get; set; }
            public string UpdatedBy { get; set; }

        }

        public static IList<FedExPackStationLoginsData> GetLogins()
        {
            List<FedExPackStationLoginsData> lstFedExPackStationLoginsData = new List<FedExPackStationLoginsData>();
            using (USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities())
            {
                lstFedExPackStationLoginsData = _db.FedExLogins.Where(i => i.felDeleted == null).Select(i =>
                          new FedExPackStationLoginsData
                          {
                              Id = i.felID,
                              Name = i.felName,
                              UserName = i.felUser,
                              HasPassword = i.felPass != null ? "Yes" : "No",
                              Updated = i.felUpdated,
                              UpdatedBy = i.felUpdatedBy
                          }).ToList();
            }
            return lstFedExPackStationLoginsData;
        }

        public static FedExPackStationLoginsVM AddOrUpdate(FedExPackStationLoginsVM fedExPackStationLoginsVM,string currentUser)
        {
            FedExPackStationLoginsVM locfedExPackStationLoginsVM = new FedExPackStationLoginsVM();
            using (USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities())
            {
                
                if (fedExPackStationLoginsVM.Id==0)
                {
                    var res = _db.FedExLogins.Where(i => i.felUser == fedExPackStationLoginsVM.UserName).FirstOrDefault();
                    if (res != null)
                    {
                        locfedExPackStationLoginsVM.Message = "ERROR - Username Already Exists";
                    }
                    else
                    {
                        FedExLogin fedExLogin = new FedExLogin();
                        fedExLogin.felName = fedExPackStationLoginsVM.Name;
                        fedExLogin.felUser = fedExPackStationLoginsVM.UserName;
                        fedExLogin.felPass = fedExPackStationLoginsVM.ResetPassword;
                        fedExLogin.felAdded = DateTime.Now;
                        fedExLogin.felAddedBy = currentUser;
                        _db.FedExLogins.Add(fedExLogin);
                        _db.SaveChanges();
                        locfedExPackStationLoginsVM.Message = "User added successfully";
                    }
                }
                else
                {
                    var res = _db.FedExLogins.Where(i => i.felID == fedExPackStationLoginsVM.Id).FirstOrDefault();
                    if (res != null)
                    {                       
                        if(fedExPackStationLoginsVM.NeedToDeleteLogin)
                        {
                            res.felDeleted = DateTime.Now;
                            res.felDeletedBy = currentUser;
                            locfedExPackStationLoginsVM.Message = "User deleted successfully";
                        }
                        else
                        {
                            res.felName = fedExPackStationLoginsVM.Name;
                            res.felUser = fedExPackStationLoginsVM.UserName;
                            res.felPass = fedExPackStationLoginsVM.ResetPassword;
                            res.felUpdated = DateTime.Now;
                            res.felUpdatedBy = currentUser;
                            locfedExPackStationLoginsVM.Message = "User updated successfully";
                        }                 
                        _db.SaveChanges();                        
                    }
                }               
            }
            locfedExPackStationLoginsVM.Details= GetLogins();
            return locfedExPackStationLoginsVM;
        }
    }
}