using Malshinon.DataBase;
using Malshinon.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.DAL
{
    public static class IntelReportsDAL 
    {
        static MySqlConnection conn;
        static SQLConnection SqlConnection = new SQLConnection("malshinon");


        public static void Add(IntelReports intelReports)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"INSERT INTO intelreports 
                            (reporter_id, target_id, text, timestamp) 
                            VALUES (@reporter_id, @target_id, @text, @timestamp)";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@reporter_id", intelReports.ReporterId);
                cmd.Parameters.AddWithValue("@target_id", intelReports.TargetId);
                cmd.Parameters.AddWithValue("@text", intelReports.Text);
                cmd.Parameters.AddWithValue("@timestamp", intelReports.Timestamp);
                cmd.ExecuteNonQuery();
                Console.WriteLine("The intel reports added successfully.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] Add: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static string GetText(int reporterId)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string query = "SELECT i.text FROM intelreports i WHERE i.reporter_id = @reporterId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@reporterId", reporterId);
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetString("text");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetText: {ex.Message}");
                return null;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }

        
    }
}
