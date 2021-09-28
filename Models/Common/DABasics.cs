using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace VTTAPI.Models.Common
{
    public class DABasics
    {
        public static string ConnectionString()
        {
            string connection = @"Data Source=20.85.226.78;Initial Catalog=DEV_VANTAGETAG;Persist Security Info=True;User ID=vantagetag;Password=VaTag@2k21#;Pooling=False;Min Pool Size=0;Max Pool Size=10000;";
            return connection;
        }
        public static string sentinelConnectionString()
        {
            string connection = @"Data Source=20.85.226.78;Initial Catalog=SentinelData;Persist Security Info=True;User ID=vantagetag;Password=VaTag@2k21#;Pooling=False;Min Pool Size=0;Max Pool Size=10000;";
            return connection;
        }
        public static DataTable RetriveSpData(string strProcedurerName, string searchString)
        {
            string conn = DABasics.ConnectionString();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            string sp_name = "";
            SqlParameter Param1;
            DataTable dt = new DataTable();
            try
            {
                sp_name = strProcedurerName;

                cmd = new SqlCommand(sp_name, con);
                cmd.CommandType = CommandType.StoredProcedure;

                Param1 = new SqlParameter("@expression", SqlDbType.VarChar, 450);
                Param1.Value = searchString;
                cmd.Parameters.Add(Param1);

                SqlDataAdapter objDA = new SqlDataAdapter(cmd);
                objDA.Fill(dt);
            }
            catch (Exception ex) { }
            return dt;
        }
    }
}
