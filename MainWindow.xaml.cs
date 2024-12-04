using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BD
{
    public partial class MainWindow : Window
    {
        public static string? _Title;
        string str = "";

        private bool _logged;

        public bool Logged
        {
            get { return _logged; }
            set 
            {
                _logged = value;
                if (_logged)
                {
                    logoffButton.Visibility = Visibility.Visible;
                    loginButton.Visibility = Visibility.Collapsed;
                    adminPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    logoffButton.Visibility = Visibility.Collapsed;
                    loginButton.Visibility = Visibility.Visible;
                    adminPanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        public object logged_string = Visibility.Hidden;

        public MainWindow()
        {
            InitializeComponent();
            _Title = Title;
            Logged = false;

            DataContext = new ViewModels.MainPageMV();
        }

        private void GoToMainPage_Click(object sender, RoutedEventArgs e)
        {
            ChangeMainPageDataContext();
            str = "Test";
        }

        public void ChangeMainPageDataContext()
        {
            DataContext = new ViewModels.MainPageMV();
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new ViewModels.LoginPageMV(this);
        }

        private void GoToLogoff_Click(object sender, RoutedEventArgs e)
        {
            Logged = false;
            DataContext = new ViewModels.MainPageMV();
        }

        private void GoToAdmin_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new ViewModels.AdminPanelMV();
        }
    }
}