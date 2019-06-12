using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Auction
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataCxt dataCxt;
        ControlFunctions controlFunctions;

        AuctionItem auctionItem;
        AuctionLocation auctionLocation;
        AuctionEvent auctionEvent;


        AuctionItem auctionItemCopy;

        int auctionItemSelectedIndex = -1;
        int auctionLocationSelectedIndex = -1;
        int auctionEventSelectedIndex = -1;

        public MainWindow() {
            initiateComponent(1);
        }

        public MainWindow(Users users) {
            initiateComponent(users.userType);
        }

        private void initiateComponent(int userType) {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de");
            InitializeComponent();
            // Enable-Disable and Show-Hide Windows Components based on the User Privileges
            controlFunctions = new ControlFunctions(userType);
            // Item Sources
            lbxItem_Grid.ItemsSource = controlFunctions.auctionItems;
            lbxLocation_Grid.ItemsSource = controlFunctions.auctionLocations;
            lbxEvent_Grid.ItemsSource = controlFunctions.auctionEvents;
            // Data Context Objects
            dataCxt = new DataCxt { aItem = new AuctionItem(), aLocation = new AuctionLocation() };
            this.DataContext = dataCxt;
        }

        private void Item_Reset(AuctionItem ai) {
            dataCxt = new DataCxt { aItem = ai, aLocation = dataCxt.aLocation };
            this.DataContext = dataCxt;
            Item_ResetLbl();
        }

        private void Location_Reset(AuctionLocation al) {
            dataCxt = new DataCxt { aItem = dataCxt.aItem, aLocation = al };
            this.DataContext = dataCxt;
        }

        private void Item_ResetLbl() {
            lblItem_Error.Visibility = Visibility.Hidden;
            lblItem_Success.Visibility = Visibility.Hidden;
            lblItem_ValueNotDefined.Visibility = Visibility.Hidden;
            lblItem_NoChangeInValue.Visibility = Visibility.Hidden;
            lblItem_ValueNotUpdatedError.Visibility = Visibility.Hidden;
            lblItem_DefineMandatoryValues.Visibility = Visibility.Hidden;
        }

        private void Location_ResetLbl() {
            lblLocation_Error.Visibility = Visibility.Hidden;
            lblLocation_Success.Visibility = Visibility.Hidden;
            lblLocation_ValueNotDefined.Visibility = Visibility.Hidden;
            lblLocation_NoChangeInValue.Visibility = Visibility.Hidden;
            lblLocation_ValueNotUpdatedError.Visibility = Visibility.Hidden;
            lblLocation_DefineMandatoryValues.Visibility = Visibility.Hidden;
        }

        public void FocusLostInt(object sender, RoutedEventArgs e) {
            string txt = ((TextBox)sender).Text;
            if (txt == "" || !int.TryParse(txt, out int n))
                ((TextBox)sender).Text = "0";
        }



        /* *************************** CODE RELATED TO AUCTION ITEM *************************** */

        public void DblClickItem_Grid(object sender, MouseEventArgs e) {
            auctionItem = (AuctionItem)lbxItem_Grid.SelectedItem;
            auctionItemSelectedIndex = lbxItem_Grid.SelectedIndex;
            Item_Reset(new AuctionItem(auctionItem));
        }

        public void ClickItem_Submit(object sender, RoutedEventArgs e) {
            Item_ResetLbl();
            AuctionItem aItem = dataCxt.aItem;
            if (aItem == null) {
                lblItem_ValueNotDefined.Visibility = Visibility.Visible;
                return;
            }
            // No Change In Values
            if (auctionItem!=null && aItem.toString().Equals(auctionItem.toString())) {
                lblItem_NoChangeInValue.Visibility = Visibility.Visible;
                return;
            }
            // Mandatory Values Check
            if (!aItem.mandatoryCheck()) {
                lblItem_DefineMandatoryValues.Visibility = Visibility.Visible;
                return;
            }
            // Insert/Update Values
            int selectedIndex = controlFunctions.submitItem(aItem, auctionItemSelectedIndex);
            if(selectedIndex>0) {
                Item_Reset(new AuctionItem());
                lbxItem_Grid.SelectedIndex = selectedIndex-1;
                lblItem_Success.Visibility = Visibility.Visible;
            }
            else if (selectedIndex > -1) {
                lblItem_ValueNotUpdatedError.Visibility = Visibility.Visible;
            }
            else {
                lblItem_Error.Visibility = Visibility.Visible;
            }
        }

        public void ClickItem_Reset(object sender, RoutedEventArgs e) {
            Item_Reset(new AuctionItem());
        }


        public void TxtChngItem_Filter_Search(object sender, TextChangedEventArgs e) {
            string q = txtItem_Filter_SearchQuantity.Text;
            string ip = txtItem_Filter_SearchPrice.Text;

            AuctionItem ai = new AuctionItem {
                itemName = txtItem_Filter_SearchName.Text,
                itemType = txtItem_Filter_SearchType.Text,
                additionalInfo = txtItem_Filter_SearchInformation.Text,
                owner = txtItem_Filter_SearchOwner.Text,
                quantity = int.TryParse(q, out int n) ? int.Parse(q) : -1,
                initialPrice = int.TryParse(ip, out int m) ? int.Parse(ip) : -1
            };
            controlFunctions.searchItem(ai);
        }

        public void ClickItem_Copy(object sender, RoutedEventArgs e) {
            auctionItemCopy = new AuctionItem().setAuctionItem(auctionItem);
        }



        /* *************************** CODE RELATED TO AUCTION LOCATION *************************** */

        public void DblClickLocation_Grid(object sender, MouseEventArgs e) {
            auctionLocation = (AuctionLocation)lbxLocation_Grid.SelectedItem;
            auctionLocationSelectedIndex = lbxLocation_Grid.SelectedIndex;
            Location_Reset(new AuctionLocation(auctionLocation));
        }

        public void ClickLocation_Submit(object sender, RoutedEventArgs e) {
            Location_ResetLbl();
            AuctionLocation aLocation = dataCxt.aLocation;
            if (aLocation == null){
                lblLocation_ValueNotDefined.Visibility = Visibility.Visible;
                return;
            }
            // No Change In Values
            if (auctionLocation != null && aLocation.toString().Equals(auctionLocation.toString())){
                lblLocation_NoChangeInValue.Visibility = Visibility.Visible;
                return;
            }
            // Mandatory Values Check
            if (!aLocation.mandatoryCheck()){
                lblLocation_DefineMandatoryValues.Visibility = Visibility.Visible;
                return;
            }
            // Insert/Update Values
            int selectedIndex = controlFunctions.submitLocation(aLocation, auctionLocationSelectedIndex);
            if (selectedIndex > 0){
                Location_Reset(new AuctionLocation());
                lbxItem_Grid.SelectedIndex = selectedIndex-1;
                lblLocation_Success.Visibility = Visibility.Visible;
            }
            else if (selectedIndex > -1){
                lblLocation_ValueNotUpdatedError.Visibility = Visibility.Visible;
            }
            else{
                lblLocation_Error.Visibility = Visibility.Visible;
            }
        }

        public void ClickLocation_Reset(object sender, RoutedEventArgs e) {
            Location_Reset(new AuctionLocation());
        }


        public void TxtChngLocation_Filter_Search(object sender, TextChangedEventArgs e) {
            string cap = txtLocation_Filter_SearchCapacity.Text;

            var al = new AuctionLocation {
                locationName = txtLocation_Filter_SearchName.Text,
                address = txtLocation_Filter_SearchAddress.Text,
                place = txtLocation_Filter_SearchPlace.Text,
                contactPerson = txtLocation_Filter_SearchContactPerson.Text,
                availability = txtLocation_Filter_SearchAvailability.Text,
                capacity = int.TryParse(cap, out int n) ? int.Parse(cap) : -1
            };
            controlFunctions.searchLocation(al);
        }



        /* *************************** CODE RELATED TO AUCTION Event *************************** */

        public void DblClickEvent_Grid(object sender, MouseEventArgs e)
        {
            auctionEvent = (AuctionEvent)lbxEvent_Grid.SelectedItem;
            auctionEventSelectedIndex = lbxEvent_Grid.SelectedIndex;
            // TODO - Need to add logic to show the data somewhere
            //Item_Reset(new AuctionEvent().setAuctionItem(auctionItem));
        }
    }

}
