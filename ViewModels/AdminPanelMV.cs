using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BD.Models;
using BD.Views;
using static BD.Models.Question;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;

namespace BD.ViewModels
{
    internal class AdminPanelMV
    {
        public delegate void StepMethodCallback(AdminPanel parent);

        private readonly MainWindow _mainwindow;
        public StepMethodCallback? StepMethod;

        List<string> strings = new List<string>();

        public AdminPanelMV(MainWindow mainWindow)
        {
            _mainwindow = mainWindow;
        }

        public void InstrQuery(string s, AdminPanel parent)
        {
            int i1 = 0;
            bool string_check = false;

            // Get instruction
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\"')
                {
                    string_check = !string_check;
                    Debug.Print("String check" + i.ToString());
                }
                if (s[i] == ' ' && !string_check)
                {
                    strings.Add(s[i1..i]);
                    Debug.Print("String Cut" + i.ToString());
                    i1 = i + 1;
                }
            }
            // Add last word
            strings.Add(s[i1..(s.Length)]);

            // Select instruction
            switch (strings[0].ToLower())
            {
                case "create":
                case "add":
                    switch (strings[1].ToLower())
                    {
                        case "user":
                        case "u":
                            if (strings[2].ToLower() == "help" || strings[2].ToLower() == "h")
                            {
                                parent.consoleScreen.Text = "Creates new user and insert them into database.\n\nHow it looks:\ncreate user <login> <password> <email> <first name> <last name> <type>\nAccepted types:\n* Student\n* Teacher\n* Admin\n";
                                break;
                            }
                            else if (_createUser(strings[2..strings.Count()].ToArray()))
                            {
                                var list = App.DBConnection.Login(strings[2], strings[3]);
                                parent.consoleScreen.Text = "User created:\n";
                                foreach (var item in list)
                                {
                                    parent.consoleScreen.Text += ("ID: " + item["id"] + ", User: " + item["login"] + ", Email: " + item["email"] + ", First name: " + item["firstName"] + ", Last name: " + item["lastName"] + ", Type: " + item["role"] + "\n\n");
                                }
                            }
                            break;

                        case "question":
                        case "q":
                            if (strings[2].ToLower() == "help" || strings[2].ToLower() == "h")
                            {
                                parent.consoleScreen.Text = "Add help text";
                                break;
                            }
                            else if (_createQuestion(strings[2..strings.Count()].ToArray()))
                            {
                                
                            }
                            else
                            {
                                parent.consoleScreen.Text = "Wrong command";
                            }
                            break;
                    }
                    break;

                case "show":
                    switch (strings[1].ToLower())
                    {
                        case "user":
                            if (strings.Count < 3)
                                break;

                            if (strings[2].ToLower() == "all")
                                ReturnAllUsersFromDB(parent);

                            else if (strings[2] == "/id")
                            {
                                if (strings.Count != 4)
                                    break;
                                int id = int.Parse(strings[3]);
                                User user = App.DBConnection.GetUserByID(id);
                                parent.consoleScreen.Text = "";
                                _printUsers(parent, user);
                            }
                            else if (strings[2].ToLower() == "h" || strings[2].ToLower() == "help")
                            {
                                parent.consoleScreen.Text = "Shows users stored in database.\nshow user all - shows all users\nshow user /id <id> - shows users with desired id";
                            }
                            break;
                        case "question":
                            if (strings.Count < 3)
                                break;
                            if (strings[2].ToLower() == "all")
                                ReturnAllQuestionsFromDB(parent);
                            break;
                    }
                    break;

                case "remove":
                    switch (strings[1].ToLower())
                    {
                        case "user":
                            if (_removeUser(strings[2..strings.Count()].ToArray()))
                            {
                                parent.consoleScreen.Text = "User removed";
                            }
                            break;
                    }
                    break;

                case "update":
                    switch (strings[1].ToLower())
                    {
                        case "user":
                            if (strings[2].ToLower() == "help" || strings[2].ToLower() == "h")
                            {
                                parent.consoleScreen.Text = "Updates user and insert changed data into database.\n\nHow it looks:\nupdate user /id <id>\nAdditional flags:" +
                                    " /l <login> /p <password> /e <email> /fn <first name> /ln <last name> /t <type>\n\nAccepted types:\n* Student\n* Teacher\n* Admin\n";
                                break;
                            }
                            else if (_modifyUser(parent, strings[2..strings.Count()].ToArray()))
                            {
                                parent.consoleScreen.Text += "\n\nUser updated";
                            }
                            else
                            {
                                parent.consoleScreen.Text = "Error, user cannot be changed";
                            }
                            break;
                    }
                    break;

                case "help":
                case "h":
                    parent.consoleScreen.Text = "Please type help or h for more information after desired command for more information. " +
                        "Possible commands:\n* show\n* create\n* update\n* remove";
                    break;

                case "clear":
                case "cl":
                    parent.consoleScreen.Text = "";
                    break;

                default:
                    parent.consoleScreen.Text = "Wrong command.\nPlease type help or h for more information";
                    break;

            }

            strings.Clear();
            parent.InputBox.Text = "";
        }

        void _printUsers(AdminPanel parent, User user)
        {
            if (user != null)
            {
                parent.InputBox.Text = "";
                parent.consoleScreen.Text += string.Format("Users:\nID: {0}, Login: {1}, Email: {2}, First name: {3}, Last name {4}, Type: {5}\n",
                    user.GetID(), user.Login, user.Email, user.FirstName, user.LastName, user.UserType);
            }
        }
        void _printUsers(AdminPanel parent, List<Dictionary<string, string>> list)
        {
            foreach (var item in list)
            {
                parent.consoleScreen.Text += ("ID: " + item["id"] + ", User: " + item["login"] + ", Email: " + item["email"] + ", First name: " + item["firstName"] + ", Last name: " + item["lastName"] + ", Type: " + item["role"] + "\n\n");
            }
        }

        void _printQuestions(AdminPanel parent, List<Question> list)
        {
            parent.InputBox.Text = "";
            parent.consoleScreen.Text = "";
            foreach (Question q in list)
            {
                parent.consoleScreen.Text += string.Format("ID: {0}, Name: {1}, Category: {3}, Question type {4}\n Text: {2}\n",
                    q.GetID(), q.Name, q.Text, q.Category, q.QuestionType);
            }
        }
        bool _createUser(string[] data)
        {
            if (data.Length != 6)
                return false;
            User.TYPE type = User.StringToType(data[5]);

            var user = new User(0, data[0], data[1], data[2], data[3], data[4], type);
            App.DBConnection.AddUser(user);

            return true;
        }
        bool _createQuestion(string[] data)
        {
            bool answ_read_check = false;
            string name = "", text = "", cat = "", type = "", points = "";
            string answ = "";
            int answer_shift = 3;
            int corrAnsw = 0;
            for (int i = 0; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case "/n":
                        name = data[i + 1]; answ_read_check = false; i++;
                        break;
                    case "/tx":
                        answ_read_check = false; 
                        i++;
                        if (data[i].StartsWith("\"", StringComparison.CurrentCulture))
                        {
                            text = data[i].Substring(1, data[i].Length - 2);
                        }
                        else
                            text = data[i];
                        break;
                    case "/c":
                        cat = data[i + 1]; answ_read_check = false; i++;
                        break;
                    case "/p":
                        points = data[i + 1]; answ_read_check = false; i++;
                        break;
                    case "/t":
                        type = data[i + 1]; answ_read_check = false; i++;
                        break;
                    case "/answ":
                        answ_read_check = true;
                        break;

                    default:
                        if (!answ_read_check)
                            continue;
                        string s = data[i];
                        if (s.StartsWith("\"", StringComparison.CurrentCulture))
                        {
                            answ += s.Substring(1, s.Length - 2);
                        }
                        else
                        {
                            corrAnsw += int.Parse(s) << answer_shift;
                            answer_shift--;
                        }
                        break;
                }
            }
            if (text.Length > 0 && name.Length > 0 && points.Length > 0 && type.Length > 0 && answ.Length > 0)
            {
                double d = double.Parse(points, CultureInfo.InvariantCulture);
                Question q;
                if (cat.Length > 0) 
                    q = new Question(name, text, Question.StringToType(type), answ, d, corrAnsw, cat);
                else
                    q = new Question(name, text, Question.StringToType(type), answ, d, corrAnsw);
                q.PrintQuestionOnConsole();
                App.DBConnection.AddQuestion(q);
                return true;
            }
            return false;
        }
        bool _removeUser(string[] data)
        {
            int id;
            string s = data[0].Trim();
            id = int.Parse(s);
            App.DBConnection.RemoveUser(id);
            return true;
        }

        bool _modifyUser(AdminPanel parent, string[] data) // for now it's only by id
        {
            Debug.Print(data.Length.ToString());
            int id = -1;
            User user;
            string log = "", pass = "", email = "", fname = "", lname = "", type = "";
            for (int i = 0; i < data.Length; i += 2)
            {
                switch (data[i])
                {
                    case "/id":
                        id = int.Parse(data[i + 1]);
                        break;
                    case "/l":
                        log = data[i + 1];
                        break;
                    case "/p":
                        pass = data[i + 1];
                        break;
                    case "/e":
                        email = data[i + 1];
                        break;
                    case "/fn":
                        fname = data[i + 1];
                        break;
                    case "/ln":
                        lname = data[i + 1];
                        break;
                    case "/t":
                        type = data[i + 1];
                        break;
                }
            }
            if (id <= 0)
                return false;
            user = App.DBConnection.GetUserByID(id);
            user.SetID(id);
            if (log != "")
                user.Login = log;
            if (pass != "")
                user.Password = pass;
            if (email != "")
                user.Email = email;
            if (fname != "")
                user.FirstName = fname;
            if (lname != "")
                user.LastName = lname;
            if (log != "")
                user.UserType = User.StringToType(type);

            user = App.DBConnection.UpdateUser(user);

            parent.consoleScreen.Text = "";
            _printUsers(parent, user);

            return true;
        }

        public void GoBack()
        {
            _mainwindow.ChangeMainPageDataContext();
        }

        public void ReturnAllUsersFromDB(AdminPanel parent)
        {
            var list = App.DBConnection.ReturnUsersList();
            parent.consoleScreen.Text = "";
            _printUsers(parent, list);
        }

        public void ReturnAllQuestionsFromDB(AdminPanel parent)
        {
            var list = App.DBConnection.ReturnQuestionList();
            parent.consoleScreen.Text = "";
            _printQuestions(parent, list);
        }

        public void DeleteUser(AdminPanel parent)
        {
            StepMethod = DeleteUser;

            switch (parent.EnterCounter)
            {
                case 0:
                    parent.consoleScreen.Text = "Insert user to delete ID:\n";
                    break;
                case 1:
                    strings.Add(parent.InputBox.Text);
                    _removeUser(strings.ToArray());

                    StepMethod = null;
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "User removed";
                    parent.EnterCounter = 0;
                    strings.Clear();
                    break;
            }
        }

        public void AddNewUser(AdminPanel parent)
        {
            StepMethod = AddNewUser;
            // Add check logic
            switch (parent.EnterCounter)
            {
                case 0:
                    parent.consoleScreen.Text = "Insert user login:\n";
                    break;
                case 1:
                    strings.Add(parent.InputBox.Text);
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "Insert user password:\n";
                    break;
                case 2:
                    strings.Add(parent.InputBox.Text);
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "Insert user email:\n";
                    break;
                case 3:
                    strings.Add(parent.InputBox.Text);
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "Insert user name:\n";
                    break;
                case 4:
                    strings.Add(parent.InputBox.Text);
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "Insert user last name:\n";
                    break;
                case 5:
                    strings.Add(parent.InputBox.Text);
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "Insert user type in lowercase. Accepted types:\n* Student\n* Teacher\n* Admin\n";
                    break;
                case 6:
                    strings.Add(parent.InputBox.Text);
                    _createUser(strings.ToArray());

                    StepMethod = null;
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "User added";
                    parent.EnterCounter = 0;
                    strings.Clear();
                    break;
            }
            //var user = new User(0, "Domino", "ColonelSanders", "kcflover@gmail.kom", "Dominik", "Filipiak", User.TYPE.Student);
            //App.DBConnection.AddUser(user);
        }

        public void ModifyUser(AdminPanel parent)
        {
            StepMethod = ModifyUser;
            string s = "Leave space empty and press enter if you don\'t want to change this field";
            switch (parent.EnterCounter)
            {

                case 0:
                    parent.consoleScreen.Text = "User id";
                    parent.InputBox.Text = "";
                    strings.Add("/id");
                    break;
                case 1:
                    strings.Add(parent.InputBox.Text);
                    parent.consoleScreen.Text = "New login\n\n" + s;
                    parent.InputBox.Text = "";
                    strings.Add("/l");
                    break;
                case 2:
                    strings.Add(parent.InputBox.Text);
                    parent.consoleScreen.Text = "New password\n\n" + s;
                    parent.InputBox.Text = "";
                    strings.Add("/p");
                    break;
                case 3:
                    strings.Add(parent.InputBox.Text);
                    parent.consoleScreen.Text = "New email\n\n" + s;
                    parent.InputBox.Text = "";
                    strings.Add("/e");
                    break;
                case 4:
                    strings.Add(parent.InputBox.Text);
                    parent.consoleScreen.Text = "New first name\n\n" + s;
                    parent.InputBox.Text = "";
                    strings.Add("/fn");
                    break;
                case 5:
                    strings.Add(parent.InputBox.Text);
                    parent.consoleScreen.Text = "New last name\n\n" + s;
                    parent.InputBox.Text = "";
                    strings.Add("/ln");
                    break;
                case 6:
                    strings.Add(parent.InputBox.Text);
                    parent.consoleScreen.Text = "New type. Insert user type in lowercase. Accepted types:\n* Student\n* Teacher\n* Admin\n\n" + s;
                    parent.InputBox.Text = "";
                    strings.Add("/t");
                    break;
                case 7:
                    strings.Add(parent.InputBox.Text);
                    _modifyUser(parent, strings.ToArray());

                    StepMethod = null;
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "User changed";
                    parent.EnterCounter = 0;
                    strings.Clear();
                    break;
            }

        }

        public void AddNewQuestion(AdminPanel parent)
        {
            StepMethod = ModifyUser;

            switch (parent.EnterCounter)
            {

                case 0:

                    break;
                case 1:

                    strings.Add("/l");
                    break;
                case 2:

                    strings.Add("/p");
                    break;
                case 3:

                    strings.Add("/e");
                    break;
                case 4:

                    strings.Add("/fn");
                    break;
                case 5:

                    strings.Add("/ln");
                    break;
                case 6:

                    strings.Add("/t");
                    break;
                case 7:

                    parent.EnterCounter = 0;
                    strings.Clear();
                    break;
            }
        }
    }
}
