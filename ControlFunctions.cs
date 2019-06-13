using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction
{
    public class DataCxt
    {
        public AuctionItem aItem { get; set; }
        public AuctionLocation aLocation { get; set; }
    }


    public class ControlFunctions
    {
        private ControlsUtil controlsUtil;
        public ObservableCollection<AuctionItem> auctionItems;
        public ObservableCollection<AuctionLocation> auctionLocations;
        public ObservableCollection<AuctionEvent> auctionEvents;

        public ControlFunctions(int userType) {
            controlsUtil = new ControlsUtil(userType);
            // controlsUtil.chkControlItemSold(chkItem_Sold);

            string queryItems = " ORDER BY itemId DESC LIMIT 20 ";
            auctionItems = new ObservableCollection<AuctionItem>(AuctionItemTableData.getAuctionItems(queryItems));

            string queryLocations = " ORDER BY locationId DESC LIMIT 20 ";
            auctionLocations = new ObservableCollection<AuctionLocation>(AuctionLocationTableData.getAuctionLocations(queryLocations));

            string queryEvents = " ORDER BY eventId DESC LIMIT 20 ";
            auctionEvents = new ObservableCollection<AuctionEvent>(AuctionEventTableData.getAuctionEvent(queryEvents));
        }


        // Inserts Auction Item
        // And Returns the Selected Index
        public int submitItem(AuctionItem auctionItem, int auctionItemSelectedIndex)
        {
            int executeQueryRowCount = -1;
            if (auctionItem.itemId < 1) {
                auctionItemSelectedIndex = 0;
                executeQueryRowCount = AuctionItemTableData.insertAuctionItem(auctionItem);
                if(executeQueryRowCount>0)
                    auctionItems.Insert(auctionItemSelectedIndex, auctionItem);
            }
            else {
                executeQueryRowCount = AuctionItemTableData.updateAuctionItem(auctionItem);
                if (executeQueryRowCount > 0) {
                    auctionItems.Insert(auctionItemSelectedIndex, auctionItem);
                    auctionItems.RemoveAt(auctionItemSelectedIndex+1);
                }
            }
            if (executeQueryRowCount < 1)
                return -1;
            return auctionItemSelectedIndex + 1;
        }

        public void searchItem(AuctionItem ai)
        {
            string queryItems = " ORDER BY itemId DESC LIMIT 20 ";
            auctionItems.Clear();
            var ais = new ObservableCollection<AuctionItem>(AuctionItemTableData.getAuctionItems(ai, queryItems));
            foreach(AuctionItem aiTemp in ais)
                auctionItems.Add(aiTemp);
            Console.WriteLine(auctionItems.Count);
        }

        // Inserts Auction Location
        // And Returns the Selected Index
        public int submitLocation(AuctionLocation auctionLocation, int auctionLocationSelectedIndex) {
            int executeQueryRowCount = -1;
            if (auctionLocation.locationId < 1) {
                auctionLocationSelectedIndex = 0;
                executeQueryRowCount = AuctionLocationTableData.insertAuctionLocation(auctionLocation);
                if (executeQueryRowCount > 0)
                    auctionLocations.Insert(auctionLocationSelectedIndex++, auctionLocation);
            }
            else {
                executeQueryRowCount = AuctionLocationTableData.updateAuctionLocation(auctionLocation);
                if (executeQueryRowCount > 0) {
                    auctionLocations.Insert(auctionLocationSelectedIndex++, auctionLocation);
                    auctionLocations.RemoveAt(auctionLocationSelectedIndex);
                }
            }
            if (executeQueryRowCount < 1)
                return -1;
            return auctionLocationSelectedIndex;
        }

        public void searchLocation(AuctionLocation al)
        {
            string queryItems = " ORDER BY locationId DESC LIMIT 20 ";
            auctionLocations.Clear();
            var als = new ObservableCollection<AuctionLocation>(AuctionLocationTableData.getAuctionLocations(al, queryItems));
            foreach (AuctionLocation alTemp in als)
                auctionLocations.Add(alTemp);
            Console.WriteLine(auctionLocations.Count);
        }
    }
}
