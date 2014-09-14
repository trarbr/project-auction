using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Structs;
using Common.Interfaces;

namespace Services
{
    class BidClient : IAuctionController
    {
        public BidClient()
        {
            // give it IP and port of server?
        }

        public void Connect()
        {
            // connect to the server, setup NetworkStream, StreamReader and StreamWriter
        }

        /* Only needed if the application needs a Bidder to keep track of bids
         * Then it returns a Bidder, not an SAuction! Bidder has not been added to Model yet
        public SAuction JoinAuction()
        {
            // send request
            // receive response
            // serialize into struct
            throw new NotImplementedException();
        }
        */

        public SAuctionItem GetCurrentItem()
        {
            // send request
            // receive response
            // deserialize into struct
            throw new NotImplementedException();
        }


        public bool PlaceBid(SAuctionItem auctionItem, decimal amount)
        {
            // beware of handling spaces correctly in the protocol!
            // maybe split on something other than spaces, like " | "
            // anyway, it's going to be pretty fragile
            // it might be less fragile if combining the args into a single Bid struct
            // and the controller wouldn't even have to know about it!

            throw new NotImplementedException();
        }

        // BidClient also needs to provide all the events that IAuctionController specify.
        // It needs to have a while-true method that reads from StreamReader and, if the message
        // comes from an event, it needs to fire that event to the user.
    }
}
