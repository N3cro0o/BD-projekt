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
            ErrorTextBlock.Text = "";
            _mv = App.Current.MainWindow.DataContext as LoginPageMV;
            LoginB.Focus();
        }

        private void OnLoginSubmit_click(object sender, RoutedEventArgs e)
        {
            Debug.Print(password);
            bool id = false;
            string error = "";
            (id, error) = _mv.Login(LoginB.Text, password);
            if (!id)
            {
                ErrorTextBlock.Text = error;
            }
            else
                PasswordB.Password = "";
        }
        
        private void Password_Changed(object sender, RoutedEventArgs e)
        {
            password = PasswordB.Password;   
        }

        private void OnLoginSubmit_key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OnLoginSubmit_click(sender, e);
        }

        private void OnGoback_Click(object sender, RoutedEventArgs e)
        {
            _mv.GoBack();
        }
    }
}
