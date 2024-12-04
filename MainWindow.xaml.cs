﻿using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BD
{
    public partial class MainWindow : Window
    {
        public static string? _Title;

        public bool Logged;

        public object logged_string = Visibility.Hidden;

        public MainWindow()
        {
            InitializeComponent();
            _Title = Title;
            Logged = false;

            ChangeMainPageDataContext();
        }

        public void ChangeMainPageDataContext()
        {
            DataContext = new ViewModels.MainPageMV(this);
        }

        public void ChangeLoginDataContext()
        {
            DataContext = new ViewModels.LoginPageMV(this);
        }

        public void ChangeLogoffDataContext()
        {
            Logged = false;
            ChangeMainPageDataContext();
        }

        public void ChangeAdminPanelDataContext()
        {
            DataContext = new ViewModels.AdminPanelMV();
        }
    }
}