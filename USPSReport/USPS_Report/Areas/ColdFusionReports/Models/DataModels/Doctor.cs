using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class Doctor
    {
        public class DoctorVM
        {
            [Required]
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The Physician ID field is only intergers.")]
            public string PhysicianID { get; set; }

            public IList<DoctorData> Details { get; set; }

        }
        public class DoctorData
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? Created { get; set; }
            public string ChangedBy { get; set; }
            public DateTime? LastChanged { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? Deleted { get; set; }

        }
        public static IList<DoctorData> GetDoctorData(int id)
        {
            List<DoctorData> lstDoctorData = new List<DoctorData>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstDoctorData = (from item in _db.sp_GetDoctorInfo(id)
                                   select new DoctorData
                                   {
                                       Id = item.ID,
                                       FirstName = item.First_Name,
                                       LastName = item.Last_Name,
                                       Created = item.CreateDate,
                                       ChangedBy = item.LastChangedBy,
                                       LastChanged = item.LastChange,
                                       DeletedBy = item.LegalName,
                                       Deleted = item.DeletedDate,
                                   }
                               ).ToList();
                }
                return lstDoctorData;
            }
            catch (Exception ex)
            {
                return new List<DoctorData>();
            }
        }
    }
}