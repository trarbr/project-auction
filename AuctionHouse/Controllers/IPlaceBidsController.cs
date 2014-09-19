using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Controllers.Structs;
using Common.Delegates;

namespace Controllers
{
    public interface IPlaceBidsController
    {
        event AuctionEvent NewRound;
        event AuctionEvent NewBidAccepted;
        event AuctioneerEvent CallFirst;
        event AuctioneerEvent CallSecond;
        event AuctioneerEvent CallThird;

        string JoinAuction();
        SAuctionItem GetCurrentItem();
        bool PlaceBid(SAuctionItem auctionItem, decimal amount, string bidder);
    }
}
