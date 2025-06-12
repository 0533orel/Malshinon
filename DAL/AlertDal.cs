using Malshinon.DataBase;
using Malshinon.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
                            (target_id, created_at, reason) 
                    VALUES (@TargetId, @CreatedAt, @Reason)";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@TargetId", alert.TargetId);
                cmd.Parameters.AddWithValue("@CreatedAt", alert.CreatedAt);
                cmd.Parameters.AddWithValue("@Reason", alert.Reason);
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


        public static void PrintAlerts(DateTime start, DateTime end)
        {
            MySqlConnection conn = null;
            try
            {
                conn = SqlConnection.OpenConnect();
                string query = @"SELECT p.id AS target_id, p.first_name, p.last_name, a.created_at, a.reason FROM alerts a
                                JOIN people p ON a.target_id = p.id
                                WHERE a.created_at BETWEEN @start AND @end";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int targetId = reader.GetInt32("target_id");
                    string firstName = reader.GetString("first_name");
                    string lastName = reader.GetString("last_name");
                    DateTime createdAt = reader.GetDateTime("created_at");
                    string reason = reader.GetString("reason");

                    Console.WriteLine($"Target ID: {targetId} | Name: {firstName} {lastName} | Time: {createdAt} | Reason: {reason}");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] PrintAlerts: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }

    }
}

