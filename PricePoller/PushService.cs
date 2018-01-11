using CryptoBot.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PricePoller
{
    public class PushService
    {
        private readonly IConfiguration _configuration;
        private readonly MyLogger _logger;

        public PushService(IConfiguration configuration, MyLogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task PushPrice(PriceModel price)
        {
            var url = _configuration["PUSH_URL"];

            try
            {
                var body = new StringContent(JsonConvert.SerializeObject(price), Encoding.UTF8, "application/json");
                await new HttpClient().PostAsync(url, body);
                _logger.LogInformation($"Prijs {price.BuyPrice} / {price.SellPrice} verzonden naar de ArbitrageDetector.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"De gevonden prijs kan niet worden verstuurd naar de ArbitrageDetector ({url}).", ex);
            }
        }
    }
}
