﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AuctionItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal MaxBid { get; set; }
        public bool Sold { get; set; }
    }
}
