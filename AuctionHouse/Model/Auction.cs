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

        private object objlock = new object();
        private bool isActive;

        public AuctionItem CurrentItem
        {
            
            get 
            {
                lock (objlock){ return _currentItem; }
            }
            set 
            {
                lock (objlock){ _currentItem = value; }
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
            isActive = true;
            this.auctioneer = auctioneer;
            auctioneer.CallThird += sellNextItem;
            _currentItem = auctionItems.Dequeue();

            // should we tell auctioneer explicitly? 
            NewRound();
        }

        private void sellNextItem(string message)
        {
            isActive = false;
            Thread.Sleep(1000);
            isActive = true;
            // sleep a bit, set next item on auction
            lock (objlock)
            {
                if (auctionItems.Count != 0)
                {
                    _currentItem = auctionItems.Dequeue();
                    NewRound();
                }
                
            }
            //problems having signale calls inside a lock?
            //NewRound();
        }

        public bool PlaceBid(int id, decimal bid, string bidder)
        {
            lock (objlock)
            {
                bool success = false;
                if (_currentItem.Id == id && isActive)
                {
                    success = _currentItem.PlaceBid(bid, bidder);
                    if (success)
                    {
                        NewBidAccepted();
                    }
                }
                return success;
            }
            
        }

        public bool IsCurrentItemSold()
        {
            lock (objlock)
            {
                return _currentItem.EvaluateIfSold();
            }
        }


    }
}
