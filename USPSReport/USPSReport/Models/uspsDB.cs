using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using USPSReport.Models;

namespace USPSReport.Models
{
    public class uspsDB
    {
        public static IList<UspsReportVM> GetReport(string _tracNum)
        {
            IList<UspsReportVM> _vm = new List<UspsReportVM>();

            using (JBDBContext _db = new JBDBContext())
            { 
                var _list = _db.USPSPod.Take(1).SingleOrDefault();

                var xml = XElement.Parse(_list.USPSReply.Trim());
           

                var _acct = xml.DescendantsAndSelf().Select(t => t.Element("AccountID")).Take(1).SingleOrDefault();
              //  var tr = xml.DescendantsAndSelf("TransactionResults");
                var Trans = xml.DescendantsAndSelf("Transaction");

                var _add = Trans.DescendantsAndSelf().Select(t => t.Element("ToAddress")).Take(1).SingleOrDefault();
               
               
                        
                 
                    UspsReportVM tableRec = new UspsReportVM();
                    tableRec.uID = _list.uID.ToString();
                    tableRec.WoID = _list.WorkOrder;
                    tableRec.PostDate = _list.PostDate;
                    tableRec.ConfirmationNum = _list.ConfirmationNum;           
                   
                    

                    _vm.Add(tableRec);
            

            
            }
            return _vm;

        }

        public static DetailReport GetData(string _id)
        {
            Guid _guid = Guid.Empty;
            DetailReport _tb = new DetailReport();
            if(_id != null)
            _guid = new Guid(_id);
            using (JBDBContext _db = new JBDBContext())
            {
                var _list = _db.USPSPod.Where(t => t.uID == _guid).SingleOrDefault();

                var xml = XElement.Parse(_list.USPSReply.Trim());
                var _acct = xml.DescendantsAndSelf().Select(t => t.Element("AccountID")).Take(1).SingleOrDefault();
                var Trans = xml.DescendantsAndSelf("Transaction");
                var _add = Trans.DescendantsAndSelf().Select(t => t.Element("ToAddress")).Take(1).SingleOrDefault();
                var _city = Trans.DescendantsAndSelf().Select(t => t.Element("ToCity")).Take(1).SingleOrDefault();
                var _state = Trans.DescendantsAndSelf().Select(t => t.Element("ToState")).Take(1).SingleOrDefault();
                var _zip = Trans.DescendantsAndSelf().Select(t => t.Element("ToZipCode")).Take(1).SingleOrDefault();

                _tb.Acccount = _acct.Value;
                _tb.Address1 = _add.Value;
                _tb.City = _city.Value;
                _tb.State = _state.Value;
                _tb.Zip = _zip.Value;
                
                

            }

            return _tb;
 
        }
    }
}