using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Common.Structs;
using Common.Interfaces;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace Services
{
    public class BidClient : IAuctionController
    {
        private TcpClient commandClient;

        public event AuctionEvent NewRound;
        public event AuctionEvent NewBidAccepted;
        public event AuctioneerEvent CallFirst;
        public event AuctioneerEvent CallSecond;
        public event AuctioneerEvent CallThird;

        private string serverIp;
        private int port;
        private StreamReader commandReader;
        private StreamWriter commandWriter;
        private StreamReader eventReader;

        public BidClient(string serverIp, int port)
        {
            // give it IP and port of server?
            this.serverIp = serverIp;
            this.port = port;
        }

        public string Connect()
        {
            // connect to the server, setup NetworkStream, StreamReader and StreamWriter
            commandClient = new TcpClient(serverIp, port);
            NetworkStream commandStream = commandClient.GetStream();
            commandReader = new StreamReader(commandStream);
            commandWriter = new StreamWriter(commandStream);
            commandWriter.AutoFlush = true;
            string welcomeMessage = commandReader.ReadLine();

            TcpClient eventClient = new TcpClient(serverIp, 16001);
            NetworkStream eventStream = eventClient.GetStream();
            eventReader = new StreamReader(eventStream);

            Thread readForeverThread = new Thread(new ThreadStart(readForever));
            readForeverThread.Start();

            return welcomeMessage;
        }

        private void readForever()
        {
            string message;

            Thread.Sleep(500);
            while (true)
            {
                message = eventReader.ReadLine();    
                
                if (message.Contains("NewRound"))
                {
                    NewRound();
                }
                else if (message.Contains("NewBidAccepted"))
                {
                    NewBidAccepted();
                }
                else if (message.Contains("First"))
                {
                    CallFirst(message);
                }
                else if (message.Contains("Second"))
                {
                    CallSecond(message);
                }
                else if (message.Contains("Item"))
                {
                    CallThird(message);
                }
            }
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

                commandWriter.WriteLine("get");
                string itemString = commandReader.ReadLine();
                SAuctionItem item = JsonConvert.DeserializeObject<SAuctionItem>(itemString);

                return item;
        }


        public bool PlaceBid(SAuctionItem auctionItem, decimal amount, string bidder)
        {
            // beware of handling spaces correctly in the protocol!
            // maybe split on something other than spaces, like " | "
            // anyway, it's going to be pretty fragile
            // it might be less fragile if combining the args into a single Bid struct
            // and the controller wouldn't even have to know about it!
            string itemAsString = JsonConvert.SerializeObject(auctionItem);

            bidder += commandClient.Client.LocalEndPoint.ToString(); 

            commandWriter.WriteLine("bid|" + itemAsString + "|" + amount + "|" + bidder);

            bool success;
            bool.TryParse(commandReader.ReadLine(), out success);

            return success;
        }

        // BidClient also needs to provide all the events that IAuctionController specify.
        // It needs to have a while-true method that reads from StreamReader and, if the message
        // comes from an event, it needs to fire that event to the user.


        public void StartAuction()
        {
            throw new NotImplementedException();
        }
    }
}
