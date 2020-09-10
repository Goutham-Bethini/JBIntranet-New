using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class BackOrders
    {
        public class BackOrdersVM
        {
            public IList<ProductsData> ProductsDetails { get; set; }
            public IList<OrdersData> OrdersDetails { get; set; }
            public int OrdersCount { get; set; }

        }
        public class ProductsData
        {
            public string HDMSproductCode { get; set; }
            public string OracleProductCode { get; set; }
            public string ETAdate { get; set; }
            public int Orders { get; set; }
            public int QtyRequested { get; set; }
            public string HDMSUOM { get; set; }
            public int QtyAvailable { get; set; }
            public string OracDFFUOM { get; set; }

        }
        public class OrdersData
        {
            public int Account { get; set; }
            public int Order { get; set; }
            public string RequestDate { get; set; }
            public string Product { get; set; }
            public int QtyOrdered { get; set; }

        }
        public static BackOrdersVM GetBackOrdersData()
        {
            BackOrdersVM backOrdersVM = new BackOrdersVM();           
            try
            {
                var conns = System.Configuration.ConfigurationManager.ConnectionStrings["ColdFusionReportsDbconnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(conns))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_GetBackOrders", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandTimeout = 0;
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
                        ProductsData productsData;
                        List<ProductsData> lstProductsData = new List<ProductsData>();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                productsData = new ProductsData();
                                productsData.HDMSproductCode = dr[0].ToString();
                                productsData.OracleProductCode = dr[1].ToString();
                                productsData.ETAdate = dr[2].ToString()!=""? Convert.ToDateTime(dr[2].ToString()).ToShortDateString(): dr[2].ToString(); 
                                productsData.Orders = Convert.ToInt32(dr[3].ToString());
                                productsData.QtyRequested = Convert.ToInt32(dr[4].ToString());
                                productsData.HDMSUOM = dr[5].ToString();
                                productsData.QtyAvailable = Convert.ToInt32(dr[6].ToString());
                                productsData.OracDFFUOM = dr[7].ToString();
                                lstProductsData.Add(productsData);
                            }
                        }
                        OrdersData ordersData;
                        List<OrdersData> lstOrdersData = new List<OrdersData>();
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt2.Rows)
                            {
                                ordersData = new OrdersData();
                                ordersData.Account = Convert.ToInt32(dr[0].ToString());
                                ordersData.Order = Convert.ToInt32(dr[1].ToString());
                                ordersData.RequestDate = dr[2].ToString() != "" ? Convert.ToDateTime(dr[2].ToString()).ToShortDateString() : dr[2].ToString(); 
                                ordersData.Product = dr[3].ToString();
                                ordersData.QtyOrdered = Convert.ToInt32(dr[4].ToString());

                                lstOrdersData.Add(ordersData);
                            }
                        }
                        backOrdersVM.ProductsDetails = lstProductsData;
                        backOrdersVM.OrdersDetails = lstOrdersData;
                        backOrdersVM.OrdersCount = Convert.ToInt32(dt3.Rows[0][0].ToString());
                    }
                }
                return backOrdersVM;
            }
            catch (Exception ex)
            {
                return new BackOrdersVM();
            }            
        }
    }
}