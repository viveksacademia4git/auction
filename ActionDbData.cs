using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Auction
{

    public class ActionDbData
    {
        public static bool blank(string str)
        {
            return str == null || str.Trim().Equals("");
        }
        /**
         * My SQL Connection Parameters
         */
        private static string conenctionParams = "SERVER=localhost;DATABASE=auction;UID=root;PASSWORD=root;";


        protected static DataTable getData(string query) {

            // Datatable to contain the retreive information upon query execution
            DataTable dataTable = new DataTable();

            // Establish the connection with the MySQL database using the connection parameters
            Console.WriteLine("Connecting Database!");
            using (var mySqlConnection = new MySqlConnection(conenctionParams)) {
                try {
                    // Open up the database connection
                    mySqlConnection.Open();
                    Console.WriteLine("Connection Established!" + "\n" + "Query:" + query);
                    // Execute the Query in order to have the user authenticated
                    MySqlDataAdapter da = new MySqlDataAdapter(query, mySqlConnection);
                    // Transfer the retreived information upon query execution
                    da.Fill(dataTable);
                }
                catch (Exception ex) {
                    Console.WriteLine("Data Processing Error: " + ex.ToString());
                }
            }
            Console.WriteLine("Closed Database Connection!");
            return dataTable;
        }

        protected static DataTable getData(string query, string[] conditionCols, object[] conditionVals, string querySuffix)
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
                    // Query Formatting
                    string queryCon = "";
                    LinkedList<object> listCol = new LinkedList<object>();
                    LinkedList<object> listVal = new LinkedList<object>();
                    for (int i = 0; i < conditionCols.Length; i++) {
                        if (conditionVals[i] != null) {
                            var col = conditionCols[i];
                            listCol.AddLast(col);
                            listVal.AddLast(conditionVals[i]);
                            queryCon += " AND  " + col + " LIKE @" + col;
                        }
                    }
                    string queryCondition = listCol.Count > 0 ? " WHERE " + queryCon.Substring(5) : "";
                    string queryFormatted = query + " " + queryCondition + " " + (querySuffix??"");
                    Console.WriteLine("Connection Established!" + "\n" + "Query:" + queryFormatted);
                    // Execute the Query in order to have the user authenticated
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    MySqlCommand cmd = new MySqlCommand(queryFormatted, mySqlConnection);
                    // Transfer the retreived information upon query execution
                    for (int i = 0; i < listCol.Count; i++) {
                        cmd.Parameters.AddWithValue("@" + listCol.ElementAt(i), listVal.ElementAt(i));
                    }
                    da.SelectCommand = cmd;
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


        private static int executeUpdate(string query, string[] columns, object[] values) {
            int count = 0;

            Console.WriteLine("Connecting Database!");
            using (var mySqlConnection = new MySqlConnection(conenctionParams)) {
                try {
                    // Open up the database connection
                    mySqlConnection.Open();
                    Console.WriteLine("Connection Established!" + "\n" + "Query:" + query);

                    MySqlCommand comm = mySqlConnection.CreateCommand();
                    comm.CommandText = query;

                    for (int i = 0; i < columns.Length; i++)
                        comm.Parameters.AddWithValue("@" + columns[i], values[i]);

                    count = comm.ExecuteNonQuery();
                }
                catch (Exception ex) {
                    Console.WriteLine("Data Processing Error: " + ex.ToString());
                }
            }
            Console.WriteLine("Closed Database Connection!");
            return count;
        }


        protected static int insertRecord(string table, string[] columns, object[] values) {
            string query = "@" + columns[0];
            for (int i = 1; i < columns.Length; i++)
                query += ", @" + columns[i];
            query = "INSERT INTO " + table + " (" + query.Replace("@","") + ") VALUES (" + query + ")";
            return executeUpdate(query, columns, values);
        }


        protected static int updateRecord(string table, string[] columns, object[] values, string conditionAndLimit) {
            string query = columns[0] + "=@" + columns[0];
            for (int i = 1; i < columns.Length; i++)
                query += ", " + columns[i] + " = @" + columns[i];
            query = "UPDATE " + table + " SET " + query + conditionAndLimit;
            return executeUpdate(query, columns, values);
        }
    }



    public class UsersTableData : ActionDbData
    {
        public static IList<Users> getUsers(string query) {
            string queryFormatted = "SELECT * FROM users " + (query ?? "");
            DataTable dataTable = getData(queryFormatted);
            return dataTable.AsEnumerable().Select(row => new Users(row)).ToList();
        }
    }



    public class AuctionItemTableData : ActionDbData
    {
        private static string[] columns = { "sold", "quantity", "initialPrice", "itemName", "itemType", "additionalInfo", "owner"};

        private static object[] values(AuctionItem ai) {
            return new object[] { ai.sold, ai.quantity, ai.initialPrice, ai.itemName, ai.itemType, ai.additionalInfo, ai.owner };
        }

        public static IList<AuctionItem> getAuctionItems(string query) {
            string queryFormatted = "SELECT * FROM auction_item " + (query ?? "");
            DataTable dataTable = getData(queryFormatted);
            return dataTable.AsEnumerable().Select(row => new AuctionItem(row)).ToList();
        }

        public static int insertAuctionItem(AuctionItem ai) {
            return insertRecord("auction_item", columns, values(ai));
        }

        public static int updateAuctionItem(AuctionItem ai) {
            return updateRecord("auction_item", columns, values(ai), " WHERE itemId="+ ai.itemId);
        }

        public static IList<AuctionItem> getAuctionItems(AuctionItem ai, string querySuffix)
        {
            string queryFormatted = "SELECT * FROM auction_item ";
            object[] vals = values(ai);
            int count = -1;
            vals[++count] = null;
            vals[++count] = (ai.quantity == -1) ? null : vals[count];
            vals[++count] = (ai.initialPrice == -1) ? null : vals[count];
            vals[++count] = blank(ai.itemName) ? null : "%" + vals[count] + "%";
            vals[++count] = blank(ai.itemType) ? null : "%" + vals[count] + "%";
            vals[++count] = blank(ai.additionalInfo) ? null : "%" + vals[count] + "%";
            vals[++count] = blank(ai.owner) ? null : "%" + vals[count] + "%";
            DataTable dataTable = getData(queryFormatted, columns, vals, querySuffix);
            return dataTable.AsEnumerable().Select(row => new AuctionItem(row)).ToList();
        }
    }



    public class AuctionLocationTableData : ActionDbData
    {
        private static string[] columns = { "locationName", "address", "availability", "place", "zipcode", "capacity", "contactPerson", "phone", "email" };

        private static object[] values(AuctionLocation al) {
            return new object[] { al.locationName, al.address, al.availability, al.place, al.zipcode, al.capacity, al.contactPerson, al.phone, al.email };
        }

        public static IList<AuctionLocation> getAuctionLocations(string query) {
            string queryFormatted = "SELECT * FROM location " + (query ?? "");
            DataTable dataTable = getData(queryFormatted);
            return dataTable.AsEnumerable().Select(row => new AuctionLocation(row)).ToList();
        }

        public static int insertAuctionLocation(AuctionLocation al) {
            return insertRecord("location", columns, values(al));
        }

        public static int updateAuctionLocation(AuctionLocation al) {
            return updateRecord("location", columns, values(al), " WHERE locationId=" + al.locationId);
        }

        public static IList<AuctionLocation> getAuctionLocations(AuctionLocation al, string querySuffix)
        {
            string queryFormatted = "SELECT * FROM location ";
            string[] columns = { "capacity", "locationName", "address", "availability", "place", "contactPerson"};
            object[] vals = { al.capacity, al.locationName, al.address, al.availability, al.place, al.contactPerson };
            int count = -1;
            vals[++count] = (al.capacity == -1) ? null : vals[count];
            vals[++count] = blank(al.locationName) ? null : "%" + vals[count] + "%";
            vals[++count] = blank(al.address) ? null : "%" + vals[count] + "%";
            vals[++count] = blank(al.availability) ? null : "%" + vals[count] + "%";
            vals[++count] = blank(al.place) ? null : "%" + vals[count] + "%";
            vals[++count] = blank(al.contactPerson) ? null : "%" + vals[count] + "%";
            DataTable dataTable = getData(queryFormatted, columns, vals, querySuffix);
            return dataTable.AsEnumerable().Select(row => new AuctionLocation(row)).ToList();
        }
    }



    public class AuctionEventTableData : ActionDbData
    {
        public static IList<AuctionEvent> getAuctionEvent(string query)
        {
            string queryFormatted = "SELECT * FROM auction_event " +
                " INNER JOIN auction_item on itemId = auctionItemId " +
                " INNER JOIN location on locationId = auctionLocationId " +
                (query ?? "");
            DataTable dataTable = getData(queryFormatted);
            return dataTable.AsEnumerable().Select(row => new AuctionEvent(row)).ToList();
        }
    }
}
