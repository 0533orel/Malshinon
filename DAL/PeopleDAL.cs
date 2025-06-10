using Malshinon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Malshinon.DataBase;

namespace Malshinon.DAL
{
    public static class PeopleDAL
    {
        static MySqlConnection conn;
        static SQLConnection SqlConnection = new SQLConnection("malshinon");
        public static void Add(People people)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"INSERT INTO people 
                            (first_name, last_name, secret_code, type, num_reports, num_mentions) 
                            VALUES (@first_name, @last_name, @secret_code, @type, @num_reports, @num_mentions)";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@first_name", people.FirstName);
                cmd.Parameters.AddWithValue("@last_name", people.LastName);
                cmd.Parameters.AddWithValue("@secret_code", people.SecretCode);
                cmd.Parameters.AddWithValue("@type", people.Type);
                cmd.Parameters.AddWithValue("@num_reports", people.NumReports);
                cmd.Parameters.AddWithValue("@num_mentions", people.NumMentions);
                cmd.ExecuteNonQuery();
                Console.WriteLine("The people added successfully.");
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


        public static int GetIdBySecretCode(string secretCode)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"SELECT p.id FROM people p WHERE p.secret_code = @secretCode";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@secretCode", secretCode);
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt32("id");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetIdBySecretCode: {ex.Message}");
                return -1;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static int GetIdByFullName(string firstName, string lastNane)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"SELECT p.id FROM people p WHERE p.first_name = @firstName AND p.last_name = lastNane";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastNane", lastNane);
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt32("id");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetIdByFullName: {ex.Message}");
                return -1;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static void UpdateSecretCode(int id, string secretCode)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"UPDATE people SET secret_code = @secretCode WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@secretCode", secretCode);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] UpdateSecretCode: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static void UpdateNumReports(int reporterId)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"UPDATE people SET num_reports = num_reports + 1 WHERE id = @reporterId";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@reporterId", reporterId);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] UpdateNumReports: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static void UpdateNumMentions(int MentionsId)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"UPDATE people SET num_mentions = num_mentions + 1 WHERE id = @MentionsId";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@MentionsId", MentionsId);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] UpdateNumMentions: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static void UpdateType(int id, int chosenType)
        {
            try
            {
                Dictionary<int, string> choseType = new Dictionary<int, string>
                {
                    {1, "reporter" },
                    {2, "target" },
                    {3, "both" },
                    {4, "potential agent" }
                };
                conn = SqlConnection.OpenConnect();
                string Query = @"UPDATE people SET type = @choseType  WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@choseType", choseType[chosenType]);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] UpdateType: {ex.Message}");
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static People GetPeople(int id)
        {
            try
            {
                People people = null;
                conn = SqlConnection.OpenConnect();
                string query = "SELECT * FROM people WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    people = new People
                    {
                        FirstName = reader.GetString("first_name"),
                        LastName = reader.GetString("last_name"),
                        SecretCode = reader.GetString("secret_code"),
                        Type = reader.GetString("type"),
                        NumReports = reader.GetInt32("num_reports"),
                        NumMentions = reader.GetInt32("num_mentions")
                    };
                }
                return people;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetPeople: {ex.Message}");
                return null;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static string GetFullName(int id)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string query = @"SELECT p.first_name, p.last_name
                                 FROM people p WHERE p.id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                reader.Read();
                string fullName = reader.GetString("first_name") + " " + reader.GetString("last_name");
                return fullName;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetFullName: {ex.Message}");
                return null;
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
                string query = "DELETE FROM people WHERE id = @id";
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
    }
}
