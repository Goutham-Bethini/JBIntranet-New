using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.Entity.SqlServer;
using USPS_Report.Models;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

namespace USPS_Report.Areas.Reports.Models
{
    public class QOHReport
    {
        public IList<QOHDetails> GetDetails(string ProductCode)
        {
            IList<QOHDetails> _list = new List<QOHDetails>();
            QOHDetails data = new QOHDetails();
            setQOHDataQuery(data, ProductCode, _list);
            return _list;
        }

        private void setQOHDataQuery(QOHDetails data, string ProductCode, IList<QOHDetails> _list)
        {
            string sql = @"SELECT *
				FROM tbl_product_table
				WHERE ProductCode='@[ProductCode]'
					AND DeletedDate IS NULL";
            string sqlQtyBo = @"SELECT SUM(linQty) AS QtyBO
							FROM
										ERP_OrdersSent		wo
								JOIN	ERP_OrderLines		lin	ON linWoID=woID
								JOIN	tbl_ps_workorder	wos	ON wos.ID=woWorkOrder
							WHERE
									linProductCode='@[ProductCode]'
								AND	woSent IS NULL
								AND woBO IS NOT NULL
								AND wos.Cancel_date IS NULL
								AND wos.Completed_date IS NULL
								AND wos.LastPrintDate IS NULL";
            sql = sql.Replace("@[ProductCode]", ProductCode);
            sqlQtyBo = sqlQtyBo.Replace("@[ProductCode]", ProductCode);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                var ProductCodedata = _db.Database.SqlQuery<QOHDetails>(sql).FirstOrDefault();
                if (ProductCodedata != null)
                {
                    data.ProductCode = ProductCodedata.ProductCode;
                    data.Discontinued = ProductCodedata.Discontinued;
                    data.ProductDescription = ProductCodedata.ProductDescription;
                    var QtyBodata = _db.Database.SqlQuery<QOHDetails>(sqlQtyBo).FirstOrDefault();
                    data.QtyBO = QtyBodata.QtyBO;
                    setQOHOracleDataQuery(data, ProductCode);
                    _list.Add(data);
                }

            }
        }

        private void setQOHOracleDataQuery(QOHDetails data, string ProductCode)
        {
            try
            {
                string _conn = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                Oracle.ManagedDataAccess.Client.OracleConnection myConnection = new Oracle.ManagedDataAccess.Client.OracleConnection(_conn);
                myConnection.Open();
                string Query = @"select Available_qty from XXJBM_ITEM_ONHAND_QUANTITY where HDMS_ITEM ='" + ProductCode + "'";
                //int availableQty = 0;
                Oracle.ManagedDataAccess.Client.OracleCommand cmd = new Oracle.ManagedDataAccess.Client.OracleCommand(Query, myConnection);
                data.Qty_Available2 = cmd.ExecuteScalar() != null ? string.IsNullOrEmpty(cmd.ExecuteScalar().ToString()) ? 0 : (int)cmd.ExecuteScalar() : 0;
                myConnection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //string _conn = ConfigurationManager.ConnectionStrings["EntitiesOracle2"].ConnectionString;
            //OleDbConnection myConnection = new OleDbConnection(_conn);
            //OleDbCommand cmd = new OleDbCommand();

            //try
            //{
            //    myConnection.Open();
            //    cmd.CommandText = "jbm_inv_pkg.get_atr_quantity";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Connection = myConnection;

            //    OleDbParameter paramA = new OleDbParameter();
            //    paramA.ParameterName = "opqProductCode";
            //    paramA.OleDbType = OleDbType.VarChar;
            //    paramA.Direction = ParameterDirection.Input;
            //    paramA.Value = ProductCode;

            //    OleDbParameter paramReturnValue = new OleDbParameter();
            //    paramReturnValue.ParameterName = "Qty_Available2";
            //    paramReturnValue.OleDbType = OleDbType.Integer;
            //    paramReturnValue.Direction = ParameterDirection.Output;



            //    cmd.Parameters.Add(paramA);
            //    cmd.Parameters.Add(paramReturnValue);


            //    cmd.ExecuteNonQuery();

            //    data.Qty_Available2 = (int)cmd.Parameters["Qty_Available2"].Value;

            //    myConnection.Close();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }


    }

    public class QOHDetails
    {
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int Qty_Available2 { get; set; }
        public bool Discontinued { get; set; }
        public string QtyBO { get; set; }
        public int QtyBOToInt
        {
            get
            {
                if (QtyBO == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt16(QtyBO);
                }
            }
        }

    }

    public class QOHSearchDataDetails
    {
        public string ProductCode { get; set; }
        public bool FieldIsDiabled
        {
            get
            {
                if (string.IsNullOrEmpty(ProductCode))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public IList<QOHDetails> QOHDetails { get; set; }

    }

}




