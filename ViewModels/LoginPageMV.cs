using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.ViewModels
{
    public class LoginPageMV : INotifyPropertyChanged
    {
        private readonly MainWindow _mainwindow;

        public void Login(string login, string pass)
        {
            if (login == "Admin" && pass == "****")
            {
                _mainwindow.Logged = true;
                _mainwindow.ChangeMainPageDataContext();
            }
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
