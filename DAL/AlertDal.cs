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
    public static class AlertDal
    {
        static MySqlConnection conn;
        static SQLConnection SqlConnection = new SQLConnection("malshinon");

        public static void Add(Alert alert)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"INSERT INTO alerts 
                            (reporter_id, target_id, reason) 
                            VALUES (@reporter_id, @target_id, @reason)";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@reporter_id", alert.ReporterId);
                cmd.Parameters.AddWithValue("@target_id", alert.TargetId);
                cmd.Parameters.AddWithValue("@reason", alert.Reason);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] AddAlert: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }
    }
}

