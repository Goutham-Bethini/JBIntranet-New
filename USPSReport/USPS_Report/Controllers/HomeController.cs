using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Models;
using USPS_Report.Areas.Reports.Models;
using USPS_Report.Areas.Reports;
using iTextSharp.text;
using USPS_Report.Areas.Reports.Controllers;
using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using System.Threading;
using System.Configuration;
using System.Data.OleDb;
using ReportsDatabase;

namespace USPS_Report.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DashboardVM _vm1 = new DashboardVM();
           
            HoldReasonVM _vm = new HoldReasonVM();
            _vm.GetHoldReason = HoldCountByReasonReport.GetHoldCountByReason();
            _vm.HoldReasonPieChart = ChartClass.HoldReasonChart(_vm.GetHoldReason);
            _vm1.holdReason = _vm;

            HoldPayerVM _vp = new HoldPayerVM();
            _vp.GetHoldPayer = ReportWOHolds.GetAllWoHoldTypes_Qty();
             _vp.HoldPayerPieChart = ChartClass.HoldPayerChart(_vp.GetHoldPayer);
            _vm1.holdPayer = _vp;


            Session["UserHasAccessToManageOrders"] =  CheckUserIsInGroup("ManageOrders");
            return View(_vm1);
        }

        bool CheckUserIsInGroup(string group)
        {
            var components = User.Identity.Name.Split('\\');
            var userName = components.Last();
            using (var identity = new WindowsIdentity(userName))
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(group);
            }
        }

        public ActionResult USPSInfo()
        {
            MainVM vm = new MainVM();
          

            return View(vm);
        }

        [HttpPost]
        public ActionResult USPSInfo(MainVM vm)
        {
             
            vm.dataList = uspsDB.GetReport(vm.trackNum);

        //  if (vm.dataList.Count == 0)
             //  vm.dataList = null;
         
            return View(vm);

        }


        public ActionResult FedexInfo()
        {
            MainVM vm = new MainVM();


            return View(vm);
        }

        [HttpPost]
        public ActionResult FedexInfo(MainVM vm)
        {

            vm.dataList = uspsDB.GetReportFedex(vm.trackNum);

            //  if (vm.dataList.Count == 0)
            //  vm.dataList = null;

            return View(vm);

        }

        public ActionResult UDPSInfo2()
        {
            MainVM vm = new MainVM();


            return View(vm);
        }

        [HttpPost]
        public ActionResult UDPSInfo2(MainVM vm)
        {

            vm.dataList = uspsDB.GetReportUSPS2(vm.trackNum);

            //  if (vm.dataList.Count == 0)
            //  vm.dataList = null;

            return View(vm);

        }

        public ActionResult MainMethod(string _pId)
        {
            DetailReport _dRpt = uspsDB.GetData(_pId);
           /* var _rec = uspsDB.GetDataList();
            foreach (var _dRpt1 in _rec)
            {*/
              //  BuildPdf(_dRpt);
           // }

            return PartialView("_MainMethod", _dRpt);
          
        }

        [HttpPost]
        public ActionResult GenPdf1(DetailReport _dRpt)
        {
            string filename = _dRpt.Confirmation + "_" + _dRpt.WorkOrderId.ToString() + ".pdf";
            _dRpt.PdfExists = false;

            string path = "C://USPS_Pdf" + "//";
            if (System.IO.File.Exists(path + filename)) 
            {
                _dRpt.PdfExists = true;
            }
            else
            {
                BuildPdf(_dRpt);
            }
            return View(_dRpt);
        }

        public void GenPdfFedex(string _pId, DateTime? _date)
        {
           // DetailReport _dRpt = uspsDB.GetData(_pId);

            string _conn = ConfigurationManager.ConnectionStrings["EntitiesOracle1"].ConnectionString;
            string _dt = _date.Value.Month + "/" + _date.Value.Day + "/" + _date.Value.Year;


            OleDbConnection myConnection = new OleDbConnection(_conn);
            String OrdersFedExQuery = string.Empty;
            myConnection.Open();

            // OrdersFedExQuery = "Select ID_WORKORDER, CONFIRMATIONNUMBER , DATESHIPPED from apps.TBL_UPS_WORKORDERS a where CONFIRMATIONNUMBER = '" + _tracNum + "' and CANCELDATE is Null";
         OrdersFedExQuery = "select Reference_Number, tracking_Number,RECIP_ADDR,RECIP_ADDR2,RECIP_CITY,REC_ST,RECIP_ZIP, DELIVER_DATE,Ship_Date,RATED_WGT,RECIP_NAME,Invoice_date, Invoice_number  from XXCUST01.jbm_FEDEX_PODS where tracking_Number = '" + _pId + "' and Ship_Date = TO_DATE ('"+_dt+"', 'MM/DD/YYYY')  ";
          
               // "select ID_WORKORDER, CONFIRMATIONNUMBER , DATESHIPPED, INTWEIGHT from  TBL_UPS_WORKORDERS where ConfirmationNumber = '" + _pId + "' ; ";

            OleDbCommand myFedExCommand = new OleDbCommand(OrdersFedExQuery, myConnection);

            OleDbDataReader FedExReader = myFedExCommand.ExecuteReader();

            DetailReport _dRpt = new DetailReport();
            int RecordsFedEx = 0;
            while (FedExReader.Read())
            {
                
                RecordsFedEx = RecordsFedEx + 1;

                _dRpt.WorkOrderId = Convert.ToInt32(FedExReader.GetValue(0).ToString());
                _dRpt.Confirmation = FedExReader.GetValue(1).ToString();
                _dRpt.Address1 = FedExReader.GetValue(2).ToString()+", "+ FedExReader.GetValue(3).ToString();
                _dRpt.City = FedExReader.GetValue(4).ToString();
                _dRpt.State = FedExReader.GetValue(5).ToString();
                _dRpt.Zip = FedExReader.GetValue(6).ToString();
                _dRpt.DDate = FedExReader.GetValue(7).ToString();
                _dRpt.TDate = FedExReader.GetValue(8).ToString();
                _dRpt.Weight = FedExReader.GetValue(9).ToString();
                _dRpt.Name = FedExReader.GetValue(10).ToString();
                _dRpt.Invoice_date = FedExReader.GetValue(11).ToString();
                _dRpt.Invoice_number = FedExReader.GetValue(12).ToString();

                _dRpt.DDate = Convert.ToDateTime(_dRpt.DDate).ToShortDateString();
                _dRpt.TDate = Convert.ToDateTime(_dRpt.TDate).ToShortDateString();
                _dRpt.Invoice_date = Convert.ToDateTime(_dRpt.Invoice_date).ToShortDateString();


                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    _dRpt.Acccount = (from tbl in _db.tbl_PS_WorkOrder
                                     where tbl.ID == _dRpt.WorkOrderId
                                     select tbl.Account).SingleOrDefault();
                }

                    //_dRpt.Confirmation = "633347834830"; //TRACKING_NUMBER
                    //_dRpt.Address1 = "5817 LODGESTONE DR";// RECIP_ADDR  ,  RECIP_ADDR2
                    //_dRpt.City = "MCKINNEY"; //RECIP_CITY
                    //_dRpt.State = "TX";  //REC_ST
                    //_dRpt.Zip = "75070010317"; // RECIP_ZIP
                    //_dRpt.DDate = "3/17/2016"; //DELIVER_DATE
                    //_dRpt.TDate = "3/14/2016";  // SHIP_DATE
                    //_dRpt.Weight = "14 lbs"; // RATED_WGT

                    //_dRpt.Name = "MARIA CARVAJAL"; //RECIP_NAME

                    //_dRpt.WorkOrderId = 5025564; //REFERENCE_NUMBER

                }


            //For FedEx pod
            //_dRpt.Confirmation = "633347834830"; //TRACKING_NUMBER
            //_dRpt.Address1 = "5817 LODGESTONE DR";// RECIP_ADDR  ,  RECIP_ADDR2
            //_dRpt.City = "MCKINNEY"; //RECIP_CITY
            //_dRpt.State = "TX";  //REC_ST
            //_dRpt.Zip = "75070010317"; // RECIP_ZIP
            //_dRpt.DDate = "3/17/2016"; //DELIVER_DATE
            //_dRpt.TDate = "3/14/2016";  // SHIP_DATE
            //_dRpt.Weight = "14 lbs"; // RATED_WGT

            //_dRpt.Name = "MARIA CARVAJAL"; //RECIP_NAME

            //_dRpt.WorkOrderId = 5025564; //REFERENCE_NUMBER
            //_dRpt.Acccount = 328411;  // get from workorder table

            BuildPdfFedEXPOD(_dRpt);
            //  BuildPdf(_dRpt);
            ShowFedexPdf(_dRpt);

        }

        public void GenPdfUSPS2(string _pId, DateTime? _date)
        {
            // DetailReport _dRpt = uspsDB.GetData(_pId);
            DetailReport _dRpt = new DetailReport();
           


                string _conn = ConfigurationManager.ConnectionStrings["EntitiesOracle1"].ConnectionString;
            string _dt = _date.Value.Month + "/" + _date.Value.Day + "/" + _date.Value.Year;


            OleDbConnection myConnection = new OleDbConnection(_conn);
            String OrdersFedExQuery = string.Empty;
            myConnection.Open();

            // OrdersFedExQuery = "Select ID_WORKORDER, CONFIRMATIONNUMBER , DATESHIPPED from apps.TBL_UPS_WORKORDERS a where CONFIRMATIONNUMBER = '" + _tracNum + "' and CANCELDATE is Null";
            OrdersFedExQuery = "Select ID_WORKORDER, CONFIRMATIONNUMBER, DATESHIPPED, INTWEIGHT from TBL_UPS_WORKORDERS where CONFIRMATIONNUMBER = '" + _pId + "' ";

            OleDbCommand myFedExCommand = new OleDbCommand(OrdersFedExQuery, myConnection);

            OleDbDataReader FedExReader = myFedExCommand.ExecuteReader();

       
            int RecordsFedEx = 0;
            while (FedExReader.Read())
            {

                RecordsFedEx = RecordsFedEx + 1;

                _dRpt.WorkOrderId = Convert.ToInt32(FedExReader.GetValue(0).ToString());
                _dRpt.Confirmation = FedExReader.GetValue(1).ToString();
               // _dRpt.Address1 = FedExReader.GetValue(2).ToString() + ", " + FedExReader.GetValue(3).ToString();
             //   _dRpt.City = FedExReader.GetValue(4).ToString();
               // _dRpt.State = FedExReader.GetValue(5).ToString();
             //   _dRpt.Zip = FedExReader.GetValue(6).ToString();
             //   _dRpt.DDate = FedExReader.GetValue(7).ToString();
                _dRpt.TDate = FedExReader.GetValue(2).ToString();
                _dRpt.Weight = FedExReader.GetValue(3).ToString();
             //   _dRpt.Name = FedExReader.GetValue(10).ToString();
             //   _dRpt.Invoice_date = FedExReader.GetValue(11).ToString();
             //   _dRpt.Invoice_number = FedExReader.GetValue(12).ToString();

             //   _dRpt.DDate = Convert.ToDateTime(_dRpt.DDate).ToShortDateString();
                _dRpt.TDate = Convert.ToDateTime(_dRpt.TDate).ToShortDateString();
              //  _dRpt.Invoice_date = Convert.ToDateTime(_dRpt.Invoice_date).ToShortDateString();


                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    _dRpt.Acccount = (from tbl in _db.tbl_PS_WorkOrder
                                      where tbl.ID == _dRpt.WorkOrderId
                                      select tbl.Account).SingleOrDefault();
                }

                using (USPS_Report.Models.ReportsEntities _rdb = new USPS_Report.Models.ReportsEntities())
                {
                _dRpt.Name  = (from d in _rdb.DetailedSPs
                                  where d.TrackingNum.Contains(_pId)
                                  &&  d.FN_LN != ""
                                  select d.FN_LN
                                  ).Take(1).SingleOrDefault();

                    _dRpt.Address1 = (from d in _rdb.DetailedSPs
                                      where d.TrackingNum.Contains(_pId)
                                      &&  d.Street != "" && d.Type == "01"
                                      select d.Street
                                  ).Take(1).SingleOrDefault();

                    _dRpt.City = (from d in _rdb.DetailedSPs
                                  where d.TrackingNum.Contains(_pId)
                                  &&  d.City_State != "" && d.Type == "01"
                                  select d.City_State
                                  ).Take(1).SingleOrDefault();

                    _dRpt.Zip = (from d in _rdb.DetailedSPs
                                 where d.TrackingNum.Contains(_pId)
                                 &&   d.Zip != "" && d.Type == "01"
                                 select d.Zip
                                  ).Take(1).SingleOrDefault();
                    _dRpt.DDate = Convert.ToDateTime( _date).ToShortDateString();
                }

                //_dRpt.Confirmation = "633347834830"; //TRACKING_NUMBER
                //_dRpt.Address1 = "5817 LODGESTONE DR";// RECIP_ADDR  ,  RECIP_ADDR2
                //_dRpt.City = "MCKINNEY"; //RECIP_CITY
                //_dRpt.State = "TX";  //REC_ST
                //_dRpt.Zip = "75070010317"; // RECIP_ZIP
                //_dRpt.DDate = "3/17/2016"; //DELIVER_DATE
                //_dRpt.TDate = "3/14/2016";  // SHIP_DATE
                //_dRpt.Weight = "14 lbs"; // RATED_WGT

                //_dRpt.Name = "MARIA CARVAJAL"; //RECIP_NAME

                //_dRpt.WorkOrderId = 5025564; //REFERENCE_NUMBER

            }


            //For FedEx pod
            //_dRpt.Confirmation = "633347834830"; //TRACKING_NUMBER
            //_dRpt.Address1 = "5817 LODGESTONE DR";// RECIP_ADDR  ,  RECIP_ADDR2
            //_dRpt.City = "MCKINNEY"; //RECIP_CITY
            //_dRpt.State = "TX";  //REC_ST
            //_dRpt.Zip = "75070010317"; // RECIP_ZIP
            //_dRpt.DDate = "3/17/2016"; //DELIVER_DATE
            //_dRpt.TDate = "3/14/2016";  // SHIP_DATE
            //_dRpt.Weight = "14 lbs"; // RATED_WGT

            //_dRpt.Name = "MARIA CARVAJAL"; //RECIP_NAME

            //_dRpt.WorkOrderId = 5025564; //REFERENCE_NUMBER
            //_dRpt.Acccount = 328411;  // get from workorder table

           // BuildPdfFedEXPOD(_dRpt);
            BuildPdf(_dRpt);
            // ShowFedexPdf(_dRpt);
            ShowPdf(_dRpt);

        }


        public void GenPdf(string _pId)
        {
            DetailReport _dRpt = uspsDB.GetData(_pId);

            //For FedEx pod
            //_dRpt.Confirmation = "633347834830"; //TRACKING_NUMBER
            //_dRpt.Address1 = "5817 LODGESTONE DR";// RECIP_ADDR  ,  RECIP_ADDR2
            //_dRpt.City = "MCKINNEY"; //RECIP_CITY
            //_dRpt.State = "TX";  //REC_ST
            //_dRpt.Zip = "75070010317"; // RECIP_ZIP
            //_dRpt.DDate = "3/17/2016"; //DELIVER_DATE
            //_dRpt.TDate = "3/14/2016";  // SHIP_DATE
            //_dRpt.Weight = "14 lbs"; // RATED_WGT

            //_dRpt.Name = "MARIA CARVAJAL"; //RECIP_NAME

            //_dRpt.WorkOrderId = 5025564; //REFERENCE_NUMBER
            //_dRpt.Acccount = 328411;  // get from workorder table

            //BuildPdfFedEXPOD(_dRpt);
            BuildPdf(_dRpt);
            ShowPdf(_dRpt);
        
        }

        // pdf from memory instead of physical memory
        private byte[] CreatePDF2(DetailReport _dRpt)
        {
            Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);

            using (MemoryStream output = new MemoryStream())
            {
                PdfWriter wri = PdfWriter.GetInstance(doc, output);
                doc.Open();

                this.GenerateUSPSInfo(doc, _dRpt);
              
                return output.ToArray();
            }

        }

        private void ShowPdf(DetailReport _dRpt)
        {
            string Date = DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString();
           
            string filename = _dRpt.Confirmation + "_" + _dRpt.WorkOrderId.ToString() + ".pdf";
            
            _dRpt.FileExists = null;

            string path = "C://POD_Docs//USPS_Pdf" + "//";

            string FilePath =path+filename;
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath);
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
            }
        }

        private void ShowFedexPdf(DetailReport _dRpt)
        {
            string Date = DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString();

            string filename = _dRpt.Confirmation + "_" + _dRpt.WorkOrderId.ToString() + ".pdf";

            _dRpt.FileExists = null;

            string path = "C://POD_Docs//FedEx_Pdf" + "//";

            string FilePath = path + filename;
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath);
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
            }
        }

        //public ActionResult FedExInfo()
        //{
        //    MainVMFedEx vm = new MainVMFedEx();

        //    return View(vm);
        //}

        //[HttpPost]
        //public ActionResult FedExInfo(MainVMFedEx vm)
        //{

        //    vm.dataList = uspsDB.GetFedExReport(vm.trackNum);

        //    return View(vm);

        //}

        public void FedExMainMethod(string _trackNum, string _date)
        {
            long size = 0;
            string _file = String.Empty;
            DateTime? Dateshipped = Convert.ToDateTime(_date);
            Int32? Year = Convert.ToInt32(Dateshipped.Value.Date.Year);
            Int32? month = Convert.ToInt32(Dateshipped.Value.Date.Month);


         
            var fileName = _trackNum.Trim() + "-" + Dateshipped.Value.Date.ToString("MMddyyyy") + ".pdf";
            WebClient webClient = new WebClient();

            // string path = "C://POD_Docs//FedEx_Pdf//" + Year.ToString() + "//";
            string path = @"\\jbmmifs003\POD\NewFedEx\" + Year.ToString() + "\\";
            

            if (System.IO.File.Exists(path + fileName))
            {

                //  ShowPdf(_dRpt);
            }
            else
            {
                if (_trackNum.Trim().Length == 15)
                {
                    if (Year == 2015)
                    {
                        if (month <= 6)
                        {
                            _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12015~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=259087058&type=SPOD&appType=&anon=";
                            webClient.DownloadFile(_file, path + fileName);
                            Thread.Sleep(200);
                            FileInfo info = new FileInfo(path + fileName);
                            size = info.Length;
                            if (size < 4000)
                            {
                                _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=22015~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=259087058&type=SPOD&appType=&anon=";
                                webClient.DownloadFile(_file, path + fileName);
                                Thread.Sleep(200);

                                //----------------------------
                                info = new FileInfo(path + fileName);
                                size = info.Length;
                                if (size < 4000)
                                {
                                    _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12014~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=259087058&type=SPOD&appType=&anon=";
                                    webClient.DownloadFile(_file, path + fileName);
                                    Thread.Sleep(200);
                                     info = new FileInfo(path + fileName);
                                    size = info.Length;
                                    if (size < 4000) {
                                        _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=22014~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=259087058&type=SPOD&appType=&anon=";
                                        webClient.DownloadFile(_file, path + fileName);
                                        Thread.Sleep(200);
                                    }
                                }
                                //---------------------------
                            }
                         
                            }
                        else
                        {
                            _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=22015~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=259087058&type=SPOD&appType=&anon=";
                            webClient.DownloadFile(_file, path + fileName);
                            Thread.Sleep(200);
                            FileInfo info = new FileInfo(path + fileName);
                            size = info.Length;
                            if (size < 4000)
                            {
                                _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12015~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=259087058&type=SPOD&appType=&anon=";
                                webClient.DownloadFile(_file, path + fileName);
                                Thread.Sleep(200);
                            }
                        }
                    }
                    else if (Year == 2014)
                    {

                        _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12014~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=259087058&type=SPOD&appType=&anon=";
                        webClient.DownloadFile(_file, path + fileName);
                        Thread.Sleep(200);
                        FileInfo info = new FileInfo(path + fileName);
                        size = info.Length;
                        if (size < 4000)
                        {
                            _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12014~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=259087058&type=SPOD&appType=&anon=";
                            webClient.DownloadFile(_file, path + fileName);
                            Thread.Sleep(200);
                        }

                    }
                }
                else
                {
                    if (Year == 2015)
                    {
                        if (month <= 6)
                        {
                            _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12015~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=325330805&type=SPOD&appType=&anon=";
                            webClient.DownloadFile(_file, path + fileName);
                            Thread.Sleep(200);
                            FileInfo info = new FileInfo(path + fileName);
                            size = info.Length;
                            if (size < 4000)
                            {
                                _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=22015~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=325330805&type=SPOD&appType=&anon=";
                                webClient.DownloadFile(_file, path + fileName);
                                Thread.Sleep(200);

                                //--------------------------------------
                                info = new FileInfo(path + fileName);
                                size = info.Length;
                                if (size < 4000)
                                {
                                    _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12014~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=325330805&type=SPOD&appType=&anon=";
                                    webClient.DownloadFile(_file, path + fileName);
                                    Thread.Sleep(200);
                                    info = new FileInfo(path + fileName);
                                    size = info.Length;
                                    if (size < 4000)
                                    {

                                        _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=22014~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=325330805&type=SPOD&appType=&anon=";
                                        webClient.DownloadFile(_file, path + fileName);
                                        Thread.Sleep(200);
                                    }

                                }
                                //-----------------------------------------
                            }
                        }
                        else
                        {
                            _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=22015~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=325330805&type=SPOD&appType=&anon=";
                            webClient.DownloadFile(_file, path + fileName);
                            Thread.Sleep(200);
                            FileInfo info = new FileInfo(path + fileName);
                            size = info.Length;
                            if (size < 4000)
                            {
                                _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12015~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=325330805&type=SPOD&appType=&anon=";
                                webClient.DownloadFile(_file, path + fileName);
                                Thread.Sleep(200);
                            }
                        }
                    }
                    else if (Year == 2014)
                    {

                        _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12014~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=325330805&type=SPOD&appType=&anon=";
                        webClient.DownloadFile(_file, path + fileName);
                        Thread.Sleep(200);
                        FileInfo info = new FileInfo(path + fileName);
                        size = info.Length;
                        if (size < 4000)
                        {
                            _file = "https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber=" + _trackNum.Trim() + "&trackingQualifier=12014~" + _trackNum.Trim() + "~FDEG&trackingCarrier=FDXG&shipDate=&destCountry=&locale=en_US&accountNbr=325330805&type=SPOD&appType=&anon=";
                            webClient.DownloadFile(_file, path + fileName);
                            Thread.Sleep(200);
                        }
                    }

                }





            }





            ShowFedEXPdf(_trackNum, _date);

        }

        private void ShowFedEXPdf(string _trackNum, string _date)
        {
          //  bool find = false;
            WebClient User = new WebClient();
            DateTime? Dateshipped = Convert.ToDateTime(_date);
            Int32? Year = Convert.ToInt32(Dateshipped.Value.Date.Year);
            Int32? Month = Convert.ToInt32(Dateshipped.Value.Date.Month);
            //string path = "C://POD_Docs//FedEx_Pdf//" + Year.ToString() + "//";
            string path = @"\\jbmmifs003\POD\NewFedEx\" + Year.ToString() + "\\";
            var fileName = _trackNum.Trim() + "-" + Dateshipped.Value.Date.ToString("MMddyyyy") + ".pdf";
            DateTime _dt = Convert.ToDateTime("2/24/2014");
         

            if (Dateshipped.Value.Date < _dt)
            {
              fileName = _trackNum + ".pdf";
              path = @"\\jbmmifs003\POD\FedEx\From_Old_System\";
            }
            string FilePath = path + fileName;
            if (System.IO.File.Exists(path + fileName))
            {
               // find = true;
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader(_trackNum, FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Thread.Sleep(200);
                }
            }
            else if (System.IO.File.Exists(@"\\jbmmifs003\POD\FedEx\From_Old_System\ManualPOD\" + fileName))
            {

                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader(_trackNum, FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Thread.Sleep(200);

                }
            }
            else if (System.IO.File.Exists(@"\\jbmmifs003\POD\FedEx\From_Old_System\Archive\August2013\" + fileName))
            {

                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader(_trackNum, FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Thread.Sleep(200);

                }
            }
            else if (System.IO.File.Exists(@"\\jbmmifs003\POD\FedEx\From_Old_System\Archive\July2013\" + fileName))
            {

                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader(_trackNum, FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Thread.Sleep(200);

                }
            }
            else if (System.IO.File.Exists(@"\\jbmmifs003\POD\FedEx\From_Old_System\Archive\June2013\" + fileName))
            {

                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader(_trackNum, FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Thread.Sleep(200);

                }
            }
        }



        private iTextSharp.text.Font _Font12BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);
        private iTextSharp.text.Font _Font12BoldUnderlineFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 12, iTextSharp.text.Font.UNDERLINE | Font.BOLD, iTextSharp.text.Color.BLACK);
        private iTextSharp.text.Font _Font12BoldBlueUnderlineFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 12, iTextSharp.text.Font.BOLD | Font.UNDERLINE, iTextSharp.text.Color.BLUE);
        private iTextSharp.text.Font _Font12Font = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
        private iTextSharp.text.Font _Font11Font = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 11, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
        private iTextSharp.text.Font _Font10Font = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
        private iTextSharp.text.Font _Font11BoldFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 11, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);

        public void BuildPdf(DetailReport _dRpt)
        {

            string Date = DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString();
            iTextSharp.text.Document doc = null;
         
            string filename = _dRpt.Confirmation + "_" + _dRpt.WorkOrderId.ToString() + ".pdf";
           
            doc = new Document();
           
            _dRpt.FileExists = null;
            
                string path = "C://POD_Docs//USPS_Pdf" + "//";
                if (System.IO.File.Exists(path + filename))
                {
                    _dRpt.FileExists = true;
                }
                else
                {

                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc,
                        new System.IO.FileStream(path + filename,
                            System.IO.FileMode.Create));
                    _dRpt.FileExists = false;

                    // Set the margins and page size
                    this.SetStandardPageSize(doc);

                    // Add metadata to the document.  This information is visible when viewing the 
                    // document properities within Adobe Reader.
                    doc.AddTitle("Report");

                    // Add Xmp metadata to the document.
                    this.CreateXmpMetadata(writer);

                    // Open the document for writing content
                    doc.Open();

                this.GenerateUSPSInfo(doc, _dRpt);
                this.SetStandardPageSize(doc);  // Reset the margins and page size
                   
                    doc.Close();
                    doc = null;
                
            }

        }

        public void BuildPdfFedEXPOD(DetailReport _dRpt)
        {

            string Date = DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString();
            Date = "March 03,2014";
            iTextSharp.text.Document doc = null;

            string filename = _dRpt.Confirmation + "_" + _dRpt.WorkOrderId.ToString() + ".pdf";

            doc = new Document();

            _dRpt.FileExists = null;

            string path = "C://POD_Docs//FedEx_Pdf" + "//";
            if (System.IO.File.Exists(path + filename))
            {
                _dRpt.FileExists = true;
            }
            else
            {

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc,
                    new System.IO.FileStream(path + filename,
                        System.IO.FileMode.Create));
                _dRpt.FileExists = false;

                // Set the margins and page size
                this.SetStandardPageSize(doc);

                // Add metadata to the document.  This information is visible when viewing the 
                // document properities within Adobe Reader.
                doc.AddTitle("Report");

                // Add Xmp metadata to the document.
                this.CreateXmpMetadata(writer);

                // Open the document for writing content
                doc.Open();

                this.GenerateFEDEXPODInfo(doc, _dRpt);
                this.SetStandardPageSize(doc);  // Reset the margins and page size

                doc.Close();
                doc = null;

            }

        }


        private void SetStandardPageSize(iTextSharp.text.Document doc)
        {
            // Set margins and page size for the document
            doc.SetMargins(20, 20, 20, 20);
            // There are a huge number of possible page sizes, including such sizes as
            // EXECUTIVE, POSTCARD, LEDGER, LEGAL, LETTER_LANDSCAPE, and NOTE
            doc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.LETTER.Width,
                iTextSharp.text.PageSize.LETTER.Height));
        }
        private void CreateXmpMetadata(iTextSharp.text.pdf.PdfWriter writer)
        {
            // Set up the buffer to hold the XMP metadata
            byte[] buffer = new byte[65536];
            System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer, true);

            try
            {
                // XMP supports a number of different schemas, which are made available by iTextSharp.
                // Here, the Dublin Core schema is chosen.
                iTextSharp.text.xml.xmp.XmpSchema dc = new iTextSharp.text.xml.xmp.DublinCoreSchema();

                // Add Dublin Core attributes
                iTextSharp.text.xml.xmp.LangAlt title = new iTextSharp.text.xml.xmp.LangAlt();
                title.Add("x-default", "FedEx Report");
                dc.SetProperty(iTextSharp.text.xml.xmp.DublinCoreSchema.TITLE, title);

                // Dublin Core allows multiple authors, so we create an XmpArray to hold the values
                iTextSharp.text.xml.xmp.XmpArray author = new iTextSharp.text.xml.xmp.XmpArray(iTextSharp.text.xml.xmp.XmpArray.ORDERED);
                author.Add("GR");
                dc.SetProperty(iTextSharp.text.xml.xmp.DublinCoreSchema.CREATOR, author);

                // Multiple subjects are also possible, so another XmpArray is used
                iTextSharp.text.xml.xmp.XmpArray subject = new iTextSharp.text.xml.xmp.XmpArray(iTextSharp.text.xml.xmp.XmpArray.UNORDERED);
                subject.Add("Report");
                subject.Add("Report");
                dc.SetProperty(iTextSharp.text.xml.xmp.DublinCoreSchema.SUBJECT, subject);

                // Create an XmpWriter using the MemoryStream defined earlier
                iTextSharp.text.xml.xmp.XmpWriter xmp = new iTextSharp.text.xml.xmp.XmpWriter(ms);
                xmp.AddRdfDescription(dc);  // Add the completed metadata definition to the XmpWriter
                xmp.Close();    // This flushes the XMP metadata into the buffer

                //---------------------------------------------------------------------------------
                // Shrink the buffer to the correct size (discard empty elements of the byte array)
                int bufsize = buffer.Length;
                int bufcount = 0;
                foreach (byte b in buffer)
                {
                    if (b == 0) break;
                    bufcount++;
                }
                System.IO.MemoryStream ms2 = new System.IO.MemoryStream(buffer, 0, bufcount);
                buffer = ms2.ToArray();
                //---------------------------------------------------------------------------------

                // Add all of the XMP metadata to the PDF doc that we're building
                writer.XmpMetadata = buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
                ms.Dispose();
            }
        }

        private void GenerateUSPSInfo(iTextSharp.text.Document doc, DetailReport _dRpt)
        {
            doc.NewPage();
            string _dt = DateTime.Today.ToString();
            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk("\nUSPS Info\n"));
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p);
            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk("\n"));
            _dRpt.DStatus = "Delivered";

            //C:\\images" + "\\USPS_ Template.jpg
            //Server.MapPath("/Image/USPS_Template.jpg")
            iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance("C:\\images" + "\\USPS_Template.jpg");
           // iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(Server.MapPath("/Image/USPS_Template.jpg"));
            jpg1.ScaleToFit(100, 80);
            PdfPCell imageCell = new PdfPCell(jpg1);
            imageCell.Colspan = 1; // either 1 if you need to insert one cell
            imageCell.Border = 0;
            // imageCell.HorizontalAlignment =Element.ALIGN_CENTER;
            imageCell.VerticalAlignment = Element.ALIGN_TOP; 
            // imageCell.se(Element.ALIGN_CENTER);

            //  cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

            PdfPTable table3 = new PdfPTable(1);
            table3.HorizontalAlignment = Element.ALIGN_LEFT;
            // PdfPCell cell1 = new PdfPCell();

            table3.WidthPercentage = 40;
            float[] widths3 = new float[] { 80f };
        
            table3.SetWidths(widths3);

            table3.DefaultCell.Border = Rectangle.NO_BORDER;


            table3.AddCell(imageCell);
         
         
            doc.Add(table3);

            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk("\n\n\n"));
          //  this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk(_dt+"\n\n"));
            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk("Dear Customer\n\n"));
            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk("The Following is the proof of delivery for tracking number:                  "+_dRpt.Confirmation+"\n\n"));

            Paragraph p1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p1);


            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk("Delivery Information:"));
          
            Paragraph p2 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p2);


            PdfPTable table4 = new PdfPTable(4);
            table4.HorizontalAlignment = Element.ALIGN_LEFT;
            // PdfPCell cell1 = new PdfPCell();
            table4.WidthPercentage = 100;
            float[] widths4 = new float[] { 60f, 60f, 60f, 90f };
            table4.SetWidths(widths4);

            table4.DefaultCell.Border = Rectangle.NO_BORDER;


            PdfPCell fCell1 = new PdfPCell(new Phrase("Status:", _Font12BoldFont));
            fCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell1.Border = 0;
            table4.AddCell(fCell1);

            PdfPCell fCell2 = new PdfPCell(new Phrase(_dRpt.DStatus, _Font12Font));
            fCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell2.Border = 0;
            table4.AddCell(fCell2);

            PdfPCell fCell3 = new PdfPCell(new Phrase("Delivery Location:", _Font12BoldFont));
            fCell3.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell3.Border = 0;
            table4.AddCell(fCell3);

            PdfPCell fCell4 = new PdfPCell(new Phrase(_dRpt.Address1 + "\n" +_dRpt.City +", " +_dRpt.State +"\n" +_dRpt.Zip, _Font12Font));
            fCell4.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell4.Border = 0;
            table4.AddCell(fCell4);

            PdfPCell fCell21 = new PdfPCell(new Phrase("Signed for by:", _Font12BoldFont));
            fCell21.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell21.Border = 0;
            table4.AddCell(fCell21);

            PdfPCell fCell22 = new PdfPCell(new Phrase("Signature not required", _Font12Font));
            fCell22.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell22.Border = 0;
            table4.AddCell(fCell22);

            PdfPCell fCell23 = new PdfPCell(new Phrase("Delivery Date:", _Font12BoldFont));
            fCell23.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell23.Border = 0;
            table4.AddCell(fCell23);

            PdfPCell fCell24 = new PdfPCell(new Phrase(_dRpt.DDate, _Font12Font));
            fCell24.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell24.Border = 0;
            table4.AddCell(fCell24);

            PdfPCell fCell31 = new PdfPCell(new Phrase("Service Type:", _Font12BoldFont));
            fCell31.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell31.Border = 0;
            table4.AddCell(fCell31);

            PdfPCell fCell32 = new PdfPCell(new Phrase("Standard",_Font12Font));
            fCell32.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell32.Border = 0;
            table4.AddCell(fCell32);

            PdfPCell fCell33 = new PdfPCell(new Phrase("", _Font12BoldFont));
            fCell33.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell33.Border = 0;
            table4.AddCell(fCell33);

            PdfPCell fCell34 = new PdfPCell(new Phrase("", _Font12Font));
            fCell34.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell34.Border = 0;
            table4.AddCell(fCell34);


            doc.Add(table4);

            Paragraph p3 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p3);


            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk("Shipping Information:"));

            Paragraph p4 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p4);


            PdfPTable tableShippingInfo = new PdfPTable(4);
            tableShippingInfo.HorizontalAlignment = Element.ALIGN_LEFT;
            // PdfPCell cell1 = new PdfPCell();
            tableShippingInfo.WidthPercentage = 100;
            float[] widthsShipInfo = new float[] { 50f, 75f, 75f, 60f };
            tableShippingInfo.SetWidths(widthsShipInfo);

            tableShippingInfo.DefaultCell.Border = Rectangle.NO_BORDER;


            PdfPCell SfCell1 = new PdfPCell(new Phrase("Tracking Number", _Font12BoldFont));
            SfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell1.Border = 0;
            tableShippingInfo.AddCell(SfCell1);

            PdfPCell SfCell2 = new PdfPCell(new Phrase(_dRpt.Confirmation, _Font12Font));
            SfCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell2.Border = 0;
            tableShippingInfo.AddCell(SfCell2);

            PdfPCell SfCell3 = new PdfPCell(new Phrase("Ship Date:", _Font12BoldFont));
            SfCell3.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell3.Border = 0;
            tableShippingInfo.AddCell(SfCell3);

            PdfPCell SfCell4 = new PdfPCell(new Phrase(_dRpt.TDate, _Font12Font));
            SfCell4.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell4.Border = 0;
            tableShippingInfo.AddCell(SfCell4);



            PdfPCell SfCell21 = new PdfPCell(new Phrase("", _Font12BoldFont));
            SfCell21.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell21.Border = 0;
            tableShippingInfo.AddCell(SfCell21);

            PdfPCell SfCell22 = new PdfPCell(new Phrase("", _Font12Font));
            SfCell22.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell22.Border = 0;
            tableShippingInfo.AddCell(SfCell22);

            PdfPCell SfCell23 = new PdfPCell(new Phrase("", _Font12BoldFont)); //Weight
            SfCell23.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell23.Border = 0;
            tableShippingInfo.AddCell(SfCell23);

            PdfPCell SfCell24 = new PdfPCell(new Phrase("", _Font12Font));//_dRpt.Weight
            SfCell24.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell24.Border = 0;
            tableShippingInfo.AddCell(SfCell24);


            var SfCell31 = new PdfPCell(new Phrase("Recipient:\n  " + _dRpt.Name +"\n  "+_dRpt.Address1 +"\n  "+_dRpt.City +", "+_dRpt.State+"\n  "+_dRpt.Zip, _Font12BoldFont));
           SfCell31.Colspan = 2;
            SfCell31.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell31.Border = 0;
            tableShippingInfo.AddCell(SfCell31);



            //PdfPCell SfCell32 = new PdfPCell(new Phrase("", _Font12Font));
            //SfCell32.HorizontalAlignment = Element.ALIGN_LEFT;
            //SfCell32.Border = 0;
            //tableShippingInfo.AddCell(SfCell32);


            PdfPCell SfCell33 = new PdfPCell(new Phrase("Shipper:\n  J & B Medical Supply Inc.\n  Shipping Department \n  50486 Pontiac Trail\n  Wixom, MI 48393", _Font12BoldFont));
            //SfCell33.Colspan = 2;
            SfCell33.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell33.Border = 0;
            tableShippingInfo.AddCell(SfCell33);
            

            PdfPCell SfCell34 = new PdfPCell(new Phrase("", _Font12Font));
            SfCell34.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell34.Border = 0;
            tableShippingInfo.AddCell(SfCell34);

            doc.Add(tableShippingInfo);

            Paragraph p5 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p5);


            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk("Reference Information:"));

            Paragraph p6 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p6);

            PdfPTable tableRefInfo = new PdfPTable(2);
            tableRefInfo.HorizontalAlignment = Element.ALIGN_LEFT;
            // PdfPCell cell1 = new PdfPCell();
            tableRefInfo.WidthPercentage = 100;
            float[] widthsRefInfo = new float[] { 70f,70f };
            tableRefInfo.SetWidths(widthsRefInfo);

            tableRefInfo.DefaultCell.Border = Rectangle.NO_BORDER;


            PdfPCell RfCell1 = new PdfPCell(new Phrase("J & B WorkOrder Number", _Font12BoldFont));
            RfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell1.Border = 0;
            tableRefInfo.AddCell(RfCell1);


            PdfPCell RfCell3 = new PdfPCell(new Phrase(_dRpt.WorkOrderId.ToString(), _Font12Font));
            RfCell3.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell3.Border = 0;
            tableRefInfo.AddCell(RfCell3);

         


            PdfPCell RfCell21 = new PdfPCell(new Phrase("J & B Customer Account Number", _Font12BoldFont));
            RfCell21.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell21.Border = 0;
            tableRefInfo.AddCell(RfCell21);

          

            PdfPCell RfCell23 = new PdfPCell(new Phrase(_dRpt.Acccount.ToString(), _Font12Font));
            RfCell23.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell23.Border = 0;
            tableRefInfo.AddCell(RfCell23);

            doc.Add(tableRefInfo);
           
        }
        private void GenerateFEDEXPODInfo(iTextSharp.text.Document doc, DetailReport _dRpt)
        {
            doc.NewPage();
            _dRpt.DStatus = "Delivered";
            _dRpt.Zip = _dRpt.Zip.Length > 5 ? _dRpt.Zip.Substring(0, 5) : _dRpt.Zip;

            //iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance("C:\\images" + "\\fedEx.jpg");

            //jpg1.ScaleToFit(100, 80);
            //PdfPCell imageCell = new PdfPCell(jpg1);
            //imageCell.Colspan = 1; 
            //imageCell.Border = 0;

            //imageCell.VerticalAlignment = Element.ALIGN_TOP;


            //PdfPTable table3 = new PdfPTable(1);
            //table3.HorizontalAlignment = Element.ALIGN_LEFT;


            //table3.WidthPercentage = 40;
            //float[] widths3 = new float[] { 80f };

            //table3.SetWidths(widths3);

            //table3.DefaultCell.Border = Rectangle.NO_BORDER;


            //table3.AddCell(imageCell);


            //  doc.Add(table3);

            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font11Font, new Chunk("\n\n\n"));
         this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font11Font, new Chunk(_dRpt.Invoice_date + "\n\n"));
            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font11Font, new Chunk("Dear Customer:\n\n"));
            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font11Font, new Chunk("The Following is the proof of delivery for tracking number:" + _dRpt.Confirmation + "\n\n"));

            Paragraph p1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p1);


            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font11BoldFont, new Chunk("Delivery Information:"));

            Paragraph p2 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p2);


            PdfPTable table4 = new PdfPTable(4);
            table4.HorizontalAlignment = Element.ALIGN_LEFT;
         
            table4.WidthPercentage = 100;
            float[] widths4 = new float[] { 60f, 60f, 60f, 80f };
            table4.SetWidths(widths4);

            table4.DefaultCell.Border = Rectangle.NO_BORDER;


            PdfPCell fCell1 = new PdfPCell(new Phrase("Status:", _Font12BoldFont));
            fCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell1.Border = 0;
            table4.AddCell(fCell1);

            PdfPCell fCell2 = new PdfPCell(new Phrase(_dRpt.DStatus, _Font12Font));
            fCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell2.Border = 0;
            table4.AddCell(fCell2);

            PdfPCell fCell3 = new PdfPCell(new Phrase("Delivery Location:", _Font12BoldFont));
            fCell3.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell3.Border = 0;
            table4.AddCell(fCell3);

            PdfPCell fCell4 = new PdfPCell(new Phrase(_dRpt.Address1 + "\n" + _dRpt.City + ", " + _dRpt.State +  _dRpt.Zip, _Font12Font));
            fCell4.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell4.Border = 0;
            table4.AddCell(fCell4);

            PdfPCell fCell21 = new PdfPCell(new Phrase("Signed for by:", _Font12BoldFont));
            fCell21.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell21.Border = 0;
            table4.AddCell(fCell21);

            PdfPCell fCell22 = new PdfPCell(new Phrase("Signature not required", _Font12Font));
            fCell22.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell22.Border = 0;
            table4.AddCell(fCell22);

            PdfPCell fCell23 = new PdfPCell(new Phrase("Delivery Date:", _Font11BoldFont));
            fCell23.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell23.Border = 0;
            table4.AddCell(fCell23);

            PdfPCell fCell24 = new PdfPCell(new Phrase(_dRpt.DDate, _Font11Font));
            fCell24.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell24.Border = 0;
            table4.AddCell(fCell24);

            PdfPCell fCell31 = new PdfPCell(new Phrase("Service Type:", _Font11BoldFont));
            fCell31.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell31.Border = 0;
            table4.AddCell(fCell31);

            PdfPCell fCell32 = new PdfPCell(new Phrase("FedEx Home Delivery", _Font11Font));
            fCell32.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell32.Border = 0;
            table4.AddCell(fCell32);

            PdfPCell fCell33 = new PdfPCell(new Phrase("", _Font11BoldFont));
            fCell33.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell33.Border = 0;
            table4.AddCell(fCell33);

            PdfPCell fCell34 = new PdfPCell(new Phrase("", _Font11Font));
            fCell34.HorizontalAlignment = Element.ALIGN_LEFT;
            fCell34.Border = 0;
            table4.AddCell(fCell34);


            doc.Add(table4);
            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font11Font, new Chunk("\n\nNO SIGNATURE REQUIRED"));
            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font10Font, new Chunk("Proof-of-delivery details appear below; however, no signature is available for this FedEx Ground shipment because a signature was not required."));

            Paragraph p3 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p3);


            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font11BoldFont, new Chunk("Shipping Information:"));

            Paragraph p4 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            doc.Add(p4);


            PdfPTable tableShippingInfo = new PdfPTable(4);
            tableShippingInfo.HorizontalAlignment = Element.ALIGN_LEFT;
            // PdfPCell cell1 = new PdfPCell();
            tableShippingInfo.WidthPercentage = 100;
            float[] widthsShipInfo = new float[] { 60f, 75f, 75f, 60f };
            tableShippingInfo.SetWidths(widthsShipInfo);

            tableShippingInfo.DefaultCell.Border = Rectangle.NO_BORDER;


            PdfPCell SfCell1 = new PdfPCell(new Phrase("Tracking Number", _Font11BoldFont));
            SfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell1.Border = 0;
            tableShippingInfo.AddCell(SfCell1);

            PdfPCell SfCell2 = new PdfPCell(new Phrase(_dRpt.Confirmation, _Font11Font));
            SfCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell2.Border = 0;
            tableShippingInfo.AddCell(SfCell2);

            PdfPCell SfCell3 = new PdfPCell(new Phrase("Ship Date:", _Font11BoldFont));
            SfCell3.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell3.Border = 0;
            tableShippingInfo.AddCell(SfCell3);

            PdfPCell SfCell4 = new PdfPCell(new Phrase(_dRpt.TDate, _Font11Font));
            SfCell4.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell4.Border = 0;
            tableShippingInfo.AddCell(SfCell4);



            PdfPCell SfCell21 = new PdfPCell(new Phrase("", _Font11BoldFont));
            SfCell21.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell21.Border = 0;
            tableShippingInfo.AddCell(SfCell21);

            PdfPCell SfCell22 = new PdfPCell(new Phrase("", _Font11Font));
            SfCell22.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell22.Border = 0;
            tableShippingInfo.AddCell(SfCell22);

            PdfPCell SfCell23 = new PdfPCell(new Phrase("Weight", _Font11BoldFont));
            SfCell23.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell23.Border = 0;
            tableShippingInfo.AddCell(SfCell23);

            PdfPCell SfCell24 = new PdfPCell(new Phrase(_dRpt.Weight + " lb", _Font11Font));
            SfCell24.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell24.Border = 0;
            tableShippingInfo.AddCell(SfCell24);

            PdfPCell SfCell31 = new PdfPCell(new Phrase("Recipient", _Font11BoldFont));
            SfCell31.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell31.Border = 0;
            tableShippingInfo.AddCell(SfCell31);

            PdfPCell SfCell32 = new PdfPCell(new Phrase("", _Font11Font));
            SfCell32.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell32.Border = 0;
            tableShippingInfo.AddCell(SfCell32);

            PdfPCell SfCell33 = new PdfPCell(new Phrase("Shipper:", _Font11BoldFont));
            SfCell33.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell33.Border = 0;
            tableShippingInfo.AddCell(SfCell33);

            PdfPCell SfCell34 = new PdfPCell(new Phrase("", _Font11Font));
            SfCell34.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell34.Border = 0;
            tableShippingInfo.AddCell(SfCell34);

            PdfPCell SfCell41 = new PdfPCell(new Phrase(_dRpt.Name   + "\n" + _dRpt.Address1 + "\n" + _dRpt.City + ", " + _dRpt.State + _dRpt.Zip + " US", _Font11Font));
            SfCell41.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell41.Border = 0;
            tableShippingInfo.AddCell(SfCell41);

            PdfPCell SfCell42 = new PdfPCell(new Phrase("", _Font11Font));
            SfCell42.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell42.Border = 0;
            tableShippingInfo.AddCell(SfCell42);

            PdfPCell SfCell43 = new PdfPCell(new Phrase("Shipping Department\n50486 Pontiac Trail\nWixom, MI 48393 US", _Font11Font));
            SfCell43.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell43.Border = 0;
            tableShippingInfo.AddCell(SfCell43);

            PdfPCell SfCell44 = new PdfPCell(new Phrase("", _Font11Font));
            SfCell44.HorizontalAlignment = Element.ALIGN_LEFT;
            SfCell44.Border = 0;
            tableShippingInfo.AddCell(SfCell44);




            //var SfCell31 = new PdfPCell(new Phrase("\n\nRecipient:\n", _Font11BoldFont));
            //SfCell31.Colspan = 2;
            //SfCell31.HorizontalAlignment = Element.ALIGN_LEFT;
            //SfCell31.Border = 0;
            //tableShippingInfo.AddCell(SfCell31);


            //PdfPCell SfCell32 = new PdfPCell(new Phrase("\n\n", _Font11Font));
            //SfCell32.HorizontalAlignment = Element.ALIGN_LEFT;
            //SfCell32.Border = 0;
            //tableShippingInfo.AddCell(SfCell32);


            //PdfPCell SfCell33 = new PdfPCell(new Phrase("\n\nShipper", _Font11BoldFont));
            ////SfCell33.Colspan = 2;
            //SfCell33.HorizontalAlignment = Element.ALIGN_LEFT;
            //SfCell33.Border = 0;
            //tableShippingInfo.AddCell(SfCell33);


            //PdfPCell SfCell34 = new PdfPCell(new Phrase("\n\n", _Font11Font));
            //SfCell34.HorizontalAlignment = Element.ALIGN_LEFT;
            //SfCell34.Border = 0;
            //tableShippingInfo.AddCell(SfCell34);

            //var SfCell41 = new PdfPCell(new Phrase(_dRpt.Name + "\n  " + _dRpt.Name + "\n  " + _dRpt.Address1 + "\n  " + _dRpt.City + ", " + _dRpt.State + _dRpt.Zip + " US", _Font11Font));
            //SfCell41.Colspan = 2;
            //SfCell41.HorizontalAlignment = Element.ALIGN_LEFT;
            //SfCell41.Border = 0;
            //tableShippingInfo.AddCell(SfCell41);

            //PdfPCell SfCell42 = new PdfPCell(new Phrase("", _Font11Font));
            //SfCell42.HorizontalAlignment = Element.ALIGN_LEFT;
            //SfCell42.Border = 0;
            //tableShippingInfo.AddCell(SfCell42);




            //PdfPCell SfCell43 = new PdfPCell(new Phrase("Shipping Department\n  Shipping Department \n  50486 Pontiac Trail\n  Wixom, MI 48393 US", _Font11Font));
            //SfCell43.HorizontalAlignment = Element.ALIGN_LEFT;
            //SfCell43.Border = 0;
            //tableShippingInfo.AddCell(SfCell43);

            //PdfPCell SfCell44 = new PdfPCell(new Phrase("", _Font11Font));
            //SfCell44.HorizontalAlignment = Element.ALIGN_LEFT;
            //SfCell44.Border = 0;
            //tableShippingInfo.AddCell(SfCell44);

            doc.Add(tableShippingInfo);

            //    Paragraph p5 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            //  doc.Add(p5);


            //  this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12Font, new Chunk("Reference Information:"));

            //   Paragraph p6 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 0)));
            //  doc.Add(p6);
         //   this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font12BoldFont, new Chunk("Reference"));

            PdfPTable tableRefInfo = new PdfPTable(2);
            tableRefInfo.HorizontalAlignment = Element.ALIGN_LEFT;
            // PdfPCell cell1 = new PdfPCell();
            tableRefInfo.WidthPercentage = 100;
            float[] widthsRefInfo = new float[] { 70f, 70f };
            tableRefInfo.SetWidths(widthsRefInfo);

            tableRefInfo.DefaultCell.Border = Rectangle.NO_BORDER;


            PdfPCell RfCell1 = new PdfPCell(new Phrase("\n\nReference", _Font11BoldFont));
            RfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell1.Border = 0;
            tableRefInfo.AddCell(RfCell1);


            PdfPCell RfCell3 = new PdfPCell(new Phrase("\n\n"+_dRpt.WorkOrderId.ToString(), _Font11Font));
            RfCell3.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell3.Border = 0;
            tableRefInfo.AddCell(RfCell3);




            PdfPCell RfCell21 = new PdfPCell(new Phrase("Purchase order number:", _Font11BoldFont));
            RfCell21.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell21.Border = 0;
            tableRefInfo.AddCell(RfCell21);



            PdfPCell RfCell23 = new PdfPCell(new Phrase(_dRpt.Acccount.ToString(), _Font11Font));
            RfCell23.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell23.Border = 0;
            tableRefInfo.AddCell(RfCell23);

            PdfPCell RfCell31 = new PdfPCell(new Phrase("Invoice Number", _Font11BoldFont));
            RfCell31.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell31.Border = 0;
            tableRefInfo.AddCell(RfCell31);



            PdfPCell RfCell32 = new PdfPCell(new Phrase(_dRpt.Invoice_number, _Font11Font));
            RfCell32.HorizontalAlignment = Element.ALIGN_LEFT;
            RfCell32.Border = 0;
            tableRefInfo.AddCell(RfCell32);

            doc.Add(tableRefInfo);
            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font10Font, new Chunk("\n\n Thank you for choosing FedEx."));

            this.AddParagraph(doc, iTextSharp.text.Element.ALIGN_LEFT, _Font11BoldFont, new Chunk("\n\n\nData provided by Fedex"));

        }
        //public ActionResult FedEx_Index()
        //{
        //    MainVMFedEx vm = new MainVMFedEx();


        //    return View(vm);
        //}

        //[HttpPost]
        //public ActionResult FedEx_Index(MainVMFedEx vm)
        //{

        //    vm.dataList = FedExDB.GetReportFedEx(vm.trackNum);

        //    //  if (vm.dataList.Count == 0)
        //    //  vm.dataList = null;

        //    return View(vm);

        //}

        //public ActionResult MainMethodFedEx(string _pId)
        //{
        //    DetailReport _dRpt = FedExDB.GetDataFedEX(_pId);

        //    return PartialView("_MainMethodFedEx", _dRpt);

        //}
        private void AddParagraph(Document doc, int alignment, iTextSharp.text.Font font, iTextSharp.text.IElement content)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.SetLeading(0f, 1.2f);
            paragraph.Alignment = alignment;
            paragraph.Font = font;
            paragraph.Add(content);
            doc.Add(paragraph);


        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Demo1()
        {
            return View();
        }
        public ActionResult Demo()
        {
            return View();
        }


        public ActionResult GenManuPdf(string _pId)
        {
            DetailReport _dRpt = new DetailReport();

            //For FedEx pod
            _dRpt.Confirmation = "056766148875748";
            _dRpt.Address1 = "3195 U S HWY 41";
            _dRpt.City = "MOHAWK";
            _dRpt.State = "MI";
            _dRpt.Zip = "";
            _dRpt.DDate = "Jan 05,2015 02:26";
            _dRpt.TDate = "Jan 07,2015";
            _dRpt.Weight = "39.7 lbs/18 kg";
            _dRpt.Name = "WENDY GENTRY";
            _dRpt.WorkOrderId = 4138743;
            _dRpt.Acccount = 193056;

             BuildPdfFedEXPOD(_dRpt);
          //  BuildManuPdf(_dRpt);
          //  ShowManuPdf(_dRpt);
            return View();

        }

        public void BuildManuPdf(DetailReport _dRpt)
        {

            string Date = DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString();
            iTextSharp.text.Document doc = null;

            string filename = _dRpt.Confirmation + "_" + _dRpt.WorkOrderId.ToString() + ".pdf";

            doc = new Document();

            _dRpt.FileExists = null;

            string path = "C://POD_Docs//USPS_Pdf" + "//";
            if (System.IO.File.Exists(path + filename))
            {
                _dRpt.FileExists = true;
            }
            else
            {

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc,
                    new System.IO.FileStream(path + filename,
                        System.IO.FileMode.Create));
                _dRpt.FileExists = false;

                // Set the margins and page size
                this.SetStandardPageSize(doc);

                // Add metadata to the document.  This information is visible when viewing the 
                // document properities within Adobe Reader.
                doc.AddTitle("Report");

                // Add Xmp metadata to the document.
                this.CreateXmpMetadata(writer);

                // Open the document for writing content
                doc.Open();

                this.GenerateUSPSInfo(doc, _dRpt);
                this.SetStandardPageSize(doc);  // Reset the margins and page size

                doc.Close();
                doc = null;

            }

        }

        private void ShowManuPdf(DetailReport _dRpt)
        {
            string Date = DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString();

            string filename = _dRpt.Confirmation + "_" + _dRpt.WorkOrderId.ToString() + ".pdf";

            _dRpt.FileExists = null;

            string path = "C://POD_Docs//USPS_Pdf" + "//";

            string FilePath = path + filename;
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath);
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
            }
        }

        //------------------------------

        //------------------------------
    }
}