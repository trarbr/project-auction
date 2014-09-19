using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Delegates;
using Controllers;
using Controllers.Structs;
using Model;

namespace Controllers
{
    public class PlaceBidsController : IPlaceBidsController
    {
        public event AuctionEvent NewRound;
        public event AuctionEvent NewBidAccepted;
        public event AuctioneerEvent CallFirst;
        public event AuctioneerEvent CallSecond;
        public event AuctioneerEvent CallThird;

        private Auction auction;
        private Auctioneer auctioneer;
        private bool auctionStarted;

        public PlaceBidsController()
        {
            auctionStarted = false;
            auction = new Auction();

            AuctionItem item1 = new AuctionItem(1, "Chair", 2000, 2000);
            auction.AddItem(item1);
            AuctionItem item2 = new AuctionItem(2, "Car", 50000, 70000);
            auction.AddItem(item2);
            AuctionItem item3 = new AuctionItem(3, "Couch", 400, 400);
            auction.AddItem(item3);

            auctioneer = new Auctioneer(auction, 10000, 5000, 3000);

            auction.NewRound += newRound;
            auction.NewBidAccepted += newBidAccepted;
            auctioneer.CallFirst += callFirst;
            auctioneer.CallSecond += callSecond;
            auctioneer.CallThird += callThird;
        }

        public string JoinAuction()
        {
            // If this is the first client that connects, start the auction.
            if (!auctionStarted)
            {
                auction.Start(auctioneer);
                auctionStarted = true;
            }

            return "Welcome.\n";
        }

        public SAuctionItem GetCurrentItem()
        {
            AuctionItem currentItem = auction.CurrentItem;
            SAuctionItem sCurrentItem = new SAuctionItem()
            {
                Id = currentItem.Id,
                ItemName = currentItem.ItemName,
                Bid = currentItem.Bid
            };
            return sCurrentItem;
        }

        public bool PlaceBid(SAuctionItem auctionItem, decimal amount, string bidder)
        {
            return auction.PlaceBid(auctionItem.Id, amount, bidder);
        }

        public void callFirst(string message)
        {
            CallFirst(message);
        }

        public void callSecond(string message)
        {
            CallSecond(message);
        }

        public void callThird(string message)
        {
            CallThird(message);
        }

        public void newBidAccepted()
        {
            NewBidAccepted();
        }

        public void newRound()
        {
            NewRound();
        }
    }
}
