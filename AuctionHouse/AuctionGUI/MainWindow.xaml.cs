using Controllers;
using Controllers.Structs;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace AuctionGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPlaceBidsController placeBidsController;
        private SAuctionItem auctionItem;

        public MainWindow()
        {
            InitializeComponent();

            bool useAppLocally = false;

            if (useAppLocally)
            {
                // Connect directly to the Controller
                placeBidsController = new PlaceBidsController();
            }
            else
            {
                // Connect to the Controller through the SocketService:
                placeBidsController = new PlaceBidsClient("localhost", 13370);
            }

            placeBidsController.NewRound += newRound;
            placeBidsController.NewBidAccepted += newBidAccepted;
            placeBidsController.CallFirst += callFirst;
            placeBidsController.CallSecond += callSecond;
            placeBidsController.CallThird += callThird;
            logTextBox.Text += placeBidsController.JoinAuction();
            getCurrentItem();
        }

        private void newRound()
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logTextBox.Text += "New Round Started. \n"));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => yourBidLabel.Content = ""));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => placeBidTextBox.Text = ""));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => getCurrentItem()));

        }

        private void newBidAccepted()
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logTextBox.Text += "New Bid Accepted. \n"));
            Dispatcher.BeginInvoke(
                new ThreadStart(() => getCurrentItem()));
        }

        private void callThird(string message)
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logTextBox.Text += message + "\n"));
        }

        private void callSecond(string message)
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logTextBox.Text += message + "\n"));
        }

        private void callFirst(string message)
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logTextBox.Text += message + "\n"));
        }

        public void getCurrentItem()
        {
            //try
            //{
                auctionItem = placeBidsController.GetCurrentItem();
                itemLabel.Content = auctionItem.Description;
                currentBidLabel.Content = auctionItem.MaxBid;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void bidButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal amount;
                decimal.TryParse(placeBidTextBox.Text, out amount);
                bool success = placeBidsController.PlaceBid(auctionItem, amount, "adolf aahhhhhh");
                if (success == true)
                {
                    yourBidLabel.Content = amount + " (Accepted)";
                }
                else
                {
                    yourBidLabel.Content = "Bid not accepted";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void logTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            logTextBox.ScrollToEnd();
        }
    }
}
