//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportsDatabase
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Referral_Source_Table
    {
        public int ID { get; set; }
        public string Type_Of_Referral { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Degree { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Office_Phone { get; set; }
        public string Home_Phone { get; set; }
        public string Fax_Number { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Internet { get; set; }
        public string License_Number { get; set; }
        public string UPIN { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> ID_Salesperson { get; set; }
        public bool IsAnOxygenFacility { get; set; }
        public string ExternalReference { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastChange { get; set; }
        public Nullable<int> ID_ChangedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> ID_DeletedBy { get; set; }
        public Nullable<short> Discontinued { get; set; }
        public string NPI { get; set; }
        public string Specialty { get; set; }
        public string Pager_Number { get; set; }
        public Nullable<short> IsClaimsAdjuster { get; set; }
        public string DEA { get; set; }
        public Nullable<short> IsCaseManager { get; set; }
        public Nullable<int> ID_RelatedToReferral { get; set; }
        public Nullable<int> ID_RelatedToEmployer { get; set; }
        public string ContactTitle { get; set; }
        public Nullable<int> ID_RelatedToPayer { get; set; }
        public Nullable<System.DateTime> License_ExpirationDate { get; set; }
        public string Taxonomy { get; set; }
        public Nullable<int> ID_ReferralCategory { get; set; }
        public Nullable<short> DoctorInPECOS { get; set; }
        public Nullable<short> CMNMethod { get; set; }
        public Nullable<int> ID_CreatedBy { get; set; }
        public string Middle_Name { get; set; }
        public Nullable<System.DateTime> Middle_Initial { get; set; }
        public Nullable<short> MidLevelPhyType { get; set; }
        public Nullable<int> ID_DoctorSpecialty { get; set; }
    }
}
