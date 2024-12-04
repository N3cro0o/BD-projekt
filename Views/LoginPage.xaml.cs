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
    /// <summary>
    /// Logika interakcji dla klasy LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        string? password;
        private LoginPageMV _mv;

        public LoginPage()
        {
            InitializeComponent();
            _mv = App.Current.MainWindow.DataContext as LoginPageMV;
        }

        private void OnLoginSubmit_click(object sender, RoutedEventArgs e)
        {
            Debug.Print(password);
            _mv.T(LoginB.Text, password);
        }

        private void Password_Changed(object sender, RoutedEventArgs e)
        {
            password = PasswordB.Password;   
        }
    }
}
