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
        public event AuctionEvent NewRound;
        public event AuctionEvent NewBidAccepted;
        public event AuctioneerEvent CallFirst;
        public event AuctioneerEvent CallSecond;
        public event AuctioneerEvent CallThird;

        private object lockage = new object();
        private Auction currentAuction;

        public AuctionController()
        {
            // make list of AuctionItems
            // make new Auction, set as currentAuction
            // subscribe to events from currentAuction
            // make Auctioneer, pass in Auction

            AuctionItem item = new AuctionItem(1, "chair", 100, 10000);
            currentAuction = new Auction();
            Auctioneer auctioneer = new Auctioneer(currentAuction);

            // resend the events
            currentAuction.NewRound += NewRound;
            currentAuction.NewBidAccepted += NewBidAccepted;

            auctioneer.CallFirst += callFirst;
            auctioneer.CallSecond += callSecond;
            auctioneer.CallThird += callThird;

            currentAuction.AddItem(item);
            currentAuction.Start(auctioneer);
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
            lock (lockage)
            {
                AuctionItem currentItem = currentAuction.CurrentItem;
                // what if the auction is sold in between the individual locks expiring..?
                // this is going to be invalid (stale) data
                // to avoid that, lock on the auction...
                SAuctionItem sCurrentItem = new SAuctionItem()
                {
                    Id = currentItem.Id,
                    Description = currentItem.ItemName,
                    MaxBid = currentItem.Bid
                };
                return sCurrentItem;
            }
            

            
        }

        public bool PlaceBid(SAuctionItem auctionItem, decimal amount)
        {
            // only pass the bid if the SAuctionItem.Id matches currentItem.Id
            // that's application specific business logic magic happening right there!

            // if you want to pass it on to currentAuction without checking Id (so you don't need 
            // currentItem), Auction needs a finder method which returns the currentItem on given
            // Id)
            return currentAuction.PlaceBid(auctionItem.Id, amount);
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
    }
}
