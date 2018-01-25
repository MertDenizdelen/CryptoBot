using System;
using Microsoft.AspNetCore.Mvc;
using CryptoBot.Shared;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Shared;
using ArbitrageDetector.Repositories;

namespace ArbitrageDetector.Controllers
{
    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        private readonly ILogger<PriceController> _logger;
        private readonly ArbitrageConfiguration _config;
        private readonly IInMemoryPriceRepository _priceRepository;

        public PriceController(ILogger<PriceController> logger, ArbitrageConfiguration config, IInMemoryPriceRepository priceRepository)
        {
            _logger = logger;
            _config = config;
            _priceRepository = priceRepository;
        }

        [HttpPost]
        public void NewPrice([FromBody] PriceModel price)
        {
            _priceRepository.Add(price);
            _logger.LogInformation(price.ToString());
        }

        [HttpGet]
        [Route("/arbitrages")]
        public List<ArbitrageModel> GetArbitrages()
        {
            var arbitrages = new List<ArbitrageModel>();
            var prices = _priceRepository.GetCopy();

            foreach (var buyModel in prices)
            {
                foreach (var sellModel in prices)
                {
                    // Different market but same currency pair and the price datetime difference is less than X. 
                    if (buyModel.Market != sellModel.Market
                        && buyModel.From == sellModel.From
                        && buyModel.To == sellModel.To
                        && Math.Abs((buyModel.DateTime - sellModel.DateTime).TotalSeconds) < _config.AllowedPriceTimeDifferenceInSeconds)
                    {
                        // Calculate the difference when buying on the buyModel and selling on the sellModel. 
                        var amount = 10;
                        var buyFee = 1.99m; // TODO: calculate fee
                        var cost = amount * buyModel.BuyPrice + buyFee;
                        var sellFee = 1.99m; // TODO: calculate fee
                        var yield = amount * sellModel.SellPrice - sellFee;

                        var profitPercentage = (yield - cost) / yield * 100;

                        arbitrages.Add(new ArbitrageModel
                        {
                            BuyModel = buyModel,
                            SellModel = sellModel,
                            Percentage = profitPercentage,
                            DateTime = DateTime.Now
                        });
                    }
                }
            }

            return arbitrages;
        }
    }
}
