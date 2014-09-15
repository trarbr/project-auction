using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Common.Interfaces;
using Common.Structs;
using Newtonsoft.Json;

namespace Services
{
    class BidHandler
    {
        private IAuctionController auctionController;

        // constructor should also take a Socket, which it is passed from the server
        public BidHandler(IAuctionController auctionController)
        {
            this.auctionController = auctionController;
        
            // subscribe to events on controller
            // when an event is fired on the controller, it has to send a new message to the
            // client.

            // setup NetworkStream, StreamReader and StreamWriter
        }

        // needs a while-true method that reads from the StreamReader
        // then it needs an if-else or switch to evaluate which command to pass the string it just
        // read to (e.g. if strings first word is "bid" it should call placeBid...
        // this while-true method will be used by the server after instantiating the handler: 
        // the while-true method is passed to a new thread and started.

        private string getCurrentItem()
        {
            SAuctionItem item = auctionController.GetCurrentItem();

            string itemAsString = JsonConvert.SerializeObject(item);

            return itemAsString;
        }

        private bool placeBid(string itemAsString, string amountAsString)
        {
            SAuctionItem item = JsonConvert.DeserializeObject<SAuctionItem>(itemAsString);
            decimal amount = decimal.Parse(amountAsString);

            return auctionController.PlaceBid(item, amount);
        }

                private Socket socket;

        public BidHandler(Socket socket)
        {
            this.socket = socket;
        }

        internal void Run()
        {
            NetworkStream stream = new NetworkStream(socket);
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            Console.WriteLine("Server started.");
            Console.Write("IP: " + socket.RemoteEndPoint + " is connected");
            writer.WriteLine("you are connected!");


            string textFromClient = reader.ReadLine();

            bool BoolRun = true;

            while (BoolRun)
            {
                if (textFromClient == "hej")
                {
                    Console.WriteLine(socket.RemoteEndPoint + "Har sendt: " + textFromClient);

                    writer.WriteLine("YEEEEEEEAAAAH!!!!");
                }
                if (textFromClient == "exit" || textFromClient == "close")
                {
                    Console.WriteLine(socket.RemoteEndPoint + "Har sendt: " + textFromClient);

                    writer.WriteLine("Serveren lukkes!");
                    BoolRun = false;
                }
                else
                {
                    Console.WriteLine(socket.RemoteEndPoint + "Har sendt: " + textFromClient);
                    writer.WriteLine("Wrong input");
                }
            }

            writer.Close();
            reader.Close();
            stream.Close();
            socket.Close();
        }
    }
}
