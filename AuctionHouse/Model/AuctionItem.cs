using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AuctionItem
    {
        public int Id 
        {
            get 
            {
                lock (auctionItemLock)
                {
                    return _id; 
                }
            }
        }
        public string ItemName
        {
            get 
            {
                lock (auctionItemLock)
                {
                    return _itemName; 
                }
            }
        }
        public decimal Bid
        {
            get 
            {
                lock (auctionItemLock)
                {
                    return _bid; 
                }
            }
        }

        private string _highestBidder;

        public string HighestBidder
        {
            get
            {
                lock (auctionItemLock)
                {
                    return _highestBidder;
                }
            }
        }
        

        private int _id;
        private string _itemName;
        private decimal _bid;

        private bool sold;
        private decimal minimumSoldPrice;
        private object auctionItemLock;
        
        public AuctionItem(int id, string itemName, decimal minimumBid, decimal minimumSoldPrice)
        {
            // No race conditions in the constructor, as the item has not yet been added to the 
            // Auction, so no use of lock.
            auctionItemLock = new object();
            _id = id;
            _itemName = itemName;
            _bid = minimumBid;
            this.minimumSoldPrice = minimumSoldPrice;
        }

        public bool PlaceBid(decimal amount, string bidder)
        {
            bool success = false;
            lock (auctionItemLock)
            {
                if (amount > _bid && sold == false)
                {
                    _bid = amount;
                    success = true;
                    _highestBidder = bidder;
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }

        public bool EvaluateIfSold()
        {
            lock (auctionItemLock)
            {
                if (_bid > minimumSoldPrice)
                {
                    sold = true;
                }
                else
                {
                    sold = false;
                }
            }

            return sold;
        }
    }
}
