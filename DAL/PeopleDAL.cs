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
    public class PeopleDAL
    {
        static MySqlConnection conn;
        public static void AddPeople(People people)
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
