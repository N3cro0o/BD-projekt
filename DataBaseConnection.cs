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

        public List<Dictionary<string, string>> Login(string login, string pass)
        {
            string query = "SELECT * FROM \"User\" WHERE \"login\" = \'" + login + "\' AND \"password\" = \'" + pass + "\'";

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

            list = getAllReaderUsers(reader);

            connection.Close();
            return list;
        }

        public List<Dictionary<string, string>> ReturnUsersList()
        {
            string query = "SELECT * FROM \"User\"";
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

            list = getAllReaderUsers(reader);

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
                using NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, connection);
                npgsqlCommand.ExecuteNonQuery();
            }
            catch
            {
                Debug.Print("Connection failed");
                return;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool RemoveUser(int id)
        {
            string query = string.Format("DELETE FROM \"User\" where \"userid\" = {0}", id);
            Debug.WriteLine(query);
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            try
            {
                connection.Open();
                using NpgsqlCommand com = new NpgsqlCommand(query, connection);
                com.ExecuteNonQuery();
            }
            catch
            {
                Debug.Print("Connection failed");
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        private List<Dictionary<string, string>> getAllReaderUsers(NpgsqlDataReader r)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            while (r.Read())
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("login", r["login"].ToString());
                dict.Add("id", r["userid"].ToString());
                dict.Add("password", r["password"].ToString());
                dict.Add("email", r["email"].ToString());
                dict.Add("firstName", r["name"].ToString());
                dict.Add("lastName", r["surname"].ToString());
                dict.Add("role", r["role"].ToString());
                list.Add(dict);
            }
            return list;
        }
    }
}
