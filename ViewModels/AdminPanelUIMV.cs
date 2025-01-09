using BD.Models;
using BD.Views;
using System.Windows;
using System.Windows.Controls;
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
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "User list";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }


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
                Header = "Passy",
                Binding = new System.Windows.Data.Binding("Password"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "First Name",
                Binding = new System.Windows.Data.Binding("FirstName"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Last Name",
                Binding = new System.Windows.Data.Binding("LastName"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "User role",
                Binding = new System.Windows.Data.Binding("UserType"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            var context = new ContextMenu();
            var item = new MenuItem { Header = "Delete User" };
            item.Click += (s, e) => // LAMBDA SUPREMACY
            {
                if (s is MenuItem menuItem && menuItem.DataContext is User user)
                {
                    MessageBoxResult result = MessageBox.Show(
                            $"Do you want to remove: {user.Login}?",
                            "Remove user",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning
                        );
                    if (result == MessageBoxResult.Yes)
                    {
                        App.DBConnection.RemoveUser(user.ID);
                        MessageBox.Show($"User has been removed.");
                        ReturnAllUsersFromDB(parent);
                    }
                }
            };
            context.Items.Add(item);
            context.Items.Add(new Separator());
            item = new MenuItem { Header = "Add new User" };
            item.Click += (s, e) => // LAMBDA SUPREMACY
            {
                if (s is MenuItem menuItem)
                {
                    AddNewUser(parent);
                }
            };
            context.Items.Add(item);

            var style = new Style(typeof(DataGridRow));
            style.Setters.Add(new Setter(DataGridRow.ContextMenuProperty, context));
            myDataGrid.RowStyle = style;

            var list = App.DBConnection.ReturnUsersListOfUsers();
            list[0].DebugPrintUser();
            myDataGrid.ItemsSource = list;
            parent.outputGrid.Children.Add(myDataGrid);
            Grid.SetRow(myDataGrid, 1);
        }

        // Add error handling
        public void AddNewUser(AdminPanelUI parent)
        {
            Debug.Print(parent.type.ToString());
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "Create new User";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }
            var stacking_panel = new StackPanel();
            stacking_panel.Margin = new Thickness(50, 15, 50, 15);
            parent.outputGrid.Children.Add(stacking_panel);

            // Login
            // Email
            // FName
            // LName
            // Password
            // Type
            var text = new TextBlock();
            text.Text = "Login";
            stacking_panel.Children.Add(text);

            var input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Email";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "First name";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Last name";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Password";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Type";
            stacking_panel.Children.Add(text);

            var radio_panel = new StackPanel();
            radio_panel.Orientation = Orientation.Horizontal;
            radio_panel.HorizontalAlignment = HorizontalAlignment.Center;
            var radio = new RadioButton();
            radio.Content = "Student";
            radio.Click += (o, e) =>
            {
                if (o.GetType() == typeof(RadioButton))
                {
                    parent.type = User.TYPE.Student;
                }
            };
            radio_panel.Children.Add(radio);
            radio = new RadioButton();
            radio.Content = "Teacher";
            radio.Click += (o, e) =>
            {
                if (o.GetType() == typeof(RadioButton))
                {
                    parent.type = User.TYPE.Teacher;
                }
            };
            radio_panel.Children.Add(radio);
            radio = new RadioButton();
            radio.Content = "Admin";
            radio.Click += (o, e) =>
            {
                if (o.GetType() == typeof(RadioButton))
                {
                    parent.type = User.TYPE.Admin;
                }
            };
            radio_panel.Children.Add(radio);
            stacking_panel.Children.Add(radio_panel);
            // Submit
            Button bttn = new Button();
            bttn.Content = "Submit";
            bttn.Click += (o, e) =>
            {
                var login = stacking_panel.Children[1] as TextBox;
                var email = stacking_panel.Children[3] as TextBox;
                var fname = stacking_panel.Children[5] as TextBox;
                var lname = stacking_panel.Children[7] as TextBox;
                var pass = stacking_panel.Children[9] as TextBox;

                // Check for more mistakes
                if (login != null && email != null && pass != null && fname != null && lname != null)
                {
                    User u = new User(0, login.Text, pass.Text, email.Text, fname.Text, lname.Text, parent.type);
                    u.DebugPrintUser();
                    if (App.DBConnection.AddUser(u))
                    {
                        ReturnAllUsersFromDB(parent);
                    }
                    else
                    {   // Add error handling
                        pass.Text = "";
                    }
                }
            };
            stacking_panel.Children.Add(bttn);
        }

        public void ShowMenu(AdminPanelUI parent)
        {

            if (showMenu == false)
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

        public void ShowAllQusetions(AdminPanelUI parent)
        {
            parent.mainTitle.Text = "Create new User";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }

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
                Header = "Name",
                Binding = new System.Windows.Data.Binding("Name"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Question text",
                Binding = new System.Windows.Data.Binding("Text"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Question category",
                Binding = new System.Windows.Data.Binding("Category"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Points",
                Binding = new System.Windows.Data.Binding("Points"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Answer Body",
                Binding = new System.Windows.Data.Binding("Answers"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });
            
            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Correct answer key",
                Binding = new System.Windows.Data.Binding("CorrectAnswersBinary"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Shared",
                Binding = new System.Windows.Data.Binding("Shared"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            var context = new ContextMenu();
            var item = new MenuItem { Header = "Delete Question" };
            item.Click += (s, e) => // LAMBDA SUPREMACY
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Question question)
                {
                    MessageBox.Show("Nah");
                    //MessageBoxResult result = MessageBox.Show(
                    //        $"Do you want to remove: {user.Login}?",
                    //        "Remove user",
                    //        MessageBoxButton.YesNo,
                    //        MessageBoxImage.Warning
                    //    );
                    //if (result == MessageBoxResult.Yes)
                    //{
                    //    App.DBConnection.RemoveUser(user.ID);
                    //    MessageBox.Show($"User has been removed.");
                    //    ReturnAllUsersFromDB(parent);
                    //}
                }
            };
            context.Items.Add(item);
            context.Items.Add(new Separator());
            item = new MenuItem { Header = "Add new User" };
            item.Click += (s, e) => // LAMBDA SUPREMACY
            {
                if (s is MenuItem menuItem)
                {
                    AddNewUser(parent);
                }
            };
            context.Items.Add(item);

            var style = new Style(typeof(DataGridRow));
            style.Setters.Add(new Setter(DataGridRow.ContextMenuProperty, context));
            myDataGrid.RowStyle = style;

            var list = App.DBConnection.ReturnQuestionList();
            myDataGrid.ItemsSource = list;
            parent.outputGrid.Children.Add(myDataGrid);
            Grid.SetRow(myDataGrid, 1);

        }

        public void ReturnAllCoursesFromDB(AdminPanelUI parent)
        {
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "Course list";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }

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
                Header = "Name",
                Binding = new System.Windows.Data.Binding("Name"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Question text",
                Binding = new System.Windows.Data.Binding("Text"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Question category",
                Binding = new System.Windows.Data.Binding("Category"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Points",
                Binding = new System.Windows.Data.Binding("Points"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Answer Body",
                Binding = new System.Windows.Data.Binding("Answers"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Correct answer key",
                Binding = new System.Windows.Data.Binding("CorrectAnswersBinary"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Shared",
                Binding = new System.Windows.Data.Binding("Shared"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

        }
        public void AddNewQuestion(AdminPanelUI parent)
        {

        }
    }
}
