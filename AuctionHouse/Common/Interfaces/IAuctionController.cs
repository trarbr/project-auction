﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Structs;

namespace Common.Interfaces
{
    // what on earth are those things doing here?
    public delegate void AuctioneerEvent(string message);
    public delegate void AuctionEvent();

    public interface IAuctionController
    {
        // JoinAuction should maybe return a bidder? Otherwise seems unnecessary
        // SAuction JoinAuction();

        SAuctionItem GetCurrentItem();

        // PlaceBid should maybe return something else than just a bool?
        bool PlaceBid(SAuctionItem auctionItem, decimal amount);

        event AuctionEvent NewRound;
        event AuctionEvent NewBidAccepted;

        event AuctioneerEvent CallFirst;
        event AuctioneerEvent CallSecond;
        event AuctioneerEvent CallThird;
    }
}
