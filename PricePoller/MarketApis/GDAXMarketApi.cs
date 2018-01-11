using CryptoBot.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PricePoller.MarketApis
{
    public class GDAXMarketApi : IMarketApi
    {
        public GDAXMarketApi(IConfiguration configuration, MyLogger logger) : base(Markets.GDAX, configuration, logger)
        {
        }

        public override async Task<PriceModel> RetrievePriceFromApi()
        {
            var from = _configuration["FROM_CURRENCY"];
            var to = _configuration["TO_CURRENCY"];
            var url = $"https://api.gdax.com/products/{from}-{to}/ticker";
            var json = await GetResponse(url);

            return new PriceModel
            {
                Market = _configuration["MARKET"],
                From = from,
                To = to,
                BuyPrice = json.Value<decimal>("ask"),
                SellPrice = json.Value<decimal>("bid")
            };
        }
    }
}
