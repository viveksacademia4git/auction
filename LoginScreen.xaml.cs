using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace Auction
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        string conenctionParams = "SERVER=localhost;DATABASE=auction;UID=root;PASSWORD=root;";

        public LoginScreen()
        {
            InitializeComponent();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Connecting Database!");

            // Execute the Query
            DataTable datatable = null;

            using (var mySqlConnection = new MySqlConnection(conenctionParams))
            {
                mySqlConnection.Open();
                Console.WriteLine("Connection Established!");
                string query = "select userId, userName, pwd, fullName from users";
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                datatable = new DataTable();
                datatable.Load(cmd.ExecuteReader());
            }

            // Execute the Query
            try
            {
                foreach (DataRow dataRow in datatable.Rows)
                {
                    foreach (var item in dataRow.ItemArray)
                    {
                        Console.Write(item + ",");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Processing Error: " + ex.ToString());
            }

            MainWindow mainWindow= new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
