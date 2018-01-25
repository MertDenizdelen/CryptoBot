using CryptoBot.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArbitrageDetector.Repositories
{
    public interface IInMemoryPriceRepository
    {
        void Add(PriceModel priceModel);

        ICollection<PriceModel> Get();

        PriceModel[] GetCopy();
    }

    public class InMemoryPriceRepository : IInMemoryPriceRepository
    {
        private readonly IDictionary<string, PriceModel> _prices;

        public InMemoryPriceRepository()
        {
            _prices = new Dictionary<string, PriceModel>();
        }

        public void Add(PriceModel priceModel)
        {
            _prices[priceModel.Key] = priceModel;
        }

        public ICollection<PriceModel> Get()
        {
            return _prices.Values;
        }

        public PriceModel[] GetCopy()
        {
            var copiedPrices = new PriceModel[_prices.Count];
            _prices.Values.CopyTo(copiedPrices, 0);
            return copiedPrices;
        }
    }
}
