using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Interfaces;
using Common.Structs;
using Model;

namespace Controllers
{
    public class AuctionController : IAuctionController
    {
        private AuctionItem currentItem;
        private Auction currentAuction;

        public AuctionController()
        {
            // make list of AuctionItems
            // make new Auction, set as currentAuction
            // subscribe to events from currentAuction
            // make Auctioneer, pass in Auction

            currentItem = new AuctionItem("chair", 100, 10000);
            Queue<AuctionItem> itemsQueue = new Queue<AuctionItem>();
            itemsQueue.Enqueue(currentItem);
            currentAuction = new Auction(itemsQueue);
            // call currentAuction.Start()
        }

        /* not needed
        public SAuction JoinAuction()
        {
            // this really should return a bidder, eh?
            return new SAuction();
        }
        */

        public SAuctionItem GetCurrentItem()
        {
            // what if the auction is sold in between the individual locks expiring..?
            // this is going to be invalid (stale) data
            // to avoid that, lock on the auction...
            SAuctionItem sCurrentItem = new SAuctionItem()
            {
                Id = this.currentItem.Id,
                Description = this.currentItem.ItemName,
                MaxBid = this.currentItem.Bid
            };

            return sCurrentItem;
        }

        public bool PlaceBid(SAuctionItem auctionItem, decimal amount)
        {
            // only pass the bid if the SAuctionItem.Id matches currentItem.Id
            // that's application specific business logic magic happening right there!

            // if you want to pass it on to currentAuction without checking Id (so you don't need 
            // currentItem), Auction needs a finder method which returns the currentItem on given
            // Id)
            bool success = false;
            if (auctionItem.Id == currentItem.Id)
            {
                success = currentAuction.PlaceBid(amount);
            }

            return success;
        }
    }
}
