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
using BD.Models;
using BD.ViewModels;

namespace BD.Views
{
    public partial class AdminPanelUI : UserControl
    {
        private readonly AdminPanelUIMV _mv;

        public int TargetChangeID = -1;
        public int TargetChangeIDSecond = -1;
        public User.TYPE type;
        public Question.QUESTION_TYPE typeQuestion;
        public List<RadioButton> radios;
        public List<int> IDs;
        public List<int> IDs1;
        public bool check = false;

        public AdminPanelUI()
        {
            InitializeComponent();
            _mv = App.Current.MainWindow.DataContext as AdminPanelUIMV;
            _mv.ShowMenu(this);
            _mv.ShowGreetPanel(this);
        }

        public void ResetParams()
        {
            TargetChangeIDSecond = -1;
            type = 0;
            typeQuestion = 0;
            radios = new List<RadioButton>();
            IDs = new List<int>();
            IDs1 = new List<int>();
            check = false;
        }

        private void Goback_Click(object sender, RoutedEventArgs e)
        {
            _mv.GoBack();
        }

        private void ShowAllUsers_Click(object sender, RoutedEventArgs e)
        {
            _mv.ReturnAllUsersFromDB(this);
            _mv.CloseMenu(this);
        }

        private void ShowAllCourses_Click(object sender, RoutedEventArgs e)
        {
            _mv.ReturnAllCoursesFromDB(this);
            _mv.CloseMenu(this);
        }

        private void ShowMenu_Click(object sender, RoutedEventArgs e)
        {
            _mv.ShowMenu(this);
        }

        private void ShowAllQuestions_Click(object sender, RoutedEventArgs e)
        {
            _mv.ShowAllQusetions(this);
            _mv.CloseMenu(this);
        }

        private void CreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            TargetChangeID = -1;
            _mv.AddNewUser(this);
            _mv.CloseMenu(this);
        } 

        private void CreateNewCourse_Click(object sender, RoutedEventArgs e)
        {
            TargetChangeID = -1;
            _mv.AddNewCourse(this);
            _mv.CloseMenu(this);
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            TargetChangeID = -1;
            _mv.AddNewQuestion(this);
            _mv.CloseMenu(this);
        }

        private void ShowAllTests_Click(object sender, RoutedEventArgs e)
        {
            _mv.ReturnAllTestsFromDB(this);
            _mv.CloseMenu(this);
        }

        private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            TargetChangeID = -1;
            _mv.AddNewTest(this);
            _mv.CloseMenu(this);
        }

        private void Callback_Click(object sender, RoutedEventArgs e)
        {
            _mv.CallbackClick(this);
        }
    }
}
