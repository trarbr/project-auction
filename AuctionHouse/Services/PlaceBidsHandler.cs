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

        private StreamReader eventReader;
        private StreamWriter eventWriter;
        private NetworkStream eventStream;

        private object lockObj;
        // constructor should also take a Socket, which it is passed from the server
        public PlaceBidsHandler(IPlaceBidsController placeBidsController, Socket commandSocket, object lockObj)
        {
            this.placeBidsController = placeBidsController;
            this.commandSocket = commandSocket;

            this.lockObj = lockObj;
            // subscribe to events on controller
            // when an event is fired on the controller, it has to send a new message to the
            // client.

            placeBidsController.NewRound += NewRound;
            placeBidsController.NewBidAccepted += NewBidAccepted;
            placeBidsController.CallFirst += First;
            placeBidsController.CallSecond += Second;
            placeBidsController.CallThird += Third;
        }

        // needs a while-true method that reads from the StreamReader
        // then it needs an if-else or switch to evaluate which command to pass the string it just
        // read to (e.g. if strings first word is "bid" it should call placeBid...
        // this while-true method will be used by the server after instantiating the handler: 
        // the while-true method is passed to a new thread and started.

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

        private void createEventSocket()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 16001);
            listener.Start();
            
            eventSocket = listener.AcceptSocket();

            eventStream = new NetworkStream(eventSocket);
            eventReader = new StreamReader(eventStream);
            eventWriter = new StreamWriter(eventStream);
            eventWriter.AutoFlush = true;

            listener.Stop();
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

            eventReader.Close();
            eventWriter.Close();
            eventStream.Close();
            eventSocket.Close();
        }

        public void First(string message)
        {
            eventWriter.WriteLine(message);
        }

        private void Third(string message)
        {
            eventWriter.WriteLine(message);
        }

        private void Second(string message)
        {
            eventWriter.WriteLine(message);
        }

        private void NewBidAccepted()
        {
            eventWriter.WriteLine("NewBidAccepted");
        }

        private void NewRound()
        {
            eventWriter.WriteLine("NewRound");
        }
    }
}
