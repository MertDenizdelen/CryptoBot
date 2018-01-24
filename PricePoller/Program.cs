using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PricePoller.MarketApis;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace PricePoller
{
    class Program
    {
        private static MyLogger _logger;
        private static IConfiguration _configuration;
        private static ManualResetEvent _Shutdown;

        static Program()
        {
            _Shutdown = new ManualResetEvent(false);
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
            _logger = new MyLogger(_configuration);
        }

        public static void Main(string[] args)
        {
            _logger.LogInformation("Start met het ophalen van nieuwe prijzen...");
            // Register for shutdown event
            AssemblyLoadContext.Default.Unloading += OnShutdown;

            // Initialise
            var marketApiService = new MarketApiService(_configuration, _logger);
            var pushService = new PushService(_configuration, _logger);

            // Act
            var refreshIntervalInSeconds = int.Parse(_configuration["REFRESH_INTERVAL_IN_SECONDS"]);
            var timer = new System.Timers.Timer(refreshIntervalInSeconds * 1000);
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
            _Shutdown.WaitOne();
            timer.Stop();
            _logger.LogInformation("PricePoller is gestopt.");
        }

        private static void OnShutdown(AssemblyLoadContext obj)
        {
            _Shutdown.Set();
        }
    }
}
