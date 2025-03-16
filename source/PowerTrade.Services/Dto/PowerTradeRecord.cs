namespace PowerTrade.Services.Dto
{
    public record PowerTradeRecord
    {
        public string TradeId { get; }

        public DateTime Date { get; }

        public PowerPeriod[] Periods { get; }

        private PowerTradeRecord(string tradeId, DateTime date, PowerPeriod[] periods)
        {
            TradeId = tradeId;
            Date = date;
            Periods = periods;
        }

        public static explicit operator PowerTradeRecord(Axpo.PowerTrade trade) => new PowerTradeRecord(trade.TradeId,
                                                                                            trade.Date,
                                                                                            trade.Periods.Select(s => (PowerPeriod)s).ToArray());

    }
}
