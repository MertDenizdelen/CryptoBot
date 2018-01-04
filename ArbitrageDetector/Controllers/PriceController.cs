using System;
using Microsoft.AspNetCore.Mvc;
using ArbitrageDetector.Models;

namespace ArbitrageDetector.Controllers
{
    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        [HttpPost]
        public void NewPrice([FromBody] Price price)
        {
            Console.WriteLine(price);
        }
    }
}
