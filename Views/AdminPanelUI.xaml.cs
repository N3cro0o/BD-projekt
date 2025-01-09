﻿using System;
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
    public partial class AdminPanelUI : UserControl
    {
        private readonly AdminPanelUIMV _mv;
        public AdminPanelUI()
        {
            InitializeComponent();

            _mv = App.Current.MainWindow.DataContext as AdminPanelUIMV;
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
            _mv.ReturnAllUsersFromDB(sender, e, this);
        }

        private void ShowAllCourses_Click(object sender, RoutedEventArgs e)
        {
            _mv.ReturnAllCoursesFromDB(this);
        }

        private void OnInputBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ShowMenu_Click(object sender, RoutedEventArgs e)
        {
            _mv.ShowMenu(this);
        }

        private void ShowAllStatistics_Click(object sender, RoutedEventArgs e)
        {
            _mv.ShowAllStatistics(this);
        }

        private void ShowAllQuestions_Click(object sender, RoutedEventArgs e)
        {
            _mv.ShowAllQusetions(this);
        }
        private void OnDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _mv.OnDeleteUserButton(sender, e, this);
        }
        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            _mv.AddNewUser(sender, e);
        }
        private void addCourse_Click(object sender, RoutedEventArgs e)
        {
            _mv.AddNewCours(sender, e);
        }
        

    }
}