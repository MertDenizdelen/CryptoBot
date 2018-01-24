using CryptoBot.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricePoller.MarketApis
{
    public class KrakenMarketApi : IMarketApi
    {
        public KrakenMarketApi(IConfiguration configuration, MyLogger logger) : base(Markets.KRAKEN, configuration, logger)
        {
        }

        public override async Task<PriceModel> RetrievePriceFromApi()
        {
            var from = _configuration["FROM_CURRENCY"];
            var to = _configuration["TO_CURRENCY"];
            var url = $"https://api.kraken.com/0/public/Ticker?pair={from}{to}";
            url = url.Replace("BTC", "XBT"); // convert BTC to XBT
            var json = await GetResponse(url);

            return new PriceModel
            {
                Market = _configuration["MARKET"],
                From = from,
                To = to,
                BuyPrice = json["result"].First.First["a"].Values<decimal>().First(),
                SellPrice = json["result"].First.First["b"].Values<decimal>().First()
            };
        }
    }
}
