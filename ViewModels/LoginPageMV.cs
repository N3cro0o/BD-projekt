using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BD.ViewModels
{
    public class LoginPageMV : INotifyPropertyChanged
    {
        private readonly MainWindow _mainwindow;

        public bool Login(string login, string pass)
        {
            string query = "SELECT * FROM \"User\" WHERE \"login\" = \'" + login + "\' AND \"password\" = \'" + pass + '\'';
            var list = App.DBConnection.ReturnUsersList(query);

            if (list.Count == 1)
            {
                if (list[0]["role"].ToLower() != "admin")
                    return false;
                _mainwindow.Logged = true;
                _mainwindow.ChangeMainPageDataContext();
                return true;
            }
            else if (list.Count > 1)
                Debug.Print("Found multiple users");
            else
                Debug.Print("User doesn't exist");

            return false;
            
        }

        public void GoBack()
        {
            _mainwindow.ChangeMainPageDataContext();
        }

        public LoginPageMV(MainWindow mw)
        {
            _mainwindow = mw;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
