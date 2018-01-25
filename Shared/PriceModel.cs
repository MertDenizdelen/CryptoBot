using System;

namespace CryptoBot.Shared
{
    public class PriceModel
    {
        public string Market { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime DateTime { get; set; }

        public PriceModel()
        {
            DateTime = DateTime.Now;
        }

        public string Key
        {
            get { return Market + From + To; }
        }

        public override string ToString()
        {
            return $"{DateTime}: Markt: {Market} / {From}-{To} / Buy: {BuyPrice} & Sell: {SellPrice}";
        }
    }
}