using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Common.Delegates;

namespace Model
{
    public class Auction
    {
        public event AuctionEvent NewRound;
        public event AuctionEvent NewBidAccepted;

        private Queue<AuctionItem> auctionItems;
        private AuctionItem _currentItem;
        private Auctioneer auctioneer;

        private object auctionLock = new object();
        private bool auctionRunning;

        public AuctionItem CurrentItem
        {
            get 
            {
                lock (auctionLock)
                { 
                    return _currentItem; 
                }
            }
        }

        public Auction()
        {
            auctionItems = new Queue<AuctionItem>();
        }

        public void AddItem(AuctionItem item)
        {
            auctionItems.Enqueue(item);
        }

        public void Start(Auctioneer auctioneer)
        {
            auctioneer.CallThird += sellNextItem;

            sellNextItem("");
        }

        // sellNextItem has to take a string as input because it subscribes to Auctioneer's 
        // CallThird, but the string is never used in sellNextItem.
        private void sellNextItem(string message)
        {
            // Stop the auction, wait a second then start the next round
            auctionRunning = false;
            Thread.Sleep(1000);
            auctionRunning = true;
            if (auctionItems.Count != 0)
            {
                lock (auctionLock)
                {
                    _currentItem = auctionItems.Dequeue();
                }

                NewRound();
            }
        }

        public bool PlaceBid(int id, decimal bid, string bidder)
        {
            bool success = false;
            lock (auctionLock)
            {
                // If the bid is for an old item or the Auction is currently paused, don't try
                // to place the bid.
                if (_currentItem.Id == id && auctionRunning)
                {
                    success = _currentItem.PlaceBid(bid, bidder);
                }
            }
            if (success)
            {
                NewBidAccepted();
            }

            return success;
        }

        public bool IsCurrentItemSold()
        {
            lock (auctionLock)
            {
                return _currentItem.EvaluateIfSold();
            }
        }
    }
}
