﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        private Socket socket;

        private StreamReader reader;
        private StreamWriter writer;

        // constructor should also take a Socket, which it is passed from the server
        public BidHandler(IAuctionController auctionController, Socket socket)
        {
            this.auctionController = auctionController;
            this.socket = socket;



            //Socket eventSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //EndPoint eventPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 16001);
            //eventSocket.Connect(eventPoint);


            //Console.WriteLine(eventSocket.LocalEndPoint);

           
            
        
            // subscribe to events on controller
            // when an event is fired on the controller, it has to send a new message to the
            // client.

            auctionController.NewRound += NewRound;
            auctionController.NewBidAccepted += NewBidAccepted;
            auctionController.CallFirst += First;
            auctionController.CallSecond += Second;
            auctionController.CallThird += Third;
            TcpClient eventClient = new TcpClient("127.0.0.1", 16001);
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

        internal void Run()
        {
            NetworkStream stream = new NetworkStream(socket);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            Console.WriteLine("Server started.");
            Console.WriteLine("IP: " + socket.RemoteEndPoint + " is connected");
            writer.WriteLine("you are connected!");

            bool BoolRun = true;

            string textFromClient;
            string[] textFromClientArray;

            while (BoolRun)
            {
                textFromClient = reader.ReadLine();
                textFromClientArray = textFromClient.Split('|');

                Console.WriteLine(socket.RemoteEndPoint + ": " + textFromClient);

                if (textFromClient == "hej")
                {
                    writer.WriteLine("YEEEEEEEAAAAH!!!!");
                }
                if (textFromClientArray[0] == "exit" || textFromClientArray[0] == "close")
                {
                    writer.WriteLine("Serveren lukkes!");
                    BoolRun = false;
                }
                if (textFromClientArray[0] == "bid")
                {
                    bool success = placeBid(textFromClientArray[1], textFromClientArray[2]);

                    writer.WriteLine(success);
                }
                if (textFromClientArray[0] == "get")
                {
                    string item = getCurrentItem();

                    writer.WriteLine(item);
                }
                else
                {
                    writer.WriteLine("Skriv nu for helvede korrekt!... ALTSÅ!..TSK");
                }
            }

            writer.Close();
            reader.Close();
            stream.Close();
            socket.Close();
        }

        public void First(string message)
        {
            writer.WriteLine(message);
        }

        private void Third(string message)
        {
            writer.WriteLine(message);
        }

        private void Second(string message)
        {
            writer.WriteLine(message);
        }

        private void NewBidAccepted()
        {
            writer.WriteLine("NewBidAccepted");
        }

        private void NewRound()
        {
            writer.WriteLine("NewRound");
        }
    }
}
