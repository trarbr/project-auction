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
        private PlaceBidsController placeBidsController;
        private object lockObj;

        public AuctionServer(int port)
        {
            this.port = port;
            this.placeBidsController = new PlaceBidsController();
            lockObj = new object();
        }

        public void Run()
        {
            TcpListener listener = new TcpListener(ip, port);
            listener.Start();

            while (true)
            {
                Socket socket = listener.AcceptSocket();

                new Thread(new ThreadStart(new PlaceBidsHandler(placeBidsController, socket, lockObj).Run)).Start();
            }
        }


    }
}
