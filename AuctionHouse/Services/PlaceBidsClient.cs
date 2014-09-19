using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Common.Delegates;
using Controllers.Structs;
using Controllers;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace Services
{
    public class PlaceBidsClient : IPlaceBidsController
    {
        private TcpClient commandClient;

        public event AuctionEvent NewRound;
        public event AuctionEvent NewBidAccepted;
        public event AuctioneerEvent CallFirst;
        public event AuctioneerEvent CallSecond;
        public event AuctioneerEvent CallThird;

        private string serverIp;
        private int serverPort;
        private StreamReader commandReader;
        private StreamWriter commandWriter;
        private StreamReader eventReader;

        public PlaceBidsClient(string serverIp, int serverPort)
        {
            this.serverIp = serverIp;
            this.serverPort = serverPort;
        }

        public string JoinAuction()
        {
            return connect();
        }

        public SAuctionItem GetCurrentItem()
        {
            commandWriter.WriteLine("get");
            string itemString = commandReader.ReadLine();
            SAuctionItem item = JsonConvert.DeserializeObject<SAuctionItem>(itemString);

            return item;
        }

        public bool PlaceBid(SAuctionItem auctionItem, decimal amount, string bidder)
        {
            string itemAsString = JsonConvert.SerializeObject(auctionItem);

            bidder += commandClient.Client.LocalEndPoint.ToString(); 

            commandWriter.WriteLine("bid|" + itemAsString + "|" + amount + "|" + bidder);

            bool success;
            bool.TryParse(commandReader.ReadLine(), out success);

            return success;
        }

        private string connect()
        {
            commandClient = new TcpClient(serverIp, serverPort);
            NetworkStream commandStream = commandClient.GetStream();
            commandReader = new StreamReader(commandStream);
            commandWriter = new StreamWriter(commandStream);
            commandWriter.AutoFlush = true;
            string welcomeMessage = commandReader.ReadLine() + "\n";

            TcpClient eventClient = new TcpClient(serverIp, 16001);
            NetworkStream eventStream = eventClient.GetStream();
            eventReader = new StreamReader(eventStream);
            Thread readEvetsThread = new Thread(new ThreadStart(readEvents));
            readEvetsThread.Start();

            return welcomeMessage;
        }

        private void readEvents()
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
                else if (message.Contains("Third"))
                {
                    CallThird(message);
                }
            }
        }
    }
}
