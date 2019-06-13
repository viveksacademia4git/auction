using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class AuctionTable
    {

        public bool blank(string str)
        {
            return str == null || str.Trim().Equals("");
        }
    }

    public class Users : AuctionTable
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
            //deleteFlag = row.Field<bool>("deleteFlag");
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

    public class UserType : AuctionTable
    {
        public static int Admin = 1;
        public static int Publisher = 2;
        public static int Viewer = 3;
    }


    /**
     * Auction Item POCO
     * TableName: auction_item
     */
    public class AuctionItem : AuctionTable
    {
        public AuctionItem()
        {
            auctioned = "";
            quantity = 1;
            sold = false;
        }
        public AuctionItem(AuctionItem ai) {
            setAuctionItem(ai);
        }

        public AuctionItem(DataRow row)
        {
            itemId = row.Field<int>("itemId");
            itemName = row.Field<string>("itemName");
            itemType = row.Field<string>("itemType");
            additionalInfo = row.Field<string>("additionalInfo");
            owner = row.Field<string>("owner");
            quantity = row.Field<int>("quantity");
            initialPrice = row.Field<int>("initialPrice");
            sold = row.Field<bool>("sold");
            //deleteFlag = row.Field<bool>("deleteFlag");
            auctioned = sold ? "Yes" : "";
        }

        public int itemId { get; set; }
        public string itemName { get; set; }
        public string itemType { get; set; }
        public string additionalInfo { get; set; }
        public string owner { get; set; }
        public int quantity { get; set; }
        public int initialPrice { get; set; }
        public bool sold { get; set; }
        public bool deleteFlag { get; set; }
        public string auctioned { get; set; }

        public AuctionItem setAuctionItem(AuctionItem ai)
        {
            itemId = ai.itemId;
            itemName = ai.itemName;
            itemType = ai.itemType;
            additionalInfo = ai.additionalInfo;
            owner = ai.owner;
            quantity = ai.quantity;
            initialPrice = ai.initialPrice;
            sold = ai.sold;
            //deleteFlag = ai.deleteFlag;
            auctioned = ai.sold ? "Yes" : "";
            return this;
        }

        public bool mandatoryCheck()
        {
            if (blank(itemName) || blank(itemType) || blank(additionalInfo) || blank(owner))
                return false;
            if (quantity < 1 || initialPrice < 0)
                return false;
            return true;
        }

        public string toString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString();
        }
    }

    /**
     * Auction Item POCO
     * TableName: auction_item
     */
    public class AuctionLocation : AuctionTable
    {
        public AuctionLocation() {
            capacity = 1;
            contact = "";
            addressComplete = "";
        }
        public AuctionLocation(AuctionLocation al) {
            setAuctionLocation(al);
        }

        public AuctionLocation(DataRow row) {
            locationId = row.Field<int>("locationId");
            locationName = row.Field<string>("locationName");
            address = row.Field<string>("address");
            availability = row.Field<string>("availability");
            place = row.Field<string>("place");
            zipcode = row.Field<string>("zipcode");
            capacity = row.Field<int>("capacity");
            contactPerson = row.Field<string>("contactPerson");
            phone = row.Field<string>("phone");
            email = row.Field<string>("email");
            //deleteFlag = row.Field<bool>("deleteFlag");
            contact = (blank(phone)? "" : phone + ", ") + (blank(email) ? "" : email);
            addressComplete = address + "\n" + zipcode;
        }

        public int locationId { get; set; }
        public string locationName { get; set; }
        public string address { get; set; }
        public string availability { get; set; }
        public string place { get; set; }
        public string zipcode { get; set; }
        public int capacity { get; set; }
        public string contactPerson { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool deleteFlag { get; set; }
        public string contact { get; set; }
        public string addressComplete { get; set; }

        public AuctionLocation setAuctionLocation(AuctionLocation al)
        {
            locationId = al.locationId;
            locationName = al.locationName;
            address = al.address;
            availability = al.availability;
            place = al.place;
            zipcode = al.zipcode;
            capacity = al.capacity;
            contactPerson = al.contactPerson;
            phone = al.phone;
            email = al.email;
            //deleteFlag = al.deleteFlag;
            contact = (phone != null && phone != null ? phone + ", " : "") + (email ?? "");
            addressComplete = address + "\n" + zipcode;
            return this;
        }

        public bool mandatoryCheck()
        {
            if (blank(locationName) || blank(address) || blank(place) || blank(zipcode) || blank(availability))
                return false;
            if (capacity < 1)
                return false;
            return true;
        }

        public string toString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString();
        }
    }
    public class AuctionEvent : AuctionTable
    {
        public AuctionEvent() { conducted = false; }

        public AuctionEvent(DataRow row) {
            eventId = row.Field<int>("eventId");
            eventName = row.Field<string>("eventName");
            eventTime = row.Field<string>("eventTime");
            registrationFee = row.Field<int>("registrationFee");
            buyer = row.Field<string>("buyer");
            conducted = row.Field<bool>("conducted");
            auctionItem = new AuctionItem(row);
            auctionLocation = new AuctionLocation(row);
        }

        public int eventId { get; set; }
        public string eventName { get; set; }
        public string eventTime { get; set; }
        public int registrationFee { get; set; }
        public string buyer { get; set; }
        public bool conducted { get; set; }
        public AuctionItem auctionItem { get; set; }
        public AuctionLocation auctionLocation { get; set; }

        public AuctionEvent setAuctionEvent(AuctionEvent ae)
        {
            eventId = ae.eventId;
            eventName = ae.eventName;
            eventTime = ae.eventTime;
            registrationFee = ae.registrationFee;
            buyer = ae.buyer;
            conducted = ae.conducted;
            auctionItem = new AuctionItem(ae.auctionItem);
            auctionLocation = new AuctionLocation(ae.auctionLocation);
            return this;
        }
    }
}
