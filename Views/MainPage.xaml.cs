using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        private readonly MainPageMV _mv;

        public MainPage()
        {
            InitializeComponent();
            //greetLabel.Text = "Witamy w panelu startowym aplikacji " + MainWindow._Title + ", życzymy miłego dnia";

            _mv = App.Current.MainWindow.DataContext as MainPageMV;
            _checkButtonVisibility();
        }

        private void _checkButtonVisibility()
        {
            if (_mv != null)
            {
                if (_mv.CheckLogged())
                {
                    loginButton.Visibility = Visibility.Collapsed;
                    // Visible buttons
                    logoffButton.Visibility = Visibility.Visible;
                    adminPanelButton.Visibility = Visibility.Visible;
                }
                else
                {
                    loginButton.Visibility = Visibility.Visible;
                    // Collapsed buttons
                    logoffButton.Visibility = Visibility.Collapsed;
                    adminPanelButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void GoToLogoff_Click(object sender, RoutedEventArgs e)
        {
            _mv.Logoff();
            _checkButtonVisibility();
        }

        private void GoToAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            _mv.GoToAdminPanel();
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            _mv.GoToLogin();
        }


    }
}
