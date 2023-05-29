using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace NewEvent
{
    public partial class App : Application
    {
        
        public App()
        {
            InitializeComponent();


            MainPage = new Login();
            
        }

    }
}