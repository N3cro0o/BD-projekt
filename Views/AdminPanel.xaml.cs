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
using BD.ViewModels;

namespace BD.Views
{
    public partial class AdminPanel : UserControl
    {
        private readonly AdminPanelVM _mv;
        public int EnterCounter = 0;

        public AdminPanel()
        {
            InitializeComponent();

            _mv = App.Current.MainWindow.DataContext as AdminPanelVM;
        }

        private void OnPanelSubmit(object sender, RoutedEventArgs e)
        {

        }

        private void Goback_Click(object sender, RoutedEventArgs e)
        {
            _mv.GoBack();
        }

        private void ShowAllUsers_Click(object sender, RoutedEventArgs e)
        {
            _mv.ReturnAllUsersFromDB(this);
        }

        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            _mv.AddNewUser(this);
        }

        private void InputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (_mv.StepMethod != null)
                {
                    EnterCounter++;
                    _mv.StepMethod(this);
                }
                else
                {
                    _mv.InstrQuery(InputBox.Text, this);
                    EnterCounter = 0;
                }
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            _mv.DeleteUser(this);
        }

        private void ModifyUser_Click(object sender, RoutedEventArgs e)
        {
            _mv.ModifyUser(this);
        }

        private void ShowQuestions_Click(object sender, RoutedEventArgs e)
        {
            _mv.ReturnAllQuestionsFromDB(this);
        }

        private void AddNewQuestion_Click(object sender, RoutedEventArgs e)
        {
            _mv.AddNewQuestion(this);
        }
    }
}
