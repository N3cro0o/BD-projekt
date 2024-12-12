using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public void GoBack()
        {
            _mainwindow.ChangeMainPageDataContext();
        }

        public void ReturnAllUsersFromDB(AdminPanel parent)
        {
            string query = "SELECT * FROM \"User\"";
            var list = App.DBConnection.ReturnUsersList();
            parent.consoleScreen.Text = "";
            foreach (var item in list)
            {
                parent.consoleScreen.Text += ("ID: " + item["id"] +", User: " + item["login"] + ", Email: " + item["email"] + ", First name: " + item["firstName"] + ", Last name: " + item["lastName"] + ", Type: " + item["role"] + "\n\n");
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
                    int id;
                    string s = parent.InputBox.Text.Trim();
                    id = int.Parse(s);
                    
                    App.DBConnection.RemoveUser(id);

                    StepMethod = null;
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "User removed";
                    parent.EnterCounter = 0;
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
                    if (strings.Count == 5)
                    {
                        string s = parent.InputBox.Text.ToLower();
                        User.TYPE type;
                        if (s == "student")
                        {
                            type = User.TYPE.Student;
                        }
                        else if (s == "teacher")
                        {
                            type = User.TYPE.Teacher;
                        }
                        else if (s == "admin")
                        {
                            type = User.TYPE.Admin;
                        }
                        else
                        {
                            type = User.TYPE.Guest;
                        }

                        var user = new User(0, strings[0], strings[1], strings[2], strings[3], strings[4], type);
                        App.DBConnection.AddUser(user);
                    }
                    StepMethod = null;
                    parent.InputBox.Text = "";
                    parent.consoleScreen.Text = "User added";
                    parent.EnterCounter = 0;
                    break;
            }
            //var user = new User(0, "Domino", "ColonelSanders", "kcflover@gmail.kom", "Dominik", "Filipiak", User.TYPE.Student);
            //App.DBConnection.AddUser(user);
        }
    }
}
