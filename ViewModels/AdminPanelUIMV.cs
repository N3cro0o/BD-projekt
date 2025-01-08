using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD.Models;
using BD.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics.PerformanceData;
using Microsoft.Windows.Themes;
using System.Diagnostics;

namespace BD.ViewModels
{
    internal class AdminPanelUIMV
    {
        private readonly MainWindow _mainwindow;
        public bool showMenu = false;
        public AdminPanelUIMV(MainWindow mainWindow)
        {
            _mainwindow = mainWindow;
        }
        
        

        public void GoBack()
        {
            _mainwindow.ChangeMainPageDataContext();
        }

        public void ReturnAllUsersFromDB(AdminPanelUI parent)
        {
           
            parent.mainTitle.Text = "User List";

            parent.UserTable.Visibility = Visibility.Visible;
            parent.CoursesTable.Visibility = Visibility.Hidden;

            var list = App.DBConnection.ReturnUsersListOfUsers();

            parent.UserTable.ItemsSource = list;

        }

        public void AddNewUser(object sender, RoutedEventArgs e)
        {
            var user = new User(0, "Domino", "ColonelSanders", "kcflover@gmail.kom", "Dominik", "Filipiak", User.TYPE.Student);
            App.DBConnection.AddUser(user);
            MessageBox.Show($"Dodawanie uzytkownika");
        }
        public void AddNewCours(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Dodawanie kursu");
        }
        public void AddNewQuestion(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Dodawanie pytania");
        }
        public void ReturnAllCoursesFromDB(AdminPanelUI parent)
        {
            parent.mainTitle.Text = "Courses List";

            parent.UserTable.Visibility = Visibility.Hidden;
            parent.CoursesTable.Visibility = Visibility.Visible;

            var list = App.DBConnection.ReturnQuestionList();
            parent.CoursesTable.ItemsSource = list;
        }
        public void ShowMenu(AdminPanelUI parent)
        {

            if (showMenu==false)
            {
                parent.showToolBox.SetValue(Grid.ColumnProperty, 0);
                showMenu = true;
                parent.menuColumn.Width = new System.Windows.GridLength(2, GridUnitType.Star);

            }
            else
            {
                parent.showToolBox.SetValue(Grid.ColumnProperty, 1);
                showMenu = false;
                parent.menuColumn.Width = new System.Windows.GridLength(0, GridUnitType.Star);
            }
            
        }
        public void ShowAllStatistics(AdminPanelUI parent)
        {
            parent.mainTitle.Text = "Statistics";
            
            

        }
        public void ShowAllQusetions(AdminPanelUI parent)
        {
            parent.mainTitle.Text = "Questions";
            
        }
        
        
    }
}
