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
using System.Windows.Data;

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

        public void ReturnAllUsersFromDB(object sender, RoutedEventArgs e, AdminPanelUI parent)
        {
            parent.mainTitle.Text = "User List";

            // Ustaw widoczność tabel
            parent.UserTable.Visibility = Visibility.Visible;
            parent.CoursesTable.Visibility = Visibility.Hidden;

            // Pobranie listy użytkowników
            var list = App.DBConnection.ReturnUsersListOfUsers();
            parent.UserTable.ItemsSource = list;

            // Dodanie kolumn dynamicznie, jeśli jeszcze nie zostały dodane
            if (!parent.UserTable.Columns.Any(c => c.Header.ToString() == "Actions"))
            {
                // Kolumna z przyciskami
                var actionColumn = new DataGridTemplateColumn
                {
                    Header = "Actions",
                    Width = 150
                };

                // Definicja przycisku w kolumnie
                var buttonTemplate = new FrameworkElementFactory(typeof(Button));
                buttonTemplate.SetValue(Button.ContentProperty, "Delete");
                buttonTemplate.SetValue(Button.WidthProperty, 100.0);

                // Przypisanie zdarzenia Click
                buttonTemplate.AddHandler(Button.ClickEvent, new RoutedEventHandler((sender, e) => OnDeleteButton(sender, e, parent)));


                // Powiązanie danych z Tag przycisku
                var binding = new Binding
                {
                    Path = new PropertyPath(".") // Bieżący obiekt User
                };
                buttonTemplate.SetBinding(Button.TagProperty, binding);

                // Ustawienie szablonu komórki
                var cellTemplate = new DataTemplate
                {
                    VisualTree = buttonTemplate
                };

                actionColumn.CellTemplate = cellTemplate;

                // Dodanie kolumny do DataGrid
                parent.UserTable.Columns.Add(actionColumn);
            }
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
        public void OnDeleteButton(object sender, RoutedEventArgs e, AdminPanelUI parent)
        {
            // Rzutowanie sender na Button
            var button = sender as Button;
            if (button == null) return;

            // Pobranie użytkownika przypisanego do przycisku
            var user = button.Tag as User;
            if (user != null)
            {
                // Potwierdzenie usunięcia użytkownika
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete user: {user.Login}?",
                    "Delete Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Usunięcie użytkownika z bazy danych
                    bool isDeleted = App.DBConnection.RemoveUser(user.ID); // Funkcja RemoveUser

                    if (isDeleted)
                    {
                        MessageBox.Show($"User {user.Login} has been successfully deleted from the database.");

                        // Pobranie listy użytkowników z DataGrid
                        var list = parent.UserTable.ItemsSource as List<User>;
                        if (list != null)
                        {
                            // Usunięcie użytkownika z listy
                            list.Remove(user);

                            // Odświeżenie DataGrid
                            parent.UserTable.ItemsSource = null;
                            parent.UserTable.ItemsSource = list;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete user {user.Login} from the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

    }
}
