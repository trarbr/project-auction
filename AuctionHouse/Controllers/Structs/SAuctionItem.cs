using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Structs
{
    public struct SAuctionItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal Bid { get; set; }
    }
}
