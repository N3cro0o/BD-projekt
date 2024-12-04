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
        private MainWindow _mainwindow;

        public void T(string login, string pass)
        {
            if (login == "Admin" && pass == "****")
            {
                _mainwindow.Logged = true;
                _mainwindow.ChangeMainPageDataContext();
            }
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
