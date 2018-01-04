using System;
using Microsoft.AspNetCore.Mvc;
using CryptoBot.Shared;

namespace ArbitrageDetector.Controllers
{
    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        [HttpPost]
        public void NewPrice([FromBody] PriceModel price)
        {
            Console.WriteLine(price);
        }
    }
}
