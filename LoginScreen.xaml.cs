using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        /**
         * Initialize Components of the Login Screen
         */
        public LoginScreen()
        {
            InitializeComponent();
        }



        /**
         * Function to carry out the login process for the particular user
         */ 
        private void ClickLogin_Submit(object sender, RoutedEventArgs e)
        {
            string username = txtLogin_Username.Text;
            string pwd = txpLogin_Pwd.Password;

            // Validate Username
            if(username.Trim()=="")
            {
                string msg = "Please Enter Username";
                Console.WriteLine(msg);
                lblLogin_Message.Content = msg;
                return;
            }
            // Validate Password
            if (pwd.Trim() == "")
            {
                string msg = "Please Enter Password";
                Console.WriteLine(msg);
                lblLogin_Message.Content = msg;
                return;
            }

            IList<Users> users = getUsers(username, pwd);
            if(users==null || users.Count==0)
            {
                string msg = "Invalid Credentials";
                Console.WriteLine(msg);
                lblLogin_Message.Content = msg;
                return;
            }

            Users user = users[0];

            Console.WriteLine("Login Successfull:");
            Console.WriteLine(user.toString());

            // Create instance of the main window
            MainWindow mainWindow = new MainWindow();

            // Close the current login window
            this.Close();

            // Show the main window
            mainWindow.Show();
        }


        /**
         * Gets the user from collection with the given username and password
         * 
         * Parameters:
         *   string:
         *     username
         *     
         *   string:
         *     pwd
         */
        public IList<Users> getUsers(string username, string pwd)
        {
            // Create Query upon establishing connection with the Database
            string query = "username='" + username + "' AND pwd=MD5('" + pwd + "') ";
            return UsersTableData.getUsers(query);
        }

    }

}
