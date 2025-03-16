namespace PowerTrade.Business.Services.Dtos
{
    public record AggregatedPowerTradeRecord
    {
        public DateTime TradeTime { get; private set; }
        public double Volume { get; private set; }

        public AggregatedPowerTradeRecord(DateTime tradeTime, double volume)
        {
            TradeTime = tradeTime;
            Volume = volume;
        }
    }
}
