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
        string connection_string = "Host=localhost; Port = 5432; Database = TesatyWiezy; User Id = postgres; Password = postgres;";

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
       

        public List<User> ReturnUsersListOfUsers()
        {
            string query = "SELECT * FROM \"User\"";
            List<Dictionary<string, string>> list_reader = new List<Dictionary<string, string>>();
            List<User> list_user = new List<User>();
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            try
            {
                connection.Open();
            }
            catch
            {
                Debug.Print("Connection failed");
                return list_user;
            }
            using NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, connection);
            using NpgsqlDataReader reader = npgsqlCommand.ExecuteReader();

            list_reader = getAllReaderUsers(reader);

            foreach (var d in list_reader)
            {
                var user = new User(int.Parse(d["id"]), d["login"], d["password"], d["email"], d["firstName"], d["lastName"], d["role"], User.StringToType(d["role"]));
                list_user.Add(user);
            }

            return list_user;
        }
        public List<Course> ReturnCourseListOfCourses()
        {
            string query = "SELECT * FROM \"Course\"";
            List<Dictionary<string, string>> list_reader = new List<Dictionary<string, string>>();
            List<Course> list_courses = new List<Course>();
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);

            try
            {
                connection.Open();
            }
            catch
            {
                Debug.Print("Connection failed");
                return list_courses;
            }

            using NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, connection);
            using NpgsqlDataReader reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    // Pobieranie danych z bazy i konwersja na obiekt Course
                    int id = int.Parse(reader["id"].ToString());
                    string name = reader["name"].ToString();
                    int category = int.Parse(reader["category"].ToString());
                    int owner = int.Parse(reader["owner"].ToString());

                    // Konwersja list (Teachers, Students, Tests) z bazy danych, jeśli są zapisane w formacie tekstowym
                    List<int> teachers = reader["teachers"] is string teachersStr
                        ? teachersStr.Split(',').Select(int.Parse).ToList()
                        : new List<int>();

                    List<int> students = reader["students"] is string studentsStr
                        ? studentsStr.Split(',').Select(int.Parse).ToList()
                        : new List<int>();

                    List<int> tests = reader["tests"] is string testsStr
                        ? testsStr.Split(',').Select(int.Parse).ToList()
                        : new List<int>();

                    // Tworzenie obiektu Course
                    Course course = new Course(id, name, category, teachers, students, tests);
                    list_courses.Add(course);
                }
                catch (Exception ex)
                {
                    Debug.Print($"Error parsing course data: {ex.Message}");
                }
            }

            return list_courses;
        }


        public List<Question> ReturnQuestionList()
        {
            string query = "SELECT * FROM \"Question\"";
            List<Question> list = new List<Question>();
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);
            try
            {
                con.Open();
                var reader = com.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("questionid");
                    string name = reader.GetString("name");
                    string cat = reader.GetString("category");
                    string questionType = reader.GetString("questiontype");
                    bool shared = reader.GetBoolean("shared");
                    double points = reader.GetDouble("maxpoints");
                    //int answer = reader.GetInt32("answerid");
                    string text = reader.GetString("questiontext");

                    var q = new Question(name, text, Question.StringToType(questionType), new List<string>(), points, new List<int>(), cat, shared, id);
                    list.Add(q);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                return list;
            }

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
        public void AddCourse(Course cours)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            string query = "INSERT INTO \"Course\"( name, cattegory, ownerid) VALUES ";
            query += string.Format("(\'{0}\', \'{1}\', \'{2}\')", cours.Name, cours.Category, cours.Owner);
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

        public void AddTest(Test test)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);

            string query_answer = "INSERT INTO \"Answer\"";

            //string query_question = "INSERT INTO \"Question\"(name, category, questiontype, shared, password, role) VALUES ";
            //query_question += string.Format("(\'{0}\', \'{1}\', \'{2}\', \'{3}\', \'{4}\', \'{5}\')", user.Login, user.FirstName, user.LastName, user.Email, user.Password, user.UserType);
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
        public bool RemoveCourse(int id)
        {
            string query = string.Format("DELETE FROM \"Course\" where \"courseid\" = {0}", id);
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

                user = new User(userID, login, pass, email, fname, lname, type, User.StringToType(type));
            }
            catch
            {
                Debug.Print("Connection failed");
            }
            return user;
        }

        public User UpdateUser(User user)
        {
            string query = string.Format("update \"User\" set \"login\" = '{0}', \"password\" = '{1}'," +
                "\"email\" = '{2}', \"name\" = '{3}',\"surname\" = '{4}', \"role\" = '{5}' where \"userid\" = {6}",
                user.Login, user.Password, user.Email, user.FirstName, user.LastName, user.UserType, user.GetID());
            string queryEnd = string.Format("SELECT * FROM \"User\" WHERE \"userid\" = {0}", user.GetID());

            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            try
            {
                connection.Open();
                using NpgsqlCommand com = new NpgsqlCommand(query, connection);
                using NpgsqlCommand comEnd = new NpgsqlCommand(queryEnd, connection);
                com.ExecuteNonQuery();
                var reader = comEnd.ExecuteReader();

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

                user = new User(userID, login, pass, email, fname, lname, type, User.StringToType(type));

                connection.Close();
            }
            catch
            {
                Debug.Print("Connection failed");
                return new User();
            }
            return user;
        }
        public bool UpdateUserRole(User user)
        {
            string query = string.Format("UPDATE \"User\" SET \"role\" = '{0}' WHERE \"userid\" = {1}",
                user.Role, user.GetID());

            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            try
            {
                connection.Open();

                using NpgsqlCommand command = new NpgsqlCommand(query, connection);
                int rowsAffected = command.ExecuteNonQuery(); 

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                return false; 
            }
            finally
            {
                connection.Close(); 
            }
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
