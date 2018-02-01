using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbitrageDetector.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace ArbitrageDetector.Controllers
{
    [Route("api/[controller]")]
    public class ArbitrageController : Controller
    {
        private readonly ArbitrageConfiguration _config;
        private readonly IInMemoryPriceRepository _priceRepository;

        public ArbitrageController(IInMemoryPriceRepository priceRepository, ArbitrageConfiguration config)
        {
            _config = config;
            _priceRepository = priceRepository;
        }

        [HttpGet]
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