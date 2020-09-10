using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class AcChanges
    {
        public class AcChangesVM
        {
            [Required]
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The Account Number field is only intergers.")]
            public string AccountNumber { get; set; }

            public IList<AcInformation> AcDetails { get; set; }
            public IList<AcPayer> AcPayerDetails { get; set; }

            public bool isDataExist { get; set; }

        }

        public class AcInformation
        {
            public int Account { get; set; }
            public string Name { get; set; }
            public DateTime? AcctUpdated { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? AcctInfoUpdated { get; set; }
            public string AcctInfoUpdatedBy { get; set; }

        }

        public class AcPayer
        {
            public string Payer { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? Updated { get; set; }

        }

        public static AcChangesVM GetAcChangesData(int acNumber)
        {
            AcChangesVM acChangesVM = new AcChangesVM();
            acChangesVM.AccountNumber = acNumber.ToString();
            AcInformation acInformationData = new AcInformation();
            List<AcInformation> lstAcInformationData = new List<AcInformation>();
            List<AcPayer> lstAcPayerData = new List<AcPayer>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    var acChanges = _db.sp_GetAcChanges(acNumber).ToList();

                    acInformationData = (from item in acChanges
                                         select new AcInformation
                                         {
                                             Account = item.Account,
                                             Name = item.Last_name + ", " + item.First_Name,
                                             AcctUpdated = item.AccountUpdated,
                                             UpdatedBy = item.AccountUpdatedBy,
                                             AcctInfoUpdated = item.AccountInfUpdated,
                                             AcctInfoUpdatedBy = item.AccountInfUpdatedBy
                                         }
                               ).FirstOrDefault();
                    lstAcPayerData = (from item in acChanges
                                      select new AcPayer
                                      {
                                          Payer = item.Payer,
                                          UpdatedBy = item.InsuranceUpdatedBy,
                                          Updated = item.InsuranceUpdated
                                      }
                             ).ToList();
                }
                if (acInformationData != null)
                {
                    lstAcInformationData.Add(acInformationData);
                }
                if(lstAcPayerData.Count>0)
                {
                    acChangesVM.isDataExist = true;
                }
                acChangesVM.AcDetails = lstAcInformationData;
                acChangesVM.AcPayerDetails = lstAcPayerData;
                return acChangesVM;
            }
            catch (Exception ex)
            {
                return new AcChangesVM();
            }
        }
        
    }
}