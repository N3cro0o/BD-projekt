using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.ViewModels
{
    public class MainPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private MainWindow _mainwindow;

        public MainPageVM(MainWindow mainWindow)
        {
            _mainwindow = mainWindow;
        }

        public void GoToMainPage()
        {
            _mainwindow.ChangeMainPageDataContext();
        }
        public void GoToLogin()
        {
            _mainwindow.ChangeLoginDataContext();
        }
        public void Logoff()
        {
            _mainwindow.ChangeLogoffDataContext();
        }
        public void GoToAdminPanel()
        {
            _mainwindow.ChangeAdminPanelV2DataContext();
        }
        public void GoToAdminConsole()
        {
            _mainwindow.ChangeAdminPanelDataContext();
        }

        public bool CheckLogged()
        {
            return _mainwindow.Logged;
        }

    }
}
