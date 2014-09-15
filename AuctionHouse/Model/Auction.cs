using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Auction
    {
        public delegate void BiddingDel();

        public event BiddingDel NewRound;
        public event BiddingDel NewBidAccepted;

        private Queue<AuctionItem> auctionItems;

        private AuctionItem _currentItem;
        private Auctioneer auctioneer;

        public AuctionItem CurrentItem
        {
            get { return _currentItem; }
            set { _currentItem = value; }
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
            this.auctioneer = auctioneer;
            auctioneer.CallThird += sellNextItem;
            _currentItem = auctionItems.Dequeue();

            // should we tell auctioneer explicitly? 
            NewRound();
        }

        private void sellNextItem(string message)
        {
            // sleep a bit, set next item on auction
            _currentItem = auctionItems.Dequeue();
            NewRound();
        }

        public bool PlaceBid(decimal bid)
        {
            bool success = CurrentItem.PlaceBid(bid);

            if (success == true)
            {
                NewBidAccepted();
            }

            return success;
        }

    }
}
