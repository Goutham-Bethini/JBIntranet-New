using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class ProductivityReport
    {
        public class ProductivityReportVM
        {
            public DateTime SelectedDate { get; set; }
            public DateTime PreDate { get; set; }
            public DateTime NextDate { get; set; }
            public IList<ProductivityReportData> Details { get; set; }
            public int WorkOrdersTotal { get; set; }
            public int PackagesTotal { get; set; }
            public decimal PtotalByWOtotal { get; set; }
        }
        public class ProductivityReportFromDB
        {
            public DateTime DateShipped { get; set; }
            public string Meter_Number { get; set; }
            public int USERID { get; set; }
            public int WorkOrders { get; set; }
            public int Packages { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        public class ProductivityReportData
        {
            public string User { get; set; }
            public string Meter_Number { get; set; }
            public string Station { get; set; }
            public string FirstScan { get; set; }
            public string LastScan { get; set; }
            public string TimeElapsed { get; set; }
            public int WorkOrders { get; set; }
            public int Packages { get; set; }
            public decimal PackagesByOrder { get; set; }
            public decimal WorkOrdersByHour { get; set; }
            public decimal PackagesByHour { get; set; }
        }
        public static ProductivityReportVM GetDetails(DateTime selectedDate)
        {
            ProductivityReportVM productivityReportVM = new ProductivityReportVM();
            productivityReportVM.SelectedDate = selectedDate;
            productivityReportVM.PreDate = selectedDate.AddDays(-1);
            productivityReportVM.NextDate = selectedDate.AddDays(1);
            string _conn = ConfigurationManager.ConnectionStrings["ColdFusionReportsEntitiesOracle"].ToString();
            //string _conn = @"Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = nchljandbdb01.jandbmedical.com)(PORT = 1541))
            //                (CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = JBTA1)));Provider=OraOLEDB.Oracle;User Id= XXCUST01;Password=XXCUST01;";        
            OracleConnection myConnection = new OracleConnection(_conn);
            String query = string.Empty;
            myConnection.Open();
            query = @"SELECT
            	DateShipped,
            	Meter_Number,
            	--UserID,
            	(CASE  
                             WHEN NVL(USERID,999)=999 THEN to_number('84')
                         ELSE to_number(USERID)
                         END) as USERID,
            	COUNT(DISTINCT ID_WORKORDER) AS WorkOrders,
            	COUNT(DISTINCT ConfirmationNumber) AS Packages,
            	MIN(DateProcessed) AS StartTime,
            	MAX(DateProcessed) AS EndTime
            FROM TBL_UPS_WORKORDERS 
            WHERE 
            		--USERID IS NOT NULL AND 
            		DateShipped = TO_DATE('" + selectedDate.ToShortDateString() + "', 'MM/DD/YYYY') "
                + "AND CancelDate IS NULL "
            + "GROUP BY "
                + "DateShipped,"
                + "Meter_Number,"
                + "UserID "
            + "ORDER BY "
                + "UserID,"
                + "Meter_Number";
            OracleCommand myCommand = new OracleCommand(query, myConnection);
            OracleDataReader reader = myCommand.ExecuteReader();
            ProductivityReportFromDB productivityReportFromDB;
            List<ProductivityReportFromDB> lstProductivityReportFromDB = new List<ProductivityReportFromDB>();
            while (reader.Read())
            {
                productivityReportFromDB = new ProductivityReportFromDB();
                productivityReportFromDB.DateShipped = Convert.ToDateTime(reader.GetValue(0).ToString());
                productivityReportFromDB.Meter_Number = reader.GetValue(1).ToString();
                productivityReportFromDB.USERID = Convert.ToInt32(reader.GetValue(2).ToString());
                productivityReportFromDB.WorkOrders = Convert.ToInt32(reader.GetValue(3).ToString());
                productivityReportFromDB.Packages = Convert.ToInt32(reader.GetValue(4).ToString());
                productivityReportFromDB.StartTime = Convert.ToDateTime(reader.GetValue(5).ToString());
                productivityReportFromDB.EndTime = Convert.ToDateTime(reader.GetValue(6).ToString());
                lstProductivityReportFromDB.Add(productivityReportFromDB);
            }
            if (lstProductivityReportFromDB.Count > 0)
            {
                List<ProductivityReportData> lstProductivityReportData = new List<ProductivityReportData>();
                ProductivityReportData productivityReportData;
                foreach (ProductivityReportFromDB item in lstProductivityReportFromDB)
                {
                    productivityReportData = new ProductivityReportData();
                    using (USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities())
                    {
                        productivityReportData.User = _db.FedExLogins.Where(i => i.felID == item.USERID).Select(i => i.felName).FirstOrDefault();
                        productivityReportData.Meter_Number = item.Meter_Number;
                        var fmnName = _db.FedExMeterNumbers.Where(i => i.fmnMeter_Number == item.Meter_Number).Select(i => i.fmnName).FirstOrDefault();
                        if (fmnName != null && fmnName != "")
                        {
                            productivityReportData.Station = fmnName;
                        }
                        else
                        {
                            productivityReportData.Station = item.Meter_Number;
                        }
                        productivityReportData.FirstScan = item.StartTime.ToString("MM/dd/yyyy h:mm t");
                        productivityReportData.LastScan = item.EndTime.ToString("MM/dd/yyyy h:mm t");
                        TimeSpan span = (item.EndTime - item.StartTime);
                        int hours = span.Hours;
                        int minutes = span.Minutes;
                        decimal forDiv = hours + decimal.Round((Convert.ToDecimal(minutes) / 60), 2, MidpointRounding.AwayFromZero);
                        productivityReportData.TimeElapsed = String.Format("{0} hours, {1} minutes", hours, minutes);
                        productivityReportData.WorkOrders = item.WorkOrders;
                        productivityReportData.Packages = item.Packages;
                        productivityReportData.PackagesByOrder = decimal.Round((Convert.ToDecimal(item.Packages) / item.WorkOrders), 2, MidpointRounding.AwayFromZero);
                        productivityReportData.WorkOrdersByHour = decimal.Round((item.WorkOrders / forDiv), 2, MidpointRounding.AwayFromZero);
                        productivityReportData.PackagesByHour = decimal.Round((item.Packages / forDiv), 2, MidpointRounding.AwayFromZero);
                        lstProductivityReportData.Add(productivityReportData);
                    }
                }
                productivityReportVM.Details = lstProductivityReportData;
                productivityReportVM.WorkOrdersTotal = lstProductivityReportData.Sum(i => i.WorkOrders);
                productivityReportVM.PackagesTotal = lstProductivityReportData.Sum(i => i.Packages);
                productivityReportVM.PtotalByWOtotal = decimal.Round((Convert.ToDecimal(productivityReportVM.PackagesTotal) / productivityReportVM.WorkOrdersTotal), 2, MidpointRounding.AwayFromZero);
            }
            return productivityReportVM;
        }

        public class StationUpdateVM
        {
            public string MeterNumber { get; set; }
            [Required]
            public string Name { get; set; }
            public IList<StationData> Details { get; set; }
        }

        public class StationData
        {
            public string MeterNumber { get; set; }
            public string StationName { get; set; }
            public DateTime? Updated { get; set; }
            public string UpdatedBy { get; set; }
        }
        public static StationUpdateVM UpdateStationDetails(StationUpdateVM stationUpdateVM, string userName)
        {
            try
            {              
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities())
                {
                    var res = _db.FedExMeterNumbers.Where(i => i.fmnMeter_Number == stationUpdateVM.MeterNumber).FirstOrDefault();
                    if(res!=null)
                    {                        
                        res.fmnName = stationUpdateVM.Name;
                        res.fmnUpdated = DateTime.Now;
                        res.fmnUpdatedBy = userName;                        
                        _db.SaveChanges();
                    }
                    else
                    {
                        FedExMeterNumber fedExMeterNumber = new FedExMeterNumber();
                        fedExMeterNumber.fmnMeter_Number = stationUpdateVM.MeterNumber;
                        fedExMeterNumber.fmnName = stationUpdateVM.Name;
                        fedExMeterNumber.fmnUpdated = DateTime.Now;
                        fedExMeterNumber.fmnUpdatedBy = userName;
                        _db.FedExMeterNumbers.Add(fedExMeterNumber);
                        _db.SaveChanges();
                    }
                }
                StationUpdateVM locStationUpdateVM = new StationUpdateVM();
                locStationUpdateVM.MeterNumber = stationUpdateVM.MeterNumber;
                locStationUpdateVM.Name = stationUpdateVM.Name;
                string _conn = ConfigurationManager.ConnectionStrings["ColdFusionReportsEntitiesOracle"].ToString();
                OracleConnection myConnection = new OracleConnection(_conn);
                String query = string.Empty;
                myConnection.Open();
                query = @"SELECT DISTINCT Meter_Number
				FROM TBL_UPS_WORKORDERS
				WHERE Meter_Number IS NOT NULL
				ORDER BY Meter_Number";
                OracleCommand myCommand = new OracleCommand(query, myConnection);
                OracleDataReader reader = myCommand.ExecuteReader();
                List<string> lstMeter_Number = new List<string>();
                while (reader.Read())
                {
                    lstMeter_Number.Add(reader.GetValue(0).ToString());
                }
                List<StationData> lstStationData = new List<StationData>();
                if (lstMeter_Number.Count > 0)
                {
                    foreach (string s in lstMeter_Number)
                    {
                        using (USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFuionHHSQLDBEntities())
                        {
                            var item = _db.FedExMeterNumbers.Where(i => i.fmnMeter_Number == s).Select(i => new StationData
                            {
                                MeterNumber = i.fmnMeter_Number,
                                StationName = i.fmnName,
                                Updated = i.fmnUpdated,
                                UpdatedBy = i.fmnUpdatedBy
                            }).FirstOrDefault();
                            if (item != null)
                            {
                                lstStationData.Add(item);
                            }
                        }
                    }
                }            
                locStationUpdateVM.Details = lstStationData;
                return locStationUpdateVM;
            }
            catch (Exception ex)
            {
                return new StationUpdateVM();
            }
        }
    }
}