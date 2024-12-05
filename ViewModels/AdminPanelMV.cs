using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD.Models;
using BD.Views;

namespace BD.ViewModels
{
    internal class AdminPanelMV
    {
        private readonly MainWindow _mainwindow;
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
            var list = App.DBConnection.ReturnUsersList(query);
            parent.consoleScreen.Text = "";
            foreach (var item in list)
            {
                parent.consoleScreen.Text += ("User: "+ item["login"]+", Email: "+ item["email"]+", First name: "+ item["firstName"]+", Last name: "+ item["lastName"]+", Type: " + item["role"]+"\n\n");
            }
        }

        /*
            INSERT INTO "User"(login, name, surname, email, password, role)
            VALUES
            ('Domino', 'Dominik', 'Filipiak', 'kfclover@gmail.kom', 'ColonelSanders', 'uczen')
         */
        public void AddNewUser(AdminPanel parent)
        {
            var user = new User(0, "Domino", "ColonelSanders", "kcflover@gmail.kom", "Dominik", "Filipiak", User.TYPE.Student);
            App.DBConnection.AddUser(user);
        }
    }
}
