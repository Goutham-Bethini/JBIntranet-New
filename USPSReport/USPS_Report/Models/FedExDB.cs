using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace USPS_Report.Models
{
    public class FedExDB
    {
        public static IList<FedExReportVM> GetReportFedEx(string _tracNum)
        {
            IList<FedExReportVM> _vm = new List<FedExReportVM>();
            FedExReportVM tableRec = new FedExReportVM();
            Int32 workOrdernum;
            using (ReportsEntities _db = new ReportsEntities())
            {
                FedEx_POD_tbl _list = new FedEx_POD_tbl();
                if (_tracNum.Length > 10)
                    _list = _db.FedEx_POD_tbl.Where(t => t.ConfirmationNum == _tracNum).SingleOrDefault();
                else
                {
                    bool result = Int32.TryParse(_tracNum, out workOrdernum);
                    if (result)
                    { _list = _db.FedEx_POD_tbl.Where(t => t.WorkOrder == workOrdernum).SingleOrDefault(); }
                    else { return _vm; }

                }

                if (_list != null)
                {
                    //tableRec.uID = _list.uID.ToString();
                    //tableRec.WoID = _list.WorkOrder;
                    //tableRec.PostDate = _list.PostDate;
                    //tableRec.ConfirmationNum = _list.ConfirmationNum;

                }
                if (_list != null)
                    _vm.Add(tableRec);


            }
            return _vm;

        }

        public static DetailReport GetDataFedEX(string _id)
        {
            Guid _guid = Guid.Empty;
            DetailReport _tb = new DetailReport();
            if (_id != null)
                _guid = new Guid(_id);
            using (ReportsEntities _db = new ReportsEntities())
            {
                var _list = _db.FedEx_POD_tbl.Where(t => t.uID == _guid).SingleOrDefault();

                var xml = XElement.Parse(_list.FedExReply.Trim());
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




            }

            return _tb;

        }
    }
}