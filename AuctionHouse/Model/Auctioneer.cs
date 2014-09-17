using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Common.Interfaces;

namespace Model
{
    public class Auctioneer
    {
        public event AuctioneerEvent CallFirst;
        public event AuctioneerEvent CallSecond;
        public event AuctioneerEvent CallThird;

        private Timer timer;
        private int callNumber;
        private Auction auction;
        private int firstTimeout;
        private int secondTimeout;
        private int thirdTimeout;


        // Would be less painful to test if timer values were set in constructor
        public Auctioneer(Auction auction, int firstTimeout, int secondTimeout, int thirdTimeout)
        {
            this.auction = auction;
            auction.NewRound += startCountDown;
            auction.NewBidAccepted += resetTimer;

            this.firstTimeout = firstTimeout;
            this.secondTimeout = secondTimeout;
            this.thirdTimeout = thirdTimeout;

            timer = new Timer();
            timer.Elapsed += timerSignal;
        }

        private void startCountDown()
        {
            callNumber = 1;
            // timer.stop?

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
                // evaluate whether sold or not
                // might be easier to make the delegate send a string
                bool IsSold = auction.IsCurrentItemSold();

                AuctionItem item = auction.CurrentItem;

                if (IsSold)
                {
                    CallThird("Third! \n Item sold to " + item.HighestBidder + " for " + item.Bid + " HollarDollars!");
                }
                else
                {
                    CallThird("Third! \n Item not sold");
                }
            }
        }
    }
}
