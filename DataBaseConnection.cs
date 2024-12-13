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

        public User GetUserByID(int id)
        {
            var user = new User();
            string query = string.Format("SELECT * FROM \"User\" where \"userid\" = \'{0}\'", id);
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            Debug.Print(query);

            try
            {
                connection.Open();
                using NpgsqlCommand com = new NpgsqlCommand(query, connection);
                using NpgsqlDataReader reader = com.ExecuteReader();

                reader.Read(); // UBER IMPORTANT THINGY!!!!!!!!!!!!!!
                int userID = 0;
                string login, pass, email, fname, lname, type;
                userID = reader.GetInt32(reader.GetOrdinal("userid"));
                login = reader.GetString(reader.GetOrdinal("login"));
                pass = reader.GetString(reader.GetOrdinal("password"));
                email = reader.GetString(reader.GetOrdinal("email"));
                fname = reader.GetString(reader.GetOrdinal("name"));
                lname = reader.GetString(reader.GetOrdinal("surname"));
                type = reader.GetString(reader.GetOrdinal("role"));
                connection.Close();

                user = new User(userID, login, pass, email, fname, lname, User.StringToType(type));
            }
            catch
            {
                Debug.Print("Connection failed");
            }
            return user;
        }

        public bool UpdateUser(User user)
        {
            string query = string.Format("update \"User\" set \"login\" = '{0}', \"password\" = '{1}'," +
                "\"email\" = '{2}', \"name\" = '{3}',\"surname\" = '{4}', \"role\" = '{5}' where \"userid\" = {6}",
                user.Login, user.Password, user.Email, user.FirstName, user.LastName, user.UserType, user.GetID());
            Debug.Print(query);
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            return false;
            try
            {
                connection.Open();
                using NpgsqlCommand com = new NpgsqlCommand(query, connection);
                com.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                Debug.Print("Connection failed");
                return false;
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
