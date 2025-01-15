﻿using BD.Models;
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
        public delegate void StepMethodCallback(AdminPanelUI parent);

        private readonly MainWindow _mainwindow;
        public bool showMenu = false;
        private StepMethodCallback _stepMethodCallback;

        void voidStepCallback(AdminPanelUI parent) { }

        public AdminPanelUIMV(MainWindow mainWindow)
        {
            _stepMethodCallback = voidStepCallback;
            _mainwindow = mainWindow;
        }

        public void CallbackClick(AdminPanelUI parent)
        {
            _stepMethodCallback(parent);
            parent.CallbackButton.Visibility = Visibility.Collapsed;
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
                BorderBrush = System.Windows.Media.Brushes.Black,
                CanUserAddRows = false
            };

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
            var item = new MenuItem { Header = "Modify User" };
            item.Click += (s, e) => // LAMBDA SUPREMACY
            {
                if (s is MenuItem menuItem && menuItem.DataContext is User user)
                {
                    parent.TargetChangeID = user.ID;
                    AddNewUser(parent);
                    StackPanel stacking_panel = parent.outputGrid.Children[0] as StackPanel;
                    var login = stacking_panel.Children[1] as TextBox;
                    var email = stacking_panel.Children[3] as TextBox;
                    var fname = stacking_panel.Children[5] as TextBox;
                    var lname = stacking_panel.Children[7] as TextBox;
                    var pass = stacking_panel.Children[9] as TextBox;
                    var radio_panel = stacking_panel.Children[11] as StackPanel;

                    login.Text = user.Login;
                    email.Text = user.Email;
                    fname.Text = user.FirstName;
                    lname.Text = user.LastName;
                    pass.Text = user.Password;
                    switch (user.UserType)
                    {
                        case User.TYPE.Student:
                        case User.TYPE.Guest:
                            (radio_panel.Children[0] as RadioButton).IsChecked = true;
                            parent.type = User.TYPE.Student;
                            break;

                        case User.TYPE.Teacher:
                            (radio_panel.Children[1] as RadioButton).IsChecked = true;
                            parent.type = User.TYPE.Teacher;
                            break;

                        case User.TYPE.Admin:
                            (radio_panel.Children[2] as RadioButton).IsChecked = true;
                            parent.type = User.TYPE.Admin;
                            break;
                    }
                }
            };
            context.Items.Add(item);

            item = new MenuItem { Header = "Delete User" };
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

            context = universalItems(parent, context, ReturnAllUsersFromDB);

            var style = new Style(typeof(DataGridRow));
            style.Setters.Add(new Setter(DataGridRow.ContextMenuProperty, context));
            myDataGrid.RowStyle = style;

            var list = App.DBConnection.ReturnUsersListOfUsers();
            Debug.Print(list.Count.ToString());
            myDataGrid.ItemsSource = list;
            parent.outputGrid.Children.Add(myDataGrid);
        }

        // Add error handling
        public void AddNewUser(AdminPanelUI parent)
        {
            if (parent.TargetChangeID != -1)
                Debug.Print($"User id to change: {parent.TargetChangeID}");
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "Create new User";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }
            parent.type = User.TYPE.Student;

            var stacking_panel = new StackPanel();
            stacking_panel.VerticalAlignment = VerticalAlignment.Stretch;
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
                        User u = new User(parent.TargetChangeID, login.Text, pass.Text, email.Text, fname.Text, lname.Text, parent.type);
                        u.DebugPrintUser();
                        if (parent.TargetChangeID > -1)
                        {
                            if (App.DBConnection.UpdateUser(u).IsEmpty())
                            {
                                ReturnAllUsersFromDB(parent);
                                return;
                            }
                            Debug.Print("Update failed, trying to add new User");
                        }
                        if (App.DBConnection.AddUser(u))
                        {
                            ReturnAllUsersFromDB(parent);
                            return;
                        }
                    }
                    // Add error handling
                    pass.Text = "";
                    login.Text = "Operation failed";
                }
            };
            stacking_panel.Children.Add(bttn);

            // Stacking_panel contex menu
            ContextMenu menu = new ContextMenu();
            menu = universalItems(parent, menu, ReturnAllUsersFromDB);
            parent.outputGrid.ContextMenu = menu;
        }

        public void ShowMenu(AdminPanelUI parent)
        {
            Debug.Print($"Show menu: {showMenu}");
            if (showMenu == false)
            {
                parent.showToolBox.SetValue(Grid.ColumnProperty, 0);
                showMenu = true;
                parent.menuColumn.Width = new System.Windows.GridLength(2, GridUnitType.Star);
            }
            else
            {
                CloseMenu(parent);
            }
        }

        public void CloseMenu(AdminPanelUI parent)
        {
            parent.showToolBox.SetValue(Grid.ColumnProperty, 1);
            showMenu = false;
            parent.menuColumn.Width = new System.Windows.GridLength(0, GridUnitType.Star);
        }

        public void ShowGreetPanel(AdminPanelUI parent)
        {
            parent.mainTitle.Text = $"Welcome, {App.CurrentUser.GetFullName()}";
            if (parent.outputGrid != null && parent.outputGrid.Children.Count > 0)
            {
                parent.outputGrid.Children.RemoveAt(0);
            }

            var stacking_panel = new StackPanel()
            {
                Margin = new Thickness(50, 15, 50, 15),
            };
            parent.outputGrid.Children.Add(stacking_panel);

            TextBlock text = new TextBlock()
            {
                FontSize = 36,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            text.Text = "Please select an option in the menu";
            stacking_panel.Children.Add(text);
        }

        public void ShowAllQusetions(AdminPanelUI parent)
        {
            parent.mainTitle.Text = "Create new User";
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
                BorderBrush = System.Windows.Media.Brushes.Black,
                CanUserAddRows = false
            };

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
                    MessageBoxResult result = MessageBox.Show(
                            $"Do you want to remove: {question.Name}?",
                            "Remove question with answer",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning
                        );
                    if (result == MessageBoxResult.Yes)
                    {
                        App.DBConnection.RemoveQuestion(question.ID);
                        MessageBox.Show($"Question and answer have been removed.");
                        ReturnAllUsersFromDB(parent);
                    }
                }
            };
            context.Items.Add(item);
            context = universalItems(parent, context, ShowAllQusetions);

            var style = new Style(typeof(DataGridRow));
            style.Setters.Add(new Setter(DataGridRow.ContextMenuProperty, context));
            myDataGrid.RowStyle = style;

            var list = App.DBConnection.ReturnQuestionList();
            myDataGrid.ItemsSource = list;
            parent.outputGrid.Children.Add(myDataGrid);
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

            // Stacking_panel contex menu
            ContextMenu menu = new ContextMenu();
            menu = universalItems(parent, menu, ShowAllQusetions);
            parent.outputGrid.ContextMenu = menu;
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
                BorderBrush = System.Windows.Media.Brushes.Black,
                CanUserAddRows = false
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
            var item = new MenuItem { Header = "Show all tests" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Course c)
                {
                    ReturnAllTestsFromDB(parent, c.ID);
                }
            };
            context.Items.Add(item);
            item = new MenuItem { Header = "Delete Course" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Course c)
                {
                    MessageBoxResult result = MessageBox.Show(
                            $"Do you want to remove: {c.Name}?\nAll connected tests will be removed too.",
                            "Remove course",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning
                        );
                    if (result == MessageBoxResult.Yes)
                    {
                        App.DBConnection.RemoveCourse(c.ID);
                        MessageBox.Show($"Course and all connected tests have been removed.");
                        ReturnAllCoursesFromDB(parent);
                    }
                }
            };
            context.Items.Add(item);
            context = universalItems(parent, context, ReturnAllCoursesFromDB);

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
                BorderBrush = System.Windows.Media.Brushes.Black,
                CanUserAddRows = false
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
            var item = new MenuItem { Header = "Add new test" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Test test)
                {
                    AddNewTest(parent);
                }
            };

            item = new MenuItem { Header = "Remove test" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Test test)
                {
                    MessageBoxResult result = MessageBox.Show(
                            $"Do you want to remove: {test.Name}?",
                            "Remove single Test",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning
                        );
                    if (result == MessageBoxResult.Yes)
                    {
                        App.DBConnection.RemoveTest(test.ID);
                        MessageBox.Show($"Test has been removed.");
                        ReturnAllUsersFromDB(parent);
                    }
                }
            };

            context = universalItems(parent, context, ReturnAllCoursesFromDB);

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
            stacking_panel.VerticalAlignment = VerticalAlignment.Stretch;
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

            // Stacking_panel contex menu
            ContextMenu menu = new ContextMenu();
            menu = universalItems(parent, menu, ReturnAllCoursesFromDB);
            parent.outputGrid.ContextMenu = menu;
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
            stacking_panel.VerticalAlignment = VerticalAlignment.Stretch;
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
            // Restrict start and end date!!!!
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

            // Stacking_panel contex menu
            ContextMenu menu = new ContextMenu();
            menu = universalItems(parent, menu, AddNewTest);
            parent.outputGrid.ContextMenu = menu;
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
            stacking_panel.VerticalAlignment = VerticalAlignment.Stretch;
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

            input = new TextBox()
            {
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
            };
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

            // Stacking_panel contex menu
            ContextMenu menu = new ContextMenu();
            menu = universalItems(parent, menu, AddNewQuestion);
            parent.outputGrid.ContextMenu = menu;
        }

        ContextMenu universalItems(AdminPanelUI parent, ContextMenu addTo, StepMethodCallback callback)
        {
            addTo.Items.Add(new Separator()); // Users
            var item = new MenuItem { Header = "Add new User" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem)
                {
                    parent.TargetChangeID = -1;
                    _stepMethodCallback = callback;
                    parent.CallbackButton.Visibility = Visibility.Visible;
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
                    parent.TargetChangeID = -1;
                    _stepMethodCallback = callback;
                    parent.CallbackButton.Visibility = Visibility.Visible;
                    AddNewUser(parent);
                }
            };
            addTo.Items.Add(item);

            addTo.Items.Add(new Separator()); // Questions and tests
            item = new MenuItem { Header = "Add new Question" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem)
                {
                    parent.TargetChangeID = -1;
                    _stepMethodCallback = callback;
                    parent.CallbackButton.Visibility = Visibility.Visible;
                    AddNewQuestion(parent);
                }
            };

            item = new MenuItem { Header = "Validate all questions" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem)
                {
                    AddNewQuestion(parent);
                }
            };

            addTo.Items.Add(item);
            item = new MenuItem { Header = "Add new Test" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem)
                {
                    parent.TargetChangeID = -1;
                    _stepMethodCallback = callback;
                    parent.CallbackButton.Visibility = Visibility.Visible;
                    AddNewTest(parent);
                }
            };
            addTo.Items.Add(item);
            return addTo;
        }
    }

}
