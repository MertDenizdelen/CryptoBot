using API;
using API.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class ArbitrageDetector
    {
        // Settings --> TODO in settings file
        private const CryptoKind from = CryptoKind.BTC;
        private const CryptoKind to = CryptoKind.ETH;
        private const int RefreshTimeInSeconds = 30; 

        private readonly CancellationTokenSource _cancellationToken;
        private Task _task;
        private List<IPriceAPI> _priceAPIs;

        public ArbitrageDetector()
        {
            _cancellationToken = new CancellationTokenSource();
            _priceAPIs = new List<IPriceAPI>
            {
                new BitrexPriceAPI(),
                new PoliniexPriceAPI()
            };
        }

        public void Start()
        {
            _task = new Task(DetectArbitrages, _cancellationToken.Token);
            _task.Start();
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
            _task.Wait();
        }

        private void DetectArbitrages()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                // Combineer elke beschikbare market met de andere beschikbare markets (buy-sell)
                _priceAPIs.ForEach(buyMarket =>
                {
                    _priceAPIs.ForEach(sellMarket =>
                    {
                        if (buyMarket != sellMarket)
                        {
                            // Bereken de winst
                            var profit = CalculateProfitPercentage(buyMarket, sellMarket);
                            Console.WriteLine($"{from}/{to} - Winst bij {buyMarket.GetType().Name.Replace("PriceAPI", "")} -> {sellMarket.GetType().Name.Replace("PriceAPI", "")}: {profit:0.##}%");
                            if(profit > 10)
                            {
                                Console.WriteLine("!Arbitrage detected!");
                            }
                        }
                    });
                });

                // Wacht een aantal seconden voor de volgende check
                Thread.Sleep(TimeSpan.FromSeconds(RefreshTimeInSeconds));
            }
        }

        private decimal CalculateProfitPercentage(IPriceAPI buyMarket, IPriceAPI sellMarket)
        {
            // TODO berekening implementeren zoals hier beschreven: https://steemit.com/arbitrage/@kesor/the-math-behind-cross-exchange-arbitrage-trading
            // TODO Unit tests toevoegen die deze berekening testen.

            var buyPrice = buyMarket.GetBuyPrice(from, to);
            var sellPrice = sellMarket.GetSellPrice(from, to);
            return (sellPrice - buyPrice) / sellPrice * 100;
        }
    }
}
