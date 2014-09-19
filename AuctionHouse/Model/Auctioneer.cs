using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Common.Delegates;

namespace Model
{
    public class Auctioneer
    {
        public event AuctioneerEvent CallFirst;
        public event AuctioneerEvent CallSecond;
        public event AuctioneerEvent CallThird;

        private Auction auction;
        private Timer timer;
        private int callNumber;
        private int firstTimeout;
        private int secondTimeout;
        private int thirdTimeout;

        public Auctioneer(Auction auction, int firstTimeout, int secondTimeout, int thirdTimeout)
        {
            this.auction = auction;
            this.firstTimeout = firstTimeout;
            this.secondTimeout = secondTimeout;
            this.thirdTimeout = thirdTimeout;

            auction.NewRound += startCountDown;
            auction.NewBidAccepted += resetTimer;

            timer = new Timer();
            timer.Elapsed += timerSignal;
        }

        private void startCountDown()
        {
            // callNumber is used to determine if the Auctioneer should call First, Second or Third
            callNumber = 1;

            timer.Interval = firstTimeout;
            timer.Start();
        }

        private void resetTimer()
        {
            timer.Stop();
            startCountDown();
        }

        private void timerSignal(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            if (callNumber == 1)
            {
                CallFirst("First!");
                callNumber++;
                timer.Interval = secondTimeout;
                timer.Start();
            }
            else if (callNumber == 2)
            {
                CallSecond("Second!");
                callNumber++;
                timer.Interval = thirdTimeout;
                timer.Start();
            }
            else
            {
                bool isSold = auction.IsCurrentItemSold();

                AuctionItem item = auction.CurrentItem;

                if (isSold)
                {
                    CallThird("Third! " + item.ItemName + " sold to " + item.HighestBidder + " for " + item.Bid + " HollarDollars!");
                }
                else
                {
                    CallThird("Third! " + item.ItemName + " not sold");
                }
            }
        }
    }
}
