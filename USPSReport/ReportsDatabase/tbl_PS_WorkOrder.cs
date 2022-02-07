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
    
    public partial class tbl_PS_WorkOrder
    {
        public int ID { get; set; }
        public Nullable<int> Account { get; set; }
        public Nullable<short> Member { get; set; }
        public int ID_PlanOfService { get; set; }
        public Nullable<int> ID_PrimaryAssignedUser { get; set; }
        public Nullable<System.DateTime> DateMovedToUser { get; set; }
        public bool AcceptAssignment { get; set; }
        public string ShipToCareOf { get; set; }
        public string ShipToAddress_1 { get; set; }
        public string ShipToAddress_2 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToPhone { get; set; }
        public string ShipToCounty { get; set; }
        public string DeliveryInstructions { get; set; }
        public Nullable<int> ID_DeliveryLocation { get; set; }
        public Nullable<int> ID_DeliveryMethod { get; set; }
        public Nullable<System.DateTime> Request_Date { get; set; }
        public Nullable<int> ID_RequestTime { get; set; }
        public Nullable<System.DateTime> Schedule_Date { get; set; }
        public Nullable<int> ID_ScheduleTime { get; set; }
        public bool UrgentDelivery { get; set; }
        public Nullable<int> ID_PurchaseOrder { get; set; }
        public Nullable<System.DateTime> Completed_Date { get; set; }
        public string Completed_By { get; set; }
        public Nullable<int> Completed_User { get; set; }
        public string ConfirmationNumber { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public Nullable<System.DateTime> PostedToBilling_Date { get; set; }
        public string StatusComments { get; set; }
        public string EDIMessage { get; set; }
        public Nullable<short> HoldFromShipping { get; set; }
        public string HoldFromShippingReason { get; set; }
        public Nullable<short> RCDontTransmit { get; set; }
        public Nullable<decimal> Cash_PaidAmount { get; set; }
        public Nullable<short> Cash_PaymentMethod { get; set; }
        public string Cash_CheckNumber { get; set; }
        public Nullable<System.DateTime> Cash_CheckDate { get; set; }
        public Nullable<decimal> Cash_AdjustmentAmount { get; set; }
        public Nullable<int> Cash_AdjustmentReasonID { get; set; }
        public string SID { get; set; }
        public Nullable<int> ID_OrigWOFromBO { get; set; }
        public Nullable<decimal> Cash_TotalMoneyTendered { get; set; }
        public Nullable<int> Cash_ID_AccountCreditCard { get; set; }
        public string Cash_CreditCardApprovalNumber { get; set; }
        public Nullable<short> Cash_CreditCardInputType { get; set; }
        public Nullable<short> Cash_PaymentMethod2 { get; set; }
        public Nullable<decimal> Cash_PaidAmount2 { get; set; }
        public Nullable<int> Cash_ID_AccountCreditCard2 { get; set; }
        public string Cash_CreditCardApprovalNumber2 { get; set; }
        public Nullable<short> Cash_CreditCardInputType2 { get; set; }
        public Nullable<System.DateTime> Cash_ConfirmedAndPosted { get; set; }
        public string Cash_TransactionRouteID { get; set; }
        public string Cash_TransactionRouteID2 { get; set; }
        public byte[] Cash_Img_AuthSignature { get; set; }
        public byte[] Cash_Img_AuthSignature2 { get; set; }
        public byte[] ProofOfDeliverySignature_Img { get; set; }
        public string ShippingRecordID { get; set; }
        public Nullable<System.DateTime> LastPrintDate { get; set; }
        public Nullable<int> ID_LastPrint_User { get; set; }
        public Nullable<System.DateTime> Last_Updated_Date { get; set; }
        public string Last_Updated_By { get; set; }
        public Nullable<int> Last_Update_User { get; set; }
        public Nullable<System.DateTime> Cancel_Date { get; set; }
        public string Cancel_By { get; set; }
        public Nullable<int> Cancel_User { get; set; }
        public string Cancel_Note { get; set; }
        public Nullable<int> Tag { get; set; }
        public Nullable<System.DateTime> DeliveredConfirmed_Date { get; set; }
        public Nullable<System.DateTime> RxDrug_MovedToFill_Date { get; set; }
        public Nullable<System.DateTime> RxDrug_Fill_Date { get; set; }
        public Nullable<int> RxDrug_Fill_Verified_By { get; set; }
        public Nullable<System.DateTime> RxDrug_LabelPrint_Date { get; set; }
        public Nullable<System.DateTime> RxDrug_Interaction_Date { get; set; }
        public Nullable<System.DateTime> RxDrug_Adjudicated_Date { get; set; }
        public Nullable<short> NumberOfTimesPrinted { get; set; }
        public Nullable<int> RxDrug_Fill_ID_Tech { get; set; }
        public Nullable<short> RxDrug_Adjudicated_ResultCode { get; set; }
        public string RxDrug_Fill_ContainerRef { get; set; }
        public Nullable<System.DateTime> RxDrug_RPh_DispenseLogConfirmDate { get; set; }
        public Nullable<int> ID_DeliveredDriver { get; set; }
        public string RxDrug_Fill_CompleteComment { get; set; }
        public Nullable<int> RxDrug_ID_RPh_Approval { get; set; }
        public Nullable<System.DateTime> RxDrug_RPh_ApprovalDate { get; set; }
        public Nullable<int> RxDrug_RPh_Approval_ID_User { get; set; }
        public Nullable<System.DateTime> RxDrug_MovedToPrintStage_Date { get; set; }
        public Nullable<System.DateTime> RxDrug_CompoundPrint_Date { get; set; }
        public Nullable<System.DateTime> PickPack_Ticket_PrintDate { get; set; }
        public Nullable<System.DateTime> PickPack_MoveTo_Date { get; set; }
        public Nullable<System.DateTime> PickPack_Complete_Date { get; set; }
        public string PickPack_CompleteComment { get; set; }
        public Nullable<int> PickPack_ID_User { get; set; }
        public Nullable<int> ID_Est { get; set; }
        public string DeliveredComment { get; set; }
        public Nullable<short> DeliveredSignatureRel { get; set; }
        public string DeliveredSignatureReason { get; set; }
        public short DeliveredMobileDriver { get; set; }
        public Nullable<System.DateTime> EDIGatherDateTime { get; set; }
        public string EDIGatherControlNumber { get; set; }
        public Nullable<System.DateTime> ConfirmationDateTime { get; set; }
        public Nullable<int> NWA_OrderID { get; set; }
        public string Cash_TicketNumber { get; set; }
        public string Cash_TicketNumber2 { get; set; }
        public string Cash_AcquirerData { get; set; }
        public string Cash_AcquirerData2 { get; set; }
        public Nullable<System.DateTime> RxDrug_MonographPrint_Date { get; set; }
        public Nullable<short> EDIShipMethod { get; set; }
        public Nullable<bool> EDISignatureRequired { get; set; }
        public string Cash_CCPreAuthApprovalNumber { get; set; }
        public string Cash_CCPreAuthTransactionRouteID { get; set; }
        public Nullable<System.DateTime> Cash_CCPreAuth_Date { get; set; }
        public Nullable<decimal> Cash_CCPreAuth_Amount { get; set; }
        public string UrgentDeliveryTimeFrame { get; set; }
        public Nullable<int> ID_DeliveryLocation_SoldFrom { get; set; }
        public Nullable<int> ID_DeliveryLocation_DelvFrom { get; set; }
        public Nullable<int> ID_DeliveryZone { get; set; }
        public string DeliveryRouteNumber { get; set; }
        public string DeliveryStopNumber { get; set; }
        public Nullable<System.DateTime> Staged_Date { get; set; }
        public Nullable<int> ID_Staged_By { get; set; }
        public Nullable<System.DateTime> AssignedToWhse_Date { get; set; }
        public Nullable<int> ID_AssignedToWhse_By { get; set; }
        public Nullable<System.DateTime> DeliveryRouteAssigned_Date { get; set; }
        public Nullable<int> ID_DeliveryRouteAssigned_By { get; set; }
    }
}
