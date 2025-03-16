using CsvHelper.Configuration.Attributes;

namespace PowerTrade.Business.Services.Dtos
{
    public record AggregatedPowerTradeRecord
    {
        [Name("Datetime")]
        [Format("yyyy-MM-ddTHH:mm:ss.fffZ")]
        public DateTime TradeTime { get; private set; }

        [Name("Volume")]
        public double Volume { get; private set; }

        public AggregatedPowerTradeRecord(DateTime tradeTime, double volume)
        {
            TradeTime = tradeTime;
            Volume = volume;
        }
    }
}
