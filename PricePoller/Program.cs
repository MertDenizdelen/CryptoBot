using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PricePoller
{
    class Program
    {
        private static MyLogger _logger;

        static Program()
        {
            _logger = new MyLogger();
        }

        public static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        public static async Task MainAsync()
        {
            _logger.LogInformation("Start met het ophalen van nieuwe prijzen...");

            // get price
            var r = new Random();
            var json = JsonConvert.SerializeObject(new
            {
                market = "CoinBase",
                rate = r.NextDouble()
            });

            // sent request
            try
            {
                var url = "http://localhost:5000/api/price";
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                await new HttpClient().PostAsync(url, body);
            }
            catch (Exception ex)
            {
                _logger.LogError("De gevonden prijs kan niet worden verstuurd naar de ArbitrageDetector.", ex);
            }

            _logger.LogInformation("PricePoller is gestopt (Druk op een toets...)");
            Console.ReadKey(true);
        }
    }
}
