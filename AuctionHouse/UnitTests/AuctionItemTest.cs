using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Model;

namespace UnitTests
{
    [TestClass]
    public class AuctionItemTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            AuctionItem item = getAuctionItem();

            Assert.AreEqual(1, item.Id);
            Assert.AreEqual("Chair", item.ItemName);
            Assert.AreEqual(100, item.Bid);
        }

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
            item.PlaceBid(200);

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
            return new AuctionItem(1, "Chair", 100, 1000);
        }
    }
}
