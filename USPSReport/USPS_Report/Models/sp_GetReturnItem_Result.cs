//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace USPS_Report.Models
{
    using System;
    
    public partial class sp_GetReturnItem_Result
    {
        public int Return_ID { get; set; }
        public Nullable<int> Account { get; set; }
        public Nullable<int> Account_Member { get; set; }
        public Nullable<int> WorkOrder_ID { get; set; }
        public string Reshipped { get; set; }
        public Nullable<System.DateTime> Date_Returned { get; set; }
        public string Tag_Type { get; set; }
        public string Return_Note { get; set; }
        public Nullable<int> Reason__List_Option_ID { get; set; }
        public string Return_Other_Reason { get; set; }
        public string Tracking_Number { get; set; }
        public Nullable<short> Send_To_Billing { get; set; }
        public Nullable<short> Dont_Display { get; set; }
        public Nullable<int> Boxes_Returned { get; set; }
        public Nullable<int> OracleRMA { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public Nullable<System.DateTime> Request_Date { get; set; }
    }
}
