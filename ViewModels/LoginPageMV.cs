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

        public (bool, string) Login(string login, string pass)
        {
            var list = App.DBConnection.Login(login, pass);

            if (list.Count == 1)
            {
                if (list[0]["role"].ToLower() != "admin")
                    return (false, "User does not have admin privileges");
                _mainwindow.Logged = true;
                _mainwindow.ChangeMainPageDataContext();
                return (true, "");
            }
            else if (list.Count > 1)
                return (false, "More than one user");
            else
                return (false, "User doesn't exist");
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
