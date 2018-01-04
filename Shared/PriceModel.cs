namespace CryptoBot.Shared
{
    public class PriceModel
    {
        public string Market { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }

        public override string ToString()
        {
            return $"Markt: {Market} / {From}-{To} / Buy: {BuyPrice} & Sell: {SellPrice}";
        }
    }
}