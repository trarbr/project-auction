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
        public delegate void TimerSignal();

        public event TimerSignal First;
        public event TimerSignal Second;
        public event TimerSignal Third;


        private Auction auction;

        public Auctioneer(Auction auction)
        {
            this.auction = auction;
        }

        public void StartCountDown()
        {
            Timer timer = new Timer(10000);
            
            timer.Elapsed += callFirst;
            timer.Enabled = true;
        }

        private void callFirst(object sender, ElapsedEventArgs e)
        {
            Timer _timer = sender as Timer;
            First();

            _timer.Stop();

            Timer timer = new Timer(5000);

            timer.Elapsed += callSecond;
            timer.Enabled = true;
        }

        private void callSecond(object sender, ElapsedEventArgs e)
        {
            Second();

            Timer timer = new Timer(5000);

            timer.Elapsed += callThird;
            timer.Enabled = true;
        }

        private void callThird(object sender, ElapsedEventArgs e)
        {
            Third();

            Timer timer = new Timer(3000);

            throw new NotImplementedException();
        }


    }
}
