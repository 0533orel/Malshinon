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
        public static void Add(People people)
        {
            try
            {
                conn = SQLConnection.OpenConnect();
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
                Console.WriteLine("the people added successfully.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SQLConnection.CloseConnection(conn);
            }
        }





        public static int GetIdBySecretCode(string secretCode)
        {
            try
            {
                conn = SQLConnection.OpenConnect();
                string Query = @"SELECT p.id FROM people p WHERE p.secret_code = @secretCode";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@secretCode", secretCode);
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt32("id");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                SQLConnection.CloseConnection(conn);
            }
        }




        public static int GetIdByFullName(string firstName, string lastMane)
        {
            try
            {
                conn = SQLConnection.OpenConnect();
                string Query = @"SELECT p.id FROM people p WHERE p.first_name = @firstName AND p.last_name = lastMane";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastMane", lastMane);
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt32("id");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                SQLConnection.CloseConnection(conn);
            }
        }






        public static void UpdateSecretCode(int id, string secretCode)
        {
            try
            {
                conn = SQLConnection.OpenConnect();
                string Query = @"UPDATE people SET secret_code = @secretCode WHERE @id = id";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@secretCode", secretCode);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SQLConnection.CloseConnection(conn);
            }
        }





        public static void UpdateNumReports(int id)
        {
            try
            {
                conn = SQLConnection.OpenConnect();
                string Query = @"UPDATE people SET num_reports = num_reports + 1 WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SQLConnection.CloseConnection(conn);
            }
        }





        public static void UpdateNumMentions(int id)
        {
            try
            {
                conn = SQLConnection.OpenConnect();
                string Query = @"UPDATE people SET num_mentions = num_mentions + 1 WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SQLConnection.CloseConnection(conn);
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
                conn = SQLConnection.OpenConnect();
                string Query = @"UPDATE people SET type = choseType  WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@types", choseType[chosenType]);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SQLConnection.CloseConnection(conn);
            }
        }
    }
}
