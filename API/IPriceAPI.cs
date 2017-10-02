namespace API
{
    public interface IPriceAPI
    {
        decimal GetBuyPrice(CryptoKind from, CryptoKind to);

        decimal GetSellPrice(CryptoKind from, CryptoKind to);
    }
}
