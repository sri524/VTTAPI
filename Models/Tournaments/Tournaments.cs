using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace VTTAPI.Models.Tournaments
{
    public class Tournament
    {
        public char action { get; set; }
        public int T_ID { get; set; }
        public string T_INSTALLATION_ID { get; set; }
        public string T_NAME { get; set; }
        public int T_LIVE_TAGS { get; set; }
        public string T_START_DATE { get; set; }
        public string T_START_TIME { get; set; }
        public string T_END_DATE { get; set; }
        public string T_END_TIME { get; set; }
        public int T_NO_OF_ROUNDS { get; set; }
        public int T_ROUND_LENGTH { get; set; }
        public int T_U_ID { get; set; }
        public char T_STATUS { get; set; }

        public static string[] CUDTournamentActions(Tournament TInfo)
        {
            string[] strResult = new string[2];
            string con = Common.DABasics.ConnectionString();
            SqlConnection conn = new SqlConnection(con);
            try
            {
                using (SqlCommand SqlCommnad = new SqlCommand("USP_ADM_SAVE_TOURNAMENTS_INFO", conn))
                {
                    SqlCommnad.CommandType = CommandType.StoredProcedure;
                    SqlCommnad.Parameters.AddWithValue("@ACTION", Convert.ToChar(TInfo.action));
                    SqlCommnad.Parameters.AddWithValue("@T_NAME", TInfo.T_NAME);
                    SqlCommnad.Parameters.AddWithValue("@T_INSTALLATION_ID", TInfo.T_INSTALLATION_ID);
                    SqlCommnad.Parameters.AddWithValue("@T_START_DATE", TInfo.T_START_DATE);
                    SqlCommnad.Parameters.AddWithValue("@T_END_DATE", TInfo.T_END_DATE);
                    SqlCommnad.Parameters.AddWithValue("@T_START_TIME", TInfo.T_START_TIME);
                    SqlCommnad.Parameters.AddWithValue("@T_END_TIME", TInfo.T_END_TIME);
                    SqlCommnad.Parameters.AddWithValue("@T_NO_OF_ROUNDS", TInfo.T_NO_OF_ROUNDS);
                    SqlCommnad.Parameters.AddWithValue("@T_U_ID", TInfo.T_U_ID);
                    SqlCommnad.Parameters.AddWithValue("@T_ROUND_LENGTH", Convert.ToInt32(TInfo.T_ROUND_LENGTH));
                    SqlCommnad.Parameters.AddWithValue("@T_STATUS", Convert.ToChar(TInfo.T_STATUS));
                    SqlParameter outputPara = new SqlParameter();
                    outputPara.ParameterName = "@ERROR_ID";
                    outputPara.Direction = System.Data.ParameterDirection.Output;
                    outputPara.SqlDbType = System.Data.SqlDbType.VarChar;
                    outputPara.Size = 20;
                    SqlCommnad.Parameters.Add(outputPara);

                    SqlParameter outputPara1 = new SqlParameter();
                    if (TInfo.T_ID == 0)
                    {
                        outputPara1.ParameterName = "@T_ID";
                        outputPara1.Direction = System.Data.ParameterDirection.Output;
                        outputPara1.SqlDbType = System.Data.SqlDbType.Int;
                        SqlCommnad.Parameters.Add(outputPara1);
                    }
                    else
                    {
                        SqlCommnad.Parameters.AddWithValue("@T_ID", TInfo.T_ID);
                    }

                    conn.Open();
                    SqlCommnad.ExecuteNonQuery();
                    int return_id;
                    if (TInfo.action == 'A') return_id = Convert.ToInt32(outputPara1.Value);
                    else return_id = Convert.ToInt32(TInfo.T_ID);

                    int return_errid = Convert.ToInt32(outputPara1.Value);
                    strResult[0] = Convert.ToString(return_id);
                    strResult[1] = Convert.ToString(outputPara.Value) == "1" ? "SUCCESS" : Convert.ToString(outputPara.Value) == "-2" ? "Already Exist" : "FAIL";
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                strResult[0] = Convert.ToString(0);
                strResult[1] = ex.Message;
            }
            return strResult;

        }
        public static string getTournaments(string searchString)
        {
            string retString = string.Empty;
            searchString = searchString == null ? "" : searchString;
            DataTable dt = new DataTable();
            string constr = Common.DABasics.ConnectionString(); ;
            try
            {
                dt = Common.DABasics.RetriveSpData("USP_ADM_GET_TOURNAMENTS", searchString);
                if (dt.Rows.Count > 0)
                {
                    var obj = new { tournaments = dt };
                    retString = JsonConvert.SerializeObject(obj);
                }
            }
            catch (Exception ex)
            {
                retString = ex.ToString();
            }
            return retString;
        }
        public static string getGolfCourse()
        {
            string retString = string.Empty;
            string constr = Common.DABasics.sentinelConnectionString();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(constr))
                using (SqlCommand cmd = new SqlCommand("SELECT Id,Name FROM GolfCourses", sqlConn))
                {
                    sqlConn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        var obj = new { courses = dt };
                        retString = JsonConvert.SerializeObject(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                retString = ex.Message.ToString();
            }
            return retString;
        }
        public static string getCourseHoles(string courseId)
        {
            string retString = string.Empty;
            string constr = Common.DABasics.sentinelConnectionString();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(constr))
                using (SqlCommand cmd = new SqlCommand("SELECT Id,Name FROM [dbo].[GolfHoles] WHERE CourseId ='" + courseId + "'", sqlConn))
                {
                    sqlConn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        var obj = new { holes = dt };
                        retString = JsonConvert.SerializeObject(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                retString = ex.Message.ToString();
            }
            return retString;
        }
        public static string getCourseTags(string courseId)
        {
            string retString = string.Empty;
            string constr = Common.DABasics.sentinelConnectionString();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(constr))
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name  FROM [dbo].[Tags] WHERE Installation_Id = (SELECT InstallationId FROM [dbo].[GolfCourses] WHERE ID ='" + courseId + "')", sqlConn))
                {
                    sqlConn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        var obj = new { tags = dt };
                        retString = JsonConvert.SerializeObject(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                retString = ex.Message.ToString();
            }
            return retString;
        }
    }
    public class tournamentdetailschedule
    {
        public char action { set; get; }
        public int id { get; set; }
        public int clubid { get; set; }
        public string courseid { get; set; }
        public int userid { get; set; }
        public char isregistereduser { get; set; }
        public int tournamentid { get; set; }
        public int scheduleid { get; set; }
        public int golferposition { get; set; }
        public string cartid { get; set; }
        public int scoregroupid { get; set; }
        public string cartname { get; set; }
        public string holename { get; set; }
        public string holeid { get; set; }
        public char frontnine { get; set; }
        public string username { get; set; }
        public char type { get; set; }
        public char status { get; set; }
        //public string createddate { get; set; }
        public char gamestatus { get; set; }

        public static string[] SaveTournamentdetailschedule(List<tournamentdetailschedule> tournamentdetails)
        {
            string[] msg = new string[2];
            string conn = Common.DABasics.ConnectionString();
            SqlConnection con = new SqlConnection(conn);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(tournamentdetails);
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
            int scorecardgroupid = 0; int i = 0; string cartid = "0";
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    using (SqlCommand SqlCommnad = new SqlCommand("USP_ADM_SAVE_TOURNAMENT_DETAILS_SCHEDULE", con))
                    {
                        int? id = Convert.ToInt32(dr["id"]);
                        if (Convert.ToChar(dr["action"]) == 'A' && i == 0 && Convert.ToInt32(dr["scoregroupid"]) == 0)
                        {
                            scorecardgroupid = 0;
                        }
                        else if (Convert.ToInt32(dr["scoregroupid"]) != 0)
                        {
                            scorecardgroupid = Convert.ToInt32(dr["scoregroupid"]);
                        }
                        if (cartid == Convert.ToString(dr["cartid"]) && i > 0)
                        {
                            scorecardgroupid = Convert.ToInt32(scorecardgroupid);
                        }
                        else if (i != 0) { scorecardgroupid = 0; }
                        SqlCommnad.CommandType = CommandType.StoredProcedure;
                        SqlCommnad.Parameters.AddWithValue("@ACTION", Convert.ToChar(dr["action"]));
                        SqlCommnad.Parameters.AddWithValue("@IZUID", Convert.ToInt32(dr["userid"]));
                        SqlCommnad.Parameters.AddWithValue("@GCBID", Convert.ToInt32(dr["clubid"]));
                        SqlCommnad.Parameters.AddWithValue("@INSTALLATIONID", Convert.ToString(dr["courseid"]));
                        SqlCommnad.Parameters.AddWithValue("@TAGID", Convert.ToString(dr["cartid"]));
                        SqlCommnad.Parameters.AddWithValue("@HOLEID", Convert.ToString(dr["holeid"]));
                        SqlCommnad.Parameters.AddWithValue("@SCHEDULEID", Convert.ToInt32(dr["scheduleid"]));
                        SqlCommnad.Parameters.AddWithValue("@TOURNAMENTID", Convert.ToInt32(dr["tournamentid"]));
                        SqlCommnad.Parameters.AddWithValue("@GOLFERPOSITION", Convert.ToInt32(dr["golferposition"]));
                        SqlCommnad.Parameters.AddWithValue("@TAGLIVESTATUS", 'N');
                        SqlCommnad.Parameters.AddWithValue("@FRONTNINE", Convert.ToChar(dr["frontnine"]));
                        SqlCommnad.Parameters.AddWithValue("@NAME", Convert.ToString(dr["username"]));
                        SqlCommnad.Parameters.AddWithValue("@TYPE", Convert.ToChar(dr["type"]));
                        SqlCommnad.Parameters.AddWithValue("@ISREGISTEREDUSER", Convert.ToChar(dr["isregistereduser"]));
                        SqlCommnad.Parameters.AddWithValue("@STATUS", Convert.ToChar(dr["status"]));

                        SqlParameter outputPara = new SqlParameter();
                        if (Convert.ToChar(dr["action"]) == 'A')
                        {
                            outputPara.ParameterName = "@ID";
                            outputPara.Direction = System.Data.ParameterDirection.Output;
                            outputPara.SqlDbType = System.Data.SqlDbType.Int;
                            SqlCommnad.Parameters.Add(outputPara);
                        }
                        else
                        {
                            SqlCommnad.Parameters.AddWithValue("@ID", id);
                        }
                        SqlParameter outputPara1 = new SqlParameter();
                        outputPara1.ParameterName = "@ERROR_ID";
                        outputPara1.Direction = System.Data.ParameterDirection.Output;
                        outputPara1.SqlDbType = System.Data.SqlDbType.Int;
                        SqlCommnad.Parameters.Add(outputPara1);

                        SqlParameter outputPara2 = new SqlParameter("@SCORECARDGROUPID", scorecardgroupid);
                        outputPara2.ParameterName = "@SCORECARDGROUPID";
                        outputPara2.Direction = System.Data.ParameterDirection.InputOutput;
                        outputPara2.SqlDbType = System.Data.SqlDbType.Int;
                        SqlCommnad.Parameters.Add(outputPara2);

                        con.Open();
                        SqlCommnad.ExecuteNonQuery();
                        int return_id;
                        if (Convert.ToChar(dr["action"]) == 'A') return_id = Convert.ToInt32(outputPara.Value);
                        else return_id = Convert.ToInt32(id);

                        int return_errid = Convert.ToInt32(outputPara1.Value);
                        scorecardgroupid = Convert.ToInt32(outputPara2.Value);

                        cartid = Convert.ToString(dr["cartid"]);

                        msg[0] = Convert.ToString(return_id);
                        msg[1] = Convert.ToString(outputPara1.Value) == "1" ? "SUCCESS" : Convert.ToString(outputPara1.Value) == "-2" ? "Already Exist" : "FAIL"; ;
                        con.Close();
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    msg[0] = "0";
                    msg[1] = ex.Message;

                }

            }
            return msg;
        }
        public static string gettournamentscheduledetails(int tournamentid, string courseId, int scorecardGroupId)
        {
            DataTable dt = new DataTable();
            string retString = "";
            string conn = Common.DABasics.ConnectionString();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                using (cmd = new SqlCommand("USP_ADM_GET_TOURNAMENT_DETAILS_SCHEDULE", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@INSTALLATIONID", SqlDbType.VarChar));
                    cmd.Parameters["@INSTALLATIONID"].Value = courseId;
                    cmd.Parameters.Add(new SqlParameter("@TOURNAMENTID", SqlDbType.Int));
                    cmd.Parameters["@TOURNAMENTID"].Value = tournamentid;
                    cmd.Parameters.Add(new SqlParameter("@SCORECARDGROUPID", SqlDbType.Int));
                    cmd.Parameters["@SCORECARDGROUPID"].Value = scorecardGroupId;

                    cmd.ExecuteNonQuery();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        var obj = new { tournamentdetails = dt };
                        retString = JsonConvert.SerializeObject(obj);
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                retString = ex.Message.ToString();
                con.Close();
            }
            return retString;
        }
    }
}
