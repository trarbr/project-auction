using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Model;

namespace UnitTests
{
    [TestClass]
    public class AuctionItemTest
    {
        [TestMethod]
        public void TestPlaceBidHigher()
        {
            AuctionItem item = getAuctionItem();
            decimal newBid = 120;

            bool success = item.PlaceBid(newBid);

            decimal expectedBid = newBid;
            decimal actualBid = item.Bid;

            Assert.IsTrue(success);
            Assert.AreEqual(expectedBid, actualBid);
        }

        [TestMethod]
        public void TestPlaceBidLower()
        {
            AuctionItem item = getAuctionItem();
            decimal newBid = 80;

            bool success = item.PlaceBid(newBid);

            decimal expectedBid = 100;
            decimal actualBid = item.Bid;

            Assert.IsFalse(success);
            Assert.AreEqual(expectedBid, actualBid);
        }

        [TestMethod]
        public void TestEvaluateIfSoldTrue()
        {
            AuctionItem item = getAuctionItem();

            item.PlaceBid(1100);

            bool isSold = item.EvaluateIfSold();

            Assert.IsTrue(isSold);
        }

        [TestMethod]
        public void TestEvaluateIfSoldFalse()
        {
            AuctionItem item = getAuctionItem();

            item.PlaceBid(100);

            bool isSold = item.EvaluateIfSold();

            Assert.IsFalse(isSold);
        }

        [TestMethod]
        public void TestPlaceBidWhenSold()
        {
            AuctionItem item = getAuctionItem();

            item.PlaceBid(1100);

            item.EvaluateIfSold();

            bool success = item.PlaceBid(1200);

            Assert.IsFalse(success);
        }


        private AuctionItem getAuctionItem()
        {
            return new AuctionItem("Chair", 100, 1000);
        }


    }
}
