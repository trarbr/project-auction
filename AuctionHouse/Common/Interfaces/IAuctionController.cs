using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Structs;

namespace Common.Interfaces
{
    public interface IAuctionController
    {
        // JoinAuction should maybe return a bidder? Otherwise seems unnecessary
        // SAuction JoinAuction();

        SAuctionItem GetCurrentItem();

        // PlaceBid should maybe return something else than just a bool?
        bool PlaceBid(SAuctionItem auctionItem, decimal amount);

        /*
        event NewItem
        event NewMaxBid
        event First
        event Second
        event Third
        */
    }
}
