using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
 using USPS_Report.Models;


namespace USPS_Report.Areas.Reports.Models
{
    public class SMOutbound
    {
        public static IList<ReassmentCalls> getOutboundReassessmentCalls(DateTime _Startdate , DateTime _enddate)
        {

            IList<ReassmentCalls> _list = new List<ReassmentCalls>();
            var conns = System.Configuration.ConfigurationManager.ConnectionStrings["CallAgentDbconnectionstring"].ConnectionString;
            SqlConnection _conn = new SqlConnection(conns);
            //SqlConnection _conn = new SqlConnection("Server=JBMAZSQL01;Database=CallAgentDB;Uid=SQLAPPUSER;pwd=$Ql@pp1");

            _conn.Open();

            var Query = "select * from OutboundReassessment where UpdateTime between '"+ _Startdate.Month+"/"+_Startdate.Day+"/"+_Startdate.Year + "' and '"+ _enddate.Month+"/"+_enddate.Day+"/"+_enddate.Year +"'";
            SqlCommand _cmd = new SqlCommand(Query, _conn);
            SqlDataReader _reader = _cmd.ExecuteReader();

            while (_reader.Read())
            {
                ReassmentCalls _acct = new ReassmentCalls();

                _acct.account = Convert.ToInt32(_reader[1]);
                _acct.SpokeWith = _reader[2].ToString();
                _acct.RelationShip = _reader[3].ToString();
                _acct.IsCallAnswered = _reader[4].ToString();
                _acct.CallAttempts = Convert.ToInt16(_reader[5]);
                _acct.CallEndStatus = _reader[6].ToString();
                _acct.LoadTime = Convert.ToDateTime(_reader[7]);


                _list.Add(_acct);
            }


            return _list;


        }

        public static IList<SACallsVM> getSAVictorCalls(string operatorName)
        {
            IList<SACallsVM> _list = new List<SACallsVM>();
            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {
                _callDb.Database.CommandTimeout = 0;
                string query = "SELECT Distinct" +

       " t1.AccountNumber, " +
    "	t1.OrderDate, " +
        " max(t1.LoadedDate) as DateConfirmed, " +
    "	t2.ID as WorkOrder,  " +
    "	t2.Request_Date,  " +
        " t2.HoldFromShippingReason , " + 
         " (case when t3.ProvidedCreditCard = 1 then 'Yes' when t3.ProvidedCreditCard = 0 then 'No' else '' end) 'ProvidedCreditCard' " +
         " ,v1.PayerName " +
  "  FROM " +
                  "  [CallAgentDB].[dbo].[OrderConfirmation]  t1 " +
" LEFT JOIN [HHSQLDB].[dbo].[tbl_PS_WorkOrder] " +
       " t2 ON  t1.AccountNumber = t2.Account " +
          " AND t1.OrderDate = t2.Request_Date " +
"  LEFT JOIN [CallAgentDB].[dbo].[OrderConfirmationInfo] " +
      "  t3 ON t3.OrderDate = t1.OrderDate " +
" and t3.Account = t1.AccountNumber  " +
"LEFT JOIN [HHSQLDB].[dbo].v__AccountMemberEffectiveInsurance_Ins1 v1 on t1.AccountNumber = v1.Account " +

            "WHERE t2.HoldFromShipping=1 " +
    "	AND t2.Cancel_Date IS NULL " +

      "  AND t2.Completed_Date IS NULL " +
       " AND t1.IsOrderConfirmed= 1 " +

     "   AND t2.HoldFromShippingReason IS NOT NULL " +

      " AND ABS(DATEDIFF(dd,t1.OrderDate,t1.LoadedDate)) < 30 " +

      " AND t1.OrderDate >= '01/01/2018' " +
       " group by t1.AccountNumber, t1.OrderDate, t2.ID ,  t2.Request_Date,  t2.HoldFromShippingReason ,  t3.ProvidedCreditCard ,v1.PayerName " +

    "ORDER BY  t1.AccountNumber "+
    "insert into Reports.dbo.tbl_ReportsAuditLine values('" + operatorName + "',15,GETDATE())";


                _list = _callDb.Database.SqlQuery<SACallsVM>(query).ToList<SACallsVM>();
            }
            return _list;
        }

        public static IList<SACallsVM> getSAVictorNotConfirmedCalls(string operatorName)
        {
            IList<SACallsVM> _list = new List<SACallsVM>();
            using (CallAgentDBEntitiesnew _callDb = new CallAgentDBEntitiesnew())
            {
                _callDb.Database.CommandTimeout = 0;
                string query = "	SELECT "+
        " t1.AccountNumber,  "+
		"t1.OrderDate, "+
    "	max(t1.LoadedDate) as DateConfirmed, " +
	"	t2.ID as WorkOrder, "+
		" t2.Request_Date,  "+
	"	t2.HoldFromShippingReason, "+
        " (case when t3.ProvidedCreditCard = 1 then 'Yes' when t3.ProvidedCreditCard = 0 then 'No' else '' end) 'ProvidedCreditCard' " +
        " ,v1.PayerName "+
 "   FROM " +
                  "  [CallAgentDB].[dbo].[OrderConfirmation] "+
       " t1 LEFT JOIN [HHSQLDB].[dbo].[tbl_PS_WorkOrder] "+
       " t2 ON  t1.AccountNumber = t2.Account "+
        "   AND t1.OrderDate = t2.Request_Date "+
         "  LEFT JOIN [CallAgentDB].[dbo].[OrderConfirmationInfo] "+
       " t3 ON t3.OrderDate = t1.OrderDate "+
        " and t3.Account = t1.AccountNumber "+
"LEFT JOIN [HHSQLDB].[dbo].v__AccountMemberEffectiveInsurance_Ins1 v1 on t1.AccountNumber = v1.Account " +
" WHERE  t2.HoldFromShipping=1 " +
	"	AND t2.Cancel_Date IS NULL "+
    " AND t2.Completed_Date IS NULL "+
   "     AND t1.IsOrderConfirmed= 0 "+
    " AND t2.HoldFromShippingReason IS NOT NULL "+
     " AND (t1.OrderDate-t1.LoadedDate) < 30 "+
     "  and 	t1.AccountNumber not in "+
     " (select distinct AccountNumber from CallAgentDB.dbo.OrderConfirmation "+

    "  where OrderDate = t1.OrderDate and IsOrderConfirmed = 1 AND OrderDate >= getdate()-14 )" +
     " group by t1.AccountNumber, t1.OrderDate, t2.ID ,   t2.Request_Date,  t2.HoldFromShippingReason,  t3.ProvidedCreditCard ,v1.PayerName" +
	" ORDER BY "+
     " t1.AccountNumber "+
     "insert into Reports.dbo.tbl_ReportsAuditLine values('" + operatorName + "',14,GETDATE())";


                _list = _callDb.Database.SqlQuery<SACallsVM>(query).ToList<SACallsVM>();
            }
            return _list;
        }


        public static IList<NewAccount> getNewAccountDetails( int? id)
        {

           
         

            IList<NewAccount> _list = new List<NewAccount>();
            var conns = System.Configuration.ConfigurationManager.ConnectionStrings["CallAgentDbconnectionstring"].ConnectionString;
            SqlConnection _conn = new SqlConnection(conns);
            //SqlConnection _conn = new SqlConnection("Server=JBMAZSQL01;Database=CallAgentDB;Uid=SQLAPPUSER;pwd=$Ql@pp1");

            _conn.Open();

            var Query = "   Select * from NewAccountDetails where UpdateStatus != 1 ";
            if (id != null  )
            {
                Query = "update NewAccountDetails "+
" set UpdateStatus = 1 "+
 " where UpdateStatus != 1 and id = "+ id +
";   select* from NewAccountDetails "+
" where UpdateStatus != 1 ";
            }

            SqlCommand _cmd = new SqlCommand(Query, _conn);
            SqlDataReader _reader = _cmd.ExecuteReader();

            while (_reader.Read())
            {
                NewAccount _acct = new NewAccount();
                _acct.id = Convert.ToInt16( _reader[0]);
                _acct.SupplyType = _reader[1].ToString();
                _acct.Gender = _reader[2].ToString();
                _acct.DOB = _reader[3].ToString();
                _acct.FullName = _reader[4].ToString();
                _acct.PrimaryPhone = _reader[5].ToString();
                _acct.AltPhone = _reader[6].ToString();
                _acct.IsNewAccountTransfer = Convert.ToBoolean(_reader[7]);
                _acct.InsuranceProvider = _reader[8].ToString();
                _acct.InsuranceProviderPhone = _reader[9].ToString();
                _acct.InsuranceID = _reader[10].ToString();
                _acct.DoctorName = _reader[11].ToString();
                _acct.DoctorLocation = _reader[12].ToString();
                _acct.DoctorPhone = _reader[13].ToString();
                _acct.DeliveryMethod = _reader[14].ToString();
                _acct.DeliveryEmail = _reader[15].ToString();
                _acct.DeliveryFax = _reader[16].ToString();
               
                _acct.Address = _reader[17].ToString();
                _acct.IsNewAddrValid = _reader[18].ToString();
                _acct.UpdateTime = _reader[19].ToString();

                _list.Add(_acct);
            }

            return _list;


        }
    }


    public class NewAcctDetails
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<NewAccount> details {get;set;}

    }
    public class NewAccount
    {
        public int id { get; set; }
        public string SupplyType { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string FullName { get; set; }
        public string PrimaryPhone { get; set; }
        public string AltPhone { get; set; }
        public bool IsNewAccountTransfer { get; set; }
        public string InsuranceProvider { get; set; }
        public string InsuranceProviderPhone { get; set; }
        public string InsuranceID { get; set; }
        public string DoctorName { get; set; }
        public string DoctorLocation { get; set; }
        public string DoctorPhone { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryEmail { get; set; }
        public string DeliveryFax { get; set; }

        public string Address { get; set; }
        public string IsNewAddrValid { get; set; }

        public string UpdateTime { get; set; }
    }


    public class ReassessmentCallsVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<ReassmentCalls> details { get; set; }

    }

    public class SAVictorCalls
    {
        public IList<SACallsVM> victorCalls { get; set; }

    }
    public class SACallsVM
    {
        public int AccountNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DateConfirmed { get; set; }
        public int WorkOrder { get; set; }
        public DateTime Request_Date { get; set; }
        public string HoldFromShippingReason { get; set;}
        public string ProvidedCreditCard { get; set; }
        public string PayerName { get; set; }

    }
    public class SAcallCount
    {
        public DateTime OrderDate { get; set; }
       
        public int count { get; set; }
       
    }
    public class SACallBackVM
    {
        public int id { get; set; }
        public int AccountNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CallTime { get; set; }
        public string CallStatus  { get; set; }
        public string SpokeWith { get; set; }
        public bool ISOrderConfirmed { get; set; }
        public string Reason { get; set;  }



    }
    public class ReassmentCalls
    {
        public int account { get; set; }
        public string SpokeWith { get; set; }
        public string RelationShip { get; set; }
        public string IsCallAnswered { get; set; }
        public int CallAttempts { get; set; }
        public string CallEndStatus { get; set; }
        
        public DateTime LoadTime { get; set; }
        
    }

}