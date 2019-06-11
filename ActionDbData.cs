using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction
{

    public class ActionDbData
    {
        /**
         * My SQL Connection Parameters
         */
        private static string conenctionParams = "SERVER=localhost;DATABASE=auction;UID=root;PASSWORD=root;";

        protected static DataTable getData(string query)
        {

            // Datatable to contain the retreive information upon query execution
            DataTable dataTable = new DataTable();

            // Establish the connection with the MySQL database using the connection parameters
            Console.WriteLine("Connecting Database!");
            using (var mySqlConnection = new MySqlConnection(conenctionParams))
            {
                try
                {
                    // Open up the database connection
                    mySqlConnection.Open();
                    Console.WriteLine("Connection Established!" + "\n" + "Query:" + query);
                    // Execute the Query in order to have the user authenticated
                    MySqlDataAdapter da = new MySqlDataAdapter(query, mySqlConnection);
                    // Transfer the retreived information upon query execution
                    da.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Data Processing Error: " + ex.ToString());
                }
            }
            Console.WriteLine("Closed Database Connection!");
            return dataTable;
        }
    }



    public class UsersTableData : ActionDbData
    {
        public static IList<Users> getUsers(string query)
        {
            // Datatable to contain the retreive information upon query execution
            string queryFormatted = "SELECT * FROM users" + (query == null || query.Trim() == "" ? "" : " WHERE " + query);
            DataTable dataTable = getData(queryFormatted);
            // Copy User Data to the IList
            return dataTable.AsEnumerable().Select(row => new Users(row)).ToList();
        }
    }



    public class AuctionItemTableData : ActionDbData
    {
        public static IList<AuctionItem> getActionItems(string query)
        {
            // Datatable to contain the retreive information upon query execution
            string queryFormatted = "SELECT * FROM auction_item" + (query==null||query.Trim() == "" ? "" : " WHERE " + query);
            DataTable dataTable = getData(queryFormatted);
            // Copy AuctionItem Data to the IList
            return dataTable.AsEnumerable().Select(row => new AuctionItem(row)).ToList();
        }
    }
}
