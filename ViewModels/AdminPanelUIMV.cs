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
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/DataGridStyles.xaml")
            };
            Style customDataGridStyle = (Style)resourceDictionary["CustomDataGridStyle"];
            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                //Style = (Style)Application.Current.Resources["CustomDataGridStyle"]
                //Margin = new Thickness(10),
                //AlternatingRowBackground = System.Windows.Media.Brushes.LightGray,
                //Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#82827D")),
                //BorderThickness = new Thickness(2),
                //BorderBrush = System.Windows.Media.Brushes.Black
            };

            // Dodanie kolumn
            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ID",
                Binding = new System.Windows.Data.Binding("ID"),
                Width = new DataGridLength(4, DataGridLengthUnitType.Star)
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
            myDataGrid.Style = customDataGridStyle;

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

            // Załaduj ResourceDictionary z pliku stylów
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/FormStyles.xaml")
            };

            // Dodaj style do zasobów aplikacji (jeśli nie zostały już dodane)
            if (!Application.Current.Resources.MergedDictionaries.Contains(resourceDictionary))
            {
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            // Używanie stylów
            var stacking_panel = new StackPanel
            {
                Style = (Style)Application.Current.Resources["FormStackPanelStyle"]
            };
            parent.outputGrid.Children.Add(stacking_panel);

            // Login
            var text = new TextBlock
            {
                Text = "Login",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            var input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            text = new TextBlock
            {
                Text = "Email",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            text = new TextBlock
            {
                Text = "First name",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            text = new TextBlock
            {
                Text = "Last name",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            text = new TextBlock
            {
                Text = "Password",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            text = new TextBlock
            {
                Text = "Type",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            var radio_panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            stacking_panel.Children.Add(radio_panel);

            var radio = new RadioButton
            {
                Content = "Student",
                Style = (Style)Application.Current.Resources["FormRadioButtonStyle"],
                IsChecked = true
            };
            radio.Click += (o, e) =>
            {
                if (o is RadioButton)
                {
                    parent.type = User.TYPE.Student;
                }
            };
            radio_panel.Children.Add(radio);

            radio = new RadioButton
            {
                Content = "Teacher",
                Style = (Style)Application.Current.Resources["FormRadioButtonStyle"]
            };
            radio.Click += (o, e) =>
            {
                if (o is RadioButton)
                {
                    parent.type = User.TYPE.Teacher;
                }
            };
            radio_panel.Children.Add(radio);

            radio = new RadioButton
            {
                Content = "Admin",
                Style = (Style)Application.Current.Resources["FormRadioButtonStyle"]
            };
            radio.Click += (o, e) =>
            {
                if (o is RadioButton)
                {
                    parent.type = User.TYPE.Admin;
                }
            };
            radio_panel.Children.Add(radio);

            // Submit
            var bttn = new Button
            {
                Content = "Submit",
                Style = (Style)Application.Current.Resources["FormButtonStyle"]
            };
            bttn.Click += (o, e) =>
            {
                var login = stacking_panel.Children[1] as TextBox;
                var email = stacking_panel.Children[3] as TextBox;
                var fname = stacking_panel.Children[5] as TextBox;
                var lname = stacking_panel.Children[7] as TextBox;
                var pass = stacking_panel.Children[9] as TextBox;

                if (login != null && email != null && pass != null && fname != null && lname != null)
                {
                    if (!string.IsNullOrEmpty(login.Text) &&
                        !string.IsNullOrEmpty(email.Text) &&
                        !string.IsNullOrEmpty(pass.Text) &&
                        !string.IsNullOrEmpty(fname.Text) &&
                        !string.IsNullOrEmpty(lname.Text))
                    {
                        User u = new User(0, login.Text, pass.Text, email.Text, fname.Text, lname.Text, parent.type);
                        u.DebugPrintUser();
                        if (App.DBConnection.AddUser(u))
                        {
                            ReturnAllUsersFromDB(parent);
                            return;
                        }
                    }
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
            
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/DataGridStyles.xaml", UriKind.Absolute)
            };

            Style customDataGridStyle = (Style)resourceDictionary["CustomDataGridStyle"];
            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                Style = customDataGridStyle
                /*Margin = new Thickness(10),
                AlternatingRowBackground = System.Windows.Media.Brushes.LightGray,
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#82827D")),
                BorderThickness = new Thickness(2),
                BorderBrush = System.Windows.Media.Brushes.Black*/
            };

            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ID",
                Binding = new System.Windows.Data.Binding("ID"),
                Width = new DataGridLength(4, DataGridLengthUnitType.Star)
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
                Header = "Shared",
                Binding = new System.Windows.Data.Binding("Shared"),
                Width = new DataGridLength(20, DataGridLengthUnitType.Star)
            });
            
            var context = new ContextMenu();

            var item = new MenuItem { Header = "Show answers for question" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Question question)
                {
                    ReturnAnswerForQuestion(parent, question.ID);
                }
            };
            context.Items.Add(item);

            item = new MenuItem { Header = "Delete Question" };
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

        public void ReturnAnswerForQuestion(AdminPanelUI parent, int question_id)
        {
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }
            var q = App.DBConnection.ReturnQuestionListByID(question_id)[0];
            var a = App.DBConnection.FetchAnswer(q.AnswerID);
            parent.mainTitle.Text = $"Answers for question {q.Name}";
            q.PrintQuestionOnConsole();

            var stacking_panel = new StackPanel();
            stacking_panel.Margin = new Thickness(50, 15, 50, 15);
            parent.outputGrid.Children.Add(stacking_panel);

            var stacking_panel_inner = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            Button button = new Button()
            {
                Content = "Go back"
            };
            button.Click += (o, e) => 
            {
                ShowAllQusetions(parent);
            };
            stacking_panel_inner.Children.Add(button);
            stacking_panel.Children.Add(stacking_panel_inner);

            TextBlock block = new TextBlock()
            {
                Text = "Question Body",
                FontWeight = FontWeights.Bold,
            };
            stacking_panel.Children.Add(block);

            block = new TextBlock()
            {
                Text = q.Text,
            };
            stacking_panel.Children.Add(block);
            block = new TextBlock()
            {
                Text = "Answers",
                FontWeight = FontWeights.Bold,
            };
            stacking_panel.Children.Add(block);

            string[] answer_list;
            answer_list = a.AnswerBody.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string t = "Correct";
            string t1 = "Incorrect";
            if (answer_list.Length == 4 && q.QuestionType == Question.QUESTION_TYPE.Closed)
            {
                var uniform = new UniformGrid()
                {
                    Columns = 2,
                    Rows = 2
                };
                for (int i = 0; i < 4; i++)
                {
                    var new_stack = new StackPanel();
                    new_stack.Orientation = Orientation.Horizontal;
                    var textBlock = new TextBlock();
                    string t_this = (a.AnswerKey & (1 << (3 - i))) >> (3 - i) == 1 ? t : t1;
                    textBlock.Text = $"{i + 1}. {answer_list[i]} - {t_this}";
                    new_stack.Children.Add(textBlock);
                    uniform.Children.Add(new_stack);
                }
                stacking_panel.Children.Add(uniform);
            }
            else
            {
                block = new TextBlock()
                {
                    Text = a.AnswerBody,
                };
                stacking_panel.Children.Add(block);

                if (q.QuestionType == Question.QUESTION_TYPE.Closed)
                {
                    block = new TextBlock()
                    {
                        Text = "Answer key",
                    };
                    stacking_panel.Children.Add(block);
                    for (int i = 0; i < 4; i++)
                    {
                        string t_this = (a.AnswerKey & (1 << (3 - i))) >> (3 - i) == 1 ? t : t1;
                        block = new TextBlock()
                        {
                            Text = $"{i + 1}. {t_this}",
                        };
                        stacking_panel.Children.Add(block);
                    }
                }
            }
        }

        public void ReturnAllCoursesFromDB(AdminPanelUI parent)
        {
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "Course list";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }

            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/DataGridStyles.xaml", UriKind.Absolute)
            };

            Style customDataGridStyle = (Style)resourceDictionary["CustomDataGridStyle"];
            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                Style = customDataGridStyle,
                /*Margin = new Thickness(10),
                AlternatingRowBackground = System.Windows.Media.Brushes.LightGray,
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#82827D")),
                BorderThickness = new Thickness(2),
                BorderBrush = System.Windows.Media.Brushes.Black*/
            };

            // Dodanie kolumn
            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ID",
                Binding = new System.Windows.Data.Binding("ID"),
                Width = new DataGridLength(4, DataGridLengthUnitType.Star)
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
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/DataGridStyles.xaml", UriKind.Absolute)
            };

            Style customDataGridStyle = (Style)resourceDictionary["CustomDataGridStyle"];
            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                Style = customDataGridStyle,
                /*Margin = new Thickness(10),
                AlternatingRowBackground = System.Windows.Media.Brushes.LightGray,
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#82827D")),
                BorderThickness = new Thickness(2),
                BorderBrush = System.Windows.Media.Brushes.Black*/
            };

            // Dodanie kolumn
            myDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ID",
                Binding = new System.Windows.Data.Binding("ID"),
                Width = new DataGridLength(4, DataGridLengthUnitType.Star)
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

            // Załaduj ResourceDictionary z pliku stylów
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/FormStyles.xaml")
            };

            // Dodaj style do zasobów aplikacji (jeśli jeszcze nie zostały dodane)
            if (!Application.Current.Resources.MergedDictionaries.Contains(resourceDictionary))
            {
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            // Tworzenie StackPanel
            var stacking_panel = new StackPanel
            {
                Style = (Style)Application.Current.Resources["FormStackPanelStyle"]
            };
            parent.outputGrid.Children.Add(stacking_panel);

            /*
                Name
                Category
                Desc
                Teacher
            */

            // Course name
            var text = new TextBlock
            {
                Text = "Course name",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            var input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            // Category
            text = new TextBlock
            {
                Text = "Category",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            // Description
            text = new TextBlock
            {
                Text = "Description",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            // Main teacher
            text = new TextBlock
            {
                Text = "Main teacher",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            // ScrollViewer for teacher selection
            var scroll = new ScrollViewer
            {
                Content = new StackPanel(),
                Style = (Style)Application.Current.Resources["FormScrollViewerStyle"]
            };

            var stacking_panel_inner = (StackPanel)scroll.Content;
            stacking_panel_inner.Style = (Style)Application.Current.Resources["FormInnerStackPanelStyle"];
            stacking_panel.Children.Add(scroll);

            // Add radio buttons for teachers
            var list = App.DBConnection.ReturnTeacherList();
            foreach (User u in list)
            {
                var radio = new RadioButton
                {
                    Content = $"{u.Login}, {u.FirstName} {u.LastName}",
                    Style = (Style)Application.Current.Resources["FormRadioButtonStyle"]
                };

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

                parent.radios.Add(radio);
                stacking_panel_inner.Children.Add(radio);
                parent.IDs.Add(u.ID);
            }

            // Submit button
            var bttn = new Button
            {
                Content = "Submit",
                Style = (Style)Application.Current.Resources["FormButtonStyle"]
            };

            bttn.Click += (o, e) =>
            {
                var name = stacking_panel.Children[1] as TextBox;
                var cat = stacking_panel.Children[3] as TextBox;
                var desc = stacking_panel.Children[5] as TextBox;

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
                Course c = new Course(0, name.Text, cat.Text, new List<User> { u });
                if (desc != null && !string.IsNullOrEmpty(desc.Text))
                {
                    c.Description = desc.Text;
                }

                if (App.DBConnection.AddCourse(c))
                {
                    ReturnAllCoursesFromDB(parent);
                }
                else
                {
                    // Add error handling
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

            // Load ResourceDictionary for styles
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/FormStyles.xaml")
            };

            if (!Application.Current.Resources.MergedDictionaries.Contains(resourceDictionary))
            {
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            var stacking_panel = new StackPanel
            {
                Margin = new Thickness(50, 15, 50, 15),
                Style = (Style)Application.Current.Resources["FormStackPanelStyle"]
            };
            parent.outputGrid.Children.Add(stacking_panel);

            /*
                Name
                Course
                Category
                Start
                End
                Questions
            */

            // Test name
            var text = new TextBlock
            {
                Text = "Test name",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            var input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            // Category
            text = new TextBlock
            {
                Text = "Category",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            // Course
            text = new TextBlock
            {
                Text = "Course",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            var scroll = new ScrollViewer
            {
                Style = (Style)Application.Current.Resources["FormScrollViewerStyle"]
            };
            var stacking_panel_inner = new StackPanel
            {
                Style = (Style)Application.Current.Resources["FormInnerStackPanelStyle"]
            };
            scroll.Content = stacking_panel_inner;
            stacking_panel.Children.Add(scroll);

            var list = App.DBConnection.ReturnCoursesList();
            foreach (Course u in list)
            {
                var radio = new RadioButton
                {
                    Content = $"{u.Name}, {u.MainTeacherName}",
                    Style = (Style)Application.Current.Resources["FormRadioButtonStyle"]
                };
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
                parent.radios.Add(radio);
                stacking_panel_inner.Children.Add(radio);
                parent.IDs.Add(u.ID);
            }

            // Start and End dates
            stacking_panel_inner = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = (Style)Application.Current.Resources["FormInnerStackPanelStyle"]
            };
            stacking_panel.Children.Add(stacking_panel_inner);

            var textBlock = new TextBlock
            {
                Text = "Start date:",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            var cal = new DatePicker
            {
                SelectedDate = DateTime.Now.AddDays(1),
                Style = (Style)Application.Current.Resources["FormDatePickerStyle"]
            };
            stacking_panel_inner.Children.Add(textBlock);
            stacking_panel_inner.Children.Add(cal);

            textBlock = new TextBlock
            {
                Text = "End date:",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            cal = new DatePicker
            {
                SelectedDate = DateTime.Now.AddDays(8),
                Style = (Style)Application.Current.Resources["FormDatePickerStyle"]
            };
            stacking_panel_inner.Children.Add(textBlock);
            stacking_panel_inner.Children.Add(cal);

            // Questions
            text = new TextBlock
            {
                Text = "Questions. Please select desired ones:",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            scroll = new ScrollViewer
            {
                Style = (Style)Application.Current.Resources["FormScrollViewerStyle"]
            };
            stacking_panel_inner = new StackPanel
            {
                Style = (Style)Application.Current.Resources["FormInnerStackPanelStyle"]
            };
            scroll.Content = stacking_panel_inner;
            stacking_panel.Children.Add(scroll);

            var list1 = App.DBConnection.ReturnQuestionList();
            foreach (Question q in list1)
            {
                var new_stack = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Style = (Style)Application.Current.Resources["FormInnerStackPanelStyle"]
                };

                var toggle = new ToggleButton
                {
                    Content = "X",
                    Style = (Style)Application.Current.Resources["FormToggleButtonStyle"]
                };

                textBlock = new TextBlock
                {
                    Text = q.Text,
                    Style = (Style)Application.Current.Resources["FormLabelStyle"]
                };

                new_stack.Children.Add(toggle);
                new_stack.Children.Add(textBlock);
                stacking_panel_inner.Children.Add(new_stack);
            }

            // Submit button
            var bttn = new Button
            {
                Content = "Submit",
                Style = (Style)Application.Current.Resources["FormButtonStyle"]
            };
            bttn.Click += (o, e) =>
            {
                var name = stacking_panel.Children[1] as TextBox;
                var cat = stacking_panel.Children[3] as TextBox;

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
            parent.mainTitle.Text = "Create new Question";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }
            parent.ResetParams();
            parent.typeQuestion = Question.QUESTION_TYPE.Closed;

            // Load ResourceDictionary for styles
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/FormStyles.xaml")
            };

            if (!Application.Current.Resources.MergedDictionaries.Contains(resourceDictionary))
            {
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            // Main StackPanel
            var stacking_panel = new StackPanel
            {
                Style = (Style)Application.Current.Resources["FormStackPanelStyle"]
            };
            parent.outputGrid.Children.Add(stacking_panel);

            // Name
            var text = new TextBlock
            {
                Text = "Name",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            var input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            // Category
            text = new TextBlock
            {
                Text = "Category",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            // Points
            text = new TextBlock
            {
                Text = "Points",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            // Question type
            text = new TextBlock
            {
                Text = "Question type",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            var radio_panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = (Style)Application.Current.Resources["FormRadioPanelStyle"]
            };

            var radio = new RadioButton
            {
                Content = "Closed",
                IsChecked = true,
                Style = (Style)Application.Current.Resources["FormRadioButtonStyle"]
            };
            radio.Click += (o, e) =>
            {
                if (o is RadioButton)
                {
                    parent.typeQuestion = Question.QUESTION_TYPE.Closed;
                }
            };
            radio_panel.Children.Add(radio);

            radio = new RadioButton
            {
                Content = "Open",
                Style = (Style)Application.Current.Resources["FormRadioButtonStyle"]
            };
            radio.Click += (o, e) =>
            {
                if (o is RadioButton)
                {
                    parent.typeQuestion = Question.QUESTION_TYPE.Open;
                }
            };
            radio_panel.Children.Add(radio);

            stacking_panel.Children.Add(radio_panel);

            // Question body
            text = new TextBlock
            {
                Text = "Question body",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            input = new TextBox
            {
                Style = (Style)Application.Current.Resources["FormInputStyle"]
            };
            stacking_panel.Children.Add(input);

            // Answer grid
            var uniform = new UniformGrid
            {
                Columns = 2,
                Rows = 2,
                Style = (Style)Application.Current.Resources["FormUniformGridStyle"]
            };

            for (int i = 0; i < 4; i++)
            {
                var new_stack = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Style = (Style)Application.Current.Resources["FormInnerStackPanelStyle"]
                };

                var toggle = new ToggleButton
                {
                    Content = "X",
                    Style = (Style)Application.Current.Resources["FormToggleButtonStyle"]
                };

                var textBox = new TextBox
                {
                    Text = $"Answer {i + 1}",
                    Style = (Style)Application.Current.Resources["FormInputStyle"]
                };

                new_stack.Children.Add(toggle);
                new_stack.Children.Add(textBox);
                uniform.Children.Add(new_stack);
            }
            stacking_panel.Children.Add(uniform);

            // Submit button
            var bttn = new Button
            {
                Content = "Submit",
                Style = (Style)Application.Current.Resources["FormButtonStyle"]
            };

            bttn.Click += (o, e) =>
            {
                var name = stacking_panel.Children[1] as TextBox;
                var cat = stacking_panel.Children[3] as TextBox;
                var points = stacking_panel.Children[5] as TextBox;
                var questionText = stacking_panel.Children[9] as TextBox;

                var uniformGrid = stacking_panel.Children[10] as UniformGrid;
                var answ1 = (uniformGrid.Children[0] as StackPanel).Children[1] as TextBox;
                var asnwBttn1 = (uniformGrid.Children[0] as StackPanel).Children[0] as ToggleButton;
                var answ2 = (uniformGrid.Children[1] as StackPanel).Children[1] as TextBox;
                var asnwBttn2 = (uniformGrid.Children[1] as StackPanel).Children[0] as ToggleButton;
                var answ3 = (uniformGrid.Children[2] as StackPanel).Children[1] as TextBox;
                var asnwBttn3 = (uniformGrid.Children[2] as StackPanel).Children[0] as ToggleButton;
                var answ4 = (uniformGrid.Children[3] as StackPanel).Children[1] as TextBox;
                var asnwBttn4 = (uniformGrid.Children[3] as StackPanel).Children[0] as ToggleButton;

                if (name != null && cat != null && points != null && questionText != null && answ1 != null && answ2 != null && answ3 != null && answ4 != null)
                {
                    if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(cat.Text) || string.IsNullOrEmpty(points.Text) || string.IsNullOrEmpty(questionText.Text))
                        return;

                    double p = double.Parse(points.Text, CultureInfo.InvariantCulture);
                    int key = 0;
                    key += asnwBttn1.IsChecked == true ? 1 << 3 : 0;
                    key += asnwBttn2.IsChecked == true ? 1 << 2 : 0;
                    key += asnwBttn3.IsChecked == true ? 1 << 1 : 0;
                    key += asnwBttn4.IsChecked == true ? 1 << 0 : 0;

                    string answers = $"{answ1.Text}\n{answ2.Text}\n{answ3.Text}\n{answ4.Text}";
                    Question q = new Question(name.Text, questionText.Text, parent.typeQuestion, answers, p, key, cat.Text);
                    q.PrintQuestionOnConsole();

                    if (App.DBConnection.AddQuestion(q))
                    {
                        ShowAllQusetions(parent);
                    }
                    else
                    {
                        // Add error handling
                        name.Text = "";
                        cat.Text = "";
                        questionText.Text = "";
                    }
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
