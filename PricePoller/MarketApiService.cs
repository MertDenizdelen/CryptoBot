using CryptoBot.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PricePoller
{
    public class MarketApiService
    {
        private readonly IConfiguration _configuration;
        private readonly MyLogger _logger;

        public MarketApiService(IConfiguration configuration, MyLogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<PriceModel> GetPrice()
        {
            try
            {
                var apiUrl = _configuration["API_URL"];
                var response = await new HttpClient().GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                    return new PriceModel
                    {
                        Market = _configuration["MARKET"],
                        From = _configuration["FROM_CURRENCY"],
                        To = _configuration["TO_CURRENCY"],
                        BuyPrice = json.SelectToken("result").Value<decimal>("Ask"),
                        SellPrice = json.SelectToken("result").Value<decimal>("Bid")
                    };
                }
                else
                {
                    _logger.LogWarning($"Het ophalen van de prijs geeft een ongeldige statuscode ({response.StatusCode}). Message: {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Er kan geen verbinding worden gemaakt met de market.", ex);
                return null;
            }
        }
    }
}
