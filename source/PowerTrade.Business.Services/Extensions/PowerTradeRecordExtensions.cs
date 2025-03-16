using PowerTrade.Business.Services.Dtos;
using PowerTrade.Services.Dto;

namespace PowerTrade.Business.Services.Extensions
{
    public static class PowerTradeRecordExtensions
    {
        public static AggregatedPowerTradeRecord[] ToAggregated(this PowerTradeRecord[] trades, DateTime tradeRequestTime)
        {
            var tradeStartTime = new DateTime(tradeRequestTime.Year, tradeRequestTime.Month, tradeRequestTime.Day, tradeRequestTime.Hour, 00, 00, DateTimeKind.Utc);
            return trades
                    .SelectMany(q => q.Periods)
                    .GroupBy(g => g.Period)
                    .Select(s => new AggregatedPowerTradeRecord(tradeStartTime.AddHours(s.Key), s.Sum(g => g.Volume)))
                    .ToArray();
        }
    }
}
