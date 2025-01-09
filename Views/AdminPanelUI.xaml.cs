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
using BD.Models;
using BD.ViewModels;

namespace BD.Views
{
    public partial class AdminPanelUI : UserControl
    {
        private readonly AdminPanelUIMV _mv;

        public User.TYPE type;

        public AdminPanelUI()
        {
            InitializeComponent();
            _mv = App.Current.MainWindow.DataContext as AdminPanelUIMV;
            _mv.ShowMenu(this);
        }

        private void Goback_Click(object sender, RoutedEventArgs e)
        {
            _mv.GoBack();
        }

        private void ShowAllUsers_Click(object sender, RoutedEventArgs e)
        {
            _mv.ReturnAllUsersFromDB(this);
        }

        private void ShowAllCourses_Click(object sender, RoutedEventArgs e)
        {
            _mv.ReturnAllCoursesFromDB(this);
        }

        private void ShowMenu_Click(object sender, RoutedEventArgs e)
        {
            _mv.ShowMenu(this);
        }

        private void ShowAllQuestions_Click(object sender, RoutedEventArgs e)
        {
            _mv.ShowAllQusetions(this);
        }

        private void CreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            _mv.AddNewUser(this);
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            _mv.AddNewQuestion(this);
        }
    }
}
