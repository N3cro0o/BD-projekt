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
            string query = "SELECT * FROM \"User\" ORDER BY userid";
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
            string query = "SELECT * FROM \"User\" ORDER BY userid";
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
                var user = new User(int.Parse(d["id"]), d["login"], d["password"], d["email"], d["firstName"], d["lastName"], User.StringToType(d["role"]));
                list_user.Add(user);
            }

            return list_user;
        }

        public List<User> ReturnTeacherList()
        {
            string query = "SELECT * FROM \"User\" WHERE role = 'teacher' OR role = 'Teacher' OR role = 'nauczyciel' OR role = 'Nauczyciel' or role = 'Admin' or role = 'admin' ORDER BY userid";
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
                var user = new User(int.Parse(d["id"]), d["login"], d["password"], d["email"], d["firstName"], d["lastName"], User.StringToType(d["role"]));
                list_user.Add(user);
            }

            return list_user;
        }

        public List<Course> ReturnCoursesList()
        {
            List<Course> list = new List<Course>();
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand("SELECT * FROM \"Course\" ORDER BY courseid", con);

            try
            {
                con.Open();
                var r = com.ExecuteReader();
                while (r.Read())
                {
                    Course c = new Course();
                    c.Name = r.GetString(1);
                    c.ID = r.GetInt32(0);
                    c.Category = r.GetString(2);
                    c.Description = r.GetString(3);
                    var u = GetUserByID(r.GetInt32(4));
                    c.Teachers.Add(u);
                    c.MainTeacherName = $"{u.FirstName} {u.LastName}";
                    list.Add(c);
                }
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }

            return list;
        }

        public List<Course> ReturnCoursesListWithID(int id)
        {
            List<Course> list = new List<Course>();
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand($"SELECT * FROM \"Course\" WHERE courseid = '{id}' ORDER BY courseid", con);

            try
            {
                con.Open();
                var r = com.ExecuteReader();
                while (r.Read())
                {
                    Course c = new Course();
                    c.Name = r.GetString(1);
                    c.ID = r.GetInt32(0);
                    c.Category = r.GetString(2);
                    c.Description = r.GetString(3);
                    var u = GetUserByID(r.GetInt32(4));
                    c.Teachers.Add(u);
                    c.MainTeacherName = $"{u.FirstName} {u.LastName}";
                    list.Add(c);
                }
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }

            return list;
        }

        public List<Question> ReturnQuestionList()
        {
            string query = "SELECT * FROM \"Question\" ORDER BY questionid";
            List<Question> list = new List<Question>();
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);
            try
            {
                con.Open();
                var reader = com.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string cat = reader.GetString(2);
                    string questionType = reader.GetString(3);
                    bool shared = reader.GetBoolean(4);
                    double points = reader.GetDouble(5);
                    int answer_id = reader.GetInt32(6);
                    string text = reader.GetString(7);
                    var q = new Question(name, text, Question.StringToType(questionType), "", points, 0, cat, shared, id);
                    q.AnswerID = answer_id;
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

        public List<Question> ReturnQuestionListByID(int id)
        {
            string query = $"SELECT * FROM \"Question\" WHERE questionid = '{id}' ORDER BY questionid";
            List<Question> list = new List<Question>();
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);
            try
            {
                con.Open();
                var reader = com.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader.GetString(1);
                    string cat = reader.GetString(2);
                    string questionType = reader.GetString(3);
                    bool shared = reader.GetBoolean(4);
                    double points = reader.GetDouble(5);
                    int answer_id = reader.GetInt32(6);
                    string text = reader.GetString(7);
                    var q = new Question(name, text, Question.StringToType(questionType), "", points, 0, cat, shared, id);
                    q.AnswerID = answer_id;
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

        public List<Test> ReturnTestsList()
        {
            var list = new List<Test>();
            string query = "SELECT * FROM \"Test\" ORDER BY testid";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                var r = com.ExecuteReader();
                while (r.Read())
                {
                    var c = ReturnCoursesListWithID(r.GetInt32(5));
                    var t = new Test(r.GetInt32(0), r.GetString(1), c[0], new List<int>(), r.GetDateTime(2), r.GetDateTime(3), r.GetString(4));
                    list.Add(t);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }

            return list;
        }

        public List<Test> ReturnCourseTestsList(int id)
        {
            var list = new List<Test>();
            string query = $"SELECT * FROM \"Test\" WHERE courseid = '{id}' ORDER BY testid";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                var r = com.ExecuteReader();
                while (r.Read())
                {
                    var c = ReturnCoursesListWithID(id);
                    var t = new Test(r.GetInt32(0), r.GetString(1), c[0], new List<int>(), r.GetDateTime(2), r.GetDateTime(3), r.GetString(4));
                    list.Add(t);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }

            return list;
        }

        public Answer FetchAnswer(int id)
        {
            Answer answer = null;
            string query1 = $"SELECT * FROM \"Answer\" WHERE answerid = '{id}' LIMIT 1";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query1, con);

            try
            {
                con.Open();
                var r = com.ExecuteReader();
                while (r.Read())
                {
                    answer = new Answer(r.GetInt32(0), r.GetDouble(1), r.GetInt32(2), r.GetString(3));
                }
            }
            catch (Exception e) { Debug.Print(e.ToString()); }

            return answer;
        }

        public bool AddUser(User user)
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
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        public bool AddQuestion(Question quest)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            using NpgsqlConnection connection1 = new NpgsqlConnection(connection_string);
            int answ_id = 0;
            int key = 0;

            Debug.Print($"Key: {key}");

            string query_answer = "INSERT INTO \"Answer\"(score, answer, key, a, b, c, d) VALUES ";
            query_answer += $"(\'{quest.Points.ToString(System.Globalization.CultureInfo.InvariantCulture)}\', \'{quest.Answers}\', '{quest.CorrectAnswers}', \'{(quest.CorrectAnswers & (1 << 3)) >> 3}\'," +
                $"\'{(quest.CorrectAnswers & (1 << 2)) >> 2}\',\'{(quest.CorrectAnswers & (1 << 1)) >> 1}\',\'{(quest.CorrectAnswers & (1 << 0)) >> 0}\');";
            query_answer += $"SELECT * FROM \"Answer\" WHERE answer = '{quest.Answers}'";
            Debug.Print(query_answer);
            string query_question = "INSERT INTO \"Question\"(name, category, questiontype, shared, maxpoints, answerid, questiontext) VALUES ";

            // Answer
            try
            {
                connection.Open();
                using NpgsqlCommand npgsqlCommand1 = new NpgsqlCommand(query_answer, connection);

                var r = npgsqlCommand1.ExecuteReader();
                while (r.Read())
                {
                    answ_id = r.GetInt32(0);
                }
                connection.Close();
                connection1.Open();
                query_question += $"('{quest.Name}', '{quest.Category}', '{quest.QuestionType.ToString().ToLower()}','{quest.Shared}','{quest.Points.ToString(System.Globalization.CultureInfo.InvariantCulture)}','{answ_id}','{quest.Text}')";
                using NpgsqlCommand npgsqlCommand3 = new NpgsqlCommand(query_question, connection1);
                npgsqlCommand3.ExecuteNonQuery();
                connection1.Close();

            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return false;
            }
            return true;
        }

        public bool AddCourse(Course c)
        {
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            string querry = "INSERT INTO \"Course\" (name, category, description, ownerid) VALUES " +
                $"('{c.Name}','{c.Category}','{c.Description}','{c.Teachers[0].ID}')";
            NpgsqlCommand com = new NpgsqlCommand(querry, con);
            Debug.Print(querry);
            try
            {
                con.Open();
                com.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.Print($"Connection failed\n{e}");
                return false;
            }
            finally
            {
                con.Close();
            }
            return true;
        }

        public bool AddTest(Test t)
        {
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            string querry = "INSERT INTO \"Test\" (name, starttime, endtime, category, courseid) VALUES " +
                $"('{t.Name}','{t.StartDate}','{t.EndDate}','{t.Category}','{t.CourseObject.ID}')";
            NpgsqlCommand com = new NpgsqlCommand(querry, con);
            Debug.Print(querry);
            try
            {
                con.Open();
                com.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.Print($"Connection failed\n{e}");
                return false;
            }
            finally
            {
                con.Close();
            }
            return true;
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

                user = new User(userID, login, pass, email, fname, lname, User.StringToType(type));

                connection.Close();
            }
            catch
            {
                Debug.Print("Connection failed");
                return new User();
            }
            return user;
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
