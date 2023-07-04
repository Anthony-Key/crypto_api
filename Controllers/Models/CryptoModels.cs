namespace CryptoPriceTrader.Controllers.Types
{
    public class Candle
    {
        public int InstrumentID { get; set; }
        public DateTime FromDate { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public object Volume { get; set; }
    }

    public class Instrument
    {
        public int InstrumentId { get; set; }
        public List<Candle> Candles { get; set; }
        public double RangeOpen { get; set; }
        public double RangeClose { get; set; }
        public double RangeHigh { get; set; }
        public double RangeLow { get; set; }
        public double Volume { get; set; }
    }

    public class Root
    {
        public string Interval { get; set; }
        public List<Instrument> Candles { get; set; }
    }

    public class CryptoDAO
    {
        public string Name { get; set; }
        public string Category { get; set; } = "Cryptocurrency";
        public string Price { get; set; }
        public string Description { get; set; }
        public string TokenImage { get; set; }
    }
}