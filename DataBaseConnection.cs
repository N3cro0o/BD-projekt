﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Security.Cryptography;
using BD.Models;
using Npgsql;

namespace BD
{
    public class DataBaseConnection
    {
        string connection_string = "Host=localhost; Port = 5432; Database = TesatWiezy; User Id = postgres; Password = 12345;";
        string hash_key = "";


        public List<Dictionary<string, string>> Login(string login, string pass)
        {
            string query = "SELECT * FROM \"User\" WHERE \"login\" = \'" + login + "\' AND \"password\" = \'" + toSHA256(pass) + "\'";

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

        public List<User> LoginWithUser(string login, string pass)
        {
            string query = "SELECT * FROM \"User\" WHERE \"login\" = \'" + login + "\' AND \"password\" = \'" + toSHA256(pass) + "\'";

            List<User> list = new List<User>();
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

            var r_list = getAllReaderUsers(reader);

            foreach (var d in r_list)
            {
                var user = new User(int.Parse(d["id"]), d["login"], d["password"], d["email"], d["firstName"], d["lastName"], User.StringToType(d["role"]));
                list.Add(user);
            }

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

        public List<User> ReturnStudentList()
        {
            string query = "SELECT * FROM \"User\" WHERE role = 'student' OR role = 'Student' OR role = 'uczne' OR role = 'Uczen' ORDER BY userid";
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
                    c.StudentsCount = courseStudentCount(c);
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
                    c.StudentsCount = courseStudentCount(c);
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
                    double points = reader.GetDouble(10);
                    string text = reader.GetString(11);
                    bool a = reader.GetBoolean(6);
                    bool b = reader.GetBoolean(7);
                    bool c = reader.GetBoolean(8);
                    bool d = reader.GetBoolean(9);
                    int key = (d ? 1 : 0) + (c ? 2 : 0) + (b ? 4 : 0) + (a ? 8 : 0);
                    var q = new Question(name, text, Question.StringToType(questionType), reader.GetString(5), points, key, cat, shared, id);
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
                    double points = reader.GetDouble(10);
                    string text = reader.GetString(11);
                    bool a = reader.GetBoolean(6);
                    bool b = reader.GetBoolean(7);
                    bool c = reader.GetBoolean(8);
                    bool d = reader.GetBoolean(9);
                    int key = (d ? 1 : 0) + (c ? 2 : 0) + (b ? 4 : 0) + (a ? 8 : 0);
                    var q = new Question(name, text, Question.StringToType(questionType), reader.GetString(5), points, key, cat, shared, id);
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
            string query = "SELECT * FROM \"Test\" WHERE archived = 'false' ORDER BY testid";
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
                    t.IsArchived = r.GetBoolean(6);
                    list.Add(t);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }

            return list;
        }
        
        public List<Test> ReturnArchivedTestsList()
        {
            var list = new List<Test>();
            string query = "SELECT * FROM \"Test\" WHERE archived = 'true' ORDER BY testid";
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
                    t.IsArchived = r.GetBoolean(6);
                    list.Add(t);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }

            return list;
        }

        public List<Test> ReturnTestsListWithID(int id)
        {
            var list = new List<Test>();
            string query = $"SELECT * FROM \"Test\" WHERE testid = '{id}' ORDER BY testid";
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
            string query = $"SELECT * FROM \"Test\" WHERE courseid = '{id}' AND archived = 'false' ORDER BY testid";
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

        public List<Answer> ReturnAllAnswersList()
        {
            List<Answer> answers = new List<Answer>();
            string query = "SELECT a.*, t.name,q.name, u.name, u.surname FROM public.\"Answer\" a " +
                "JOIN \"Test\" t on a.testid = t.testid " +
                "JOIN \"Question\" q on a.questionid = q.questionid " +
                "JOIN \"User\" u on a.userid = u.userid " +
                "ORDER BY answerid ASC";
            NpgsqlConnection _con = new NpgsqlConnection(connection_string);
            NpgsqlCommand _com = new NpgsqlCommand(query, _con);
            try
            {
                _con.Open();
                var r = _com.ExecuteReader();
                while (r.Read())
                {
                    int id = r.GetInt32(0);
                    int userid = r.GetInt32(9);
                    int testid = r.GetInt32(8);
                    int questid = r.GetInt32(7);
                    double p = r.GetDouble(1);
                    string text = r.GetString(2);
                    bool a = r.GetBoolean(3);
                    bool b = r.GetBoolean(4);
                    bool c = r.GetBoolean(5);
                    bool d = r.GetBoolean(6);
                    int key = (a ? 8 : 0) + (b ? 4 : 0) + (c ? 2 : 0) + (d ? 1 : 0);

                    string testName = r.GetString(10);
                    string questName = r.GetString(11);
                    string studentName = r.GetString(12) + " " + r.GetString(13);
                    var answ = new Answer(id, userid, questid, testid, p, key, text);
                    answ.TestName = testName;
                    answ.QuestName = questName;
                    answ.UserName = studentName;
                    answers.Add(answ);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return new List<Answer>();
            }

            return answers;
        }

        public bool AddUser(User user)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            string query = "INSERT INTO \"User\"(login, name, surname, email, password, role) VALUES ";
            query += string.Format("(\'{0}\', \'{1}\', \'{2}\', \'{3}\', \'{4}\', \'{5}\')", user.Login, user.FirstName, user.LastName, user.Email, toSHA256(user.Password), user.UserType);
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
            using NpgsqlConnection connection1 = new NpgsqlConnection(connection_string);
            int a = (quest.CorrectAnswers & (1 << 3)) >> 3;
            int b = (quest.CorrectAnswers & (1 << 2)) >> 2; ;
            int c = (quest.CorrectAnswers & (1 << 1)) >> 1; ;
            int d = (quest.CorrectAnswers & (1 << 0)) >> 0; ;
            string query_question = "INSERT INTO \"Question\"(name, category, questiontype, shared, maxpoints, answer, a, b, c, d, questionbody) VALUES ";
            query_question += $"('{quest.Name}', '{quest.Category}', '{quest.QuestionType.ToString().ToLower()}','{quest.Shared}'," +
                $"'{quest.Points.ToString(System.Globalization.CultureInfo.InvariantCulture)}','{quest.Answers}','{a}','{b}','{c}','{d}','{quest.Text}')";

            // Answer
            try
            {
                connection1.Open();
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

        public (bool, int) AddCourse(Course c)
        {
            int id;
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            string querry = "INSERT INTO \"Course\" (name, category, description, ownerid) VALUES " +
                $"('{c.Name}','{c.Category}','{c.Description}','{c.Teachers[0].ID}')";
            string query_end = "SELECT courseid FROM \"Course\" ORDER BY courseid DESC LIMIT 1";
            NpgsqlCommand com = new NpgsqlCommand(querry, con);
            NpgsqlCommand comEnd = new NpgsqlCommand(query_end, con);
            try
            {
                con.Open();
                com.ExecuteNonQuery();
                var r = comEnd.ExecuteReader();
                r.Read();
                id = r.GetInt32(0);
            }
            catch (Exception e)
            {
                Debug.Print($"Connection failed\n{e}");
                return (false, -1);
            }
            finally
            {
                con.Close();
            }
            return (true, id);
        }
        //Select testid from "Test" order by testid desc limit 1
        public (bool, int) AddTest(Test t)
        {
            int id;
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            string querry = "INSERT INTO \"Test\" (name, starttime, endtime, category, courseid) VALUES " +
                $"('{t.Name}','{t.StartDate}','{t.EndDate}','{t.Category}','{t.CourseObject.ID}')";
            string query_end = "SELECT testid FROM \"Test\" ORDER BY testid DESC LIMIT 1";
            NpgsqlCommand com = new NpgsqlCommand(querry, con);
            NpgsqlCommand comEnd = new NpgsqlCommand(query_end, con);
            try
            {
                con.Open();
                com.ExecuteNonQuery();
                var r = comEnd.ExecuteReader();
                r.Read();
                id = r.GetInt32(0);
            }
            catch (Exception e)
            {
                Debug.Print($"Connection failed\n{e}");
                return (false, 0);
            }
            finally
            {
                con.Close();
            }

            return (true, id);
        }

        public bool RemoveUser(User user)
        {
            RemoveCourseToStudent(user);

            string query = string.Format("DELETE FROM \"User\" where \"userid\" = {0}", user.ID);
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

        public bool RemoveCourse(Course course)
        {
            RemoveCourseToStudent(course);

            if (RemoveTestsWithCourse(course.ID))
            {
                string query = string.Format("DELETE FROM \"Course\" where \"courseid\" = {0}", course.ID);
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
            return false;
        }

        public bool RemoveQuestion(Question question)
        {
            RemoveTestToQuestion(question);

            string query = string.Format("DELETE FROM \"Question\" where \"questionid\" = {0}", question.ID);
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

        bool removeAnswer(int id)
        {
            string query = $"DELETE FROM \"Answer\" WHERE answerid = '{id}'";
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

        public bool RemoveTest(Test test)
        {
            RemoveTestToQuestion(test);
            string query = string.Format("DELETE FROM \"Test\" where \"testid\" = {0}", test.ID);
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

        public bool RemoveTestsWithCourse(int course_id)
        {
            string query = string.Format("DELETE FROM \"Test\" where \"courseid\" = {0}", course_id);
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

        public User UpdateUser(User user, bool samePassword = false)
        {
            string query;
            if (!samePassword)
                query = string.Format("update \"User\" set \"login\" = '{0}', \"password\" = '{1}'," +
                    "\"email\" = '{2}', \"name\" = '{3}',\"surname\" = '{4}', \"role\" = '{5}' where \"userid\" = {6}",
                    user.Login, user.Password, user.Email, user.FirstName, user.LastName, user.UserType, user.GetID());
            else
                query = $"update \"User\" set \"login\" = '{user.Login}', " +
                    $"\"email\" = '{user.Email}', \"name\" = '{user.FirstName}',\"surname\" = '{user.LastName}', \"role\" = '{user.UserType}' where \"userid\" = {user.GetID()}";
            string queryEnd = string.Format("SELECT * FROM \"User\" WHERE \"userid\" = {0}", user.GetID());

            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            try
            {
                connection.Open();
                using NpgsqlCommand com = new NpgsqlCommand(query, connection);
                com.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception e)
            {
                Debug.Print($"Connection failed\n{e}");
                return new User();
            }
            user = GetUserByID(user.ID);
            return user;
        }

        public Question UpdateQuestion(Question question)
        {
            int a = (question.CorrectAnswers & (1 << 3)) >> 3;
            int b = (question.CorrectAnswers & (1 << 2)) >> 2;
            int c = (question.CorrectAnswers & (1 << 1)) >> 1;
            int d = (question.CorrectAnswers & (1 << 0)) >> 0;
            int shared = question.Shared ? 1 : 0;
            string point = question.Points.ToString(System.Globalization.CultureInfo.InvariantCulture);

            string query_question = $"UPDATE \"Question\" SET name = '{question.Name}', category = '{question.Category}', " +
                $"questiontype = '{question.QuestionType.ToString().ToLower()}', shared = '{shared}', maxpoints = '{point}', questionbody = '{question.Text}', " +
                $"answer = '{question.Answers}', a = '{a}', b = '{b}', c = '{c}', d = '{d}' WHERE \"questionid\" = '{question.ID}'";
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);
            try
            {
                connection.Open();
                using NpgsqlCommand com_q = new NpgsqlCommand(query_question, connection);
                com_q.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Debug.Print($"Connection failed\n{e}");
                return new Question();
            }
            var q = ReturnQuestionListByID(question.ID)[0];
            return q;
        }

        public Course UpdateCourse(Course course)
        {
            string query = $"UPDATE \"Course\" SET name = '{course.Name}', category = '{course.Category}', description = '{course.Description}', " +
                $"ownerid = '{course.Teachers[0].ID}' WHERE courseid = '{course.ID}'";
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);

            try
            {
                connection.Open();
                using NpgsqlCommand com_a = new NpgsqlCommand(query, connection);
                com_a.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Debug.Print($"Connection failed\n{e}");
                return new Course();
            }
            var c = ReturnCoursesListWithID(course.ID)[0];
            return c;
        }

        public Test UpdateTest(Test test)
        {
            string query = $"UPDATE \"Test\" SET name = '{test.Name}', starttime = '{test.StartDate}', endtime = '{test.EndDate}', " +
            $"category = '{test.Category}', courseid = '{test.CourseObject.ID}' WHERE testid = '{test.ID}'";
            using NpgsqlConnection connection = new NpgsqlConnection(connection_string);

            try
            {
                connection.Open();
                using NpgsqlCommand com_a = new NpgsqlCommand(query, connection);
                com_a.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Debug.Print($"Connection failed\n{e}");
                return new Test();
            }
            var t = ReturnTestsListWithID(test.ID)[0];
            return t;
        }

        public bool ConnectTestToQuestion(Test test_target, IEnumerable<Question> question_arr)
        {
            int id = test_target.ID;
            string query = "INSERT INTO \"QuestionToTest\" VALUES ";
            for (int i = 0; i < question_arr.Count() - 1; i++)
            {
                query += $"('{id}','{question_arr.ElementAt(i).ID}'), ";
            }
            query += $"('{id}','{question_arr.ElementAt(question_arr.Count() - 1).ID}') ";
            Debug.Print(query);

            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return false;
            }

            return true;
        }

        public bool ConnectCourseToStudent(Course course_target, IEnumerable<User> user_arr)
        {
            int id = course_target.ID;
            string query = "INSERT INTO \"UserToCourse\" VALUES ";
            for (int i = 0; i < user_arr.Count() - 1; i++)
            {
                query += $"('{user_arr.ElementAt(i).ID}','{id}'), ";
            }
            query += $"('{user_arr.ElementAt(user_arr.Count() - 1).ID}','{id}') ";
            Debug.Print(query);

            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return false;
            }

            return true;
        }

        public bool UpdateTestToQuestion(Test test_target, IEnumerable<Question> question_arr)
        {
            if (!RemoveTestToQuestion(test_target))
                return false;
            return ConnectTestToQuestion(test_target, question_arr);
        }

        public bool UpdateCourseToStudent(Course course_target, IEnumerable<User> user_arr)
        {
            if (!RemoveCourseToStudent(course_target))
                return false;
            return ConnectCourseToStudent(course_target, user_arr);
        }

        public bool RemoveTestToQuestion(Test test_target)
        {
            string query = $"DELETE FROM \"QuestionToTest\" WHERE testid = '{test_target.ID}'";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return false;
            }

            return true;
        }
        public bool RemoveTestToQuestion(Question question_target)
        {
            string query = $"DELETE FROM \"QuestionToTest\" WHERE questionid = '{question_target.ID}'";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return false;
            }

            return true;
        }

        public bool RemoveCourseToStudent(Course course_target)
        {
            string query = $"DELETE FROM \"UserToCourse\" WHERE courseid = '{course_target.ID}'";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return false;
            }

            return true;
        }

        public bool RemoveCourseToStudent(User user_target)
        {
            string query = $"DELETE FROM \"UserToCourse\" WHERE userid = '{user_target.ID}'";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return false;
            }

            return true;
        }
        public bool RemoveCourseToStudent(Course course_target, User user_target)
        {
            string query = $"DELETE FROM \"UserToCourse\" WHERE userid = '{user_target.ID}' AND courseid = '{course_target.ID}'";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return false;
            }

            return true;
        }

        public List<int> ReturnConnectedQuestionsToTest(Test test)
        {
            List<int> list = new List<int>();

            string query = $"SELECT questionid FROM \"QuestionToTest\" WHERE testid = '{test.ID}' ORDER BY questionid ASC";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                var r = com.ExecuteReader();
                while (r.Read())
                    list.Add(r.GetInt32(0));
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return new List<int>();
            }

            return list;
        }

        public List<int> ReturnConnectedStudentsToCourse(Course course)
        {
            List<int> list = new List<int>();

            string query = $"SELECT userid FROM \"UserToCourse\" WHERE courseid = '{course.ID}' ORDER BY userid ASC";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                var r = com.ExecuteReader();
                while (r.Read())
                    list.Add(r.GetInt32(0));
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return new List<int>();
            }

            return list;
        }

        public List<Question> ReturnQuestionListByTest(Test t)
        {
            List<Question> list = new List<Question>();
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            string query = $"SELECT \"Question\".questionid, \"name\", category, questiontype, shared, answer, a, b, c, d, maxpoints, questionbody " +
                $"FROM \"Question\" JOIN \"QuestionToTest\" ON \"QuestionToTest\".testid = {t.ID} WHERE \"Question\".questionid = \"QuestionToTest\".questionid ORDER BY \"Question\".questionid";
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                var r = com.ExecuteReader();
                while (r.Read())
                {
                    int id = r.GetInt32(0);
                    string name = r.GetString(1);
                    string cat = r.GetString(2);
                    string questionType = r.GetString(3);
                    bool shared = r.GetBoolean(4);
                    double points = r.GetDouble(10);
                    string text = r.GetString(11);
                    bool a = r.GetBoolean(6);
                    bool b = r.GetBoolean(7);
                    bool c = r.GetBoolean(8);
                    bool d = r.GetBoolean(9);
                    int key = (d ? 1 : 0) + (c ? 2 : 0) + (b ? 4 : 0) + (a ? 8 : 0);
                    var q = new Question(name, text, Question.StringToType(questionType), r.GetString(5), points, key, cat, shared, id);
                    list.Add(q);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return new List<Question>();
            }
            return list;
        }

        public bool ToggleArchiveTest(Test test)
        {
            string query = $"UPDATE \"Test\" SET archived = '{!test.IsArchived}' WHERE testid = '{test.ID}'";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);

            try
            {
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return false;
            }
            return true;
        }

        int courseStudentCount(Course course)
        {
            int c = 0;
            string query = $"SELECT count(*) AS \"count\" FROM \"UserToCourse\" WHERE courseid = {course.ID} ORDER BY \"count\" ASC";
            NpgsqlConnection con = new NpgsqlConnection(connection_string);
            NpgsqlCommand com = new NpgsqlCommand(query, con);
            try
            {
                con.Open();
                c = Convert.ToInt32(com.ExecuteScalar());
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return 0;
            }
            return c;
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

        string toSHA256(string s)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(s));

            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
