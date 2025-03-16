using PowerTrade.Business.Services.Dtos;
using PowerTrade.Services.Dto;

namespace PowerTrade.Business.Services.Extensions
{
    public static class PowerTradeRecordExtensions
    {
        public static AggregatedPowerTradeRecord[] ToAggregated(this PowerTradeRecord[] trades, DateTime tradeRequestTimeUtc, string localTimeZoneId)
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneId);
            var tradeStartTime = new DateTime(tradeRequestTimeUtc.Year, tradeRequestTimeUtc.Month, tradeRequestTimeUtc.Day, tradeRequestTimeUtc.Hour, 00, 00, DateTimeKind.Utc);
            return trades
                    .SelectMany(q => q.Periods)
                    .Select(s =>
                    {
                        DateTime utcDate = AdjustToLocalTime(s, tradeStartTime, localTimeZone);
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
        private static DateTime AdjustToLocalTime(PowerPeriod tradePeriod, DateTime tradeStartTime, TimeZoneInfo timeZoneInfo)
        {
            var tradeTime = tradeStartTime.AddHours(tradePeriod.Period);
            return TimeZoneInfo.ConvertTimeFromUtc(tradeTime, timeZoneInfo);
        }

        private static DateTime AdjustToUtcTime(DateTime time, TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeToUtc(time, timeZoneInfo);
        }
    }
}
