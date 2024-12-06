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
            _mainwindow.Logged = true;
            _mainwindow.ChangeMainPageDataContext();
            return true;
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
