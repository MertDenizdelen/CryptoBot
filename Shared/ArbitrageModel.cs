using CryptoBot.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class ArbitrageModel
    {
        public PriceModel BuyModel { get; set; }
        public PriceModel SellModel { get; set; }
        public decimal Percentage { get; set; }
        public DateTime DateTime { get; set; }
    }
}
