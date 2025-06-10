using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Malshinon.DataBase
{
    public class SQLConnection
    {
        public  string NameDB;
        private string connStr;

        public SQLConnection(string nameDB)
        {
            NameDB = nameDB;
            connStr = $"server=localhost;user=root;password=;database={NameDB}";
        }

        

        public MySqlConnection OpenConnect()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                return conn;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public void CloseConnection(MySqlConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

    }
}
