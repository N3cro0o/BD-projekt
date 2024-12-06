using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;
using BD.Models;
using Npgsql;

namespace BD
{
    public class DataBaseConnection
    {
        string connection_string = "Host=localhost; Port = 5432; Database = TesatWiezy; User Id = postgres; Password = 12345;";

        public List<Dictionary<string, string>> ReturnUsersList(string query)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            try
            {
                connection.Open();
            }
            catch
            {
                Debug.Print("Connection failed");
                return list;
            }
            using NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, connection);
            using NpgsqlDataReader reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("login", reader["login"].ToString());
                dict.Add("id", reader["userid"].ToString());
                dict.Add("password", reader["password"].ToString());
                dict.Add("email", reader["email"].ToString());
                dict.Add("firstName", reader["name"].ToString());
                dict.Add("lastName", reader["surname"].ToString());
                dict.Add("role", reader["role"].ToString());
                list.Add(dict);
            }
            connection.Close();
            return list;
        }

        public void AddUser(User user)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            string query = "INSERT INTO \"User\"(login, name, surname, email, password, role) VALUES ";
            query += string.Format("(\'{0}\', \'{1}\', \'{2}\', \'{3}\', \'{4}\', \'{5}\')", user.Login, user.FirstName, user.LastName, user.Email, user.Password, user.UserType);
            Debug.Print(query);
            try
            {
                connection.Open();
            }
            catch
            {
                Debug.Print("Connection failed");
                return;
            }
            using NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, connection);
            npgsqlCommand.ExecuteNonQuery();
            connection.Close();
        }
    }
}
