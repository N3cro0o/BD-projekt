using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BD.ViewModels;

namespace BD.Views
{
    public partial class AdminPanel : UserControl
    {
        private readonly AdminPanelMV _mv;
        public AdminPanel()
        {
            InitializeComponent();

            _mv = App.Current.MainWindow.DataContext as AdminPanelMV;
        }

        private void OnPanelSubmit(object sender, RoutedEventArgs e)
        {
               
        }

        private void Goback_Click(object sender, RoutedEventArgs e)
        {
            _mv.GoBack();
        }

        private void ShowAllUsers_Click(object sender, RoutedEventArgs e)
        {
            _mv.ReturnAllUsersFromDB(this);
        }

        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            _mv.AddNewUser(this);
        }

        private void OnInputBox_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
