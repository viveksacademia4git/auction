using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction
{
    public class AuctionDb : DbContext
    {
        public DbSet<Users> Users { get; set; }
    }

    public class Users
    {

        /**
         * User Item POCO
         * TableName: users
         */
        public Users() { }

        public Users(DataRow row)
        {
            userId = row.Field<int>("userId");
            userName = row.Field<string>("userName");
            pwd = row.Field<string>("pwd");
            fullname = row.Field<string>("fullname");
            deleteFlag = row.Field<bool>("deleteFlag");
            userType = row.Field<int>("userType");
        }

        public int userId { get; set; }
        public string userName { get; set; }
        public string pwd { get; set; }
        public string fullname { get; set; }
        public int userType { get; set; }
        public bool deleteFlag { get; set; }

        public string toString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString();
        }

    }

    public class UserType
    {
        public static int Admin = 1;
        public static int Publisher = 2;
        public static int Viewer = 3;
    }


    /**
     * Auction Item POCO
     * TableName: auction_item
     */
    public class AuctionItem
    {
        public AuctionItem() { }

        public AuctionItem(DataRow row)
        {
            itemId = row.Field<int>("itemId");
            itemName = row.Field<string>("itemName");
            initialPrice = row.Field<decimal>("initialPrice");
            additionalInfo = row.Field<string>("additionalInfo");
            sold = row.Field<bool>("sold");
            deleteFlag = row.Field<bool>("deleteFlag");
            auctioned = sold ? "Yes" : "No" ;
        }

        public int itemId { get; set; }
        public string itemName { get; set; }
        public decimal initialPrice { get; set; }
        public string additionalInfo { get; set; }
        public bool sold { get; set; }
        public bool deleteFlag { get; set; }
        public string auctioned { get; set; }

        public string toString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString();
        }

    }
}
