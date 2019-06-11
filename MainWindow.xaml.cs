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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Auction
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ControlsUtil controlsUtil;
        ObservableCollection<AuctionItem> auctionItems;

        public MainWindow()
        {
            initiateComponent(3);
        }

        public MainWindow(Users users)
        {
            initiateComponent(users.userType);
        }

        private void initiateComponent(int userType)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de");
            InitializeComponent();

            controlsUtil = new ControlsUtil(userType);
            // controlsUtil.chkControlItemSold(chkItem_Sold);

            auctionItems = new ObservableCollection<AuctionItem>(AuctionItemTableData.getActionItems(null));
            lbxItem_Items.ItemsSource = auctionItems;
        }


        private void ClickItem_Submit(object sender, RoutedEventArgs e)
        {

        }
    }
}
