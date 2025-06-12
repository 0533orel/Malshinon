using Malshinon.DataBase;
using Malshinon.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] AddIntelReports: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static List<string> GetTextByReporterId(int reporterId)
        {
            try
            {
                List<string> texts = new List<string>();
                conn = SqlConnection.OpenConnect();
                string query = "SELECT i.text FROM intelreports i WHERE i.reporter_id = @reporterId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@reporterId", reporterId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    texts.Add(reader.GetString("text"));
                }
                return texts;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetTextByReporterId: {ex.Message}");
                return null;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static List<string> GetTextByTargetId(int targetId)
        {
            try
            {
                List<string> texts = new List<string>();
                conn = SqlConnection.OpenConnect();
                string query = "SELECT i.text FROM intelreports i WHERE i.target_id = @targetId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@targetId", targetId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    texts.Add(reader.GetString("text"));
                }
                return texts;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetTextByTargetId: {ex.Message}");
                return null;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static void GetAllInfoReporter(int reporterId)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string query = @"SELECT p.first_name, p.last_name, p.secret_code, p.type, p.num_reports, i.text, i.timestamp
                                FROM intelreports i JOIN people p ON i.reporter_id = @reporterId AND p.id = @reporterId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@reporterId", reporterId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string firstName = reader.GetString("first_name");
                    string lastName = reader.GetString("last_name");
                    int secretCode = reader.GetInt32("secret_code");
                    string type = reader.GetString("type");
                    int numReports = reader.GetInt32("num_reports");
                    string text = reader.GetString("text");
                    DateTime timestamp = reader.GetDateTime("timestamp");
                    Console.WriteLine($"name: {firstName} {lastName}: secret code: {secretCode}. type: {type}. num reports: {numReports} \n" +
                        $"text: {text} at time: {timestamp}");
                    Console.WriteLine();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetAllInfoReporter: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static void GetAllInfoMention(int mentionId)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string query = @"SELECT p.first_name, p.last_name, p.secret_code, p.type, p.num_mentions, i.text, i.timestamp
                                FROM intelreports i JOIN people p ON i.reporter_id = @mentionId AND p.id = @mentionId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@mentionId", mentionId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string firstName = reader.GetString("first_name");
                    string lastName = reader.GetString("last_name");
                    int secretCode = reader.GetInt32("secret_code");
                    string type = reader.GetString("type");
                    int numMentions = reader.GetInt32("num_mentions");
                    string text = reader.GetString("text");
                    DateTime timestamp = reader.GetDateTime("timestamp");
                    Console.WriteLine($"name: {firstName} {lastName}: secret code: {secretCode}. type: {type}. num reports: {numMentions} \n" +
                        $"text: {text} at time: {timestamp}");
                    Console.WriteLine();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetAllInfoMention: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static DateTime? GetDateTime(int mentionId)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"SELECT i.timestamp FROM intelreports i WHERE i.target_id = @mentionId";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@mentionId", mentionId);
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetDateTime("timestamp");
            }
            catch (MySqlException ex)
            {
                //Console.WriteLine($"[ERROR] GetNumMentions: {ex.Message}");
                return null;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static bool ThereIsThreeReports(int targetId, DateTime dateTime)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string query = @"SELECT i.timestamp 
                                FROM intelreports i
                                WHERE target_id = @targetId AND i.timestamp BETWEEN @startFrom AND @dateTime";
                DateTime startFrom = dateTime.AddMinutes(-15);
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@targetId", targetId);
                cmd.Parameters.AddWithValue("@startFrom", startFrom);
                cmd.Parameters.AddWithValue("@dateTime", dateTime);
                var reader = cmd.ExecuteReader();
                int countReports = 0;
                while (reader.Read())
                {
                    countReports++;
                }
                if (countReports >= 3)
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
                return false;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static void Delete(int id)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string query = "DELETE FROM intelreports WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] Delete: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static void PrintDangouresTargets()
        {
            MySqlConnection conn = null;
            try
            {
                conn = SqlConnection.OpenConnect();

                string query = @"
            SELECT 
                p.id, 
                p.first_name, 
                p.last_name, 
                p.secret_code, 
                p.num_mentions
            FROM 
                people p
            WHERE 
                p.type IN ('dangerous target', 'dangerous target and reporter');";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    int counter = 1;
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string firstName = reader.GetString("first_name");
                        string lastName = reader.GetString("last_name");
                        string secretCode = reader.GetString("secret_code");
                        int numMentions = reader.GetInt32("num_mentions");

                        Console.WriteLine($"\nTarget number {counter}");
                        Console.WriteLine($"ID           : {id}");
                        Console.WriteLine($"Name         : {firstName} {lastName}");
                        Console.WriteLine($"Secret Code  : {secretCode}");
                        Console.WriteLine($"Mentions     : {numMentions}\n");
                        counter++;
                    }
                }
                else
                    Console.WriteLine($"\nThere are no dangerous targets\n");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] PrintAgens: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


    }
}
