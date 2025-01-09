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
            public User NewUser { get;  set; }

            public NewChangeRoleWindow()
            {
                InitializeComponent();
            }

        private void ChangeRole_Click(object sender, RoutedEventArgs e)
        {
            
            if (TypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            // Pobranie wybranego elementu z ComboBox
            var selectedItem = TypeComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                
                NewUser.Role = selectedItem.Content.ToString();
            }
            else
            {
                MessageBox.Show("Invalid selection.");
                return;
            }

            // Zamykanie okna
            DialogResult = true;
            Close();
        }
    }
    
}
