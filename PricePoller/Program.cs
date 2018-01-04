using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PricePoller
{
    class Program
    {
        private static MyLogger _logger;
        private static IConfiguration _configuration;

        static Program()
        {
            _logger = new MyLogger();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public static void Main(string[] args)
        {
            _logger.LogInformation("Start met het ophalen van nieuwe prijzen...");

            // Initialise
            var marketApiService = new MarketApiService(_configuration, _logger);
            var pushService = new PushService(_configuration, _logger);

            // Act
            var refreshIntervalInSeconds = int.Parse(_configuration["REFRESH_INTERVAL_IN_SECONDS"]);
            var timer = new Timer(refreshIntervalInSeconds * 1000);
            timer.Elapsed += new ElapsedEventHandler(async (sender, e) =>
            {
                // Retrieve and push price every n seconds
                var price = await marketApiService.GetPrice();
                if (price != null)
                {
                    await pushService.PushPrice(price);
                }
            });
            timer.Start();

            // Dispose
            Console.ReadKey(true);
            timer.Stop();
            _logger.LogInformation("PricePoller is gestopt.");
        }
    }
}
