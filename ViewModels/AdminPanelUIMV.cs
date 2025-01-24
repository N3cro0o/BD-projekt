using BD.Models;
using BD.Views;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Globalization;
using System.Reflection;


namespace BD.ViewModels
{
    internal class AdminPanelUIMV
    {
        public delegate void StepMethodCallback(AdminPanelUI parent);

        private readonly MainWindow _mainwindow;
        public bool showMenu = false;
        private StepMethodCallback _stepMethodCallback;
        private ValidateVM _validateVM;

        void voidStepCallback(AdminPanelUI parent) { }

        public AdminPanelUIMV(MainWindow mainWindow)
        {
            _stepMethodCallback = voidStepCallback;
            _mainwindow = mainWindow;
            _validateVM = new ValidateVM();
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
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/DataGridStyles.xaml")
            };
            Style customDataGridStyle = (Style)resourceDictionary["CustomDataGridStyle"];
            DataGrid myDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                //Style = (Style)Application.Current.Resources["CustomDataGridStyle"]
                //Margin = new Thickness(10),
                //AlternatingRowBackground = System.Windows.Media.Brushes.LightGray,
                //Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#82827D")),
                //BorderThickness = new Thickness(2),
                //BorderBrush = System.Windows.Media.Brushes.Black
            };

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
                    pass.Text = "";
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
                        App.DBConnection.RemoveUser(user);
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
                Style = (Style)Application.Current.Resources["FormButtonStyle"],
                VerticalAlignment = VerticalAlignment.Bottom
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
                        !(string.IsNullOrEmpty(pass.Text) || parent.TargetChangeID >= 0) &&
                        !string.IsNullOrEmpty(fname.Text) &&
                        !string.IsNullOrEmpty(lname.Text))
                    {
                        User u = new User(parent.TargetChangeID, login.Text, pass.Text, email.Text, fname.Text, lname.Text, parent.type);
                        u.DebugPrintUser();

                        if (parent.TargetChangeID > -1)
                        {
                            if (!App.DBConnection.UpdateUser(u, string.IsNullOrEmpty(pass.Text)).IsEmpty())
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

        public void ReturnAllQuestionsFromDB(AdminPanelUI parent)
        {
            parent.mainTitle.Text = "Question list";
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
                CanUserAddRows = false

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
                Header = "Type",
                Binding = new System.Windows.Data.Binding("QuestionType"),
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

            item = new MenuItem() { Header = "Update question" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Question q)
                {
                    parent.TargetChangeID = q.ID;
                    AddNewQuestion(parent);
                    StackPanel stacking_panel = parent.outputGrid.Children[0] as StackPanel;
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
                    var a = App.DBConnection.FetchAnswer(q.AnswerID);
                    parent.TargetChangeIDSecond = q.AnswerID;

                    name.Text = q.Name;
                    cat.Text = q.Category;
                    points.Text = q.Points.ToString(CultureInfo.InvariantCulture);
                    text.Text = q.Text;

                    switch (q.QuestionType)
                    {
                        case Question.QUESTION_TYPE.Closed:
                        case Question.QUESTION_TYPE.Invalid:
                            parent.typeQuestion = Question.QUESTION_TYPE.Closed;
                            ((stacking_panel.Children[7] as StackPanel).Children[0] as RadioButton).IsChecked = true;
                            break;

                        case Question.QUESTION_TYPE.Open:
                            parent.typeQuestion = Question.QUESTION_TYPE.Open;
                            ((stacking_panel.Children[7] as StackPanel).Children[1] as RadioButton).IsChecked = true;
                            break;

                    }

                    var answer_list = a.AnswerBody.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (answer_list.Length != 4)
                    {
                        answ1.Text = "Invalid answer format";
                        answ2.Text = "Invalid answer format";
                        answ3.Text = "Invalid answer format";
                        answ4.Text = "Invalid answer format";
                    }
                    else
                    {
                        answ1.Text = answer_list[0];
                        answ2.Text = answer_list[1];
                        answ3.Text = answer_list[2];
                        answ4.Text = answer_list[3];
                    }
                    asnwBttn1.IsChecked = ((a.AnswerKey & 1 << 3) >> 3) == 1;
                    asnwBttn2.IsChecked = ((a.AnswerKey & 1 << 2) >> 2) == 1;
                    asnwBttn3.IsChecked = ((a.AnswerKey & 1 << 1) >> 1) == 1;
                    asnwBttn4.IsChecked = ((a.AnswerKey & 1 << 0) >> 0) == 1;
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
                        App.DBConnection.RemoveQuestion(question);
                        MessageBox.Show($"Question and answer have been removed.");
                        ReturnAllQuestionsFromDB(parent);
                    }
                }
            };
            context.Items.Add(item);
            context = universalItems(parent, context, ReturnAllQuestionsFromDB);

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
                ReturnAllQuestionsFromDB(parent);
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
            menu = universalItems(parent, menu, ReturnAllQuestionsFromDB);
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
                CanUserAddRows = false

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
                Binding = new System.Windows.Data.Binding("StudentsCount"),
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

            item = new MenuItem() { Header = "Update course" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Course course)
                {
                    parent.TargetChangeID = course.ID;
                    AddNewCourse(parent);
                    StackPanel stacking_panel = parent.outputGrid.Children[0] as StackPanel;

                    var name = (stacking_panel.Children[1] as TextBox);
                    var cat = (stacking_panel.Children[3] as TextBox);
                    var desc = (stacking_panel.Children[5] as TextBox);
                    var radio_panel = (stacking_panel.Children[7] as ScrollViewer).Content as StackPanel;
                    var student_panel = (stacking_panel.Children[9] as ScrollViewer).Content as StackPanel;

                    name.Text = course.Name;
                    cat.Text = course.Category;
                    desc.Text = course.Description;

                    for (int i = 0; i < radio_panel.Children.Count; i++)
                    {
                        if (course.Teachers[0].ID == parent.IDs[i])
                            (radio_panel.Children[i] as RadioButton).IsChecked = true;
                    }

                    var list = App.DBConnection.ReturnConnectedStudentsToCourse(course);
                    for (int i = 0; i < parent.IDs1.Count; i++)
                    {
                        Debug.Print($"Question to check ID: {parent.IDs1[i]}");
                        foreach (int j in list)
                        {
                            Debug.Print($"Question checked ID: {j}");
                            if (parent.IDs1[i] == j)
                            {
                                ToggleButton b = (student_panel.Children[i] as StackPanel).Children[0] as ToggleButton;
                                b.IsChecked = true;
                                continue;
                            }
                        }
                    }
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
                        App.DBConnection.RemoveCourse(c);
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
                CanUserAddRows = false

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
            var item = new MenuItem { Header = "Update test" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem && menuItem.DataContext is Test test)
                {
                    parent.TargetChangeID = test.ID;
                    AddNewTest(parent);
                    StackPanel stacking_panel = parent.outputGrid.Children[0] as StackPanel;

                    var name = (stacking_panel.Children[1] as TextBox);
                    var cat = (stacking_panel.Children[3] as TextBox);
                    var cal_start = (stacking_panel.Children[6] as StackPanel).Children[1] as DatePicker;
                    var cal_end = (stacking_panel.Children[6] as StackPanel).Children[3] as DatePicker;
                    var question_panel = (stacking_panel.Children[8] as ScrollViewer).Content as StackPanel;

                    name.Text = test.Name;
                    cat.Text = test.Category;
                    cal_start.SelectedDate = test.StartDate;
                    cal_end.SelectedDate = test.EndDate;

                    for (int i = 0; i < parent.radios.Count; i++)
                    {
                        if (test.CourseObject.ID == parent.IDs[i])
                        {
                            parent.radios[i].IsChecked = true;
                        }
                    }

                    var list = App.DBConnection.ReturnConnectedQuestionsToTest(test);
                    for (int i = 0; i < parent.IDs1.Count; i++)
                    {
                        Debug.Print($"Question to check ID: {parent.IDs1[i]}");
                        foreach (int j in list)
                        {
                            Debug.Print($"Question checked ID: {j}");
                            if (parent.IDs1[i] == j)
                            {
                                ToggleButton b = (question_panel.Children[i] as StackPanel).Children[0] as ToggleButton;
                                b.IsChecked = true;
                                continue;
                            }
                        }
                    }
                }
            };
            context.Items.Add(item);

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
                        App.DBConnection.RemoveTest(test);
                        MessageBox.Show($"Test has been removed.");
                        ReturnAllTestsFromDB(parent);
                    }
                }
            };
            context.Items.Add(item);

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

                Student
                Test - maybe?

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
                MaxHeight = 120,
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


            // Students
            text = new TextBlock() 
            {
                Text = "Select students",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            scroll = new ScrollViewer()
            {
                //Style = (Style)Application.Current.Resources["ScrolViewer1"],
                MaxHeight = 150
            };
            stacking_panel_inner = new StackPanel();
            scroll.Content = stacking_panel_inner;
            stacking_panel.Children.Add(scroll);
            var list_students_to_add = App.DBConnection.ReturnStudentList();
            foreach (User u in list_students_to_add) 
            {
                var toggle = new ToggleButton() 
                {
                    Content = "X",
                    Style = (Style)Application.Current.Resources["FormToggleButtonStyle"]
                };
                text = new TextBlock()
                {
                    Text = u.GetFullName(),
                    Style = (Style)Application.Current.Resources["FormLabelStyle"]
                };
                StackPanel inner = new StackPanel() 
                {
                    Orientation = Orientation.Horizontal,
                };
                inner.Children.Add(toggle);
                inner.Children.Add(text);
                stacking_panel_inner.Children.Add(inner);

                parent.IDs1.Add(u.ID);
            }

            // Submit
            Button bttn = new Button()
            {
                Content = "Submit",
                Style = (Style)Application.Current.Resources["FormButtonStyle"],
                VerticalAlignment = VerticalAlignment.Bottom
            };
            stacking_panel.Children.Add(bttn);
            bttn.Click += (o, e) =>
            {
                var name = (stacking_panel.Children[1] as TextBox);
                var cat = (stacking_panel.Children[3] as TextBox);
                var desc = (stacking_panel.Children[5] as TextBox);
                var student_panel = (stacking_panel.Children[9] as ScrollViewer).Content as StackPanel;
                int teach = -1;
                List<User> student_list = new List<User>();
                for (int i = 0; i < parent.IDs.Count; i++)
                {
                    if (parent.radios[i].IsChecked == true)
                    {
                        teach = parent.IDs[i];
                    }
                }

                for (int i = 0; i < student_panel.Children.Count; i++)
                {
                    ToggleButton t = (student_panel.Children[i] as StackPanel).Children[0] as ToggleButton;
                    if (t != null && t.IsChecked == true)
                    {
                        student_list.Add(list_students_to_add[i]);
                        Debug.Print($"Debil to be added: {list_students_to_add[i].GetFullName()}");
                    }
                }

                if (name == null || cat == null || teach < 0)
                    return;

                if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(cat.Text))
                    return;

                User u = App.DBConnection.GetUserByID(teach);
                Course c = new Course(parent.TargetChangeID, name.Text, cat.Text, new List<User>([u]));
              
                if (desc != null && !string.IsNullOrEmpty(desc.Text))
                {
                    c.Description = desc.Text;
                }

                if (parent.TargetChangeID >= 0)
                {
                    if (!App.DBConnection.UpdateCourse(c).IsEmpty())
                    {
                        if (!App.DBConnection.UpdateCourseToStudent(c, student_list)) 
                        {
                            Debug.Print("Updating Course-Student failed");
                        }
                        ReturnAllCoursesFromDB(parent);
                        return;
                    }
                }

                bool check;
                int id_test;
                (check, id_test) = App.DBConnection.AddCourse(c);
                if (check)
                {
                    c.ID = id_test;
                    if (!App.DBConnection.ConnectCourseToStudent(c, student_list))
                        Debug.Print("Connecting Course-Student failed");
                    ReturnAllCoursesFromDB(parent);
                }
                else
                {
                    // Add error handling
                    name.Text = "";
                    cat.Text = "";
                }
            };
            // Stacking_panel contex menu
            ContextMenu menu = new ContextMenu();
            menu = universalItems(parent, menu, ReturnAllCoursesFromDB);
            parent.outputGrid.ContextMenu = menu;
        }

        public void AddNewTest(AdminPanelUI parent)
        {
            Debug.Print(parent.type.ToString());
            // Basic stuff, title and reset body
            parent.mainTitle.Text = "Create new Test";
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
                Style = (Style)Application.Current.Resources["FormScrollViewerStyle"],
                MaxHeight = 150
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
            var cal_start = new DatePicker
            {
                SelectedDate = DateTime.Now.AddDays(1),
                DisplayDateStart = DateTime.Now,
                Style = (Style)Application.Current.Resources["FormDatePickerStyle"]
            };
            stacking_panel_inner.Children.Add(textBlock);
            stacking_panel_inner.Children.Add(cal_start);

            textBlock = new TextBlock
            {
                Text = "End date:",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            var cal_end = new DatePicker
            {
                SelectedDate = DateTime.Now.AddDays(8),
                Style = (Style)Application.Current.Resources["FormDatePickerStyle"]
            };
            stacking_panel_inner.Children.Add(textBlock);
            stacking_panel_inner.Children.Add(cal_end);

            cal_start.CalendarClosed += (s, e) =>
            {
                cal_end.DisplayDateStart = cal_start.SelectedDate;
            };
            cal_end.CalendarClosed += (s, e) =>
            {
                cal_start.DisplayDateEnd = cal_end.SelectedDate;
            };

            // Questions
            text = new TextBlock
            {
                Text = "Questions. Please select desired ones:",
                Style = (Style)Application.Current.Resources["FormLabelStyle"]
            };
            stacking_panel.Children.Add(text);

            scroll = new ScrollViewer
            {
                Style = (Style)Application.Current.Resources["FormScrollViewerStyle"],
                MaxHeight = 150
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
                parent.IDs1.Add(q.ID);
            }

            // Submit button
            var bttn = new Button
            {
                Content = "Submit",
                Style = (Style)Application.Current.Resources["FormButtonStyle"],
                VerticalAlignment = VerticalAlignment.Bottom
            };
            bttn.Click += (o, e) =>
            {
                var name = (stacking_panel.Children[1] as TextBox);
                var cat = (stacking_panel.Children[3] as TextBox);
                var cal_start = (stacking_panel.Children[6] as StackPanel).Children[1] as DatePicker;
                var cal_end = (stacking_panel.Children[6] as StackPanel).Children[3] as DatePicker;
                var question_panel = (stacking_panel.Children[8] as ScrollViewer).Content as StackPanel;
                List<Question> question_list = new List<Question>();
                int course = -1;
                for (int i = 0; i < parent.IDs.Count; i++)
                {
                    if (parent.radios[i].IsChecked == true)
                    {
                        course = parent.IDs[i];
                    }
                }
                for (int i = 0; i < question_panel.Children.Count; i++)
                {
                    ToggleButton b = (question_panel.Children[i] as StackPanel).Children[0] as ToggleButton;
                    if (b.IsChecked == true)
                    {
                        question_list.Add(list1[i]);
                        Debug.Print($"ID: {list1[i].ID}, Name: {list1[i].Name}, {list1[i].Text}");
                    }
                }

                if (name == null || cat == null || course < 0)
                    return;
                if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(cat.Text))
                    return;

                Course c = App.DBConnection.ReturnCoursesListWithID(course)[0];
                Test t = new Test(c, cat.Text, cal_start.SelectedDate.Value, cal_end.SelectedDate.Value);
                t.Name = name.Text;
                t.ID = parent.TargetChangeID;

                if (parent.TargetChangeID >= 0)
                {
                    if (!App.DBConnection.UpdateTest(t).IsEmpty())
                    {
                        if (!App.DBConnection.UpdateTestToQuestion(t, question_list))
                            Debug.Print("Connecting test-question failed");
                        ReturnAllTestsFromDB(parent);
                        return;
                    }
                }

                bool check;
                int id_test;
                (check, id_test) = App.DBConnection.AddTest(t);
                if (check)
                {
                    t.ID = id_test;
                    if (!App.DBConnection.ConnectTestToQuestion(t, question_list))
                        Debug.Print("Connecting test-question failed");
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
            if (parent.TargetChangeID != -1)
                Debug.Print($"User id to change: {parent.TargetChangeID}");
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
                Width = 50,
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
                Width = 500,
                TextWrapping = TextWrapping.Wrap,
                Style = (Style)Application.Current.Resources["FormInputStyle"],
                AcceptsReturn = true,
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
                    Style = (Style)Application.Current.Resources["CustomStackPanelStyle"]
                };

                var toggle = new ToggleButton
                {
                    
                    Style = (Style)Application.Current.Resources["CustomToggleButtonStyle"]
                };

                var textBox = new TextBox
                {
                    Text = $"Answer {i + 1}",
                    Style = (Style)Application.Current.Resources["CustomTextBoxStyle"]
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
                Style = (Style)Application.Current.Resources["FormButtonStyle"],
                VerticalAlignment = VerticalAlignment.Bottom
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
                    string answ = answ1.Text + "\n";
                    answ += answ2.Text + "\n";
                    answ += answ3.Text + "\n";
                    answ += answ4.Text;
                    Question q = new Question(name.Text, questionText.Text, parent.typeQuestion, answ, p, key, cat.Text);
                    q.ID = parent.TargetChangeID;
                    q.AnswerID = parent.TargetChangeIDSecond;
                    q.PrintQuestionOnConsole();
                    if (parent.TargetChangeID > -1)
                    {
                        Question q1;
                        Answer a1;
                        (q1, a1) = App.DBConnection.UpdateQuestion(q);
                        if (!q1.IsEmpty())
                        {
                            ReturnAllQuestionsFromDB(parent);
                            return;
                        }
                        Debug.Print("Update failed, trying to add new Question");
                    }
                    if (App.DBConnection.AddQuestion(q))
                    {
                        ReturnAllQuestionsFromDB(parent);
                        return;
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
            addTo.Items.Add(item);

            item = new MenuItem { Header = "Validate all questions" };
            item.Click += (s, e) =>
            {
                if (s is MenuItem menuItem)
                {
                    _validateVM.ValidateQuestions();
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
