﻿using Malshinon.DataBase;
using Malshinon.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] AddPeople: {ex.Message}");
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
                //Console.WriteLine($"[ERROR] GetIdBySecretCode: {ex.Message}");
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
                string Query = @"SELECT p.id FROM people p WHERE p.first_name = @firstName AND p.last_name = @lastNane";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastNane", lastNane);
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt32("id");
            }
            catch (MySqlException ex)
            {
                //Console.WriteLine($"[ERROR] GetIdByFullName: {ex.Message}");
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


        public static void UpdateType(int id, int keyType)
        {
            try
            {
                Dictionary<int, string> types = new Dictionary<int, string>
                {
                    {1, "reporter" },
                    {2, "target" },
                    {3, "both" },
                    {4, "potential agent" },
                    {5, "agent" },
                    {6, "dangerous target" },
                    {7, "dangerous target and reporter" }
                };
                conn = SqlConnection.OpenConnect();
                string Query = @"UPDATE people SET type = @type  WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@type", types[keyType]);
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


        public static People GetPeopleById(int id)
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
                        Id = reader.GetInt32("id"),
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
                Console.WriteLine($"[ERROR] GetPeopleById: {ex.Message}");
                return null;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static People GetPeopleByFullName(string firstName, string lastName)
        {
            try
            {
                People people = null;
                conn = SqlConnection.OpenConnect();
                string query = @"SELECT * FROM people
                                 WHERE first_name = @firstName AND last_name = @lastName";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    people = new People
                    {
                        Id = reader.GetInt32("id"),
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
                //Console.WriteLine($"[ERROR] GetPeopleById: {ex.Message}");
                return null;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }



        public static People GetPeopleBySecretCode(string secretCode)
        {
            try
            {
                People people = null;
                conn = SqlConnection.OpenConnect();
                string query = "SELECT * FROM people WHERE secret_code = @secretCode";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@secretCode", secretCode);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    people = new People
                    {
                        Id = reader.GetInt32("id"),
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
                //Console.WriteLine($"[ERROR] GetPeopleBySecretCode: {ex.Message}");
                return null;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }



        public static string GetTypeById(int id)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string query = @"SELECT p.type
                                 FROM people p WHERE p.id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader["type"].ToString();
                }
                else
                {
                    return null;
                }

            }
            catch (MySqlException ex)
            {
                //Console.WriteLine($"[ERROR] GetTypeById: {ex.Message}");
                return null;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static void PrintPotentialAgents()
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string query = @"SELECT
                                    p.id,
                                    p.first_name,
                                    p.last_name,
                                    p.secret_code,
                                    COUNT(i.id) AS num_reports,
                                    AVG(CHAR_LENGTH(i.text)) AS avg_text_length
                                FROM
                                    people p
                                JOIN IntelReports i ON
                                    p.id = i.reporter_id
                                WHERE
                                    p.type = 'potential agent'
                                GROUP BY
                                    p.id,
                                    p.first_name,
                                    p.last_name;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    int counter = 1;
                    while (reader.Read())
                    {
                        int reporterId = reader.GetInt32("id");
                        string firstName = reader.GetString("first_name");
                        string lastName = reader.GetString("last_name");
                        string secretCode = reader.GetString("secret_code");
                        int numReporter = reader.GetInt32("num_reports");
                        string avgText = reader.GetString("avg_text_length");

                        Console.WriteLine($"\nReporter number {counter}");
                        Console.WriteLine($"Reporter ID  : {reporterId}");
                        Console.WriteLine($"Name         : {firstName} {lastName}");
                        Console.WriteLine($"Secret Code  : {secretCode}");
                        Console.WriteLine($"Reports      : {numReporter}");
                        Console.WriteLine($"Avg len texts: {avgText}\n");
                        counter++;
                    }
                }
                else
                    Console.WriteLine($"\nThere are no potential agents\n");

            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetPotentialAgents: {ex.Message}");
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


        public static int GetNumReports(int reporterId)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"SELECT p.num_reports FROM people p WHERE p.id = @reporterId";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@reporterId", reporterId);
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt32("num_reports");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetNumReports: {ex.Message}");
                return -1;
            }
            finally
            {
                SqlConnection.CloseConnection(conn);
            }
        }


        public static int GetNumMentions(int mentionId)
        {
            try
            {
                conn = SqlConnection.OpenConnect();
                string Query = @"SELECT p.num_mentions FROM people p WHERE p.id = @mentionId";
                MySqlCommand cmd = new MySqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@mentionId", mentionId);
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt32("num_mentions");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[ERROR] GetNumMentions: {ex.Message}");
                return -1;
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
