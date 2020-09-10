using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ReportsDatabase;

namespace USPS_Report.Models
{
    public class uspsDB
    {
        public static IList<UspsReportVM> GetReport(string _tracNum)
        {
            IList<UspsReportVM> _vm = new List<UspsReportVM>();
        
            Int32 workOrdernum;
            using (ReportsEntities _db = new ReportsEntities())
            {
                //  USPS_POD_tbl _list = new USPS_POD_tbl();

             IList<USPS_POD_tbl> _list = new List<USPS_POD_tbl>();
                if (_tracNum.Length > 10)
                    _list = _db.USPS_POD_tbl.Where(t => t.ConfirmationNum == _tracNum).ToList();  //.SingleOrDefault();
                else
                {
                    bool result = Int32.TryParse(_tracNum, out workOrdernum);
                    if (result)
                    { _list = _db.USPS_POD_tbl.Where(t => t.WorkOrder == workOrdernum).ToList(); }  //.SingleOrDefault(); }
                    else { return _vm; }

                }

                if (_list != null)
                {

                    foreach (var rec in _list)
                    {
                        UspsReportVM tableRec = new UspsReportVM();
                        tableRec.uID = rec.uID.ToString();
                        tableRec.WoID = rec.WorkOrder;
                        tableRec.PostDate = rec.PostDate;
                        tableRec.ConfirmationNum = rec.ConfirmationNum;
                        _vm.Add(tableRec);
                    }

                }
             
                  


            }
            return _vm;

        }


        public static IList<UspsReportVM> GetReportFedex(string _tracNum)
        {
            IList<UspsReportVM> _vm = new List<UspsReportVM>();

            Int32 workOrdernum;

            string _conn = ConfigurationManager.ConnectionStrings["EntitiesOracle1"].ConnectionString;

            OleDbConnection myConnection = new OleDbConnection(_conn);
            String OrdersFedExQuery = string.Empty;
            myConnection.Open();
            if (_tracNum.Length > 10)
            {
                // OrdersFedExQuery = "Select ID_WORKORDER, CONFIRMATIONNUMBER , DATESHIPPED from apps.TBL_UPS_WORKORDERS a where CONFIRMATIONNUMBER = '" + _tracNum + "' and CANCELDATE is Null";
                OrdersFedExQuery = "select Reference_Number, tracking_Number, Ship_Date from XXCUST01.jbm_FEDEX_PODS where tracking_Number = '" + _tracNum + "'";

            }
            else
            {
                bool result = Int32.TryParse(_tracNum, out workOrdernum);
                if (result)
                    OrdersFedExQuery = "Select ID_WORKORDER, CONFIRMATIONNUMBER , DATESHIPPED from apps.TBL_UPS_WORKORDERS a where ID_WORKORDER = '" + workOrdernum + "' and CANCELDATE is Null";

            }



            OleDbCommand myFedExCommand = new OleDbCommand(OrdersFedExQuery, myConnection);

            OleDbDataReader FedExReader = myFedExCommand.ExecuteReader();

            int RecordsFedEx = 0;
            while (FedExReader.Read())
            {
                UspsReportVM tableRec = new UspsReportVM();
                RecordsFedEx = RecordsFedEx + 1;

                string WorkOrder = FedExReader.GetValue(0).ToString();
                string confirmationNum = FedExReader.GetValue(1).ToString();
                string DATESHIPPED = FedExReader.GetValue(2).ToString();


                tableRec.WoID = Convert.ToInt32(WorkOrder);
                tableRec.PostDate = Convert.ToDateTime(DATESHIPPED);
                tableRec.ConfirmationNum = confirmationNum;
                _vm.Add(tableRec);

            }
            return _vm;

        }
        
        public static IList<UspsReportVM> GetReportUSPS2(string _tracNum)
        {
            IList<UspsReportVM> _vm = new List<UspsReportVM>();
           
            using (ReportsEntities _rdb = new ReportsEntities())
                if (_tracNum.Length > 10)
                {
                    var _list = (from t in _rdb.DetailedSPs
                                 where t.TrackingNum.Contains(_tracNum)
                                 && t.Type == "01" && t.DDate != null
                                 select t).ToList();

                    foreach (var v in _list)
                    {
                        UspsReportVM rec = new UspsReportVM();
                        rec.ConfirmationNum =  _tracNum;
                        rec.PostDate =  Convert.ToDateTime(v.DDate.Substring(4, 2) + "/"+ v.DDate.Substring(6, 2)+"/"+v.DDate.Substring(0,4));//Convert.ToDateTime("12/01/2016")
                        _vm.Add(rec);
                    }

                }

          
            
            return _vm;

        }
        public static IList<FedExReportVM> GetFedExReport(string _tracNum)
        {
            IList<FedExReportVM> _vm = new List<FedExReportVM>();
          
            Int32 workOrdernum;

            string _conn = ConfigurationManager.ConnectionStrings["EntitiesOracle1"].ConnectionString;

            OleDbConnection myConnection = new OleDbConnection(_conn);
            String OrdersFedExQuery = string.Empty;
            myConnection.Open();
            if (_tracNum.Length > 10)
            {
                OrdersFedExQuery = "Select ID_WORKORDER, CONFIRMATIONNUMBER , DATESHIPPED from apps.TBL_UPS_WORKORDERS a where CONFIRMATIONNUMBER = '" + _tracNum + "'";
            }
            else
            {
                bool result = Int32.TryParse(_tracNum, out workOrdernum);
                 if (result)
                 OrdersFedExQuery = "Select ID_WORKORDER, CONFIRMATIONNUMBER , DATESHIPPED from apps.TBL_UPS_WORKORDERS a where ID_WORKORDER = '" + workOrdernum + "'";

            }



            OleDbCommand myFedExCommand = new OleDbCommand(OrdersFedExQuery, myConnection);

            OleDbDataReader FedExReader = myFedExCommand.ExecuteReader();

            int RecordsFedEx = 0;
            while (FedExReader.Read())
            {
                FedExReportVM tableRec = new FedExReportVM();
                RecordsFedEx = RecordsFedEx + 1;

                string WorkOrder = FedExReader.GetValue(0).ToString();
                string confirmationNum = FedExReader.GetValue(1).ToString();
                string DATESHIPPED = FedExReader.GetValue(2).ToString();

               
                tableRec.WoID = WorkOrder;
                tableRec.DateShipped = DATESHIPPED;
                tableRec.ConfirmationNum = confirmationNum;
                _vm.Add(tableRec);

            }


            using (HHSQLDBEntities _db = new HHSQLDBEntities())
             {
                DateTime _dt = Convert.ToDateTime("2/24/2014");
                IList<tbl_PS_WorkOrder> OldRec = new List<tbl_PS_WorkOrder>();
                Int32 workOrder;
                if (_tracNum.Length > 10)
                {
                    OldRec = (from tbl in _db.tbl_PS_WorkOrder
                              where tbl.ConfirmationNumber == _tracNum
                              &&( tbl.Completed_Date <= _dt || tbl.Completed_Date == null)
                              select tbl).ToList();
                }
                else
                {
                    bool result = Int32.TryParse(_tracNum, out workOrder);
                    if (result)
                        OldRec = (from tbl in _db.tbl_PS_WorkOrder
                                  where tbl.ID == workOrder
                                  && tbl.Completed_Date < _dt
                                  select tbl).ToList();
                }

                foreach(var item in OldRec)
                {
                    FedExReportVM tableRec2 = new FedExReportVM();
                    tableRec2.WoID = item.ID.ToString();
                    tableRec2.ConfirmationNum = item.ConfirmationNumber;
                    if(item.Completed_Date != null)
                    tableRec2.DateShipped = item.Completed_Date.ToString();
                    _vm.Add(tableRec2);
                }
               
            }

                return _vm;

        }
        //GetFedExDetail
        public static DetailReport GetFedExData(string _id)
        {
            Guid _guid = Guid.Empty;
            DetailReport _tb = new DetailReport();
            if (_id != null)
                _guid = new Guid(_id);
            using (ReportsEntities _db = new ReportsEntities())
            {
                var _list = _db.FedEx_POD_tbl.Where(t => t.uID == _guid).SingleOrDefault();

                _tb.Acccount = _list.Account;
              
                _tb.Address1 = _list.Address1;
                _tb.City = _list.City;
                _tb.State = _list.State;
                _tb.Zip = _list.Zip;
                _tb.DStatus = _list.Status == "DL" ? "Delivered" : "Not Delivered"; // need to confirm
                //// _tb.DStatus = _statusCode.Value;

                _tb.DDate = _list.DeliveryDate.ToString();
                _tb.Weight = _list.Weight.ToString(); ;
                _tb.TDate = ""; // not available
                _tb.Name = _list.Name;
                _tb.Confirmation = _list.ConfirmationNum;
                _tb.WorkOrderId = _list.WorkOrder;




            }

            return _tb;

        }

        public static DetailReport GetData(string _id)
        {
            Guid _guid = Guid.Empty;
            DetailReport _tb = new DetailReport();
            if (_id != null)
                _guid = new Guid(_id);
            using (ReportsEntities _db = new ReportsEntities())
            {
                var _list = _db.USPS_POD_tbl.Where(t => t.uID == _guid).SingleOrDefault();

                var xml = XElement.Parse(_list.USPSReply.Trim());
               // var _acct = xml.DescendantsAndSelf().Select(t => t.Element("AccountID")).Take(1).SingleOrDefault();
                var Trans = xml.DescendantsAndSelf("Transaction");
                var _add = Trans.DescendantsAndSelf().Select(t => t.Element("ToAddress")).Take(1).SingleOrDefault();
                var _city = Trans.DescendantsAndSelf().Select(t => t.Element("ToCity")).Take(1).SingleOrDefault();
                var _state = Trans.DescendantsAndSelf().Select(t => t.Element("ToState")).Take(1).SingleOrDefault();
                var _zip = Trans.DescendantsAndSelf().Select(t => t.Element("ToZipCode")).Take(1).SingleOrDefault();
                var _statusCode = Trans.DescendantsAndSelf().Select(t => t.Element("StatusCode")).Take(1).SingleOrDefault();
                //var _status = Trans.DescendantsAndSelf().Select(t => t.Element("Status")).Take(1).SingleOrDefault();

                var _Ddate = Trans.DescendantsAndSelf().Select(t => t.Element("DeliveryDateTime")).Take(1).SingleOrDefault();
                var _wt = Trans.DescendantsAndSelf().Select(t => t.Element("Weight")).Take(1).SingleOrDefault();
                var _TDate = Trans.DescendantsAndSelf().Select(t => t.Element("TransactionDateTime")).Take(1).SingleOrDefault();
                var _Name = Trans.DescendantsAndSelf().Select(t => t.Element("ToName")).Take(1).SingleOrDefault(); 
                      var _TracNo = Trans.DescendantsAndSelf().Select(t => t.Element("PICNumber")).Take(1).SingleOrDefault();


                _tb.Acccount = _list.Account;
             //   _tb.Acccount = _acct.Value;
                _tb.Address1 = _add.Value;
                _tb.City = _city.Value;
                _tb.State = _state.Value;
                _tb.Zip = _zip.Value.Substring(0,5);
                _tb.DStatus = _statusCode.Value == "D" ? "Delivered" : "Not Delivered";
                // _tb.DStatus = _statusCode.Value;

                _tb.DDate = _Ddate.Value;
                _tb.Weight = _wt.Value;
                _tb.TDate = _TDate.Value;
                _tb.Name = _Name.Value;
                _tb.Confirmation = _TracNo.Value;
                _tb.WorkOrderId = _list.WorkOrder;
                _tb._id = _id;
            



            }

            return _tb;

        }

        public static IList<DetailReport> GetDataList()
        {
            Guid _guid = Guid.Empty;
            IList<DetailReport> _list1 = new List<DetailReport>();
            DetailReport _tb = new DetailReport();
           
            using (ReportsEntities _db = new ReportsEntities())
            {
                DateTime _dt = DateTime.Now.AddDays(-1);
                var _list3 = _db.USPS_POD_tbl.Where(t => t.Updated > _dt);
                foreach (var _list in _list3)
                {
                    _tb = new DetailReport();
                    var xml = XElement.Parse(_list.USPSReply.Trim());
                    // var _acct = xml.DescendantsAndSelf().Select(t => t.Element("AccountID")).Take(1).SingleOrDefault();
                    var Trans = xml.DescendantsAndSelf("Transaction");
                    var _add = Trans.DescendantsAndSelf().Select(t => t.Element("ToAddress")).Take(1).SingleOrDefault();
                    var _city = Trans.DescendantsAndSelf().Select(t => t.Element("ToCity")).Take(1).SingleOrDefault();
                    var _state = Trans.DescendantsAndSelf().Select(t => t.Element("ToState")).Take(1).SingleOrDefault();
                    var _zip = Trans.DescendantsAndSelf().Select(t => t.Element("ToZipCode")).Take(1).SingleOrDefault();
                    var _statusCode = Trans.DescendantsAndSelf().Select(t => t.Element("StatusCode")).Take(1).SingleOrDefault();
                    //var _status = Trans.DescendantsAndSelf().Select(t => t.Element("Status")).Take(1).SingleOrDefault();

                    var _Ddate = Trans.DescendantsAndSelf().Select(t => t.Element("DeliveryDateTime")).Take(1).SingleOrDefault();
                    var _wt = Trans.DescendantsAndSelf().Select(t => t.Element("Weight")).Take(1).SingleOrDefault();
                    var _TDate = Trans.DescendantsAndSelf().Select(t => t.Element("TransactionDateTime")).Take(1).SingleOrDefault();
                    var _Name = Trans.DescendantsAndSelf().Select(t => t.Element("ToName")).Take(1).SingleOrDefault();
                    var _TracNo = Trans.DescendantsAndSelf().Select(t => t.Element("PICNumber")).Take(1).SingleOrDefault();


                    _tb.Acccount = _list.Account;
                    //   _tb.Acccount = _acct.Value;
                    _tb.Address1 = _add.Value;
                    _tb.City = _city.Value;
                    _tb.State = _state.Value;
                    _tb.Zip = _zip.Value.Substring(0, 5);
                    _tb.DStatus = _statusCode.Value == "D" ? "Delivered" : "Not Delivered";
                    // _tb.DStatus = _statusCode.Value;

                    _tb.DDate = _Ddate.Value;
                    _tb.Weight = _wt.Value;
                    _tb.TDate = _TDate.Value;
                    _tb.Name = _Name.Value;
                    _tb.Confirmation = _TracNo.Value;
                    _tb.WorkOrderId = _list.WorkOrder;
                    _list1.Add(_tb);

                }

            }

            return _list1;

        }
    }
}