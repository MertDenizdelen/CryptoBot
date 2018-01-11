using CryptoBot.Shared;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PricePoller.MarketApis
{
    public abstract class IMarketApi
    {
        public readonly string Market;
        protected readonly IConfiguration _configuration;
        protected readonly MyLogger _logger;

        public IMarketApi(string market, IConfiguration configuration, MyLogger logger)
        {
            Market = market;
            _configuration = configuration;
            _logger = logger;
        }

        public abstract Task<PriceModel> RetrievePriceFromApi();

        protected async Task<JObject> GetResponse(string url)
        {
            url = url
                .Replace("{from}", _configuration["FROM_CURRENCY"])
                .Replace("{to}", _configuration["TO_CURRENCY"]);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "CustomAgent");

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return JObject.Parse(await response.Content.ReadAsStringAsync());
            }
            else
            {
                var message = $"Het ophalen van de prijs geeft een ongeldige statuscode({response.StatusCode}). Message: { await response.Content.ReadAsStringAsync()}";
                _logger.LogWarning(message);
                throw new Exception(message);
            }
        }
    }
}
