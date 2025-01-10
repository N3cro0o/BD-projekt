using BD.Models;
using BD.Views;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Globalization;

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

            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
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

            context = universalItems(parent, context);

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
            parent.type = User.TYPE.Student;

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
            radio.IsChecked = true;
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
                    if (!string.IsNullOrEmpty(login.Text) && !string.IsNullOrEmpty(email.Text) && !string.IsNullOrEmpty(pass.Text) && !string.IsNullOrEmpty(fname.Text) && !string.IsNullOrEmpty(lname.Text))
                    {
                        User u = new User(0, login.Text, pass.Text, email.Text, fname.Text, lname.Text, parent.type);
                        u.DebugPrintUser();
                        if (App.DBConnection.AddUser(u))
                        {
                            ReturnAllUsersFromDB(parent);
                            return;
                        }
                    }
                    // Add error handling
                    pass.Text = "";
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
            item.Click += (s, e) =>
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

            context = universalItems(parent, context);

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

            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
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
                Header = "Main Teacher",
                Binding = new System.Windows.Data.Binding("MainTeacherName"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Course category",
                Binding = new System.Windows.Data.Binding("Category"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Students count",
                Binding = new System.Windows.Data.Binding("Students.Count"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Description",
                Binding = new System.Windows.Data.Binding("Description"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            var context = new ContextMenu();
            var item = new MenuItem { Header = "Delete Course" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Course c)
                {
                    MessageBoxResult result = MessageBox.Show(
                            $"Do you want to remove: {c.Name}?",
                            "Remove user",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning
                        );
                    if (result == MessageBoxResult.Yes)
                    {
                        App.DBConnection.RemoveCourse(c.ID);
                        MessageBox.Show($"User has been removed.");
                        ReturnAllCoursesFromDB(parent);
                    }
                }
            };
            context.Items.Add(item);

            item = new MenuItem { Header = "Show all tests" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Course c)
                {
                    ReturnAllTestsFromDB(parent, c.ID);
                }
            };
            context.Items.Add(item);

            context = universalItems(parent, context);

            var style = new Style(typeof(DataGridRow));
            style.Setters.Add(new Setter(DataGridRow.ContextMenuProperty, context));
            myDataGrid.RowStyle = style;

            var list = App.DBConnection.ReturnCoursesList();
            list[0].Teachers[0].DebugPrintUser();
            Debug.Print(list[0].MainTeacherName);
            myDataGrid.ItemsSource = list;
            parent.outputGrid.Children.Add(myDataGrid);
            Grid.SetRow(myDataGrid, 1);
        }

        public void ReturnAllTestsFromDB(AdminPanelUI parent, int courseID = -1)
        {
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "Test list";
            if (courseID > -1)
                parent.mainTitle.Text += $" for Course {App.DBConnection.ReturnCoursesListWithID(courseID)[0].Name}";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }

            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
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
                Header = "Category",
                Binding = new System.Windows.Data.Binding("Category"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Start Date",
                Binding = new System.Windows.Data.Binding("StartDate"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "End Date",
                Binding = new System.Windows.Data.Binding("EndDate"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Course name",
                Binding = new System.Windows.Data.Binding("CourseObject.Name"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });

            var context = new ContextMenu();
            context = universalItems(parent, context);

            var style = new Style(typeof(DataGridRow));
            style.Setters.Add(new Setter(DataGridRow.ContextMenuProperty, context));
            myDataGrid.RowStyle = style;
            List<Test> list;
            if (courseID > -1)
                list = App.DBConnection.ReturnCourseTestsList(courseID);
            else
                list = App.DBConnection.ReturnTestsList();
            myDataGrid.ItemsSource = list;
            parent.outputGrid.Children.Add(myDataGrid);
            Grid.SetRow(myDataGrid, 1);

        }

        public void AddNewCourse(AdminPanelUI parent)
        {
            Debug.Print(parent.type.ToString());
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "Create new Course";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }
            parent.ResetParams();

            var stacking_panel = new StackPanel();
            stacking_panel.Margin = new Thickness(50, 15, 50, 15);
            parent.outputGrid.Children.Add(stacking_panel);

            /*
                Name
                Category
                Desc
                Teacher

                Student
                Test
            */
            var text = new TextBlock();
            text.Text = "Course name";
            stacking_panel.Children.Add(text);

            var input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Category";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Description";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Main teacher";
            stacking_panel.Children.Add(text);

            var scroll = new ScrollViewer();
            var stacking_panel_inner = new StackPanel();
            scroll.Content = stacking_panel_inner;
            stacking_panel.Children.Add(scroll);

            var list = App.DBConnection.ReturnTeacherList();
            foreach (User u in list)
            {
                var radio = new RadioButton();
                radio.Click += (s, e) =>
                {
                    for (int i = 0; i < parent.IDs.Count; i++)
                    {
                        if (parent.radios[i].IsChecked == true)
                        {
                            Debug.Print(parent.IDs[i].ToString());
                        }
                    }
                };
                radio.Content = $"{u.Login}, {u.FirstName} {u.LastName}";
                parent.radios.Add(radio);
                stacking_panel_inner.Children.Add(radio);
                parent.IDs.Add(u.ID);
            }
            // Submit
            Button bttn = new Button();
            bttn.Content = "Submit";
            bttn.Click += (o, e) =>
            {
                var name = (stacking_panel.Children[1] as TextBox);
                var cat = (stacking_panel.Children[3] as TextBox);
                var desc = (stacking_panel.Children[5] as TextBox);
                int teach = -1;
                for (int i = 0; i < parent.IDs.Count; i++)
                {
                    if (parent.radios[i].IsChecked == true)
                    {
                        teach = parent.IDs[i];
                    }
                }
                if (name == null || cat == null || teach < 0)
                    return;
                if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(cat.Text))
                    return;

                User u = App.DBConnection.GetUserByID(teach);
                Course c = new Course(0, name.Text, cat.Text, new List<User>([u]));
                if (desc != null && !string.IsNullOrEmpty(desc.Text))
                    c.Description = desc.Text;

                if (App.DBConnection.AddCourse(c))
                {
                    ReturnAllCoursesFromDB(parent);
                }
                else
                {   // Add error handling
                    name.Text = "";
                    cat.Text = "";
                }
            };
            stacking_panel.Children.Add(bttn);

        }

        public void AddNewTest(AdminPanelUI parent)
        {
            Debug.Print(parent.type.ToString());
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "Create new Course";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }
            parent.ResetParams();

            var stacking_panel = new StackPanel();
            stacking_panel.Margin = new Thickness(50, 15, 50, 15);
            parent.outputGrid.Children.Add(stacking_panel);

            /*
                Name
                Course
                Category
                Start
                End

                Questions
            */
            var text = new TextBlock();
            text.Text = "Test name";
            stacking_panel.Children.Add(text);

            var input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Category";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Course";
            stacking_panel.Children.Add(text);

            var scroll = new ScrollViewer();
            var stacking_panel_inner = new StackPanel();
            scroll.Content = stacking_panel_inner;
            stacking_panel.Children.Add(scroll);
            var list = App.DBConnection.ReturnCoursesList();
            foreach (Course u in list)
            {
                var radio = new RadioButton();
                radio.Click += (s, e) =>
                {
                    for (int i = 0; i < parent.IDs.Count; i++)
                    {
                        if (parent.radios[i].IsChecked == true)
                        {
                            Debug.Print(parent.IDs[i].ToString());
                        }
                    }
                };
                radio.Content = $"{u.Name}, {u.MainTeacherName}";
                parent.radios.Add(radio);
                stacking_panel_inner.Children.Add(radio);
                parent.IDs.Add(u.ID);
            }

            stacking_panel_inner = new StackPanel();
            stacking_panel_inner.Orientation = Orientation.Horizontal;
            stacking_panel_inner.HorizontalAlignment = HorizontalAlignment.Center;
            stacking_panel.Children.Add(stacking_panel_inner);
            var textBlock = new TextBlock();
            textBlock.Text = "Start date:";
            var cal = new DatePicker();
            cal.SelectedDate = DateTime.Now.AddDays(1);
            stacking_panel_inner.Children.Add(textBlock);
            stacking_panel_inner.Children.Add(cal);

            textBlock = new TextBlock();
            textBlock.Text = "End date:";
            cal = new DatePicker();
            cal.SelectedDate = DateTime.Now.AddDays(8);
            stacking_panel_inner.Children.Add(textBlock);
            stacking_panel_inner.Children.Add(cal);

            text = new TextBlock();
            text.Text = "Questions. Please select desired ones:";
            stacking_panel.Children.Add(text);

            scroll = new ScrollViewer();
            stacking_panel_inner = new StackPanel();
            scroll.Content = stacking_panel_inner;
            stacking_panel.Children.Add(scroll);
            var list1 = App.DBConnection.ReturnQuestionList();
            foreach (Question q in list1)
            {
                // Add the rest of logic later
                var new_stack = new StackPanel();
                new_stack.Orientation = Orientation.Horizontal;
                var toggle = new ToggleButton();
                toggle.Content = "X";
                textBlock = new TextBlock();
                textBlock.Text = q.Text;

                new_stack.Children.Add(toggle);
                new_stack.Children.Add(textBlock);
                stacking_panel_inner.Children.Add(new_stack);
            }

            // Submit
            Button bttn = new Button();
            bttn.Content = "Submit";
            bttn.Click += (o, e) =>
            {
                var name = (stacking_panel.Children[1] as TextBox);
                var cat = (stacking_panel.Children[3] as TextBox);

                var cal_start = (stacking_panel.Children[6] as StackPanel).Children[1] as DatePicker;
                var cal_end = (stacking_panel.Children[6] as StackPanel).Children[3] as DatePicker;
                int course = -1;
                for (int i = 0; i < parent.IDs.Count; i++)
                {
                    if (parent.radios[i].IsChecked == true)
                    {
                        course = parent.IDs[i];
                    }
                }
                if (name == null || cat == null || course < 0)
                    return;
                if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(cat.Text))
                    return;

                Course c = App.DBConnection.ReturnCoursesListWithID(course)[0];
                Test t = new Test(c, cat.Text, cal_start.SelectedDate.Value, cal_end.SelectedDate.Value);
                t.Name = name.Text;

                if (App.DBConnection.AddTest(t))
                {
                    ReturnAllTestsFromDB(parent, course);
                }
                else
                {   // Add error handling
                    name.Text = "";
                    cat.Text = "";
                }

            };
            stacking_panel.Children.Add(bttn);

        }

        public void AddNewQuestion(AdminPanelUI parent)
        {
            Debug.Print(parent.type.ToString());
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "Create new User";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }
            parent.ResetParams();
            parent.typeQuestion = Question.QUESTION_TYPE.Closed;

            var stacking_panel = new StackPanel();
            stacking_panel.Margin = new Thickness(50, 15, 50, 15);
            parent.outputGrid.Children.Add(stacking_panel);

            /*
             * Name +
             * Text +
             * Type +
             * Points +
             * Category +
             * Answers +
             * Correct answers +
             */
            var text = new TextBlock();
            text.Text = "Name";
            stacking_panel.Children.Add(text);

            var input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Category";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Points";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            text = new TextBlock();
            text.Text = "Question type";
            stacking_panel.Children.Add(text);

            var radio_panel = new StackPanel();
            radio_panel.Orientation = Orientation.Horizontal;
            radio_panel.HorizontalAlignment = HorizontalAlignment.Center;
            var radio = new RadioButton();
            radio.Content = "Closed";
            radio.Click += (o, e) =>
            {
                if (o.GetType() == typeof(RadioButton))
                {
                    parent.typeQuestion = Question.QUESTION_TYPE.Closed;
                }
            };
            radio.IsChecked = true;
            radio_panel.Children.Add(radio);
            radio = new RadioButton();
            radio.Content = "Open";
            radio.Click += (o, e) =>
            {
                if (o.GetType() == typeof(RadioButton))
                {
                    parent.typeQuestion = Question.QUESTION_TYPE.Open;
                }
            };
            radio_panel.Children.Add(radio);
            stacking_panel.Children.Add(radio_panel);

            text = new TextBlock();
            text.Text = "Question body";
            stacking_panel.Children.Add(text);

            input = new TextBox();
            stacking_panel.Children.Add(input);

            var uniform = new UniformGrid()
            {
                Columns = 2,
                Rows = 2
            };
            for (int i = 0; i < 4; i++)
            {
                // Add the rest of logic later
                var new_stack = new StackPanel();
                new_stack.Orientation = Orientation.Horizontal;
                var toggle = new ToggleButton();
                toggle.Content = "X";
                var textBox = new TextBox();
                textBox.Text = $"Answer {i + 1}";

                new_stack.Children.Add(toggle);
                new_stack.Children.Add(textBox);
                uniform.Children.Add(new_stack);
            }
            stacking_panel.Children.Add(uniform);

            // Submit

            /*
             * Name +
             * Text +
             * Type +
             * Points +
             * Category +
             * Answers +
             * Correct answers +
             */

            Button bttn = new Button();
            bttn.Content = "Submit";
            bttn.Click += (o, e) =>
            {
                var name = (stacking_panel.Children[1] as TextBox);
                var cat = (stacking_panel.Children[3] as TextBox);
                var points = (stacking_panel.Children[5] as TextBox);
                var text = (stacking_panel.Children[9] as TextBox);
                var answ1 = ((stacking_panel.Children[10] as UniformGrid).Children[0] as StackPanel).Children[1] as TextBox;
                var asnwBttn1 = ((stacking_panel.Children[10] as UniformGrid).Children[0] as StackPanel).Children[0] as ToggleButton;
                var answ2 = ((stacking_panel.Children[10] as UniformGrid).Children[1] as StackPanel).Children[1] as TextBox;
                var asnwBttn2 = ((stacking_panel.Children[10] as UniformGrid).Children[1] as StackPanel).Children[0] as ToggleButton;
                var answ3 = ((stacking_panel.Children[10] as UniformGrid).Children[2] as StackPanel).Children[1] as TextBox;
                var asnwBttn3 = ((stacking_panel.Children[10] as UniformGrid).Children[2] as StackPanel).Children[0] as ToggleButton;
                var answ4 = ((stacking_panel.Children[10] as UniformGrid).Children[3] as StackPanel).Children[1] as TextBox;
                var asnwBttn4 = ((stacking_panel.Children[10] as UniformGrid).Children[3] as StackPanel).Children[0] as ToggleButton;

                if (name != null && cat != null && points != null && text != null && answ1 != null && asnwBttn1 != null && answ2 != null && asnwBttn2 != null &&
                    answ3 != null && asnwBttn3 != null && answ4 != null && asnwBttn4 != null)
                {
                    // Now strings................
                    if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(cat.Text) || string.IsNullOrEmpty(points.Text) || string.IsNullOrEmpty(text.Text) ||
                    string.IsNullOrEmpty(answ1.Text) || string.IsNullOrEmpty(answ2.Text) || string.IsNullOrEmpty(answ3.Text) || string.IsNullOrEmpty(answ4.Text))
                        return;
                    double p = double.Parse(points.Text, CultureInfo.InvariantCulture);
                    // Answer key binary shenanigans
                    int key = 0;
                    key += asnwBttn1.IsChecked == true ? 1 << 3 : 0;
                    key += asnwBttn2.IsChecked == true ? 1 << 2 : 0;
                    key += asnwBttn3.IsChecked == true ? 1 << 1 : 0;
                    key += asnwBttn4.IsChecked == true ? 1 << 0 : 0;
                    string answ = answ1.Text + "\n";
                    answ += answ2.Text + "\n";
                    answ += answ3.Text + "\n";
                    answ += answ4.Text;
                    Question q = new Question(name.Text, text.Text, parent.typeQuestion, answ, p, key, cat.Text);
                    q.PrintQuestionOnConsole();
                    if (App.DBConnection.AddQuestion(q))
                    {
                        ShowAllQusetions(parent);
                        return;
                    }
                    // Add error handling
                    name.Text = "";
                    cat.Text = "";
                    text.Text = "";
                }
            };
            stacking_panel.Children.Add(bttn);
        }

        ContextMenu universalItems(AdminPanelUI parent, ContextMenu addTo)
        {
            addTo.Items.Add(new Separator()); // Users
            var item = new MenuItem { Header = "Add new User" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem)
                {
                    AddNewUser(parent);
                }
            };
            addTo.Items.Add(item);

            addTo.Items.Add(new Separator()); // Courses
            item = new MenuItem { Header = "Add new Course" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem)
                {
                    AddNewUser(parent);
                }
            };
            addTo.Items.Add(item);

            addTo.Items.Add(new Separator()); // Questions and tests
            //item = new MenuItem { Header = "Add new Course" };
            //item.Click += (s, e) => 
            //{
            //    if (s is MenuItem menuItem)
            //    {
            //        AddNewUser(parent);
            //    }
            //};
            //addTo.Items.Add(item);
            return addTo;
        }
    }
}
