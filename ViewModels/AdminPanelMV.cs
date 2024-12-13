using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using BD.Models;
using BD.Views;

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

            // Get instruction
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                {
                    strings.Add(s[i1..i]);
                    i1 = i + 1;
                }
            }
            // Add last word
            strings.Add(s[i1..(s.Length)]);

            // Select instruction
            if (strings[0].ToLower() == "create" || strings[0].ToLower() == "add")
            {
                switch (strings[1].ToLower())
                {
                    case "user":
                        if (strings[2].ToLower() == "help" || strings[2].ToLower() == "h")
                        {
                            parent.consoleScreen.Text = "Creates new user and insert them into database.\n\nHow it looks:\ncreate user <id> <login> <password> <email> <first name> <last name> <type>\nAccepted types:\n* Student\n* Teacher\n* Admin\n";
                            break;
                        }
                        if (_createUser(strings[2..strings.Count()].ToArray()))
                        {
                            var list = App.DBConnection.Login(strings[2], strings[3]);
                            parent.consoleScreen.Text = "";
                            foreach (var item in list)
                            {
                                parent.consoleScreen.Text += ("ID: " + item["id"] + ", User: " + item["login"] + ", Email: " + item["email"] + ", First name: " + item["firstName"] + ", Last name: " + item["lastName"] + ", Type: " + item["role"] + "\n\n");
                            }
                        }
                        break;
                }
            }
            if (strings[0].ToLower() == "show")
            {
                ReturnAllUsersFromDB(parent);
            }
            if (strings[0].ToLower() == "remove")
            {
                switch (strings[1].ToLower())
                {
                    case "user":
                        if (_removeUser(strings[2..strings.Count()].ToArray()))
                        {
                            parent.consoleScreen.Text = "User removed";
                        }
                        break;
                }
            }
            if (strings[0].ToLower() == "update")
            {
                switch (strings[1].ToLower())
                {
                    case "user":
                        if (strings[2].ToLower() == "help" || strings[2].ToLower() == "h")
                        {
                            parent.consoleScreen.Text = "Updates user and insert changed data into database.\n\nHow it looks:\nupdate user /id <id>\nAdditional flags:" +
                                " /l <login> /p <password> /e <email> /fn <first name> /ln <last name> /t <type>\n\nAccepted types:\n* Student\n* Teacher\n* Admin\n";
                            break;
                        }
                        if (_modifyUser(strings[2..strings.Count()].ToArray()))
                        {
                            parent.consoleScreen.Text = "User updated";
                        }
                        else
                        {
                            parent.consoleScreen.Text = "Error, user cannot be changed";
                        }
                        break;
                }
            }
            if (strings[0].ToLower() == "help" || strings[0].ToLower() == "h")
            {
                parent.consoleScreen.Text = "Possible commands:\n* show\n* create\n* update\n* remove";
            }

            strings.Clear();
            parent.InputBox.Text = "";
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

        bool _removeUser(string[] data)
        {
            int id;
            string s = data[0].Trim();
            id = int.Parse(s);
            App.DBConnection.RemoveUser(id);
            return true;
        }

        bool _modifyUser(string[] data) // for now it's only by id
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

            App.DBConnection.UpdateUser(user);

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
            foreach (var item in list)
            {
                parent.consoleScreen.Text += ("ID: " + item["id"] + ", User: " + item["login"] + ", Email: " + item["email"] + ", First name: " + item["firstName"] + ", Last name: " + item["lastName"] + ", Type: " + item["role"] + "\n\n");
            }
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
                    _modifyUser(strings.ToArray());

                    StepMethod = null;
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "User changed";
                    parent.EnterCounter = 0;
                    strings.Clear();
                    break;
            }

        }
    }
}
