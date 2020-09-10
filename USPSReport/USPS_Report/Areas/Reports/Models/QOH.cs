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
            string _conn = ConfigurationManager.ConnectionStrings["EntitiesOracle"].ConnectionString;
            OleDbConnection myConnection = new OleDbConnection(_conn);
            OleDbCommand cmd = new OleDbCommand();

            DataSet ds = null;
            OleDbDataAdapter adapter;

            try
            {
                myConnection.Open();
                ////Stored procedure calling. It is already in sample db.
                cmd.CommandText = "proc=get_atr_quantity";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = myConnection;

                OleDbParameter paramReturnValue = new OleDbParameter();
                paramReturnValue.ParameterName = "Qty_Available2";
                paramReturnValue.OleDbType = OleDbType.Integer;
                paramReturnValue.Direction = ParameterDirection.Output;

                OleDbParameter paramA = new OleDbParameter();
                paramA.ParameterName = "opqProductCode";
                paramA.OleDbType = OleDbType.VarChar;
                paramA.Direction = ParameterDirection.Input;
                paramA.Value = "ONE550";

                cmd.Parameters.Add(paramReturnValue);
                cmd.Parameters.Add(paramA);

                cmd.ExecuteNonQuery();

                int returnValue = (int)cmd.Parameters["Qty_Available2"].Value;

              //  Output.Items.Add(returnValue);

                myConnection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
       
    }
}