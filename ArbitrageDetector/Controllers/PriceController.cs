using System;
using Microsoft.AspNetCore.Mvc;
using CryptoBot.Shared;
using Microsoft.Extensions.Logging;

namespace ArbitrageDetector.Controllers
{
    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        private readonly ILogger<PriceController> _logger;

        public PriceController(ILogger<PriceController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void NewPrice([FromBody] PriceModel price)
        {
            _logger.LogInformation(price.ToString());
        }
    }
}
