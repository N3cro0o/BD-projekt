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
using System.Data;

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
            parent.CourseTable.Visibility = Visibility.Hidden;
            parent.addUser.Visibility = Visibility.Visible;
            parent.addCourse.Visibility = Visibility.Hidden;

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
                    Width = 200
                };

                // Kontener dla przycisków
                var panelTemplate = new FrameworkElementFactory(typeof(StackPanel));
                panelTemplate.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                // Definicja pierwszego przycisku (Delete)
                var deleteButton = new FrameworkElementFactory(typeof(Button));
                deleteButton.SetValue(Button.ContentProperty, "Delete");
                deleteButton.SetValue(Button.WidthProperty, 100.0);
                deleteButton.AddHandler(Button.ClickEvent, new RoutedEventHandler((sender, e) => OnDeleteUserButton(sender, e, parent)));

                // Powiązanie danych z Tag przycisku (Delete)
                var deleteBinding = new Binding
                {
                    Path = new PropertyPath(".") // Bieżący obiekt User
                };
                deleteButton.SetBinding(Button.TagProperty, deleteBinding);

                // Dodanie pierwszego przycisku do kontenera
                panelTemplate.AppendChild(deleteButton);

                // Definicja drugiego przycisku (ChangeRole)
                var changeRoleButton = new FrameworkElementFactory(typeof(Button));
                changeRoleButton.SetValue(Button.ContentProperty, "Change Role");
                changeRoleButton.SetValue(Button.WidthProperty, 100.0);
                changeRoleButton.AddHandler(Button.ClickEvent, new RoutedEventHandler((sender, e) => OnChangeRoleButton(sender, e, parent)));

                // Powiązanie danych z Tag przycisku (ChangeRole)
                var changeRoleBinding = new Binding
                {
                    Path = new PropertyPath(".") // Bieżący obiekt User
                };
                changeRoleButton.SetBinding(Button.TagProperty, changeRoleBinding);

                // Dodanie drugiego przycisku do kontenera
                panelTemplate.AppendChild(changeRoleButton);

                // Ustawienie szablonu komórki
                var cellTemplate = new DataTemplate
                {
                    VisualTree = panelTemplate
                };

                actionColumn.CellTemplate = cellTemplate;

                // Dodanie kolumny do DataGrid
                parent.UserTable.Columns.Add(actionColumn);
            }
        }
        
        
        

        public void AddNewUser(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new NewUserWindow();
            if (newUserWindow.ShowDialog() == true)
            {
                var user = newUserWindow.NewUser;
                App.DBConnection.AddUser(user);
                MessageBox.Show($"User {user.FirstName} {user.LastName} has been successfully added.");
            }
        }
        public void AddNewCours(object sender, RoutedEventArgs e)
        {
            var newCourseWindow = new NewCourseWindow();
            if (newCourseWindow.ShowDialog() == true)
            {
                var course = newCourseWindow.NewCours;
                App.DBConnection.AddCourse(course);
                MessageBox.Show($"Cours {course.Name} has been successfully added.");
            }
            
        }
        public void AddNewQuestion(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Dodawanie pytania");
        }
        public void ReturnAllCoursesFromDB(AdminPanelUI parent)
        {
            parent.mainTitle.Text = "Courses List";

            parent.UserTable.Visibility = Visibility.Hidden;
            parent.CourseTable.Visibility = Visibility.Visible;
            parent.addUser.Visibility = Visibility.Hidden;
            parent.addCourse.Visibility = Visibility.Visible;

            var list = App.DBConnection.ReturnCourseListOfCourses();
            parent.CourseTable.ItemsSource = list;

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
                buttonTemplate.AddHandler(Button.ClickEvent, new RoutedEventHandler((sender, e) => OnDeleteCoursesButton(sender, e, parent)));


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
        private void OnChangeRoleButton(object sender, RoutedEventArgs e, AdminPanelUI parent)
        {
            var button = sender as Button;
            var user = button?.Tag; // Pobranie danych użytkownika z Tag
            if (user != null)
            {
                var newChangeRoleWindow = new NewChangeRoleWindow();
                newChangeRoleWindow.Title = "Change Role" + user.GetType().Name;
                if (newChangeRoleWindow.ShowDialog() == true)
                {
                    var user1 = newChangeRoleWindow.NewUser;
                    bool update = App.DBConnection.UpdateUserRole(user1);
                    if ((update=true))
                    {
                        MessageBox.Show($"User {user1.FirstName} {user1.LastName} has been successfully changed Role to {newChangeRoleWindow.TypeComboBox.SelectedItem}.");
                    }
                    else
                    {
                        MessageBox.Show($"User {user1.FirstName} {user1.LastName} has been NOT successfully changed Role to {newChangeRoleWindow.TypeComboBox.SelectedItem}.");
                    }
                }
            }
        }
        public void OnDeleteUserButton(object sender, RoutedEventArgs e, AdminPanelUI parent)
        {
            
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
        public void OnDeleteCoursesButton(object sender, RoutedEventArgs e, AdminPanelUI parent)
        {
            var button = sender as Button;
            if (button == null) return;

            // Pobranie kursu przypisanego do przycisku
            var course = button.Tag as Course;
            if (course != null)
            {
                // Potwierdzenie usunięcia kursu
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete the course: {course.Name}?",
                    "Delete Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Usunięcie kursu z bazy danych
                    bool isDeleted = App.DBConnection.RemoveCourse(course.ID); // Funkcja RemoveCourse

                    if (isDeleted)
                    {
                        MessageBox.Show($"Course {course.Name} has been successfully deleted from the database.");

                        // Pobranie listy kursów z DataGrid
                        var list = parent.CourseTable.ItemsSource as List<Course>;
                        if (list != null)
                        {
                            // Usunięcie kursu z listy
                            list.Remove(course);

                            // Odświeżenie DataGrid
                            parent.CourseTable.ItemsSource = null;
                            parent.CourseTable.ItemsSource = list;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete the course {course.Name} from the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
