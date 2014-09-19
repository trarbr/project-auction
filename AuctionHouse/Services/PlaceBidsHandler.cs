using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Controllers;
using Controllers.Structs;
using Newtonsoft.Json;
using System.Threading;

namespace Services
{
    class PlaceBidsHandler
    {
        private IPlaceBidsController placeBidsController;
        private Socket commandSocket;
        private Socket eventSocket;
        private StreamReader commandReader;
        private StreamWriter commandWriter;
        private StreamWriter eventWriter;
        private NetworkStream eventStream;
        private object lockObj;

        public PlaceBidsHandler(IPlaceBidsController placeBidsController, Socket commandSocket, object lockObj)
        {
            this.placeBidsController = placeBidsController;
            this.commandSocket = commandSocket;

            // This lock is used to lock access to port 16001 so that different Handlers don't try
            // to use it concurrently
            this.lockObj = lockObj;

            placeBidsController.NewRound += newRound;
            placeBidsController.NewBidAccepted += newBidAccepted;
            placeBidsController.CallFirst += first;
            placeBidsController.CallSecond += second;
            placeBidsController.CallThird += third;
        }

        internal void Run()
        {
            NetworkStream commandStream;

            lock (lockObj)
            {
                commandStream = new NetworkStream(commandSocket);
                commandReader = new StreamReader(commandStream);
                commandWriter = new StreamWriter(commandStream);
                commandWriter.AutoFlush = true;

                Console.WriteLine("Server started.");
                Console.WriteLine("IP: " + commandSocket.RemoteEndPoint + " is connected");
                commandWriter.WriteLine("you are connected!");
                
                createEventSocket();
            }

            placeBidsController.JoinAuction();

            bool BoolRun = true;

            string textFromClient;
            string[] textFromClientArray;

            while (BoolRun)
            {
                textFromClient = commandReader.ReadLine();
                textFromClientArray = textFromClient.Split('|');

                Console.WriteLine(commandSocket.RemoteEndPoint + ": " + textFromClient);

                if (textFromClientArray[0] == "bid")
                {
                    bool success = placeBid(textFromClientArray[1], textFromClientArray[2], textFromClientArray[3]);

                    commandWriter.WriteLine(success);
                }
                else if (textFromClientArray[0] == "get")
                {
                    string item = getCurrentItem();

                    commandWriter.WriteLine(item);
                }
                else if (textFromClientArray[0] == "exit")
                {
                    commandWriter.WriteLine("Fucking Off!");
                    BoolRun = false;
                }
                else
                {
                    commandWriter.WriteLine("Skriv nu for helvede korrekt!... ALTSÅ!..TSK");
                }
            }

            commandWriter.Close();
            commandReader.Close();
            commandStream.Close();
            commandSocket.Close();

            eventWriter.Close();
            eventStream.Close();
            eventSocket.Close();
        }

        private void createEventSocket()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 16001);
            listener.Start();
            
            eventSocket = listener.AcceptSocket();

            eventStream = new NetworkStream(eventSocket);
            eventWriter = new StreamWriter(eventStream);
            eventWriter.AutoFlush = true;

            listener.Stop();
        }

        private string getCurrentItem()
        {
            SAuctionItem item = placeBidsController.GetCurrentItem();
            string itemAsString = JsonConvert.SerializeObject(item);

            return itemAsString;
        }

        private bool placeBid(string itemAsString, string amountAsString, string bidder)
        {
            SAuctionItem item = JsonConvert.DeserializeObject<SAuctionItem>(itemAsString);
            decimal amount = decimal.Parse(amountAsString);

            return placeBidsController.PlaceBid(item, amount, bidder);
        }

        private void first(string message)
        {
            eventWriter.WriteLine(message);
        }

        private void second(string message)
        {
            eventWriter.WriteLine(message);
        }

        private void third(string message)
        {
            eventWriter.WriteLine(message);
        }

        private void newBidAccepted()
        {
            eventWriter.WriteLine("NewBidAccepted");
        }

        private void newRound()
        {
            eventWriter.WriteLine("NewRound");
        }
    }
}
