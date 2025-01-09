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
    
    
        public partial class NewCourseWindow : Window
        {
            public Course NewCours { get; private set; }

            public NewCourseWindow()
            {
                InitializeComponent();
            }

            private void AddButton_Click(object sender, RoutedEventArgs e)
            {
                // Walidacja danych (możesz ją rozszerzyć według potrzeb)
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(CattegoryTextBox.Text) ||
                    string.IsNullOrWhiteSpace(OwnerNameTextBox.Text)
                    )
                   
                {
                    MessageBox.Show("Wszystkie pola muszą być wypełnione.");
                    return;
                }

            if (!int.TryParse(CattegoryTextBox.Text, out int category))
            {
                MessageBox.Show("Kategoria musi być liczbą.");
                return;
            }

            NewCours = new Course(
                    0,
                    NameTextBox.Text,
                    category,
                    new List<int>(),             // Lista nauczycieli (pusta lista)
                    new List<int>(),             // Lista studentów (pusta lista)
                    new List<int>()
                );

                // Zamykanie okna
                DialogResult = true;
                Close();
            }
        }
    
}
