using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
