using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddEvent.xaml
    /// </summary>
    public partial class AddEvent : Window
    {
        AuctionItem auctionItemCopy;
        AuctionLocation auctionLocationCopy;
        MainWindow mainWindow;

        public AddEvent(AuctionItem  ai, AuctionLocation al, MainWindow mw)
        {
            auctionItemCopy = ai;
            auctionLocationCopy = al;
            mainWindow = mw;
            InitializeComponent();
            txtNewEvent_ItemName.Text = ai==null?"Item Not Copied":ai.itemName;
            txtNewEvent_Location.Text = al==null ? "Location Not Copied":al.locationName;
        }

        public void ClickEvent_Add(object sender, RoutedEventArgs e)
        {
            var name = txtNewEvent_Name.Text;
            var timing = txtNewEvent_Time.Text;
            var registrationFee = txtNewEvent_RegistrationFee.Text;
            if(blank(name) || blank(timing) || blank(registrationFee) || auctionItemCopy == null || auctionLocationCopy==null)
            {
                LblNewEvent_Error.Content = "Insert All Values";
                return;
            }

            string[] columns = { "eventName", "eventTime", "registrationFee", "auctionItemId", "auctionLocationId" };
            object[] values= { name, timing, registrationFee, 1, 1 };

            ActionDbData.insertRecord("auction_event", columns, values);
            mainWindow.clearCopyItem();

            this.Close();
        }
        public static bool blank(string str)
        {
            return str == null || str.Trim().Equals("");
        }
    }
}
