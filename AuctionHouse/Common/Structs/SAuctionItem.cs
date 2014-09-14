using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Structs
{
    public struct SAuctionItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal MaxBid { get; set; }
    }
}
