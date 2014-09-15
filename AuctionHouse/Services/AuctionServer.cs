using Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    class AuctionServer
    {
        public IPAddress ip = IPAddress.Parse("127.0.0.1");
        private int port;
        private AuctionController auctionController;

        public AuctionServer(int port)
        {
            this.port = port;
            this.auctionController = new AuctionController();
        }

        public void Run()
        {
            TcpListener listener = new TcpListener(ip, port);
            listener.Start();

            while (true)
            {
                Socket socket = listener.AcceptSocket();

                new Thread(new ThreadStart(new BidHandler(auctionController, socket).Run)).Start();
            }
        }


    }
}
