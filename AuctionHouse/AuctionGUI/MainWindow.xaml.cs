using Common.Structs;
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

namespace AuctionGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BidClient bidClient;
        private SAuctionItem auctionItem;

        public MainWindow()
        {
            InitializeComponent();
            bidClient = new BidClient("localhost", 13370);
            bidClient.NewRound += newRound;
            bidClient.NewBidAccepted += newBidAccepted;
            bidClient.CallFirst += callFirst;
            bidClient.CallSecond += callSecond;
            bidClient.CallThird += callThird;
            logLabel.Content += bidClient.Connect();
            getCurrentItem();
        }

        private void newBidAccepted()
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logLabel.Content += "New bid event accepted!! \n"));
        }

        private void callThird(string message)
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logLabel.Content += message + "\n"));
        }

        private void callSecond(string message)
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logLabel.Content += message + "\n"));
        }

        private void callFirst(string message)
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logLabel.Content += message + "\n"));
        }

        public void getCurrentItem()
        {
            //try
            //{
                auctionItem = bidClient.GetCurrentItem();
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
                bool success = bidClient.PlaceBid(auctionItem, amount);
                if (success == true)
                {
                    yourBidLabel.Content = amount + " (Accepted)";
                }
                else
                {
                    yourBidLabel.Content = "lort";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void newRound()
        {
            Dispatcher.BeginInvoke(
                new ThreadStart(() => logLabel.Content += "New round event triggered!! \n"));
        }
    }
}
