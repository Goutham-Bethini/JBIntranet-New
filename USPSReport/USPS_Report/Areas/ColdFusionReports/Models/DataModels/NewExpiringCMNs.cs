using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class NewExpiringCMNs
    {
        public class NewExpiringCMNsVM
        {
            [Required]
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The Month field is only intergers.")]
            public string Month { get; set; }
            [Required]
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The Year field is only intergers.")]
            public string Year { get; set; }

            public IList<NewExpiringCMNsData> Details { get; set; }

        }
        public class NewExpiringCMNsData
        {
            public int? Account { get; set; }
            public string AlphaSplit { get; set; }
            public string PatientName { get; set; }
            public string ProductLine { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public string DueDate { get; set; }
            public string Attempt1st { get; set; }
            public string Attempt2nd { get; set; }
            public string Attempt3rd { get; set; }
            public string AdvActionLetter { get; set; }
            public string UnableToSvcLetter { get; set; }
            public string Completed { get; set; }
            public string NotNeeded { get; set; }
            public string ComplianceCheck { get; set; }
            public string DateCMNEntered { get; set; }
        }

        public static IList<NewExpiringCMNsData> GetNewExpiringCMNs(int month, int year)
        {
            List<NewExpiringCMNsData> lstNewExpiringCMNs = new List<NewExpiringCMNsData>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstNewExpiringCMNs = (from item in _db.sp_GetNewExpiringCMNs(month, year)
                                          select new NewExpiringCMNsData
                                          {
                                              Account = item.Account,
                                              AlphaSplit = item.Alpha_Split,
                                              PatientName = item.Patient_Name,
                                              ProductLine = item.Product_Line,
                                              ExpirationDate = item.Expiration_Date,
                                              DueDate = item.Due_Date,
                                              Attempt1st = item.C1st_Attempt,
                                              Attempt2nd = item.C2nd_Attempt,
                                              Attempt3rd = item.C3rd_Attempt,
                                              AdvActionLetter = item.Adv_Action_Letter_,
                                              UnableToSvcLetter = item.Unable_to_Svc_Letter_,
                                              Completed = item.Completed_,
                                              NotNeeded = item.Not_Needed_,
                                              ComplianceCheck = item.Compliance_Check_,
                                              DateCMNEntered = item.Date_CMN_Entered
                                          }
                               ).ToList();
                }
                return lstNewExpiringCMNs;
            }
            catch (Exception ex)
            {
                return new List<NewExpiringCMNsData>();
            }
        }
    }
}