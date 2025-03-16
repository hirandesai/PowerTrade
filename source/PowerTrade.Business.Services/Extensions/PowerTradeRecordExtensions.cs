using PowerTrade.Business.Services.Dtos;
using PowerTrade.Services.Dto;

namespace PowerTrade.Business.Services.Extensions
{
    public static class PowerTradeRecordExtensions
    {
        public static AggregatedPowerTradeRecord[] ToAggregated(this PowerTradeRecord[] trades, DateTime tradeRequestTime, string localTimeZoneId)
        {
            TimeZoneInfo MadridTimezone = TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneId);
            var tradeStartTime = new DateTime(tradeRequestTime.Year, tradeRequestTime.Month, tradeRequestTime.Day, tradeRequestTime.Hour, 00, 00);
            return trades
                    .SelectMany(q => q.Periods)
                    .Select(s =>
                    {
                        DateTime utcDate = AdjustToUtcTime(s, MadridTimezone, tradeStartTime);
                        return new
                        {
                            UtcDate = utcDate,
                            Volume = s.Volume
                        };                        
                    })
                    .GroupBy(g => g.UtcDate)
                    .Select(s => new AggregatedPowerTradeRecord(s.Key, s.Sum(g => g.Volume)))
                    .ToArray();
        }
        private static DateTime AdjustToUtcTime(PowerPeriod tradePeriod, TimeZoneInfo madridTimezone, DateTime tradeStartTime)
        {
            var tradeTime = tradeStartTime.AddHours(tradePeriod.Period);
            var utcDate = TimeZoneInfo.ConvertTimeToUtc(tradeTime, madridTimezone);
            return utcDate;
        }
    }
}
