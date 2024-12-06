using System.Configuration;
using System.Data;
using System.Windows;

namespace BD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool logged = true;
        public readonly static DataBaseConnection DBConnection = new DataBaseConnection();
    }

}
