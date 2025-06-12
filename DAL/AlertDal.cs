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


        public static void PrintAlerts()
        {
            try
            {
                conn = SqlConnection.OpenConnect();

                string query = @"
            SELECT
                p.id AS target_id,
                p.first_name,
                p.last_name,
                p.secret_code,
                p.TYPE,
                p.num_mentions,
                a.created_at,
                a.reason
            FROM
                alerts a
            JOIN people p ON
                a.target_id = p.id
            ORDER BY
                a.created_at DESC;";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                
                int counter = 1;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int targetId = reader.GetInt32("target_id");
                        string firstName = reader.GetString("first_name");
                        string lastName = reader.GetString("last_name");
                        string secretCode = reader.GetString("secret_code");
                        string type = reader.GetString("type");
                        int numMentions = reader.GetInt32("num_mentions");
                        DateTime createdAt = reader.GetDateTime("created_at");
                        string reason = reader.GetString("reason");

                        Console.WriteLine($"\nAlert number {counter}");
                        Console.WriteLine($"Target ID    : {targetId}");
                        Console.WriteLine($"Name         : {firstName} {lastName}");
                        Console.WriteLine($"Secret Code  : {secretCode}");
                        Console.WriteLine($"Type         : {type}");
                        Console.WriteLine($"Reports      : {numMentions}");
                        Console.WriteLine($"Created At   : {createdAt}");
                        Console.WriteLine($"Reason       : {reason}\n");
                        counter++;
                    }
                }
                else 
                    Console.WriteLine($"\nThere are no notifications\n");

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

