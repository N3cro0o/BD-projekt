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
    
    
        public partial class NewChangeRoleWindow : Window
        {
            public User NewUser { get; private set; }

            public NewChangeRoleWindow()
            {
                InitializeComponent();
            }

            private void ChangeRole_Click(object sender, RoutedEventArgs e)
            {
                // Walidacja danych (możesz ją rozszerzyć według potrzeb)
                if (
                    TypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Complete all fields.");
                    return;
                }

                // Tworzenie nowego użytkownika
                NewUser = new User(
                    0,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    (TypeComboBox.SelectedIndex == 0) ? User.TYPE.Student.ToString() : User.TYPE.Teacher.ToString(),
                    (TypeComboBox.SelectedIndex == 0) ? User.TYPE.Student : User.TYPE.Teacher
                );

                // Zamykanie okna
                DialogResult = true;
                Close();
            }
        }
    
}
