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
    
    public partial class tbl_DeliveryLocation_Table
    {
        public int ID { get; set; }
        public string DeliveryLocationName { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone_Number { get; set; }
        public string Fax_Number { get; set; }
        public string Contact { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> NonRetailDeliveryLocation { get; set; }
        public Nullable<int> ID_ReturnProductDeliveryLocation { get; set; }
        public Nullable<int> ID_Provider { get; set; }
        public Nullable<int> ID_LocationSupportedBy { get; set; }
        public Nullable<int> ID_OperatorBuyer { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> ID_DeletedBy { get; set; }
        public Nullable<int> CashAccountNumber { get; set; }
        public string ExternalReference { get; set; }
        public string County { get; set; }
        public string CCMerchantAccountNumber { get; set; }
        public string CCProcessorCode { get; set; }
        public string CCProcessingSoftwareComputer { get; set; }
        public string CCProcessingSoftwarePath { get; set; }
        public string CCProcessingSoftwareIP { get; set; }
        public string CCProcessingSoftwarePort { get; set; }
        public Nullable<short> AutoCreatePOforAllProducts { get; set; }
        public string Email { get; set; }
        public Nullable<short> SuppressFromQOHSearch { get; set; }
        public string CCMerchantKey { get; set; }
        public string CCProcessingSoftwareClientID { get; set; }
        public string CCProcessingSoftwareURL { get; set; }
        public Nullable<System.DateTime> LastChange { get; set; }
        public Nullable<int> ID_ChangedBy { get; set; }
        public Nullable<short> RequireDayClose { get; set; }
        public Nullable<int> ID_OperatorConfirmation { get; set; }
        public string AP_InterfaceID { get; set; }
        public string AP_ReqLocation { get; set; }
        public string AP_Requestor { get; set; }
        public string AP_FileName { get; set; }
        public Nullable<short> SuppressFromBatchCreatePO { get; set; }
        public string DelvCompl_LocationToken { get; set; }
        public Nullable<short> MobileDeliveryLocation { get; set; }
        public string ElementAccountID { get; set; }
        public string ElementAccountToken { get; set; }
        public string ElementApplicationID { get; set; }
        public string ElementAcceptorID { get; set; }
        public Nullable<short> ExcludeFromRFwithQOH { get; set; }
        public string DelvCompl_CompanyAccessToken { get; set; }
        public Nullable<short> PharmacyDeliveryLocation { get; set; }
        public Nullable<int> ID_PharmacySuppliesDeliveryLocation { get; set; }
    }
}
