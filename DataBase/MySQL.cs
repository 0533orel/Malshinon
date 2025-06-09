using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Malshinon.DataBase
{
    public static class SQLConnection
    {
        static string connStr = "server=localhost;user=root;password=;database=malshinon";
        public static MySqlConnection conn;

        public static MySqlConnection OpenConnect()
        {
            if (conn == null)
                conn = new MySqlConnection(connStr);

            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            return conn;
        }

        public static void CloseConnection()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
                conn = null;
            }
        }

    }
}
