using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{
    public class SmartActionReport
    {
        public static IList<SmartActionNotesHoldingOrders> GetSmartActionNotesHoldingOrders()
        {

            try
            {
                IList<SmartActionNotesHoldingOrders> _list = new List<SmartActionNotesHoldingOrders>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {


                    _list = _db.Database.SqlQuery<SmartActionNotesHoldingOrders>("SELECT "+
       " acn.ID,"+
       " acn.Account, "+
       " mem.First_Name, "+
       " mem.Last_Name, "+ 
       " acn.NoteHeading,  "+
       " ach.NoteText, "+
       " max(ach.NoteDate) as 'Latest_Note' "+
    " FROM  "+
         " HHSQLDB..tbl_Account_Note AS acn "+
         " JOIN HHSQLDB..tbl_Account_Note_History AS ach ON ach.ID_Note = acn.ID "+ 
        " JOIN tbl_operator_table opp ON opp.id = acn.NoteCreatedBy "+
        " JOIN dbo.tbl_PS_WorkOrder wo on wo.account = acn.account "+
        " JOIN tbl_account_member mem on mem.account = acn.account "+
               " and mem.member = acn.member "+
 " WHERE  "+
         " acn.NoteCreatedBy = 571 "+
       " AND(ach.NoteDate between GETDATE() - 10 and getdate()) " +
       "  and wo.HoldFromShipping = 1 "  +
        " and wo.cancel_date is null " +
        " and wo.completed_date is null " +
 " GROUP BY acn.ID, acn.Account, mem.First_Name, " + 
     "   mem.Last_Name, acn.NoteHeading, ach.NoteText, ach.NoteDate " +
"  ORDER BY ach.NoteText, ach.NoteDate DESC").ToList<SmartActionNotesHoldingOrders>();


                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<SmartActionNotesHoldingOrders>();
            }


        }

        public static IList<SmartActionPayers> GetSmartActionPayer()
        {
          
            try
            {
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {

               
                var _list = (from t1 in _db.JBCCServicePayers
                             from t2 in _db.JBCCServiceTypes.Where(st => st.Id == t1.ServiceTypeID).DefaultIfEmpty()
                             from t3 in _db.tbl_Payer_Table.Where(p => p.ID == t1.PayerID).DefaultIfEmpty()
                             from t4 in _db.JBCCServiceSmartActionProductsToConfirm.Where(a => a.PayerId == t1.PayerID).DefaultIfEmpty()
                             from t5 in _db.JBCCServiceProductLine.Where(j => j.Id == t4.ProductLineId).DefaultIfEmpty()
                             where t3.Discontinued ==  false && t5.Name != null && t1.DeletedBy == 0
                             select new SmartActionPayers
                             {
                                 PayerID = t1.PayerID,
                                 Name = t3.Name,
                                 ProdLine = t5.Name,
                                 ServiceType = t2.ServiceType
                             }
                ).OrderBy(t=>t.PayerID).ToList<SmartActionPayers>();


                    return _list;
                
            }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<SmartActionPayers>();
            }


        }

        public static IList<ConfirmedHoldingOrders> GetConfirmedHoldingOrders()
        {

            try
            {
                IList<ConfirmedHoldingOrders> _list = new List<ConfirmedHoldingOrders>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    _list = _db.Database.SqlQuery<ConfirmedHoldingOrders>("SELECT  "+
       " t1.AccountNumber, "+
       " t1.OrderDate, "+
       " t1.LoadedDate as DateConfirmed, "+
       " t2.ID as WorkOrder, "+
       " t2.Request_Date,  "+
      "  t2.HoldFromShippingReason  "+
   " FROM  "+
                  "  [CallAgentDB].[dbo].[OrderConfirmation] t1  "+
       " LEFT JOIN[HHSQLDB].[dbo].[tbl_PS_WorkOrder]      t2  ON  t1.AccountNumber = t2.Account  "+
                                                              "  AND t1.OrderDate = t2.Request_Date  "+
   " WHERE  "+
          "  t2.HoldFromShipping = 1  "+
      "  AND t2.Cancel_Date IS NULL  "+
       " AND t2.Completed_Date IS NULL  "+
       " AND t1.IsOrderConfirmed = 1 "+
       " AND t2.HoldFromShippingReason IS NOT NULL "+
       " AND(t1.OrderDate - t1.LoadedDate) < 7 "+
         " 	ORDER BY " +
       " t1.AccountNumber, t1.OrderDate, " +
       " t1.LoadedDate, t2.ID, " +
       " t2.Request_Date, t2.HoldFromShippingReason ").ToList<ConfirmedHoldingOrders>();


                    return _list;
                

                       

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<ConfirmedHoldingOrders>();
            }


        }

        public static IList<UnConfirmedHoldingOrders> GetUnConfirmedHoldingOrders()
        {

            try
            {
                IList<UnConfirmedHoldingOrders> _list = new List<UnConfirmedHoldingOrders>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    _list = _db.Database.SqlQuery<UnConfirmedHoldingOrders>("SELECT  " +
       " t1.AccountNumber, " +
       " t1.OrderDate, " +
       " t1.LoadedDate as DateConfirmed, " +
       " t2.ID as WorkOrder, " +
       " t2.Request_Date,  " +
      "  t2.HoldFromShippingReason  " +
   " FROM  " +
                  "  [CallAgentDB].[dbo].[OrderConfirmation] t1  " +
       " LEFT JOIN[HHSQLDB].[dbo].[tbl_PS_WorkOrder]      t2  ON  t1.AccountNumber = t2.Account  " +
                                                              "  AND t1.OrderDate = t2.Request_Date  " +
   " WHERE  " +
          "  t2.HoldFromShipping = 1  " +
      "  AND t2.Cancel_Date IS NULL  " +
       " AND t2.Completed_Date IS NULL  " +
       " AND t1.IsOrderConfirmed = 0 " +
       " AND t2.HoldFromShippingReason IS NOT NULL " +
       " AND(t1.OrderDate - t1.LoadedDate) < 7 " +
       " 	ORDER BY "+
       " t1.AccountNumber, t1.OrderDate, "+
       " t1.LoadedDate, t2.ID, "+
       " t2.Request_Date, t2.HoldFromShippingReason ").ToList<UnConfirmedHoldingOrders>();


                    return _list;




                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<UnConfirmedHoldingOrders>();
            }


        }


    }
    public class SmartActionNotesHoldingOrders
    {
        public int ID { get; set; }
        public int? Account { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string NoteHeading { get; set; }

        public string NoteText { get; set; }

        public DateTime? Latest_Note { get; set; }

    }

    public class SmartActionNoteHoldingVm
    {
      public  IList<SmartActionNotesHoldingOrders> smartActionNotesHoldingOrders { get; set; }
    }

    public class SmartActionPayers
    {
        public int PayerID { get; set; }
        public string ServiceType { get; set; }
        

       public string ProdLine { get; set; }
        public string Name { get; set; }

    }

    public class SmartActionPayerVM
    {
        public IList<SmartActionPayers> smartActionPayer { get; set; }
    }

    public class ConfirmedHoldingOrders
    {
        public int AccountNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public int WorkOrder { get; set; }
        public DateTime DateConfirmed { get; set; }

        public string HoldFromShippingReason { get; set; }
        public DateTime? Request_Date { get; set; }
    }

    public class ConfirmedHoldingOrdersVM
    {
        public IList<ConfirmedHoldingOrders> confirmedHoldingOrders { get; set; }
    }

    public class UnConfirmedHoldingOrders
    {
        public int AccountNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public int WorkOrder { get; set; }
        public DateTime DateConfirmed { get; set; }

        public string HoldFromShippingReason { get; set; }
        public DateTime? Request_Date { get; set; }
    }

    public class UnConfirmedHoldingOrdersVM
    {
        public IList<UnConfirmedHoldingOrders> unConfirmedHoldingOrders { get; set; }
    }
}