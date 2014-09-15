using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Model
{
    public class Auctioneer
    {
        public delegate void TimerSignal(string message);

        public event TimerSignal CallFirst;
        public event TimerSignal CallSecond;
        public event TimerSignal CallThird;

        private Timer timer;
        private int callNumber;
        private Auction auction;

        public Auctioneer(Auction auction)
        {
            this.auction = auction;
            auction.NewRound += startCountDown;
            auction.NewBidAccepted += resetTimer;

            timer = new Timer();
            timer.Elapsed += timerSignal;
        }

        private void startCountDown()
        {
            callNumber = 1;
            // timer.stop?

            timer.Interval = 10000;
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
                timer.Interval = 5000;
                timer.Start();
            }
            else if (callNumber == 2)
            {
                CallSecond("Second!");
                callNumber++;
                timer.Interval = 3000;
                timer.Start();
            }
            else
            {
                // evaluate whether sold or not
                // might be easier to make the delegate send a string
                CallThird("Third! Sold or not? I have no idea!");
            }
        }
    }
}
