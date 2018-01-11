using CryptoBot.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PricePoller.MarketApis
{
    public class CoinbaseMarketApi : IMarketApi
    {
        public CoinbaseMarketApi(IConfiguration configuration, MyLogger logger) : base(Markets.COINBASE, configuration, logger)
        {
        }

        public override async Task<PriceModel> RetrievePriceFromApi()
        {
            var fromCurrency = _configuration["FROM_CURRENCY"];
            var toCurrency = _configuration["TO_CURRENCY"];
            var buyUrl = $"https://api.coinbase.com/v2/prices/{fromCurrency}-{toCurrency}/buy";
            var buyPrice = (await GetResponse(buyUrl)).SelectToken("data").Value<decimal>("amount");
            var sellUrl = $"https://api.coinbase.com/v2/prices/{fromCurrency}-{toCurrency}/sell";
            var sellPrice = (await GetResponse(sellUrl)).SelectToken("data").Value<decimal>("amount");

            return new PriceModel
            {
                Market = _configuration["MARKET"],
                From = fromCurrency,
                To = toCurrency,
                BuyPrice = buyPrice,
                SellPrice = sellPrice
            };
        }
    }
}
