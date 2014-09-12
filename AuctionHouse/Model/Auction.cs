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

        public AuctionItem CurrentItem
        {
            get { return _currentItem; }
            set { _currentItem = value; }
        }
        

        public Auction(Queue<AuctionItem> Items)
        {
            auctionItems = Items;
        }

        public void Start()
        {
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
