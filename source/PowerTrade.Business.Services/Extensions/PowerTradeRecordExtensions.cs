using PowerTrade.Business.Services.Dtos;
using PowerTrade.Services.Dto;

namespace PowerTrade.Business.Services.Extensions
{
    public static class PowerTradeRecordExtensions
    {
        public static AggregatedPowerTradeRecord[] ToAggregated(this PowerTradeRecord[] trades, DateTime tradeRequestTimeUtc, string localTimeZoneId)
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneId);
            var localTime = AdjustToLocalTime(tradeRequestTimeUtc, localTimeZone);
            var tradeStartLocalTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, localTime.Hour, 00, 00);
            return trades
                    .SelectMany(q => q.Periods)
                    .Select(s =>
                    {
                        DateTime localTime = AdjustToLocalTime(s, tradeStartLocalTime, localTimeZone);
                        return new
                        {
                            LocalTime = localTime,
                            Volume = s.Volume
                        };
                    })
                    .GroupBy(g => g.LocalTime)
                    .Select(s => new AggregatedPowerTradeRecord(AdjustToUtcTime(s.Key, localTimeZone), s.Sum(g => g.Volume)))
                    .ToArray();
        }
        private static DateTime AdjustToLocalTime(PowerPeriod tradePeriod, DateTime tradeStartTime, TimeZoneInfo timeZoneInfo)
        {
            var tradeTime = tradeStartTime.AddHours(tradePeriod.Period);
            if (timeZoneInfo.IsInvalidTime(tradeTime))
            {
                var adjustmentRules = timeZoneInfo.GetAdjustmentRules().FirstOrDefault(r => r.DateStart <= tradeTime && r.DateEnd >= tradeTime);
                var gapDuration = adjustmentRules?.DaylightDelta ?? TimeSpan.FromHours(1);
                return tradeTime.Add(gapDuration);
            }
            else if (timeZoneInfo.IsAmbiguousTime(tradeTime))
            {
                var offsets = timeZoneInfo.GetAmbiguousTimeOffsets(tradeTime);
                var offsetToUse = offsets.Min();
                return tradeTime - offsetToUse;
            }
            return tradeTime;
        }

        private static DateTime AdjustToUtcTime(DateTime time, TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeToUtc(time, timeZoneInfo);
        }

        private static DateTime AdjustToLocalTime(DateTime time, TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, timeZoneInfo);
        }
    }
}
