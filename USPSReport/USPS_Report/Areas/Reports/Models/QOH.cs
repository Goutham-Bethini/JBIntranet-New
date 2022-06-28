using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;

namespace USPS_Report.Areas.Reports.Models
{
    public class QOH
    {

        public static void getQOHOracle()
        {
            try
            {
                string _conn = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                Oracle.ManagedDataAccess.Client.OracleConnection myConnection = new Oracle.ManagedDataAccess.Client.OracleConnection(_conn);
                myConnection.Open();
                string Query = @"select Available_qty from XXJBM_ITEM_ONHAND_QUANTITY where HDMS_ITEM ='ONE550'";
                int availableQty = 0;
                Oracle.ManagedDataAccess.Client.OracleCommand cmd = new Oracle.ManagedDataAccess.Client.OracleCommand(Query, myConnection);
                availableQty = cmd.ExecuteScalar() != null ? string.IsNullOrEmpty(cmd.ExecuteScalar().ToString()) ? 0 : (int)cmd.ExecuteScalar() : 0;
                myConnection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //DataSet ds = null;
            //OleDbDataAdapter adapter;

            //try
            //{
            //    myConnection.Open();
            //    ////Stored procedure calling. It is already in sample db.
            //    cmd.CommandText = "proc=get_atr_quantity";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Connection = myConnection;

            //    OleDbParameter paramReturnValue = new OleDbParameter();
            //    paramReturnValue.ParameterName = "Qty_Available2";
            //    paramReturnValue.OleDbType = OleDbType.Integer;
            //    paramReturnValue.Direction = ParameterDirection.Output;

            //    OleDbParameter paramA = new OleDbParameter();
            //    paramA.ParameterName = "opqProductCode";
            //    paramA.OleDbType = OleDbType.VarChar;
            //    paramA.Direction = ParameterDirection.Input;
            //    paramA.Value = "ONE550";

            //    cmd.Parameters.Add(paramReturnValue);
            //    cmd.Parameters.Add(paramA);

            //    cmd.ExecuteNonQuery();

            //    int returnValue = (int)cmd.Parameters["Qty_Available2"].Value;

            //  //  Output.Items.Add(returnValue);

            //    myConnection.Close();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}

        }

    }
}