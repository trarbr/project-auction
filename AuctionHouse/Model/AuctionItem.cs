using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AuctionItem
    {
        // Added by Troels
        public int Id { get; set; }

        private string _itemName;
        private decimal _bid;
        private bool _sold;
        private decimal _minimumSoldPrice;

        public decimal MinimunSoldPrice
        {
            get { return _minimumSoldPrice;}
            set { _minimumSoldPrice = value;}
        }

        public bool Sold
        {
            get { return _sold; }
            set { _sold = value; }
        }
        
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        public decimal Bid
        {
            get { return _bid; }
            set { _bid = value; }
        }
        
        public AuctionItem(string itemName, int bid, decimal minimumSoldPrice)
        {
            ItemName = itemName;
            Bid = bid;
            MinimunSoldPrice = minimumSoldPrice;
        }

        public bool PlaceBid(decimal newBid)
        {
            if (newBid > Bid && Sold == false)
            {
                Bid = newBid;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsSold()
        {
            if (Bid > MinimunSoldPrice)
            {
                Sold = true;
            }
            else
            {
                Sold = false;
            }
            return Sold;
        }
    }
}
