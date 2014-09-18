using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Delegates;
using Model;

namespace UnitTests
{
    [TestClass]
    public class AuctioneerTest
    {
        [TestMethod]
        public void TestTimersNotSold()
        {
            List<string> receivedEvents = new List<string>();

            AuctionItem item = new AuctionItem(1, "Chair", 100, 1000);
            Auction auction = new Auction();
            auction.AddItem(item);

            Auctioneer auctioneer = new Auctioneer(auction, 500, 500, 500);

            auctioneer.CallFirst += delegate(string message)
            {
                receivedEvents.Add(message);
            };

            auctioneer.CallSecond += delegate(string message)
            {
                receivedEvents.Add(message);
            };

            auctioneer.CallThird += delegate(string message)
            {
                receivedEvents.Add(message);
            };

            auction.Start(auctioneer);

            Thread.Sleep(2000);

            Assert.AreEqual("First!", receivedEvents[0]);
            Assert.AreEqual("Second!", receivedEvents[1]);
            bool containsThird = receivedEvents[2].Contains("Third!");
            Assert.IsTrue(containsThird);
        }

        [TestMethod]
        public void TestTimersSold()
        {
            List<string> receivedEvents = new List<string>();

            AuctionItem item = new AuctionItem(1, "Chair", 100, 1000);
            Auction auction = new Auction();
            auction.AddItem(item);

            Auctioneer auctioneer = new Auctioneer(auction, 500, 500, 500);

            auctioneer.CallFirst += delegate(string message)
            {
                receivedEvents.Add(message);
            };

            auctioneer.CallSecond += delegate(string message)
            {
                receivedEvents.Add(message);
            };

            auctioneer.CallThird += delegate(string message)
            {
                receivedEvents.Add(message);
            };

            auction.Start(auctioneer);

            auction.PlaceBid(1, 1100, "bla");

            Thread.Sleep(2000);

            Assert.AreEqual("First!", receivedEvents[0]);
            Assert.AreEqual("Second!", receivedEvents[1]);
            bool containsThird = receivedEvents[2].Contains("Third!");
            Assert.IsTrue(containsThird);
        }
    }
}
