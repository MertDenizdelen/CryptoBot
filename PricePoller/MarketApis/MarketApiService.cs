using CryptoBot.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PricePoller.MarketApis
{
    public class MarketApiService
    {
        private readonly List<IMarketApi> _marketApis;
        private readonly IConfiguration _configuration;
        private readonly MyLogger _logger;

        public MarketApiService(IConfiguration configuration, MyLogger logger)
        {
            _marketApis = new List<IMarketApi>
            {
                new CoinbaseMarketApi(configuration, logger),
                new GDAXMarketApi(configuration, logger),
                new BittrexMarketApi(configuration, logger),
                new KrakenMarketApi(configuration, logger),
            };

            _configuration = configuration;
            _logger = logger;
        }

        public async Task<PriceModel> GetPrice()
        {
            try
            {
                var market = _marketApis.First(m => m.Market == _configuration["Market"]);
                return await market.RetrievePriceFromApi();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"De opgegeven market wordt niet ondersteund: {_configuration["Market"]}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Er is iets misgegaan bij het ophalen van een prijs.", ex);
            }

            return null;
        }
    }
}
