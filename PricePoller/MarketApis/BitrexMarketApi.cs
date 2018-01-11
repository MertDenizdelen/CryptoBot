using CryptoBot.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PricePoller.MarketApis
{
    public class BitrexMarketApi : IMarketApi
    {
        public BitrexMarketApi(IConfiguration configuration, MyLogger logger) : base(Markets.BITREX, configuration, logger)
        {
        }

        public override async Task<PriceModel> RetrievePriceFromApi()
        {
            var from = _configuration["FROM_CURRENCY"];
            var to = _configuration["TO_CURRENCY"];
            var url = $"https://bittrex.com/api/v1.1/public/getticker?market={from}-{to}";
            var json = await GetResponse(url);

            return new PriceModel
            {
                Market = _configuration["MARKET"],
                From = from,
                To = to,
                BuyPrice = json.SelectToken("result").Value<decimal>("Ask"),
                SellPrice = json.SelectToken("result").Value<decimal>("Bid")
            };
        }
    }
}
