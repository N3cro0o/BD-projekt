using System.Configuration;
using System.Data;
using System.Windows;
using BD.Models;

namespace BD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool logged = true;
        public readonly static DataBaseConnection DBConnection = new DataBaseConnection();
        public static User CurrentUser { get; set; } = new User();
    }

}
