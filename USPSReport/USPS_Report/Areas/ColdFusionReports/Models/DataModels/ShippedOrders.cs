using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class ShippedOrders
    {
        public class ShippedOrdersVM 
        {
            public int Total { get; set; }
            public IList<ShippedOrdersData> Details { get; set; }
            public IList<MonthlyTotals> MonthlyTotalsDetails { get; set; }
            public IList<DailyAvg> DailyAvgDetails { get; set; }
        }
        public class TaskViewModel : ISchedulerEvent
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public bool IsAllDay { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string StartTimezone { get; set; }
            public string EndTimezone { get; set; }
            public string RecurrenceRule { get; set; }
            public string RecurrenceException { get; set; }
            public int? chk { get; set; }
        }
        public class ShippedOrdersData : ISchedulerEvent
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public bool IsAllDay { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string StartTimezone { get; set; }
            public string EndTimezone { get; set; }
            public string RecurrenceRule { get; set; }
            public string RecurrenceException { get; set; }
            public string Location { get; set; }
        }

        public class MonthlyTotals
        {
            public string Name { get; set; }
            public int Qty { get; set; }            
        }

        public class DailyAvg
        {
            public string Name { get; set; }
            public int Qty { get; set; }
        }

        public static ShippedOrdersVM GetShippedOrders(DateTime fromDate, DateTime toDate)
        {            
            ShippedOrdersVM shippedOrdersVM = new ShippedOrdersVM();
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetShippedOrders", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
                        command.Parameters.AddWithValue("@from", fromDate);
                        command.Parameters.AddWithValue("@to", toDate);
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(ds);
                        DataTable dt = new DataTable();
                        DataTable dt2 = new DataTable();
                        DataTable dt3 = new DataTable();
                        dt = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        dt3 = ds.Tables[2];
                        ShippedOrdersData shippedOrdersData;
                        List<ShippedOrdersData> lstShippedOrdersData = new List<ShippedOrdersData>();
                        if (dt!=null&& dt.Rows.Count>1)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                shippedOrdersData = new ShippedOrdersData();
                                shippedOrdersData.Title = dr[1].ToString() + ": " + dr[2].ToString();
                                shippedOrdersData.Start = Convert.ToDateTime(dr[3].ToString());
                                shippedOrdersData.End = Convert.ToDateTime(dr[3].ToString());
                                lstShippedOrdersData.Add(shippedOrdersData);
                            }
                        }
                        MonthlyTotals monthlyTotals;
                        List<MonthlyTotals> lstMonthlyTotals = new List<MonthlyTotals>();
                        if (dt2 != null && dt2.Rows.Count > 1)
                        {
                            foreach (DataRow dr in dt2.Rows)
                            {
                                monthlyTotals = new MonthlyTotals();
                                monthlyTotals.Name = dr[1].ToString();
                                monthlyTotals.Qty = Convert.ToInt32(dr[2].ToString());
                                lstMonthlyTotals.Add(monthlyTotals);
                            }
                        }
                        DailyAvg dailyAvg;
                        List<DailyAvg> lstDailyAvg = new List<DailyAvg>();
                        if (dt3 != null && dt3.Rows.Count > 1)
                        {
                            foreach (DataRow dr in dt3.Rows)
                            {
                                dailyAvg = new DailyAvg();
                                dailyAvg.Name = dr[1].ToString();
                                dailyAvg.Qty = Convert.ToInt32(dr[2].ToString());
                                lstDailyAvg.Add(dailyAvg);
                            }
                        }
                        shippedOrdersVM.Details = lstShippedOrdersData;
                        shippedOrdersVM.MonthlyTotalsDetails = lstMonthlyTotals;
                        shippedOrdersVM.DailyAvgDetails = lstDailyAvg;                        
                    }
                }            
                return shippedOrdersVM;
            }
            catch (Exception ex)
            {
                return new ShippedOrdersVM();
            }
        }
    }
}