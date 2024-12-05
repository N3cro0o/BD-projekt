using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
