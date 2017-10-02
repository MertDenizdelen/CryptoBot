using System;

namespace API.Markets
{
    public class PoliniexPriceAPI : IPriceAPI
    {
        public decimal GetBuyPrice(CryptoKind from, CryptoKind to)
        {
            // TODO: Vervangen met implementatie die de Poliniex API aanroept.
            var random = new Random();
            var n = random.Next(700, 800);
            return Decimal.Parse($"0.0{n}"); // decimal between 0.07 and 0.00
        }

        public decimal GetSellPrice(CryptoKind from, CryptoKind to)
        {
            // TODO: Vervangen met implementatie die de Poliniex API aanroept.
            var random = new Random();
            var n = random.Next(700, 800);
            return Decimal.Parse($"0.0{n}"); // decimal between 0.07 and 0.00
        }
    }
}
