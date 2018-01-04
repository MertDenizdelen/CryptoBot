namespace ArbitrageDetector.Models
{
    public class Price
    {
        public string Market { get; set; }
        public decimal Rate { get; set; }

        public override string ToString()
        {
            return $"Markt: {Market} Prijs: {Rate}";
        }
    }
}