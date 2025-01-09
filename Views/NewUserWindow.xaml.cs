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
    
    
        public partial class NewUserWindow : Window
        {
            public User NewUser { get; private set; }

            public NewUserWindow()
            {
                InitializeComponent();
            }

            private void AddButton_Click(object sender, RoutedEventArgs e)
            {
                // Walidacja danych (możesz ją rozszerzyć według potrzeb)
                if (string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
                    string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                    string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                    TypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Wszystkie pola muszą być wypełnione.");
                    return;
                }

                // Tworzenie nowego użytkownika
                NewUser = new User(
                    0,
                    LoginTextBox.Text,
                    PasswordTextBox.Text,
                    EmailTextBox.Text,
                    FirstNameTextBox.Text,
                    LastNameTextBox.Text,
                    (TypeComboBox.SelectedIndex == 0) ? User.TYPE.Student.ToString() : User.TYPE.Teacher.ToString(),
                    (TypeComboBox.SelectedIndex == 0) ? User.TYPE.Student : User.TYPE.Teacher
                );

                // Zamykanie okna
                DialogResult = true;
                Close();
            }
        }
    
}
