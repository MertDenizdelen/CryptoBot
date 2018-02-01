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
        private readonly IInMemoryPriceRepository _priceRepository;

        public PriceController(ILogger<PriceController> logger, IInMemoryPriceRepository priceRepository)
        {
            _logger = logger;
            _priceRepository = priceRepository;
        }

        [HttpPost]
        public void NewPrice([FromBody] PriceModel price)
        {
            _priceRepository.Add(price);
            _logger.LogInformation(price.ToString());
        }
    }
}
