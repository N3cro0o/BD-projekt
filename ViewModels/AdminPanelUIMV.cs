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
            
            // Tworzymy DataGrid
            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false, // Ręczne tworzenie kolumn
                Margin = new Thickness(10),
                AlternatingRowBackground = System.Windows.Media.Brushes.LightGray,
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#82827D")),
                BorderThickness = new Thickness(2),
                BorderBrush = System.Windows.Media.Brushes.Black
            };

            // Dodanie kolumn
            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ID",
                Binding = new System.Windows.Data.Binding("ID"),
                Width = new DataGridLength(2, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Login",
                Binding = new System.Windows.Data.Binding("Login"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Email",
                Binding = new System.Windows.Data.Binding("Email"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Name",
                Binding = new System.Windows.Data.Binding("FirstName"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Second Name",
                Binding = new System.Windows.Data.Binding("LastName"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Role",
                Binding = new System.Windows.Data.Binding("UserType"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });
            DataGridTemplateColumn actionColumn = new DataGridTemplateColumn
            {
                Header = "Action",
                Width = new DataGridLength(10, DataGridLengthUnitType.Star)
            };

            var context = new ContextMenu();
            var item = new MenuItem { Header = "Delete User" };
            item.Click += (s, e) => // LAMBDA SUPREMACY
            {
                if (s is MenuItem menuItem && menuItem.DataContext is User user)
                {
                    MessageBox.Show($"User ID: {user.ID}\n");
                }
            };
            context.Items.Add(item);
            context.Items.Add(new Separator());
            item = new MenuItem { Header = "Add new User" };
            // Click event
            context.Items.Add(item);

            var style = new Style(typeof(DataGridRow));
            style.Setters.Add(new Setter(DataGridRow.ContextMenuProperty, context));
            myDataGrid.RowStyle = style;

            var list = App.DBConnection.ReturnUsersListOfUsers();
            myDataGrid.ItemsSource = list;
            parent.outputGrid.Children.Add(myDataGrid);
            Grid.SetRow(myDataGrid, 1);
        }

        /*
            INSERT INTO "User"(login, name, surname, email, password, role)
            VALUES
            ('Domino', 'Dominik', 'Filipiak', 'kfclover@gmail.kom', 'ColonelSanders', 'uczen')
         */
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
            
            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false, // Ręczne tworzenie kolumn
                Margin = new Thickness(10),
                AlternatingRowBackground = System.Windows.Media.Brushes.LightGray,
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#82827D")),
                BorderThickness = new Thickness(2),
                BorderBrush = System.Windows.Media.Brushes.Black
            };

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Cours Name",
                Binding = new System.Windows.Data.Binding("Login"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Cattegory",
                Binding = new System.Windows.Data.Binding("Email"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Owner",
                Binding = new System.Windows.Data.Binding("FirstName"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Description",
                Binding = new System.Windows.Data.Binding("LastName"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

            parent.outputGrid.Children.Add(myDataGrid);
            Grid.SetRow(myDataGrid, 1);
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
